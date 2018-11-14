using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsPurchasesMade : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        LocationManager LM = new LocationManager();

        double totalPurchAmount = 0;
        int totalPurchases = 0;
        int totalCheques = 0;
        List<Purchases> purch = new List<Purchases>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsPurchasesMade.aspx";
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
                    int locationID = (int)repInfo[1];
                    //Builds string to display in label
                    lblPurchasesMadeDate.Text = "Purchases Made Between: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + LM.ReturnLocationName(locationID);
                    //Creating a cashout list and calling a method that grabs all mops and amounts paid
                    purch = R.returnPurchasesDuringDates(startDate, endDate, locationID);
                    grdPurchasesMade.DataSource = purch;
                    grdPurchasesMade.DataBind();
                    foreach (GridViewRow row in grdPurchasesMade.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            cell.Attributes.CssStyle["text-align"] = "center";
                        }
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
        protected void grdPurchasesMade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdPurchasesMade_RowDataBound";
            try
            {
                // check row type
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "chequeNumber")) > 0)
                    {
                        totalCheques += 1;
                    }
                    totalPurchases += 1;
                    totalPurchAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amountPaid"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = totalPurchases.ToString();
                    e.Row.Cells[3].Text = totalCheques.ToString();
                    e.Row.Cells[4].Text = String.Format("{0:c}", totalPurchAmount);
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
        protected void printReport(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "printReport";
            //Current method does nothing
            try
            { }
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
        protected void lbtnReceiptNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnReceiptNumber_Click";
            try
            {
                LinkButton btn = sender as LinkButton;
                //Changes to the Reports Cash Out page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", btn.Text);
                //Changes page to printable invoice
                Response.Redirect("PrintableReceipt.aspx?" + nameValues, false);
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
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                object[] passing = (object[])Session["reportInfo"];
                string loc = LM.ReturnLocationName(Convert.ToInt32(passing[1]));
                string fileName = "Purchases Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet purchasesExport = xlPackage.Workbook.Worksheets.Add("Purchases");
                    // write to sheet   
                    purchasesExport.Cells[1, 1].Value = lblPurchasesMadeDate.Text;
                    purchasesExport.Cells[2, 1].Value = "Receipt Number";
                    purchasesExport.Cells[2, 2].Value = "Receipt Date";
                    purchasesExport.Cells[2, 3].Value = "Purchase Method";
                    purchasesExport.Cells[2, 4].Value = "Cheque Number";
                    purchasesExport.Cells[2, 5].Value = "Purchase Amount";
                    int recordIndex = 3;
                    foreach (Purchases p in purch)
                    {

                        purchasesExport.Cells[recordIndex, 1].Value = p.receiptNumber;
                        purchasesExport.Cells[recordIndex, 2].Value = p.receiptDate.ToString("d");
                        purchasesExport.Cells[recordIndex, 3].Value = p.mopDescription;
                        purchasesExport.Cells[recordIndex, 4].Value = p.chequeNumber;
                        purchasesExport.Cells[recordIndex, 5].Value = p.amountPaid;
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