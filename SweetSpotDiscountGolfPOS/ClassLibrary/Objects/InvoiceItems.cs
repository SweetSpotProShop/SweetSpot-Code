using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The invoice items class is used to define and keep track of what an invoice item is.
    //Used in storing the items from a sale in the database
    [Serializable]
    public class InvoiceItems
    {
        public int intInvoiceItemID { get; set; }
        public int intInvoiceID { get; set; }
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public string varItemDescription { get; set; }
        public int intItemQuantity { get; set; }
        public double fltItemCost { get; set; }
        public double fltItemPrice { get; set; }
        public double fltItemDiscount { get; set; }
        public double fltItemRefund { get; set; }
        public bool bitIsDiscountPercent { get; set; }
        public int intItemTypeID { get; set; }
        public bool bitIsClubTradeIn { get; set; }
        public int intLocationID { get; set; }
        public List<InvoiceItemTax> invoiceItemTaxes { get; set; }
        public string varLocationName { get; set; }
        public string varAdditionalInformation { get; set; }

        //May be able to amalgamate all into one and then remove the Items and ItemsManager Classes
        public InvoiceItems(){}

    }
}