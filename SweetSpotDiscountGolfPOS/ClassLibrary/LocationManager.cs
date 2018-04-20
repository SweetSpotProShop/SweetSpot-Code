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
        private Nullable<int> MakeDataBaseCallToReturnInt(string sqlCmd, object[][] parms)
        {
            try
            {
                int returnInt = Convert.ToInt32(dbc.returnDataTableData(sqlCmd, parms).Rows[0][0]);
                return returnInt;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //Returns list of locations based on a location ID
        public List<Location> ReturnLocation(int loc)
        {
            string sqlCmd = "SELECT locationID, locationName, primaryPhoneINT, secondaryPhoneINT, "
                + "email, address,city, provStateID, country, postZip, secondaryIdentifier, "
                + "taxNumber FROM tbl_location WHERE locationID = @locationID";

            object[][] parms =
            {
                 new object[] { "@locationID", loc }
            };

            List<Location> location = ConvertFromDataTableToLocation(dbc.returnDataTableData(sqlCmd, parms));
            return location;
        }
        //Provinve/State Name based on Province/State ID
        public string ReturnProvinceName(int provID)
        {
            string sqlCmd = "SELECT provName FROM tbl_provState WHERE provStateID = @provStateID";

            object[][] parms =
            {
                 new object[] { "@provStateID", provID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        public string ReturnCountryName(int countryID)
        {
            string sqlCmd = "SELECT countryDesc FROM tbl_country WHERE countryID = @countryID";

            object[][] parms =
            {
                 new object[] { "@countryID", countryID }
            };
            
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        public string ReturnLocationName(int locationID)
        {
            string sqlCmd = "SELECT locationName FROM tbl_location WHERE locationID = @locationID";

            object[][] parms =
            {
                 new object[] { "@locationID", locationID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }

        public DataTable ReturnLocationDropDown()
        {
            string sqlCmd = "SELECT locationID, city FROM tbl_location WHERE isRetailStore = 1";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public DataTable ReturnLocationDropDownAll()
        {
            string sqlCmd = "SELECT locationID, locationName FROM tbl_location";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public DataTable ReturnProvinceDropDown(int countryID)
        {
            string sqlCmd = "SELECT provStateID, provName FROM tbl_provSTate WHERE countryID = @countryID";
            object[][] parms = { new object[] { "@countryID", countryID} };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public DataTable ReturnCountryDropDown()
        {
            string sqlCmd = "SELECT countryID, countryDesc FROM tbl_country";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //Province/State ID based on Province/State name
        //public int pronvinceID(string provName)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select provStateID from tbl_provState where provName = '" + provName + "'";

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    int provStateID = 0;
        //    while (reader.Read())
        //    {
        //        int n = Convert.ToInt32(reader["provStateID"]);
        //        provStateID = n;
        //    }
        //    conn.Close();
        //    //Returns province/state ID
        //    return provStateID;
        //}
        ////Country name based on country ID
        //public string countryName(int countryID)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select countryDesc from tbl_country where countryID = " + countryID;

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    string countryName = null;
        //    while (reader.Read())
        //    {
        //        string name = reader["countryDesc"].ToString();
        //        countryName = name;
        //    }
        //    conn.Close();
        //    //Returns country name
        //    return countryName;
        //}
        ////Country ID based on country name
        //public int countryID(string countryName)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select countryID from tbl_country where countryDesc = '" + countryName + "'";

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    int countryID = 0;
        //    while (reader.Read())
        //    {
        //        int n = Convert.ToInt32(reader["countryID"]);
        //        countryID = n;
        //    }
        //    conn.Close();
        //    //Returns country ID
        //    return countryID;
        //}
        ////CountryID based on provinceID
        //public int countryIDFromProvince(int provID)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select countryID from tbl_provState where provStateID = " + provID;

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    int countryID = 0;
        //    while (reader.Read())
        //    {
        //        int n = Convert.ToInt32(reader["countryID"]);
        //        countryID = n;
        //    }
        //    conn.Close();
        //    //Returns country ID
        //    return countryID;
        //}
        private string connectionString;
        public LocationManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        ////Location name based on location ID
        public string locationName(int locationID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select locationName from tbl_location where locationID = " + locationID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string locationN = null;
            while (reader.Read())
            {
                string name = reader["locationName"].ToString();
                locationN = name;
            }
            conn.Close();
            //Returns location name
            return locationN;
        }
        ////Location ID based on location name
        //public int locationID(string locationName)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = $"Select locationID from tbl_location where locationName like '{locationName}'";

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    int locID = 1;
        //    while (reader.Read())
        //    {
        //        int n = Convert.ToInt32(reader["locationID"]);
        //        locID = n;
        //    }
        //    conn.Close();
        //    //Returns location ID
        //    return locID;
        //}
        ////Location ID based on City
        //public int locationIDfromCity(string locationName)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select locationID from tbl_location where city = '" + locationName + "'";

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    int locID = 0;
        //    while (reader.Read())
        //    {
        //        int n = Convert.ToInt32(reader["locationID"]);
        //        locID = n;
        //    }
        //    conn.Close();
        //    //REturns location ID
        //    return locID;
        //}
        ////Get location for invoice based on City
        //public Location returnLocationForInvoice(string cityName)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select locationName, address, city, provStateID, postZip, PrimaryPhoneINT from tbl_location where city = '" + cityName + "'";

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    Location locationN = null;
        //    while (reader.Read())
        //    {
        //        locationN = new Location(reader["locationName"].ToString(), reader["address"].ToString(),
        //            reader["city"].ToString(), Convert.ToInt32(reader["provStateID"]),
        //            reader["postZip"].ToString(), reader["PrimaryPhoneINT"].ToString());
        //    }
        //    conn.Close();
        //    //Returns object location
        //    return locationN;
        //}
        //public Location returnLocationForReceiptFromID(int locationID)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select locationName, PrimaryPhoneINT, secondaryPhoneINT, email, address, "
        //        + "city, provStateID, country, postZip, secondaryIdentifier, taxNumber from tbl_location where locationID = @locationID";
        //    cmd.Parameters.AddWithValue("locationID", locationID);
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    Location locationN = null;
        //    while (reader.Read())
        //    {
        //        locationN = new Location(reader["locationName"].ToString(), reader["address"].ToString(),
        //            reader["city"].ToString(), Convert.ToInt32(reader["provStateID"]),
        //            reader["postZip"].ToString(), reader["PrimaryPhoneINT"].ToString());
        //    }
        //    conn.Close();
        //    //Returns object location
        //    return locationN;
        //}
        ////Gets city based on location id
        //public string locationCity(int locID)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select city from tbl_location where locationID  = "  + locID.ToString();
        //    string cityName = "";
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();            
        //    while (reader.Read())
        //    {

        //        cityName = reader["city"].ToString();
        //    }
        //    conn.Close();
        //    //Returns city name
        //    return cityName;
        //}
        ////G
        public int getProvIDFromLocationID(int locID)
        {
            string sqlCmd = "Select provStateID from tbl_location where locationID = @locID";
            object[][] parms =
            {
                new object[] { "@locID", locID }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        public int getCountryIDFromProvID(int provID)
        {
            string sqlCmd = "Select countryID from tbl_provState where provStateID = @provID";
            object[][] parms =
            {
                new object[] { "@provID", provID }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        ////Get location ID from "Destination"
        //public int getLocationIDFromDestination(string dest)
        //{
        //    int locID = 0;
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();

        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select locationID from tbl_location where secondaryIdentifier = @dest";
        //    cmd.Parameters.AddWithValue("dest", dest);

        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        locID = Convert.ToInt32(reader["locationID"]);
        //    }
        //    conn.Close();
        //    //REturns location ID
        //    return locID;
        //}
        ////returning the location ID from a Receipt Number
        //public int returnlocationIDFromReceiptNumber(int number)
        //{
        //    int locationID = 1;
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select locationID from tbl_receipt where receiptNumber  = @receiptNumber";
        //    cmd.Parameters.AddWithValue("receiptNumber", number);
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {

        //        locationID = Convert.ToInt32(reader["locationID"]);
        //    }
        //    conn.Close();
        //    return locationID;
        //}
        ////Gets all locations ID's
        //public List<Location> getAllLocations()
        //{
        //    List<Location> l = new List<Location>();
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select locationID, locationName from tbl_location";
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        l.Add(new Location(Convert.ToInt32(reader["locationID"]), reader["locationName"].ToString()));
        //    }
        //    conn.Close();
        //    return l;
        //}
        ////This method returns the location provStateID based on locationID
        //public int returnLocationID(int lID)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        //    int locID = 0;            
        //    DataTable table = new DataTable();
        //    SqlConnection con = new SqlConnection(connectionString);
        //    using (var cmd = new SqlCommand("getprovStateIDFromCity", con))
        //    using (var da = new SqlDataAdapter(cmd))
        //    {
        //        cmd.Parameters.AddWithValue("@cityName", lID);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da.Fill(table);
        //    }
        //    foreach (DataRow row in table.Rows)
        //    {
        //        locID = Convert.ToInt32(row["provStateID"]);
        //    }
        //    //Returns the provStateID
        //    return locID;
        //}

        //public DataTable returnProvinceDropDown(int countryID)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "SELECT provStateID, provName FROM tbl_provState WHERE countryID = @countryID ORDER BY provName";
        //    cmd.Parameters.AddWithValue("countryID", countryID);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    return dt;
        //}
    }
}