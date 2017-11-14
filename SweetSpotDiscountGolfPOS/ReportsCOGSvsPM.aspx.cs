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
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager l = new LocationManager();
        CustomMessageBox cmb = new CustomMessageBox();
        CurrentUser cu = new CurrentUser();
        DateTime startDate;
        DateTime endDate;
        int locationID;
        double tCost;
        double tPrice;
        //double tDiscount;
        double tProfit;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsCOGSvsPM";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!IsPostBack)
                {
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

                    List<Invoice> inv = new List<Invoice>();
                    //Binding the gridview
                    inv = r.returnInvoicesForCOGS(startDate, endDate, locationID);
                    //Checking if there are any values
                    if(inv.Count > 0)
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
            else if(e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", tCost);
                e.Row.Cells[2].Text = String.Format("{0:C}", tPrice);
                e.Row.Cells[5].Text = String.Format("{0:C}", tProfit);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            try
            {
                //Gathering the start and end dates
                Object[] passing = (Object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])passing[0];
                startDate = reportDates[0];
                endDate = reportDates[1];
                locationID = (int)passing[1];
                //Sets up database connection
                string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connectionString);
                //Selects everything form the invoice table
                DataTable cogsInvoices = new DataTable();
                using (var cmd = new SqlCommand("getInvoiceForCOGS", sqlCon)) //Calling the SP   
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@locationID", locationID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(cogsInvoices);
                }
                DataColumnCollection headers = cogsInvoices.Columns;
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
                    cogsExport.Cells[2, 5].Value = "Total Profit";
                    int recordIndex = 3;
                    foreach(DataRow row in cogsInvoices.Rows)
                    {
                        
                        cogsExport.Cells[recordIndex, 1].Value = row["invoice"];
                        cogsExport.Cells[recordIndex, 2].Value = row["totalCost"];
                        cogsExport.Cells[recordIndex, 3].Value = row["totalPrice"];
                        if (Convert.ToBoolean(row["percentage"]) == true)
                        {
                            cogsExport.Cells[recordIndex, 4].Value = row["totalDiscount"] + "%";
                        }
                        else
                        {
                            cogsExport.Cells[recordIndex, 4].Value = "$" + row["totalDiscount"];
                        }
                        cogsExport.Cells[recordIndex, 4].Style.Numberformat.Format = "0.0";

                        cogsExport.Cells[recordIndex, 5].Value = row["totalProfit"];
                        recordIndex++;
                    }

                    ////Export main invoice
                    //for (int i = 1; i < cogsInvoices.Rows.Count; i++)
                    //{
                    //    for (int j = 1; j < cogsInvoices.Columns.Count + 1; j++)
                    //    {
                    //        if (i == 1)
                    //        {
                    //            cogsExport.Cells[i, j].Value = headers[j - 1].ToString();
                    //        }
                    //        else
                    //        {
                    //            cogsExport.Cells[i, j].Value = cogsInvoices.Rows[i - 1][j - 1];
                    //        }
                    //    }
                    //}
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