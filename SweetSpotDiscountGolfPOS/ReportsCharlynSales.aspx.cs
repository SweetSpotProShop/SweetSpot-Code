using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsCharlynSales : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double salesDollars;
        double governmentTax;
        double harmonizedTax;
        double liquorTax;
        double provincialTax;
        double quebecTax;
        double retailTax;
        double totalSales;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsSales";
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
                    if (!IsPostBack)
                    {
                        CU = (CurrentUser)Session["currentUser"];
                        //Gathering the start and end dates
                        ReportInformation repInfo = (ReportInformation)Session["reportInfo"];

                        //Calendar calStartDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalStartDate");
                        //calStartDate.SelectedDate = repInfo.dtmStartDate;
                        //Calendar calEndDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalEndDate");
                        //calEndDate.SelectedDate = repInfo.dtmEndDate;
                        //DropDownList ddlDatePeriod = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlDatePeriod");
                        //ddlDatePeriod.SelectedValue = repInfo.intGroupTimeFrame.ToString();
                        //DropDownList ddlLocation = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlLocation");
                        //DataTable dt = LM.CallReturnLocationDropDown(objPageDetails);
                        //dt.Rows.Add(99, "All Locations");
                        //ddlLocation.DataSource = dt;
                        //ddlLocation.DataBind();
                        //ddlLocation.SelectedValue = repInfo.intLocationID.ToString();

                        //Builds string to display in label
                        lblDates.Text = "Items sold on: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                        DataTable resultSet = R.CallReturnSalesForSelectedDateCharlynReport(repInfo, objPageDetails);
                        GrdSalesByDate.DataSource = resultSet;
                        GrdSalesByDate.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdSalesByDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "GrdSalesByDate_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    salesDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSalesDollars"));
                    governmentTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                    harmonizedTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltHarmonizedTaxAmount"));
                    liquorTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                    provincialTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    quebecTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltquebecTaxAmount"));
                    retailTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltretailTaxAmount"));
                    totalSales += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalSales"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", salesDollars);
                    e.Row.Cells[2].Text = String.Format("{0:C}", governmentTax);
                    e.Row.Cells[3].Text = String.Format("{0:C}", harmonizedTax);
                    e.Row.Cells[4].Text = String.Format("{0:C}", liquorTax);
                    e.Row.Cells[5].Text = String.Format("{0:C}", provincialTax);
                    e.Row.Cells[6].Text = String.Format("{0:C}", quebecTax);
                    e.Row.Cells[7].Text = String.Format("{0:C}", retailTax);
                    e.Row.Cells[8].Text = String.Format("{0:C}", totalSales);

                    if (governmentTax == 0)
                    {
                        GrdSalesByDate.Columns[2].Visible = false;
                    }
                    if (harmonizedTax == 0)
                    {
                        GrdSalesByDate.Columns[3].Visible = false;
                    }
                    if (liquorTax == 0)
                    {
                        GrdSalesByDate.Columns[4].Visible = false;
                    }
                    if (provincialTax == 0)
                    {
                        GrdSalesByDate.Columns[5].Visible = false;
                    }
                    if (quebecTax == 0)
                    {
                        GrdSalesByDate.Columns[6].Visible = false;
                    }
                    if (retailTax == 0)
                    {
                        GrdSalesByDate.Columns[7].Visible = false;
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");

                ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                DataTable dt = R.CallReturnSalesForSelectedDate(repInfo, objPageDetails);
                string fileName = "Sales Report by Date-" + repInfo.varLocationName + "_" + repInfo.dtmStartDate.ToShortDateString() + " - " + repInfo.dtmEndDate.ToShortDateString() + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet salesExport = xlPackage.Workbook.Worksheets.Add("Sales");
                    // write to sheet   
                    salesExport.Cells[1, 1].Value = lblDates.Text;
                    salesExport.Cells[2, 1].Value = "Date";
                    salesExport.Cells[2, 2].Value = "Sales Dollars";
                    salesExport.Cells[2, 3].Value = "GST";
                    salesExport.Cells[2, 4].Value = "HST";
                    salesExport.Cells[2, 5].Value = "LCT";
                    salesExport.Cells[2, 6].Value = "PST";
                    salesExport.Cells[2, 7].Value = "QST";
                    salesExport.Cells[2, 8].Value = "RST";
                    salesExport.Cells[2, 9].Value = "Total Sales";
                    int recordIndex = 3;
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime d = (DateTime)row[0];
                        salesExport.Cells[recordIndex, 1].Value = d.ToString("d");
                        salesExport.Cells[recordIndex, 2].Value = Convert.ToDouble(row[7]).ToString("C");
                        salesExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[1]).ToString("C");
                        salesExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[2]).ToString("C");
                        salesExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[3]).ToString("C");
                        salesExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[4]).ToString("C");
                        salesExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[5]).ToString("C");
                        salesExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[6]).ToString("C");
                        salesExport.Cells[recordIndex, 9].Value = Convert.ToDouble(row[9]).ToString("C");
                        recordIndex++;
                    }

                    salesExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    salesExport.Cells[recordIndex + 1, 2].Value = salesDollars.ToString("C");
                    salesExport.Cells[recordIndex + 1, 3].Value = governmentTax.ToString("C");
                    salesExport.Cells[recordIndex + 1, 4].Value = harmonizedTax.ToString("C");
                    salesExport.Cells[recordIndex + 1, 5].Value = liquorTax.ToString("C");
                    salesExport.Cells[recordIndex + 1, 6].Value = provincialTax.ToString("C");
                    salesExport.Cells[recordIndex + 1, 7].Value = quebecTax.ToString("C");
                    salesExport.Cells[recordIndex + 1, 8].Value = retailTax.ToString("C");
                    salesExport.Cells[recordIndex + 1, 9].Value = totalSales.ToString("C");


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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}