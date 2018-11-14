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
    public partial class ReportsSpecificApparel : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        double pmCost;
        double pmPrice;
        int pmQuantity;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsSpecificApparel";
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
                    //Builds string to display in label
                    if (startDate == endDate) { lblDates.Text = "Apparel sold through: " + startDate.ToString("d"); }
                    else { lblDates.Text = "Apparel sold through: " + startDate.ToString("d") + " to " + endDate.ToString("d"); }
                    grdStats.DataSource = R.returnSpecificApparelDataTableForReport(startDate, endDate);
                    grdStats.DataBind();
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

        protected void grdStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                pmPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem,"overallPrice"));
                pmCost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "overallCost"));
                pmQuantity += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "overallQuantity"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = String.Format("{0:N0}", pmQuantity);
                e.Row.Cells[4].Text = String.Format("{0:C}", pmCost / pmQuantity);
                e.Row.Cells[5].Text = String.Format("{0:C}", pmPrice /pmQuantity);
                e.Row.Cells[6].Text = String.Format("{0:C}", pmCost);
                e.Row.Cells[7].Text = String.Format("{0:P2}", (pmPrice - pmCost) / pmPrice);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            try
            {
                MessageBox.ShowMessage("Download for this report is currently not available.", this);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}