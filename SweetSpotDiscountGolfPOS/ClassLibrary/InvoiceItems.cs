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
        public string description { get; set; }
        public int quantity { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public double itemDiscount { get; set; }
        public bool percentage { get; set; }
        public int typeID { get; set; }
        public bool isTradeIn { get; set; }

        //May be able to amalgamate all into one and then remove the Items and ItemsManager Classes
        public InvoiceItems(){}
        //InvoiceItemsInCurrentSalesItems
        public InvoiceItems(int InvoiceNum, int InvoiceSubNum, int Sku, string Description, int Quantity, double Cost,
            double Price, double ItemDiscount, double ItemRefund, bool Percentage, int TypeID, bool isTradeIN)
        {
            invoiceNum = InvoiceNum;
            invoiceSubNum = InvoiceSubNum;
            sku = Sku;
            description = Description;
            quantity = Quantity;
            cost = Cost;
            price = Price;
            itemDiscount = ItemDiscount;
            itemRefund = ItemRefund;
            percentage = Percentage;
            typeID = TypeID;
            isTradeIn = isTradeIN;
        }
    }
}