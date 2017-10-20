using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using SweetShop;

namespace SweetSpotDiscountGolfPOS
{
    public partial class LoginPage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sets focus on the pasword text box
            txtPassword.Focus();
        }
        //test
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Connectes to the database and returns the employee id based on the password used
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
            //SqlCommand cmd = new SqlCommand("SELECT empID, password FROM tbl_userInfo WHERE password = @password", con);
            //New Select Statement Attempt
            SqlCommand cmd = new SqlCommand("SELECT tbl_employee.empID, tbl_employee.jobID, "
                + "tbl_employee.locationID, tbl_location.city, tbl_userInfo.password "
                + "FROM tbl_employee join tbl_location on tbl_employee.locationID = "
                + "tbl_location.locationID join tbl_userInfo on tbl_employee.empID = "
                + "tbl_userInfo.empID where tbl_userInfo.password = @password", con);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            //Stores data into data table
            sda.Fill(dt);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            ////checking for admin and sales staff
            //SqlCommand adm = new SqlCommand("SELECT tbl_userInfo.password,tbl_employee.jobID,tbl_employee.empID FROM tbl_userInfo JOIN tbl_employee ON tbl_employee.empID = tbl_userInfo.empID AND tbl_userInfo.password = @pwd", con);
            //adm.Parameters.AddWithValue("@pwd", Convert.ToInt32(txtPassword.Text));
            //SqlDataAdapter admA = new SqlDataAdapter(adm);
            //DataTable dt1 = new DataTable();
            ////stores data into data table 1
            //admA.Fill(dt1);
            //con.Open();
            //int job = adm.ExecuteNonQuery();
            //con.Close();

            //if (dt1.Rows.Count > 0)
            //{
            //    //verifies that data was returned in data table 1
            //    DataRow admDr = dt1.Rows[0];
            //    if (Convert.ToInt32(admDr.ItemArray[1]) == 0)
            //    {
            //        //If role returned = 0 then user is an admin
            //        Session["Admin"] = "Admin";
            //    }
            //    //else
            //    //{
            //    //Checks the location of the employee
            //    SqlCommand loc = new SqlCommand("SELECT  tbl_location.locationID,tbl_location.city FROM tbl_location JOIN tbl_employee ON tbl_location.locationID = tbl_employee.locationID AND tbl_employee.empID = @emp", con);
            //    loc.Parameters.AddWithValue("@emp", admDr.ItemArray[2]);
            //    SqlDataAdapter locA = new SqlDataAdapter(loc);
            //    DataTable dt2 = new DataTable();
            //    //stores results into data table 2
            //    locA.Fill(dt2);
            //    con.Open();
            //    loc.ExecuteNonQuery();
            //    con.Close();
            //    int cnt = dt2.Rows.Count;
            //    //Stores location info into Sessions
            //    DataRow locDR = dt2.Rows[0];
            //    Session["Loc"] = locDR.ItemArray[1];
            //    Session["locationID"] = locDR.ItemArray[0];
            //    //}
            //}
            if (dt.Rows.Count > 0)
            {
                //if data table contains rows then password is valid, store info into sessions
                //Session["loggedIn"] = true;
                //Session["id"] = txtPassword.Text;
                //Session["loginEmployeeID"] = dt.Rows[0].ItemArray[0];
                //Session["prevPage"] = "LoginPage.aspx";
                //Changes to the home page
                Session["currentUser"] = new CurrentUser(Convert.ToInt32(dt.Rows[0].ItemArray[0]),
                    Convert.ToInt32(dt.Rows[0].ItemArray[1]), Convert.ToInt32(dt.Rows[0].ItemArray[2]), 
                    Convert.ToString(dt.Rows[0].ItemArray[3]), Convert.ToInt32(dt.Rows[0].ItemArray[4]));
                Server.Transfer("HomePage.aspx", false);
                Session.RemoveAll();
            }
            else
            {
                //password was incorrect, nothing occurs
                lblError.Text = "Your password is incorrect";
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}