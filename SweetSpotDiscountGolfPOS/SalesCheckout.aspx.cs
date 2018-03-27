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

        //SweetShopManager ssm = new SweetShopManager();
        //List<Mops> mopList = new List<Mops>();
        //List<Cart> itemsInCart = new List<Cart>();
        //ItemDataUtilities idu = new ItemDataUtilities();
        //LocationManager lm = new LocationManager();
        //CheckoutManager ckm;
        //EmployeeManager em = new EmployeeManager();
        //public double dblRemaining;
        //public double subtotal;
        //public double gst;
        //public double pst;
        //public double balancedue;
        //public double dblAmountPaid;
        //public double tradeInCost;
        //public double taxAmount;
        ////Remove Prov or Gov Tax
        //bool chargeGST;
        //bool chargePST;
        //double amountPaid;
        //double dblShippingAmount;
        //int tranType;
        //int gridID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCheckout.aspx";
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
                        //List<Cart> cart = new List<Cart>();
                        //SalesCalculationManager SCM = new SalesCalculationManager();
                        //Retrieves items in the cart from Session
                        //cart = (List<Cart>)Session["ItemsInCart"];
                        //Retrieves date from session
                        //DateTime recDate = Convert.ToDateTime(Session["strDate"]);
                        //Retrieves shipping
                        //bool bolShipping = Convert.ToBoolean(Session["shipping"]);
                        //Checks if shipping was charged 
                        Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        //int prov = I[0].location.provID;
                        //if (I[0].shippingAmount > 0)
                        //{
                        //Retrieves customer number from Session
                        //int custNum = Convert.ToInt32(Session["key"].ToString());
                        //Uses customer number to fill a customer class
                        //Customer c = ssm.GetCustomerbyCustomerNumber(Convert.ToInt32(Request.QueryString["cust"].ToString()));
                        //Based on customer province and date get taxes
                        //prov = I[0].customer.province;
                        //t = ssm.getTaxes(I[0].customer.province, I[0].invoiceDate);
                        //lblShipping.Visible = true;
                        //lblShippingAmount.Visible = true;
                        //Retrieve shipping amount from Session
                        //dblShippingAmount = Convert.ToDouble(Session["ShippingAmount"].ToString());
                        //}
                        //else
                        //{
                        //    //Query returns taxes based on current location
                        //    t = ssm.getTaxes(lm.getProvIDFromLocationID(CU.locationID), I[0].invoiceDate);
                        //    //Sets shipping amouunt to 0
                        //    //lblShipping.Visible = false;
                        //    //lblShippingAmount.Visible = false;
                        //    //dblShippingAmount = 0;
                        //}
                        //t = SCM.getTaxes(prov, I[0].invoiceDate);
                        //Retrieves location id from Session
                        //int location = CU.locationID;
                        //Creates checkout manager based on current items in cart
                        //ckm = new CheckoutManager(cm.returnTotalAmount(cart, location), cm.returnDiscount(cart), cm.returnTradeInAmount(cart, location), dblShippingAmount, true, true, 0, 0, 0);
                        //Loops through each tax
                        //foreach (var T in t)
                        //{
                        //switch (T.taxName)
                        //{
                        //If tax is GST calculate and make visible
                        //case "GST":
                        //lblGovernment.Visible = true;
                        //I[0].governmentTax = SCM.CallReturnTaxAmount(T.taxRate, I[0].subTotal + I[0].shippingAmount);
                        //lblGovernmentAmount.Text = "$ " + I[0].governmentTax.ToString("#0.00");
                        //lblGovernmentAmount.Visible = true;
                        //btnRemoveGov.Visible = true;
                        //break;
                        //If tax is PST calculate and make visible
                        //case "PST":
                        //lblProvincial.Visible = true;
                        //I[0].provincialTax = SCM.CallReturnTaxAmount(T.taxRate, I[0].subTotal);
                        //lblProvincialAmount.Text = "$ " + I[0].provincialTax.ToString("#0.00");
                        //lblProvincialAmount.Visible = true;
                        //btnRemoveProv.Visible = true;
                        //break;
                        //If tax is HST calculate and make visible
                        //case "HST":
                        //lblProvincial.Visible = false;
                        //lblGovernment.Text = "HST";
                        //I[0].governmentTax = SCM.CallReturnTaxAmount(T.taxRate, I[0].subTotal + I[0].shippingAmount);
                        //lblGovernmentAmount.Text = "$ " + I[0].governmentTax.ToString("#0.00");
                        //lblGovernmentAmount.Visible = true;
                        //btnRemoveProv.Visible = false;
                        //btnRemoveGov.Text = "HST";
                        //break;
                        //If tax is RST calculate and make visible
                        //case "RST":
                        //lblProvincial.Visible = true;
                        //lblProvincial.Text = "RST";
                        //I[0].provincialTax = SCM.CallReturnTaxAmount(T.taxRate, I[0].subTotal);
                        //lblProvincialAmount.Text = "$ " + I[0].provincialTax.ToString("#0.00");
                        //lblProvincialAmount.Visible = true;
                        //btnRemoveProv.Visible = true;
                        //btnRemoveProv.Text = "RST";
                        //break;
                        //If tax is QST calculate and make visible
                        //case "QST":
                        //lblProvincial.Visible = true;
                        //lblProvincial.Text = "QST";
                        //I[0].provincialTax = SCM.CallReturnTaxAmount(T.taxRate, I[0].subTotal);
                        //lblProvincialAmount.Text = "$ " + I[0].provincialTax.ToString("#0.00");
                        //lblProvincialAmount.Visible = true;
                        //btnRemoveProv.Visible = true;
                        //btnRemoveProv.Text = "QST";
                        //break;
                        //}
                        //}
                        //I[0].balanceDue += I[0].subTotal + I[0].governmentTax + I[0].provincialTax;
                        //IM.UpdateCurrentInvoice(I[0]);
                        //Add taxes to balance due and remaining balance
                        //ckm.dblBalanceDue += ckm.dblGst + ckm.dblPst;

                        object[] taxText = { "Add GST", "Add PST" };
                        object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                        I = (Invoice)results[0];
                        object[] taxStatus = (object[])results[1];
                        if (Convert.ToBoolean(taxStatus[0]))
                        {
                            lblGovernment.Visible = true;
                            lblGovernmentAmount.Text = "$ " + I.governmentTax.ToString("#0.00");
                            lblGovernmentAmount.Visible = true;
                            btnRemoveGov.Text = taxStatus[1].ToString();
                            btnRemoveGov.Visible = true;
                        }
                        if (Convert.ToBoolean(taxStatus[2]))
                        {
                            lblProvincial.Visible = true;
                            lblProvincialAmount.Text = "$ " + I.provincialTax.ToString("#0.00");
                            lblProvincialAmount.Visible = true;
                            btnRemoveProv.Text = taxStatus[3].ToString();
                            btnRemoveProv.Visible = true;
                        }

                        UpdatePageTotals();

                        double dblRemainingBalance = I.balanceDue;
                        //double dblAmountPaid = 0;
                        //Checks if there are any stored methods of payment
                        //if (Session["MethodsofPayment"] != null)
                        //{
                        //Retrieve Mops from session
                        //InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                        //List<InvoiceMOPs>  mopList = IMM.ReturnInvoiceMOPsCurrentSale(Request.QueryString["inv"]);
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
                        //    //ckm.dblAmountPaid = dblAmountPaid;
                        //    dblRemainingBalance -= dblAmountPaid;
                        ////}

                        //***Assign each item to its Label.
                        lblTotalInCartAmount.Text = "$ " + (I.subTotal + I.discountAmount - I.tradeinAmount).ToString("#0.00");
                        lblTotalInDiscountsAmount.Text = "$ " + I.discountAmount.ToString("#0.00");
                        lblTradeInsAmount.Text = "$ " + I.tradeinAmount.ToString("#0.00");
                        lblSubTotalAmount.Text = "$ " + (I.subTotal + I.shippingAmount).ToString("#0.00");
                        lblShippingAmount.Text = "$ " + I.shippingAmount.ToString("#0.00");
                        //lblGovernmentAmount.Text = "$ " + I.governmentTax.ToString("#0.00");
                        //lblProvincialAmount.Text = "$ " + I.provincialTax.ToString("#0.00");
                        //lblBalanceAmount.Text = "$ " + I.balanceDue.ToString("#0.00");
                        //lblRemainingBalanceDueDisplay.Text = "$ " + dblRemainingBalance.ToString("#0.00");
                        //Stores totals in the checkout manager
                        //Session["CheckOutTotals"] = ckm;
                        //Updates the amount paying with the remaining balance
                        //txtAmountPaying.Text = dblRemainingBalance.ToString("#0.00");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //American Express
        //protected void mopAmericanExpress_Click(object sender, EventArgs e)
        //{
        //    ckm = (CheckoutManager)Session["CheckOutTotals"];
        //    //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
        //    string boxResult = txtAmountPaying.Text;
        //    if (boxResult != "")
        //    {
        //        amountPaid = Convert.ToDouble(boxResult);
        //        string methodOfPayment = "American Express";
        //        populateGridviewMOP(amountPaid, methodOfPayment);
        //    }

        //}
        //Cash
        protected void mopCash_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopCash_Click";
            try
            {
                //Retrieves checkout totals from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Checks that string is not empty
                if (txtAmountPaying.Text != "")
                {
                    //Converts amount to double
                    //amountPaid = Convert.ToDouble(txtAmountPaying.Text);
                    //Calls client side script to display change due to customer
                    ClientScript.RegisterStartupScript(GetType(), "hwa", "userInput(" + Convert.ToDouble(txtAmountPaying.Text) + ")", true);
                    //Collects mop type
                    //string methodOfPayment = "Cash";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), "Cash");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Account
        //protected void mopOnAccount_Click(object sender, EventArgs e)
        //{
        //    amountPaid = Convert.ToDouble(Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Account", "", -1, -1));
        //    String methodOfPayment = "Account";

        //    populateGridviewMOP(amountPaid, methodOfPayment);
        //}

        //Cheque
        //protected void mopCheque_Click(object sender, EventArgs e)
        //{
        //    ckm = (CheckoutManager)Session["CheckOutTotals"];
        //    //string boxResult = Microsoft.VisualBasic.Interaction.InputBox("Enter Amount Paid", "Cash", ckm.dblRemainingBalance.ToString("#0.00"), -1, -1);
        //    string boxResult = txtAmountPaying.Text;
        //    if (boxResult != "")
        //    {
        //        amountPaid = Convert.ToDouble(boxResult);
        //        string methodOfPayment = "Cheque";
        //        populateGridviewMOP(amountPaid, methodOfPayment);
        //    }
        //}
        //MasterCard
        protected void mopMasterCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopMasterCard_Click";
            try
            {
                //Retrieves checkout totals from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Checks that string is not empty
                if (txtAmountPaying.Text != "")
                {
                    //Converts amount to double
                    //amountPaid = Convert.ToDouble(txtAmountPaying.Text);
                    //Collects mop type
                    //string methodOfPayment = "MasterCard";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), "MasterCard");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieves checkout totals from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Checks that string is not empty
                if (txtAmountPaying.Text != "")
                {
                    //Converts amount to double
                    //amountPaid = Convert.ToDouble(txtAmountPaying.Text);
                    //Collects mop type
                    //string methodOfPayment = "Debit";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), "Debit");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieves checkout totals from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Checks that string is not empty
                if (txtAmountPaying.Text != "")
                {
                    //Converts amount to double
                    //amountPaid = Convert.ToDouble(txtAmountPaying.Text);
                    //Collects mop type
                    //string methodOfPayment = "Visa";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), "Visa");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieves checkout totals from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Checks that string is not empty
                if (txtAmountPaying.Text != "")
                {
                    //Converts amount to double
                    //amountPaid = Convert.ToDouble(txtAmountPaying.Text);
                    //Collects mop type
                    //string methodOfPayment = "Gift Card";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(Convert.ToDouble(txtAmountPaying.Text), "Gift Card");
                }   
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieves index of selected row
                //int index = e.RowIndex;
                //Retrieves the checkout manager from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Gathers the mop info based on the index
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text);
                //double paidAmount = double.Parse(gvCurrentMOPs.Rows[index].Cells[2].Text, NumberStyles.Currency);
                //Retrieves Mop list from Session
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.RemoveMopFromList(Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text), Request.QueryString["inv"].ToString());
                //Loops through each mop in list
                //double dblAmountPaid = 0;
                //foreach (var mop in mopList)
                //{
                //    //Checks if the mop id do not match
                //    //if (mop.tableID != mopRemovingID)
                //    //{
                //        //Not selected mop add back into the mop list
                //    //    mopList.Add(mop);
                //        //Calculate amount paid
                //        dblAmountPaid += mop.amountPaid;
                //    //}
                //    //else
                //    //{
                //        //Add removed mops paid amount back into the remaining balance
                //    //    ckm.dblRemainingBalance += paidAmount;
                //    //}
                //    //updtae the new amount paid total
                //    //ckm.dblAmountPaid = dblAmountPaid;
                //}
                ////Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                ////Store updated mops in Session
                ////Session["MethodsofPayment"] = mopList;
                //List<Invoice> I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString());
                //double dblRemainingBalance = I[0].balanceDue - dblAmountPaid;
                ////Bind the session to the grid view
                //gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                ////Display remaining balance
                //lblRemainingBalanceDueDisplay.Text = "$ " + dblRemainingBalance.ToString("#0.00");
                //txtAmountPaying.Text = dblRemainingBalance.ToString("#0.00");
                //buttonDisable(dblRemainingBalance);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Update from here and below
        protected void btnRemoveGovTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveGovTax";
            try
            {
                TaxManager TM = new TaxManager();
                InvoiceManager IM = new InvoiceManager();
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                object[] taxText = { btnRemoveGov.Text, "Do Nothing" };
                object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                I = (Invoice)results[0];
                object[] taxStatus = (object[])results[1];
                if (Convert.ToBoolean(taxStatus[0]))
                {
                    lblGovernment.Visible = true;
                    lblGovernmentAmount.Text = "$ " + I.governmentTax.ToString("#0.00");
                    lblGovernmentAmount.Visible = true;
                    btnRemoveGov.Text = taxStatus[1].ToString();
                    btnRemoveGov.Visible = true;
                }
                UpdatePageTotals();

                //lblBalanceAmount.Text = I.balanceDue.ToString();
                //InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                //List<InvoiceMOPs> mopList = IMM.ReturnInvoiceMOPsCurrentSale(Request.QueryString["inv"]);
                ////Loops through each mop
                //double dblAmountPaid = 0;
                //foreach (var mop in mopList)
                //{
                //    //Adds the total amount paid fropm each mop type
                //    dblAmountPaid += mop.amountPaid;
                //}
                //gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                ////Displays the remaining balance
                //btnRemoveGov.Text = results[1].ToString();
                //lblRemainingBalanceDueDisplay.Text = "$ " + (I.balanceDue - dblAmountPaid).ToString("#0.00");
                //txtAmountPaying.Text = (I.balanceDue - dblAmountPaid).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
            try
            {
                TaxManager TM = new TaxManager();
                InvoiceManager IM = new InvoiceManager();
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                object[] taxText = { "Do Nothing", btnRemoveProv.Text };
                object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                I = (Invoice)results[0];
                object[] taxStatus = (object[])results[1];
                if (Convert.ToBoolean(taxStatus[2]))
                {
                    lblProvincial.Visible = true;
                    lblProvincialAmount.Text = "$ " + I.provincialTax.ToString("#0.00");
                    lblProvincialAmount.Visible = true;
                    btnRemoveProv.Text = taxStatus[3].ToString();
                    btnRemoveProv.Visible = true;
                }
                UpdatePageTotals();

                //lblBalanceAmount.Text = I.balanceDue.ToString();
                //InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                //List<InvoiceMOPs> mopList = IMM.ReturnInvoiceMOPsCurrentSale(Request.QueryString["inv"]);
                ////Loops through each mop
                //double dblAmountPaid = 0;
                //foreach (var mop in mopList)
                //{
                //    //Adds the total amount paid fropm each mop type
                //    dblAmountPaid += mop.amountPaid;
                //}
                //gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                ////Displays the remaining balance
                //btnRemoveProv.Text = results[1].ToString();
                //lblRemainingBalanceDueDisplay.Text = "$ " + (I.balanceDue - dblAmountPaid).ToString("#0.00");
                //txtAmountPaying.Text = (I.balanceDue - dblAmountPaid).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
            try
            {
                ////Checks session to see if it's null
                //if (Session["ItemsInCart"] != null)
                //{
                //    //Retrieves items in the cart from Session
                //    itemsInCart = (List<Cart>)Session["ItemsInCart"];
                //}
                ////Loops through each item in the cart
                //foreach (var cart in itemsInCart)
                //{
                //    //Queries the database to return the remainig quantity of the sku
                //    int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                //    //Updates the quantity in stock adding the quantity in the cart
                //    idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
                //}
                ////Nullifies each of the sessions 
                //Session["returnedCart"] = null;
                //Session["key"] = null;
                //Session["shipping"] = null;
                //Session["ShippingAmount"] = null;
                //Session["ItemsInCart"] = null;
                //Session["CheckOutTotals"] = null;
                //Session["MethodsofPayment"] = null;
                //Session["Invoice"] = null;
                //Session["TranType"] = null;
                //Session["searchReturnInvoices"] = null;
                //Session["strDate"] = null;
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                IIM.LoopThroughTheItemsToReturnToInventory(Request.QueryString["inv"].ToString());
                IIM.RemoveInitialTotalsForTable(Request.QueryString["inv"].ToString());
                //Changes to the Home page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
            try
            {
                TaxManager TM = new TaxManager();
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                object[] taxText = { "Remove GST", "Remove PST" };
                object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                I.transactionType = 1;
                IM.UpdateCurrentInvoice(I);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
            try
            {
                TaxManager TM = new TaxManager();
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                object[] taxText = { "Remove GST", "Remove PST" };
                object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                I.transactionType = 6;
                IM.UpdateCurrentInvoice(I);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                //Sets session to true
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                nameValues.Set("inv", Request.QueryString["inv"].ToString());
                //Changes to Sales Cart page
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Employee
                //******Need to get the employee somehow
                EmployeeManager EM = new EmployeeManager();

                Employee emp = EM.getEmployeeByID(EM.returnEmployeeIDFromPassword(Convert.ToInt32(txtEmployeePasscode.Text)));
                bool bolValidPassword = EM.returnCanEmployeeMakeSale(emp.employeeID);
                if (bolValidPassword)
                {
                    //Checks the amount paid and the bypass check box
                    if (!txtAmountPaying.Text.Equals("0.00"))
                    {
                        //Displays message
                        MessageBox.ShowMessage("Remaining Balance Does NOT Equal $0.00.", this);
                    }
                    else
                    {
                        //Gathering needed information for the invoice
                        //Retrieves transaction type from Session
                        //tranType = Convert.ToInt32(Session["TranType"]);
                        //Determines the Cart to be used based on transaction
                        //if (tranType == 1) { cart = (List<Cart>)Session["ItemsInCart"]; }
                        //else if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }

                        //Customer
                        //int custNum = Convert.ToInt32(Session["key"]);
                        //Customer c = ssm.GetCustomerbyCustomerNumber(Convert.ToInt32(Request.QueryString["cust"].ToString()));
                        
                        //CheckoutTotals
                        //ckm = (CheckoutManager)Session["CheckOutTotals"];
                        //MOP
                        //mopList = (List<Mops>)Session["MethodsofPayment"];

                        //Stores all the Sales data to the database
                        IM.FinalizeInvoice(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0], txtComments.Text);

                        //Nullifies all related sessions
                        //Session["useInvoice"] = false;
                        //Session["shipping"] = null;
                        //Session["ShippingAmount"] = null;
                        //Session["searchReturnInvoices"] = null;
                        //Session["actualInvoiceInfo"] = ssm.getSingleInvoice(Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[1]), Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[2]));
                        //Changes page to printable invoice
                        string printableInvoiceNum = Request.QueryString["inv"].ToString().Split('-')[1] + "-" + Request.QueryString["inv"].ToString().Split('-')[2];
                        Response.Redirect("PrintableInvoice.aspx?inv=" + printableInvoiceNum, false);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Populating gridview with MOPs
        private void populateGridviewMOP(double amountPaid, string methodOfPayment)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            try
            {
                //gridID = 0;
                //Checks if there are any current mops used
                //if (Session["MethodsofPayment"] != null)
                //{
                //Retrieves current mops from Session
                //mopList = (List<Mops>)Session["MethodsofPayment"];
                //Loops through each mop
                //foreach (var mop in mopList)
                //{
                //Sets grid id to be the largest current id in table
                //    if (mop.tableID > gridID)
                //        gridID = mop.tableID;
                //}
                //}
                //Sets a temp checkout with the new Mop
                //Mops tempCK = new Mops(methodOfPayment, amountPaid, gridID + 1);
                //Retrieves totals for check out from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Adds new mop to the current mop list
                //mopList.Add(tempCK);
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.AddNewMopToList(Request.QueryString["inv"].ToString(), amountPaid, methodOfPayment);
                //Loops through each mop
                //double dblAmountPaid = 0;
                //foreach (var mop in mopList)
                //{
                //    //Adds the total amount paid fropm each mop type
                //    dblAmountPaid += mop.amountPaid;
                //}
                ////Updates the amount paid and remaining balance in the checkout manager
                ////ckm.dblAmountPaid = dblAmountPaid;
                ////Binds the moplist to the gridview
                //gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                ////Center the mop grid view
                ////foreach (GridViewRow row in gvCurrentMOPs.Rows)
                ////{
                ////    foreach (TableCell cell in row.Cells)
                ////    {
                ////        cell.Attributes.CssStyle["text-align"] = "center";
                ////    }
                ////}
                ////Store moplist into session
                ////Session["MethodsofPayment"] = mopList;
                //List<Invoice> I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString());
                //double dblRemainingBalance = I[0].balanceDue - dblAmountPaid;
                ////Sets the remaining balance due
                //lblRemainingBalanceDueDisplay.Text = "$ " + dblRemainingBalance.ToString("#0.00");
                //txtAmountPaying.Text = dblRemainingBalance.ToString("#0.00");
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void buttonDisable(double rb)
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive the message please contact "
                    + "your system administrator.", this);
            }
        }
        private void UpdatePageTotals()
        {
            //lblBalanceAmount.Text = I.balanceDue.ToString();
            //InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            //List<InvoiceMOPs> mopList = IMM.ReturnInvoiceMOPsCurrentSale(Request.QueryString["inv"]);
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
            lblBalanceAmount.Text = "$ " + (I.balanceDue + I.shippingAmount).ToString("#0.00");
            lblRemainingBalanceDueDisplay.Text = "$ " + ((I.balanceDue + I.shippingAmount) - dblAmountPaid).ToString("#0.00");
            txtAmountPaying.Text = ((I.balanceDue + I.shippingAmount) - dblAmountPaid).ToString("#0.00");
            buttonDisable(((I.balanceDue + I.shippingAmount) - dblAmountPaid));
        }
    }
}