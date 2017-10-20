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
        ErrorReporting er = new ErrorReporting();
        CurrentUser cu;
        List<Customer> c;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "CustomerHomePage";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                SweetShopManager ssm = new SweetShopManager();
                 c = ssm.GetCustomerfromSearch(txtSearch.Text);
                //Binds the results to the gridview
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = c;
                grdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnAddNewCustomer_Click";
            try
            {
                //Opens the page to add a new customer
                Server.Transfer("CustomerAddNew.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdCustomersSearched_RowCommand";
            try
            {
                //grabs the command argument for the command pressed 
                string key = e.CommandArgument.ToString();
                if (e.CommandName == "ViewProfile")
                {
                    //if command argument is view profile 
                    Session["key"] = key;
                    //then set key and open Add New Customer page
                    Server.Transfer("CustomerAddNew.aspx", false);
                }
                else if (e.CommandName == "StartSale")
                {
                    //Set transaction type to sale
                    Session["TranType"] = 1;
                    //Set key to the customer number
                    Session["key"] = key;
                    //Open the Sales Cart page
                    Server.Transfer("SalesCart.aspx");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }

        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomersSearched.PageIndex = e.NewPageIndex;
            //Looks through database and returns a list of customers
            //based on the search criteria entered
            SweetShopManager ssm = new SweetShopManager();
            c = ssm.GetCustomerfromSearch(txtSearch.Text);
            //Binds the results to the gridview
            grdCustomersSearched.Visible = true;
            grdCustomersSearched.DataSource = c;
            grdCustomersSearched.DataBind();
        }
        
    }
}