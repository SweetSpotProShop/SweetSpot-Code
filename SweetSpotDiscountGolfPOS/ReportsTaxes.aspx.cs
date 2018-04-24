using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsTaxes : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();



        List<TaxReport> tr = new List<TaxReport>();
        LocationManager l = new LocationManager();
        double colGST;
        double colPST;
        double retGST;
        double retPST;
        double ovrGST;
        double ovrPST;
        List<TaxReport> collected = new List<TaxReport>();
        List<TaxReport> returned = new List<TaxReport>();
        List<TaxReport> overall = new List<TaxReport>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsTaxes.aspx";
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    //Gathering the start and end dates
                    Object[] passing = (Object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    //Builds string to display in label
                    lblTaxDate.Text = "Taxes Through: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(Convert.ToInt32(passing[1]));
                    //Creating a cashout list and calling a method that grabs all mops and amounts paid
                    tr = R.returnTaxReportDetails(startDate, endDate);

                    foreach (var item in tr)
                    {
                        if (item.locationID == Convert.ToInt32(passing[1]))
                        {
                            if (item.transactionType == 1)
                            {
                                collected.Add(item);
                            }
                            if (item.transactionType == 2)
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
        protected void grdTaxesCollected_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdTaxesCollected_RowDataBound";
            try
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
        protected void grdTaxesReturned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdTaxesReturned_RowDataBound";
            try
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
        protected void grdTaxesOverall_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdTaxesOverall_RowDataBound";
            try
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
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                Object[] passing = (Object[])Session["reportInfo"];
                string loc = l.locationName(Convert.ToInt32(passing[1]));
                string fileName = "Taxes Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet salesTax = xlPackage.Workbook.Worksheets.Add("Sales");
                    ExcelWorksheet returnsTax = xlPackage.Workbook.Worksheets.Add("Returns");
                    ExcelWorksheet allTax = xlPackage.Workbook.Worksheets.Add("All Transactions");
                    //Writing       
                    salesTax.Cells[1, 1].Value = lblTaxDate.Text; returnsTax.Cells[1, 1].Value = lblTaxDate.Text; allTax.Cells[1, 1].Value = lblTaxDate.Text;
                    salesTax.Cells[2, 1].Value = "Sales"; returnsTax.Cells[2, 1].Value = "Returns"; allTax.Cells[2, 1].Value = "All Transactions";
                    salesTax.Cells[3, 1].Value = "Date"; salesTax.Cells[3, 2].Value = "GST"; salesTax.Cells[3, 3].Value = "PST";
                    returnsTax.Cells[3, 1].Value = "Date"; returnsTax.Cells[3, 2].Value = "GST"; returnsTax.Cells[3, 3].Value = "PST";
                    allTax.Cells[3, 1].Value = "Date"; allTax.Cells[3, 2].Value = "GST"; allTax.Cells[3, 3].Value = "PST";
                    int recordIndexSales = 4;
                    if (collected.Count > 0)
                    {
                        foreach (TaxReport trCollected in collected)
                        {
                            salesTax.Cells[recordIndexSales, 1].Value = trCollected.dtmInvoiceDate.ToString("d");
                            salesTax.Cells[recordIndexSales, 2].Value = trCollected.govTax;
                            salesTax.Cells[recordIndexSales, 3].Value = trCollected.provTax;
                            recordIndexSales++;
                        }
                    }
                    int recordIndexReturns = 4;
                    if (returned.Count > 0)
                    {
                        foreach (TaxReport trReturned in returned)
                        {
                            returnsTax.Cells[recordIndexReturns, 1].Value = trReturned.dtmInvoiceDate.ToString("d");
                            returnsTax.Cells[recordIndexReturns, 2].Value = trReturned.govTax;
                            returnsTax.Cells[recordIndexReturns, 3].Value = trReturned.provTax;
                            recordIndexReturns++;
                        }
                    }
                    int recordIndexOverall = 4;
                    if (overall.Count > 0)
                    {
                        foreach (TaxReport trOverall in overall)
                        {
                            allTax.Cells[recordIndexOverall, 1].Value = trOverall.dtmInvoiceDate.ToString("d");
                            allTax.Cells[recordIndexOverall, 2].Value = trOverall.govTax;
                            allTax.Cells[recordIndexOverall, 3].Value = trOverall.provTax;
                            recordIndexOverall++;
                        }
                    }
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
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
    }
}