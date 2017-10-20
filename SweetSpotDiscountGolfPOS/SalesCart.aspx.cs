using System;
using SweetShop;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotProShop;
using System.Data;
using System.Threading.Tasks;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using System.Threading;
using System.Globalization;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesCart : System.Web.UI.Page
    {
        public string skuString;
        public int skuInt;
        public int invNum;
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        List<Items> invoiceItems = new List<Items>();
        List<Cart> itemsInCart = new List<Cart>();
        List<Cart> returnedCart = new List<Cart>();
        List<Cart> temp = new List<Cart>();
        LocationManager lm = new LocationManager();
        Cart tempItemInCart;
        Object o = new Object();
        CurrentUser cu;
        List<Customer> c;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCart.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                lblInvalidQty.Visible = false;
                if (!Page.IsPostBack)
                {
                    //Retrieves transaction type from session
                    int tranType = Convert.ToInt32(Session["TranType"]);
                    if (tranType == 1)
                    {
                        //if transaction type is sales then set focus on the search field
                        txtSearch.Focus();
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
                        string loc = cu.locationName;
                        //Get the next invoice number, sets in texyt box and stores in session
                        if (Session["Invoice"] == null)
                        {
                            invNum = idu.getNextInvoiceNum();
                            lblInvoiceNumberDisplay.Text = loc + "-" + invNum + "-1";
                            Session["Invoice"] = lblInvoiceNumberDisplay.Text;
                        }
                        else { lblInvoiceNumberDisplay.Text = Session["Invoice"].ToString(); }
                        //Checks the cart to see if there are any items in it
                        if (Session["ItemsInCart"] != null)
                        {
                            //If there are bind to data grid and get a subtotal of the items
                            grdCartItems.DataSource = Session["ItemsInCart"];
                            grdCartItems.DataBind();
                            lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"]).ToString();
                        }
                    }
                    //This will no longer be used as there is a seperate page for returns
                    //else if (tranType == 2)
                    //{
                    //    btnJumpToInventory.Visible = false;
                    //    Invoice rInvoice = (Invoice)Session["searchReturnInvoices"];
                    //    if (Session["returnedCart"] != null)
                    //    {
                    //        itemsInCart = (List<Cart>)Session["ItemsInCart"];
                    //        returnedCart = (List<Cart>)Session["returnedCart"];
                    //        grdReturningItems.DataSource = returnedCart;
                    //        grdReturningItems.DataBind();
                    //        lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(returnedCart).ToString("#0.00");
                    //    }
                    //    else
                    //    {
                    //        temp = ssm.returningItems(rInvoice.invoiceNum, rInvoice.invoiceSub);
                    //        foreach (var item in temp)
                    //        {
                    //            if (item.typeID != 0)
                    //            {
                    //                itemsInCart.Add(item);
                    //            }
                    //        }
                    //    }
                    //    lblCustomerDisplay.Visible = true;
                    //    lblCustomerDisplay.Text = rInvoice.customerName.ToString();
                    //    txtCustomer.Visible = false;
                    //    btnCustomerSelect.Visible = false;
                    //    RadioButton1.Visible = false;
                    //    RadioButton2.Visible = false;
                    //    lblShipping.Visible = false;
                    //    txtShippingAmount.Visible = false;
                    //    lblInvoiceNumberDisplay.Text = Convert.ToString(Session["Loc"]) + "-" + rInvoice.invoiceNum.ToString() + "-" + idu.getNextInvoiceSubNum(rInvoice.invoiceNum).ToString();
                    //    Session["Invoice"] = lblInvoiceNumberDisplay.Text;
                    //    lblDateDisplay.Text = rInvoice.invoiceDate.ToString();
                    //    txtSearch.Visible = false;
                    //    btnInventorySearch.Visible = false;
                    //    grdInventorySearched.Visible = false;
                    //    grdCartItems.Visible = false;
                    //    grdInvoicedItems.Visible = true;
                    //    grdReturningItems.Visible = true;

                    //    lblSubtotal.Text = "Return Total:";
                    //    btnCancelSale.Text = "Cancel Return";
                    //    btnProceedToCheckout.Text = "Reimburse Customer";
                    //    Session["ItemsInCart"] = itemsInCart;
                    //    grdInvoicedItems.DataSource = itemsInCart;
                    //    grdInvoicedItems.DataBind();

                    //}
                }
                //Store date in a session
                Session["strDate"] = lblDateDisplay.Text;
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




        //protected void btnCustomerSelect_Click(object sender, EventArgs e)
        //{
        //    //Collects current method for error tracking
        //    string method = "btnCustomerSelect_Click";
        //    try
        //    {
        //        //Check if there are items in the cart
        //        if (Session["ItemsInCart"] != null)
        //        {
        //            //If there are pass the session into variable for use
        //            itemsInCart = (List<Cart>)Session["ItemsInCart"];
        //        }
        //        //loops through each item in th cart
        //        foreach (var cart in itemsInCart)
        //        {
        //            //Queries the remaining quantity in stock
        //            int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
        //            //Updates the quantity add the cart quantity back into stock
        //            idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
        //        }

        //        lblInvalidQty.Visible = false;
        //        //Nullifies are relative sessions
        //        Session["key"] = null;
        //        Session["shipping"] = null;
        //        Session["ItemsInCart"] = null;
        //        Session["CheckOutTotals"] = null;
        //        Session["MethodsofPayment"] = null;
        //        //Changes page to Customer Home page to select a customer
        //        Server.Transfer("CustomerHomePage.aspx", false);
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log employee number
        //        int employeeID = cu.empID;
        //        //Log current page
        //        string currPage = Convert.ToString(Session["currPage"]);
        //        //Log all info into error table
        //        er.logError(ex, employeeID, currPage, method, this);
        //        //string prevPage = Convert.ToString(Session["prevPage"]);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occured and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator", this);
        //        //Server.Transfer(prevPage, false);
        //    }
        //}

        protected void btnCustomerSelect_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCustomerSelect_Click";
            try
            {
                if (btnCustomerSelect.Text == "Cancel")
                {
                    btnCustomerSelect.Text = "Select Different Customer";
                    grdCustomersSearched.Visible = false;
                    int custNum = (int)(Convert.ToInt32(Session["key"].ToString()));
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
        protected void btnSearchCustomers_Click(object sender, EventArgs e)
        {
            grdCustomersSearched.Visible = true;
            c = ssm.GetCustomerfromSearch(txtCustomer.Text);
            grdCustomersSearched.DataSource = c;
            grdCustomersSearched.DataBind();
        }
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //Get info from textboxes
            TextBox fName = grdCustomersSearched.FooterRow.FindControl("txtFirstName") as TextBox;
            TextBox lName = grdCustomersSearched.FooterRow.FindControl("txtLastName") as TextBox;
            TextBox phoneNumber = grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber") as TextBox;
            //Using current user's info
            int provStateID = lm.getProvIDFromLocationID(cu.locationID);
            int countryID = lm.countryIDFromProvince(provStateID);
            //Creating a customer
            Customer c = new Customer(0, fName.Text, lName.Text, "", "", phoneNumber.Text, "", false, "", "", provStateID, countryID, "");
            //Set the session key to customer ID
            string key = ssm.addCustomer(c).ToString();
            Session["key"] = key;
            //Hide stuff
            grdCustomersSearched.Visible = false;
            //Set name in text box
            txtCustomer.Text = fName.Text + " " + lName.Text;
            MessageBox.ShowMessage(key, this.Page);
            btnCustomerSelect.Text = "Select Different Customer";
        }
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomersSearched.PageIndex = e.NewPageIndex;
            //Looks through database and returns a list of customers
            //based on the search criteria entered
            SweetShopManager ssm = new SweetShopManager();
            c = ssm.GetCustomerfromSearch(txtSearch.Text);
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
                btnCustomerSelect.Text = "Select Different Customer";
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


        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInventorySearch_Click";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                lblInvalidQty.Visible = false;
                //Retrieves the location from session
                string loc = cu.locationName;
                //Checks to see if some number or text has been entered into the search field
                if (!txtSearch.Text.Equals("") && !txtSearch.Text.Equals(null))
                {
                    //If something was entered then check to see if it is words
                    if (!int.TryParse(txtSearch.Text, out skuInt))
                    {
                        //If it is then pass into string
                        skuString = txtSearch.Text;
                        //use string and location to call query
                        //Query will return list of items that match the word(s)
                        invoiceItems = ssm.returnSearchFromAllThreeItemSets(skuString, loc);
                    }
                    else
                    {
                        //Text entered is a number
                        skuString = txtSearch.Text;
                        //this looks for the sku and returns all items that match sku
                        List<Items> i = idu.getItemByID(Convert.ToInt32(skuInt), loc);

                        //Checks to see if at least one item was returned
                        if (i != null && i.Count >= 1)
                        {
                            //Add item to the list
                            invoiceItems = i;
                            //invoiceItems.Add(i.ElementAt(0));
                        }
                    }
                }
                //Binds list to the grid view
                grdInventorySearched.DataSource = invoiceItems;
                grdInventorySearched.DataBind();
                //Clears search text box
                txtSearch.Text = "";
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
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            try
            {
                lblInvalidQty.Visible = false;
                //Gets the index of the selected row
                int index = e.RowIndex;
                //Stores all the data for each element in the row
                int sku = Convert.ToInt32(grdCartItems.Rows[index].Cells[2].Text);
                int quantity = Convert.ToInt32(grdCartItems.Rows[index].Cells[3].Text);
                int itemType = Convert.ToInt32(((Label)grdCartItems.Rows[index].Cells[8].FindControl("lblTypeID")).Text);
                //Queries database to get the total quantity of the selected sku
                int remainingQTY = idu.getquantity(sku, itemType);
                //Takes the items currently in the cart puts them into a duplicate
                //variable from the session
                List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
                //Loops through each item in the duplicate cart
                foreach (var cart in duplicateCart)
                {
                    //Checks to see if sku in duplicate cart = the selected sku
                    if (cart.sku != sku)
                    { //when sku doesn't match add sku back into the cart
                        itemsInCart.Add(cart);
                    }
                    else
                    {//When the skus match add the quantity from cart back into stock
                        idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY + quantity));
                    }
                }
                //Remove the indexed pointer
                grdCartItems.EditIndex = -1;
                //Store updated cart into session
                Session["ItemsInCart"] = itemsInCart;
                //bind items back to grid view
                grdCartItems.DataSource = itemsInCart;
                grdCartItems.DataBind();
                //Calculate new subtotal
                lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(itemsInCart).ToString("#0.00");
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
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowEditing";
            try
            {
                lblInvalidQty.Visible = false;
                //Gets the index of the selected row
                int index = e.NewEditIndex;
                //Stores the quantity of selected row into session
                int quantity = Convert.ToInt32(grdCartItems.Rows[index].Cells[3].Text);
                Session["originalQTY"] = quantity;
                //binds grid view to the items in cart setting the indexed item up to edit
                //it's available columns
                grdCartItems.DataSource = (List<Cart>)Session["ItemsInCart"];
                grdCartItems.EditIndex = index;
                grdCartItems.DataBind();
                //Recalculates subtotal
                lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
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
        //Currently used for cancelling the edit
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "ORowCanceling";
            try
            {
                lblInvalidQty.Visible = false;
                //Clears the indexed row
                grdCartItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdCartItems.DataSource = Session["ItemsInCart"];
                grdCartItems.DataBind();
                //Recalcluate subtotal
                lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
                //Nulls the quantity session
                Session["originalQTY"] = null;
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
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowUpdating";
            try
            {
                lblInvalidQty.Visible = false;
                //Gets the index of the selected row
                int index = e.RowIndex;
                //Stores all the data for each element in the row
                TextBox discountAmount = (TextBox)grdCartItems.Rows[index].Cells[6].FindControl("txtAmnt");
                string discountOnItem = discountAmount.Text;
                Label price = (Label)grdCartItems.Rows[index].Cells[5].FindControl("price");
                double sPrice = double.Parse(price.Text, NumberStyles.Currency);
                Label cost = (Label)grdCartItems.Rows[index].Cells[5].FindControl("cost");
                double sCost = double.Parse(cost.Text, NumberStyles.Currency);
                bool radioButtonSelected = false;
                CheckBox chkPerecent = (CheckBox)grdCartItems.Rows[index].Cells[6].FindControl("ckbPercentageEdit");

                radioButtonSelected = chkPerecent.Checked;

                //if (chkPerecent.Checked)
                //{
                //    radioButtonSelected = true;
                //}
                //else
                //{
                //    radioButtonSelected = false;
                //}

                bool tradeInItemInCart = ((CheckBox)grdCartItems.Rows[index].Cells[7].FindControl("chkTradeIn")).Checked;
                string itemType = ((Label)grdCartItems.Rows[index].Cells[8].FindControl("lblTypeID")).Text;
                string sku = grdCartItems.Rows[index].Cells[2].Text;
                string quantity = ((TextBox)grdCartItems.Rows[index].Cells[3].Controls[0]).Text;
                string desc = grdCartItems.Rows[index].Cells[4].Text;
                //creates a temp item with the new updates
                tempItemInCart = new Cart(Convert.ToInt32(sku), desc, Convert.ToInt32(quantity), sPrice, sCost,
                    Convert.ToDouble(discountOnItem), radioButtonSelected, tradeInItemInCart, Convert.ToInt32(itemType));

                //Sets current items in cart from stored session into duplicate cart 
                List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
                //Queries the database to get the current remaining quantity of the selected sku
                int remainingQTY = idu.getquantity(Convert.ToInt32(sku), Convert.ToInt32(itemType));
                //Returns the difference between the original quantity in the cart and the new
                //quantity in the cart
                int differenceInQTY = Convert.ToInt32(Session["originalQTY"]) - Convert.ToInt32(quantity);
                //Checks the total between remaining qty and difference is a negative
                if ((remainingQTY + differenceInQTY) < 0)
                {
                    //if it is less than 0 then there is not enough in invenmtory to sell
                    //set items in cart to all original items
                    itemsInCart = duplicateCart;
                    lblInvalidQty.Visible = true;
                    //Display error message
                    lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                    lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    //Else there is enogh quantity to make the sale
                    //Loop through each item in the duplicate cart
                    foreach (var cart in duplicateCart)
                    {
                        //Check to see when the duplicate cart sku matches the selected updated sku
                        if (cart.sku == tempItemInCart.sku)
                        {
                            //Checks if the updated quantity does not = 0
                            if (tempItemInCart.quantity != 0)
                            {
                                //As long as there is a valid quantity add the updated 
                                //sku to the cart
                                itemsInCart.Add(tempItemInCart);
                                //Query database to remove the addition quantity
                                idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY + differenceInQTY));
                            }
                        }
                        else
                        {
                            //if sku does not match selected sku then add item back into cart
                            itemsInCart.Add(cart);
                        }
                    }
                }
                //Clears the indexed row
                grdCartItems.EditIndex = -1;
                //Sets cart items to Session
                Session["ItemsInCart"] = itemsInCart;
                //Binds cart items to grid view
                grdCartItems.DataSource = itemsInCart;
                grdCartItems.DataBind();
                //Recalculates the new subtotal
                lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(itemsInCart).ToString("#0.00");
                //Nullifies the quantity session
                Session["originalQTY"] = null;
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
        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                //Retrieves the transaction type from session
                int tranType = Convert.ToInt32(Session["TranType"]);
                //Check transaction type for Sale
                if (tranType == 1)
                {
                    //Checks if there are already items in the cart
                    if (Session["ItemsInCart"] != null)
                    {
                        //Sets cart session into variable
                        itemsInCart = (List<Cart>)Session["ItemsInCart"];
                    }
                    //Loops through each item in the cart
                    foreach (var cart in itemsInCart)
                    {
                        //Queries database to get the remaing quantity left in stock
                        int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                        //Updates the quntity in stock adding the quantity in cart
                        idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
                    }
                }

                lblInvalidQty.Visible = false;
                //* *update * *to null any new seesions btnCancelSale_Click;
                Session["returnedCart"] = null;
                Session["key"] = null;
                Session["shipping"] = null;
                Session["ItemsInCart"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["Invoice"] = null;
                Session["searchReturnInvoices"] = null;
                Session["TranType"] = null;
                Session["ShippingAmount"] = null;
                Session["strDate"] = null;
                //Change to Home Page
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
        protected void btnProceedToCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProceedToCheckout_Click";
            try
            {
                lblInvalidQty.Visible = false;
                //Checks to see if shipping was selected
                if (!RadioButton2.Checked)
                {
                    //Sets sessions based on result
                    Session["shipping"] = false;
                    Session["ShippingAmount"] = 0;
                }
                else
                {
                    //Sets sessions based on result
                    Session["shipping"] = true;
                    Session["ShippingAmount"] = txtShippingAmount.Text;
                }
                //Changes to Sales Checkout page
                Server.Transfer("SalesCheckout.aspx", false);
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
        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInventorySearched_RowCommand";
            try
            {
                lblInvalidQty.Visible = false;
                bool bolAdded = false;
                //Checks to see if there are any items in the current cart
                if (Session["ItemsInCart"] != null)
                {
                    //If there are set the session into variable
                    itemsInCart = (List<Cart>)Session["ItemsInCart"];
                }
                //Retrieves the selected sku
                int itemKey = Convert.ToInt32(e.CommandArgument.ToString());
                //Checks that the selected command is to AddItem
                if (e.CommandName == "AddItem")
                {
                    //Loops through each item in the cart
                    foreach (var cart in itemsInCart)
                    {
                        //Checks to see if the cart sku matches selected sku
                        //and that the item hasn't already been added
                        if (cart.sku == itemKey && !bolAdded)
                        {
                            //Queries the remaining quantity in stock of the selected sku
                            int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                            //Checks that the new quantity will exceed the remaing qunatity
                            if ((cart.quantity + 1) > remainingQTY)
                            {
                                //Advises user that not enough qunatity to sell item
                                lblInvalidQty.Visible = true;
                                lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                                lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                //There is enough in stock to make sale
                                //increase quantity in the cart and remove from stock
                                cart.quantity += 1;
                                idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY - 1));
                            }
                            bolAdded = true;
                        }
                    }

                    //int locationID = Convert.ToInt32(lblLocationID.Text);
                    //int locationID = cu.locationID;
                    ////Finding the min and max range for trade ins
                    //int[] range = idu.tradeInSkuRange(locationID);

                    //If the itemKey is between or equal to the ranges, do trade in
                    if (itemKey == 100000)
                    {
                        //Trade In Sku to add in SK
                        string redirect = "<script>window.open('TradeINEntry.aspx');</script>";
                        Response.Write(redirect);
                    }
                    //Cart is empty or sku didn't match any items currently in stock
                    else if (itemsInCart.Count == 0 || !bolAdded)
                    {
                        //Returns a club, clothing, accessories based on the selected sku
                        Clubs c = ssm.singleItemLookUp(itemKey);
                        Clothing cl = ssm.getClothing(itemKey);
                        Accessories ac = ssm.getAccessory(itemKey);
                        int itemType = 0;
                        //Checks that club looked up an item
                        if (c.sku != 0)
                        {
                            //if club has value then set type and pass to object
                            itemType = c.typeID;
                            o = c as Object;
                        }
                        //Checks that clothing looked up an item
                        else if (cl.sku != 0)
                        {
                            //if clothing has value then set type and pass to object
                            itemType = cl.typeID;
                            o = cl as Object;
                        }
                        //Checks that accessory looked up an item
                        else if (ac.sku != 0)
                        {
                            //if accessories has value then set type and pass to object
                            itemType = ac.typeID;
                            o = ac as Object;
                        }
                        //Retrieves the quantity in stock of the selected item
                        int remainingQTY = idu.getquantity(itemKey, itemType);
                        //Checks to see if there is 0 of the item in stock
                        if (1 > remainingQTY)
                        {
                            //Displays to user that item has no quantity
                            MessageBox.ShowMessage("This item has 0 quantity", this);
                            lblInvalidQty.Visible = true;
                        }
                        else
                        {
                            //Query database for item matching object and add to the current cart
                            itemsInCart.Add(idu.addingToCart(o));
                            //Remove the quantity from amount in stock
                            idu.removeQTYfromInventoryWithSKU(itemKey, itemType, (remainingQTY - 1));
                        }
                    }
                }
                //Set items in cart into Session
                Session["ItemsInCart"] = itemsInCart;
                //Bind items in cart to grid view
                grdCartItems.DataSource = itemsInCart;
                grdCartItems.DataBind();
                //Set an empty variable to bind to the searched items grid view so it is empty
                List<Items> nullGrid = new List<Items>();
                nullGrid = null;
                grdInventorySearched.DataSource = nullGrid;
                grdInventorySearched.DataBind();
                //Recalculate the new subtotal
                lblSubtotalDisplay.Text = "$ " + ssm.returnSubtotalAmount(itemsInCart).ToString("#.00");
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
        protected void btnJumpToInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnJumpToInventory_Click";
            try
            {
                //Inventory screen in new window/tab
                string redirect = "<script>window.open('InventoryHomePage.aspx');</script>";
                Response.Write(redirect);
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
    }
}