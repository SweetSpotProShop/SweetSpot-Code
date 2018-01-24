using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsCOGSvsPM : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();

        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager l = new LocationManager();
        CustomMessageBox cmb = new CustomMessageBox();
        DateTime startDate;
        DateTime endDate;
        int locationID;
        double tCost;
        double tPrice;
        //double tDiscount;
        double tProfit;
        List<Invoice> inv = new List<Invoice>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsCOGSvsPM";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }

                //Gathering the start and end dates
                Object[] passing = (Object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])passing[0];
                startDate = reportDates[0];
                endDate = reportDates[1];
                locationID = (int)passing[1];
                //Builds string to display in label
                if (startDate == endDate)
                {
                    lblDates.Text = "Cost of Goods Sold & Profit Margin on: " + startDate.ToString("d") + " for " + l.locationName(locationID);
                }
                else
                {
                    lblDates.Text = "Cost of Goods Sold & Profit Margin on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                }                
                //Binding the gridview
                inv = r.returnInvoicesForCOGS(startDate, endDate, locationID);
                //Checking if there are any values
                if (inv.Count > 0)
                {
                    grdInvoiceSelection.DataSource = inv;
                    grdInvoiceSelection.DataBind();
                    //Displaying the total cost
                    //lblTotalCostDisplay.Text = r.returnCOGSCost(startDate, endDate, locationID).ToString("C");
                    //Displaying the total price/sold at
                    //lblSoldDisplay.Text = r.returnCOGSPrice(startDate, endDate, locationID).ToString("C");
                    //Displaying the profit margin
                    //lblProfitMarginDisplay.Text = r.returnCOGSProfitMargin(startDate, endDate, locationID).ToString("C");
                }
                else
                {
                    if (startDate == endDate)
                    {
                        lblDates.Text = "There are no invoices for: " + startDate.ToString("d");
                    }
                    else
                    {
                        lblDates.Text = "There are no invoices for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                    }
                    grdInvoiceSelection.Visible = false;
                    //lblTotalCostDisplay.Visible = false;
                    //lblSoldDisplay.Visible = false;
                    //lblProfitMarginDisplay.Visible = false;
                    //lblItemsSold.Visible = false;
                    //lblCost.Visible = false;
                    //lblPM.Visible = false;
                    //lblProfitMargin.Visible = false;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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

                Server.Transfer("PrintableInvoice.aspx?inv=" + invNum + "-" + invSNum, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdInvoiceSelection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label percent = (Label)e.Row.FindControl("lblPercentage");
                Label discount = (Label)e.Row.FindControl("lblTotalDiscount");
                if (percent.Text.Equals("True"))
                {
                    discount.Text = discount.Text + "%";
                }
                else
                {
                    if (discount.Text.Equals("0"))
                    {
                        discount.Text = "-";
                    }
                    else
                    {
                        discount.Text = "$" + discount.Text;
                    }
                }
                Label lblProfit = (Label)e.Row.FindControl("lblTotalProfit");
                string profitText = lblProfit.Text;
                if (profitText.Contains("("))
                {
                    lblProfit.ForeColor = System.Drawing.Color.Red;
                }
                tCost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalCost"));
                tPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "balanceDue"));
                tProfit += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "totalProfit"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", tPrice);
                e.Row.Cells[2].Text = String.Format("{0:C}", tCost);
                //Maybe calculate the average profit margin
                // e.Row.Cells[5].Text = String.Format("{0:C}", tProfit);
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
                string loc = l.locationName(locationID);
                string fileName = "COGS and PM Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet cogsExport = xlPackage.Workbook.Worksheets.Add("COGS");
                    // write to sheet     
                    cogsExport.Cells[1, 1].Value = lblDates.Text;
                    cogsExport.Cells[2, 1].Value = "Invoice Number";
                    cogsExport.Cells[2, 2].Value = "Total Cost";
                    cogsExport.Cells[2, 3].Value = "Total Price";
                    cogsExport.Cells[2, 4].Value = "Total Discount";
                    cogsExport.Cells[2, 5].Value = "Profit Margin";
                    int recordIndex = 3;
                    foreach (Invoice i in inv)
                    {
                        cogsExport.Cells[recordIndex, 1].Value = i.invoice;
                        cogsExport.Cells[recordIndex, 2].Value = i.totalCost;
                        cogsExport.Cells[recordIndex, 3].Value = i.balanceDue;
                        if (i.percentage == true)
                        {
                            cogsExport.Cells[recordIndex, 4].Value = i.discountAmount + "%";
                        }
                        else
                        {
                            cogsExport.Cells[recordIndex, 4].Value = "$" + i.discountAmount;
                        }
                        cogsExport.Cells[recordIndex, 4].Style.Numberformat.Format = "0.0";

                        cogsExport.Cells[recordIndex, 5].Value = i.totalProfit + "%";
                        recordIndex++;
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}