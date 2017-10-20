using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The checkout manager class is used to define and create easy to access checkout manager information for sales.
    public class CheckoutManager
    {
        public double dblTotal { get; set; }
        public double dblDiscounts { get; set; }
        public double dblTradeIn { get; set; }
        public double dblShipping { get; set; }
        public double dblSubTotal { get; set; }
        public bool blGst { get; set; }
        public bool blPst { get; set; }
        public double dblGst { get; set; }
        public double dblPst { get; set; }
        public double dblRemainingBalance { get; set; }
        public double dblAmountPaid { get; set; }
        public double dblBalanceDue { get; set; }

        public CheckoutManager() { }
        public CheckoutManager(double T, double D, double TI, double S, bool bG, bool bP, double dG, double dP, double AP)
        {//Subtotals are calculated differently both of these are getting used so need to determin which is right and remove the other
            dblTotal = T;
            dblDiscounts = D;
            dblTradeIn = TI;
            dblShipping = S;
            blGst = bG;
            blPst = bP;
            dblGst = dG;
            dblPst = dP;
            dblAmountPaid = AP;
            dblSubTotal = T + TI - D;
            dblRemainingBalance = dblSubTotal + S;
            dblBalanceDue = dblSubTotal + S + dblGst + dblPst;
        }
        public CheckoutManager(double D, double TI, double S, bool bG, bool bP, double dG, double dP, double AP, double st)
        {//Subtotals are calculated differently both of these are getting used so need to determin which is right and remove the other
            dblDiscounts = D;
            dblTradeIn = TI;
            dblShipping = S;
            blGst = bG;
            blPst = bP;
            dblGst = dG;
            dblPst = dP;
            dblAmountPaid = AP;
            dblSubTotal = st;
            dblRemainingBalance = dblSubTotal + S;
            dblBalanceDue = dblSubTotal + S + dblGst + dblPst;
        }
    }
}