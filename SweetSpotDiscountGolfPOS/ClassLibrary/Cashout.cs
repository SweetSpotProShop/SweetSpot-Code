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
        public DateTime cashoutDate { get; set; }
        public DateTime cashoutTime { get; set; }
        public double saleTradeIn { get; set; }
        public double saleGiftCard { get; set; }
        public double saleCash { get; set; }
        public double saleDebit { get; set; }
        public double saleMasterCard { get; set; }
        public double saleVisa { get; set; }
        public double receiptTradeIn { get; set; }
        public double receiptGiftCard { get; set; }
        public double receiptCash { get; set; }
        public double receiptDebit { get; set; }
        public double receiptMasterCard { get; set; }
        public double receiptVisa { get; set; }
        public double preTax { get; set; }
        public double saleGST { get; set; }
        public double salePST { get; set; }
        public double overShort { get; set; }
        public bool finalized { get; set; }
        public bool processed { get; set; }
        public int locationID { get; set; }
        public int empID { get; set; }
        

        public Cashout() { } //These could be cleaned up after the cashout process get cleaned up
        public Cashout(DateTime CDate, DateTime CTime, double SalesTradeIn, double SalesGiftCard, 
            double SalesCash, double SalesDebit, double SalesMasterCard, double SalesVisa, 
            double ReceiptsTradeIn, double ReceiptsGiftCard, double ReceiptsCash, double ReceiptsDebit, 
            double ReceiptsMasterCard, double ReceiptsVisa, double PreTax, double GovTax, double ProvTax,
            double OverOrShort, bool Final, bool Process, int LocID, int EmployeeID)
        {
            cashoutDate = CDate;
            cashoutTime = CTime;
            saleTradeIn = SalesTradeIn;
            saleGiftCard = SalesGiftCard;
            saleCash = SalesCash;
            saleDebit = SalesDebit;
            saleMasterCard = SalesMasterCard;
            saleVisa = SalesVisa;
            receiptTradeIn = ReceiptsTradeIn;
            receiptGiftCard = ReceiptsGiftCard;
            receiptCash = ReceiptsCash;
            receiptDebit = ReceiptsDebit;
            receiptMasterCard = ReceiptsMasterCard;
            receiptVisa = ReceiptsVisa;
            preTax = PreTax;
            saleGST = GovTax;
            salePST = ProvTax;
            overShort = OverOrShort;
            finalized = Final;
            processed = Process;
            locationID = LocID;
            empID = EmployeeID;
        }
    }
}