using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Location
    {
        //Used to define and create a location
        public int locationID { get; set; }
        public string locationName { get; set; }
        public string primaryPhone { get; set; }
        public string secondaryPhone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int provID { get; set; }
        public int countryID { get; set; }
        public string postal { get; set; }
        public string relationName { get; set; }
        public string taxNumber { get; set; }

        public Location() { }
        public Location(int ID, string Name, string PPhone, string SPhone, string EmailAddy, string Addy, string City, int ProvID, int CountryID, string PostZip,
            string RelationName, string TaxNumber)
        {
            locationID = ID;
            locationName = Name;
            primaryPhone = PPhone;
            secondaryPhone = SPhone;
            email = EmailAddy;
            address = Addy;
            city = City;
            provID = ProvID;
            countryID = CountryID;
            postal = PostZip;
            relationName = RelationName;
            taxNumber = TaxNumber;
        }
        public Location(int id, string name)
        {
            locationID = id;
            locationName = name;
        }
    }
}