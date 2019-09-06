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
                intLocationID = row.Field<int>("intLocationID"),
                varLocationName = row.Field<string>("varLocationName"),
                varContactNumber = row.Field<string>("varContactNumber"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varAddress = row.Field<string>("varAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode"),
                varSecondLocationID = row.Field<string>("varSecondLocationID"),
                varTaxNumber = row.Field<string>("varTaxNumber")
            }).ToList();
            return location;
        }
        private Nullable<int> MakeDataBaseCallToReturnInt(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                int returnInt = Convert.ToInt32(dbc.returnDataTableData(sqlCmd, parms).Rows[0][0]);
                //int returnInt = Convert.ToInt32(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]);
                return returnInt;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Returns list of locations based on a location ID
        public List<Location> ReturnLocation(int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnLocation";
            string sqlCmd = "SELECT intLocationID, varLocationName, varContactNumber, varEmailAddress, varAddress, "
                + "varCityName, intProvinceID, intCountryID, varPostalCode, varSecondLocationID, varTaxNumber FROM "
                + "tbl_location WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                 new object[] { "@intLocationID", locationID }
            };

            return ConvertFromDataTableToLocation(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToLocation(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
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

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnCountryName(int countryID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCountryName";
            string sqlCmd = "SELECT countryDesc FROM tbl_country WHERE countryID = @countryID";

            object[][] parms =
            {
                 new object[] { "@countryID", countryID }
            };
            
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnLocationName(int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationName";
            string sqlCmd = "SELECT varLocationName FROM tbl_location WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                 new object[] { "@intLocationID", locationID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public DataTable ReturnLocationDropDown(object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationDropDown";
            string sqlCmd = "SELECT intLocationID, varCityName FROM tbl_location WHERE bitIsRetailStore = 1";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnLocationDropDownAll(object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationDropDownAll";
            string sqlCmd = "SELECT locationID, locationName FROM tbl_location "
                + "ORDER BY locationName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnProvinceDropDown(int countryID, object[] objPageDetails)
        {
            string strQueryName = "ReturnProvinceDropDown";
            string sqlCmd = "SELECT intProvinceID, varProvinceName FROM tbl_provSTate WHERE intCountryID = @intCountryID";
            object[][] parms = { new object[] { "@intCountryID", countryID} };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnCountryDropDown(object[] objPageDetails)
        {
            string strQueryName = "ReturnCountryDropDown";
            string sqlCmd = "SELECT intCountryID, varCountryName FROM tbl_country";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
    }
}