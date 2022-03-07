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
using System.Data;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsTaxes : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double colGST;
        double colHST;
        double colLCT;
        double colPST;
        double colQST;
        double colRST;

        double retGST;
        double retHST;
        double retLCT;
        double retPST;
        double retQST;
        double retRST;

        double ovrGST;
        double ovrHST;        
        double ovrLCT;
        double ovrPST;
        double ovrQST;
        double ovrRST;

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
                    if (!IsPostBack)
                    {
                        CU = (CurrentUser)Session["currentUser"];
                        //Gathering the start and end dates
                        ReportInformation repInfo = (ReportInformation)Session["reportInfo"];

                        //Calendar calStartDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalStartDate");
                        //calStartDate.SelectedDate = repInfo.dtmStartDate;
                        //Calendar calEndDate = (Calendar)CustomExtensions.CallFindControlRecursive(Master, "CalEndDate");
                        //calEndDate.SelectedDate = repInfo.dtmEndDate;
                        //DropDownList ddlDatePeriod = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlDatePeriod");
                        //ddlDatePeriod.SelectedValue = repInfo.intGroupTimeFrame.ToString();
                        //DropDownList ddlLocation = (DropDownList)CustomExtensions.CallFindControlRecursive(Master, "ddlLocation");
                        //DataTable dt = LM.CallReturnLocationDropDown(objPageDetails);
                        //dt.Rows.Add(99, "All Locations");
                        //ddlLocation.DataSource = dt;
                        //ddlLocation.DataBind();
                        //ddlLocation.SelectedValue = repInfo.intLocationID.ToString();

                        //Builds string to display in label
                        lblTaxDate.Text = "Taxes Through: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                        //Creating a cashout list and calling a method that grabs all mops and amounts paid
                        List<TaxReport> taxReport = R.CallReturnTaxReportDetails(repInfo, objPageDetails);
                        GrdTaxList.DataSource = taxReport;
                        GrdTaxList.DataBind();
                    }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");

                ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                string fileName = "Taxes Report-" + repInfo.varLocationName + "_" + repInfo.dtmStartDate.ToShortDateString() + " - " + repInfo.dtmEndDate.ToShortDateString() + ".xlsx";
                
                List<TaxReport> taxReport = R.CallReturnTaxReportDetails(repInfo, objPageDetails);
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet taxes = xlPackage.Workbook.Worksheets.Add("Taxes");
                    //Writing       
                    taxes.Cells[1, 1].Value = lblTaxDate.Text;

                    taxes.Cells[2, 1].Value = "Date";

                    taxes.Cells[2, 2].Value = "GST Collected";
                    taxes.Cells[2, 3].Value = "HST Collected";
                    taxes.Cells[2, 4].Value = "LCT Collected";
                    taxes.Cells[2, 5].Value = "PST Collected";
                    taxes.Cells[2, 6].Value = "QST Collected";
                    taxes.Cells[2, 7].Value = "RST Collected";

                    taxes.Cells[2, 8].Value = "GST Returned";
                    taxes.Cells[2, 9].Value = "HST Returned";
                    taxes.Cells[2, 10].Value = "LCT Returned";
                    taxes.Cells[2, 11].Value = "PST Returned";
                    taxes.Cells[2, 12].Value = "QST Returned";
                    taxes.Cells[2, 13].Value = "RST Returned";

                    taxes.Cells[2, 14].Value = "GST Total";
                    taxes.Cells[2, 15].Value = "HST Total";
                    taxes.Cells[2, 16].Value = "LCT Total";
                    taxes.Cells[2, 17].Value = "PST Total";
                    taxes.Cells[2, 18].Value = "QST Total";
                    taxes.Cells[2, 19].Value = "RST Total";

                    int recordIndex = 3;
                    if (taxReport.Count > 0)
                    {
                        foreach (TaxReport tr in taxReport)
                        {
                            taxes.Cells[recordIndex, 1].Value = tr.dtmInvoiceDate.ToShortDateString();
                            taxes.Cells[recordIndex, 2].Value = tr.fltGovernmentTaxAmountCollected;
                            taxes.Cells[recordIndex, 3].Value = tr.fltHarmonizedTaxAmountCollected;
                            taxes.Cells[recordIndex, 4].Value = tr.fltLiquorTaxAmountCollected;
                            taxes.Cells[recordIndex, 5].Value = tr.fltProvincialTaxAmountCollected;
                            taxes.Cells[recordIndex, 6].Value = tr.fltQuebecTaxAmountCollected;
                            taxes.Cells[recordIndex, 7].Value = tr.fltRetailTaxAmountCollected;

                            taxes.Cells[recordIndex, 8].Value = tr.fltGovernmentTaxAmountReturned;
                            taxes.Cells[recordIndex, 9].Value = tr.fltHarmonizedTaxAmountReturned;
                            taxes.Cells[recordIndex, 10].Value = tr.fltLiquorTaxAmountReturned;
                            taxes.Cells[recordIndex, 11].Value = tr.fltProvincialTaxAmountReturned;
                            taxes.Cells[recordIndex, 12].Value = tr.fltQuebecTaxAmountReturned;
                            taxes.Cells[recordIndex, 13].Value = tr.fltRetailTaxAmountReturned;

                            taxes.Cells[recordIndex, 14].Value = tr.fltGovernmentTaxAmountCollected + tr.fltGovernmentTaxAmountReturned;
                            taxes.Cells[recordIndex, 15].Value = tr.fltHarmonizedTaxAmountCollected + tr.fltHarmonizedTaxAmountReturned;
                            taxes.Cells[recordIndex, 16].Value = tr.fltLiquorTaxAmountCollected + tr.fltLiquorTaxAmountReturned;
                            taxes.Cells[recordIndex, 17].Value = tr.fltProvincialTaxAmountCollected + tr.fltProvincialTaxAmountReturned;
                            taxes.Cells[recordIndex, 18].Value = tr.fltQuebecTaxAmountCollected + tr.fltQuebecTaxAmountReturned;
                            taxes.Cells[recordIndex, 19].Value = tr.fltRetailTaxAmountCollected + tr.fltRetailTaxAmountReturned;
                            recordIndex++;
                        }

                        taxes.Cells[recordIndex + 1, 1].Value = "Totals:";
                        taxes.Cells[recordIndex + 1, 2].Value = colGST;
                        taxes.Cells[recordIndex + 1, 3].Value = colHST;
                        taxes.Cells[recordIndex + 1, 4].Value = colLCT;
                        taxes.Cells[recordIndex + 1, 5].Value = colPST;
                        taxes.Cells[recordIndex + 1, 6].Value = colQST;
                        taxes.Cells[recordIndex + 1, 7].Value = colRST;

                        taxes.Cells[recordIndex + 1, 8].Value = retGST;
                        taxes.Cells[recordIndex + 1, 9].Value = retHST;
                        taxes.Cells[recordIndex + 1, 10].Value = retLCT;
                        taxes.Cells[recordIndex + 1, 11].Value = retPST;
                        taxes.Cells[recordIndex + 1, 12].Value = retQST;
                        taxes.Cells[recordIndex + 1, 13].Value = retRST;

                        taxes.Cells[recordIndex + 1, 14].Value = ovrGST;
                        taxes.Cells[recordIndex + 1, 15].Value = ovrHST;
                        taxes.Cells[recordIndex + 1, 16].Value = ovrLCT;
                        taxes.Cells[recordIndex + 1, 17].Value = ovrPST;
                        taxes.Cells[recordIndex + 1, 18].Value = ovrQST;
                        taxes.Cells[recordIndex + 1, 19].Value = ovrRST;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                    colHST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltHarmonizedTaxAmountCollected"));
                    colLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountCollected"));
                    colPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountCollected"));
                    colQST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltQuebecTaxAmountCollected"));
                    colRST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltRetailTaxAmountCollected"));

                    retGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountReturned"));
                    retHST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltHarmonizedTaxAmountReturned"));
                    retLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountReturned"));
                    retPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountReturned"));
                    retQST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltQuebecTaxAmountReturned"));
                    retRST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltRetailTaxAmountReturned"));

                    ovrGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmountReturned"));
                    ovrHST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltHarmonizedTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltHarmonizedTaxAmountReturned"));
                    ovrLCT += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmountReturned"));
                    ovrPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmountReturned"));
                    ovrQST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltQuebecTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltQuebecTaxAmountReturned"));
                    ovrRST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltRetailTaxAmountCollected")) + Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltRetailTaxAmountReturned"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = String.Format("{0:C}", colGST);
                    e.Row.Cells[2].Text = String.Format("{0:C}", colHST);
                    e.Row.Cells[3].Text = String.Format("{0:C}", colLCT);
                    e.Row.Cells[4].Text = String.Format("{0:C}", colPST);
                    e.Row.Cells[5].Text = String.Format("{0:C}", colQST);
                    e.Row.Cells[6].Text = String.Format("{0:C}", colRST);

                    e.Row.Cells[7].Text = String.Format("{0:C}", retGST);
                    e.Row.Cells[8].Text = String.Format("{0:C}", retHST);
                    e.Row.Cells[9].Text = String.Format("{0:C}", retLCT);
                    e.Row.Cells[10].Text = String.Format("{0:C}", retPST);
                    e.Row.Cells[11].Text = String.Format("{0:C}", retQST);
                    e.Row.Cells[12].Text = String.Format("{0:C}", retRST);

                    e.Row.Cells[13].Text = String.Format("{0:C}", ovrGST);
                    e.Row.Cells[14].Text = String.Format("{0:C}", ovrHST);
                    e.Row.Cells[15].Text = String.Format("{0:C}", ovrLCT);
                    e.Row.Cells[16].Text = String.Format("{0:C}", ovrPST);
                    e.Row.Cells[17].Text = String.Format("{0:C}", ovrQST);
                    e.Row.Cells[18].Text = String.Format("{0:C}", ovrRST);

                    if (colGST == 0)
                    {
                        GrdTaxList.Columns[1].Visible = false;
                    }
                    if (colHST == 0)
                    {
                        GrdTaxList.Columns[2].Visible = false;
                    }
                    if (colLCT == 0)
                    {
                        GrdTaxList.Columns[3].Visible = false;
                    }
                    if (colPST == 0)
                    {
                        GrdTaxList.Columns[4].Visible = false;
                    }
                    if (colQST == 0)
                    {
                        GrdTaxList.Columns[5].Visible = false;
                    }
                    if (colRST == 0)
                    {
                        GrdTaxList.Columns[6].Visible = false;
                    }

                    if (retGST == 0)
                    {
                        GrdTaxList.Columns[7].Visible = false;
                    }
                    if (retHST == 0)
                    {
                        GrdTaxList.Columns[8].Visible = false;
                    }
                    if (retLCT == 0)
                    {
                        GrdTaxList.Columns[9].Visible = false;
                    }
                    if (retPST == 0)
                    {
                        GrdTaxList.Columns[10].Visible = false;
                    }
                    if (retQST == 0)
                    {
                        GrdTaxList.Columns[11].Visible = false;
                    }
                    if (retRST == 0)
                    {
                        GrdTaxList.Columns[12].Visible = false;
                    }

                    if (ovrGST == 0)
                    {
                        GrdTaxList.Columns[13].Visible = false;
                    }
                    if (ovrHST == 0)
                    {
                        GrdTaxList.Columns[14].Visible = false;
                    }
                    if (ovrLCT == 0)
                    {
                        GrdTaxList.Columns[15].Visible = false;
                    }
                    if (ovrPST == 0)
                    {
                        GrdTaxList.Columns[16].Visible = false;
                    }
                    if (ovrQST == 0)
                    {
                        GrdTaxList.Columns[17].Visible = false;
                    }
                    if (ovrRST == 0)
                    {
                        GrdTaxList.Columns[18].Visible = false;
                    }
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
    }
}