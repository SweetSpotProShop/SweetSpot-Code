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
    public partial class ReportsStoreStats : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        Reports R = new Reports();
        CurrentUser CU;

        double gTax = 0;
        double pTax = 0;
        double cogs = 0;
        double salesPrT = 0;
        double salesPoT = 0;
        double pm = 0;
        double avp = 0;
        string timeFrame;
        DataTable stats = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsStoreStats";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                    object[] passing = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    timeFrame = (string)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate) { lblDates.Text = "Store stats for: " + startDate.ToString("dd/MMM/yy"); }
                    else { lblDates.Text = "Store stats for: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy"); }
                    //Binding the gridview
                    if (timeFrame.Equals("Day") || timeFrame.Equals("Default")){ stats = R.returnStoreStats(startDate, endDate, 1, objPageDetails); }
                    else if (timeFrame.Equals("Week")){ stats = R.returnStoreStats(startDate, endDate, 2, objPageDetails); }
                    else if (timeFrame.Equals("Month")){ stats = R.returnStoreStats(startDate, endDate, 3, objPageDetails); }
                    //Checking if there are any values
                    if (stats.Rows.Count > 0)
                    {
                        grdStats.DataSource = stats;
                        grdStats.DataBind();
                        if(grdStats.Rows.Count > 0)
                        {
                            if (timeFrame.Equals("Day") || timeFrame.Equals("Default")) { grdStats.HeaderRow.Cells[2].Text = "Date"; }
                            else if (timeFrame.Equals("Week")) { grdStats.HeaderRow.Cells[2].Text = "Week Start Date"; }
                            else if (timeFrame.Equals("Month")) { grdStats.Columns[2].Visible = false; }
                        }
                    }
                    else
                    {
                        if (startDate == endDate) { lblDates.Text = "There are no stats for: " + startDate.ToString("dd/MMM/yy"); }
                        else { lblDates.Text = "There are no states for: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy"); }
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

        protected void grdStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdStats_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    gTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "governmentTax"));
                    pTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provincialTax"));
                    cogs += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalCOGS"));
                    pm += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "averageProfitMargin"));
                    salesPrT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "salespretax"));
                    salesPoT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "salesposttax"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblGovTaxTotal = (Label)e.Row.FindControl("lblGovTaxTotal");
                    Label lblProvTaxTotal = (Label)e.Row.FindControl("lblProvTaxTotal");
                    Label lblCOGSTotal = (Label)e.Row.FindControl("lblCOGSTotal");
                    Label lblAverageProfitMarginTotal = (Label)e.Row.FindControl("lblAverageProfitMarginTotal");
                    Label lblSalesPreTaxTotal = (Label)e.Row.FindControl("lblSalesPreTaxTotal");
                    Label lblSalesPostTaxTotal = (Label)e.Row.FindControl("lblSalesPostTaxTotal");

                    lblGovTaxTotal.Text = Math.Round(gTax, 2).ToString("C");
                    lblProvTaxTotal.Text = Math.Round(pTax, 2).ToString("C");
                    lblCOGSTotal.Text = Math.Round(cogs, 2).ToString("C");
                    avp = ((salesPrT - cogs) / salesPrT);
                    lblAverageProfitMarginTotal.Text = String.Format("{0:P}", avp);
                    lblSalesPreTaxTotal.Text = Math.Round(salesPrT, 2).ToString("C");
                    lblSalesPostTaxTotal.Text = Math.Round(salesPoT, 2).ToString("C");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                object[] passing = (object[])Session["reportInfo"];
                string type = (string)passing[1];
                string fileName = "Store Stats Report - " + type + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    int rowAdjust = 0;
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet statsExport = xlPackage.Workbook.Worksheets.Add("Stats");
                    // write to sheet   
                    statsExport.Cells[1, 1].Value = lblDates.Text;
                    statsExport.Cells[2, 1].Value = "Year";
                    statsExport.Cells[2, 2].Value = "Month";
                    if (timeFrame.Equals("Day") || timeFrame.Equals("Default")) { statsExport.Cells[2, 3].Value = "Trade-In Amount"; }
                    else if (timeFrame.Equals("Week")) { statsExport.Cells[2, 3].Value = "Week Start Date"; }
                    else if (timeFrame.Equals("Month")) { rowAdjust = 1; }
                    

                    statsExport.Cells[2, 4 - rowAdjust].Value = "City Name";
                    statsExport.Cells[2, 5 - rowAdjust].Value = "Government Tax";
                    statsExport.Cells[2, 6 - rowAdjust].Value = "Provincial Tax";
                    statsExport.Cells[2, 7 - rowAdjust].Value = "Total COGS";
                    statsExport.Cells[2, 8 - rowAdjust].Value = "Average Profit Margin";
                    statsExport.Cells[2, 9 - rowAdjust].Value = "Sales Pre-Tax";
                    statsExport.Cells[2, 10 - rowAdjust].Value = "Sales Post-Tax";
                    int recordIndex = 3;
                    foreach (DataRow row in stats.Rows)
                    {                       
                        if (!timeFrame.Equals("Month"))
                        {
                            statsExport.Cells[recordIndex, 1].Value = row[0].ToString();
                            statsExport.Cells[recordIndex, 2].Value = row[1].ToString();
                            DateTime date = Convert.ToDateTime(row[2]);
                            statsExport.Cells[recordIndex, 3].Value = date.ToString("dd-MM-yyyy");
                            statsExport.Cells[recordIndex, 4].Value = row[3].ToString();
                            statsExport.Cells[recordIndex, 5].Value = row[4].ToString();
                            statsExport.Cells[recordIndex, 6].Value = row[5].ToString();
                            statsExport.Cells[recordIndex, 7].Value = row[6].ToString();
                            statsExport.Cells[recordIndex, 8].Value = row[7].ToString();
                            statsExport.Cells[recordIndex, 9].Value = row[8].ToString();
                            statsExport.Cells[recordIndex, 10].Value = row[9].ToString();
                        }
                        else
                        {
                            statsExport.Cells[recordIndex, 1].Value = row[1].ToString(); //Year
                            statsExport.Cells[recordIndex, 2].Value = row[2].ToString(); //Month
                            statsExport.Cells[recordIndex, 4 - rowAdjust].Value = row[4].ToString(); //City
                            statsExport.Cells[recordIndex, 5 - rowAdjust].Value = row[5].ToString(); //GTax
                            statsExport.Cells[recordIndex, 6 - rowAdjust].Value = row[6].ToString(); //PTax
                            statsExport.Cells[recordIndex, 7 - rowAdjust].Value = row[7].ToString(); //COGS
                            statsExport.Cells[recordIndex, 8 - rowAdjust].Value = row[8].ToString(); //AVP
                            statsExport.Cells[recordIndex, 9 - rowAdjust].Value = row[9].ToString(); //Sale pre
                            statsExport.Cells[recordIndex, 10 - rowAdjust].Value = row[10].ToString(); //Sale pos
                        }
                        
                           
                        recordIndex++;
                    }
                    //Totals
                    statsExport.Cells[recordIndex + 1, 1].Value = "Totals:";                    
                    statsExport.Cells[recordIndex + 1, 5 - rowAdjust].Value = gTax.ToString();
                    statsExport.Cells[recordIndex + 1, 6 - rowAdjust].Value = pTax.ToString();
                    statsExport.Cells[recordIndex + 1, 7 - rowAdjust].Value = cogs.ToString();
                    statsExport.Cells[recordIndex + 1, 8 - rowAdjust].Value = avp.ToString();
                    statsExport.Cells[recordIndex + 1, 9 - rowAdjust].Value = salesPrT.ToString();
                    statsExport.Cells[recordIndex + 1, 10 - rowAdjust].Value = salesPoT.ToString();
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}