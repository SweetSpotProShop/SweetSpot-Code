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
        CurrentUser CU = new CurrentUser();
        LocationManager LM = new LocationManager();
        Reports R = new Reports();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsHomePage";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (!IsPostBack)
                {
                    //Sets the calendar and text boxes start and end dates
                    calStartDate.SelectedDate = DateTime.Today;
                    calEndDate.SelectedDate = DateTime.Today;
                    ddlLocation.DataSource = LM.ReturnLocationDropDown();
                    ddlLocation.DataTextField = "locationName";
                    ddlLocation.DataValueField = "locationID";
                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = CU.locationID.ToString();

                }
                if (CU.jobID != 0)
                {
                    //User is not an admin
                    lblReport.Text = "You are not authorized to view reports";
                    lblReport.Visible = true;
                    lblReport.ForeColor = System.Drawing.Color.Red;
                    //Disables buttons
                    btnRunReport.Visible = false;
                    pnlDefaultButton.Visible = false;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //This is the Cashout Report
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSubmit_Click";
            try
            {
                if (calStartDate.SelectedDate != calEndDate.SelectedDate)
                {
                    //One of the date boxes is empty
                    lbldate.Visible = true;
                    lbldate.Text = "Please Select the same date for this report.";
                    lbldate.ForeColor = System.Drawing.Color.Red;

                }
                else
                {
                    //Stores report dates into Session
                    DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
                    Object[] repInfo = new Object[] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                    int indicator = R.verifyCashoutCanBeProcessed(repInfo);
                    //Check to see if there are sales first
                    if (indicator == 0)
                    {
                        Session["reportInfo"] = repInfo;
                        //Changes to the Reports Cash Out page
                        Response.Redirect("ReportsCashOut.aspx", false);
                    }
                    else if (indicator == 1)
                    {
                        MessageBox.ShowMessage("No transactions have been processed for selected date.", this);
                    }
                    else if (indicator == 2)
                    {
                        MessageBox.ShowMessage("A cashout has already been completed for selected date.", this);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnPurchasesReport_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnPurchasesReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }

        }
        //Change to add Error checking to this page prior to opening report
        protected void btnTaxReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnTesting_Click";
            try
            {
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsTaxes.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Change to add Error checking to this page prior to opening report
        protected void btnCOGSvsPMReport_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnCOGSvsPMReport_Click";
            try
            {
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;

                Response.Redirect("ReportsCOGSvsPM.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Change to add Error checking to this page prior to opening report
        protected void btnItemsSold_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnItemsSold_Click";
            try
            {
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsItemsSold.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Change to add Error checking to this page prior to opening report
        protected void btnMostSold_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnMostSold_Click";
            try
            {
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
                Object[] passing = new Object[2] { dtm, Convert.ToInt32(ddlLocation.SelectedValue) };
                Session["reportInfo"] = passing;
                Response.Redirect("ReportsMostSold.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnSalesByDate_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSalesByDate_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnPaymentsByDateReport_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnPaymentsByDateReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnTradeInsByDateReport_Click(object sendr, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnTradeInsByDateReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }

        protected void btnExtensiveInvoice_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnExtensiveInvoice_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = new DateTime[2] { calStartDate.SelectedDate, calEndDate.SelectedDate };
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
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