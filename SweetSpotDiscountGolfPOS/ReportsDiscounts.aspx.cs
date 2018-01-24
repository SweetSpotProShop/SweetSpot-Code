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
    public partial class ReportDiscounts : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();

        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        DateTime startDate;
        DateTime endDate;
        Employee e;
        Reports reports = new Reports();
        LocationManager l = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        double tDiscount;
        List<Invoice> discounts = new List<Invoice>();

        protected void Page_Load(object sender, EventArgs e)
        {

            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsDiscounts.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
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
                discounts = reports.returnDiscountsBetweenDates(startDate, endDate);
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
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = String.Format("{0:C}", tDiscount);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                string fileName = "Discount Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet discountsExport = xlPackage.Workbook.Worksheets.Add("Discounts");
                    // write to sheet   
                    discountsExport.Cells[1, 1].Value = lblReportDate.Text;
                    discountsExport.Cells[2, 1].Value = "Invoice Number";
                    discountsExport.Cells[2, 2].Value = "Invoice Date";
                    discountsExport.Cells[2, 3].Value = "Customer Name";
                    discountsExport.Cells[2, 4].Value = "Discount";
                    discountsExport.Cells[2, 5].Value = "Employee Name";
                    int recordIndex = 3;
                    List<Items> items = new List<Items>();
                    foreach (Invoice i in discounts)
                    {
                        discountsExport.Cells[recordIndex, 1].Value = i.invoiceNum + "-" + i.invoiceSub;
                        discountsExport.Cells[recordIndex, 2].Value = i.invoiceDate.ToString("d");
                        discountsExport.Cells[recordIndex, 3].Value = i.customerName;
                        discountsExport.Cells[recordIndex, 4].Value = "$" + i.discountAmount;
                        discountsExport.Cells[recordIndex, 5].Value = i.employeeName;
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
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}