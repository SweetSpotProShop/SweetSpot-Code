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
    public partial class ReturnsCheckout : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        InvoiceManager IM = new InvoiceManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCheckout.aspx";
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
                        List<Tax> t = new List<Tax>();
                        TaxManager TM = new TaxManager();
                        Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        //Retrieve taxes based on current location
                        string gTax = "Do Nothing";
                        string pTax = "Do Nothing";
                        if (I.governmentTax != 0) { gTax = "Add GST"; }
                        if (I.provincialTax != 0) { pTax = "Add PST"; }
                        object[] taxText = { gTax, pTax };
                        object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                        I = (Invoice)results[0];
                        object[] taxStatus = (object[])results[1];
                        if (Convert.ToBoolean(taxStatus[0]))
                        {
                            lblGovernment.Visible = true;
                            lblGovernmentAmount.Text = "$ " + I.governmentTax.ToString("#0.00");
                            lblGovernmentAmount.Visible = true;
                            I.balanceDue = I.balanceDue + I.governmentTax;
                            IM.UpdateCurrentInvoice(I);
                        }
                        if (Convert.ToBoolean(taxStatus[2]))
                        {
                            lblProvincial.Visible = true;
                            lblProvincialAmount.Text = "$ " + I.provincialTax.ToString("#0.00");
                            lblProvincialAmount.Visible = true;
                            I.balanceDue = I.balanceDue + I.provincialTax;
                            IM.UpdateCurrentInvoice(I);
                        }
                        UpdatePageTotals();
                        lblRefundSubTotalAmount.Text = "$ " + I.subTotal.ToString("#0.00");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Cash", amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //MasterCard
        protected void mopMasterCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopMasterCard_Click";
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "MasterCard", amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Debit", amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Visa
        protected void mopVisa_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopVisa_Click";
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Visa", amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Gift Card", amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            try
            {
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text);
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.RemoveMopFromList(mopRemovingID, Request.QueryString["inv"].ToString());
                //Clear the row index
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Other functionality
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                IIM.LoopThroughTheItemsToReturnToInventory(Request.QueryString["inv"].ToString());
                IIM.RemoveInitialTotalsForTable(Request.QueryString["inv"].ToString());
                //Change page to the Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnReturnToCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnToCart_Click";
            try
            {
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                //Changes page to Returns Cart page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inv", Request.QueryString["inv"].ToString());
                Response.Redirect("ReturnsCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnFinalize_Click";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Checks to make sure total is 0
                if (!txtAmountRefunding.Text.Equals("0.00"))
                {
                    //Displays message box that refund will need to = 0
                    MessageBox.ShowMessage("Remaining Refund Does NOT Equal 0.", this);
                }
                else
                {
                    if (IM.VerifyMOPHasBeenAdded(Request.QueryString["inv"].ToString()))
                    {
                        IM.FinalizeInvoice(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0], txtComments.Text, "tbl_invoiceItemReturns");
                        string printableInvoiceNum = Request.QueryString["inv"].ToString().Split('-')[1] + "-" + Request.QueryString["inv"].ToString().Split('-')[2];
                        var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        nameValues.Set("inv", printableInvoiceNum);
                        Response.Redirect("PrintableInvoice.aspx?" + nameValues, false);
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 return.", this);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Populating gridview with MOPs
        protected void populateGridviewMOP(double amountPaid, string methodOfPayment, object[] amounts)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            try
            {
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.AddNewMopToList(Request.QueryString["inv"].ToString(), amountPaid, methodOfPayment, amounts);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        public void buttonDisable(double rb)
        {
            string method = "buttonDisable";
            try
            {
                if (rb >= -.001 && rb <= 0.001)
                {
                    mopCash.Enabled = false;
                    mopDebit.Enabled = false;
                    mopGiftCard.Enabled = false;
                    mopMasterCard.Enabled = false;
                    mopVisa.Enabled = false;
                }
                else
                {
                    mopCash.Enabled = true;
                    mopDebit.Enabled = true;
                    mopGiftCard.Enabled = true;
                    mopMasterCard.Enabled = true;
                    mopVisa.Enabled = true;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void UpdatePageTotals()
        {
            string method = "UpdatePageTotals";
            try
            {
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                //Loops through each mop
                double dblAmountPaid = 0;
                foreach (var mop in I.usedMops)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += mop.amountPaid;
                }
                gvCurrentMOPs.DataSource = I.usedMops;
                gvCurrentMOPs.DataBind();
                //Displays the remaining balance
                lblRefundBalanceAmount.Text = "$ " + I.balanceDue.ToString("#0.00");
                lblRemainingRefundDisplay.Text = "$ " + (I.balanceDue - dblAmountPaid).ToString("#0.00");
                txtAmountRefunding.Text = (I.balanceDue - dblAmountPaid).ToString("#0.00");
                buttonDisable(I.balanceDue - dblAmountPaid);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch(Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}