using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesHomePage : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly InvoiceManager IM = new InvoiceManager();
        //LocationManager LM = new LocationManager();
        CurrentUser CU;

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
                    if (!IsPostBack)
                    {
                        CalSearchDate.SelectedDate = DateTime.Today;
                        //Binds invoice list to the grid view
                        GrdCurrentOpenSales.DataSource = IM.CallReturnCurrentOpenInvoices(CU.location.intLocationID, CU.location.intProvinceID, objPageDetails);
                        GrdCurrentOpenSales.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void BtnQuickSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnQuickSale_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", "1");
                string invoice = "-10";
                nameValues.Set("invoice", invoice);
                //Changes page to Sales Cart
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnReturns_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturns_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnInvoiceSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInvoiceSearch_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Response.Redirect("InvoiceSearch.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnProcessCashOut_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProcessCashOut_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Reports R = new Reports();
                int indicator = R.VerifyCashoutCanBeProcessed(CU.location.intLocationID, CalSearchDate.SelectedDate, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    R.RemoveUnprocessedReturns(CU.location.intLocationID, CalSearchDate.SelectedDate, objPageDetails);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("selectedDate", CalSearchDate.SelectedDate.ToShortDateString());
                    nameValues.Set("location", CU.location.intLocationID.ToString());
                    //Changes to the Reports Cash Out page
                    Response.Redirect("SalesCashOut.aspx?" + nameValues, false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No transactions have been processed for selected date.", this);
                }
                else if (indicator == 2)
                {
                    MessageBoxCustom.ShowMessage("There are still open transactions that need to be processed or cancelled.", this);
                }
                else if (indicator == 3)
                {
                    MessageBoxCustom.ShowMessage("A cashout has already been completed for selected date.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }        
        protected void GrdCurrentOpenSales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdCurrentOpenSales_RowCommand";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                //Still need to get the cust on the Invoice
                int index = ((GridViewRow)(((LinkButton)e.CommandSource).NamingContainer)).RowIndex;
                nameValues.Set("customer", ((Label)GrdCurrentOpenSales.Rows[index].Cells[11].FindControl("lblCustID")).Text);
                int invoiceID = Convert.ToInt32(e.CommandArgument);
                nameValues.Set("invoice", invoiceID.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                //Changes page to Sales Cart
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void CalSearchDate_SelectionChanged(object sender, EventArgs e)
        {
            string method = "CalSearchDate_SelectionChanged";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try { }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}