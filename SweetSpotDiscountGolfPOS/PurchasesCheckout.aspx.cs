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
        ErrorReporting ER = new ErrorReporting();
        InvoiceManager IM = new InvoiceManager();
        CurrentUser CU;


        //SweetShopManager ssm = new SweetShopManager();
        //List<Mops> mopList = new List<Mops>();
        //List<Cart> itemsInCart = new List<Cart>();
        //ItemDataUtilities idu = new ItemDataUtilities();
        //CheckoutManager ckm = new CheckoutManager();
        //public double dblRemaining;
        //public double subtotal;
        //public double gst;
        //public double pst;
        //public double balancedue;
        //public double dblAmountPaid;
        //public double tradeInCost;
        //public double taxAmount;
        ////Remove Prov or Gov Tax
        //double amountPaid;
        //int tranType;
        //int gridID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PurchasesCheckout.aspx";
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    if (!Page.IsPostBack)
                    {
                        //Retrieves items in the cart from Session
                        //List<Cart> cart = (List<Cart>)Session["ItemsInCart"];
                        //SalesCalculationManager cm = new SalesCalculationManager();
                        //Retrieves date from session
                        //DateTime recDate = Convert.ToDateTime(Session["strDate"]);
                        //Creates checkout manager based on current items in cart
                        //ckm.dblTotal = cm.returnPurchaseAmount(cart);
                        //ckm.dblRemainingBalance = ckm.dblTotal;
                        //Checks if there are any stored methods of payment
                        //if (Session["MethodsofPayment"] != null)
                        //{
                        //    //Retrieve Mops from session
                        //    mopList = (List<Mops>)Session["MethodsofPayment"];
                        //    //Loops through each mop
                        //    foreach (var mop in mopList)
                        //    {
                        //        //Adds amount of each mop to the amount paid total
                        //        dblAmountPaid += mop.amountPaid;
                        //    }
                        //    //Binds mops to grid view
                        //    gvCurrentMOPs.DataSource = mopList;
                        //    gvCurrentMOPs.DataBind();
                        //    //Update the amount paid and the remaining balance
                        //    ckm.dblAmountPaid = dblAmountPaid;
                        //    ckm.dblRemainingBalance = ckm.dblTotal - ckm.dblAmountPaid;
                        //}
                        UpdatePageTotals();
                        ////***Assign each item to its Label.
                        //lblTotalPurchaseAmount.Text = "$ " + ckm.dblTotal.ToString("#0.00");
                        //lblRemainingPurchaseDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                        ////Updates the amount paying with the remaining balance
                        //txtPurchaseAmount.Text = ckm.dblRemainingBalance.ToString("#0.00");
                    }
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
        //Cash
        protected void mopCash_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopCash_Click";
            try
            {
                //Checks that string is not empty
                if (txtPurchaseAmount.Text != "")
                {
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), "Cash", 0);
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
        //Cheque
        protected void mopCheque_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopCheque_Click";
            try
            {
                //Checks that string is not empty
                if (txtPurchaseAmount.Text != "")
                {
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), "Cheque", Convert.ToInt32(txtChequeNumber.Text));
                    txtChequeNumber.Text = "0000";
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
        //Debit
        protected void mopDebit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopDebit_Click";
            try
            {
                //Checks that string is not empty
                if (txtPurchaseAmount.Text != "")
                {
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), "Debit", 0);
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
        //Gift Card
        protected void mopGiftCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopGiftCard_Click";
            try
            {
                //Checks that string is not empty
                if (txtPurchaseAmount.Text != "")
                {
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), "Gift Card", 0);
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
        //Populating gridview with MOPs
        protected void populateGridviewMOP(double amountPaid, string methodOfPayment, int chequeNumber)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            try
            {
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.AddNewMopToReceiptList(Request.QueryString["receipt"].ToString(), amountPaid, methodOfPayment, chequeNumber);
                //Center the mop grid view
                UpdatePageTotals();
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

        ////Pick up Here for final code clean up
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            try
            {
                //Retrieves index of selected row
                int index = e.RowIndex;
                //Retrieves the checkout manager from Session
                ////ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Gathers the mop info based on the index
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[index].Cells[3].FindControl("lblTableID")).Text);
                double paidAmount = double.Parse(gvCurrentMOPs.Rows[index].Cells[2].Text, NumberStyles.Currency);
                //Retrieves Mop list from Session
                List<Mops> tempMopList = (List<Mops>)Session["MethodsofPayment"];
                //Loops through each mop in list
                foreach (var mop in tempMopList)
                {
                    //Checks if the mop id do not match
                    if (mop.tableID != mopRemovingID)
                    {
                        //Not selected mop add back into the mop list
                        ////mopList.Add(mop);
                        //Calculate amount paid
                        ////dblAmountPaid += mop.amountPaid;
                    }
                    else
                    {
                        //Add removed mops paid amount back into the remaining balance
                        ////ckm.dblRemainingBalance += paidAmount;
                    }
                    //updtae the new amount paid total
                    ////ckm.dblAmountPaid = dblAmountPaid;
                }
                //Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                //Store updated mops in Session
                ////Session["MethodsofPayment"] = mopList;
                //Bind the session to the grid view
                ////gvCurrentMOPs.DataSource = mopList;
                gvCurrentMOPs.DataBind();
                //Display remaining balance
                ////lblRemainingPurchaseDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                ////txtPurchaseAmount.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
        protected void UpdatePageTotals()
        {
            Invoice R = IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString() + "-1")[0];
            lblTotalPurchaseAmount.Text = "$ " + R.subTotal.ToString("#0.00");

            double dblAmountPaid = 0;
            foreach (var mop in R.usedMops)
            {
                //Adds the total amount paid fropm each mop type
                dblAmountPaid += mop.amountPaid;
            }
            gvCurrentMOPs.DataSource = R.usedMops;
            gvCurrentMOPs.DataBind();


            lblRemainingPurchaseDueDisplay.Text = "$ " + (R.balanceDue - dblAmountPaid).ToString("#0.00");
            //Updates the amount paying with the remaining balance
            txtPurchaseAmount.Text = (R.balanceDue - dblAmountPaid).ToString("#0.00");
        }
        //Other functionality
        protected void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelPurchase_Click";
            try
            {
                IM.CancellingReceipt(IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString() + "-1")[0]);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
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
        protected void btnReturnToPurchaseCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnToPurchaseCart_Click";
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void btnFinalizePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnFinalizePurchase_Click";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Checks the amount paid and the bypass check box
                if (!txtPurchaseAmount.Text.Equals("0.00"))
                {
                    //Displays message
                    MessageBox.ShowMessage("Remaining Amount Does NOT Equal $0.00.", this);
                }
                else
                {
                    //Gathering needed information for the invoice
                    List<Cart> cart = (List<Cart>)Session["ItemsInCart"];
                    //Customer
                    int custNum = Convert.ToInt32(Session["key"]);
                    ////Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //Employee
                    //******Need to get the employee somehow
                    EmployeeManager em = new EmployeeManager();
                    Employee emp = em.getEmployeeByID(CU.emp.employeeID);
                    //CheckoutTotals
                    ////ckm = (CheckoutManager)Session["CheckOutTotals"];
                    //MOP
                    ////mopList = (List<Mops>)Session["MethodsofPayment"];

                    //Stores all the Sales data to the database
                    ////idu.mainPurchaseInvoice(ckm, cart, mopList, c, emp, tranType, (Session["Invoice"]).ToString(), txtComments.Text, CU);
                    //Nullifies all related sessions
                    Session["shipping"] = null;
                    Session["ShippingAmount"] = null;
                    Session["searchReturnInvoices"] = null;
                    //Changes page to printable invoice
                    Response.Redirect("PrintableReceipt.aspx", false);
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
    }
}