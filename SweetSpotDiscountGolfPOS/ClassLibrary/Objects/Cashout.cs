using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The cashout class is used to define and create easy to access cashout information for the cashout report.
    [Serializable]
    public class Cashout
    {
        public DateTime dtmCashoutDate { get; set; }
        public DateTime dtmCashoutTime { get; set; }
        public int intEmployeeID { get; set; }
        public int intLocationID { get; set; }
        public double fltSystemCountedBasedOnSystemTradeIn { get; set; }
        public double fltSystemCountedBasedOnSystemGiftCard { get; set; }
        public double fltSystemCountedBasedOnSystemCash { get; set; }
        public double fltSystemCountedBasedOnSystemDebit { get; set; }
        public double fltSystemCountedBasedOnSystemMastercard { get; set; }
        public double fltSystemCountedBasedOnSystemVisa { get; set; }
        public double fltManuallyCountedBasedOnReceiptsTradeIn { get; set; }
        public double fltManuallyCountedBasedOnReceiptsGiftCard { get; set; }
        public double fltManuallyCountedBasedOnReceiptsCash { get; set; }
        public double fltManuallyCountedBasedOnReceiptsDebit { get; set; }
        public double fltManuallyCountedBasedOnReceiptsMastercard { get; set; }
        public double fltManuallyCountedBasedOnReceiptsVisa { get; set; }
        public double fltSalesSubTotal { get; set; }
        public double fltGovernmentTaxAmount { get; set; }
        public double fltHarmonizedTaxAmount { get; set; }
        public double fltLiquorTaxAmount { get; set; }
        public double fltProvincialTaxAmount { get; set; }
        public double fltQuebecTaxAmount { get; set; }
        public double fltRetailTaxAmount { get; set; }
        public double fltCashDrawerOverShort { get; set; }
        public bool bitIsCashoutFinalized { get; set; }
        public bool bitIsCashoutProcessed { get; set; }
        public int intSalesCount { get; set; }

        public Cashout() { }
    }
}