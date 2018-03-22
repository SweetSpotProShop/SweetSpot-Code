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
    public partial class ReportsCashOut : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();
        Reports R = new Reports();
        LocationManager L = new LocationManager();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsSales";
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
                    DateTime startDate = DateTime.Parse(Request.QueryString["from"].ToString());
                    DateTime endDate = DateTime.Parse(Request.QueryString["to"].ToString());
                    DateTime[] rptDate = { startDate, endDate };
                    int locationID = Convert.ToInt32(Request.QueryString["location"].ToString());
                    object[] passing = { rptDate, locationID };
                    lblDates.Text = "Items sold on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + L.locationName(locationID);
                    dt = R.ReturnCashoutsForSelectedDates(passing);
                    grdCashoutByDate.DataSource = dt;
                    grdCashoutByDate.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
                string loc = L.locationName(Convert.ToInt32(Request.QueryString["location"].ToString()));
                string fileName = "CashOut Report by Date - " + loc + ".xlsx";

                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet salesExport = xlPackage.Workbook.Worksheets.Add("CashOut");
                    // write to sheet   
                    salesExport.Cells[1, 1].Value = lblDates.Text;
                    salesExport.Cells[2, 1].Value = "Date";
                    salesExport.Cells[2, 2].Value = "Sales Dollars";
                    int recordIndex = 3;
                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime d = (DateTime)row[0];
                        salesExport.Cells[recordIndex, 1].Value = d.ToString("d");
                        salesExport.Cells[recordIndex, 2].Value = row[1].ToString();
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}