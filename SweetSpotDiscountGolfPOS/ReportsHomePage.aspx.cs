using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.FormulaParsing;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        LocationManager LM = new LocationManager();
        Reports R = new Reports();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsHomePage";
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
                        ddlLocation.DataSource = LM.ReturnLocationDropDown();
                        ddlLocation.DataTextField = "city";
                        ddlLocation.DataValueField = "locationID";
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = CU.location.locationID.ToString();
                    }
                    if (CU.jobID != 0)
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
        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calStart_SelectionChanged";
            try { }
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
        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calEnd_SelectionChanged";
            try { }
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
        //This is the Cashout Report
        protected void btnCashOutReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCashOutReport_Click";
            try
            {
                ////NEED TO UPDATE THIS FOR NEW CASHOUT REPORTING
                ////THIS WILL SHOW A LIST OF THE CASHOUTS THAT CAN THEN BE
                ////EDITED AND THEN FINALIZED
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                object[] repInfo = new object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.CashoutsProcessed(repInfo);
                ////Check to see if there are sales first
                if (indicator == 0)
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("from", dtm[0].ToString());
                    nameValues.Set("to", dtm[1].ToString());
                    nameValues.Set("location", ddlLocation.SelectedValue.ToString());
                    Response.Redirect("ReportsCashOut.aspx?" + nameValues, false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No CashOuts have been processed for selected date range.", this);
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
        protected void btnPurchasesReport_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnPurchasesReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] repInfo = new Object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.verifyPurchasesMade(repInfo);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsPurchasesMade.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No purchases have been processed for selected dates.", this);
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
        //Change to add Error checking to this page prior to opening report
        protected void btnTaxReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnTesting_Click";
            try
            {
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsTaxes.aspx", false);
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
        //Change to add Error checking to this page prior to opening report
        protected void btnCOGSvsPMReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnCOGSvsPMReport_Click";
            try
            {
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;

                Response.Redirect("ReportsCOGSvsPM.aspx", false);
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
        //Change to add Error checking to this page prior to opening report
        protected void btnItemsSold_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnItemsSold_Click";
            try
            {
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsItemsSold.aspx", false);
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
        //Change to add Error checking to this page prior to opening report
        protected void btnMostSold_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnMostSold_Click";
            try
            {
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsMostSold.aspx", false);
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
        //Change to add Error checking to this page prior to opening report
        protected void btnDiscountReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDiscountReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                //Changes to the Reports Cash Out page
                Response.Redirect("ReportsDiscounts.aspx", false);
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
        protected void btnSalesByDate_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSalesByDate_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] repInfo = new Object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.verifySalesHaveBeenMade(repInfo);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsSales.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Sales have been processed for selected dates.", this);
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
        protected void btnPaymentsByDateReport_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnPaymentsByDateReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] repInfo = new Object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                int indicator = R.verifySalesHaveBeenMade(repInfo);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Response.Redirect("ReportsPaymentType.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Sales have been processed for selected dates.", this);
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
        protected void btnTradeInsByDateReport_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnTradeInsByDateReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                int loc = Convert.ToInt32(ddlLocation.SelectedValue);
                Object[] repInfo = new Object[] { dtm, loc };
                int indicator = R.verifyTradeInsHaveBeenMade(repInfo);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Server.Transfer("ReportsTradeIns.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Trade Ins have been processed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnTesting_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnTesting_Click";
            //Method currently not used
            try
            {
                //string variable = " ";
                //Response.Write("<script>Request.QueryString("variable")</script>");
                //Label1.Text = variable;
                //ErrorReporting er = new ErrorReporting();
                //er.sendError("This is a test");        

                //string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                //string pathDownload = (pathUser + "\\Downloads\\");
                //FileInfo newFile = new FileInfo(pathDownload + "mynewfile.xlsx");
                //using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                //{
                //    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Test Sheet");
                //    // write to sheet
                //    worksheet.Cells[1, 1].Value = "Test";
                //    //xlPackage.SaveAs(aFile);

                //    Response.Clear();
                //    Response.AddHeader("content-disposition", "attachment; filename=test.xlsx");
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.BinaryWrite(xlPackage.GetAsByteArray());
                //    Response.End();
                //}
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
        protected void btnExtensiveInvoice_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnExtensiveInvoice_Click";
            try
            {

                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);

                int loc = Convert.ToInt32(ddlLocation.SelectedValue);
                Object[] repInfo = new Object[] { dtm, loc };
                int indicator = R.verifySalesHaveBeenMade(repInfo);
                //Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportInfo"] = repInfo;
                    Server.Transfer("ReportsExtensiveInvoice.aspx", false);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No sales have been processed for selected dates.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnCostOfInventory_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnCostOfInventory_Click";
            try
            {
                Server.Transfer("ReportsCostOfInventory.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
                //Server.Transfer(prevPage, false);
            }
        }       

        protected void btnStoreStatsReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnStoreStatsReport";
            try
            {
                DateTime[] dtm = getDateRange(calStartDate.SelectedDate, calEndDate.SelectedDate);
                Object[] passing = new Object[2] { dtm, ddlDatePeriod.SelectedItem.Text.ToString() };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsStoreStats.aspx", false);
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
        protected DateTime[] getDateRange(DateTime startDate, DateTime endDate)
        {
            if (ddlDatePeriod.SelectedItem.Text.Equals("Day"))
            { return new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate }; }
            else if (ddlDatePeriod.SelectedItem.Text.Equals("Default"))
            { return new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate }; }
            else if (ddlDatePeriod.SelectedItem.Text.Equals("Week"))
            { return new DateTime[2] { calStartDate.SelectedDate.GetWeekStart(), calEndDate.SelectedDate.GetWeekEnd() }; }
            else
            { return new DateTime[2] { calStartDate.SelectedDate.GetMonthStart(), calEndDate.SelectedDate.GetMonthEnd() }; }
        }
    }
}