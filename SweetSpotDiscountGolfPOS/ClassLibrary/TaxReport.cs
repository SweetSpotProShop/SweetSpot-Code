using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class TaxReport
    {
        public DateTime dtmInvoiceDate;
        public int locationID;
        public double subTotal;
        public double shippingAmount;
        public double discountAmount;
        public double tradeInAmount;
        public double govTax;
        public double provTax;
        public double balanceDue;
        public int transactionType;

        public TaxReport() { }
        public TaxReport(DateTime Dtm, int L, double Sub, double Ship, double D, double T,
            double G, double P, double B, int Tran)
        {
            dtmInvoiceDate = Dtm;
            locationID = L;
            subTotal = Sub;
            shippingAmount = Ship;
            discountAmount = D;
            tradeInAmount = T;
            govTax = G;
            provTax = P;
            balanceDue = B;
            transactionType = Tran;
        }
    }
}