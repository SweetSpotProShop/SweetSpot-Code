using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsSpecificGrip : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double pmCost;
        double pmPrice;
        int pmQuantity;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsSpecificGrip";
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
                    lblDates.Text = "Grips sold through: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString();
                    GrdStats.DataSource = R.CallReturnSpecificGripDataTableForReport(repInfo, objPageDetails);
                    GrdStats.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdStats_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdStats_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    pmPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltOverallPrice"));
                    pmCost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltOverallCost"));
                    pmQuantity += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "intOverallQuantity"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[3].Text = String.Format("{0:N0}", pmQuantity);
                    e.Row.Cells[4].Text = String.Format("{0:C}", pmCost / pmQuantity);
                    e.Row.Cells[5].Text = String.Format("{0:C}", pmPrice / pmQuantity);
                    e.Row.Cells[6].Text = String.Format("{0:C}", pmCost);
                    e.Row.Cells[7].Text = String.Format("{0:P2}", (pmPrice - pmCost) / pmPrice);
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
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                MessageBoxCustom.ShowMessage("Download for this report is currently not available.", this);
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