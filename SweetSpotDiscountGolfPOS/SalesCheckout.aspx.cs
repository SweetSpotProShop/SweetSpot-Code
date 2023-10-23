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

    public partial class SalesCheckout : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly InvoiceItemsManager IIM = new InvoiceItemsManager();
        CurrentUser CU;
        //private static Invoice invoice;
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
                        //List<Tax> t = new List<Tax>();
                        //TaxManager TM = new TaxManager();

                        //Checks if shipping was charged 
                        //Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];                        
                        UpdatePageTotals();
                        Invoice invoice = (Invoice)Session["currentInvoice"];
                        TaxChecker(invoice, objPageDetails);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                if (txtAmountPaying.Text != "")
                {
                    //ClientScript.RegisterStartupScript(GetType(), "sCheckout", "userInput(" + Convert.ToDouble(txtAmountPaying.Text) + ")", true);
                    object[] amounts = VerifyTenderAndChange();
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 5, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 2, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 7, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 1, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 6, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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

        //American Express
        protected void MopAmEx_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopAmEx_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtAmountPaying.Text != "")
                {
                    object[] amounts = { txtAmountPaying.Text, 0 };
                    hdnTender.Value = txtAmountPaying.Text;
                    hdnChange.Value = "0";
                    PopulateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), 3, amounts);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                //Gathers the mop info based on the index
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("mopID")).Text);
                //Retrieves Mop list from Session
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.CallRemoveMopFromList(mopRemovingID, objPageDetails);

                ////Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnRemoveGovTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveGovTax";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                TaxManager TM = new TaxManager();
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                bool chargeGST;
                if (!BtnRemoveGov.Text.Contains("Remove"))
                {
                    chargeGST = true;
                }
                else
                {
                    chargeGST = false;
                }
                
                foreach (InvoiceItems invoiceItem in invoice.invoiceItems)
                {
                    foreach (InvoiceItemTax invoiceItemTax in invoiceItem.invoiceItemTaxes)
                    {
                        if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("GST", objPageDetails) || invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("HST", objPageDetails))
                        {
                            invoiceItemTax.bitIsTaxCharged = chargeGST;
                        }
                    }
                    IIM.CallUpdateItemTaxesFromCurrentSalesTableActualQuery(invoiceItem, invoice.intTransactionTypeID, objPageDetails);
                }

                IM.CallUpdateCurrentInvoice(invoice, objPageDetails);                
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnRemoveProvTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveProvTax";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                TaxManager TM = new TaxManager();                
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                bool chargePST;
                if (!BtnRemoveProv.Text.Contains("Remove"))
                {
                    chargePST = true;
                }
                else
                {
                    chargePST = false;
                }
                foreach (InvoiceItems invoiceItem in invoice.invoiceItems)
                {
                    foreach (InvoiceItemTax invoiceItemTax in invoiceItem.invoiceItemTaxes)
                    {
                        if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("PST", objPageDetails) || invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("RST", objPageDetails) || invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("QST", objPageDetails))
                        {
                            invoiceItemTax.bitIsTaxCharged = chargePST;
                        }
                    }
                    IIM.CallUpdateItemTaxesFromCurrentSalesTableActualQuery(invoiceItem, invoice.intTransactionTypeID, objPageDetails);
                }

                IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnRemoveLiqTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveLiqTax";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                TaxManager TM = new TaxManager();                
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                bool chargeLCT;
                if (!BtnRemoveLiq.Text.Contains("Remove"))
                {
                    chargeLCT = true;
                }
                else
                {
                    chargeLCT = false;
                }
                foreach (InvoiceItems invoiceItem in invoice.invoiceItems)
                {
                    foreach (InvoiceItemTax invoiceItemTax in invoiceItem.invoiceItemTaxes)
                    {
                        if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("LCT", objPageDetails))
                        {
                            invoiceItemTax.bitIsTaxCharged = chargeLCT;
                        }
                    }
                    IIM.CallUpdateItemTaxesFromCurrentSalesTableActualQuery(invoiceItem, invoice.intTransactionTypeID, objPageDetails);
                }

                IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                IIM.LoopThroughTheItemsToReturnToInventory(invoice.intInvoiceID, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                IIM.CallRemoveInitialTotalsForTable(invoice.intInvoiceID, objPageDetails);
                //Changes to the Home page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnExitSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExitSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //TODO: btnExitSale_Click is good as it doesn't read the new values. It removes the entry 
                //TaxManager TM = new TaxManager();
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                //object[] taxText = { "Remove GST", "Remove PST" };
                //object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails);
                invoice.intTransactionTypeID = Convert.ToInt32(IM.CallReturnTransactionID("On Hold", objPageDetails));           
                IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        //protected void btnLayaway_Click(object sender, EventArgs e)
        //{
        //    //Collects current method for error tracking
        //    string method = "btnLayaway_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        //TaxManager TM = new TaxManager();
        //        //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
        //        //object[] taxText = { "Remove GST", "Remove PST" };
        //        //object[] results = TM.ReturnChargedTaxForSale(I, taxText, objPageDetails);
        //        //I.transactionType = 6;
        //        //IM.UpdateCurrentInvoice(I, objPageDetails);
        //        //Response.Redirect("HomePage.aspx", false);
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        protected void BtnReturnToCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnToCart_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                //object[] taxText = { "Remove GST", "Remove PST" };
                //if ((btnRemoveGov.Text).Split(' ')[0] != "Remove")
                //{
                //    taxText[0] = "Do Nothing";
                //}
                //if ((btnRemoveProv.Text).Split(' ')[0] != "Remove")
                //{
                //    taxText[1] = "Do Nothing";
                //}
                //TaxManager TM = new TaxManager();
                //object[] results = TM.ReturnChargedTaxForSale(invoice, taxText, objPageDetails); //UPDATING THE CURRENT SALES TABLE
                //Sets session to true
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", Request.QueryString["customer"].ToString());
                nameValues.Set("invoice", Request.QueryString["invoice"].ToString());
                //Changes to Sales Cart page
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
                if (VerifyToProceedForFinalize())
                {
                    CU = (CurrentUser)Session["currentUser"];
                    Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                    //Employee
                    EmployeeManager EM = new EmployeeManager();
                    if (EM.ReturnCanEmployeeMakeSale(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails))
                {
                    //Checks the amount paid and the bypass check box
                    if (!txtAmountPaying.Text.Equals("0.00"))
                    {
                        //Displays message
                        MessageBoxCustom.ShowMessage("Remaining Balance Does NOT Equal $0.00.", this);
                    }
                    else
                    {
                        if (IM.CallVerifyMOPHasBeenAdded(invoice.intInvoiceID, objPageDetails))
                        {
                            //Stores all the Sales data to the database
                            invoice = IM.CallReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];
                            invoice.intEmployeeID = EM.CallReturnEmployeeFromPassword(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails);
                            //invoice.Employee = EM.CallReturnEmployeeFromPassword(Convert.ToInt32(txtEmployeePasscode.Text), objPageDetails)[0];
                            invoice.varAdditionalInformation = txtComments.Text;
                            invoice.intTransactionTypeID = IM.CallReturnTransactionID("Sale", objPageDetails);
                            IM.FinalizeInvoice(invoice, "tbl_invoiceItem", objPageDetails);
                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                            Response.Redirect("PrintableInvoice.aspx?" + nameValues, false);
                        }
                        else
                        {
                            MessageBoxCustom.ShowMessage("At least one method of payment "
                                + "is required even for a $0.00 sale.", this);
                        }
                    }
                }
                    else
                    {
                        MessageBoxCustom.ShowMessage("Invalid employee passcode entered. "
                        + "Please try again.", this);
                    }
                }
                else
                {
                    MessageBoxCustom.ShowMessage("If you wish to proceed you will need to correct the taxes.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        private void PopulateGridviewMOP(double amountPaid, int methodOfPayment, object[] amounts)
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
                    intPaymentID = methodOfPayment,
                    fltAmountPaid = amountPaid,
                    fltTenderedAmount = Convert.ToDouble(amounts[0]),
                    fltCustomerChange = Convert.ToDouble(amounts[1])
                };
                IMM.CallAddNewMopToList(invoicePayment, objPageDetails);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        private void ButtonDisable(double rb)
        {
            string method = "buttonDisable";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (rb >= -.001 && rb <= 0.001)
                {
                    if (IM.CallVerifyMOPHasBeenAdded(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails))
                    {
                        MopCash.Enabled = false;
                    }
                    else
                    {
                        MessageBoxCustom.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 sale.", this);
                    }
                    MopDebit.Enabled = false;
                    MopGiftCard.Enabled = false;
                    MopMasterCard.Enabled = false;
                    MopVisa.Enabled = false;
                    MopAmEx.Enabled = false;
                }
                else
                {
                    MopCash.Enabled = true;
                    MopDebit.Enabled = true;
                    MopGiftCard.Enabled = true;
                    MopMasterCard.Enabled = true;
                    MopVisa.Enabled = true;
                    MopAmEx.Enabled = true;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                TaxManager TM = new TaxManager();
                //Loops through each mop
                double dblAmountPaid = 0;
                foreach (var payment in invoice.invoiceMops)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += payment.fltAmountPaid;
                }
                gvCurrentMOPs.DataSource = invoice.invoiceMops;
                gvCurrentMOPs.DataBind();

                string governmentName = "GST";
                string provincialName = "PST";
                string liquorName = "LCT";
                double governmentTax = 0;
                double provincialTax = 0;
                double liquorTax = 0;

                foreach (var invoiceItem in invoice.invoiceItems)
                {
                    foreach (var invoiceItemTax in invoiceItem.invoiceItemTaxes)
                    {
                        if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("GST", objPageDetails) || invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("HST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                governmentTax += invoiceItemTax.fltTaxAmount;
                                lblGovernment.Visible = true;
                                lblGovernment.Text = invoiceItemTax.varTaxName;
                                lblGovernmentAmount.Visible = true;
                                BtnRemoveGov.Visible = true;
                                BtnRemoveGov.Text = "Remove " + invoiceItemTax.varTaxName.ToString();
                            }
                            governmentName = invoiceItemTax.varTaxName.ToString();
                        }
                        else if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("PST", objPageDetails) || invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("RST", objPageDetails) || invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("QST", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                provincialTax += invoiceItemTax.fltTaxAmount;
                                lblProvincial.Visible = true;
                                lblProvincial.Text = invoiceItemTax.varTaxName;
                                lblProvincialAmount.Visible = true;
                                BtnRemoveProv.Visible = true;
                                BtnRemoveProv.Text = "Remove " + invoiceItemTax.varTaxName.ToString();
                            }
                            provincialName = invoiceItemTax.varTaxName.ToString();
                        }
                        else if (invoiceItemTax.intTaxTypeID == TM.GatherTaxIDFromString("LCT", objPageDetails))
                        {
                            if (invoiceItemTax.bitIsTaxCharged)
                            {
                                liquorTax += invoiceItemTax.fltTaxAmount;
                                lblLiquorTax.Visible = true;
                                lblLiquorTax.Text = invoiceItemTax.varTaxName;
                                lblLiquorTaxAmount.Visible = true;
                                BtnRemoveLiq.Visible = true;
                                BtnRemoveLiq.Text = "Remove " + invoiceItemTax.varTaxName.ToString();
                            }
                            liquorName = invoiceItemTax.varTaxName.ToString();
                        }
                    }
                }

                if (governmentTax == 0)
                {
                    BtnRemoveGov.Text = "Add " + governmentName.ToString();
                }
                if (provincialTax == 0)
                {
                    BtnRemoveProv.Text = "Add " + provincialName.ToString();
                }
                if (liquorTax == 0)
                {
                    BtnRemoveLiq.Text = "Add " + liquorName.ToString();
                }

                lblGovernmentAmount.Text = (governmentTax + invoice.fltShippingTaxAmount).ToString("C");
                lblProvincialAmount.Text = provincialTax.ToString("C");
                lblLiquorTaxAmount.Text = liquorTax.ToString("C");

                double tx = governmentTax + provincialTax + liquorTax + invoice.fltShippingTaxAmount;

                //***Assign each item to its Label.
                lblTotalInCartAmount.Text = (invoice.fltSubTotal + invoice.fltTotalDiscount - invoice.fltTotalTradeIn).ToString("C");
                lblTotalInDiscountsAmount.Text = invoice.fltTotalDiscount.ToString("C");
                lblTradeInsAmount.Text = invoice.fltTotalTradeIn.ToString("C");
                lblSubTotalAmount.Text = (invoice.fltSubTotal + invoice.fltShippingCharges).ToString("C");
                lblShippingAmount.Text = invoice.fltShippingCharges.ToString("C");
                //Displays the remaining balance
                lblBalanceAmount.Text = (invoice.fltBalanceDue + invoice.fltShippingCharges + tx).ToString("C");
                lblRemainingBalanceDueDisplay.Text = ((invoice.fltBalanceDue + invoice.fltShippingCharges + tx) - dblAmountPaid).ToString("C");
                txtAmountPaying.Text = ((invoice.fltBalanceDue + invoice.fltShippingCharges + tx) - dblAmountPaid).ToString("#0.00");
                ButtonDisable(((invoice.fltBalanceDue + invoice.fltShippingCharges + tx) - dblAmountPaid));
                Session["currentInvoice"] = invoice;
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive the message please contact "
                    + "your system administrator.", this);
            }
        }
        private object[] VerifyTenderAndChange()
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

        protected int TaxChecker(Invoice invoice, object[] objPageDetails)
        {
            int intTaxIsValid = 0;
            string strOU = "Over: $";
            double approxGSTAmount = Math.Round(invoice.fltSubTotal * 0.05, 2);
            double actualGSTAmount = Math.Round(invoice.fltGovernmentTaxAmount, 2);
            //chkProceed.Checked = true;
            if (approxGSTAmount != actualGSTAmount)
            {
                intTaxIsValid = 1;
                double dblDiscrepency = Math.Round((actualGSTAmount - approxGSTAmount), 2);
                strOU = strOU.ToString() + dblDiscrepency.ToString();
                if (approxGSTAmount > actualGSTAmount)
                {
                    strOU = "Under: $" + dblDiscrepency.ToString();
                }
                lblTaxDiscrepency.Text = "GST amount shoud be: " + approxGSTAmount.ToString() + ". " + strOU.ToString() + ".";

                lblTaxDiscrepency.Visible = true;
                chkProceed.Visible = true;
                ER.CallLogTaxError(invoice, objPageDetails);
            }
            return intTaxIsValid;
        }
        protected bool VerifyToProceedForFinalize()
        {
            bool bolProceed = true;
            if(chkProceed.Visible == true)
            {
                if (!chkProceed.Checked) { bolProceed = false; }
            }
            return bolProceed;
        }
    }
}