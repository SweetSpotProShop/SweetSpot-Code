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
    public partial class ReportsPurchasesMade : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        Reports reports = new Reports();
        CurrentUser cu = new CurrentUser();
        double totalPurchAmount = 0;
        int totalPurchases = 0;
        int totalCheques = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsPurchasesMade.aspx";
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
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];
                //Builds string to display in label
                lblPurchasesMadeDate.Text = "Purchases Made Between: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + cu.locationName;
                int locationID = cu.locationID;
                //Creating a cashout list and calling a method that grabs all mops and amounts paid
                List<Purchases> purch = reports.returnPurchasesDuringDates(startDate, endDate, locationID);
                grdPurchasesMade.DataSource = purch;
                grdPurchasesMade.DataBind();
                foreach (GridViewRow row in grdPurchasesMade.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
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
        protected void grdPurchasesMade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // check row type
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "chequeNumber")) > 0)
                {
                    totalCheques += 1;
                }
                totalPurchases += 1;
                totalPurchAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amountPaid"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = totalPurchases.ToString();
                e.Row.Cells[3].Text = totalCheques.ToString();
                e.Row.Cells[4].Text = String.Format("{0:c}", totalPurchAmount);
            }
        }
        protected void printReport(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "printReport";
            //Current method does nothing
            try
            { }
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