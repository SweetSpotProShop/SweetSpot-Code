using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsExtensiveInvoice : System.Web.UI.Page
    {
        CurrentUser CU;
        ErrorReporting ER = new ErrorReporting();
        Reports R = new Reports();
        LocationManager LM = new LocationManager();

        double shipping;
        double tradein;
        double discount;
        double preTax;
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
                    CU = (CurrentUser)Session["currentUser"];
                    //Gathering the start and end dates
                    object[] repInfo = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])repInfo[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = Convert.ToInt32(repInfo[1]);
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Extensive Invoice Report on: " + startDate.ToString("dd/MMM/yy") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    else
                    {
                        lblDates.Text = "Extensive Invoice Report on: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    DataTable invoices = R.returnExtensiveInvoices(startDate, endDate, locationID, objPageDetails);
                    grdInvoices.DataSource = invoices;
                    grdInvoices.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoices_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    shipping += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltShippingCharges"));
                    tradein += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalTradeIn"));
                    discount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalDiscount"));
                    preTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltPreTax"));
                    governmentTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                    provincialTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                    liquorTax += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltLiquorTaxAmount"));
                    salesDollars += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalSales"));
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
                    e.Row.Cells[4].Text = String.Format("{0:C}", preTax);
                    e.Row.Cells[5].Text = String.Format("{0:C}", governmentTax);                    
                    e.Row.Cells[6].Text = String.Format("{0:C}", provincialTax);
                    e.Row.Cells[7].Text = String.Format("{0:C}", liquorTax);
                    e.Row.Cells[8].Text = String.Format("{0:C}", salesDollars);
                    e.Row.Cells[9].Text = String.Format("{0:C}", costofGoods);
                    e.Row.Cells[10].Text = String.Format("{0:C}", revenue);

                    e.Row.Cells[11].Text = (revenue / preTax).ToString("P");
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                                + "If you continue to receive this message please contact "
                                + "your system administrator.", this);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");

                object[] repInfo = (object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])repInfo[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];

                DataTable invoices = R.returnExtensiveInvoices(startDate, endDate, Convert.ToInt32(repInfo[1]), objPageDetails);
                string fileName = "Extensive Invoice Report-" + LM.ReturnLocationName(Convert.ToInt32(repInfo[1]), objPageDetails) + "_" + startDate.ToShortDateString() + " - " + endDate.ToShortDateString() + ".xlsx";
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
                    invoicesExport.Cells[2, 5].Value = "Pre-Tax";
                    invoicesExport.Cells[2, 6].Value = "Government Tax";
                    invoicesExport.Cells[2, 7].Value = "Provincial Tax";
                    invoicesExport.Cells[2, 8].Value = "Liquor Tax";
                    invoicesExport.Cells[2, 9].Value = "Sales Dollars";
                    invoicesExport.Cells[2, 10].Value = "COGS";
                    invoicesExport.Cells[2, 11].Value = "Revenue Earned";
                    invoicesExport.Cells[2, 12].Value = "Profit Margin";
                    invoicesExport.Cells[2, 13].Value = "Customer";
                    invoicesExport.Cells[2, 14].Value = "Employee";
                    invoicesExport.Cells[2, 15].Value = "Date";
                    int recordIndex = 3;
                    foreach (DataRow row in invoices.Rows)
                    {
                        invoicesExport.Cells[recordIndex, 1].Value = row[2].ToString(); //varInvoice
                        invoicesExport.Cells[recordIndex, 2].Value = Convert.ToDouble(row[3]).ToString("C"); //Shipping
                        invoicesExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[4]).ToString("C"); //Trade-In
                        invoicesExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[5]).ToString("C"); //Discount
                        invoicesExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[6]).ToString("C"); //PreTax
                        invoicesExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[7]).ToString("C"); //GST
                        invoicesExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[8]).ToString("C"); //PST
                        invoicesExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[9]).ToString("C"); //LCT
                        invoicesExport.Cells[recordIndex, 9].Value = Convert.ToDouble(row[11]).ToString("C"); //SalesDollars
                        invoicesExport.Cells[recordIndex, 10].Value = Convert.ToDouble(row[10]).ToString("C"); //Cost of Good
                        invoicesExport.Cells[recordIndex, 11].Value = Convert.ToDouble(row[12]).ToString("C"); //Revenue
                        invoicesExport.Cells[recordIndex, 12].Value = (Convert.ToDouble(row[12]) / Convert.ToDouble(row[6])).ToString("P"); //Profit Margin
                        invoicesExport.Cells[recordIndex, 13].Value = row[13].ToString(); //Cust
                        invoicesExport.Cells[recordIndex, 14].Value = row[14].ToString(); //Emp
                        invoicesExport.Cells[recordIndex, 15].Value = Convert.ToDateTime(row[1]).ToString("dd-MM-yyyy"); //Date
                        recordIndex++;
                    }
                    //Totals
                    invoicesExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    invoicesExport.Cells[recordIndex + 1, 2].Value = shipping.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 3].Value = tradein.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 4].Value = discount.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 5].Value = preTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 6].Value = governmentTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 7].Value = provincialTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 8].Value = liquorTax.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 9].Value = salesDollars.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 10].Value = costofGoods.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 11].Value = revenue.ToString("C");
                    invoicesExport.Cells[recordIndex + 1, 12].Value = (revenue / preTax).ToString("P");                   

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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void grdInvoices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnInvoiceNumber_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                string invoice = e.CommandArgument.ToString();
                //Changes page to display a printable invoice
                Response.Redirect("PrintableInvoice.aspx?invoice=" + invoice, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}