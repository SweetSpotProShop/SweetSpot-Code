using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //Used to gather information about locations
    public class LocationManager
    {
        string connectionString;
        //Connection string
        public LocationManager()
        {
           connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //Provinve/State Name based on Province/State ID
        public string provinceName(int provID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select provName from tbl_provState where provStateID = " + provID;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string provStateName = null;
            while (reader.Read())
            {
                string name = reader["provName"].ToString();
                provStateName = name;
            }
            conn.Close();
            //Returns province/state name
            return provStateName;
        }
        //Province/State ID based on Province/State name
        public int pronvinceID(string provName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select provStateID from tbl_provState where provName = '" + provName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int provStateID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["provStateID"]);
                provStateID = n;
            }
            conn.Close();
            //Returns province/state ID
            return provStateID;
        }
        //Country name based on country ID
        public string countryName(int countryID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select countryDesc from tbl_country where countryID = " + countryID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string countryName = null;
            while (reader.Read())
            {
                string name = reader["countryDesc"].ToString();
                countryName = name;
            }
            conn.Close();
            //Returns country name
            return countryName;
        }
        //Country ID based on country name
        public int countryID(string countryName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select countryID from tbl_country where countryDesc = '" + countryName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int countryID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["countryID"]);
                countryID = n;
            }
            conn.Close();
            //Returns country ID
            return countryID;
        }
        //CountryID based on provinceID
        public int countryIDFromProvince(int provID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select countryID from tbl_provState where provStateID = " + provID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int countryID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["countryID"]);
                countryID = n;
            }
            conn.Close();
            //Returns country ID
            return countryID;
        }
        //Location name based on location ID
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
        //Location ID based on location name
        public int locationID(string locationName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = $"Select locationID from tbl_location where locationName like '{locationName}'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int locID = 1;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["locationID"]);
                locID = n;
            }
            conn.Close();
            //Returns location ID
            return locID;
        }
        //Location ID based on City
        public int locationIDfromCity(string locationName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select locationID from tbl_location where city = '" + locationName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int locID = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["locationID"]);
                locID = n;
            }
            conn.Close();
            //REturns location ID
            return locID;
        }
        //Get location for invoice based on City
        public Location returnLocationForInvoice(string cityName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select locationName, address, city, provStateID, postZip, PrimaryPhoneINT from tbl_location where city = '" + cityName + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Location locationN = null;
            while (reader.Read())
            {
                locationN = new Location(reader["locationName"].ToString(), reader["address"].ToString(),
                    reader["city"].ToString(), Convert.ToInt32(reader["provStateID"]),
                    reader["postZip"].ToString(), reader["PrimaryPhoneINT"].ToString());
            }
            conn.Close();
            //Returns object location
            return locationN;
        }
        //Gets city based on location id
        public string locationCity(int locID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select city from tbl_location where locationID  = "  + locID.ToString();
            string cityName = "";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();            
            while (reader.Read())
            {

                cityName = reader["city"].ToString();
            }
            conn.Close();
            //Returns city name
            return cityName;
        }
        //G
        public int getProvIDFromLocationID(int locID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select provStateID from tbl_location where locationID = @locID";
            cmd.Parameters.AddWithValue("locID", locID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int prov = 0;
            while (reader.Read())
            {
                int n = Convert.ToInt32(reader["provStateID"]);
                prov = n;
            }
            conn.Close();
            //REturns location ID
            return prov;
        }
        //Get location ID from "Destination"
        public int getLocationIDFromDestination(string dest)
        {
            int locID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select locationID from tbl_location where secondaryIdentifier = @dest";
            cmd.Parameters.AddWithValue("dest", dest);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                locID = Convert.ToInt32(reader["locationID"]);
            }
            conn.Close();
            //REturns location ID
            return locID;
        }

    }
}