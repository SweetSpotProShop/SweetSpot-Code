using System;
using SweetShop;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReturnsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        InvoiceManager IM = new InvoiceManager();

        SweetShopManager ssm = new SweetShopManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesHomePage.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (!IsPostBack) { calSearchDate.SelectedDate = DateTime.Today; }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Searches invoices and displays them 
        //By date or customer
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSearch_Click";
            try
            {
                grdInvoiceSelection.DataSource = IM.ReturnInvoicesBasedOnSearchForReturns(txtInvoiceSearch.Text, calSearchDate.SelectedDate);
                grdInvoiceSelection.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoiceSelection_RowCommand";
            try
            {
                //Checks that the command name is return invoice
                if (e.CommandName == "returnInvoice")
                {
                    //Retrieves all the invoices that were searched
                    List<Invoice> returnInvoice = IM.ReturnInvoice(Convert.ToString(e.CommandArgument));
                    //Sets the Customer key id
                    Session["key"] = returnInvoice[0].customer.customerId;
                    //Sets the session to the single invoice
                    Session["searchReturnInvoices"] = returnInvoice[0];
                    //Sets transaction type to return
                    Session["TranType"] = 2;
                    //Changes to Returns cart
                    Response.Redirect("ReturnsCart.aspx", false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }

        protected void calSearchDate_SelectionChanged(object sender, EventArgs e)
        {
            string method = "calSearchDate_SelectionChanged";
            try { }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}