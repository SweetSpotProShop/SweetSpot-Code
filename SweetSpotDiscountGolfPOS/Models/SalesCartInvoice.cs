using System;
using System.Collections.Generic;

namespace SweetSpotDiscountGolfPOS.Models
{
    //The invoice class is used to keep track or populate the printable invoice webpage with the current invoice's information
    public class SalesCartInvoice
    {




        public int intInvoiceID { get; set; }
        public string varInvoiceNumber { get; set; }
        public int intInvoiceSubNumber { get; set; }
        public DateTime dtmInvoiceDate { get; set; }
        public DateTime dtmInvoiceTime { get; set; }
        public int intCustomerID { get; set; }
        public string varCustName { get; set; }
        public int intEmployeeID { get; set; }
        public string varEmpName { get; set; }
        public int intLocationID { get; set; }
        public string varLocName { get; set; }
        public int intShippingProvinceID { get; set; }        
        public double fltShippingCharges { get; set; }
        public bool bitIsShipping { get; set; }
        public double fltShippingTaxAmount { get; set; }
        public double fltTotalDiscount { get; set; }
        public double fltTotalTradeIn { get; set; }
        public double fltSubTotal { get; set; }
        public double fltBalanceDue { get; set; }
        public double fltGovernmentTaxAmount { get; set; }
        public double fltHarmonizedTaxAmount { get; set; }
        public double fltLiquorTaxAmount { get; set; }
        public double fltProvincialTaxAmount { get; set; }
        public double fltQuebecTaxAmount { get; set; }
        public double fltRetailTaxAmount { get; set; }
        public int intTransactionTypeID { get; set; }
        public string varTransactionName { get; set; }
        public string varAdditionalInformation { get; set; }

        public SalesCartInvoice() { }
    }
}
