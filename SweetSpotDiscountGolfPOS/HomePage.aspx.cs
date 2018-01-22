using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class HomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        List<Invoice> invoiceList = new List<Invoice>();
        CurrentUser cu;
        int totalSales = 0;
        double totalDiscounts = 0;
        double totalTradeIns = 0;
        double totalSubtotals = 0;
        double totalGST = 0;
        double totalPST = 0;
        double totalBalancePaid = 0;
        double totalMOPAmount = 0;
        string oldInvoice = string.Empty;
        string newInvoice = string.Empty;
        int currentRow = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "HomePage.aspx";
            //Session["prevPage"] = "HomePage.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!this.IsPostBack)
                {
                    //Sets sql connection and executes location command
                    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString);
                    SqlCommand cmd = new SqlCommand("SELECT city FROM tbl_location ", con);
                    //Checks current location and populates drop down
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    ddlLocation.DataSource = cmd.ExecuteReader();
                    ddlLocation.DataTextField = "City";
                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = cu.locationName;
                    con.Close();
                }
                //Checks user for admin status
                if (cu.jobID == 0)
                {
                    lbluser.Text = "You have Admin Access";
                    lbluser.Visible = true;
                }
                else/* if (Session["Loc"] != null)*/
                {
                    //If no admin status shows location as label instead of drop down
                    lblLocation.Text = cu.locationName;
                    lblLocation.Visible = true;
                    ddlLocation.Visible = false;
                }
                //populate gridview with todays sales
                int locationID = lm.locationIDfromCity(ddlLocation.SelectedValue);

                //invoiceList = ssm.getInvoiceBySaleDate(DateTime.Today, locationID);
                DataTable invoices = ssm.getInvoiceBySaleDate(DateTime.Today, locationID);
                grdSameDaySales.DataSource = invoices;
                grdSameDaySales.DataBind();
                foreach (GridViewRow row in grdSameDaySales.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }
                MergeRows(grdSameDaySales);
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
        //Currently used for Removing the row
        //protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    //Collects current method for error tracking
        //    string method = "OnRowDeleting";
        //    try
        //    {
        //        string deleteReason = hidden.Value;
        //        //if (deleteReason.Equals("Code:CancelDelete"))
        //        //{

        //        //}
        //        //else
        //        //Checks fo the reason why invoice is being deleted
        //        if (!deleteReason.Equals("Code:CancelDelete") && !deleteReason.Equals(""))
        //        {
        //            //Gathers selected invoice number
        //            int index = e.RowIndex;
        //            Label lblInvoice = (Label)grdSameDaySales.Rows[index].FindControl("lblInvoiceNumber");
        //            string invoice = lblInvoice.Text;
        //            char[] splitchar = { '-' };
        //            string[] invoiceSplit = invoice.Split(splitchar);
        //            int invoiceNum = Convert.ToInt32(invoiceSplit[0]);
        //            int invoiceSubNum = Convert.ToInt32(invoiceSplit[1]);
        //            string deletionReason = deleteReason;
        //            //calls deletion method
        //            idu.deleteInvoice(invoiceNum, invoiceSubNum, deletionReason);
        //            MessageBox.ShowMessage("Invoice " + invoice + " has been deleted", this);
        //            //Refreshes current  page
        //            Server.Transfer(Request.RawUrl);
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log employee number
        //        int employeeID = cu.empID;
        //        //Log current page
        //        string currPage = Convert.ToString(Session["currPage"]);
        //        //Log all info into error table
        //        er.logError(ex, employeeID, currPage, method, this);
        //        //string prevPage = Convert.ToString(Session["prevPage"]);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occured and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator", this);
        //        //Server.Transfer(prevPage, false);
        //    }
        //}
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
        protected void grdSameDaySales_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //Problems with looping 
            //Collects current method for error tracking
            string method = "grdSameDaySales_RowDataBound";
            //Current method does nothing
            try
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lbtnInvoiceNumber");
                if (lb != null)
                {
                    oldInvoice = lb.Text;
                    if (oldInvoice == newInvoice)
                    {
                        //Setting the contents of the cells to be blank and to remove them                        
                        for (int l = 2; l < 9; l++)
                        {
                            e.Row.Cells[l].Visible = false;
                            e.Row.Cells[l].Text = "";
                        }
                        e.Row.Cells[0].Text = "";
                        e.Row.Cells[1].Text = "";
                        e.Row.Cells[1].ColumnSpan = 8;
                    }
                    else if (oldInvoice != newInvoice)
                    {
                        //This is where the totals at the bottom are calculated
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //This triggers when the row is not the footer(Not the end)
                            //Need to determine if the cell is empty
                            totalSales += 1;
                            totalDiscounts += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discountAmount"));
                            totalTradeIns += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "tradeinAmount"));
                            totalSubtotals += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "subTotal"));
                            totalGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "governmentTax"));
                            totalPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provincialTax"));
                            totalBalancePaid += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "balanceDue"));
                        }
                        newInvoice = oldInvoice;
                    }
                }
                //This is separate because every line will have a MOP on it
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    totalMOPAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amountPaid"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Triggers when the row is the footer(End)
                    e.Row.Cells[1].Text = totalSales.ToString();
                    e.Row.Cells[3].Text = String.Format("{0:C}", totalDiscounts);
                    e.Row.Cells[4].Text = String.Format("{0:C}", totalTradeIns);
                    e.Row.Cells[5].Text = String.Format("{0:C}", totalSubtotals);
                    e.Row.Cells[6].Text = String.Format("{0:C}", totalGST);
                    e.Row.Cells[7].Text = String.Format("{0:C}", totalPST);
                    e.Row.Cells[8].Text = String.Format("{0:C}", totalBalancePaid);
                    e.Row.Cells[10].Text = String.Format("{0:C}", totalMOPAmount);
                }
                currentRow++;
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

        protected static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string rowText = row.Cells[1].Text;
                string previousRowText = previousRow.Cells[1].Text;
                string rowInvoice;
                string prevRowInvoice;
                LinkButton lbtnRow = (LinkButton)row.FindControl("lbtnInvoiceNumber");
                LinkButton lbtnPrevRow = (LinkButton)previousRow.FindControl("lbtnInvoiceNumber");
                if (lbtnRow != null)
                    rowInvoice = lbtnRow.Text;
                else
                    rowInvoice = "";
                if (lbtnPrevRow != null)
                    prevRowInvoice = lbtnPrevRow.Text;
                else
                    prevRowInvoice = "";
                if (rowText.Equals(previousRowText) && !rowText.IsNumeric() && !previousRowText.IsNumeric())
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2
                                     ? 2 // merge the first two cells
                                     : previousRow.Cells[1].RowSpan + 1; //any subsequent merging
                    previousRow.Cells[1].Visible = false;
                }
                if (rowInvoice.Equals(prevRowInvoice) || prevRowInvoice.Equals(""))
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2
                                     ? 2 // merge the first two cells
                                     : previousRow.Cells[0].RowSpan + 1; //any subsequent merging
                    previousRow.Cells[0].Visible = false;
                }
            }
        }
    }
}