using System;
using System.Collections.Generic;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;

namespace SweetSpotDiscountGolfPOS
{
    public partial class LoginPage : System.Web.UI.Page
    {
        readonly EmployeeManager EM = new EmployeeManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["currPage"] = "LoginPage.aspx";
            //Sets focus on the pasword text box
            txtPasswordEntry.Focus();
        }
        //test
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string method = "BtnLogin_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            //Connectes to the database and returns the employee id based on the password used
            List<CurrentUser> CU = EM.CallReturnCurrentUserFromPassword(txtPasswordEntry.Text, objPageDetails);
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