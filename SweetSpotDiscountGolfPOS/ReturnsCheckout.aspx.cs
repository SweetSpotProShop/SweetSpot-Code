using System;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReturnsCheckout : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly TaxManager TM = new TaxManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCheckout.aspx";
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

                        Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                        UpdatePageTotals();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Cash
        protected void MopCash_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopCash_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), 5, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //MasterCard
        protected void MopMasterCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopMasterCard_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), 2, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Debit
        protected void MopDebit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopDebit_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), 7, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Visa
        protected void MopVisa_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopVisa_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), 1, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Gift Card
        protected void MopGiftCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopGiftCard_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountRefunding.Text != "")
                {
                    object[] amounts = { txtAmountRefunding.Text, 0 };
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), 6, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text);
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.CallRemoveMopFromList(mopRemovingID, objPageDetails);
                //Clear the row index
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Other functionality
        protected void BtnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                IIM.LoopThroughTheItemsToReturnToInventory(returnInvoice.intInvoiceID, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                IIM.CallRemoveInitialTotalsForTable(returnInvoice.intInvoiceID, objPageDetails);
                //Change page to the Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnReturnToCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnToCart_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Changes page to Returns Cart page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("invoice", Request.QueryString["invoice"].ToString());
                Response.Redirect("ReturnsCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnFinalize_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnFinalize_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                CU = (CurrentUser)Session["currentUser"];
                EmployeeManager EM = new EmployeeManager();
                if (EM.ReturnCanEmployeeMakeSale(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails))
                {
                    //Checks to make sure total is 0
                    if (!txtAmountRefunding.Text.Equals("0.00"))
                    {
                        //Displays message box that refund will need to = 0
                        MessageBoxCustom.ShowMessage("Remaining Refund Does NOT Equal 0.", this);
                    }
                    else
                    {
                        if (IM.CallVerifyMOPHasBeenAdded(returnInvoice.intInvoiceID, objPageDetails))
                        {
                            returnInvoice.employee = EM.CallReturnEmployeeFromPassword(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails)[0];
                            returnInvoice.varAdditionalInformation = txtComments.Text;
                            IM.FinalizeInvoice(returnInvoice, "tbl_invoiceItemReturns", objPageDetails);
                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("invoice", returnInvoice.intInvoiceID.ToString());
                            Response.Redirect("PrintableInvoiceReturn.aspx?" + nameValues, false);
                        }
                        else
                        {
                            MessageBoxCustom.ShowMessage("At least one method of payment "
                                + "is required even for a $0.00 return.", this);
                        }
                    }
                }
                else
                {
                    MessageBoxCustom.ShowMessage("Invalid employee passcode entered. "
                    + "Please try again.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Populating gridview with MOPs
        protected void PopulateGridviewMOP(double amountPaid, int paymentID, object[] amounts)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                InvoiceMOPs invoicePayment = new InvoiceMOPs
                {
                    intInvoiceID = Convert.ToInt32(Request.QueryString["invoice"].ToString()),
                    intPaymentID = paymentID,
                    fltAmountPaid = amountPaid,
                    fltTenderedAmount = Convert.ToDouble(amounts[0]),
                    fltCustomerChange = Convert.ToDouble(amounts[1])
                };
                IMM.CallAddNewMopToList(invoicePayment, objPageDetails);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        public void ButtonDisable(double rb)
        {
            string method = "buttonDisable";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (rb >= -.001 && rb <= 0.001)
                {
                    MopCash.Enabled = false;
                    MopDebit.Enabled = false;
                    MopGiftCard.Enabled = false;
                    MopMasterCard.Enabled = false;
                    MopVisa.Enabled = false;
                }
                else
                {
                    MopCash.Enabled = true;
                    MopDebit.Enabled = true;
                    MopGiftCard.Enabled = true;
                    MopMasterCard.Enabled = true;
                    MopVisa.Enabled = true;
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void UpdatePageTotals()
        {
            string method = "UpdatePageTotals";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                //Loops through each mop
                double dblAmountPaid = 0;
                foreach (var payment in returnInvoice.invoiceMops)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += payment.fltAmountPaid;
                }
                gvCurrentMOPs.DataSource = returnInvoice.invoiceMops;
                gvCurrentMOPs.DataBind();



                double governmentTax = 0;
                double harmonizedTax = 0;
                double liquorTax = 0;
                double provincialTax = 0;
                double quebecTax = 0;
                double retailTax = 0;


                foreach (var invoiceItem in returnInvoice.invoiceItems)
                {
                    foreach(var invoiceItemTax in invoiceItem.invoiceItemTaxes)
                    {
                        if(invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("GST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                governmentTax += invoiceItemTax.fltTaxAmount;
                                lblGovernment.Visible = true;
                                lblGovernment.Text = invoiceItemTax.varTaxName;
                                lblGovernmentAmount.Visible = true;
                            }
                        }
                        else if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("HST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                harmonizedTax += invoiceItemTax.fltTaxAmount;
                                lblGovernment.Visible = true;
                                lblGovernment.Text = invoiceItemTax.varTaxName;
                                lblGovernmentAmount.Visible = true;
                            }
                        }
                        else if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("LCT", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                liquorTax += invoiceItemTax.fltTaxAmount;
                                lblLiquorTax.Visible = true;
                                lblLiquorTax.Text = invoiceItemTax.varTaxName;
                                lblLiquorTaxAmount.Visible = true;
                            }
                        }
                        else if(invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("PST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                provincialTax += invoiceItemTax.fltTaxAmount;
                                lblProvincial.Visible = true;
                                lblProvincial.Text = invoiceItemTax.varTaxName;
                                lblProvincialAmount.Visible = true;
                            }
                        }
                        else if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("QST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                quebecTax += invoiceItemTax.fltTaxAmount;
                                lblProvincial.Visible = true;
                                lblProvincial.Text = invoiceItemTax.varTaxName;
                                lblProvincialAmount.Visible = true;
                            }
                        }
                        else if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("RST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                retailTax += invoiceItemTax.fltTaxAmount;
                                lblProvincial.Visible = true;
                                lblProvincial.Text = invoiceItemTax.varTaxName;
                                lblProvincialAmount.Visible = true;
                            }
                        }
                    }
                }

                lblGovernmentAmount.Text = (governmentTax + harmonizedTax).ToString("C");
                lblProvincialAmount.Text = (provincialTax + quebecTax + retailTax).ToString("C");
                lblLiquorTaxAmount.Text = liquorTax.ToString("C");

                double tx = governmentTax + harmonizedTax + liquorTax + provincialTax + quebecTax + retailTax;

                //Displays the remaining balance
                lblRefundBalanceAmount.Text = (returnInvoice.fltBalanceDue + tx).ToString("C");
                lblRefundSubTotalAmount.Text = returnInvoice.fltSubTotal.ToString("C");
                lblRemainingRefundDisplay.Text = ((returnInvoice.fltBalanceDue + tx) - dblAmountPaid).ToString("C");
                txtAmountRefunding.Text = ((returnInvoice.fltBalanceDue + tx) - dblAmountPaid).ToString("#0.00");
                ButtonDisable((returnInvoice.fltBalanceDue + tx) - dblAmountPaid);

                returnInvoice.fltGovernmentTaxAmount = governmentTax;
                returnInvoice.fltHarmonizedTaxAmount = harmonizedTax;
                returnInvoice.fltLiquorTaxAmount = liquorTax;
                returnInvoice.fltProvincialTaxAmount = provincialTax;
                returnInvoice.fltQuebecTaxAmount = quebecTax;
                returnInvoice.fltRetailTaxAmount = retailTax;
                IM.CallUpdateCurrentInvoice(returnInvoice, objPageDetails);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}