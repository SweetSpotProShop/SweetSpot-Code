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
    public partial class ReportsSales : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double salesDollars;
        double gstDollars;
        double pstDollars;
        double lctDollars;

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
                    CU = (CurrentUser)Session["currentUser"];
                    //Gathering the start and end dates
                    object[] passing = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("dd/MMM/yy") + " for " + LM.CallReturnLocationName(locationID, objPageDetails);
                    }
                    else
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy") + " for " + LM.CallReturnLocationName(locationID, objPageDetails);
                    }
                    DataTable dt = R.CallReturnSalesForSelectedDate(passing, objPageDetails);
                    GrdSalesByDate.DataSource = dt;
                    GrdSalesByDate.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
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
                    salesDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalSales"));
                    gstDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                    pstDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    lctDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", gstDollars);
                    e.Row.Cells[2].Text = String.Format("{0:C}", pstDollars);
                    e.Row.Cells[3].Text = String.Format("{0:C}", lctDollars);
                    e.Row.Cells[4].Text = String.Format("{0:C}", salesDollars + gstDollars + pstDollars + lctDollars);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
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

                object[] passing = (object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])passing[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];

                DataTable dt = R.CallReturnSalesForSelectedDate(passing, objPageDetails);
                string fileName = "Sales Report by Date-" + LM.CallReturnLocationName(Convert.ToInt32(passing[1]), objPageDetails) + "_" + startDate.ToShortDateString() + " - " + endDate.ToShortDateString() + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet salesExport = xlPackage.Workbook.Worksheets.Add("Sales");
                    // write to sheet   
                    salesExport.Cells[1, 1].Value = lblDates.Text;
                    salesExport.Cells[2, 1].Value = "Date";
                    salesExport.Cells[2, 2].Value = "GST";
                    salesExport.Cells[2, 3].Value = "PST";
                    salesExport.Cells[2, 4].Value = "LCT";
                    salesExport.Cells[2, 5].Value = "Sales Dollars";
                    int recordIndex = 3;
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime d = (DateTime)row[0];
                        salesExport.Cells[recordIndex, 1].Value = d.ToString("d");
                        salesExport.Cells[recordIndex, 2].Value = Convert.ToDouble(row[1]).ToString("C");
                        salesExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[2]).ToString("C");
                        salesExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[3]).ToString("C");
                        salesExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[4]).ToString("C");
                        recordIndex++;
                    }

                    salesExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    salesExport.Cells[recordIndex + 1, 2].Value = gstDollars;
                    salesExport.Cells[recordIndex + 1, 3].Value = pstDollars;
                    salesExport.Cells[recordIndex + 1, 4].Value = lctDollars;
                    salesExport.Cells[recordIndex + 1, 5].Value = salesDollars + gstDollars + pstDollars + lctDollars;

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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}