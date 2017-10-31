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
    public partial class ReportsTaxes : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        Reports reports = new Reports();
        CurrentUser cu = new CurrentUser();
        List<TaxReport> tr = new List<TaxReport>();
        LocationManager l = new LocationManager();
        double colGST;
        double colPST;
        double retGST;
        double retPST;
        double ovrGST;
        double ovrPST;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsTaxes.aspx";
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
                Object[] passing = (Object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])passing[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];
                //Builds string to display in label
                lblTaxDate.Text = "Taxes Through: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(Convert.ToInt32(passing[1]));
                //Creating a cashout list and calling a method that grabs all mops and amounts paid
                tr = reports.returnTaxReportDetails(startDate, endDate);

                List<TaxReport> collected = new List<TaxReport>();
                List<TaxReport> returned = new List<TaxReport>();
                List<TaxReport> overall = new List<TaxReport>();
                foreach (var item in tr)
                {
                    if(item.locationID == Convert.ToInt32(passing[1]))
                    {
                        if(item.transactionType == 1)
                        {
                            collected.Add(item);
                        }
                        if(item.transactionType == 2)
                        {
                            returned.Add(item);
                        }
                        overall.Add(item);
                    }
                }

                grdTaxesCollected.DataSource = collected;
                grdTaxesCollected.DataBind();
                foreach (GridViewRow row in grdTaxesCollected.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }

                grdTaxesReturned.DataSource = returned;
                grdTaxesReturned.DataBind();
                foreach (GridViewRow row in grdTaxesReturned.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }

                grdTaxesOverall.DataSource = overall;
                grdTaxesOverall.DataBind();
                foreach (GridViewRow row in grdTaxesOverall.Rows)
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
        protected void grdTaxesCollected_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // check row type
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                colGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "govTax"));
                colPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provTax"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", colGST);
                e.Row.Cells[2].Text = String.Format("{0:C}", colPST);
            }
        }
        protected void grdTaxesReturned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // check row type
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                retGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "govTax"));
                retPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provTax"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", retGST);
                e.Row.Cells[2].Text = String.Format("{0:C}", retPST);
            }
        }
        protected void grdTaxesOverall_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // check row type
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ovrGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "govTax"));
                ovrPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provTax"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", ovrGST);
                e.Row.Cells[2].Text = String.Format("{0:C}", ovrPST);
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