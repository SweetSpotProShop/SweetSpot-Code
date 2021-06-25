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
    public partial class ReportsStoreStats : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly Reports R = new Reports();
        readonly LocationManager LM = new LocationManager();
        CurrentUser CU;

        double governmentTax;
        double harmonizedTax;
        double liquorTax;
        double provincialTax;
        double quebecTax;
        double retailTax;
        double costofGoods;
        double subTotal;
        double salesDollars;
        double totalSales;

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
                    if (!Page.IsPostBack)
                    {
                        CU = (CurrentUser)Session["currentUser"];
                        //Gathering the start and end dates
                        ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                        //Builds string to display in label
                        Calendar calStartDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalStartDate");
                        calStartDate.SelectedDate = repInfo.dtmStartDate;
                        Calendar calEndDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalEndDate");
                        calEndDate.SelectedDate = repInfo.dtmEndDate;
                        DropDownList ddlDatePeriod = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlDatePeriod");
                        ddlDatePeriod.SelectedValue = repInfo.intGroupTimeFrame.ToString();
                        DropDownList ddlLocation = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlLocation");
                        DataTable dt = LM.CallReturnLocationDropDown(objPageDetails);
                        dt.Rows.Add(99, "All Locations");
                        ddlLocation.DataSource = dt;
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = repInfo.intLocationID.ToString();

                        lblDates.Text = "Store Stats through: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;

                        //Binding the gridview
                        DataTable resultSet = R.CallReturnStoreStats(repInfo, objPageDetails);
                        GrdStats.DataSource = resultSet;
                        GrdStats.DataBind();
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
        protected void GrdStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdStats_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    governmentTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                    harmonizedTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltHarmonizedTaxAmount"));
                    liquorTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                    provincialTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    quebecTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltQuebecTaxAmount"));
                    retailTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltRetailTaxAmount"));
                    costofGoods += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltCostofGoods"));
                    subTotal += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSubTotal"));
                    salesDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSalesDollars"));
                    totalSales += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalSales"));

                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = string.Format("{0:C}", subTotal);
                    e.Row.Cells[3].Text = string.Format("{0:C}", governmentTax);
                    e.Row.Cells[4].Text = string.Format("{0:C}", harmonizedTax);
                    e.Row.Cells[5].Text = string.Format("{0:C}", liquorTax);
                    e.Row.Cells[6].Text = string.Format("{0:C}", provincialTax);
                    e.Row.Cells[7].Text = string.Format("{0:C}", quebecTax);
                    e.Row.Cells[8].Text = string.Format("{0:C}", retailTax);
                    e.Row.Cells[9].Text = string.Format("{0:C}", totalSales);
                    e.Row.Cells[10].Text = string.Format("{0:P}", (salesDollars - costofGoods) / salesDollars);
                    e.Row.Cells[11].Text = string.Format("{0:C}", costofGoods);
                    e.Row.Cells[12].Text = string.Format("{0:C}", salesDollars);

                    if (governmentTax == 0)
                    {
                        GrdStats.Columns[3].Visible = false;
                    }
                    if (harmonizedTax == 0)
                    {
                        GrdStats.Columns[4].Visible = false;
                    }
                    if (liquorTax == 0)
                    {
                        GrdStats.Columns[5].Visible = false;
                    }
                    if (provincialTax == 0)
                    {
                        GrdStats.Columns[6].Visible = false;
                    }
                    if (quebecTax == 0)
                    {
                        GrdStats.Columns[7].Visible = false;
                    }
                    if (retailTax == 0)
                    {
                        GrdStats.Columns[8].Visible = false;
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
            string method = "btnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");

                ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                DataTable stats = R.CallReturnStoreStats(repInfo, objPageDetails);

                string fileName = "Store Stats Report-" + repInfo.varLocationName + "_" + repInfo.dtmStartDate.ToShortDateString() + " - " + repInfo.dtmEndDate.ToShortDateString() + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet statsExport = xlPackage.Workbook.Worksheets.Add("Stats");
                    // write to sheet   
                    statsExport.Cells[1, 1].Value = lblDates.Text;
                    statsExport.Cells[2, 1].Value = "Grouped By";
                    statsExport.Cells[2, 2].Value = "Government Tax";
                    statsExport.Cells[2, 3].Value = "Harmonized Tax";
                    statsExport.Cells[2, 4].Value = "Liquor Tax";
                    statsExport.Cells[2, 5].Value = "Provincial Tax";
                    statsExport.Cells[2, 6].Value = "Quebec Tax";
                    statsExport.Cells[2, 7].Value = "Retail Tax";
                    statsExport.Cells[2, 8].Value = "Cost of Goods";
                    statsExport.Cells[2, 9].Value = "Sales Pre-Tax";
                    statsExport.Cells[2, 10].Value = "Profit Margin";
                    statsExport.Cells[2, 11].Value = "Sales Dollars";
                    int recordIndex = 3;
                    foreach (DataRow row in stats.Rows)
                    {
                        statsExport.Cells[recordIndex, 1].Value = row[0].ToString();

                        statsExport.Cells[recordIndex, 2].Value = Convert.ToDouble(row[2]).ToString("C");
                        statsExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[3]).ToString("C");
                        statsExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[4]).ToString("C");
                        statsExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[5]).ToString("C");
                        statsExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[6]).ToString("C");
                        statsExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[7]).ToString("C");
                        statsExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[8]).ToString("C");
                        statsExport.Cells[recordIndex, 9].Value = Convert.ToDouble(row[9]).ToString("C");
                        statsExport.Cells[recordIndex, 10].Value = Convert.ToDouble(row[10]).ToString("P");
                        statsExport.Cells[recordIndex, 11].Value = Convert.ToDouble(row[11]).ToString("C");
                        recordIndex++;
                    }
                    //Totals
                    statsExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    statsExport.Cells[recordIndex + 1, 2].Value = governmentTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 3].Value = harmonizedTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 4].Value = liquorTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 5].Value = provincialTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 6].Value = quebecTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 7].Value = retailTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 8].Value = costofGoods.ToString("C");
                    statsExport.Cells[recordIndex + 1, 9].Value = subTotal.ToString("C");

                    statsExport.Cells[recordIndex + 1, 11].Value = salesDollars.ToString();

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