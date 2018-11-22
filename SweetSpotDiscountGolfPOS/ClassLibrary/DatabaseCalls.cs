using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SweetSpotDiscountGolfPOS
{
    public class DatabaseCalls
    {
        readonly string connectionString;

        public DatabaseCalls()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        public DataTable returnDataTableData(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string strParameters = "";
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                strParameters += parms[times][0].ToString() + ", " + parms[times][1].ToString() + " - ";
                times++;
            }
            QueryStringCapture(sqlCmd, strParameters, objPageDetails, strQueryName);
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            //Returns a datatable
            return dt;
        }
        //public System.Data.DataTable returnDataTableData(string sqlCmd, object[] objPageDetails)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    SqlConnection con = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand
        //    {
        //        CommandText = sqlCmd,
        //        Connection = con
        //    };
        //    string strParameters = "";
        //    QueryStringCapture(sqlCmd, strParameters, objPageDetails);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    //Returns a datatable
        //    return dt;
        //}
        public string MakeDataBaseCallToReturnString(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                return (returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int MakeDataBaseCallToReturnInt(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                return Convert.ToInt32((returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]).ToString());
            }
            catch (Exception)
            {
                return -10;
            }
        }
        public double MakeDataBaseCallToReturnDouble(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                return Convert.ToDouble((returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]).ToString());
            }
            catch (Exception)
            {
                return -10;
            }
        }
        public void executeInsertQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string strParameters = "";
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                strParameters += parms[times][0].ToString() + ", " + parms[times][1].ToString() + " - ";
                times++;
            }
            QueryStringCapture(sqlCmd, strParameters, objPageDetails, strQueryName);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //search has a 0 and 1
        //parms has 0,1,2,3 and 4,5,6,7
        public DataTable returnDataTableDataFromArrayLists(string sqlCmd, ArrayList parms, ArrayList search, object[] objPageDetails, string strQueryName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string strParameters = "";
            cmd.CommandText = sqlCmd;
            int searchTimes = 0;
            int parmsTimes = 0;
            while (search.Count > searchTimes)
            {
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), search[searchTimes]);
                strParameters += parms[parmsTimes].ToString() + ", " + search[searchTimes].ToString() + " - ";
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), search[searchTimes]);
                strParameters += parms[parmsTimes].ToString() + ", " + search[searchTimes].ToString() + " - ";
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), search[searchTimes]);
                strParameters += parms[parmsTimes].ToString() + ", " + search[searchTimes].ToString() + " - ";
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), search[searchTimes]);
                strParameters += parms[parmsTimes].ToString() + ", " + search[searchTimes].ToString() + " - ";
                parmsTimes++;
                searchTimes++;
            }
            cmd.Connection = con;
            QueryStringCapture(sqlCmd, strParameters, objPageDetails, strQueryName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            //Returns a datatable
            return dt;
        }

        private void QueryStringCapture(string strSQLCommand, string strParameters, object[] objPageDetails, string strQueryName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO tbl_QueryStringCapture VALUES(@dtmCaptureDate, "
                + "@dtmCaptureTime, @strPageName, @strMethodName, @strQueryName,  "
                + "@strSQLCommand, @strParameters)";

            cmd.Parameters.AddWithValue("@dtmCaptureDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@dtmCaptureTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@strPageName", objPageDetails[0].ToString());
            cmd.Parameters.AddWithValue("@strMethodName", objPageDetails[1].ToString());
            cmd.Parameters.AddWithValue("@strQueryName", strQueryName);
            cmd.Parameters.AddWithValue("@strSQLCommand", strSQLCommand);
            cmd.Parameters.AddWithValue("@strParameters", strParameters);

            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void executeErrorInsertQuery(string sqlCmd, object[][] parms)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                times++;
            }
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}