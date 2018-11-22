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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesHomePage.aspx";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    if (!IsPostBack) { calSearchDate.SelectedDate = DateTime.Today; }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Searches invoices and displays them 
        //By date or customer
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                grdInvoiceSelection.DataSource = IM.ReturnInvoicesBasedOnSearchForReturns(txtInvoiceSearch.Text, calSearchDate.SelectedDate, objPageDetails);
                grdInvoiceSelection.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoiceSelection_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Checks that the command name is return invoice
                if (e.CommandName == "returnInvoice")
                {
                    //Retrieves all the invoices that were searched
                    Invoice RI = IM.ReturnInvoice(Convert.ToString(e.CommandArgument), objPageDetails)[0];
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    string invoice = CU.locationName + "-" + RI.invoiceNum + "-" + IM.CalculateNextInvoiceSubNum(RI.invoiceNum, objPageDetails);
                    nameValues.Set("inv", invoice);
                    //Changes to Returns cart
                    Response.Redirect("ReturnsCart.aspx?" + nameValues, false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void calSearchDate_SelectionChanged(object sender, EventArgs e)
        {
            string method = "calSearchDate_SelectionChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try { }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}