using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //This class is for getting the employee ID from the userinfo
    public class tbl_userinfomanager
    {
        private string connectionstring;
        //Connection String
        public tbl_userinfomanager()
        {
            connectionstring = "SweetSpotSBConnectionString";
        }

        //This method gets the employee ID from a password
        public int getuserinfologin(int password)
        {
            //Variable to store the employee ID
            int empID = 0;
            //Creating a table to store the results
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionstring);
            using (var cmd = new SqlCommand("getEmloyeeIDFromPassword", con)) //Calling the SP   
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@password", password);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            //Looping through the table and creating employees from the rows
            foreach (DataRow row in table.Rows)
            {
                empID = Convert.ToInt32(row["empID"]);
            }
            //Returns employee ID
            return empID;
        }

    }
}
