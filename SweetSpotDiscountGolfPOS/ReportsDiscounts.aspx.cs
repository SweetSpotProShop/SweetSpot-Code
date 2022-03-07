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
    public partial class ReportDiscounts : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double tDiscount;
        double tBalance;
        double tGST;
        double tPST;
        double tLCT;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsDiscounts.aspx";
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
                        lblReportDate.Text = "Discount Report on: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                        DataTable resultSet = R.CallReturnDiscountsBetweenDates(repInfo, objPageDetails);
                        GrdInvoiceDisplay.DataSource = resultSet;
                        GrdInvoiceDisplay.DataBind();
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
        protected void GrdInvoiceDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "GrdInvoiceDisplay_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    tDiscount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalDiscount"));
                    tBalance += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltBalanceDue"));
                    tPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                    tGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    tLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = String.Format("{0:C}", tDiscount);
                    e.Row.Cells[4].Text = String.Format("{0:C}", tBalance + tGST + tPST + tLCT);
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
                DataTable discounts = R.CallReturnDiscountsBetweenDates(repInfo, objPageDetails);

                string fileName = "Discount Report-" + repInfo.varLocationName + "_" + repInfo.dtmStartDate.ToShortDateString() + " - " + repInfo.dtmEndDate.ToShortDateString() + ".xlsx";
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
                    discountsExport.Cells[2, 5].Value = "Invoice Total";
                    discountsExport.Cells[2, 6].Value = "Employee Name";
                    int recordIndex = 3;
                    //List<Items> items = new List<Items>();
                    foreach (DataRow i in discounts.Rows)
                    {
                        discountsExport.Cells[recordIndex, 1].Value = i[1].ToString() + "-" + i[2].ToString();
                        discountsExport.Cells[recordIndex, 2].Value = Convert.ToDateTime(i[3]).ToShortDateString();
                        discountsExport.Cells[recordIndex, 3].Value = i[4].ToString();
                        discountsExport.Cells[recordIndex, 4].Value = Convert.ToDouble(i[6]).ToString("C");
                        discountsExport.Cells[recordIndex, 5].Value = (Convert.ToDouble(i[7]) + Convert.ToDouble(i[8]) + Convert.ToDouble(i[9]) + Convert.ToDouble(i[10])).ToString("C");
                        discountsExport.Cells[recordIndex, 6].Value = i[5].ToString();
                        recordIndex++;
                    }

                    discountsExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    discountsExport.Cells[recordIndex + 1, 4].Value = tDiscount;
                    discountsExport.Cells[recordIndex + 1, 5].Value = tBalance + tGST + tPST + tLCT;

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