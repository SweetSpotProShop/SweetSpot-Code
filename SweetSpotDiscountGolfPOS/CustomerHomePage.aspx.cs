using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        CustomerManager CM = new CustomerManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "CustomerHomePage";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnCustomerSearch_Click";
            try
            {
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtSearch.Text);
                grdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnAddNewCustomer_Click";
            try
            {
                //Opens the page to add a new customer
                Response.Redirect("CustomerAddNew.aspx?cust=-10", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdCustomersSearched_RowCommand";
            try
            {
                if (e.CommandName == "ViewProfile")
                {
                    //open Add New Customer page
                    Response.Redirect("CustomerAddNew.aspx?cust=" + e.CommandArgument.ToString(), false);
                }
                else if (e.CommandName == "StartSale")
                {
                    //Open the Sales Cart page
                    Response.Redirect("SalesCart.aspx?cust=" + e.CommandArgument.ToString(), false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdCustomersSearched_PageIndexChanging";
            try
            {
                grdCustomersSearched.PageIndex = e.NewPageIndex;
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtSearch.Text);
                grdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                                + "If you continue to receive this message please contact "
                                + "your system administrator", this);
            }
        }
    }
}