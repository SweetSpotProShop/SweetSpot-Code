using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportDiscounts : System.Web.UI.Page
    {
        SweetShopManager ssm = new SweetShopManager();
        ErrorReporting er = new ErrorReporting();
        LocationManager lm = new LocationManager();
        DateTime startDate;
        DateTime endDate;
        Employee e;
        Reports reports = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CurrentUser cu = new CurrentUser();
        double tDiscount;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsDiscounts.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                //Gathering the start and end dates
                Object[] repInfo = (Object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])repInfo[0];
                startDate = reportDates[0];
                endDate = reportDates[1];
                int locID = Convert.ToInt32(repInfo[1]);
                //Builds string to display in label
                if (startDate == endDate)
                {
                    lblReportDate.Text = "Discount Report on: " + startDate.ToString("d") + " for " + lm.locationName(locID);
                }
                else
                {
                    lblReportDate.Text = "Discount Report on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + lm.locationName(locID);
                }
                List<Invoice> discounts = reports.returnDiscountsBetweenDates(startDate, endDate);
                if (discounts.Count == 0)
                {
                    if (startDate == endDate)
                    {
                        lblReportDate.Text = "There are no invoices with discounts on: " + startDate.ToString("d");
                    }
                    else
                    {
                        lblReportDate.Text = "There are no invoices with discounts betweeen: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                    }
                }
                else
                {
                    grdInvoiceDisplay.DataSource = discounts;
                    grdInvoiceDisplay.DataBind();
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
        protected void grdInvoiceDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdInvoiceDisplay_RowDataBound";
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    tDiscount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discountAmount"));
                }
                else if(e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = String.Format("{0:C}", tDiscount);
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
    }
}