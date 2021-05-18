using SweetSpotDiscountGolfPOS.OB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.Misc;
using System.Threading;
using System.Data;
using SweetSpotDiscountGolfPOS.FP;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportingMPage : System.Web.UI.MasterPage
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly Reports R = new Reports();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session["currentUser"] = null;
            Response.Redirect("LoginPage.aspx", false);
        }

        protected void btnRerunReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnRerunReport_Click";
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
                    object[] repInfo = new object[] { GetDateRange(), Convert.ToInt32(ddlLocation.SelectedValue), Convert.ToInt32(ddlDatePeriod.SelectedValue) };
                    Session["reportInfo"] = repInfo;

                    if (Session["currPage"].ToString() == "ReportsStoreStats")
                    {
                        StoreStatsReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsTaxes")
                    {
                        TaxesReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsDiscounts")
                    {
                        DiscountsReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsSales")
                    {
                        SalesReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsExtensiveInvoice")
                    {
                        ExtensiveInvoiceReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsCostOfInventory")
                    {
                        CostOfInventoryReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsInventoryChange")
                    {
                        InventoryChangeReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsSpecificApparel")
                    {
                        SpecificApparelReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsSpecificgrip")
                    {
                        SpecificgripReRun(repInfo, objPageDetails);
                    }
                    else if (Session["currPage"].ToString() == "ReportsCashOut")
                    {
                        CashOutReRun(repInfo, objPageDetails);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }

        protected void StoreStatsReRun(object[] repInfo, object[] objPageDetails)
        {
            DateTime[] reportDates = (DateTime[])repInfo[0];
            DateTime startDate = reportDates[0];
            DateTime endDate = reportDates[1];
            if (startDate == endDate)
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Store stats on: " + startDate.ToString("dd/MMM/yy"); //+ " for " + LM.ReturnLocationName(locationID, objPageDetails);
            }
            else
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Store stats on: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy"); //+ " for " + LM.ReturnLocationName(locationID, objPageDetails);
            }
            //Binding the gridview
            DataTable stats = R.CallReturnStoreStats(startDate, endDate, Convert.ToInt32(repInfo[2]), Convert.ToInt32(repInfo[1]), objPageDetails);
            GridView GrdStats = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdStats");
            GrdStats.DataSource = stats;
            GrdStats.DataBind();
        }
        protected void TaxesReRun(object[] repInfo, object[] objPageDetails) { }
        protected void DiscountsReRun(object[] repInfo, object[] objPageDetails) { }
        protected void SalesReRun(object[] repInfo, object[] objPageDetails) { }
        protected void ExtensiveInvoiceReRun(object[] repInfo, object[] objPageDetails) { }
        protected void CostOfInventoryReRun(object[] repInfo, object[] objPageDetails) { }
        protected void InventoryChangeReRun(object[] repInfo, object[] objPageDetails) { }
        protected void SpecificApparelReRun(object[] repInfo, object[] objPageDetails) { }
        protected void SpecificgripReRun(object[] repInfo, object[] objPageDetails) { }
        protected void CashOutReRun(object[] repInfo, object[] objPageDetails) { }
        protected DateTime[] GetDateRange()
        {
            if (ddlDatePeriod.SelectedItem.Text.Equals("Month"))
            {
                return new DateTime[2] { CalStartDate.SelectedDate.GetMonthStart(), CalEndDate.SelectedDate.GetMonthEnd() };
            }
            else if (ddlDatePeriod.SelectedItem.Text.Equals("Week"))
            {
                return new DateTime[2] { CalStartDate.SelectedDate.GetWeekStart(), CalEndDate.SelectedDate.GetWeekEnd() };
            }
            else
            {
                return new DateTime[2] { CalStartDate.SelectedDate, CalEndDate.SelectedDate };
            }
        }
    }
}
