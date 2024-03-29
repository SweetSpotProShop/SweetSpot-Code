﻿using System;
using System.Threading;
using System.IO;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;
using System.Collections.Generic;
using System.Data;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly ImportExport IE = new ImportExport();
        readonly Reports R = new Reports();
        CurrentUser CU;

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
                        CalStartDate.SelectedDate = DateTime.Today;
                        CalEndDate.SelectedDate = DateTime.Today;
                        DataTable dt = LM.CallReturnLocationDropDown(objPageDetails);
                        dt.Rows.Add(99, "All Locations");
                        ddlLocation.DataSource = dt;
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
                        BtnCashOutReport.Visible = false;
                        pnlDefaultButton.Visible = false;
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void CalStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calStart_SelectionChanged";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try { }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void CalEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calEnd_SelectionChanged";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try { }
            //Exception catch
            catch (ThreadAbortException) { }
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

        //This is the Cashout Report
        protected void BtnCashOutReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCashOutReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 1, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                //Stores report dates into Session
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.CashoutsProcessed(repInfo, objPageDetails);
                ////Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsCashOut.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No CashOuts have been processed for selected date range.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Displays taxes charged
        protected void BtnTaxReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnTesting_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 3, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifyTaxesCharged(repInfo, objPageDetails);
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsTaxes.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("Taxes have not been charged for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Displays the Discounts given
        protected void BtnDiscountReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDiscountReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 6, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                //Stores report dates into Session
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifyInvoicesCompleted(repInfo, objPageDetails);
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsDiscounts.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No Discounts have been given for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Displays sales totals grouped by date
        protected void BtnSalesByDate_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSalesByDate_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 7, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                //Stores report dates into Session
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifySalesHaveBeenMade(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSales.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No Sales have been completed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Similar to the COGSvsPM report with a little more detail
        //only fixed download
        protected void BtnExtensiveInvoice_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnExtensiveInvoice_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 11, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                int loc = Convert.ToInt32(ddlLocation.SelectedValue);
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifySalesHaveBeenMade(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsExtensiveInvoice.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No sales have been processed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }
        //Displays the total cost of currently stocked inventory
        //only fixed download
        protected void BtnCostOfInventory_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnCostOfInventory_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 12, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation(dtm[0], dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                Session["reportInfo"] = repInfo;
                Response.Redirect("ReportsCostOfInventory.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }
        //Another report similar to COGSvsPM and Extensive Invoice, can be broken out by month, week, or day.
        //only fixed download
        protected void BtnStoreStatsReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnStoreStatsReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 13, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifyStatsAvailable(repInfo, objPageDetails);
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsStoreStats.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No sales have been processed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Displays chnages made to inventory items in a date range
        //only fixed download
        protected void BtnInventoryChangeReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnInventoryChangeReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 16, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifyInventoryChange(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsInventoryChange.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No changes to Invnetory for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Displays specific apparel skus sold, their average cost, average price, and profit margin
        //only fixed download
        protected void BtnSpecificApparelReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSpecificApparelReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 14, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifySpecificApparel(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSpecificApparel.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No sales of Specific Apparel Items for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        //Displays specific apparel skus sold, their average cost, average price, and profit margin
        //only fixed download
        protected void BtnSpecificGripReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSpecificGripReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                object[] reportLog = { 15, CU.employee.intEmployeeID, CU.location.intLocationID };
                R.CallReportLogger(reportLog, objPageDetails);
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation((DateTime)dtm[0], (DateTime)dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                int indicator = R.VerifySpecificGrip(repInfo, objPageDetails);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSpecificgrip.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBoxCustom.ShowMessage("No sales of Specific Grip Items for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void BtnExportInvoices_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExportInvoices_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {

                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                DateTime[] dtm = GetDateRange();
                DateTime startDate = dtm[0];
                DateTime endDate = dtm[1];

                string filename = "Invoices-" + startDate.ToString("dd.MM.yyyy") + " To " + endDate.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);

                IE.CallExportInvoiceDateRange(dtm, newFile, filename);

                MessageBoxCustom.ShowMessage("Invoices between " + startDate.ToString("dd.MM.yyyy") + " and " + endDate.ToString("dd.MM.yyyy") + " have been exported.", this);
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
        protected void BtnCreatePDFFiles_Click(object sender, EventArgs e)
        {
            string method = "BtnCreatePDFFiles";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceManager IM = new InvoiceManager();
                CashoutUtilities COU = new CashoutUtilities();
                //PdfCustomerInvoice pdf = new PdfCustomerInvoice();
                //List<int> invoices = IM.CallListofInvoicesForDayForLocation(CalStartDate.SelectedDate, CalEndDate.SelectedDate, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                //foreach (int inv in invoices)
                List<int> locations = new List<int> { 1, 2, 8 };
                //int loc = 0;
                List<DateTime> selectedDates = new List<DateTime>();
                for (DateTime date = CalStartDate.SelectedDate; date <= CalEndDate.SelectedDate; date = date.AddDays(1))
                {
                    selectedDates.Add(date);
                }
                foreach (int loc in locations)
                {
                    foreach (DateTime dtm in selectedDates)
                    {
                        //pdf.GenerateInvoiceSaveFile(IM.CallReturnInvoice(Convert.ToInt32(inv), objPageDetails)[0], objPageDetails);
                        COU.CollectAndStoreDailySalesData(dtm, loc, objPageDetails);
                    }
                }
                //MessageBoxCustom.ShowMessage("PDFs between " + CalStartDate.SelectedDate.ToString("dd.MM.yyyy") + " and " + CalEndDate.SelectedDate.ToString("dd.MM.yyyy") + " have been created.", this);
                MessageBoxCustom.ShowMessage("Daily Sales data between " + CalStartDate.SelectedDate.ToString("dd.MM.yyyy") + " and " + CalEndDate.SelectedDate.ToString("dd.MM.yyyy") + " have been added to database.", this);
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
        //protected void btnUpdatingTaxes_Click(object sender, EventArgs e)
        //{
        //    string method = "btnUpdatingTaxes";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        TemporaryItems TI = new TemporaryItems();
        //        TI.UpdateTaxesInTaxTable(CalStartDate.SelectedDate, CalEndDate.SelectedDate, objPageDetails);

        //        MessageBoxCustom.ShowMessage("Taxes have been updated for " + CalStartDate.SelectedDate.ToString("dd.MM.yyyy") + " and " + CalEndDate.SelectedDate.ToString("dd.MM.yyyy") + ".", this);
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}

        protected void BtnCharlynReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCharlynReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                DateTime[] dtm = GetDateRange();
                string locationName = "All Locations";
                if (Convert.ToInt32(ddlLocation.SelectedValue) != 99)
                {
                    locationName = LM.CallReturnLocationName(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                }
                ReportInformation repInfo = new ReportInformation(dtm[0], dtm[1], Convert.ToInt32(ddlDatePeriod.SelectedValue), Convert.ToInt32(ddlLocation.SelectedValue), locationName);
                Session["reportInfo"] = repInfo;
                Response.Redirect("ReportsCharlynSales.aspx", false);
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