using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The invoice MOPS class is used to define and keep track of what an invoice MOP is.
    //Used in storing the MOPs from a sale in the database
    public class InvoiceMOPs
    {
        public int intInvoicePaymentID { get; set; }
        public int intInvoiceID { get; set; }
        public int intPaymentID { get; set; }
        public string varPaymentName { get; set; }
        public double fltAmountPaid { get; set; }
        public double fltTenderedAmount { get; set; }
        public double fltCustomerChange { get; set; }
        public int intChequeNumber { get; set; }

        public InvoiceMOPs() { }
    }
}