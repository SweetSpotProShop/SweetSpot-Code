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
        public int invoiceNum { get; set; }
        public int invoiceSubNum { get; set; }
        public int sku { get; set; }
        public string itemDescription { get; set; }
        public int itemQuantity { get; set; }
        public double itemCost { get; set; }
        public double itemPrice { get; set; }
        public double itemDiscount { get; set; }
        public double itemRefund { get; set; }
        public bool percentage { get; set; }

        public InvoiceItems(){}
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
    }
}