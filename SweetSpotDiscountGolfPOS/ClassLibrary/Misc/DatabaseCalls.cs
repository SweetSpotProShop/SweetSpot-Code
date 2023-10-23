using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SweetSpotDiscountGolfPOS.Misc
{
    public class DatabaseCalls
    {
        public DatabaseCalls()
        { }
        //These do not track query strings        
        public string MakeDataBaseCallToReturnString(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                return (ReturnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]).ToString();
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
                return Convert.ToInt32((ReturnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public double MakeDataBaseCallToReturnDouble(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            try
            {
                return Convert.ToDouble((ReturnDataTableData(sqlCmd, parms, objPageDetails, strQueryName).Rows[0][0]).ToString());
            }
            catch (Exception)
            {
                return -10;
            }
        }
        public void MakeDataBaseCallToNonReturnDataQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void MakeDataBaseCallToNonReturnErrorQuery(string sqlCmd, object[][] parms)
        {
            ExecuteErrorInsertQuery(sqlCmd, parms);
        }
        public DataTable MakeDataBaseCallToReturnDataTable(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            return ReturnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable MakeDataBaseCallToReturnDataTableFromArrayLists(string sqlCmd, ArrayList parms, ArrayList search, object[] objPageDetails, string strQueryName)
        {
            return ReturnDataTableDataFromArrayLists(sqlCmd, parms, search, objPageDetails, strQueryName);
        }
        public DataTable MakeDataBaseCallToReturnDataTableFromStoredProcedure(string procedureName, object[][] parms)
        {
            return ReturnDataTableFromStoredProcedure(procedureName, parms);
        }


        private DataTable ReturnDataTableData(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            string strParameters = "";
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                strParameters += parms[times][0].ToString() + ", " + parms[times][1].ToString() + " - ";
                times++;
            }
            //QueryStringCapture(sqlCmd, strParameters, objPageDetails, strQueryName);
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            //Returns a datatable
            return dt;
        }
        private DataTable ReturnDataTableDataFromArrayLists(string sqlCmd, ArrayList parms, ArrayList search, object[] objPageDetails, string strQueryName)
        {
            string strParameters = "";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
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
            //QueryStringCapture(sqlCmd, strParameters, objPageDetails, strQueryName);
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 300;
            da.Fill(dt);
            //Returns a datatable
            return dt;
        }
        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            string strParameters = "";
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                strParameters += parms[times][0].ToString() + ", " + parms[times][1].ToString() + " - ";
                times++;
            }
            //QueryStringCapture(sqlCmd, strParameters, objPageDetails, strQueryName);
            cmd.Connection = con;
            cmd.CommandTimeout = 300;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void ExecuteErrorInsertQuery(string sqlCmd, object[][] parms)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);             
                times++;
            }
            cmd.Connection = con;
            cmd.CommandTimeout = 300;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void QueryStringCapture(string strSQLCommand, string strParameters, object[] objPageDetails, string strQueryName)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
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
            cmd.CommandTimeout = 300;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }        
        private DataTable ReturnDataTableFromStoredProcedure(string procedureName, object[][] parms)
        {
            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            DataTable dt = new DataTable();
            using (var cmd = new SqlCommand(procedureName, sqlCon))
            using (var da = new SqlDataAdapter(cmd))
            {            
                cmd.CommandType = CommandType.StoredProcedure;
                int times = 0;
                while (parms.Count() > times)
                {
                    cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                    times++;
                }
                da.SelectCommand.CommandTimeout = 300;
                da.Fill(dt);
            }
            return dt;
        }
    }
}