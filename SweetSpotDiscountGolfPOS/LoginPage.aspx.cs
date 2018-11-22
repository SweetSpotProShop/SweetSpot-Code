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
        EmployeeManager EM = new EmployeeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["currPage"] = "LoginPage.aspx";
            //Sets focus on the pasword text box
            txtPasswordEntry.Focus();
        }
        //test
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string method = "btnLogin_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            //Connectes to the database and returns the employee id based on the password used
            List<CurrentUser> CU = EM.ReturnCurrentUserFromPassword(txtPasswordEntry.Text, objPageDetails);
            if (CU.Count > 0)
            {
                Session["currentUser"] = CU[0];
                Response.Redirect("HomePage.aspx", false);
            }
            else
            {
                //password was incorrect, nothing occurs
                lblError.Text = "Your password is incorrect";
            }
        }
    }
}