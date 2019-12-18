using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class ItemChangeTracking
    {
        public int intChangeID { get; set; }
        public DateTime dtmChangeDate { get; set; }
        public DateTime dtmChangeTime { get; set; }
        public int intEmployeeID { get; set; }
        public int intLocationID { get; set; }
        public int intInventoryID { get; set; }
        public double fltOriginalCost { get; set; }
        public double fltNewCost { get; set; }
        public double fltOriginalPrice { get; set; }
        public double fltNewPrice { get; set; }
        public int intOriginalQuantity { get; set; }
        public int intNewQuantity { get; set; }
        public string varOriginalDescription { get; set; }
        public string varNewDescription { get; set; }

        public ItemChangeTracking() { }
    }
}