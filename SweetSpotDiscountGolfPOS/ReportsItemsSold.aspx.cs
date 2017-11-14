using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsItemsSold : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CustomMessageBox cmb = new CustomMessageBox();
        CurrentUser cu = new CurrentUser();
        LocationManager l = new LocationManager();
        DateTime startDate;
        DateTime endDate;
        double tCost;
        double tPrice;
        //double tDiscount;
        double tProfit;

        List<Items> items = new List<Items>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsItemsSold";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                
                    //Gathering the start and end dates
                    Object[] passing = (Object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " for " + l.locationName(locationID);
                    }
                    else
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                    }

                    
                    //Binding the gridview
                    items = r.returnItemsSold(startDate, endDate, locationID);
                    //Checking if there are any values
                    if (items.Count > 0)
                    {
                        grdItems.DataSource = items;
                        grdItems.DataBind();
                    }
                    else
                    {
                        if (startDate == endDate)
                        {
                            lblDates.Text = "There are no items sold for: " + startDate.ToString("d");
                        }
                        else
                        {
                            lblDates.Text = "There are no items sold for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                        }
                    }
                

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                er.logError(ex, cu.empID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdItems_RowDataBound(object sender, GridViewRowEventArgs e)
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
                tCost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cost"));
                tPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "price"));
                tProfit += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "difference"));
            }
            else if(e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = String.Format("{0:C}", tCost);
                e.Row.Cells[3].Text = String.Format("{0:C}", tPrice);
                e.Row.Cells[6].Text = String.Format("{0:C}", tProfit);
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
                Server.Transfer("PrintableInvoice.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                string fileName = "Items Sold Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet itemsSoldExport = xlPackage.Workbook.Worksheets.Add("Items Sold");
                    // write to sheet   
                    itemsSoldExport.Cells[1, 1].Value = lblDates.Text;
                    itemsSoldExport.Cells[2, 1].Value = "Invoice Number";
                    itemsSoldExport.Cells[2, 2].Value = "SKU";
                    itemsSoldExport.Cells[2, 3].Value = "Item Cost";
                    itemsSoldExport.Cells[2, 4].Value = "Item Price";
                    itemsSoldExport.Cells[2, 5].Value = "Item Discount";
                    itemsSoldExport.Cells[2, 6].Value = "Item Profit";
                    int recordIndex = 3;
                    foreach (Items i in items)
                    {
                        itemsSoldExport.Cells[recordIndex, 1].Value = i.invoice;
                        itemsSoldExport.Cells[recordIndex, 2].Value = i.sku;
                        itemsSoldExport.Cells[recordIndex, 3].Value = i.cost;
                        itemsSoldExport.Cells[recordIndex, 4].Value = i.price;
                        if (i.percent)
                        {
                            itemsSoldExport.Cells[recordIndex, 5].Value = i.discount + "%";
                        }
                        else
                        {
                            itemsSoldExport.Cells[recordIndex, 5].Value = "$" + i.discount;
                        }
                        itemsSoldExport.Cells[recordIndex, 6].Value = i.difference;
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
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