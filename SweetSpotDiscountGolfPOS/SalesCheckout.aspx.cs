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

    public partial class SalesCheckout : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        InvoiceManager IM = new InvoiceManager();
        private static Invoice invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCheckout.aspx";
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
                        List<Tax> t = new List<Tax>();
                        TaxManager TM = new TaxManager();

                        //Checks if shipping was charged 
                        invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];

                        object[] taxText = { "Add GST", "Add PST" };
                        //This is where the taxes are set?
                        object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails);
                        invoice = (Invoice)results[0];
                        object[] taxStatus = (object[])results[1];
                        if (Convert.ToBoolean(taxStatus[0]))
                        {
                            lblGovernment.Visible = true;
                            if (invoice.bitChargeGST)
                            {
                                lblGovernmentAmount.Text = "$ " + invoice.fltGovernmentTaxAmount.ToString("#0.00");
                            }
                            else
                            {
                                lblGovernmentAmount.Text = "$ 0.00";
                            }
                            lblGovernmentAmount.Visible = true;
                            btnRemoveGov.Text = taxStatus[1].ToString();
                            btnRemoveGov.Visible = true;
                        }
                        if (Convert.ToBoolean(taxStatus[2]))
                        {
                            lblProvincial.Visible = true;
                            if (invoice.bitChargePST)
                            {
                                lblProvincialAmount.Text = "$ " + invoice.fltProvincialTaxAmount.ToString("#0.00");
                            }
                            else
                            {
                                lblProvincialAmount.Text = "$ 0.00";
                            }
                            lblProvincialAmount.Visible = true;
                            btnRemoveProv.Text = taxStatus[3].ToString();
                            btnRemoveProv.Visible = true;
                        }
                        UpdatePageTotals();
                        //***Assign each item to its Label.
                        lblTotalInCartAmount.Text = "$ " + (invoice.fltSubTotal + invoice.fltTotalDiscount - invoice.fltTotalTradeIn).ToString("#0.00");
                        lblTotalInDiscountsAmount.Text = "$ " + invoice.fltTotalDiscount.ToString("#0.00");
                        lblTradeInsAmount.Text = "$ " + invoice.fltTotalTradeIn.ToString("#0.00");
                        lblSubTotalAmount.Text = "$ " + (invoice.fltSubTotal + invoice.fltShippingCharges).ToString("#0.00");
                        lblShippingAmount.Text = "$ " + invoice.fltShippingCharges.ToString("#0.00");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtAmountPaying.Text != "")
                {
                    //ClientScript.RegisterStartupScript(GetType(), "sCheckout", "userInput(" + Convert.ToDouble(txtAmountPaying.Text) + ")", true);
                    object[] amounts = verifyTenderAndChange();
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 5, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 2, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 7, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 1, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 6, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                //Gathers the mop info based on the index
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("mopID")).Text);
                //Retrieves Mop list from Session
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.RemoveMopFromList(mopRemovingID, objPageDetails);

                ////Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnRemoveGovTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveGovTax";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                TaxManager TM = new TaxManager();
                InvoiceManager IM = new InvoiceManager();
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                object[] taxText = { btnRemoveGov.Text, "Do Nothing" };
                object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails);
                invoice = (Invoice)results[0];
                object[] taxStatus = (object[])results[1];
                if (Convert.ToBoolean(taxStatus[0]))
                {
                    lblGovernment.Visible = true;
                    if (invoice.bitChargeGST)
                    {
                        lblGovernmentAmount.Text = "$ " + invoice.fltGovernmentTaxAmount.ToString("#0.00");
                    }
                    else
                    {
                        lblGovernmentAmount.Text = "$ 0.00";
                    }
                    lblGovernmentAmount.Visible = true;
                    btnRemoveGov.Text = taxStatus[1].ToString();
                    btnRemoveGov.Visible = true;
                }
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnRemoveProvTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveProvTax";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                TaxManager TM = new TaxManager();
                InvoiceManager IM = new InvoiceManager();
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                object[] taxText = { "Do Nothing", btnRemoveProv.Text };
                object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails);
                invoice = (Invoice)results[0];
                object[] taxStatus = (object[])results[1];
                if (Convert.ToBoolean(taxStatus[2]))
                {
                    lblProvincial.Visible = true;
                    if (invoice.bitChargePST)
                    {
                        lblProvincialAmount.Text = "$ " + invoice.fltProvincialTaxAmount.ToString("#0.00");
                    }
                    else
                    {
                        lblProvincialAmount.Text = "$ 0.00";
                    }
                    lblProvincialAmount.Visible = true;
                    btnRemoveProv.Text = taxStatus[3].ToString();
                    btnRemoveProv.Visible = true;
                }
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Other functionality
        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                IIM.LoopThroughTheItemsToReturnToInventory(invoice.intInvoiceID, objPageDetails);
                IIM.RemoveInitialTotalsForTable(invoice.intInvoiceID, objPageDetails);
                //Changes to the Home page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnExitSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExitSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //TODO: btnExitSale_Click is good as it doesn't read the new values. It removes the entry 
                TaxManager TM = new TaxManager();
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                object[] taxText = { "Remove GST", "Remove PST" };
                object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails);
                invoice.intTransactionTypeID = 1;
                IM.UpdateCurrentInvoice(invoice, objPageDetails);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnLayaway_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnLayaway_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //TaxManager TM = new TaxManager();
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                //object[] taxText = { "Remove GST", "Remove PST" };
                //object[] results = TM.ReturnChargedTaxForSale(I, taxText, objPageDetails);
                //I.transactionType = 6;
                //IM.UpdateCurrentInvoice(I, objPageDetails);
                //Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                object[] taxText = { "Remove GST", "Remove PST" };
                if ((btnRemoveGov.Text).Split(' ')[0] != "Remove")
                {
                    taxText[0] = "Do Nothing";
                }
                if ((btnRemoveProv.Text).Split(' ')[0] != "Remove")
                {
                    taxText[1] = "Do Nothing";
                }
                TaxManager TM = new TaxManager();
                object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails); //UPDATING THE CURRENT SALES TABLE
                //Sets session to true
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                //Changes to Sales Cart page
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Employee
                EmployeeManager EM = new EmployeeManager();
                if (EM.returnCanEmployeeMakeSale(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails))
                {
                    //Checks the amount paid and the bypass check box
                    if (!txtAmountPaying.Text.Equals("0.00"))
                    {
                        //Displays message
                        MessageBox.ShowMessage("Remaining Balance Does NOT Equal $0.00.", this);
                    }
                    else
                    {
                        if (IM.VerifyMOPHasBeenAdded(invoice.intInvoiceID, objPageDetails))
                        {
                            //Stores all the Sales data to the database
                            invoice = IM.ReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];
                            invoice.employee = EM.returnEmployeeFromPassword(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails)[0];
                            invoice.varAdditionalInformation = txtComments.Text;
                            IM.FinalizeInvoice(invoice, "tbl_invoiceItem", objPageDetails);
                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                            Response.Redirect("PrintableInvoice.aspx?" + nameValues, false);
                        }
                        else
                        {
                            MessageBox.ShowMessage("At least one method of payment "
                                + "is required even for a $0.00 sale.", this);
                        }
                    }
                }
                else
                {
                    MessageBox.ShowMessage("Invalid employee passcode entered. "
                    + "Please try again.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Populating gridview with MOPs
        private void populateGridviewMOP(double amountPaid, int methodOfPayment, object[] amounts)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                InvoiceMOPs invoicePayment = new InvoiceMOPs();
                invoicePayment.intInvoiceID = invoice.intInvoiceID;
                invoicePayment.intPaymentID = methodOfPayment;
                invoicePayment.fltAmountPaid = amountPaid;
                invoicePayment.fltTenderedAmount = Convert.ToDouble(amounts[0]);
                invoicePayment.fltCustomerChange = Convert.ToDouble(amounts[1]);
                IMM.AddNewMopToList(invoicePayment, objPageDetails);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                    if (IM.VerifyMOPHasBeenAdded(invoice.intInvoiceID, objPageDetails))
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
                    mopMasterCard.Enabled = false;
                    mopVisa.Enabled = false;
                    btnRemoveGov.Enabled = false;
                    btnRemoveProv.Enabled = false;
                }
                else
                {
                    mopCash.Enabled = true;
                    mopDebit.Enabled = true;
                    mopGiftCard.Enabled = true;
                    mopMasterCard.Enabled = true;
                    mopVisa.Enabled = true;
                    btnRemoveGov.Enabled = true;
                    btnRemoveProv.Enabled = true;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        private void UpdatePageTotals()
        {
            string method = "UpdatePageTotals";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                invoice = IM.ReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];
                //Loops through each mop
                double dblAmountPaid = 0;
                foreach (var payment in invoice.invoiceMops)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += payment.fltAmountPaid;
                }
                gvCurrentMOPs.DataSource = invoice.invoiceMops;
                gvCurrentMOPs.DataBind();
                double tx = 0;
                if (invoice.bitChargeGST)
                {
                    tx += invoice.fltGovernmentTaxAmount;
                }
                if (invoice.bitChargePST)
                {
                    tx += invoice.fltProvincialTaxAmount;
                }
                //Displays the remaining balance
                lblBalanceAmount.Text = "$ " + (invoice.fltBalanceDue + invoice.fltShippingCharges + tx).ToString("#0.00");
                lblRemainingBalanceDueDisplay.Text = "$ " + ((invoice.fltBalanceDue + invoice.fltShippingCharges + tx) - dblAmountPaid).ToString("#0.00");
                txtAmountPaying.Text = ((invoice.fltBalanceDue + invoice.fltShippingCharges + tx) - dblAmountPaid).ToString("#0.00");
                buttonDisable(((invoice.fltBalanceDue + invoice.fltShippingCharges + tx) - dblAmountPaid));
            }
            catch (ThreadAbortException tae) { }
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
        private object[] verifyTenderAndChange()
        {
            double cash = Convert.ToDouble(txtAmountPaying.Text);
            double paid = cash;
            double change = 0;
            if (!hdnTender.Value.Equals(0))
            {
                if (hdnTender.Value != "")
                {
                    paid = Convert.ToDouble(hdnTender.Value);
                    change = Convert.ToDouble(hdnChange.Value);
                }
            }
            object[] amounts = { paid, change };
            return amounts;
        }
    }
}