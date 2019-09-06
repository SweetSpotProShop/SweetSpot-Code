using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The cashout class is used to define and create easy to access cashout information for the cashout report.
    [Serializable]
    public class Cashout
    {
        public DateTime dtmCashoutDate { get; set; }
        public DateTime dtmCashoutTime { get; set; }
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
        public double fltProvincialTaxAmount { get; set; }
        public double fltCashDrawerOverShort { get; set; }
        public bool bitIsCashoutFinalized { get; set; }
        public bool bitIsCashoutProcessed { get; set; }
        public int intLocationID { get; set; }
        public int intEmployeeID { get; set; }
        

        public Cashout() { } //These could be cleaned up after the cashout process get cleaned up
        //public Cashout(DateTime CDate, DateTime CTime, double SalesTradeIn, double SalesGiftCard, 
        //    double SalesCash, double SalesDebit, double SalesMasterCard, double SalesVisa, 
        //    double ReceiptsTradeIn, double ReceiptsGiftCard, double ReceiptsCash, double ReceiptsDebit, 
        //    double ReceiptsMasterCard, double ReceiptsVisa, double PreTax, double GovTax, double ProvTax,
        //    double OverOrShort, bool Final, bool Process, int LocID, int EmployeeID)
        //{
        //    cashoutDate = CDate;
        //    cashoutTime = CTime;
        //    saleTradeIn = SalesTradeIn;
        //    saleGiftCard = SalesGiftCard;
        //    saleCash = SalesCash;
        //    saleDebit = SalesDebit;
        //    saleMasterCard = SalesMasterCard;
        //    saleVisa = SalesVisa;
        //    receiptTradeIn = ReceiptsTradeIn;
        //    receiptGiftCard = ReceiptsGiftCard;
        //    receiptCash = ReceiptsCash;
        //    receiptDebit = ReceiptsDebit;
        //    receiptMasterCard = ReceiptsMasterCard;
        //    receiptVisa = ReceiptsVisa;
        //    preTax = PreTax;
        //    saleGST = GovTax;
        //    salePST = ProvTax;
        //    overShort = OverOrShort;
        //    finalized = Final;
        //    processed = Process;
        //    locationID = LocID;
        //    empID = EmployeeID;
        //}
    }
}