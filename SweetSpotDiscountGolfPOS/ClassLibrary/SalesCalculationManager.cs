using SweetShop;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The calculation manager class is a hub where calculations are stored to clean up the codebehind on the webpages
    public class SalesCalculationManager
    {
        //This method returns the total discounts applied in the cart as a total **Checked and Verified
        private double returnDiscount(List<InvoiceItems> itemsSold)
        {
            double singleDiscoount = 0;
            double totalDiscount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in itemsSold)
            {
                //Determines if the discount was a percentage or a number
                if (cart.percentage)
                {
                    //If the discount is a percentage
                    singleDiscoount = cart.quantity * (cart.price * (cart.itemDiscount / 100));
                }
                else
                {
                    //If the discount is a dollar amount
                    singleDiscoount = cart.quantity * cart.itemDiscount;
                }
                totalDiscount += singleDiscoount;
            }
            //Returns the total discount as a double going to two decimal places
            return Math.Round(totalDiscount, 2);
        }
        //This method returns the total trade in amount for the cart
        private double returnTradeInAmount(List<InvoiceItems> itemsSold, int loc, object[] objPageDetails)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;
            //Checks the range of trade in sku's by location
            int[] range = tradeInSkuRange(loc, objPageDetails);
            //Loops through the cart and pulls each item
            foreach (var cart in itemsSold)
            {
                //Checking the sku and seeing if it falls in the trade in range.
                //If it does, the item is a trade in
                if (cart.sku <= range[1] && cart.sku >= range[0])
                {
                    //Adding the trade in value to the total trade in amount
                    singleTradeInAmount = cart.quantity * cart.price;
                    totalTradeinAmount += singleTradeInAmount;
                }
            }
            //Returns the total trade in amount for the cart
            return totalTradeinAmount;
        }
        //This method returns the total total amount of the cart
        private double returnTotalAmount(List<InvoiceItems> itemsSold, int loc, object[] objPageDetails)
        {
            //Checks the range of trade in sku's by location
            int[] range = tradeInSkuRange(loc, objPageDetails);
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in itemsSold)
            {
                //Checks if the sku is outside of the range for the trade in sku's
                if (cart.sku >= range[1] || cart.sku <= range[0])
                {
                    singleTotalAmount = cart.quantity * cart.price;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            //Returns the total amount value of the cart
            return totalTotalAmount;
        }
        //This method returns the total subtotal amount for the cart
        private double returnSubtotalAmount(List<InvoiceItems> itemsSold, int loc, object[] objPageDetails)
        {
            double totalSubtotalAmount = 0;
            //Gets the total discount value of the cart
            double totalDiscountAmount = returnDiscount(itemsSold);
            //Gets the total trade in value of the cart
            double totalTradeInAmount = returnTradeInAmount(itemsSold, loc, objPageDetails);
            //Gets the total total value of the cart
            double totalTotalAmount = returnTotalAmount(itemsSold, loc, objPageDetails);
            //Calculations using the above three methods to determine the total subtotal value
            totalSubtotalAmount = totalSubtotalAmount + totalTotalAmount;
            totalSubtotalAmount = totalSubtotalAmount - totalDiscountAmount;
            totalSubtotalAmount = totalSubtotalAmount - (totalTradeInAmount * (-1));
            //Returns the subtotal value of the cart
            return totalSubtotalAmount;
        }
        private double returnReceiptSubtotalAmount(List<InvoiceItems> itemsSold, int loc)
        {
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in itemsSold)
            {
                singleTotalAmount = cart.quantity * cart.cost;
                totalTotalAmount += singleTotalAmount;
            }
            //Returns the total amount value of the cart
            return totalTotalAmount * -1;
        }

        //This method returns the total refund subtotal amount **Checked and Verified
        private double returnRefundTotalAmount(List<InvoiceItems> itemsSold)
        {
            double singleRefundSubtotalAmount = 0;
            double totalRefundSubtotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in itemsSold)
            {
                singleRefundSubtotalAmount = cart.quantity * cart.itemRefund;
                totalRefundSubtotalAmount += singleRefundSubtotalAmount;
            }
            //Returns the total refund subtotal amount
            return totalRefundSubtotalAmount;
        }
        private double returnPurchaseAmount(List<InvoiceItems> itemsSold)
        {
            double singlePurchaseAmount = 0;
            double totalPurchaseAmount = 0;
            foreach (var cart in itemsSold)
            {
                singlePurchaseAmount = cart.quantity * cart.cost;
                totalPurchaseAmount += singlePurchaseAmount;
            }
            //Returns the total amount of the cart
            return totalPurchaseAmount * -1;
        }
        public double returnRefundSubtotalAmount(string invoice, object[] objPageDetails)
        {
            string strQueryName = "returnRefundSubtotalAmount";
            string sqlCmd = "SELECT (CASE WHEN SUM(itemRefund * quantity) IS NULL OR "
                + "SUM(itemRefund * quantity) = '' THEN 0 ELSE SUM(itemRefund * quantity) END) "
                + "AS totalRefund FROM tbl_currentSalesItems WHERE invoiceNum = @invoiceNum "
                + "AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };
            //Returns the total refund subtotal of the cart
            DatabaseCalls dbc = new DatabaseCalls();
            return dbc.MakeDataBaseCallToReturnDouble(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnDouble(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //Finds and returns an array containing the upper and lower range for the trade in skus
        private int[] tradeInSkuRange(int location, object[] objPageDetails)
        {
            string strQueryName = "tradeInSkuRange";
            int[] range = new int[2];
            string sqlCmd = "Select skuStartAt, skuStopAt from tbl_tradeInSkusForCart where locationID = @locationID";

            object[][] parms =
            {
                new object[] { "@locationID", location }
            };
            DatabaseCalls dbc = new DatabaseCalls();
            DataTable dt = dbc.returnDataTableData(sqlCmd, parms);
            //DataTable dt = dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
            //Setting the values in the array
            range[0] = dt.Rows[0].Field<int>("skuStartAt");
            range[1] = dt.Rows[0].Field<int>("skuStopAt");

            //Returns the range
            return range;
        }
        public Invoice SaveAllInvoiceTotals(Invoice I, object[] objPageDetails)
        {
            I.subTotal = returnSubtotalAmount(I.soldItems, I.location.locationID, objPageDetails);
            I.discountAmount = returnDiscount(I.soldItems);
            I.tradeinAmount = returnTradeInAmount(I.soldItems, I.location.locationID, objPageDetails);
            I.balanceDue = I.subTotal;
            return I;
        }

        public Invoice SaveAllInvoiceTotalsForReturn(Invoice I)
        {
            I.subTotal = returnSubtotalReturnAmount(I);
            I.tradeinAmount = returnTradeInReturnAmount(I);
            I.balanceDue = I.subTotal;
            return I;
        }
        public Invoice SaveAllReceiptTotals(Invoice I)
        {
            I.subTotal = returnReceiptSubtotalAmount(I.soldItems, I.location.locationID);
            I.balanceDue = I.subTotal;
            return I;
        }
        private double returnSubtotalReturnAmount(Invoice I)
        {
            double totalSubtotalAmount = 0;
            double totalTotalAmount = returnTotalAmountForReturn(I);
            double totalTradeInAmount = returnTradeInReturnAmount(I);
            totalSubtotalAmount = totalSubtotalAmount + totalTotalAmount;
            totalSubtotalAmount = totalSubtotalAmount - (totalTradeInAmount * (-1));
            return totalSubtotalAmount;
        }
        private double returnTotalAmountForReturn(Invoice I)
        {
            //Checks the range of trade in sku's by location
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in I.soldItems)
            {
                if (!cart.isTradeIn)
                {
                    //Checks if the sku is outside of the range for the trade in sku's
                    singleTotalAmount = cart.quantity * cart.itemRefund;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            //Returns the total amount value of the cart
            return totalTotalAmount;
        }
        private double returnTradeInReturnAmount(Invoice I)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in I.soldItems)
            {
                //Checking the sku and seeing if it falls in the trade in range.
                //If it does, the item is a trade in
                if (cart.isTradeIn)
                {
                    //Adding the trade in value to the total trade in amount
                    singleTradeInAmount = cart.quantity * cart.itemRefund;
                    totalTradeinAmount += singleTradeInAmount;
                }
            }
            //Returns the total trade in amount for the cart
            return totalTradeinAmount;
        }

    }
}