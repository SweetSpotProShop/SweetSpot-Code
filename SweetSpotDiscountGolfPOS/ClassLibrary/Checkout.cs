using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The checkout class is used to define and create easy to access checkout information for sales.
    public class Checkout
    {
        public string methodOfPayment { get; set; }
        public double amountPaid { get; set; }
        public int tableID { get; set; }
        public int chequeNum { get; set; }

        public Checkout() { }
        public Checkout(string m, double a)
        {
            methodOfPayment = m;
            amountPaid = a;
        }
        public Checkout(string m, double a, int i)
        {
            methodOfPayment = m;
            amountPaid = a;
            tableID = i;
        }
        public Checkout(string m, double a, int i, int c)
        {
            methodOfPayment = m;
            amountPaid = a;
            tableID = i;
            chequeNum = c;
        }
    }
}