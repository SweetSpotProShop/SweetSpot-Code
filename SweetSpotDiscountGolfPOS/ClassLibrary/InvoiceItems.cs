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
        public int itemQuantity { get; set; }
        public double itemCost { get; set; }
        public double itemPrice { get; set; }
        public double itemDiscount { get; set; }
        public bool percentage { get; set; }

        public InvoiceItems(){}
        public InvoiceItems(int inn, int isn, int s, int iq, double ic, double ip, double id, bool p)
        {
            invoiceNum = inn;
            invoiceSubNum = isn;
            sku = s;
            itemQuantity = iq;
            itemCost = ic;
            itemPrice = ip;
            itemDiscount = id;
            percentage = p;
        }
    }
}