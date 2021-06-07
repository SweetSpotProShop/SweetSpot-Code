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
        readonly LocationManager LM = new LocationManager();
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
        protected void BtnRerunReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "BtnRerunReport_Click";
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
                    DateTime[] reportDates = GetDateRange();
                    string locationName = "All Locations";
                    if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                    {
                        locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    }
                    ReportInformation repInfo = new ReportInformation((DateTime)reportDates[0], (DateTime)reportDates[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
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
                        SpecificGripReRun(repInfo, objPageDetails);
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }

        protected void StoreStatsReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "StoreStatsReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Store Stats through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                //Binding the gridview
                DataTable stats = R.CallReturnStoreStats(repInfo, objPageDetails);
                GridView GrdStats = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdStats");
                GrdStats.DataSource = stats;
                GrdStats.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void TaxesReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "TaxesReRun";
            try
            {
                //Builds string to display in label
                Label lblTaxDate = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblTaxDate");
                lblTaxDate.Text = "Taxes Through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;
                
                //Creating a cashout list and calling a method that grabs all mops and amounts paid
                List<TaxReport> taxReport = R.CallReturnTaxReportDetails(repInfo, objPageDetails);
                GridView GrdTaxList = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdTaxList");
                GrdTaxList.DataSource = taxReport;
                GrdTaxList.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void DiscountsReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "DiscountsReRun";
            try
            {
                Label lblReportDate = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblReportDate");
                lblReportDate.Text = "Discount Report through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                GridView GrdInvoiceDisplay = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdInvoiceDisplay");
                DataTable discounts = R.CallReturnDiscountsBetweenDates(repInfo, objPageDetails);
                GrdInvoiceDisplay.DataSource = discounts;
                GrdInvoiceDisplay.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void SalesReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "SalesReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Items Sold through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                GridView GrdSalesByDate = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdSalesByDate");
                DataTable dt = R.CallReturnSalesForSelectedDate(repInfo, objPageDetails);
                GrdSalesByDate.DataSource = dt;
                GrdSalesByDate.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void ExtensiveInvoiceReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "ExtensiveInvoiceReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Extensive Invoice Report through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                DataTable invoices2 = R.CallReturnExtensiveInvoices2(repInfo, objPageDetails);
                GridView GrdInvoices = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdInvoices");
                GrdInvoices.DataSource = invoices2;
                GrdInvoices.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void CostOfInventoryReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "CostOfInventoryReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Cost of Inventory through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                DataTable list = list = R.CallCostOfInventoryReport(repInfo, objPageDetails);
                GridView grdCostOfInventory = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "grdCostOfInventory");
                grdCostOfInventory.DataSource = list;
                grdCostOfInventory.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void InventoryChangeReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "InventoryChangeReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Changes in Inventory through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                GridView grdStats = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "grdStats");
                grdStats.DataSource = R.CallReturnChangedInventoryForDateRange(repInfo, objPageDetails);
                grdStats.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void SpecificApparelReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "SpecificApparelReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Apparel Sold through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                GridView GrdStats = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdStats");
                GrdStats.DataSource = R.CallReturnSpecificApparelDataTableForReport(repInfo, objPageDetails);
                GrdStats.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void SpecificGripReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "SpecificGripReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Grips Sold through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                GridView GrdStats = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GtdStats");
                GrdStats.DataSource = R.CallReturnSpecificGripDataTableForReport(repInfo, objPageDetails);
                GrdStats.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
        protected void CashOutReRun(ReportInformation repInfo, object[] objPageDetails)
        {
            string method = "CashOutReRun";
            try
            {
                Label lblDates = (Label)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "lblDates");
                lblDates.Text = "Cashouts through: " + repInfo.dtmStartDate.ToString("dd/MMM/yy") + " to " + repInfo.dtmEndDate.ToString("dd/MMM/yy") + " for " + repInfo.varLocationName;

                DataTable dt = R.CallReturnCashoutsForSelectedDates(repInfo, objPageDetails);
                GridView GrdCashoutByDate = (GridView)CustomExtensions.CallFindControlRecursive(IndividualPageContent, "GrdCashoutByDate");
                GrdCashoutByDate.DataSource = dt;
                GrdCashoutByDate.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V4", method, this.Page);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this.Page);
            }
        }
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
