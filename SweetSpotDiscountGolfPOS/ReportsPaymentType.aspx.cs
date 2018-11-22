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
    public partial class ReportsPaymentType : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        LocationManager LM = new LocationManager();
      
        double salesCash;
        double salesDebit;
        double salesGiftCard;
        double salesMastercard;
        double salesVisa;
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
                    object[] passing = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    else
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    dt = R.returnSalesByPaymentTypeForSelectedDate(passing, objPageDetails);
                    grdSalesByDate.DataSource = dt;
                    grdSalesByDate.DataBind();
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
        protected void grdSalesByDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdSalesByDate_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    salesCash += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Cash"));
                    salesDebit += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Debit"));
                    salesGiftCard += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "GiftCard"));
                    salesMastercard += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Mastercard"));
                    salesVisa += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Visa"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", salesCash);
                    e.Row.Cells[2].Text = String.Format("{0:C}", salesDebit);
                    e.Row.Cells[3].Text = String.Format("{0:C}", salesGiftCard);
                    e.Row.Cells[4].Text = String.Format("{0:C}", salesMastercard);
                    e.Row.Cells[5].Text = String.Format("{0:C}", salesVisa);
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
                string loc = LM.ReturnLocationName(Convert.ToInt32(passing[1]), objPageDetails);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}