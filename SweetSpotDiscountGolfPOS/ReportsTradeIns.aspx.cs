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
    public partial class ReportsTradeIns : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CustomMessageBox cmb = new CustomMessageBox();
        CurrentUser cu = new CurrentUser();
        LocationManager l = new LocationManager();
        double tradeInDollars;

        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsTradeIns";
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
                    cu = (CurrentUser)Session["currentUser"];
                    //Gathering the start and end dates
                    Object[] passing = (Object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " for " + l.locationName(locationID);
                    }
                    else
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                    }
                    dt = r.returnTradeInsForSelectedDate(passing);
                    grdTradeInsByDate.DataSource = dt;
                    grdTradeInsByDate.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                er.logError(ex, cu.empID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdTradeInsByDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                tradeInDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalTradeIns"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", tradeInDollars);
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
                Object[] passing = (Object[])Session["reportInfo"];
                string loc = l.locationName(Convert.ToInt32(passing[1]));
                string fileName = "Trade Ins Report by Date - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet salesExport = xlPackage.Workbook.Worksheets.Add("TradeIns");
                    // write to sheet   
                    salesExport.Cells[1, 1].Value = lblDates.Text;
                    salesExport.Cells[2, 1].Value = "Date";
                    salesExport.Cells[2, 2].Value = "Trade Ins Dollars";
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
    }
}