using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (amountPaid > 0)
                {
                    amountPaid = amountPaid * -1;
                }
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.AddNewMopToReceiptList(Request.QueryString["receipt"].ToString(), amountPaid, methodOfPayment, chequeNumber, objPageDetails);
                //Center the mop grid view
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Retrieves index of selected row
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text);
                //double paidAmount = double.Parse(gvCurrentMOPs.Rows[index].Cells[2].Text, NumberStyles.Currency);
                //Retrieves Mop list from Session
                //List<Mops> tempMopList = (List<Mops>)Session["MethodsofPayment"];
                //Loops through each mop in list
                //foreach (var mop in tempMopList)
                //{
                //Checks if the mop id do not match
                //if (mop.tableID != mopRemovingID)
                //{
                //Not selected mop add back into the mop list
                ////mopList.Add(mop);
                //Calculate amount paid
                ////dblAmountPaid += mop.amountPaid;
                //}
                //else
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.RemoveMopFromPurchaseList(mopRemovingID, Request.QueryString["receipt"].ToString(), objPageDetails);
                //{
                //Add removed mops paid amount back into the remaining balance
                ////ckm.dblRemainingBalance += paidAmount;
                //}
                //updtae the new amount paid total
                ////ckm.dblAmountPaid = dblAmountPaid;
                //}
                //Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                //Store updated mops in Session
                ////Session["MethodsofPayment"] = mopList;
                //Bind the session to the grid view
                ////gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                //Display remaining balance
                ////lblRemainingPurchaseDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                ////txtPurchaseAmount.Text = ckm.dblRemainingBalance.ToString("#0.00");
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
            string method = "UpdatePageTotals";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice R = IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString() + "-1", objPageDetails)[0];
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
                buttonDisable(R.balanceDue - dblAmountPaid);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        private void buttonDisable(double rb)
        {
            string method = "buttonDisable";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (rb >= -.001 && rb <= 0.001)
                {
                    if (IM.VerifyPurchaseMOPHasBeenAdded(Request.QueryString["receipt"].ToString(), objPageDetails))
                    {
                        mopCash.Enabled = false;
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 sale.", this);
                    }
                    mopDebit.Enabled = false;
                    mopGiftCard.Enabled = false;
                    mopCheque.Enabled = false;
                }
                else
                {
                    mopCash.Enabled = true;
                    mopDebit.Enabled = true;
                    mopGiftCard.Enabled = true;
                    mopCheque.Enabled = true;
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive the message please contact "
                    + "your system administrator.", this);
            }
        }
        //Other functionality
        protected void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelPurchase_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.CancellingReceipt(IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString(), objPageDetails)[0], objPageDetails);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                Response.Redirect("PurchasesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                    if (IM.VerifyPurchaseMOPHasBeenAdded(Request.QueryString["receipt"].ToString(), objPageDetails))
                    {
                        //Stores all the Sales data to the database
                        IM.FinalizeReceipt(IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString(), objPageDetails)[0], txtComments.Text, "tbl_receiptItem", objPageDetails);
                        string printableInvoiceNum = Request.QueryString["receipt"].ToString().Split('-')[1];
                        var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        nameValues.Set("receipt", printableInvoiceNum);
                        //Changes page to printable invoice
                        Response.Redirect("PrintableReceipt.aspx?" + nameValues, false);
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 sale.", this);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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