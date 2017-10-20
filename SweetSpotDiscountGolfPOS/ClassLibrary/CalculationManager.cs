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
    public class CalculationManager
    {
        ItemDataUtilities idu = new ItemDataUtilities();

        //This method returns the total discounts applied in the cart as a total
        public double returnDiscount(List<Cart> itemsSold)
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
                    singleDiscoount = cart.quantity * (cart.price * (cart.discount / 100));
                }
                else
                {
                    //If the discount is a dollar amount
                    singleDiscoount = cart.quantity * cart.discount;
                }
                totalDiscount += singleDiscoount;
            }
            //Returns the total discount as a double going to two decimal places
            return Math.Round(totalDiscount, 2);
        }

        //This method returns the total trade in amount for the cart
        public double returnTradeInAmount(List<Cart> itemsSold, int loc)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;
            //Checks the range of trade in sku's by location
            int[] range = idu.tradeInSkuRange(loc);
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

        //This method returns the total subtotal amount for the cart
        public double returnSubtotalAmount(List<Cart> itemsSold, int loc)
        {
            double totalSubtotalAmount = 0;
            //Gets the total discount value of the cart
            double totalDiscountAmount = returnDiscount(itemsSold);
            //Gets the total trade in value of the cart
            double totalTradeInAmount = returnTradeInAmount(itemsSold, loc);
            //Gets the total total value of the cart
            double totalTotalAmount = returnTotalAmount(itemsSold, loc);
            //Calculations using the above three methods to determine the total subtotal value
            totalSubtotalAmount = totalSubtotalAmount + totalTotalAmount;
            totalSubtotalAmount = totalSubtotalAmount - totalDiscountAmount;
            totalSubtotalAmount = totalSubtotalAmount - (totalTradeInAmount * (-1));
            //Returns the subtotal value of the cart
            return totalSubtotalAmount;
        }

        //This method returns the total total amount of the cart
        public double returnTotalAmount(List<Cart> itemsSold, int loc)
        {
            //Checks the range of trade in sku's by location
            int[] range = idu.tradeInSkuRange(loc);
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

        //This method returns the total refund subtotal amount
        public double returnRefundTotalAmount(List<Cart> itemsSold)
        {
            double singleRefundSubtotalAmount = 0;
            double totalRefundSubtotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var cart in itemsSold)
            {
                singleRefundSubtotalAmount = cart.quantity * cart.returnAmount;
                totalRefundSubtotalAmount += singleRefundSubtotalAmount;
            }
            //Returns the total refund subtotal amount
            return totalRefundSubtotalAmount;
        }

        //This method returns the gst amount of the cart based on subtotal
        public double returnGSTAmount(double rate, double subtotal)
        {
            double GSTAmount = 0;
            GSTAmount = Math.Round((rate * subtotal), 2);
            //Returns the gst amount 
            return GSTAmount;
        }

        //This method returns the pst amount of the cart based on subtotal
        public double returnPSTAmount(double rate, double subtotal)
        {
            double PSTAmount = 0;
            PSTAmount = Math.Round((rate * subtotal), 2);
            //Returns the pst amount
            return PSTAmount;
        }

        //This method returns the locationID based on a string location name
        public int returnLocationID(int lID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            int locID = 0;
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getprovStateIDFromCity", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@cityName", lID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                locID = Convert.ToInt32(row["provStateID"]);
            }
            //Returns the locationID
            return locID;
        }
        
    }
}