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
    public partial class ReturnsHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        EmployeeManager em = new EmployeeManager();
        CurrentUser cu;
        
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
        //Searches invoices and displays them 
        //By date or customer
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSearch_Click";
            try
            {
                //Sets the search by invoice or customer
                bool bolSearchInvoice = rdbSearchByInvoiceNumber.Checked;
                //Sets the searched string
                string strSearch = txtInvoiceSearch.Text;
                List<Invoice> fullInvoices = new List<Invoice>();
                Nullable<DateTime> dtmSearch;
                //Checks to see if date is empty
                if (txtSearchDate.Text == "")
                {
                    //sets a null
                    dtmSearch = null;
                }
                else
                {
                    //Retrieves date from text box
                    dtmSearch = DateTime.ParseExact(txtSearchDate.Text, "M/dd/yy", null);
                }
                //Checks how the user wants to search
                if (bolSearchInvoice)
                {
                    //Searches through invoices using invoice number
                    fullInvoices = ssm.getInvoice(Convert.ToInt32(strSearch));
                }
                else
                {
                    //Searches through invoices using customer name and date
                    fullInvoices = ssm.multiTypeSearchInvoices(strSearch, dtmSearch);
                }
                List<Invoice> viewInvoices = new List<Invoice>();
                //Loops through each invoice
                foreach (var i in fullInvoices)
                {
                    //Sets customer and employee class for the last invoice
                    Customer c = ssm.GetCustomerbyCustomerNumber(i.customerID);
                    Employee emp = em.getEmployeeByID(i.employeeID);
                    //Uses the classes to set customer name and employee name of each invoice
                    Invoice iv = new Invoice(i.invoiceNum, i.invoiceSub, i.invoiceDate, c.firstName + " " + c.lastName, i.balanceDue, lm.locationName(i.locationID), emp.firstName + " " + emp.lastName);
                    //Adds each invoice to invoice list
                    viewInvoices.Add(iv);
                }
                //Binds invoice list to the grid view
                grdInvoiceSelection.DataSource = viewInvoices;
                grdInvoiceSelection.DataBind();
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
                    //Retrieves all the invoices that were searched
                    List<Invoice> combData = (List<Invoice>)Session["searchReturnInvoices"];
                    Invoice returnInvoice = new Invoice();
                    //Loops through each invoice
                    foreach (var inv in combData)
                    {
                        //Checks to match the selected invoice number with the searched invoice
                        if (inv.invoiceNum == invNum && inv.invoiceSub == invSNum)
                        {
                            //Sets customer class based on the found invoice customer number
                            Customer c = ssm.GetCustomerbyCustomerNumber(inv.customerID);
                            //Sets invoice and customer name
                            returnInvoice = inv;
                            returnInvoice.customerName = c.firstName + " " + c.lastName;
                            //Sets the Customer key id
                            Session["key"] = inv.customerID;
                            //Sets the session to the single invoice
                            Session["searchReturnInvoices"] = returnInvoice;
                        }
                    }
                    //Sets transaction type to return
                    Session["TranType"] = 2;
                    //Changes to Returns cart
                    Server.Transfer("ReturnsCart.aspx", false);
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