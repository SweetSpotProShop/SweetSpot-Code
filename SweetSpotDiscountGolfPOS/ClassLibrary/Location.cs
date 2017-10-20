using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Location
    {
        //Used to define and create a location
        public string location { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int provID { get; set; }
        public string postal { get; set; }
        public string phone { get; set; }

        public Location() { }
        public Location(string l, string a, string c, int pID, string p, string ph)
        {
            location = l;
            address = a;
            city = c;
            provID = pID;
            postal = p;
            phone = ph;
        }
    }
}