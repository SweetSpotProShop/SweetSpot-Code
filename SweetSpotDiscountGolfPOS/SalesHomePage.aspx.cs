using System;
using SweetShop;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        //private String connectionString;
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        EmployeeManager em = new EmployeeManager();
        CurrentUser cu;
        //public SalesHomePage()
        //{
        //    connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesHomePage.aspx";
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
                    //Sets the calendar and text boxes start and end dates
                    calStartDate.SelectedDate = DateTime.Today;
                    calEndDate.SelectedDate = DateTime.Today;
                    ddlLocation.SelectedValue = cu.locationID.ToString();

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
        protected void btnQuickSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnQuickSale_Click";
            try
            {
                //Sets transaction type to sale
                Session["TranType"] = 1;
                //Sets customer id to guest cust
                Session["key"] = 1;
                //Changes page to Sales Cart
                Server.Transfer("SalesCart.aspx", false);
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
        protected void btnReturns_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturns_Click";
            try
            {
                //Changes page to Returns Home Page
                Server.Transfer("ReturnsHomePage.aspx", false);
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

        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calStart_SelectionChanged";
            try
            {
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
        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calEnd_SelectionChanged";
            try
            {
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

        protected void btnInvoiceSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSearch_Click";
            try
            {
                List<Invoice> fullInvoices = new List<Invoice>();
                //Retrieves dates from Calendar
                DateTime dtmStartDate = calStartDate.SelectedDate;
                DateTime dtmEndDate = calEndDate.SelectedDate;
                if (txtInvoiceNum.Text != "")
                {
                    //Searches through invoices using invoice number
                    fullInvoices = ssm.getInvoice(Convert.ToInt32(txtInvoiceNum.Text));
                    if (!fullInvoices.Any())
                    {
                        fullInvoices = ssm.getInvoiceFromSku(Convert.ToInt32(txtInvoiceNum.Text), Convert.ToInt32(ddlLocation.SelectedValue));
                    }
                }
                else
                {
                    //Searches through invoices using customer name and date
                    fullInvoices = ssm.getInvoiceBetweenDates(dtmStartDate, dtmEndDate);
                }
                List<Invoice> viewInvoices = new List<Invoice>();
                //Loops through each invoice
                foreach (var i in fullInvoices)
                {
                    if (Convert.ToInt32(ddlLocation.SelectedValue) == i.locationID)
                    {
                        //Sets customer and employee class for the last invoice
                        Customer c = ssm.GetCustomerbyCustomerNumber(i.customerID);
                        Employee emp = em.getEmployeeByID(i.employeeID);
                        //Uses the classes to set customer name and employee name of each invoice
                        Invoice iv = new Invoice(i.invoiceNum, i.invoiceSub, i.invoiceDate, c.firstName + " " + c.lastName, i.discountAmount, i.tradeinAmount, i.subTotal, i.governmentTax, i.provincialTax, i.balanceDue, emp.firstName + " " + emp.lastName);
                        //Adds each invoice to invoice list
                        viewInvoices.Add(iv);
                    }
                }
                //Binds invoice list to the grid view
                grdInvoiceSelection.DataSource = viewInvoices;
                grdInvoiceSelection.DataBind();
                //Center the mop grid view
                foreach (GridViewRow row in grdInvoiceSelection.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }

                //Stores invoices in session
                Session["searchReturnInvoices"] = fullInvoices;
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
        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoiceSelection_RowCommand";
            try
            {
                //Sets the string of the command argument(invoice number
                string strInvoice = Convert.ToString(e.CommandArgument);
                //Splits the invoice string into numbers
                int invNum = Convert.ToInt32(strInvoice.Split('-')[0]);
                int invSNum = Convert.ToInt32(strInvoice.Split('-')[1]);
                //Checks that the command name is return invoice
                if (e.CommandName == "returnInvoice")
                {
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
                    //Session["Invoice"] = strInvoice;
                    Session["actualInvoiceInfo"] = rInvoice;
                    Session["useInvoice"] = true;
                    //Session["strDate"] = rInvoice.invoiceDate;
                    Session["ItemsInCart"] = ssm.invoice_getItems(invNum, invSNum, "tbl_invoiceItem" + table);
                    Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invNum, invSNum, "tbl_invoice");
                    Session["MethodsOfPayment"] = ssm.invoice_getMOP(invNum, invSNum, "tbl_invoiceMOP");
                    Session["TranType"] = tran;
                    //Changes to printable invoice page
                    Server.Transfer("PrintableInvoice.aspx", false);
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