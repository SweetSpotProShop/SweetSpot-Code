﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class TaxReport
    {
        public DateTime dtmInvoiceDate { get; set; }
        public int locationID { get; set; }
        public double subTotal { get; set; }
        public double shippingAmount { get; set; }
        public double discountAmount { get; set; }
        public double tradeInAmount { get; set; }
        public double govTax { get; set; }
        public double provTax { get; set; }
        public double balanceDue { get; set; }
        public int transactionType { get; set; }

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