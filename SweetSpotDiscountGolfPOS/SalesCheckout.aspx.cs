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
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        List<Checkout> mopList = new List<Checkout>();
        List<Cart> itemsInCart = new List<Cart>();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();
        CheckoutManager ckm;
        CurrentUser cu;
        EmployeeManager em = new EmployeeManager();
        public double dblRemaining;
        public double subtotal;
        public double gst;
        public double pst;
        public double balancedue;
        public double dblAmountPaid;
        public double tradeInCost;
        public double taxAmount;
        //Remove Prov or Gov Tax
        bool chargeGST;
        bool chargePST;
        double amountPaid;
        double dblShippingAmount;
        int tranType;
        int gridID;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCheckout.aspx";
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

                    List<Tax> t = new List<Tax>();
                    List<Cart> cart = new List<Cart>();
                    CalculationManager cm = new CalculationManager();
                    //Retrieves items in the cart from Session
                    cart = (List<Cart>)Session["ItemsInCart"];
                    //Retrieves date from session
                    DateTime recDate = Convert.ToDateTime(Session["strDate"]);
                    //Retrieves shipping
                    bool bolShipping = Convert.ToBoolean(Session["shipping"]);
                    //Checks if shipping was charged 
                    if (bolShipping)
                    {
                        //Retrieves customer number from Session
                        int custNum = (int)Convert.ToInt32(Session["key"].ToString());
                        //Uses customer number to fill a customer class
                        Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                        //Based on customer province and date get taxes
                        t = ssm.getTaxes(c.province, recDate);
                        lblShipping.Visible = true;
                        lblShippingAmount.Visible = true;
                        //Retrieve shipping amount from Session
                        dblShippingAmount = Convert.ToDouble(Session["ShippingAmount"].ToString());
                    }
                    else
                    {
                        //Query returns taxes based on current location
                        t = ssm.getTaxes(lm.getProvIDFromLocationID(cu.locationID), recDate);
                        //Sets shipping amouunt to 0
                        lblShipping.Visible = false;
                        lblShippingAmount.Visible = false;
                        dblShippingAmount = 0;
                    }
                    //Retrieves location id from Session
                    int location = cu.locationID;
                    //Creates checkout manager based on current items in cart
                    ckm = new CheckoutManager(cm.returnTotalAmount(cart, location), cm.returnDiscount(cart), cm.returnTradeInAmount(cart, location), dblShippingAmount, true, true, 0, 0, 0);
                    //Loops through each tax
                    foreach (var T in t)
                    {
                        switch (T.taxName)
                        {
                            //If tax is GST calculate and make visible
                            case "GST":
                                lblGovernment.Visible = true;
                                ckm.dblGst = cm.returnGSTAmount(T.taxRate, ckm.dblSubTotal + ckm.dblShipping);
                                lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                                lblGovernmentAmount.Visible = true;
                                btnRemoveGov.Visible = true;
                                break;
                            //If tax is PST calculate and make visible
                            case "PST":
                                lblProvincial.Visible = true;
                                ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                lblProvincialAmount.Visible = true;
                                btnRemoveProv.Visible = true;
                                break;
                            //If tax is HST calculate and make visible
                            case "HST":
                                lblProvincial.Visible = false;
                                lblGovernment.Text = "HST";
                                ckm.dblGst = cm.returnGSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblGovernmentAmount.Text = "$ " + gst.ToString("#0.00");
                                lblGovernmentAmount.Visible = true;
                                btnRemoveProv.Visible = false;
                                btnRemoveGov.Text = "HST";
                                break;
                            //If tax is RST calculate and make visible
                            case "RST":
                                lblProvincial.Visible = true;
                                lblProvincial.Text = "RST";
                                ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                lblProvincialAmount.Visible = true;
                                btnRemoveProv.Visible = true;
                                btnRemoveProv.Text = "RST";
                                break;
                            //If tax is QST calculate and make visible
                            case "QST":
                                lblProvincial.Visible = true;
                                lblProvincial.Text = "QST";
                                ckm.dblPst = cm.returnPSTAmount(T.taxRate, ckm.dblSubTotal);
                                lblProvincialAmount.Text = "$ " + pst.ToString("#0.00");
                                lblProvincialAmount.Visible = true;
                                btnRemoveProv.Visible = true;
                                btnRemoveProv.Text = "QST";
                                break;
                        }
                    }
                    //Add taxes to balance due and remaining balance
                    ckm.dblBalanceDue += ckm.dblGst + ckm.dblPst;
                    ckm.dblRemainingBalance += ckm.dblGst + ckm.dblPst;
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
                        ckm.dblRemainingBalance = ckm.dblBalanceDue - ckm.dblAmountPaid;
                    }

                    //***Assign each item to its Label.
                    lblTotalInCartAmount.Text = "$ " + ckm.dblTotal.ToString("#0.00");
                    lblTotalInDiscountsAmount.Text = "$ " + ckm.dblDiscounts.ToString("#0.00");
                    lblTradeInsAmount.Text = "$ " + ckm.dblTradeIn.ToString("#0.00");
                    lblSubTotalAmount.Text = "$ " + (ckm.dblSubTotal + ckm.dblShipping).ToString("#0.00");
                    lblShippingAmount.Text = "$ " + ckm.dblShipping.ToString("#0.00");
                    lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                    lblProvincialAmount.Text = "$ " + ckm.dblPst.ToString("#0.00");
                    lblBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                    lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                    //Stores totals in the checkout manager
                    Session["CheckOutTotals"] = ckm;
                    //Updates the amount paying with the remaining balance
                    txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
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
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtAmountPaying.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Calls client side script to display change due to customer
                    ClientScript.RegisterStartupScript(GetType(), "hwa", "userInput(" + amountPaid + ")", true);
                    //Collects mop type
                    string methodOfPayment = "Cash";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment);
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
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtAmountPaying.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "MasterCard";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment);
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
                string boxResult = txtAmountPaying.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "Debit";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment);
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
        //Visa
        protected void mopVisa_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "mopVisa_Click";
            try
            {
                //Retrieves checkout totals from Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Collects the amount paying as string
                string boxResult = txtAmountPaying.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "Visa";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment);
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
                string boxResult = txtAmountPaying.Text;
                //Checks that string is not empty
                if (boxResult != "")
                {
                    //Converts amount to double
                    amountPaid = Convert.ToDouble(boxResult);
                    //Collects mop type
                    string methodOfPayment = "Gift Card";
                    //Calls procedure to add it to a grid view
                    populateGridviewMOP(amountPaid, methodOfPayment);
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
        protected void populateGridviewMOP(double amountPaid, string methodOfPayment)
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
                Checkout tempCK = new Checkout(methodOfPayment, amountPaid, gridID + 1);
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
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
                buttonDisable(ckm.dblRemainingBalance);
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
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
                buttonDisable(ckm.dblRemainingBalance);
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

        protected void btnRemoveGovTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveGovTax";
            try
            {
                //Retrieves the checkout manager from the Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Sets variable to the current status of GST true/false
                chargeGST = ckm.blGst;
                //Sets tax amount to 0
                taxAmount = 0;
                //Checks if the current status is true
                if (chargeGST)
                {
                    //chargeGST is now false so remove GST
                    chargeGST = false;
                    ckm.blGst = chargeGST;
                    lblGovernmentAmount.Text = "$ 0.00";
                    //Sets tax amount to a negative version of GST
                    taxAmount = -(ckm.dblGst);
                    //Changes button name
                    btnRemoveGov.Text = "Add GST";
                }
                else
                {
                    //chargeGST is now True so add GST
                    chargeGST = true;
                    ckm.blGst = chargeGST;
                    //Sets label to the GST amount
                    lblGovernmentAmount.Text = "$ " + ckm.dblGst.ToString("#0.00");
                    //Sets tax amount to a positive version of GST
                    taxAmount = ckm.dblGst;
                    //Changes button name
                    btnRemoveGov.Text = "Remove GST";
                }
                //Adds the tax amount to the balance due
                ckm.dblBalanceDue += taxAmount;
                //Displays new balance due amount
                lblBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                //Adds the tax amount to the Remaining balance
                ckm.dblRemainingBalance += taxAmount;
                //Displays the remaining balance
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
                //Stores the checkout manager
                Session["CheckOutTotals"] = ckm;
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
        protected void btnRemoveProvTax(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnRemoveProvTax";
            try
            {
                //Retrieves the checkout manager from the Session
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Sets variable to the current status of PST true/false
                chargePST = ckm.blPst;
                //Sets tax amount to 0
                taxAmount = 0;
                //Checks if the current status is true
                if (chargePST)
                {
                    //chargePST is now false so remove PST
                    chargePST = false;
                    ckm.blPst = chargePST;
                    lblProvincialAmount.Text = "$ 0.00";
                    //Sets tax amount to a negative version of PST
                    taxAmount = -(ckm.dblPst);
                    //Changes button name
                    btnRemoveProv.Text = "Add PST"; //*** Need to figure out proper name of tax
                }
                else
                {
                    //chargePST is now True so add PST
                    chargePST = true;
                    ckm.blPst = chargePST;
                    //Sets label to the PST amount
                    lblProvincialAmount.Text = "$ " + ckm.dblPst.ToString("#0.00");
                    //Sets tax amount to a positive version of PST
                    taxAmount = ckm.dblPst;
                    //Changes button name
                    btnRemoveProv.Text = "Remove PST";
                }
                //Adds the tax amount to the balance due
                ckm.dblBalanceDue += taxAmount;
                //Displays new balance due amount
                lblBalanceAmount.Text = "$ " + ckm.dblBalanceDue.ToString("#0.00");
                //Adds the tax amount to the Remaining balance
                ckm.dblRemainingBalance += taxAmount;
                //Displays the remaining balance
                lblRemainingBalanceDueDisplay.Text = "$ " + ckm.dblRemainingBalance.ToString("#0.00");
                txtAmountPaying.Text = ckm.dblRemainingBalance.ToString("#0.00");
                //Stores the checkout manager
                Session["CheckOutTotals"] = ckm;
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
        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                //Checks session to see if it's null
                if (Session["ItemsInCart"] != null)
                {
                    //Retrieves items in the cart from Session
                    itemsInCart = (List<Cart>)Session["ItemsInCart"];
                }
                //Loops through each item in the cart
                foreach (var cart in itemsInCart)
                {
                    //Queries the database to return the remainig quantity of the sku
                    int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                    //Updates the quantity in stock adding the quantity in the cart
                    idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
                }
                //Nullifies each of the sessions 
                Session["returnedCart"] = null;
                Session["key"] = null;
                Session["shipping"] = null;
                Session["ShippingAmount"] = null;
                Session["ItemsInCart"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["Invoice"] = null;
                Session["TranType"] = null;
                Session["searchReturnInvoices"] = null;
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

        protected void btnReturnToCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnReturnToCart_Click";
            try
            {
                //Sets session to true
                //Changes to Sales Cart page
                Server.Transfer("SalesCart.aspx", false);
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

        protected void btnLayaway_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnLayaway_Click";
            try
            { //Currently not being used
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

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnFinalize_Click";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                bool bolValidPassword = em.returnCanEmployeeMakeSale(em.returnEmployeeIDFromPassword(Convert.ToInt32(txtEmployeePasscode.Text)));
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
                        tranType = Convert.ToInt32(Session["TranType"]);
                        List<Cart> cart = new List<Cart>();
                        //Determines the Cart to be used based on transaction
                        if (tranType == 1) { cart = (List<Cart>)Session["ItemsInCart"]; }
                        else if (tranType == 2) { cart = (List<Cart>)Session["returnedCart"]; }

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
                        idu.mainInvoice(ckm, cart, mopList, c, emp, tranType, (Session["Invoice"]).ToString(), txtComments.Text);

                        //Nullifies all related sessions
                        Session["useInvoice"] = false;
                        Session["shipping"] = null;
                        Session["ShippingAmount"] = null;
                        Session["searchReturnInvoices"] = null;
                        Session["actualInvoiceInfo"] = ssm.getSingleInvoice(Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[1]), Convert.ToInt32(((Session["Invoice"]).ToString()).Split('-')[2]));
                        //Changes page to printable invoice
                        Server.Transfer("PrintableInvoice.aspx", false);
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
        public void buttonDisable(double rb)
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
    }
}