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
        double provincialTax;
        double liquorTax;
        double costofGoods;
        double subTotal;
        double salesDollars;
        double totalSales;
        //double profitMargin;
        //int profitMarginCount = 0;

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
                        object[] passing = (object[])Session["reportInfo"];
                        DateTime[] reportDates = (DateTime[])passing[0];
                        DateTime startDate = reportDates[0];
                        DateTime endDate = reportDates[1];
                        int locationID = Convert.ToInt32(passing[1]);
                        int timeFrame = Convert.ToInt32(passing[2]);

                        Calendar calStartDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalStartDate");
                        calStartDate.SelectedDate = startDate;
                        Calendar calEndDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalEndDate");
                        calEndDate.SelectedDate = endDate;
                        DropDownList ddlDatePeriod = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlDatePeriod");
                        ddlDatePeriod.SelectedValue = timeFrame.ToString();
                        DropDownList ddlLocation = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlLocation");
                        DataTable dt = LM.CallReturnLocationDropDown(objPageDetails);
                        dt.Rows.Add(99, "All Locations");
                        ddlLocation.DataSource = dt;
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = locationID.ToString();

                        //Builds string to display in label
                        if (startDate == endDate)
                        {
                            lblDates.Text = "Store stats on: " + startDate.ToString("dd/MMM/yy"); //+ " for " + LM.ReturnLocationName(locationID, objPageDetails);
                        }
                        else
                        {
                            lblDates.Text = "Store stats on: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy"); //+ " for " + LM.ReturnLocationName(locationID, objPageDetails);
                        }
                        //Binding the gridview
                        DataTable stats = R.CallReturnStoreStats(startDate, endDate, timeFrame, locationID, objPageDetails);

                        GrdStats.DataSource = stats;
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
                    provincialTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    liquorTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                    costofGoods += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltCostofGoods"));
                    subTotal += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSubTotal"));
                    //profitMargin += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProfitMargin"));
                    salesDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSalesDollars"));
                    totalSales += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalSales"));

                    //profitMarginCount++;
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = string.Format("{0:C}", subTotal);
                    e.Row.Cells[3].Text = string.Format("{0:C}", governmentTax);
                    e.Row.Cells[4].Text = string.Format("{0:C}", liquorTax);
                    e.Row.Cells[5].Text = string.Format("{0:C}", provincialTax);
                    e.Row.Cells[6].Text = string.Format("{0:C}", totalSales);
                    e.Row.Cells[7].Text = string.Format("{0:P}", (salesDollars - costofGoods) / salesDollars);
                    e.Row.Cells[8].Text = string.Format("{0:C}", costofGoods);
                    e.Row.Cells[9].Text = string.Format("{0:C}", salesDollars);

                    if (provincialTax == 0)
                    {
                        GrdStats.Columns[5].Visible = false;
                    }
                    if (liquorTax == 0)
                    {
                        GrdStats.Columns[4].Visible = false;
                    }
                    if (governmentTax == 0)
                    {
                        GrdStats.Columns[3].Visible = false;
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

                object[] passing = (object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])passing[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];
                int locationID = Convert.ToInt32(passing[1]);
                int timeFrame = Convert.ToInt32(passing[2]);

                DataTable stats = R.CallReturnStoreStats(startDate, endDate, timeFrame, locationID, objPageDetails);

                string fileName = "Store Stats Report-" + LM.CallReturnLocationName(locationID, objPageDetails) + "_" + startDate.ToShortDateString() + " - " + endDate.ToShortDateString() + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet statsExport = xlPackage.Workbook.Worksheets.Add("Stats");
                    // write to sheet   
                    statsExport.Cells[1, 1].Value = lblDates.Text;
                    statsExport.Cells[2, 1].Value = "Grouped By";
                    statsExport.Cells[2, 2].Value = "Government Tax";
                    statsExport.Cells[2, 3].Value = "Provincial Tax";
                    statsExport.Cells[2, 4].Value = "Liquor Tax";
                    statsExport.Cells[2, 5].Value = "Cost of Goods";
                    statsExport.Cells[2, 6].Value = "Sales Pre-Tax";
                    statsExport.Cells[2, 7].Value = "Average Profit Margin";
                    statsExport.Cells[2, 8].Value = "Sales Dollars";
                    int recordIndex = 3;
                    foreach (DataRow row in stats.Rows)
                    {
                        //if (timeFrame == 3)
                        //{
                            statsExport.Cells[recordIndex, 1].Value = row[0].ToString();
                        //}
                        //else
                        //{
                            //statsExport.Cells[recordIndex, 1].Value = Convert.ToDateTime(row[0]).ToString("dd-MM-yyyy");
                        //}
                        statsExport.Cells[recordIndex, 2].Value = Convert.ToDouble(row[1]).ToString("C");
                        statsExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[2]).ToString("C");
                        statsExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[3]).ToString("C");
                        statsExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[4]).ToString("C");
                        statsExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[5]).ToString("C");
                        statsExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[6]).ToString("P");
                        statsExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[7]).ToString("C");

                        recordIndex++;
                    }
                    //Totals
                    statsExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    statsExport.Cells[recordIndex + 1, 2].Value = governmentTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 3].Value = provincialTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 4].Value = liquorTax.ToString("C");
                    statsExport.Cells[recordIndex + 1, 5].Value = costofGoods.ToString("C");
                    statsExport.Cells[recordIndex + 1, 6].Value = subTotal.ToString("C");
                    //statsExport.Cells[recordIndex + 1, 7].Value = (profitMargin / profitMarginCount).ToString("P");
                    statsExport.Cells[recordIndex + 1, 8].Value = salesDollars.ToString();

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