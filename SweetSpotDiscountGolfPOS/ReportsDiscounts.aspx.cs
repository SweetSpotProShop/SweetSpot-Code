using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportDiscounts : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        LocationManager LM = new LocationManager();

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
                    CU = (CurrentUser)Session["currentUser"];
                    //Gathering the start and end dates
                    object[] repInfo = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])repInfo[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = Convert.ToInt32(repInfo[1]);
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblReportDate.Text = "Discount Report on: " + startDate.ToString("dd/MMM/yy") + " for " + LM.CallReturnLocationName(locationID, objPageDetails);
                    }
                    else
                    {
                        lblReportDate.Text = "Discount Report on: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy") + " for " + LM.CallReturnLocationName(locationID, objPageDetails);
                    }
                    DataTable discounts = R.CallReturnDiscountsBetweenDates(repInfo, objPageDetails);
                    grdInvoiceDisplay.DataSource = discounts;
                    grdInvoiceDisplay.DataBind();

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
        protected void grdInvoiceDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdInvoiceDisplay_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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

                object[] repInfo = (object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])repInfo[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];
                int locationID = Convert.ToInt32(repInfo[1]);

                DataTable discounts = R.CallReturnDiscountsBetweenDates(repInfo, objPageDetails);

                string fileName = "Discount Report-" + LM.CallReturnLocationName(locationID, objPageDetails) + "_" + startDate.ToShortDateString() + " - " + endDate.ToShortDateString() + ".xlsx";
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}