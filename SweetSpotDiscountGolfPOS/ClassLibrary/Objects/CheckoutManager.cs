using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS.OB
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
    }
}