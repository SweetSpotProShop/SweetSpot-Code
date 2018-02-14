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
        ErrorReporting ER = new ErrorReporting();
        EmployeeManager EM = new EmployeeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sets focus on the pasword text box
            txtPasswordEntry.Focus();
        }
        //test
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Connectes to the database and returns the employee id based on the password used
            List<CurrentUser> cu = EM.ReturnCurrentUserFromPassword(txtPasswordEntry.Text);
            if (cu.Count > 0)
            {
                Session["currentUser"] = cu[0];
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