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
        public int cheque { get; set; }
        public double tender { get; set; }
        public double change { get; set; }

        public InvoiceMOPs() { }
        public InvoiceMOPs(int ID, int InvoiceNum, int InvoiceSubNum, string MopType, double AmountPaid, double Tender, double Change)
        {
            id = ID;
            invoiceNum = InvoiceNum;
            invoiceSubNum = InvoiceSubNum;
            mopType = MopType;
            amountPaid = AmountPaid;
            tender = Tender;
            change = Change;
        }
        public InvoiceMOPs(int ID, int InvoiceNum, string MopType, int Cheque, double AmountPaid)
        {
            id = ID;
            invoiceNum = InvoiceNum;
            mopType = MopType;
            cheque = Cheque;
            amountPaid = AmountPaid;
        }
    }
}