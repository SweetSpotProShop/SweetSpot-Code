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
        DateTime startDate;
        DateTime endDate;
        Employee e;
        Reports reports = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CurrentUser cu = new CurrentUser();

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
                DateTime[] reportDates = (DateTime[])Session["reportDates"];
                startDate = reportDates[0];
                endDate = reportDates[1];
                //Builds string to display in label
                if (startDate == endDate)
                {
                    lblReportDate.Text = "Discount report for: " + startDate.ToString("d");
                }
                else
                {
                    lblReportDate.Text = "Discount report for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                }              


                List<Invoice> discounts = reports.returnDiscountsBetweenDates(startDate, endDate);
                if (discounts.Count == 0)
                {
                    lblInvoices.Visible = false;
                    lblTotalDiscounts.Visible = false;
                    lblTotals.Visible = false;
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
                    string totalDiscounts = reports.returnDiscountTotalBetweenDates(startDate, endDate).ToString();
                    lblTotalDiscounts.Text = "Total Discount: $" + totalDiscounts;
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



    }
}