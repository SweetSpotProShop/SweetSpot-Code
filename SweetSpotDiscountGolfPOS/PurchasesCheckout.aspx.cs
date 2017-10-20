using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class PurchasesCheckout : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        List<Checkout> mopList = new List<Checkout>();
        List<Cart> itemsInCart = new List<Cart>();
        ItemDataUtilities idu = new ItemDataUtilities();
        CheckoutManager ckm = new CheckoutManager();
        CurrentUser cu;
        public double dblRemaining;
        public double subtotal;
        public double gst;
        public double pst;
        public double balancedue;
        public double dblAmountPaid;
        public double tradeInCost;
        public double taxAmount;
        //Remove Prov or Gov Tax
        double amountPaid;
        int tranType;
        int gridID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PurchasesCheckout.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!Page.IsPostBack)
                {
                    //Retrieves items in the cart from Session
                    List<Cart> cart = (List<Cart>)Session["ItemsInCart"];
                    CalculationManager cm = new CalculationManager();
                    //Retrieves date from session
                    DateTime recDate = Convert.ToDateTime(Session["strDate"]);
                    //Creates checkout manager based on current items in cart
                    ckm.dblTotal = ssm.returnPurchaseAmount(cart);
                    ckm.dblRemainingBalance = ckm.dblTotal;
                    //Checks if there are any stored methods of payment
                    if (Session["MethodsofPayment"] != null)
                    {
                        //Retrieve Mops from session
                        mopList = (List<Checkout>)Session["MethodsofPayment"];
                        //Loops through each mop
                        foreach (var mop in mopList)
                        {
                            //Adds amount of each mop to the amount paid total
                            dblAmountPaid += mop.amountPaid;
                        }
                        //Binds mops to grid view
                        gvCurrentMOPs.DataSource = mopList;
                        gvCurrentMOPs.DataBind();
                        //Update the amount paid and the remaining balance
                        ckm.dblAmountPaid = dblAmountPaid;
                        ckm.dblRemainingBalance = ckm.dblTotal - ckm.dblAmountPaid;
                    }

                    //***Assign each item to its Label.
                    lblTotalPurchaseAmount.Text = "$ " + ckm.dblTotal.ToString("#0.00");
                    lblRemainingPurchaseDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                    //Stores totals in the checkout manager
                    Session["CheckOutTotals"] = ckm;
                    //Updates the amount paying with the remaining balance
                    txtPurchaseAmount.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
        //Cash
        protected void mopCash_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopCash_Click";
            try
            {
                //Retrieves checkout totals from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtPurchaseAmount.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "Cash";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment, 0);
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
        //Cheque
        protected void mopCheque_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopCheque_Click";
            try
            {
                //Retrieves checkout totals from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtPurchaseAmount.Text;
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collect mop type
                    string methodOfPayment = "Cheque";
                    //Calls procedure to add it to the grid view
                    populateGridviewMOP(amountPaid, methodOfPayment, Convert.ToInt32(txtChequeNumber.Text));
                    txtChequeNumber.Text = "0000";
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
        //Debit
        protected void mopDebit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopDebit_Click";
            try
            {
                //Retrieves checkout totals from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtPurchaseAmount.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "Debit";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment, 0);
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
        //Gift Card
        protected void mopGiftCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopGiftCard_Click";
            try
            {
                //Retrieves checkout totals from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtPurchaseAmount.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "Gift Card";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment, 0);
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
        //Populating gridview with MOPs
        protected void populateGridviewMOP(double amountPaid, string methodOfPayment, int chequeNumber)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            try
            {
                gridID = 0;
                //Checks if there are any current mops used
                if (Session["MethodsofPayment"] != null)
                {
                    //Retrieves current mops from Session
                    mopList = (List<Checkout>)Session["MethodsofPayment"];
                    //Loops through each mop
                    foreach (var mop in mopList)
                    {
                        //Sets grid id to be the largest current id in table
                        if (mop.tableID > gridID)
                            gridID = mop.tableID;
                    }
                }
                //Sets a temp checkout with the new Mop
                Checkout tempCK = new Checkout(methodOfPayment, amountPaid, gridID + 1, chequeNumber);
                //Retrieves totals for check out from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Adds new mop to the current mop list
                mopList.Add(tempCK);
                //Loops through each mop
                foreach (var mop in mopList)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += mop.amountPaid;
                }
                //Updates the amount paid and remaining balance in the checkout manager
                ckm.dblAmountPaid = dblAmountPaid;
                ckm.dblRemainingBalance -= amountPaid;
                //Binds the moplist to the gridview
                gvCurrentMOPs.DataSource = mopList;
                gvCurrentMOPs.DataBind();
                //Center the mop grid view
                foreach (GridViewRow row in gvCurrentMOPs.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }
                //Store moplist into session
                Session["MethodsofPayment"] = mopList;
                //Sets the remaining balance due
                txtPurchaseAmount.Text = ckm.dblRemainingBalance.ToString("#0.00");
                lblRemainingPurchaseDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
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
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            try
            {
                //Retrieves index of selected row
                int index = e.RowIndex;
                //Retrieves the checkout manager from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Gathers the mop info based on the index
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[index].Cells[3].FindControl("lblTableID")).Text);
                double paidAmount = double.Parse(gvCurrentMOPs.Rows[index].Cells[2].Text, NumberStyles.Currency);
                //Retrieves Mop list from Session
                List<Checkout> tempMopList = (List<Checkout>)Session["MethodsofPayment"];
                //Loops through each mop in list
                foreach (var mop in tempMopList)
                {
                    //Checks if the mop id do not match
                    if (mop.tableID != mopRemovingID)
                    {
                        //Not selected mop add back into the mop list
                        mopList.Add(mop);
                        //Calculate amount paid
                        dblAmountPaid += mop.amountPaid;
                    }
                    else
                    {
                        //Add removed mops paid amount back into the remaining balance
                        ckm.dblRemainingBalance += paidAmount;
                    }
                    //updtae the new amount paid total
                    ckm.dblAmountPaid = dblAmountPaid;
                }
                //Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                //Store updated mops in Session
                Session["MethodsofPayment"] = mopList;
                //Bind the session to the grid view
                gvCurrentMOPs.DataSource = mopList;
                gvCurrentMOPs.DataBind();
                //Display remaining balance
                lblRemainingPurchaseDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtPurchaseAmount.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
        //Other functionality
        protected void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelPurchase_Click";
            try
            {
                //Checks session to see if it's null
                if (Session["ItemsInCart"] != null)
                {
                    //Retrieves items in the cart from Session
                    itemsInCart = (List<Cart>)Session["ItemsInCart"];
                }
                //Nullifies each of the sessions 
                Session["key"] = null;
                Session["ItemsInCart"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["Invoice"] = null;
                Session["TranType"] = null;
                Session["strDate"] = null;
                //Changes to the Home page
                Server.Transfer("HomePage.aspx", false);
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
        protected void btnReturnToPurchaseCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnToPurchaseCart_Click";
            try
            {
                //Sets session to true
                //Changes to Sales Cart page
                Server.Transfer("PurchasesCart.aspx", false);
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
        protected void btnFinalizePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnFinalizePurchase_Click";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //Checks the amount paid and the bypass check box
                if (!txtPurchaseAmount.Text.Equals("0.00"))
                {
                    //Displays message
                    MessageBox.ShowMessage("Remaining Amount Does NOT Equal $0.00.", this);
                }
                else
                {
                    //Gathering needed information for the invoice
                    //Retrieves transaction type from Session
                    tranType = Convert.ToInt32(Session["TranType"]);
                    List<Cart> cart = new List<Cart>();
                    //Determines the Cart to be used based on transaction
                    if (tranType == 5) { cart = (List<Cart>)Session["ItemsInCart"]; }

                    //Customer
                    int custNum = Convert.ToInt32(Session["key"]);
                    Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //Employee
                    //******Need to get the employee somehow
                    EmployeeManager em = new EmployeeManager();
                    //int empNum = idu.returnEmployeeIDfromPassword(Convert.ToInt32(Session["id"]));
                    Employee emp = em.getEmployeeByID(cu.empID);
                    //CheckoutTotals
                    ckm = (CheckoutManager)Session["CheckOutTotals"];
                    //MOP
                    mopList = (List<Checkout>)Session["MethodsofPayment"];

                    //Stores all the Sales data to the database
                    idu.mainPurchaseInvoice(ckm, cart, mopList, c, emp, tranType, (Session["Invoice"]).ToString(), txtComments.Text);

                    //Nullifies all related sessions
                    Session["shipping"] = null;
                    Session["ShippingAmount"] = null;
                    Session["searchReturnInvoices"] = null;
                    //Changes page to printable invoice
                    Server.Transfer("PrintableReceipt.aspx", false);
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