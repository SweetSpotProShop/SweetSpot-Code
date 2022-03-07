using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.OB
{
    public class TaxReport
    {
        public DateTime dtmInvoiceDate { get; set; }
        public int intLocationID { get; set; }
        public double fltGovernmentTaxAmountCollected { get; set; }
        public double fltHarmonizedTaxAmountCollected { get; set; }
        public double fltLiquorTaxAmountCollected { get; set; }
        public double fltProvincialTaxAmountCollected { get; set; }
        public double fltQuebecTaxAmountCollected { get; set; }
        public double fltRetailTaxAmountCollected { get; set; }

        public double fltGovernmentTaxAmountReturned { get; set; }
        public double fltHarmonizedTaxAmountReturned { get; set; }
        public double fltLiquorTaxAmountReturned { get; set; }
        public double fltProvincialTaxAmountReturned { get; set; }
        public double fltQuebecTaxAmountReturned { get; set; }
        public double fltRetailTaxAmountReturned { get; set; }

        public TaxReport() { }
        
    }
}