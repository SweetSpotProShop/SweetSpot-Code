using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Threading;
using System.Web;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        LocationManager LM = new LocationManager();
        Reports R = new Reports();

        //Add counter to record how many times each report gets viewed.

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsHomePage";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    if (!IsPostBack)
                    {
                        //Sets the calendar and text boxes start and end dates
                        calStartDate.SelectedDate = DateTime.Today;
                        calEndDate.SelectedDate = DateTime.Today;
                        ddlLocation.DataSource = LM.ReturnLocationDropDown(objPageDetails);
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = CU.location.intLocationID.ToString();
                    }
                    if (CU.employee.intJobID != 0)
                    {
                        //User is not an admin
                        lblReport.Text = "You are not authorized to view reports";
                        lblReport.Visible = true;
                        lblReport.ForeColor = System.Drawing.Color.Red;
                        //Disables buttons
                        btnCashOutReport.Visible = false;
                        pnlDefaultButton.Visible = false;
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calStart_SelectionChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try { }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calEnd_SelectionChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try { }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //This is the Cashout Report
        protected void btnCashOutReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCashOutReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 1, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.CashoutsProcessed(repInfo, objPageDetails);
                ////Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsCashOut.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No CashOuts have been processed for selected date range.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        
        //Displays taxes charged
        protected void btnTaxReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnTesting_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 3, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.verifyTaxesCharged(repInfo, objPageDetails);
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsTaxes.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("Taxes have not been charged for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Displays the Discounts given
        protected void btnDiscountReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDiscountReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 6, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.verifyInvoicesCompleted(repInfo, objPageDetails);
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsDiscounts.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Discounts have been given for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Displays sales totals grouped by date
        protected void btnSalesByDate_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSalesByDate_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 7, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.verifySalesHaveBeenMade(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSales.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Sales have been completed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Similar to the COGSvsPM report with a little more detail
        protected void btnExtensiveInvoice_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnExtensiveInvoice_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 11, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                int loc = Convert.ToInt32(ddlLocation.SelectedValue);
                object[] repInfo = new object[] { dtm, loc };
                int indicator = R.verifySalesHaveBeenMade(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsExtensiveInvoice.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No sales have been processed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }

        //Displays the total cost of currently stocked inventory
        protected void btnCostOfInventory_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnCostOfInventory_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 12, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                Response.Redirect("ReportsCostOfInventory.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }

        //Another report similar to COGSvsPM and Extensive Invoice, can be broken out by month, week, or day.
        protected void btnStoreStatsReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnStoreStatsReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 13, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm, ddlDatePeriod.SelectedValue.ToString() };
                int indicator = R.verifyStatsAvailable(repInfo, objPageDetails);
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsStoreStats.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No sales have been processed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Displays chnages made to inventory items in a date range
        protected void btnInventoryChangeReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnInventoryChangeReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 16, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm };
                int indicator = R.verifyInventoryChange(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsInventoryChange.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No changes to Invnetory for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Displays specific apparel skus sold, their average cost, average price, and profit margin
        protected void btnSpecificApparelReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSpecificApparelReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 14, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm };
                int indicator = R.verifySpecificApparel(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSpecificApparel.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No sales of Specific Apparel Items for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Displays specific apparel skus sold, their average cost, average price, and profit margin
        protected void btnSpecificGripReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSpecificGripReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 15, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm };
                int indicator = R.verifySpecificGrip(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSpecificgrip.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No sales of Specific Grip Items for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected DateTime[] getDateRange(DateTime startDate, DateTime endDate)
        {
            if (ddlDatePeriod.SelectedItem.Text.Equals("Month"))
            {
                return new DateTime[2] { calStartDate.SelectedDate.GetMonthStart(), calEndDate.SelectedDate.GetMonthEnd() };
            }
            else if (ddlDatePeriod.SelectedItem.Text.Equals("Week"))
            {
                return new DateTime[2] { calStartDate.SelectedDate.GetWeekStart(), calEndDate.SelectedDate.GetWeekEnd() };
            }
            else
            {
                return new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
            }
        }


        ////Displays Cost of Sold Items and a Profit margin calulation
        //protected void btnCOGSvsPMReport_Click(object sender, EventArgs e)
        //{
        //    //Collects current method and page for error tracking
        //    string method = "btnCOGSvsPMReport_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        object[] reportLog = { 4, CU.emp.employeeID, CU.location.locationID };
        //        R.CallReportLogger(reportLog, objPageDetails);
        //        DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
        //        object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
        //        int indicator = R.verifyInvoicesCompleted(repInfo, objPageDetails);
        //        if (indicator == 0)
        //        {
        //            Session["reportInfo"] = repInfo;
        //            Response.Redirect("ReportsCOGSvsPM.aspx", false);
        //        }
        //        else if (indicator == 1)
        //        {
        //            MessageBox.ShowMessage("No sales have been completed for selected dates.", this);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        ////Almost identical to the COGSvsPM Report, instead of PM shows profit in dollars
        //protected void btnItemsSold_Click(object sender, EventArgs e)
        //{
        //    //Collects current method and page for error tracking
        //    string method = "btnItemsSold_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        object[] reportLog = { 5, CU.emp.employeeID, CU.location.locationID };
        //        R.CallReportLogger(reportLog, objPageDetails);
        //        DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
        //        object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
        //        int indicator = R.verifyInvoicesCompleted(repInfo, objPageDetails);
        //        if (indicator == 0)
        //        {
        //            Session["reportInfo"] = repInfo;
        //            Response.Redirect("ReportsItemsSold.aspx", false);
        //        }
        //        else if (indicator == 1)
        //        {
        //            MessageBox.ShowMessage("No sales have been completed for selected dates.", this);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        ////Displays purchases made
        //protected void btnPurchasesReport_Click(object sendr, EventArgs e)
        //{
        //    //Collects current method and page for error tracking
        //    string method = "btnPurchasesReport_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        object[] reportLog = { 2, CU.emp.employeeID, CU.location.locationID };
        //        R.CallReportLogger(reportLog, objPageDetails);
        //        //Stores report dates into Session
        //        DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
        //        object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
        //        int indicator = R.verifyPurchasesMade(repInfo, objPageDetails);
        //        //Check to see if there are sales first
        //        if (indicator == 0)
        //        {
        //            Session["reportInfo"] = repInfo;
        //            Response.Redirect("ReportsPurchasesMade.aspx", false);
        //        }
        //        else if (indicator == 1)
        //        {
        //            MessageBox.ShowMessage("No purchases have been completed for selected dates.", this);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }

        //}
        ////Displays the totals for accepted payment methods
        //protected void btnPaymentsByDateReport_Click(object sendr, EventArgs e)
        //{
        //    //Collects current method and page for error tracking
        //    string method = "btnPaymentsByDateReport_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        object[] reportLog = { 9, CU.emp.employeeID, CU.location.locationID };
        //        R.CallReportLogger(reportLog, objPageDetails);
        //        //Stores report dates into Session
        //        DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
        //        object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
        //        int indicator = R.verifySalesHaveBeenMade(repInfo, objPageDetails);
        //        //Check to see if there are sales first
        //        if (indicator == 0)
        //        {
        //            Session["reportInfo"] = repInfo;
        //            Response.Redirect("ReportsPaymentType.aspx", false);
        //        }
        //        else if (indicator == 1)
        //        {
        //            MessageBox.ShowMessage("No Sales have been completed for selected dates.", this);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        ////Displays dollar value of trade ins accepted
        //protected void btnTradeInsByDateReport_Click(object sendr, EventArgs e)
        //{
        //    //Collects current method and page for error tracking
        //    string method = "btnTradeInsByDateReport_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        object[] reportLog = { 10, CU.emp.employeeID, CU.location.locationID };
        //        R.CallReportLogger(reportLog, objPageDetails);
        //        //Stores report dates into Session
        //        DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
        //        int loc = Convert.ToInt32(ddlLocation.SelectedValue);
        //        object[] repInfo = new object[] { dtm, loc };
        //        int indicator = R.verifyTradeInsHaveBeenMade(repInfo, objPageDetails);
        //        //Check to see if there are sales first
        //        if (indicator == 0)
        //        {
        //            Session["reportInfo"] = repInfo;
        //            Server.Transfer("ReportsTradeIns.aspx", false);
        //        }
        //        else if (indicator == 1)
        //        {
        //            MessageBox.ShowMessage("No Trade Ins have been processed for selected dates.", this);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
        //        //string prevPage = Convert.ToString(Session["prevPage"]);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //        //Server.Transfer(prevPage, false);
        //    }
        //}
        ////Displays the Top 10 SKUs, Brands, and Models sold
        //protected void btnMostSold_Click(object sender, EventArgs e)
        //{
        //    //Collects current method and page for error tracking
        //    string method = "btnMostSold_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        object[] reportLog = { 8, CU.emp.employeeID, CU.location.locationID };
        //        R.CallReportLogger(reportLog, objPageDetails);
        //        DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
        //        object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
        //        int indicator = R.verifyInvoicesCompleted(repInfo, objPageDetails);
        //        if (indicator == 0)
        //        {
        //            Session["reportInfo"] = repInfo;
        //            Response.Redirect("ReportsMostSold.aspx", false);
        //        }
        //        else if (indicator == 1)
        //        {
        //            MessageBox.ShowMessage("No sales have been completed for selected dates.", this);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
    }
}