using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS
{
    public class DatabaseCalls
    {
        string connectionString;

        public DatabaseCalls()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        public System.Data.DataTable returnDataTableData(string sqlCmd, object[][] parms)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
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

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            //Returns a datatable
            return dt;
        }
        public System.Data.DataTable returnDataTableData(string sqlCmd)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            //Returns a datatable
            return dt;
        }
        public string MakeDataBaseCallToReturnString(string sqlCmd, object[][] parms)
        {
            try
            {
                return (returnDataTableData(sqlCmd, parms).Rows[0][0]).ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int MakeDataBaseCallToReturnInt(string sqlCmd, object[][] parms)
        {
            try
            {
                return Convert.ToInt32((returnDataTableData(sqlCmd, parms).Rows[0][0]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public double MakeDataBaseCallToReturnDouble(string sqlCmd, object[][] parms)
        {
            try
            {
                return Convert.ToDouble((returnDataTableData(sqlCmd, parms).Rows[0][0]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public void executeInsertQuery(string sqlCmd, object[][] parms)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
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