﻿using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class PurchasesCart : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        CustomerManager CM = new CustomerManager();

        public int receiptNum;
        SweetShopManager ssm = new SweetShopManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        List<Cart> itemsInCart = new List<Cart>();
        Cart purchItem = new Cart();
        List<Customer> c;
        LocationManager lm = new LocationManager();
        SalesCalculationManager scm = new SalesCalculationManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PurchasesCart.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (!Page.IsPostBack)
                {
                    //Checks if there is a Customer Number stored in the Session
                    if (Session["key"] != null)
                    {
                        //If yes then convert number to int and call Customer class using it
                        int custNum = (int)(Convert.ToInt32(Session["key"].ToString()));
                        Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                        //Set name in text box
                        txtCustomer.Text = c.firstName + " " + c.lastName;
                    }
                    //display system time in Sales Page
                    DateTime today = DateTime.Today;
                    lblDateDisplay.Text = today.ToString("yyyy-MM-dd");
                    //Retrieves location from Session
                    string loc = CU.locationName;

                    if (Session["Invoice"] == null)
                    {
                        receiptNum = idu.getNextReceiptNum();
                        lblReceiptNumberDisplay.Text = loc + "-" + receiptNum;
                        Session["Invoice"] = lblReceiptNumberDisplay.Text;
                    }
                    else { lblReceiptNumberDisplay.Text = Session["Invoice"].ToString(); }

                    if (Session["ItemsInCart"] != null)
                    {
                        itemsInCart = (List<Cart>)Session["ItemsInCart"];
                    }
                    grdPurchasedItems.DataSource = Session["ItemsInCart"];
                    grdPurchasedItems.DataBind();
                    lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount(itemsInCart).ToString();
                }
                //Store date in a session
                Session["strDate"] = lblDateDisplay.Text;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnCustomerSelect_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCustomerSelect_Click";
            try
            {
                if (btnCustomerSelect.Text == "Cancel")
                {
                    btnCustomerSelect.Text = "Change Customer";
                    grdCustomersSearched.Visible = false;
                    int custNum = (Convert.ToInt32(Session["key"].ToString()));
                    Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //Set name in text box
                    txtCustomer.Text = c.firstName + " " + c.lastName;
                }
                else
                {
                    grdCustomersSearched.Visible = true;
                    c = ssm.GetCustomerfromSearch(txtCustomer.Text);
                    grdCustomersSearched.DataSource = c;
                    grdCustomersSearched.DataBind();
                    if (grdCustomersSearched.Rows.Count > 0)
                    {
                        btnCustomerSelect.Text = "Cancel";
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //Get info from textboxes
            TextBox fName = grdCustomersSearched.FooterRow.FindControl("txtFirstName") as TextBox;
            TextBox lName = grdCustomersSearched.FooterRow.FindControl("txtLastName") as TextBox;
            TextBox phoneNumber = grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber") as TextBox;
            TextBox email = grdCustomersSearched.FooterRow.FindControl("txtEmail") as TextBox;
            CheckBox marketing = grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment") as CheckBox;
            bool enrolled = false;
            if (marketing.Checked)
            {
                enrolled = true;
            }
            //Using current user's info
            int provStateID = lm.getProvIDFromLocationID(CU.locationID);
            int countryID = lm.getCountryIDFromProvID(provStateID);
            //Creating a customer
            Customer c = new Customer(0, fName.Text, lName.Text, "", "", phoneNumber.Text, "", email.Text, "", provStateID, countryID, "", enrolled);
            //Set the session key to customer ID
            string key = CM.addCustomer(c).ToString();
            Session["key"] = key;
            //Hide stuff
            grdCustomersSearched.Visible = false;
            //Set name in text box
            txtCustomer.Text = fName.Text + " " + lName.Text;
            btnCustomerSelect.Text = "Change Customer";
        }
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomersSearched.PageIndex = e.NewPageIndex;
            //Looks through database and returns a list of customers
            //based on the search criteria entered
            SweetShopManager ssm = new SweetShopManager();
            c = ssm.GetCustomerfromSearch(txtCustomer.Text);
            //Binds the results to the gridview
            grdCustomersSearched.Visible = true;
            grdCustomersSearched.DataSource = c;
            grdCustomersSearched.DataBind();
        }
        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdCustomersSearched_RowCommand";
            try
            {
                //grabs the command argument for the command pressed 
                string key = e.CommandArgument.ToString();
                if (e.CommandName == "SwitchCustomer")
                {
                    //if command argument is SwitchCustomer, set the new key
                    Session["key"] = key;
                    //Hide stuff
                    grdCustomersSearched.Visible = false;
                    //Get customer name
                    Customer c = ssm.GetCustomerbyCustomerNumber(Convert.ToInt32(key));
                    //Set name in text box
                    txtCustomer.Text = c.firstName + " " + c.lastName;
                }
                btnCustomerSelect.Text = "Change Customer";
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void btnAddPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method error tracking
            string method = "btnAddPurchase_Click";
            try
            {
                //Check if there are items in the cart
                if (Session["ItemsInCart"] != null)
                {
                    //If there are pass the session into variable for use
                    itemsInCart = (List<Cart>)Session["ItemsInCart"];
                }
                purchItem.sku = idu.reserveTradeInSKu(CU.locationID);
                purchItem.quantity = 1;
                purchItem.description = "";
                purchItem.cost = 0.00;
                itemsInCart.Add(purchItem);
                //Set items in cart into Session
                Session["ItemsInCart"] = itemsInCart;
                //Bind items in cart to grid view
                grdPurchasedItems.DataSource = itemsInCart;
                grdPurchasedItems.DataBind();
                lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount(itemsInCart).ToString();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowEditing";
            try
            {
                //Gets the index of the selected row
                int index = e.NewEditIndex;
                //binds grid view to the items in cart setting the indexed item up to edit
                //it's available columns
                grdPurchasedItems.DataSource = (List<Cart>)Session["ItemsInCart"];
                grdPurchasedItems.EditIndex = index;
                grdPurchasedItems.DataBind();
                //Recalculates subtotal
                lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount(itemsInCart).ToString();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Currently used for cancelling the edit
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "ORowCanceling";
            try
            {
                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdPurchasedItems.DataSource = Session["ItemsInCart"];
                grdPurchasedItems.DataBind();
                //Recalcluate subtotal
                lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowUpdating";
            try
            {
                int index = e.RowIndex;
                //Stores all the data for each element in the row
                int sku = Convert.ToInt32(grdPurchasedItems.Rows[index].Cells[1].Text);
                string desc = ((TextBox)grdPurchasedItems.Rows[index].Cells[2].Controls[0]).Text;
                double cost = Convert.ToDouble(((TextBox)grdPurchasedItems.Rows[index].Cells[3].Controls[0]).Text);

                //creates a temp item with the new updates
                purchItem.sku = sku;
                purchItem.cost = cost;
                purchItem.description = desc;

                //Sets current items in cart from stored session into duplicate cart 
                List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];

                //Loop through each item in the duplicate cart
                foreach (var cart in duplicateCart)
                {
                    //Check to see when the duplicate cart sku matches the selected updated sku
                    if (cart.sku == purchItem.sku)
                    {
                        purchItem.quantity = cart.quantity;
                        //sku to the cart
                        itemsInCart.Add(purchItem);
                    }
                    else
                    {
                        //if sku does not match selected sku then add item back into cart
                        itemsInCart.Add(cart);
                    }
                }
                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Sets cart items to Session
                Session["ItemsInCart"] = itemsInCart;
                //Binds cart items to grid view
                grdPurchasedItems.DataSource = itemsInCart;
                grdPurchasedItems.DataBind();
                //Recalculates the new subtotal
                lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount(itemsInCart).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                                + "If you continue to receive this message please contact "
                                + "your system administrator.", this);
            }
        }
        protected void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                Session["key"] = null;
                Session["ItemsInCart"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["Invoice"] = null;
                Session["TranType"] = null;
                Session["strDate"] = null;
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnProceedToPayOut_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProceedToCheckout_Click";
            try
            {
                //Changes to Sales Checkout page
                Response.Redirect("PurchasesCheckout.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}