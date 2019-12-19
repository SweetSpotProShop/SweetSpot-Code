using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
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
        public double fltProvincialTaxAmount { get; set; }
        public double fltLiquorTaxAmount { get; set; }
        public int intTransactionTypeID { get; set; }
        //public string varTransactionName { get; set; }
        public string varAdditionalInformation { get; set; }
        //public bool bitChargeGST { get; set; }
        //public bool bitChargePST { get; set; }
        //public bool bitChargeLCT { get; set; }
        public List<InvoiceItems> invoiceItems { get; set; }
        public List<InvoiceMOPs> invoiceMops { get; set; }
        //public List<InvoiceItemTax> invoiceItemTaxes { get; set; }

        public Invoice() { }
    }
}
