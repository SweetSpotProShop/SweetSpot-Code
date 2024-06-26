﻿using OfficeOpenXml;
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
    public partial class ReportsPaymentType : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double salesCash;
        double salesDebit;
        double salesGiftCard;
        double salesMastercard;
        double salesVisa;
        double salesAmEx;
        DataTable dt = new DataTable();

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
                    ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                    //Builds string to display in label
                    lblDates.Text = "Items sold on: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                    //dt = R.CallReturnSalesByPaymentTypeForSelectedDate(repInfo, objPageDetails);
                    //GrdSalesByDate.DataSource = dt;
                    //GrdSalesByDate.DataBind();
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
                    salesCash += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Cash"));
                    salesDebit += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Debit"));
                    salesGiftCard += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "GiftCard"));
                    salesMastercard += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Mastercard"));
                    salesVisa += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Visa"));
                    salesAmEx += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "AmEx"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", salesCash);
                    e.Row.Cells[2].Text = String.Format("{0:C}", salesDebit);
                    e.Row.Cells[3].Text = String.Format("{0:C}", salesGiftCard);
                    e.Row.Cells[4].Text = String.Format("{0:C}", salesMastercard);
                    e.Row.Cells[5].Text = String.Format("{0:C}", salesVisa);
                    e.Row.Cells[5].Text = String.Format("{0:C}", salesAmEx);
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
                object[] passing = (object[])Session["reportInfo"];
                string loc = LM.CallReturnLocationName(Convert.ToInt32(passing[1]), objPageDetails);
                string fileName = "Payment Type Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet paymentTypeExport = xlPackage.Workbook.Worksheets.Add("Payment Type");
                    // write to sheet   
                    paymentTypeExport.Cells[1, 1].Value = lblDates.Text;
                    paymentTypeExport.Cells[2, 1].Value = "Date";
                    paymentTypeExport.Cells[2, 2].Value = "Cash";
                    paymentTypeExport.Cells[2, 3].Value = "Debit";
                    paymentTypeExport.Cells[2, 4].Value = "Gift Card";
                    paymentTypeExport.Cells[2, 5].Value = "Mastercard";
                    paymentTypeExport.Cells[2, 6].Value = "Visa";
                    paymentTypeExport.Cells[2, 67].Value = "AmEx";
                    int recordIndex = 3;
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime d = (DateTime)row[0];
                        paymentTypeExport.Cells[recordIndex, 1].Value = d.ToString("d");
                        paymentTypeExport.Cells[recordIndex, 2].Value = row[1];
                        paymentTypeExport.Cells[recordIndex, 3].Value = row[2];
                        paymentTypeExport.Cells[recordIndex, 4].Value = row[3];
                        paymentTypeExport.Cells[recordIndex, 5].Value = row[4];
                        paymentTypeExport.Cells[recordIndex, 6].Value = row[5];
                        paymentTypeExport.Cells[recordIndex, 7].Value = row[6];
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}