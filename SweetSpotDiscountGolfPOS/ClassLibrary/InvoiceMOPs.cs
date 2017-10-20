using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The invoice MOPS class is used to define and keep track of what an invoice MOP is.
    //Used in storing the MOPs from a sale in the database
    public class InvoiceMOPs
    {
        public int id { get; set; }
        public int invoiceNum { get; set; }
        public int invoiceSubNum { get; set; }
        public string mopType { get; set; }
        public double amountPaid { get; set; }

        public InvoiceMOPs() { }
        public InvoiceMOPs(int i, int inn, int isn, string mp, double ap)
        {
            id = i;
            invoiceNum = inn;
            invoiceSubNum = isn;
            mopType = mp;
            amountPaid = ap;
        }
    }
}