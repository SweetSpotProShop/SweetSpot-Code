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
    public partial class ReportsCOGSvsPM : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        LocationManager LM = new LocationManager();
        
        int locationID;
        double tCost;
        double tPrice;
        DataTable inv = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsCOGSvsPM";
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
                    locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Cost of Goods Sold & Profit Margin on: " + startDate.ToString("d") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    else
                    {
                        lblDates.Text = "Cost of Goods Sold & Profit Margin on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    //Binding the gridview
                    inv = R.returnInvoicesForCOGS(startDate, endDate, locationID, objPageDetails);
                    grdInvoiceSelection.DataSource = inv;
                    grdInvoiceSelection.DataBind();
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
        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnInvoiceNumber_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Text of the linkbutton
                LinkButton btn = sender as LinkButton;
                string invoice = btn.Text;
                //Changes page to display a printable invoice
                Server.Transfer("PrintableInvoice.aspx?inv=" + invoice, false);
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
        protected void grdInvoiceSelection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdInvoiceSelection_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label percent = (Label)e.Row.FindControl("lblPercentage");
                    Label discount = (Label)e.Row.FindControl("lblTotalDiscount");
                    if (percent.Text.Equals("True"))
                    {
                        discount.Text = discount.Text + "%";
                    }
                    else
                    {
                        if (discount.Text.Equals("0"))
                        {
                            discount.Text = "-";
                        }
                        else
                        {
                            discount.Text = "$" + discount.Text;
                        }
                    }
                    Label lblProfit = (Label)e.Row.FindControl("lblTotalProfit");
                    string profitText = lblProfit.Text;
                    if (profitText.Contains("("))
                    {
                        lblProfit.ForeColor = System.Drawing.Color.Red;
                    }
                    tCost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalCost"));
                    tPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalPrice"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", tPrice);
                    e.Row.Cells[2].Text = String.Format("{0:C}", tCost);
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
                string loc = LM.ReturnLocationName(locationID, objPageDetails);
                string fileName = "COGS and PM Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet cogsExport = xlPackage.Workbook.Worksheets.Add("COGS");
                    // write to sheet     
                    cogsExport.Cells[1, 1].Value = lblDates.Text;
                    cogsExport.Cells[2, 1].Value = "Invoice Number";
                    cogsExport.Cells[2, 2].Value = "Total Cost";
                    cogsExport.Cells[2, 3].Value = "Total Price";
                    cogsExport.Cells[2, 4].Value = "Total Discount";
                    cogsExport.Cells[2, 5].Value = "Profit Margin";
                    int recordIndex = 3;
                    foreach (DataRow i in inv.Rows)
                    {
                        cogsExport.Cells[recordIndex, 1].Value = i[0].ToString();
                        cogsExport.Cells[recordIndex, 2].Value = i[2].ToString();
                        cogsExport.Cells[recordIndex, 3].Value = i[1].ToString();
                        if (Convert.ToInt32(i[4]) == 1)
                        {
                            cogsExport.Cells[recordIndex, 4].Value = i[3].ToString() + "%";
                        }
                        else
                        {
                            cogsExport.Cells[recordIndex, 4].Value = "$" + i[3].ToString();
                        }
                        cogsExport.Cells[recordIndex, 4].Style.Numberformat.Format = "0.0";


                        cogsExport.Cells[recordIndex, 5].Value = i[5].ToString() + "%";
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