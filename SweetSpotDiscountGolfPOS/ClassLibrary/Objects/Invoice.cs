using System;
using System.Collections.Generic;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The invoice class is used to keep track or populate the printable invoice webpage with the current invoice's information
    public class Invoice
    {
        public int intInvoiceID { get; set; }
        public string varInvoiceNumber { get; set; }
        public int intInvoiceSubNumber { get; set; }
        public DateTime dtmInvoiceDate { get; set; }
        public DateTime dtmInvoiceTime { get; set; }
        public Customer customer { get; set; }
        public Employee employee { get; set; }
        public Location location { get; set; }
        public int intShippingProvinceID { get; set; }        
        public double fltShippingCharges { get; set; }
        public bool bitIsShipping { get; set; }
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
        public double fltShippingTaxAmount { get; set; }
        public int intTransactionTypeID { get; set; }
        public string varAdditionalInformation { get; set; }
        public List<InvoiceItems> invoiceItems { get; set; }
        public List<InvoiceMOPs> invoiceMops { get; set; }

        public Invoice() { }
    }
}
