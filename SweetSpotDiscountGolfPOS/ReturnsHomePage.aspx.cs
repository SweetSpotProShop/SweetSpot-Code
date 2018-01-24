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
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        InvoiceManager IM = new InvoiceManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesHomePage.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                calSearchDate.SelectedDate = DateTime.Today;
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
        //Searches invoices and displays them 
        //By date or customer
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSearch_Click";
            try
            {
                grdInvoiceSelection.DataSource = IM.ReturnInvoicesBasedOnSearchForReturns(txtInvoiceSearch.Text, calSearchDate.SelectedDate);
                grdInvoiceSelection.DataBind();
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
                            //Customer c = ssm.GetCustomerbyCustomerNumber(inv.customerID);
                            //Sets invoice and customer name
                            returnInvoice = inv;
                            //returnInvoice.customerName = c.firstName + " " + c.lastName;
                            //Sets the Customer key id
                            //Session["key"] = inv.customerID;
                            //Sets the session to the single invoice
                            Session["searchReturnInvoices"] = returnInvoice;
                        }
                    }
                    //Sets transaction type to return
                    Session["TranType"] = 2;
                    //Changes to Returns cart
                    Response.Redirect("ReturnsCart.aspx", false);
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