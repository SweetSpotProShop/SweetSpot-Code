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
    public partial class SalesHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        InvoiceManager IM = new InvoiceManager();
        LocationManager LM = new LocationManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesHomePage.aspx";
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
                    if (!IsPostBack)
                    {
                        //Binds invoice list to the grid view
                        grdCurrentOpenSales.DataSource = IM.ReturnCurrentOpenInvoices(CU.locationID);
                        grdCurrentOpenSales.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void btnQuickSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnQuickSale_Click";
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", "1");
                string invoice = CU.locationName + "-" + IM.ReturnNextInvoiceNumber() + "-1";
                nameValues.Set("inv", invoice);
                //Changes page to Sales Cart
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnReturns_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturns_Click";
            try
            {
                //Changes page to Returns Home Page
                Response.Redirect("ReturnsHomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnInvoiceSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInvoiceSearch_Click";
            try
            {
                Response.Redirect("InvoiceSearch.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnProcessCashOut_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProcessCashOut_Click";
            try
            {
                Reports R = new Reports();
                int indicator = R.verifyCashoutCanBeProcessed(CU.locationID);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("dtm", DateTime.Today.ToShortDateString());
                    nameValues.Set("location", CU.locationID.ToString());
                    //Changes to the Reports Cash Out page
                    Response.Redirect("SalesCashOut.aspx?" + nameValues, false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No transactions have been processed for selected date.", this);
                }
                else if (indicator == 2)
                {
                    MessageBox.ShowMessage("There are still open transactions that need to be processed or cancelled.", this);
                }
                else if (indicator == 3)
                {
                    MessageBox.ShowMessage("A cashout has already been completed for selected date.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Still Needs to be Updated
        protected void grdCurrentOpenSales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdCurrentOpenSales_RowCommand";
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                //Still need to get the cust on the Invoice
                int index = ((GridViewRow)(((LinkButton)e.CommandSource).NamingContainer)).RowIndex;
                nameValues.Set("cust", ((Label)grdCurrentOpenSales.Rows[index].Cells[11].FindControl("lblCustID")).Text);
                string strInvoice = Convert.ToString(e.CommandArgument);
                nameValues.Set("inv", CU.locationName + "-" + strInvoice);
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                //Changes page to Sales Cart
                Response.Redirect("SalesCart.aspx?" + nameValues, false);

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}