using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class TaxReport
    {
        public DateTime dtmInvoiceDate { get; set; }
        public int intLocationID { get; set; }
        public double fltGovernmentTaxAmountCollected { get; set; }
        public double fltProvincialTaxAmountCollected { get; set; }
        public double fltLiquorTaxAmountCollected { get; set; }
        public double fltGovernmentTaxAmountReturned { get; set; }
        public double fltProvincialTaxAmountReturned { get; set; }
        public double fltLiquorTaxAmountReturned { get; set; }        
        
        public TaxReport() { }
        
    }
}