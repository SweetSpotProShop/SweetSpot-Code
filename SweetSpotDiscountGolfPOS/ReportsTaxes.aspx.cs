using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsTaxes : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double colGST;
        double colPST;
        double colLCT;
        double retGST;
        double retPST;
        double retLCT;
        double ovrGST;
        double ovrPST;
        double ovrLCT;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsTaxes.aspx";
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
                    object[] passing = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    //Builds string to display in label
                    lblTaxDate.Text = "Taxes Through: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy") + " for " + LM.CallReturnLocationName(Convert.ToInt32(passing[1]), objPageDetails);
                    //Creating a cashout list and calling a method that grabs all mops and amounts paid
                    List<TaxReport> taxReport = R.CallReturnTaxReportDetails(startDate, endDate, Convert.ToInt32(passing[1]), objPageDetails);

                    GrdTaxList.DataSource = taxReport;
                    GrdTaxList.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void PrintReport(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "PrintReport";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            //Current method does nothing
            try
            { }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");

                object[] passing = (object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])passing[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];

                string fileName = "Taxes Report-" + LM.CallReturnLocationName(Convert.ToInt32(passing[1]), objPageDetails) + "_" + startDate.ToShortDateString() + " - " + endDate.ToShortDateString() + ".xlsx";
                
                List<TaxReport> taxReport = R.CallReturnTaxReportDetails(startDate, endDate, Convert.ToInt32(passing[1]), objPageDetails);
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet taxes = xlPackage.Workbook.Worksheets.Add("Taxes");
                    //Writing       
                    taxes.Cells[1, 1].Value = lblTaxDate.Text;

                    taxes.Cells[2, 1].Value = "Date";

                    taxes.Cells[2, 2].Value = "GST Collected";
                    taxes.Cells[2, 3].Value = "PST Collected";
                    taxes.Cells[2, 4].Value = "LCT Collected";

                    taxes.Cells[2, 5].Value = "GST Returned";
                    taxes.Cells[2, 6].Value = "PST Returned";
                    taxes.Cells[2, 7].Value = "LCT Returned";

                    taxes.Cells[2, 8].Value = "GST Total";
                    taxes.Cells[2, 9].Value = "PST Total";
                    taxes.Cells[2, 10].Value = "LCT Total";

                    int recordIndex = 3;
                    if (taxReport.Count > 0)
                    {
                        foreach (TaxReport tr in taxReport)
                        {
                            taxes.Cells[recordIndex, 1].Value = tr.dtmInvoiceDate.ToShortDateString();
                            taxes.Cells[recordIndex, 2].Value = tr.fltGovernmentTaxAmountCollected;
                            taxes.Cells[recordIndex, 3].Value = tr.fltProvincialTaxAmountCollected;
                            taxes.Cells[recordIndex, 4].Value = tr.fltLiquorTaxAmountCollected;
                            taxes.Cells[recordIndex, 5].Value = tr.fltGovernmentTaxAmountReturned;
                            taxes.Cells[recordIndex, 6].Value = tr.fltProvincialTaxAmountReturned;
                            taxes.Cells[recordIndex, 7].Value = tr.fltLiquorTaxAmountReturned;
                            taxes.Cells[recordIndex, 8].Value = tr.fltGovernmentTaxAmountCollected + tr.fltGovernmentTaxAmountReturned;
                            taxes.Cells[recordIndex, 9].Value = tr.fltProvincialTaxAmountCollected + tr.fltProvincialTaxAmountReturned;
                            taxes.Cells[recordIndex, 10].Value = tr.fltLiquorTaxAmountCollected + tr.fltLiquorTaxAmountReturned;
                            recordIndex++;
                        }

                        taxes.Cells[recordIndex + 1, 1].Value = "Totals:";
                        taxes.Cells[recordIndex + 1, 2].Value = colGST;
                        taxes.Cells[recordIndex + 1, 3].Value = colPST;
                        taxes.Cells[recordIndex + 1, 4].Value = colLCT;
                        taxes.Cells[recordIndex + 1, 5].Value = retGST;
                        taxes.Cells[recordIndex + 1, 6].Value = retPST;
                        taxes.Cells[recordIndex + 1, 7].Value = retLCT;
                        taxes.Cells[recordIndex + 1, 8].Value = ovrGST;
                        taxes.Cells[recordIndex + 1, 9].Value = ovrPST;
                        taxes.Cells[recordIndex + 1, 10].Value = ovrLCT;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdTaxList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            string method = "GrdTaxesOverall_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                // check row type
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    colGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountCollected"));
                    colPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountCollected"));
                    colLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountCollected"));
                    retGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountReturned"));
                    retPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountReturned"));
                    retLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountReturned"));
                    ovrGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountReturned"));
                    ovrPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountReturned"));
                    ovrLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountReturned"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", colGST);
                    e.Row.Cells[2].Text = String.Format("{0:C}", colPST);
                    e.Row.Cells[3].Text = String.Format("{0:C}", colLCT);
                    e.Row.Cells[4].Text = String.Format("{0:C}", retGST);
                    e.Row.Cells[5].Text = String.Format("{0:C}", retPST);
                    e.Row.Cells[6].Text = String.Format("{0:C}", retLCT);
                    e.Row.Cells[7].Text = String.Format("{0:C}", ovrGST);
                    e.Row.Cells[8].Text = String.Format("{0:C}", ovrPST);
                    e.Row.Cells[9].Text = String.Format("{0:C}", ovrLCT);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}