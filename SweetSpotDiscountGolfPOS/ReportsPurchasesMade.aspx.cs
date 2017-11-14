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
    public partial class ReportsPurchasesMade : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        Reports reports = new Reports();
        CurrentUser cu = new CurrentUser();
        LocationManager l = new LocationManager();
        SweetShopManager ssm = new SweetShopManager();
        double totalPurchAmount = 0;
        int totalPurchases = 0;
        int totalCheques = 0;

        List<Purchases> purch = new List<Purchases>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsPurchasesMade.aspx";
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
                Object[] repInfo = (Object[])Session["reportInfo"];
                DateTime[] reportDates = (DateTime[])repInfo[0];
                DateTime startDate = reportDates[0];
                DateTime endDate = reportDates[1];
                int locationID = (int)repInfo[1];
                //Builds string to display in label
                lblPurchasesMadeDate.Text = "Purchases Made Between: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                //Creating a cashout list and calling a method that grabs all mops and amounts paid
                purch = reports.returnPurchasesDuringDates(startDate, endDate, locationID);
                grdPurchasesMade.DataSource = purch;
                grdPurchasesMade.DataBind();
                foreach (GridViewRow row in grdPurchasesMade.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
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
        protected void grdPurchasesMade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // check row type
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "chequeNumber")) > 0)
                {
                    totalCheques += 1;
                }
                totalPurchases += 1;
                totalPurchAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amountPaid"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = totalPurchases.ToString();
                e.Row.Cells[3].Text = totalCheques.ToString();
                e.Row.Cells[4].Text = String.Format("{0:c}", totalPurchAmount);
            }
        }
        protected void printReport(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "printReport";
            //Current method does nothing
            try
            { }
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
        protected void lbtnReceiptNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnReceiptNumber_Click";
            try
            {
                LinkButton btn = sender as LinkButton;
                Object[] o = new Object[3];
                o = ssm.getSingleReceipt(Convert.ToInt32(btn.Text));

                Invoice receipt = (Invoice)o[0];
                List<Cart> rItems = (List<Cart>)o[1];
                List<Checkout> rOut = (List<Checkout>)o[2];

                Session["key"] = receipt.customerID;
                Session["Invoice"] = receipt.invoiceNum;
                Session["strDate"] = receipt.invoiceDate;
                Session["TranType"] = 6;
                Session["ItemsInCart"] = rItems;
                Session["CheckOutTotals"] = new CheckoutManager(receipt.balanceDue);
                Session["MethodsofPayment"] = rOut;

                //Changes to the Reports Cash Out page
                Server.Transfer("PrintableReceipt.aspx", false);
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
                ////Gathering the start and end dates
                //Object[] passing = (Object[])Session["reportInfo"];
                //DateTime[] reportDates = (DateTime[])passing[0];
                //DateTime startDate = reportDates[0];
                //DateTime endDate = reportDates[1];
                //int locationID = (int)passing[1];
                ////Sets up database connection
                //string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
                //SqlConnection sqlCon = new SqlConnection(connectionString);
                ////Selects everything form the invoice table
                //DataTable cogsInvoices = new DataTable();
                //using (var cmd = new SqlCommand("getInvoiceForCOGS", sqlCon)) //Calling the SP   
                //using (var da = new SqlDataAdapter(cmd))
                //{
                //    cmd.Parameters.AddWithValue("@startDate", startDate);
                //    cmd.Parameters.AddWithValue("@endDate", endDate);
                //    cmd.Parameters.AddWithValue("@locationID", locationID);
                //    //Executing the SP
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da.Fill(cogsInvoices);
                //}
                //DataColumnCollection headers = cogsInvoices.Columns;
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                FileInfo newFile = new FileInfo(pathDownload + "Purchases Report.xlsx");
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet purchasesExport = xlPackage.Workbook.Worksheets.Add("Purchases");
                    // write to sheet     

                    //cogsExport.Cells["A1"].LoadFromDataTable(cogsInvoices, true);
                    //xlPackage.Save();
                    purchasesExport.Cells[1, 1].Value = "Receipt Number";
                    purchasesExport.Cells[1, 2].Value = "Receipt Date";
                    purchasesExport.Cells[1, 3].Value = "Purchase Method";
                    purchasesExport.Cells[1, 4].Value = "Cheque Number";
                    purchasesExport.Cells[1, 5].Value = "Purchase Amount";
                    int recordIndex = 2;
                    foreach (Purchases p in purch)
                    {

                        purchasesExport.Cells[recordIndex, 1].Value = p.receiptNumber;
                        purchasesExport.Cells[recordIndex, 2].Value = p.receiptDate.ToString("d");
                        purchasesExport.Cells[recordIndex, 3].Value = p.mopDescription;
                        purchasesExport.Cells[recordIndex, 4].Value = p.chequeNumber;
                        purchasesExport.Cells[recordIndex, 5].Value = p.amountPaid;
                        recordIndex++;
                    }
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=Purchases Report.xlsx");
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