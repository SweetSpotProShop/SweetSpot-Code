﻿using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsExtensiveInvoice : System.Web.UI.Page
    {

        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        double shipping;
        double tradein;
        double discount;
        double subTotal;
        double totalSales;
        double governmentTax;
        double provincialTax;
        double liquorTax;
        double salesDollars;
        double costofGoods;
        double revenue;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsExtensiveInvoice.aspx";
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
                        lblDates.Text = "Extensive Invoice Report on: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                        //DataTable invoices = R.returnExtensiveInvoices(startDate, endDate, locationID, objPageDetails);

                        DataTable resultSet = R.CallReturnExtensiveInvoices2(repInfo, objPageDetails);
                        GrdInvoices.DataSource = resultSet;
                        GrdInvoices.DataBind();
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
        protected void GrdInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInvoices_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    shipping += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltShippingCharges"));
                    tradein += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalTradeIn"));
                    discount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalDiscount"));
                    subTotal += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSubTotal"));
                    totalSales += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalSales"));
                    governmentTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                    provincialTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    liquorTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                    salesDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSalesDollars"));
                    costofGoods += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltCostofGoods"));
                    revenue += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltRevenueEarned"));

                    //Removing the time from the date
                    //string date = lblDate.Text; //Error object reference not set
                    //DateTime invoiceDate = Convert.ToDateTime(date);
                    //lblDate.Text = invoiceDate.ToString("dd-MM-yyyy");
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {

                    e.Row.Cells[1].Text = String.Format("{0:C}", shipping);
                    e.Row.Cells[2].Text = String.Format("{0:C}", tradein);
                    e.Row.Cells[3].Text = String.Format("{0:C}", discount);
                    e.Row.Cells[4].Text = String.Format("{0:C}", subTotal);
                    e.Row.Cells[5].Text = String.Format("{0:C}", totalSales);
                    e.Row.Cells[6].Text = String.Format("{0:C}", governmentTax);
                    e.Row.Cells[7].Text = String.Format("{0:C}", provincialTax);
                    e.Row.Cells[8].Text = String.Format("{0:C}", liquorTax);
                    e.Row.Cells[9].Text = String.Format("{0:C}", salesDollars);
                    e.Row.Cells[10].Text = String.Format("{0:C}", costofGoods);
                    e.Row.Cells[11].Text = String.Format("{0:C}", revenue);

                    e.Row.Cells[12].Text = (revenue / salesDollars).ToString("P");

                    if (liquorTax == 0)
                    {
                        GrdInvoices.Columns[8].Visible = false;
                    }
                    if (provincialTax == 0)
                    {
                        GrdInvoices.Columns[7].Visible = false;
                    }
                    if (governmentTax == 0)
                    {
                        GrdInvoices.Columns[6].Visible = false;
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
        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");

                ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                DataTable invoices = R.CallReturnExtensiveInvoices2(repInfo, objPageDetails);
                string fileName = "Extensive Invoice Report-" + repInfo.varLocationName + "_" + repInfo.dtmStartDate.ToShortDateString() + " - " + repInfo.dtmEndDate.ToShortDateString() + ".xlsx";
                FileInfo newFile = new FileInfo(Path.Combine(pathDownload, fileName));
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet invoicesExport = xlPackage.Workbook.Worksheets.Add("Invoices");
                    // write to sheet   
                    invoicesExport.Cells[1, 1].Value = lblDates.Text;
                    invoicesExport.Cells[2, 1].Value = "Invoice";
                    invoicesExport.Cells[2, 2].Value = "Shipping";
                    invoicesExport.Cells[2, 3].Value = "Trade-In Amount";
                    invoicesExport.Cells[2, 4].Value = "Total Discount";
                    invoicesExport.Cells[2, 5].Value = "Sub-Total";
                    invoicesExport.Cells[2, 6].Value = "Total Sales";
                    invoicesExport.Cells[2, 7].Value = "Government Tax";
                    invoicesExport.Cells[2, 8].Value = "Provincial Tax";
                    invoicesExport.Cells[2, 9].Value = "Liquor Tax";
                    invoicesExport.Cells[2, 10].Value = "Sales Dollars";
                    invoicesExport.Cells[2, 11].Value = "COGS";
                    invoicesExport.Cells[2, 12].Value = "Revenue Earned";
                    invoicesExport.Cells[2, 13].Value = "Profit Margin";
                    invoicesExport.Cells[2, 14].Value = "Customer";
                    invoicesExport.Cells[2, 15].Value = "Employee";
                    invoicesExport.Cells[2, 16].Value = "Date";
                    int recordIndex = 3;
                    foreach (DataRow row in invoices.Rows)
                    {
                        invoicesExport.Cells[recordIndex, 1].Value = row[2].ToString(); //varInvoice
                        invoicesExport.Cells[recordIndex, 2].Value = Convert.ToDouble(row[3]).ToString("C"); //Shipping
                        invoicesExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[4]).ToString("C"); //Trade-In
                        invoicesExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[5]).ToString("C"); //Discount
                        invoicesExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[6]).ToString("C"); //SubTotal
                        invoicesExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[12]).ToString("C"); //TotalSales
                        invoicesExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[7]).ToString("C"); //GST
                        invoicesExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[8]).ToString("C"); //PST
                        invoicesExport.Cells[recordIndex, 9].Value = Convert.ToDouble(row[9]).ToString("C"); //LCT
                        invoicesExport.Cells[recordIndex, 10].Value = Convert.ToDouble(row[11]).ToString("C"); //SalesDollars
                        invoicesExport.Cells[recordIndex, 11].Value = Convert.ToDouble(row[10]).ToString("C"); //Cost of Good
                        invoicesExport.Cells[recordIndex, 12].Value = Convert.ToDouble(row[13]).ToString("C"); //Revenue
                        invoicesExport.Cells[recordIndex, 13].Value = (Convert.ToDouble(row[13]) / Convert.ToDouble(row[11])).ToString("P"); //Profit Margin
                        invoicesExport.Cells[recordIndex, 14].Value = row[14].ToString(); //Cust
                        invoicesExport.Cells[recordIndex, 15].Value = row[15].ToString(); //Emp
                        invoicesExport.Cells[recordIndex, 16].Value = Convert.ToDateTime(row[1]).ToString("dd-MM-yyyy"); //Date
                        recordIndex++;
                    }
                    //Totals
                    invoicesExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    invoicesExport.Cells[recordIndex + 1, 2].Value = shipping.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 3].Value = tradein.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 4].Value = discount.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 5].Value = subTotal.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 6].Value = totalSales.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 7].Value = governmentTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 8].Value = provincialTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 9].Value = liquorTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 10].Value = salesDollars.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 11].Value = costofGoods.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 12].Value = revenue.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 13].Value = (revenue / salesDollars).ToString("P");

                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
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
        protected void GrdInvoices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInvoices_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets the string of the command argument(invoice number
                int invoiceID = Convert.ToInt32(e.CommandArgument.ToString());
                InvoiceManager IM = new InvoiceManager();
                if (IM.InvoiceIsReturn(invoiceID, objPageDetails))
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoiceReturn.aspx?invoice=" + invoiceID.ToString(), false);
                }
                else
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoice.aspx?invoice=" + invoiceID.ToString(), false);
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
    }
}