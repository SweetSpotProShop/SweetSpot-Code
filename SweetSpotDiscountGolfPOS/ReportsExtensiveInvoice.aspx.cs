using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsExtensiveInvoice : System.Web.UI.Page
    {
        CurrentUser CU;
        ErrorReporting ER = new ErrorReporting();
        Reports R = new Reports();


        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        DateTime startDate;
        DateTime endDate;
        Employee e;
        LocationManager l = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        DataTable invoices = new DataTable();
        double shipping;
        double discount;
        double preTax;
        double govTax;
        double proTax;
        double postTax;
        double cogs;
        double revenue;
        double margin;
        int marginCounter;
        double tradein;

        //Counter for rows
        int counter = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsExtensiveInvoice.aspx";
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
                    Object[] repInfo = (Object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])repInfo[0];
                    startDate = reportDates[0];
                    endDate = reportDates[1];
                    int locID = Convert.ToInt32(repInfo[1]);
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Extensive Invoice Report on: " + startDate.ToString("d") + " for " + lm.locationName(locID);
                    }
                    else
                    {
                        lblDates.Text = "Extensive Invoice Report on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + lm.locationName(locID);
                    }

                    grdInvoices.DataSource = R.returnExtensiveInvoices(startDate, endDate, locID);
                    grdInvoices.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdInvoices_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            Label lblShipping = (Label)e.Row.FindControl("lblShipping");
            Label lblTradeIn = (Label)e.Row.FindControl("lblTradeIn");
            Label lblDiscount = (Label)e.Row.FindControl("lblDiscount");
            Label lblPreTax = (Label)e.Row.FindControl("lblPreTax");
            Label lblGovTax = (Label)e.Row.FindControl("lblGovernmentTax");
            Label lblProvTax = (Label)e.Row.FindControl("lblProvincialTax");
            Label lblPostTax = (Label)e.Row.FindControl("lblPostTax");
            Label lblCOGS = (Label)e.Row.FindControl("lblCOGS");
            Label lblRevenue = (Label)e.Row.FindControl("lblRevenue");
            Label lblProfitMargin = (Label)e.Row.FindControl("lblProfitMargin");
            Label lblDate = (Label)e.Row.FindControl("lblDate");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Shipping
                if (lblShipping.Text.isNumber())
                {
                    shipping += Convert.ToDouble(lblShipping.Text);
                    lblShipping.Text = "$" + lblShipping.Text;
                }
                //TradeIn
                if (lblTradeIn.Text.isNumber())
                {
                    tradein += Convert.ToDouble(lblTradeIn.Text);
                    lblTradeIn.Text = "$" + lblTradeIn.Text;
                }
                //Discount
                if (lblDiscount.Text.isNumber())
                {
                    discount += Convert.ToDouble(lblDiscount.Text);
                    lblDiscount.Text = "$" + lblDiscount.Text;
                }
                //Pre-Tax
                if (lblPreTax.Text.isNumber())
                {
                    preTax += Convert.ToDouble(lblPreTax.Text);
                    lblPreTax.Text = "$" + lblPreTax.Text;
                }
                //Gov Tax
                if (lblGovTax.Text.isNumber())
                {
                    govTax += Convert.ToDouble(lblGovTax.Text);
                    lblGovTax.Text = "$" + lblGovTax.Text;
                }
                //Prov Tax
                if (lblProvTax.Text.isNumber())
                {
                    proTax += Convert.ToDouble(lblProvTax.Text);
                    lblProvTax.Text = "$" + lblProvTax.Text;
                }
                //Post-Tax
                if (lblPostTax.Text.isNumber())
                {
                    postTax += Convert.ToDouble(lblPostTax.Text);
                    lblPostTax.Text = "$" + lblPostTax.Text;
                }
                //COGS
                if (lblCOGS.Text.isNumber())
                {
                    cogs += Convert.ToDouble(lblCOGS.Text);
                    lblCOGS.Text = "$" + lblCOGS.Text;
                }
                //Revenue
                if (lblRevenue.Text.isNumber())
                {
                    revenue += Convert.ToDouble(lblRevenue.Text);
                    lblRevenue.Text = "$" + lblRevenue.Text;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Removing the time from the date
                string date = lblDate.Text;
                DateTime invoiceDate = Convert.ToDateTime(date);
                lblDate.Text = invoiceDate.ToString("dd-MM-yyyy");
                Label lblShippingTotal = (Label)e.Row.FindControl("lblShippingTotal");
                Label lblTradeinTotal = (Label)e.Row.FindControl("lblTradeInTotal");
                Label lblDiscountTotal = (Label)e.Row.FindControl("lblDiscountTotal");
                Label lblPreTaxTotal = (Label)e.Row.FindControl("lblPreTaxTotal");
                Label lblGovTaxTotal = (Label)e.Row.FindControl("lblGovernmentTaxTotal");
                Label lblProvTaxTotal = (Label)e.Row.FindControl("lblProvincialTaxTotal");
                Label lblPostTaxTotal = (Label)e.Row.FindControl("lblPostTaxTotal");
                Label lblCOGSTotal = (Label)e.Row.FindControl("lblCOGSTotal");
                Label lblRevenueTotal = (Label)e.Row.FindControl("lblRevenueTotal");
                Label lblProfitMarginTotal = (Label)e.Row.FindControl("lblProfitMarginTotal");

                lblShippingTotal.Text = String.Format("{0:C}", shipping);
                lblTradeinTotal.Text = String.Format("{0:C}", tradein);
                lblDiscountTotal.Text = String.Format("{0:C}", discount);
                lblPreTaxTotal.Text = String.Format("{0:C}", preTax);
                lblGovTaxTotal.Text = String.Format("{0:C}", govTax);
                lblProvTaxTotal.Text = String.Format("{0:C}", proTax);
                lblPostTaxTotal.Text = String.Format("{0:C}", postTax);
                lblCOGSTotal.Text = String.Format("{0:C}", cogs);
                lblRevenueTotal.Text = String.Format("{0:C}", revenue);
                double profitMarginAverage = (margin / marginCounter);
                lblProfitMarginTotal.Text = profitMarginAverage.ToString("#.##") + "%";
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                Object[] passing = (Object[])Session["reportInfo"];
                string loc = l.locationName(Convert.ToInt32(passing[1]));
                string fileName = "Extensive Invoice Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet invoicesExport = xlPackage.Workbook.Worksheets.Add("Invoices"); //TODO:DONE Update the download to include the trade in column
                                                                                                   // write to sheet   
                    invoicesExport.Cells[1, 1].Value = lblDates.Text;
                    invoicesExport.Cells[2, 1].Value = "Invoice";
                    invoicesExport.Cells[2, 2].Value = "Shipping";
                    invoicesExport.Cells[2, 3].Value = "Trade-In Amount";
                    invoicesExport.Cells[2, 4].Value = "Total Discount";
                    invoicesExport.Cells[2, 5].Value = "Pre-Tax";
                    invoicesExport.Cells[2, 6].Value = "Government Tax";
                    invoicesExport.Cells[2, 7].Value = "Provincial Tax";
                    invoicesExport.Cells[2, 8].Value = "Post-Tax";
                    invoicesExport.Cells[2, 9].Value = "COGS";
                    invoicesExport.Cells[2, 10].Value = "Revenue Earned";
                    invoicesExport.Cells[2, 11].Value = "Profit Margin";
                    invoicesExport.Cells[2, 12].Value = "Customer";
                    invoicesExport.Cells[2, 13].Value = "Employee";
                    invoicesExport.Cells[2, 14].Value = "Date";
                    int recordIndex = 3;
                    foreach (DataRow row in invoices.Rows)
                    {

                        invoicesExport.Cells[recordIndex, 1].Value = row[0].ToString();
                        invoicesExport.Cells[recordIndex, 2].Value = row[1].ToString();
                        invoicesExport.Cells[recordIndex, 3].Value = row[2].ToString();
                        invoicesExport.Cells[recordIndex, 4].Value = row[3].ToString();
                        invoicesExport.Cells[recordIndex, 5].Value = row[4].ToString();
                        invoicesExport.Cells[recordIndex, 6].Value = row[5].ToString();
                        invoicesExport.Cells[recordIndex, 7].Value = row[6].ToString();
                        invoicesExport.Cells[recordIndex, 8].Value = row[7].ToString();
                        invoicesExport.Cells[recordIndex, 9].Value = row[8].ToString();
                        invoicesExport.Cells[recordIndex, 10].Value = row[9].ToString();
                        invoicesExport.Cells[recordIndex, 11].Value = row[10].ToString();
                        invoicesExport.Cells[recordIndex, 12].Value = row[11].ToString();
                        invoicesExport.Cells[recordIndex, 13].Value = row[12].ToString();
                        DateTime date = Convert.ToDateTime(row[13]);
                        invoicesExport.Cells[recordIndex, 14].Value = date.ToString("dd-MM-yyyy");
                        recordIndex++;
                    }
                    //Totals
                    invoicesExport.Cells[recordIndex + 1, 1].Value = "Totals:";
                    invoicesExport.Cells[recordIndex + 1, 2].Value = shipping.ToString();
                    invoicesExport.Cells[recordIndex + 1, 3].Value = tradein.ToString();
                    invoicesExport.Cells[recordIndex + 1, 4].Value = discount.ToString();
                    invoicesExport.Cells[recordIndex + 1, 5].Value = preTax.ToString();
                    invoicesExport.Cells[recordIndex + 1, 6].Value = govTax.ToString();
                    invoicesExport.Cells[recordIndex + 1, 7].Value = proTax.ToString();
                    invoicesExport.Cells[recordIndex + 1, 8].Value = postTax.ToString();
                    invoicesExport.Cells[recordIndex + 1, 9].Value = cogs.ToString();
                    invoicesExport.Cells[recordIndex + 1, 10].Value = revenue.ToString();

                    invoicesExport.Cells[recordIndex + 1, 11].Value = (margin / marginCounter).ToString();

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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnInvoiceNumber_Click";
            try
            {
                //Text of the linkbutton
                LinkButton btn = sender as LinkButton;
                string invoice = btn.Text;
                //Parsing into invoiceNum and invoiceSubNum
                char[] splitchar = { '-' };
                string[] invoiceSplit = invoice.Split(splitchar);
                int invNum = Convert.ToInt32(invoiceSplit[0]);
                int invSNum = Convert.ToInt32(invoiceSplit[1]);
                //determines the table to use for queries
                string table = "";
                int tran = 3;
                if (invSNum > 1)
                {
                    table = "Returns";
                    tran = 4;
                }
                //Stores required info into Sessions
                Invoice rInvoice = ssm.getSingleInvoice(invNum, invSNum);
                //Session["key"] = rInvoice.customerID;
                //Session["Invoice"] = invoice;
                Session["actualInvoiceInfo"] = rInvoice;
                Session["useInvoice"] = true;
                //Session["strDate"] = rInvoice.invoiceDate;
                Session["ItemsInCart"] = ssm.invoice_getItems(invNum, invSNum, "tbl_invoiceItem" + table);
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invNum, invSNum, "tbl_invoice");
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invNum, invSNum, "tbl_invoiceMOP");
                Session["TranType"] = tran;
                //Changes page to display a printable invoice
                Response.Redirect("PrintableInvoice.aspx?inv=" + invoice, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}