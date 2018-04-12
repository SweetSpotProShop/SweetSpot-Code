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
        CurrentUser CU = new CurrentUser();
        InvoiceManager IM = new InvoiceManager();

        //SweetShopManager ssm = new SweetShopManager();
        //List<Mops> mopList = new List<Mops>();
        //List<Cart> itemsInCart = new List<Cart>();
        //ItemDataUtilities idu = new ItemDataUtilities();
        //LocationManager lm = new LocationManager();
        //CheckoutManager ckm;
        //public double dblRemaining;
        //public double subtotal;
        //public double gst;
        //public double pst;
        //public double balancedue;
        //public double dblAmountPaid;
        //public double tradeInCost;
        //public double taxAmount;
        //double amountPaid;
        //double dblShippingAmount;
        //int tranType;
        //int gridID;

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
                        //List<Cart> cart = new List<Cart>();
                        //SalesCalculationManager CM = new SalesCalculationManager();
                        //Retrieves cart from session
                        Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        //Retrieve taxes based on current location
                        string gTax = "Do Nothing";
                        string pTax = "Do Nothing";
                        if (I.governmentTax > 0) { gTax = "Add GST"; }
                        if (I.provincialTax > 0) { pTax = "Add PST"; }
                        object[] taxText = { gTax, pTax };
                        object[] results = TM.ReturnChargedTaxForSale(I, taxText);
                        I = (Invoice)results[0];
                        object[] taxStatus = (object[])results[1];
                        if (Convert.ToBoolean(taxStatus[0]))
                        {
                            lblGovernment.Visible = true;
                            lblGovernmentAmount.Text = "$ " + I.governmentTax.ToString("#0.00");
                            lblGovernmentAmount.Visible = true;
                            //btnRemoveGov.Text = taxStatus[1].ToString();
                            //btnRemoveGov.Visible = true;
                        }
                        if (Convert.ToBoolean(taxStatus[2]))
                        {
                            lblProvincial.Visible = true;
                            lblProvincialAmount.Text = "$ " + I.provincialTax.ToString("#0.00");
                            lblProvincialAmount.Visible = true;
                            //btnRemoveProv.Text = taxStatus[3].ToString();
                            //btnRemoveProv.Visible = true;
                        }

                        ////Retrieve the location from Session
                        //int location = CU.locationID;
                        ////Retrieve invoice from session
                        //Invoice rInvoice = (Invoice)Session["searchReturnInvoices"];
                        ////stores all checkout info in class
                        ////ckm = new CheckoutManager(cm.returnRefundTotalAmount(cart), 0, 0, 0, false, false, 0, 0, 0);
                        //foreach (var T in t)
                        //{
                        //    //Cycles through each tax to display the correct ones and the
                        //    //amount of each one
                        //    switch (T.taxName)
                        //    {
                        //        case "GST":
                        //            lblGovernment.Visible = true;
                        //            lblGovernmentAmount.Visible = true;
                        //            //Only use tax amount if it was higher than 0
                        //            if (rInvoice.governmentTax > 0)
                        //            {
                        //                //ckm.dblGst = cm.returnTaxAmount(T.taxRate, ckm.dblSubTotal);
                        //                ckm.blGst = true;
                        //            }
                        //            lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                        //            break;
                        //        case "PST":
                        //            lblProvincial.Visible = true;
                        //            lblProvincialAmount.Visible = true;
                        //            //Only use tax amount if it was higher than 0
                        //            if (rInvoice.provincialTax > 0)
                        //            {
                        //                //ckm.dblPst = cm.returnTaxAmount(T.taxRate, ckm.dblSubTotal);
                        //                ckm.blPst = true;
                        //            }
                        //            lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                        //            break;
                        //        case "HST":
                        //            lblProvincial.Visible = false;
                        //            lblGovernmentAmount.Visible = true;
                        //            lblGovernment.Text = "HST";
                        //            //Only use tax amount if it was higher than 0
                        //            if (rInvoice.governmentTax > 0)
                        //            {
                        //                //ckm.dblGst = cm.returnTaxAmount(T.taxRate, ckm.dblSubTotal);
                        //                ckm.blGst = true;
                        //            }
                        //            lblGovernmentAmount.Text = "$ " + gst.ToString("#0.00");
                        //            break;
                        //        case "RST":
                        //            lblProvincial.Visible = true;
                        //            lblProvincialAmount.Visible = true;
                        //            lblProvincial.Text = "RST";
                        //            //Only use tax amount if it was higher than 0
                        //            if (rInvoice.provincialTax > 0)
                        //            {
                        //                //ckm.dblPst = cm.returnTaxAmount(T.taxRate, ckm.dblSubTotal);
                        //                ckm.blPst = true;
                        //            }
                        //            lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                        //            break;
                        //        case "QST":
                        //            lblProvincial.Visible = true;
                        //            lblProvincialAmount.Visible = true;
                        //            lblProvincial.Text = "QST";
                        //            //Only use tax amount if it was higher than 0
                        //            if (rInvoice.provincialTax > 0)
                        //            {
                        //                //ckm.dblPst = cm.returnTaxAmount(T.taxRate, ckm.dblSubTotal);
                        //                ckm.blPst = true;
                        //            }
                        //            lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                        //            break;
                        //    }
                        //}
                        ////Setting the balance due and remaining balance
                        //ckm.dblBalanceDue += ckm.dblGst + ckm.dblPst;
                        //ckm.dblRemainingBalance += ckm.dblGst + ckm.dblPst;
                        ////Checks the Session for any method of payments
                        //if (Session["MethodsofPayment"] != null)
                        //{
                        //    //Retrieves methods of payment from session
                        //    mopList = (List<Mops>)Session["MethodsofPayment"];
                        //    foreach (var mop in mopList)
                        //    {
                        //        //creates total amount from each mop
                        //        dblAmountPaid += mop.amountPaid;
                        //    }
                        //    //Binds mops to grid view
                        //    gvCurrentMOPs.DataSource = mopList;
                        //    gvCurrentMOPs.DataBind();
                        //    //Sets amount currently paid
                        //    ckm.dblAmountPaid = dblAmountPaid;
                        //    //Recalculate the remaining balance
                        //    ckm.dblRemainingBalance = ckm.dblBalanceDue - ckm.dblAmountPaid;
                        //}
                        UpdatePageTotals();
                        //***Assign each item to its Label.
                        lblRefundSubTotalAmount.Text = "$ " + I.subTotal.ToString("#0.00");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieve checkout info from session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = txtAmountRefunding.Text;
                //Checks if there is an amount entered into payment text box
                if (txtAmountRefunding.Text != "")
                {
                    //Converts amount to Double
                    //amountPaid = Convert.ToDouble(boxResult);
                    //sets mop type string
                    //string methodOfPayment = "Cash";
                    //Sends amount and mop type to procedure to add mop to gridview
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Cash");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieve checkout info from session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = txtAmountRefunding.Text;
                //Checks if there is an amount entered into payment text box
                if (txtAmountRefunding.Text != "")
                {
                    //Converts amount to Double
                    //amountPaid = Convert.ToDouble(txtAmountRefunding.Text);
                    //sets mop type string
                    //string methodOfPayment = "MasterCard";
                    //Sends amount and mop type to procedure to add mop to gridview
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "MasterCard");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieve checkout info from session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = txtAmountRefunding.Text;
                //Checks if there is an amount entered into payment text box
                if (txtAmountRefunding.Text != "")
                {
                    //Converts amount to Double
                    //amountPaid = Convert.ToDouble(txtAmountRefunding.Text);
                    //sets mop type string
                    //string methodOfPayment = "Debit";
                    //Sends amount and mop type to procedure to add mop to gridview
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Debit");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieve checkout info from session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = txtAmountRefunding.Text;
                //Checks if there is an amount entered into payment text box
                if (txtAmountRefunding.Text != "")
                {
                    //Converts amount to Double
                    //amountPaid = Convert.ToDouble(txtAmountRefunding.Text);
                    //sets mop type string
                    //string methodOfPayment = "Visa";
                    //Sends amount and mop type to procedure to add mop to gridview
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Visa");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieve checkout info from session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //string boxResult = txtAmountRefunding.Text;
                //Checks if there is an amount entered into payment text box
                if (txtAmountRefunding.Text != "")
                {
                    //Converts amount to Double
                    //amountPaid = Convert.ToDouble(txtAmountRefunding.Text);
                    //sets mop type string
                    //string methodOfPayment = "Gift Card";
                    //Sends amount and mop type to procedure to add mop to gridview
                    populateGridviewMOP(Convert.ToDouble(txtAmountRefunding.Text), "Gift Card");
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Get index of current selected row
                //int index = e.RowIndex;
                //Retrieve checkout manager from Session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Get mop info based on index
                int mopRemovingID = Convert.ToInt32(((Label)gvCurrentMOPs.Rows[e.RowIndex].Cells[3].FindControl("lblTableID")).Text);
                //double paidAmount = double.Parse(gvCurrentMOPs.Rows[index].Cells[2].Text, NumberStyles.Currency);
                //Set a temp mop list from the session
                //List<Mops> tempMopList = (List<Mops>)Session["MethodsofPayment"];
                //Loop through each mop to find the matching mop id
                //foreach (var mop in tempMopList)
                //{
                //    //When the mop id doesn't match
                //    if (mop.tableID != mopRemovingID)
                //    {
                //        //add it back into the mop list
                //        mopList.Add(mop);
                //        //Total the amount that has been paid
                //        dblAmountPaid += mop.amountPaid;
                //    }
                //    else
                //    {
                //        //Mop id match so add the amount back of the selected mop
                //        ckm.dblRemainingBalance += paidAmount;
                //    }
                //    //Set amount currently paid
                //    ckm.dblAmountPaid = dblAmountPaid;
                //}
                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.RemoveMopFromList(mopRemovingID, Request.QueryString["inv"].ToString());
                //Clear the row index
                gvCurrentMOPs.EditIndex = -1;
                //Set mop list to Session
                //Session["MethodsofPayment"] = mopList;
                //Bind the gridview to the mop list
                //gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                //Display new amount owing
                //lblRemainingRefundDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                //txtAmountRefunding.Text = ckm.dblRemainingBalance.ToString("#0.00");
                //buttonDisable(ckm.dblRemainingBalance);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                //Retrieves transaction type from Session
                //int tranType = Convert.ToInt32(Session["TranType"]);
                //if (tranType == 2)
                //{
                //    //Checks if there are items in the return cart
                //    if (Session["returnedCart"] != null)
                //    {
                //        //Retrieves items in cart from Session
                //        itemsInCart = (List<Cart>)Session["returnedCart"];
                //    }
                //    //Loops through each item in the cart
                //    foreach (var cart in itemsInCart)
                //    {
                //        //Query returns the in stock quantity of the item
                //        int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                //        //removes the aunatity of that item from the stock as it has now
                //        //no longer been returned
                //        idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY - cart.quantity));
                //    }
                //}
                ////Nullify all related Sessions
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
                //Change page to the Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                    //Gathering needed information for the invoice
                    //Retrieve transaction type
                    //tranType = Convert.ToInt32(Session["TranType"]);
                    ////Cart
                    //List<Cart> cart = new List<Cart>();
                    //if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }

                    ////Customer
                    //int custNum = Convert.ToInt32(Session["key"]);
                    //Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    ////Employee
                    //EmployeeManager em = new EmployeeManager();
                    //Employee emp = em.getEmployeeByID(CU.empID);
                    ////CheckoutTotals
                    //ckm = (CheckoutManager)Session["CheckOutTotals"];
                    ////MOP
                    //mopList = (List<Mops>)Session["MethodsofPayment"];

                    ////CheckoutManager ckm, List<Cart> cart, List<Checkout> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments)
                    //idu.mainInvoice(ckm, cart, mopList, c, emp, tranType, (Session["Invoice"]).ToString(), txtComments.Text, CU);

                    ////Nullifies all retlated Sessions
                    //Session["useInvoice"] = false;
                    //Session["shipping"] = null;
                    //Session["ShippingAmount"] = null;
                    //Session["searchReturnInvoices"] = null;
                    //Session["actualInvoiceInfo"] = ssm.getSingleInvoice(Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[1]), Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[2]));

                    IM.FinalizeInvoice(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0], txtComments.Text, "tbl_invoiceItemReturns");

                    string printableInvoiceNum = Request.QueryString["inv"].ToString().Split('-')[1] + "-" + Request.QueryString["inv"].ToString().Split('-')[2];
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("inv", printableInvoiceNum);
                    Response.Redirect("PrintableInvoice.aspx?" + nameValues, false);
                    //Response.Redirect("PrintableInvoice.aspx?inv=" + Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[1]) + "-" + Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[2]), false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Populating gridview with MOPs
        protected void populateGridviewMOP(double amountPaid, string methodOfPayment)
        {
            //Collects current method for error tracking
            string method = "populateGridviewMOP";
            try
            {
                //gridID = 0;
                ////Checks if there have already been other MOPs added
                //if (Session["MethodsofPayment"] != null)
                //{
                //    //If there have been retrieve from session 
                //    mopList = (List<Mops>)Session["MethodsofPayment"];
                //    //Loop through each mop
                //    foreach (var mop in mopList)
                //    {
                //        //this will get the highest id in the mops
                //        if (mop.tableID > gridID)
                //            gridID = mop.tableID;
                //    }

                //}
                ////Store the new mop info into a checkout class
                //Mops tempCK = new Mops(methodOfPayment, amountPaid, gridID + 1);
                ////Retrieve the check out manager info from session
                //ckm = (CheckoutManager)Session["CheckOutTotals"];
                ////Add new mop method to gridview
                //mopList.Add(tempCK);
                ////Loop through new list of mops to get a new total of amount paid
                //foreach (var mop in mopList)
                //{
                //    dblAmountPaid += mop.amountPaid;
                //}
                ////Store amount paid and remove the new amount paid from the remaining balance
                //ckm.dblAmountPaid = dblAmountPaid;
                //ckm.dblRemainingBalance -= amountPaid;
                ////This is supposed to center all the info in the gridview 
                ////but doesn't seem to work
                //foreach (GridViewRow row in gvCurrentMOPs.Rows)
                //{
                //    foreach (TableCell cell in row.Cells)
                //    {
                //        cell.Attributes.CssStyle["text-align"] = "center";
                //    }
                //}
                ////Bind mop list to gridview
                //gvCurrentMOPs.DataSource = mopList;
                //gvCurrentMOPs.DataBind();
                //foreach (GridViewRow row in gvCurrentMOPs.Rows)
                //{
                //    foreach (TableCell cell in row.Cells)
                //    {
                //        cell.Attributes.CssStyle["text-align"] = "center";
                //    }
                //}
                ////Store mop list in a session
                //Session["MethodsofPayment"] = mopList;
                ////Display the remaining balance amounts
                //lblRemainingRefundDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                //txtAmountRefunding.Text = ckm.dblRemainingBalance.ToString("#0.00");
                //buttonDisable(ckm.dblRemainingBalance);

                InvoiceMOPsManager IMM = new InvoiceMOPsManager();
                IMM.AddNewMopToList(Request.QueryString["inv"].ToString(), amountPaid, methodOfPayment);
                UpdatePageTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}