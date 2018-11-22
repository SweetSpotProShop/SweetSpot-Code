using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //Used to gather information about locations
    public class LocationManager
    {
        DatabaseCalls dbc = new DatabaseCalls();
        public LocationManager() { }
        private List<Location> ConvertFromDataTableToLocation(DataTable dt)
        {
            List<Location> location = dt.AsEnumerable().Select(row =>
            new Location
            {
                locationID = row.Field<int>("locationID"),
                locationName = row.Field<string>("locationName"),
                primaryPhone = row.Field<string>("primaryPhoneINT"),
                secondaryPhone = row.Field<string>("secondaryPhoneINT"),
                email = row.Field<string>("email"),
                address = row.Field<string>("address"),
                city = row.Field<string>("city"),
                provID = row.Field<int>("provStateID"),
                countryID = row.Field<int>("country"),
                postal = row.Field<string>("postZip"),
                relationName = row.Field<string>("secondaryIdentifier"),
                taxNumber = row.Field<string>("taxNumber")
            }).ToList();
            return location;
        }
        private Nullable<int> MakeDataBaseCallToReturnInt(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                int returnInt = Convert.ToInt32(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]);
                return returnInt;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Returns list of locations based on a location ID
        public List<Location> ReturnLocation(int loc, object[] objPageDetails)
        {
            string strQueryName = "ReturnLocation";
            string sqlCmd = "SELECT locationID, locationName, primaryPhoneINT, secondaryPhoneINT, "
                + "email, address,city, provStateID, country, postZip, secondaryIdentifier, "
                + "taxNumber FROM tbl_location WHERE locationID = @locationID";

            object[][] parms =
            {
                 new object[] { "@locationID", loc }
            };

            List<Location> location = ConvertFromDataTableToLocation(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
            return location;
        }
        //Provinve/State Name based on Province/State ID
        public string ReturnProvinceName(int provID, object[] objPageDetails)
        {
            string strQueryName = "ReturnProvinceName";
            string sqlCmd = "SELECT provName FROM tbl_provState WHERE provStateID = @provStateID";

            object[][] parms =
            {
                 new object[] { "@provStateID", provID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnCountryName(int countryID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCountryName";
            string sqlCmd = "SELECT countryDesc FROM tbl_country WHERE countryID = @countryID";

            object[][] parms =
            {
                 new object[] { "@countryID", countryID }
            };
            
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnLocationName(int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationName";
            string sqlCmd = "SELECT locationName FROM tbl_location WHERE locationID = @locationID";

            object[][] parms =
            {
                 new object[] { "@locationID", locationID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public DataTable ReturnLocationDropDown(object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationDropDown";
            string sqlCmd = "SELECT locationID, city FROM tbl_location WHERE isRetailStore = 1";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnLocationDropDownAll(object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationDropDownAll";
            string sqlCmd = "SELECT locationID, locationName FROM tbl_location";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnProvinceDropDown(int countryID, object[] objPageDetails)
        {
            string strQueryName = "ReturnProvinceDropDown";
            string sqlCmd = "SELECT provStateID, provName FROM tbl_provSTate WHERE countryID = @countryID";
            object[][] parms = { new object[] { "@countryID", countryID} };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnCountryDropDown(object[] objPageDetails)
        {
            string strQueryName = "ReturnCountryDropDown";
            string sqlCmd = "SELECT countryID, countryDesc FROM tbl_country";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
    }
}