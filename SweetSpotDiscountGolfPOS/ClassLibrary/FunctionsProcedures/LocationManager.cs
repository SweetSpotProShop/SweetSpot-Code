using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS.FP
{
    //Used to gather information about locations
    public class LocationManager
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();
        public LocationManager() { }

        //Converter
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




        //DB calls
        //Returns list of locations based on a location ID
        private List<Location> ReturnLocation(int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnLocation";
            string sqlCmd = "SELECT intLocationID, varLocationName, varContactNumber, varEmailAddress, varAddress, "
                + "varCityName, intProvinceID, intCountryID, varPostalCode, varSecondLocationID, varTaxNumber FROM "
                + "tbl_location WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                 new object[] { "@intLocationID", locationID }
            };

            return ConvertFromDataTableToLocation(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ConvertFromDataTableToLocation(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        //Provinve/State Name based on Province/State ID
        private string ReturnProvinceName(int provID, object[] objPageDetails)
        {
            string strQueryName = "ReturnProvinceName";
            string sqlCmd = "SELECT provName FROM tbl_provState WHERE provStateID = @provStateID";

            object[][] parms =
            {
                 new object[] { "@provStateID", provID }
            };

            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //private string ReturnCountryName(int countryID, object[] objPageDetails)
        //{
        //    string strQueryName = "ReturnCountryName";
        //    string sqlCmd = "SELECT countryDesc FROM tbl_country WHERE countryID = @countryID";

        //    object[][] parms =
        //    {
        //         new object[] { "@countryID", countryID }
        //    };
            
        //    return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        //    //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        //}
        private string ReturnLocationName(int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationName";
            string sqlCmd = "SELECT varLocationName FROM tbl_location WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                 new object[] { "@intLocationID", locationID }
            };

            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnLocationDropDown(object[] objPageDetails)
        {
            string strQueryName = "ReturnLocationDropDown";
            string sqlCmd = "SELECT intLocationID, CONCAT(varLocationName, ' - ', varCityName) AS varCityName FROM tbl_location WHERE bitIsRetailStore = 1";
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //private DataTable ReturnLocationDropDownAll(object[] objPageDetails)
        //{
        //    string strQueryName = "ReturnLocationDropDownAll";
        //    string sqlCmd = "SELECT locationID, locationName FROM tbl_location "
        //        + "ORDER BY locationName";
        //    object[][] parms = { };
        //    return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        //    //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        //}
        private DataTable ReturnProvinceDropDown(int countryID, object[] objPageDetails)
        {
            string strQueryName = "ReturnProvinceDropDown";
            string sqlCmd = "SELECT intProvinceID, varProvinceName FROM tbl_provState WHERE intCountryID = @intCountryID ORDER BY varProvinceName";
            object[][] parms = { new object[] { "@intCountryID", countryID} };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnCountryDropDown(object[] objPageDetails)
        {
            string strQueryName = "ReturnCountryDropDown";
            string sqlCmd = "SELECT intCountryID, varCountryName FROM tbl_country";
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }


        //Public calls
        public List<Location> CallReturnLocation(int locationID, object[] objPageDetails)
        {
            return ReturnLocation(locationID, objPageDetails);
        }
        public DataTable CallReturnCountryDropDown(object[] objPageDetails)
        {
            return ReturnCountryDropDown(objPageDetails);
        }
        public DataTable CallReturnProvinceDropDown(int countryID, object[] objPageDetails)
        {
            return ReturnProvinceDropDown(countryID, objPageDetails);
        }
        public string CallReturnLocationName(int locationID, object[] objPageDetails)
        {
            return ReturnLocationName(locationID, objPageDetails);
        }
        public DataTable CallReturnLocationDropDown(object[] objPageDetails)
        {
            return ReturnLocationDropDown(objPageDetails);
        }
        public string CallReturnProvinceName(int provID, object[] objPageDetails)
        {
            return ReturnProvinceName(provID, objPageDetails);
        }

    }
}