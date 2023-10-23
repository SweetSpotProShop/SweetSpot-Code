using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsCashOut : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly CashoutUtilities COU = new CashoutUtilities();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsCashOut";
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
                    if (!IsPostBack)
                    {
                        //Gathering the start and end dates
                        ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                        //Calendar calStartDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalStartDate");
                        //calStartDate.SelectedDate = repInfo.dtmStartDate;
                        //Calendar calEndDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalEndDate");
                        //calEndDate.SelectedDate = repInfo.dtmEndDate;
                        //DropDownList ddlDatePeriod = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlDatePeriod");
                        //ddlDatePeriod.SelectedValue = repInfo.intGroupTimeFrame.ToString();
                        //DropDownList ddlLocation = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlLocation");
                        DataTable dt = LM.CallReturnLocationDropDown(objPageDetails);
                        //dt.Rows.Add(99, "All Locations");
                        //ddlLocation.DataSource = dt;
                        //ddlLocation.DataBind();
                        //ddlLocation.SelectedValue = repInfo.intLocationID.ToString();

                        lblDates.Text = "Cashout report for: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                        DataTable resultSet = R.CallReturnCashoutsForSelectedDates(repInfo, objPageDetails);
                        GrdCashoutByDate.DataSource = resultSet;
                        GrdCashoutByDate.DataBind();
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
                //Gathering the start and end dates
                ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                DataTable resultSet = R.CallReturnCashoutsForSelectedDates(repInfo, objPageDetails);

                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string fileName = "CashOut Report by Date - " + repInfo.varLocationName + ".xlsx";

                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet salesExport = xlPackage.Workbook.Worksheets.Add("CashOut");
                    //Title
                    salesExport.Cells[1, 1, 1, 11].Merge = true;
                    salesExport.Cells[1, 1].Value = lblDates.Text;
                    salesExport.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    salesExport.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //Headers
                    salesExport.Cells[2, 1].Value = "Date";
                    salesExport.Cells[2, 3].Value = "Trade-In";
                    salesExport.Cells[2, 4].Value = "Gift Card";
                    salesExport.Cells[2, 5].Value = "Cash";
                    salesExport.Cells[2, 6].Value = "Debit";
                    salesExport.Cells[2, 7].Value = "MasterCard";
                    salesExport.Cells[2, 8].Value = "Visa";
                    salesExport.Cells[2, 9].Value = "AmEx";
                    salesExport.Cells[2, 10].Value = "Over/Short";
                    salesExport.Cells[2, 11].Value = "Processed";
                    salesExport.Cells[2, 12].Value = "Finalized";
                    salesExport.Cells[2, 1, 2, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                    //salesExport.Cells[2, 2].Value = "Sales Dollars";
                    int recordIndex = 3;
                    foreach (DataRow row in resultSet.Rows)
                    {
                        //Date
                        DateTime d = (DateTime)row[0];
                        salesExport.Cells[recordIndex, 1, recordIndex + 1, 1].Merge = true;
                        salesExport.Cells[recordIndex, 1].Value = d.ToString("d");
                        salesExport.Cells[recordIndex, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        salesExport.Cells[recordIndex, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        //Receipt over Sale
                        salesExport.Cells[recordIndex, 2].Value = "Receipts:";
                        salesExport.Cells[recordIndex + 1, 2].Value = "Sales:";
                        //TradeIn
                        salesExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[3].ToString());
                        salesExport.Cells[recordIndex + 1, 3].Value = Convert.ToDouble(row[2].ToString());
                        //GiftCard
                        salesExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[5].ToString());
                        salesExport.Cells[recordIndex + 1, 4].Value = Convert.ToDouble(row[4].ToString());
                        //Cash
                        salesExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[7].ToString());
                        salesExport.Cells[recordIndex + 1, 5].Value = Convert.ToDouble(row[6].ToString());
                        //Debit
                        salesExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[9].ToString());
                        salesExport.Cells[recordIndex + 1, 6].Value = Convert.ToDouble(row[8].ToString());
                        //MasterCard
                        salesExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[11].ToString());
                        salesExport.Cells[recordIndex + 1, 7].Value = Convert.ToDouble(row[10].ToString());
                        //Visa
                        salesExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[13].ToString());
                        salesExport.Cells[recordIndex + 1, 8].Value = Convert.ToDouble(row[12].ToString());

                        //AmEx
                        salesExport.Cells[recordIndex, 9].Value = Convert.ToDouble(row[15].ToString());
                        salesExport.Cells[recordIndex + 1, 9].Value = Convert.ToDouble(row[14].ToString());
                        //Over/Short
                        salesExport.Cells[recordIndex, 10, recordIndex + 1, 10].Merge = true;
                        salesExport.Cells[recordIndex, 10].Value = Convert.ToDouble(row[16].ToString());
                        salesExport.Cells[recordIndex, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        salesExport.Cells[recordIndex, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        //Processed
                        salesExport.Cells[recordIndex, 11, recordIndex + 1, 11].Merge = true;
                        salesExport.Cells[recordIndex, 11].Value = row[17].ToString();
                        salesExport.Cells[recordIndex, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        salesExport.Cells[recordIndex, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        //Finalized
                        salesExport.Cells[recordIndex, 12, recordIndex + 1, 12].Merge = true;
                        salesExport.Cells[recordIndex, 12].Value = row[18].ToString();
                        salesExport.Cells[recordIndex, 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        salesExport.Cells[recordIndex, 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        //Border
                        salesExport.Cells[recordIndex, 1, recordIndex + 1, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                        //Converting numbers to numbers
                        salesExport.Cells[recordIndex, 3, recordIndex + 1, 9].Style.Numberformat.Format = "#,##0.00";
                        //Autofit columns
                        salesExport.Cells[2, 1, recordIndex + 1, 11].AutoFitColumns();
                        //Incrementng the row
                        recordIndex += 2;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdCashoutByDate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "GrdCashoutByDate_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.CommandName == "EditCashout")
                {
                    string arg = e.CommandArgument.ToString();
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("selectedDate", arg.Split(' ')[0]);
                    nameValues.Set("location", arg.Split(' ')[1]);
                    //Changes to the Reports Cash Out page
                    Response.Redirect("SalesCashOut.aspx?" + nameValues, false);
                }
                else if (e.CommandName == "FinalizeCashout")
                {
                    COU.CallFinalizeCashout(e.CommandArgument.ToString(), objPageDetails);
                    ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                    DataTable resultSet = R.CallReturnCashoutsForSelectedDates(repInfo, objPageDetails);
                    GrdCashoutByDate.DataSource = resultSet;
                    GrdCashoutByDate.DataBind();
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
        protected void GrdCashoutByDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "GrdCashoutByDate_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "bitIsCashoutFinalized")) == 1)
                    {
                        Button edit = (Button)e.Row.FindControl("btnEdit");
                        edit.Enabled = false;
                        Button final = (Button)e.Row.FindControl("btnFinalize");
                        final.Enabled = false;
                    }
                    Label trade = (Label)e.Row.FindControl("lblTradeInBalance");
                    Label gift = (Label)e.Row.FindControl("lblGiftCardBalance");
                    Label cash = (Label)e.Row.FindControl("lblCashBalance");
                    Label debit = (Label)e.Row.FindControl("lblDebitBalance");
                    Label master = (Label)e.Row.FindControl("lblMasterCardBalance");
                    Label visa = (Label)e.Row.FindControl("lblVisaBalance");
                    if (trade.Text == "Discrepancy")
                    {
                        trade.ForeColor = System.Drawing.Color.Red;
                    }
                    if (gift.Text == "Discrepancy")
                    {
                        gift.ForeColor = System.Drawing.Color.Red;
                    }
                    if (cash.Text == "Discrepancy")
                    {
                        cash.ForeColor = System.Drawing.Color.Red;
                    }
                    if (debit.Text == "Discrepancy")
                    {
                        debit.ForeColor = System.Drawing.Color.Red;
                    }
                    if (master.Text == "Discrepancy")
                    {
                        master.ForeColor = System.Drawing.Color.Red;
                    }
                    if (visa.Text == "Discrepancy")
                    {
                        visa.ForeColor = System.Drawing.Color.Red;
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
    }
}