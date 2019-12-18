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
        //private static Invoice receipt;

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
                        UpdatePageTotals();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), 5, 0);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), 8, Convert.ToInt32(txtChequeNumber.Text));
                    txtChequeNumber.Text = "0000";
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), 7, 0);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    populateGridviewMOP(Convert.ToDouble(txtPurchaseAmount.Text), 6, 0);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Populating gridview with MOPs
        protected void populateGridviewMOP(double amountPaid, int paymentID, int chequeNumber)
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
                InvoiceMOPs payment = new InvoiceMOPs();
                payment.intInvoiceID = Convert.ToInt32(Request.QueryString["receipt"]);
                payment.fltAmountPaid = amountPaid;
                payment.intPaymentID = paymentID;
                payment.intChequeNumber = chequeNumber;
                IMM.AddNewMopToReceiptList(payment, objPageDetails);
                //Center the mop grid view
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Retrieves index of selected row
                int receiptPaymentID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text);
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.RemoveMopFromPurchaseList(receiptPaymentID, Convert.ToInt32(Request.QueryString["receipt"]), objPageDetails);
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                lblTotalPurchaseAmount.Text = "$ " + receipt.fltSubTotal.ToString("#0.00");

                double dblAmountPaid = 0;
                foreach (var payment in receipt.invoiceMops)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += payment.fltAmountPaid;
                }
                gvCurrentMOPs.DataSource = receipt.invoiceMops;
                gvCurrentMOPs.DataBind();


                lblRemainingPurchaseDueDisplay.Text = "$ " + (receipt.fltBalanceDue - dblAmountPaid).ToString("#0.00");
                //Updates the amount paying with the remaining balance
                txtPurchaseAmount.Text = (receipt.fltBalanceDue - dblAmountPaid).ToString("#0.00");
                buttonDisable(receipt.fltBalanceDue - dblAmountPaid);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    if (IM.VerifyPurchaseMOPHasBeenAdded(Convert.ToInt32(Request.QueryString["receipt"].ToString()), objPageDetails))
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                IM.CancellingReceipt(IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                nameValues.Set("customer", Request.QueryString["customer"].ToString());
                Response.Redirect("PurchasesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    if (IM.VerifyPurchaseMOPHasBeenAdded(Convert.ToInt32(Request.QueryString["receipt"].ToString()), objPageDetails))
                    {
                        //Stores all the Sales data to the database
                        Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                        receipt.varAdditionalInformation = txtComments.Text;
                        IM.FinalizeReceipt(receipt, "tbl_receiptItem", objPageDetails);
                        var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        nameValues.Set("receipt", receipt.intInvoiceID.ToString());
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}