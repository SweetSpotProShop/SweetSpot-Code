using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class ChangeItem
    {
        public int sku { get; set; }
        public double dblOriginalCost { get; set; }
        public double dblNewCost { get; set; }
        public double dblOriginalPrice { get; set; }
        public double dblNewPrice { get; set; }
        public int intOriginalQuantity { get; set; }
        public int intNewQuantity { get; set; }
        public string strOriginalDescription { get; set; }
        public string strNewDescription { get; set; }

        public ChangeItem() { }
    }
}