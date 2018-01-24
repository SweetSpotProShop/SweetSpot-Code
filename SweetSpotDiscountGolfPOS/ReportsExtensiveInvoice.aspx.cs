using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsExtensiveInvoice : System.Web.UI.Page
    {
        SweetShopManager ssm = new SweetShopManager();
        ErrorReporting er = new ErrorReporting();
        LocationManager lm = new LocationManager();
        DateTime startDate;
        DateTime endDate;
        Employee e;
        Reports reports = new Reports();
        LocationManager l = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        CurrentUser cu = new CurrentUser();

        DataTable invoices;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsExtensiveInvoice.aspx";
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
                    lblDates.Text = "Extensive Invoice Report on: " + startDate.ToString("d") + " for " + lm.locationName(locID);
                }
                else
                {
                    lblDates.Text = "Extensive Invoice Report on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + lm.locationName(locID);
                }
                invoices = new DataTable();
                invoices = reports.returnExtensiveInvoices(startDate, endDate, locID);

                grdInvoices.DataSource = invoices;
                grdInvoices.DataBind();
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

        protected void grdInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // check row type
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Shipping
                if (e.Row.Cells[1].Text.isNumber())
                {
                    e.Row.Cells[1].Text = "$" + e.Row.Cells[1].Text;
                }
                //Discount
                if (e.Row.Cells[2].Text.isNumber())
                {
                    e.Row.Cells[2].Text = "$" + e.Row.Cells[2].Text;
                }
                //Pre-Tax
                if(e.Row.Cells[3].Text.isNumber())
                {
                    e.Row.Cells[3].Text = "$" + e.Row.Cells[3].Text;
                }
                //Gov Tax
                if(e.Row.Cells[4].Text.isNumber())
                {
                    e.Row.Cells[4].Text = "$" + e.Row.Cells[4].Text;    
                }
                //Prov Tax
                if(e.Row.Cells[5].Text.isNumber())
                {
                    e.Row.Cells[5].Text = "$" + e.Row.Cells[5].Text;
                }
                //Post-Tax
                if (e.Row.Cells[6].Text.isNumber())
                {
                    e.Row.Cells[6].Text = "$" + e.Row.Cells[6].Text;
                }
                //COGS
                if (e.Row.Cells[7].Text.isNumber())
                {
                    e.Row.Cells[7].Text = "$" + e.Row.Cells[7].Text;
                }
                //Revenue
                if (e.Row.Cells[8].Text.isNumber())
                {
                    e.Row.Cells[8].Text = "$" + e.Row.Cells[8].Text;
                }
                //Profit Margin
                if (e.Row.Cells[9].Text.isNumber())
                {
                    e.Row.Cells[9].Text = e.Row.Cells[9].Text + "%";
                }
                //Removing the time from the date
                DateTime invoiceDate = Convert.ToDateTime(e.Row.Cells[13].Text);
                e.Row.Cells[13].Text = invoiceDate.ToString("dd-MM-yyyy");             
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                Object[] passing = (Object[])Session["reportInfo"];
                string loc = l.locationName(Convert.ToInt32(passing[1]));
                string fileName = "Extensive Invoice Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet invoicesExport = xlPackage.Workbook.Worksheets.Add("Invoices");
                    // write to sheet   
                    invoicesExport.Cells[1, 1].Value = lblDates.Text;
                    invoicesExport.Cells[2, 1].Value = "Invoice";
                    invoicesExport.Cells[2, 2].Value = "Shipping";
                    invoicesExport.Cells[2, 3].Value = "Total Discount";
                    invoicesExport.Cells[2, 4].Value = "Pre-Tax";
                    invoicesExport.Cells[2, 5].Value = "Government Tax";
                    invoicesExport.Cells[2, 6].Value = "Provincial Tax";
                    invoicesExport.Cells[2, 7].Value = "Post-Tax";
                    invoicesExport.Cells[2, 8].Value = "COGS";
                    invoicesExport.Cells[2, 9].Value = "Revenue Earned";
                    invoicesExport.Cells[2, 10].Value = "Profit Margin";
                    invoicesExport.Cells[2, 11].Value = "Customer";
                    invoicesExport.Cells[2, 12].Value = "Employee";
                    invoicesExport.Cells[2, 13].Value = "Location";
                    invoicesExport.Cells[2, 14].Value = "Date";
                    int recordIndex = 3;
                    foreach (DataRow row in invoices.Rows)
                    {

                        invoicesExport.Cells[recordIndex, 1].Value = row[0].ToString();
                        invoicesExport.Cells[recordIndex, 2].Value = row[1].ToString();
                        invoicesExport.Cells[recordIndex, 3].Value = row[2].ToString();
                        invoicesExport.Cells[recordIndex, 4].Value = row[3].ToString();
                        invoicesExport.Cells[recordIndex, 5].Value = row[4].ToString();
                        invoicesExport.Cells[recordIndex, 6].Value = row[5].ToString();
                        invoicesExport.Cells[recordIndex, 7].Value = row[6].ToString();
                        invoicesExport.Cells[recordIndex, 8].Value = row[7].ToString();
                        invoicesExport.Cells[recordIndex, 9].Value = row[8].ToString();
                        invoicesExport.Cells[recordIndex, 10].Value = row[9].ToString();
                        invoicesExport.Cells[recordIndex, 11].Value = row[10].ToString();
                        invoicesExport.Cells[recordIndex, 12].Value = row[11].ToString();
                        invoicesExport.Cells[recordIndex, 13].Value = row[12].ToString();
                        DateTime date = Convert.ToDateTime(row[13]);
                        invoicesExport.Cells[recordIndex, 14].Value = date.ToString("dd-MM-yyyy");
                        recordIndex++;
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