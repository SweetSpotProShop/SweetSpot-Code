using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.OB
{
    public class Location
    {
        //Used to define and create a location
        public int intLocationID { get; set; }
        public string varLocationName { get; set; }
        public string varContactNumber { get; set; }
        public string secondaryPhone { get; set; }
        public string varEmailAddress { get; set; }
        public string varAddress { get; set; }
        public string varCityName { get; set; }
        public int intProvinceID { get; set; }
        public int intCountryID { get; set; }
        public string varPostalCode { get; set; }
        public string varSecondLocationID { get; set; }
        public string varTaxNumber { get; set; }
        public bool bitIsRetailStore { get; set; }

        public Location() { }
    }
}