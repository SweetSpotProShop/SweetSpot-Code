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
        public double overShort { get; set; }
        public bool finalized { get; set; }
        public bool processed { get; set; }
        public int locationID { get; set; }
        public int empID { get; set; }


        public string mop { get; set; }
        public double amount { get; set; }
        public double tradeIn { get; set; }
        public double saleGST { get; set; }
        public double salePST { get; set; }
        public double saleSubTotal { get; set; }
        public double receiptGST { get; set; }
        public double receiptPST { get; set; }
        public double receiptSubTotal { get; set; }
        public double preTax { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public double shippingAmount { get; set; }


        public Cashout() { } //These could be cleaned up after the cashout process get cleaned up
        public Cashout(string m, double a)
        {
            mop = m;
            amount = a;
        }
        public Cashout(DateTime CDate, DateTime CTime, double SalesTradeIn, double SalesGiftCard, 
            double SalesCash, double SalesDebit, double SalesMasterCard, double SalesVisa, 
            double ReceiptsTradeIn, double ReceiptsGiftCard, double ReceiptsCash, double ReceiptsDebit, 
            double ReceiptsMasterCard, double ReceiptsVisa, double OverOrShort, bool Final, bool Process, 
            int LocID, int EmployeeID)
        {
            cashoutDate = CDate;//
            cashoutTime = CTime;//
            saleTradeIn = SalesTradeIn;//
            saleGiftCard = SalesGiftCard;//
            saleCash = SalesCash;//
            saleDebit = SalesDebit;//
            saleMasterCard = SalesMasterCard;//
            saleVisa = SalesVisa;//
            receiptTradeIn = ReceiptsTradeIn;//
            receiptGiftCard = ReceiptsGiftCard;//
            receiptCash = ReceiptsCash;//
            receiptDebit = ReceiptsDebit;//
            receiptMasterCard = ReceiptsMasterCard;//
            receiptVisa = ReceiptsVisa;//
            overShort = OverOrShort;//
            finalized = Final;//
            processed = Process;//
            locationID = LocID;
            empID = EmployeeID;
        }






        //*** This is a MOP not a cashout, needs to be deleted. MOP types and how much was paid
        //public Cashout(string m, double a)
        //{
        //    mop = m;
        //    amount = a;
        //}
        //This is used to store the rest of the data from the invoices
        public Cashout(double tiTotal, double sTotal, double gTax, double pTax, double shTotal)
        {
            saleTradeIn = tiTotal;
            saleSubTotal = sTotal;
            saleGST = gTax;
            salePST = pTax;
            shippingAmount = shTotal;
        }
        public Cashout(double sgt, double spt, double ssub)
        {
            saleGST = sgt;
            salePST = spt;
            saleSubTotal = ssub;
        }
        //Used to store values for Session["saleCashout"]
        public Cashout(double st, double sg, double sc, double sd, double smc,
            double sv, double sgt, double spt, double ssub)
        {
            saleTradeIn = st;
            saleGiftCard = sg;
            saleCash = sc;
            saleDebit = sd;
            saleMasterCard = smc;
            saleVisa = sv;
            saleGST = sgt;
            salePST = spt;
            saleSubTotal = ssub;
        }
        //Used to store values for Session["receiptCashout"]
        public Cashout(string throwaway, double rt, double rg, double rc, double rd,
            double rmc, double rv, double rgt, double rpt, double rsub, double os)
        {
            receiptTradeIn = rt;
            receiptGiftCard = rg;
            receiptCash = rc;
            receiptDebit = rd;
            receiptMasterCard = rmc;
            receiptVisa = rv;
            receiptGST = rgt;
            receiptPST = rpt;
            receiptSubTotal = rsub;
            overShort = os;
        }
        //Used for storing the cashout
        public Cashout(string d, string t, double st, double sg, double sc, double sd, double smc,
            double sv, double sgt, double spt, double ssub, double rt, double rg, double rc, double rd,
            double rmc, double rv, double rgt, double rpt, double rsub, double os, bool f, bool p, double prt, int lid, int eid)
        {
            date = d;
            time = t;

            saleTradeIn = st;
            saleGiftCard = sg;
            saleCash = sc;
            saleDebit = sd;
            saleMasterCard = smc;
            saleVisa = sv;
            saleGST = sgt;
            salePST = spt;
            saleSubTotal = ssub;
            receiptTradeIn = rt;
            receiptGiftCard = rg;
            receiptCash = rc;
            receiptDebit = rd;
            receiptMasterCard = rmc;
            receiptVisa = rv;
            receiptGST = rgt;
            receiptPST = rpt;
            receiptSubTotal = rsub;
            preTax = prt;
            overShort = os;
            finalized = f;
            processed = p;
            locationID = lid;
            empID = eid;

        }
    }
}