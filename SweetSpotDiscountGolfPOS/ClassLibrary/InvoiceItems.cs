using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The invoice items class is used to define and keep track of what an invoice item is.
    //Used in storing the items from a sale in the database
    public class InvoiceItems
    {

        //Specific to Invoice Items class
        public int invoiceNum { get; set; }
        public int invoiceSubNum { get; set; }
        public double itemRefund { get; set; }

        //Tied to both classes
        public int sku { get; set; }
        public string itemDescription { get; set; }
        public int itemQuantity { get; set; }
        public double itemCost { get; set; }
        public double itemPrice { get; set; }
        public double itemDiscount { get; set; }
        public bool percentage { get; set; }
        public int typeID { get; set; }
        public bool tradeIn { get; set; }

        //May be able to amalgamate all into one and then remove the Items and ItemsManager Classes

        public InvoiceItems(){}
        //InvoiceItems
        public InvoiceItems(int InvoiceNum, int InvoiceSubNum, int Sku, int ItemQuantity, double ItemCost, double ItemPrice, 
            double ItemDiscount, double ItemRefund, bool Percentage)
        {
            invoiceNum = InvoiceNum;
            invoiceSubNum = InvoiceSubNum;
            sku = Sku;
            itemQuantity = ItemQuantity;
            itemCost = ItemCost;
            itemPrice = ItemPrice;
            itemDiscount = ItemDiscount;
            itemRefund = ItemRefund;
            percentage = Percentage;
        }
        //InvoiceItemsWithDescription
        public InvoiceItems(int InvoiceNum, int InvoiceSubNum, int Sku, string ItemDescription, int ItemQuantity, double ItemCost, 
            double ItemPrice, double ItemDiscount, double ItemRefund, bool Percentage)
        {
            invoiceNum = InvoiceNum;
            invoiceSubNum = InvoiceSubNum;
            sku = Sku;
            itemDescription = ItemDescription;
            itemQuantity = ItemQuantity;
            itemCost = ItemCost;
            itemPrice = ItemPrice;
            itemDiscount = ItemDiscount;
            itemRefund = ItemRefund;
            percentage = Percentage;
        }
        //InvoiceItemsInCurrentSalesItems
        public InvoiceItems(int InvoiceNum, int InvoiceSubNum, int Sku, string ItemDescription, int ItemQuantity, double ItemCost,
            double ItemPrice, double ItemDiscount, double ItemRefund, bool Percentage, int TypeID, bool TradeIN)
        {
            invoiceNum = InvoiceNum;
            invoiceSubNum = InvoiceSubNum;
            sku = Sku;
            itemDescription = ItemDescription;
            itemQuantity = ItemQuantity;
            itemCost = ItemCost;
            itemPrice = ItemPrice;
            itemDiscount = ItemDiscount;
            itemRefund = ItemRefund;
            percentage = Percentage;
            typeID = TypeID;
            tradeIn = TradeIN;
        }
    }
}