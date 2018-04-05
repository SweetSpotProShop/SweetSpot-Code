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
        ErrorReporting ER = new ErrorReporting();
        CustomerManager CM = new CustomerManager();
        LocationManager LM = new LocationManager();
        InvoiceManager IM = new InvoiceManager();
        ItemsManager ITM = new ItemsManager();
        InvoiceItemsManager IIM = new InvoiceItemsManager();
        SalesCalculationManager SCM = new SalesCalculationManager();
        CurrentUser CU;

        //Still need to account for a duplicate item being added
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCart.aspx";
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
                    lblInvalidQty.Visible = false;
                    if (!Page.IsPostBack)
                    {
                        //if transaction type is sales then set focus on the search field
                        txtSearch.Focus();
                        //If yes then convert number to int and call Customer class using it
                        List<Customer> c = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["cust"].ToString()));
                        //Set name in text box
                        txtCustomer.Text = c[0].firstName + " " + c[0].lastName;
                        lblDateDisplay.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        lblInvoiceNumberDisplay.Text = Request.QueryString["inv"].ToString();
                        object[] invoiceInfo = { Request.QueryString["inv"].ToString(), c[0].customerId, CU.empID, CU.locationID };
                        if (!IM.ReturnBolInvoiceExists(Request.QueryString["inv"].ToString()))
                        {
                            IM.CreateInitialTotalsForTable(invoiceInfo);
                        }
                        //change to gather the items from table
                        grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                        grdCartItems.DataBind();
                        Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        ////lblSubtotalDisplay.Text = "$ " + scm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"], CU.locationID).ToString(); //With each item update update totals in database. Return this from the database
                        txtShippingAmount.Text = I.shippingAmount.ToString();
                        lblSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
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
        
        protected void txtShippingAmount_TextChanged(object sender, EventArgs e)
        {
            //call the invoice totals
            List<Invoice> i = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString());
            //change the needed elements
            double shipAmount = 0;
            rdbShipping.Checked = false;
            if (txtShippingAmount.Text != "")
            {
                if (Convert.ToDouble(txtShippingAmount.Text) != 0)
                {
                    shipAmount = Convert.ToDouble(txtShippingAmount.Text);
                    rdbShipping.Checked = true;
                }
            }
            i[0].shippingAmount = shipAmount;
            //send back to update
            IM.UpdateCurrentInvoice(i[0]);
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
                    //int custNum = (Convert.ToInt32(Session["key"].ToString()));
                    //Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //Set name in text box
                    //txtCustomer.Text = c.firstName + " " + c.lastName;
                }
                else
                {
                    grdCustomersSearched.Visible = true;
                    grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnSearchCustomers_Click(object sender, EventArgs e)
        {
            string method = "btnSearchCustomers_Click";
            try
            {
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text);
                grdCustomersSearched.DataBind();
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
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string method = "btnAddCustomer_Click";
            try
            {
                List<Location> L = LM.ReturnLocation(CU.locationID);
                Customer c = new Customer();
                c.firstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text;
                c.lastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text;
                c.primaryAddress = "";
                c.secondaryAddress = "";
                c.primaryPhoneNumber = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text;
                c.secondaryPhoneNumber = "";
                c.emailList = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked;
                c.email = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmail")).Text;
                c.city = "";
                c.province = L[0].provID;
                c.country = L[0].countryID;
                c.postalCode = "";
                //Set name in text box
                ////txtCustomer.Text = c.firstName + " " + c.lastName;
                ////btnCustomerSelect.Text = "Change Customer";
                //Hide stuff
                ////grdCustomersSearched.Visible = false;
                //Set the session key to customer ID
                int custNum = CM.addCustomer(c);
                List<Invoice> i = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString());
                c.customerId = custNum;
                i[0].customer = c;
                IM.UpdateCurrentInvoice(i[0]);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", custNum.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string method = "grdCustomersSearched_PageIndexChanging";
            try
            {
                grdCustomersSearched.PageIndex = e.NewPageIndex;
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text);
                grdCustomersSearched.DataBind();
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
        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "grdCustomersSearched_RowCommand";
            try
            {
                //grabs the command argument for the command pressed 
                //string key = e.CommandArgument.ToString();
                if (e.CommandName == "SwitchCustomer")
                {
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()))[0];
                    Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                    I.customer = C;
                    IM.UpdateCurrentInvoice(I);

                    //if command argument is SwitchCustomer, set the new key
                    //Session["key"] = key;
                    //Hide stuff
                    //grdCustomersSearched.Visible = false;
                    //Get customer name
                    //Customer c = ssm.GetCustomerbyCustomerNumber(Convert.ToInt32(key));
                    //Set name in text box
                    //txtCustomer.Text = c.firstName + " " + c.lastName;

                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("cust", C.customerId.ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
                //btnCustomerSelect.Text = "Change Customer";
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
        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInventorySearch_Click";
            try
            {
                lblInvalidQty.Visible = false;
                //Retrieves the location from session
                //string loc = CU.locationName;
                ////Checks to see if some number or text has been entered into the search field
                //if (!txtSearch.Text.Equals("") && !txtSearch.Text.Equals(null))
                //{
                //    //If something was entered then check to see if it is text
                //    if (!int.TryParse(txtSearch.Text, out skuInt))
                //    {
                //        //If it is then pass into string
                //        skuString = txtSearch.Text;
                //        //use string and location to call query
                //        //Query will return list of items that match the text
                //        invoiceItems = ssm.returnSearchFromAllThreeItemSets(skuString, loc);
                //    }
                //    else
                //    {
                //        //Text entered is a number
                //        skuString = txtSearch.Text;
                //        //this looks for the sku and returns all items that match sku
                //        List<Cart> i = idu.getItemByID(Convert.ToInt32(skuInt), loc);

                //        //Checks to see if at least one item was returned
                //        if (i != null && i.Count >= 1)
                //        {
                //            //Add item to the list
                //            invoiceItems = i;
                //        }
                //    }
                //}
                //Binds list to the grid view
                grdInventorySearched.DataSource = ITM.ReturnInvoiceItemsFromSearchStringForSale(txtSearch.Text); // invoiceItems;
                grdInventorySearched.DataBind();
                //Clears search text box
                txtSearch.Text = "";
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
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            try
            {
                lblInvalidQty.Visible = false;
                ////Gets the index of the selected row
                //int index = e.RowIndex;
                //Stores all the data for each element in the row
                int sku = Convert.ToInt32(grdCartItems.Rows[e.RowIndex].Cells[2].Text);

                IIM.ReturnQTYToInventory(sku, Request.QueryString["inv"].ToString());

                //int quantity = Convert.ToInt32(grdCartItems.Rows[index].Cells[3].Text);
                //int itemType = Convert.ToInt32(((Label)grdCartItems.Rows[index].Cells[8].FindControl("lblTypeID")).Text);
                //Queries database to get the total quantity of the selected sku
                //int remainingQTY = idu.getquantity(sku, itemType);
                //Takes the items currently in the cart puts them into a duplicate
                //variable from the session
                //List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
                //Loops through each item in the duplicate cart
                //foreach (var cart in duplicateCart)
                //{
                //Checks to see if sku in duplicate cart = the selected sku
                //if (cart.sku != sku)
                //{ //when sku doesn't match add sku back into the cart
                //itemsInCart.Add(cart);
                //}
                //else
                //{//When the skus match add the quantity from cart back into stock
                //            idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY + quantity));
                //}
                //}
                //Remove the indexed pointer
                grdCartItems.EditIndex = -1;
                //Store updated cart into session
                //Session["ItemsInCart"] = itemsInCart;
                //bind items back to grid view
                grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                grdCartItems.DataBind();
                //Calculate new subtotal
                ////lblSubtotalDisplay.Text = "$ " + scm.returnSubtotalAmount(itemsInCart, CU.locationID).ToString("#0.00"); //With each item update update totals in database. Return this from the database
                IM.CalculateNewInvoiceTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                lblSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
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
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowEditing";
            try
            {
                lblInvalidQty.Visible = false;
                //Gets the index of the selected row
                //int index = e.NewEditIndex;
                //Stores the quantity of selected row into session
                //int quantity = Convert.ToInt32(grdCartItems.Rows[e.NewEditIndex].Cells[3].Text);
                //Session["originalQTY"] = quantity;
                //binds grid view to the items in cart setting the indexed item up to edit
                //it's available columns
                grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                grdCartItems.EditIndex = e.NewEditIndex;
                grdCartItems.DataBind();
                //Recalculates subtotal
                ////lblSubtotalDisplay.Text = "$ " + scm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"], CU.locationID).ToString("#0.00");
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
                grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                grdCartItems.DataBind();
                //Recalcluate subtotal
                ////lblSubtotalDisplay.Text = "$ " + scm.returnSubtotalAmount((List<Cart>)Session["ItemsInCart"], CU.locationID).ToString("#0.00");
                //Nulls the quantity session
                Session["originalQTY"] = null;
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
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowUpdating";
            try
            {
                lblInvalidQty.Visible = false;
                //Stores all the data for each element in the row
                InvoiceItems newItemInfo = new InvoiceItems();
                newItemInfo.invoiceNum = Convert.ToInt32(Request.QueryString["inv"].ToString().Split('-')[1]);
                newItemInfo.invoiceSubNum = Convert.ToInt32(Request.QueryString["inv"].ToString().Split('-')[2]);
                newItemInfo.sku = Convert.ToInt32(grdCartItems.Rows[e.RowIndex].Cells[2].Text);
                newItemInfo.itemDiscount = Convert.ToDouble(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("txtAmnt")).Text);
                newItemInfo.quantity = Convert.ToInt32(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                newItemInfo.percentage = ((CheckBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked;
                newItemInfo.typeID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[8].FindControl("lblTypeID")).Text);

                if (!IIM.ValidQTY(newItemInfo))
                {
                    //if it is less than 0 then there is not enough in invenmtory to sell
                    lblInvalidQty.Visible = true;
                    //Display error message
                    lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                    lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    IIM.UpdateItemFromCurrentSalesTable(newItemInfo);
                }
                
                //Clears the indexed row
                grdCartItems.EditIndex = -1;
                //Recalculates the new subtotal and Binds cart items to grid view
                UpdateInvoiceTotal();
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

        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                //Retrieves the transaction type from session
                //int tranType = Convert.ToInt32(Session["TranType"]);
                //Check transaction type for Sale
                //if (tranType == 1)
                //{
                //Checks if there are already items in the cart
                //if (Session["ItemsInCart"] != null)
                //{
                //Sets cart session into variable
                ////itemsInCart = (List<Cart>)Session["ItemsInCart"];
                //}
                //Loops through each item in the cart
                ////foreach (var cart in itemsInCart)
                ////{
                //Queries database to get the remaing quantity left in stock
                ////int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                //Updates the quntity in stock adding the quantity in cart
                ////idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY + cart.quantity));
                ////}
                //}

                //////Create procedure to loop through the currentSalesItems and return each to qty.
                IIM.LoopThroughTheItemsToReturnToInventory(Request.QueryString["inv"].ToString());
                IIM.RemoveInitialTotalsForTable(Request.QueryString["inv"].ToString());

                //lblInvalidQty.Visible = false;
                //* *update * *to null any new seesions btnCancelSale_Click;
                //Session["returnedCart"] = null;
                //Session["key"] = null;
                //Session["shipping"] = null;
                //Session["ItemsInCart"] = null;
                //Session["CheckOutTotals"] = null;
                //Session["MethodsofPayment"] = null;
                //Session["Invoice"] = null;
                //Session["searchReturnInvoices"] = null;
                //Session["TranType"] = null;
                //Session["ShippingAmount"] = null;
                //Session["strDate"] = null;
                //Change to Home Page
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
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
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
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
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
        //Once Checkouot Page is updated this can be updated.
        protected void btnProceedToCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProceedToCheckout_Click";
            try
            {
                lblInvalidQty.Visible = false;
                //Validates that what is entered in the shipping amount textbox is a number
                //if (!RadioButton2.Checked)
                //{
                //    Session["shipping"] = false;
                //    Session["ShippingAmount"] = 0;
                //    Response.Redirect("SalesCheckout.aspx", false);
                //}
                //else
                //{
                //    if (txtShippingAmount.Text.isNumber())
                //    {
                //        Session["shipping"] = true;
                //        Session["ShippingAmount"] = txtShippingAmount.Text;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                nameValues.Set("inv", Request.QueryString["inv"].ToString());
                Response.Redirect("SalesCheckout.aspx?" + nameValues, false);
                //    }
                //    else
                //    {
                //        lblShippingWarning.Visible = true;
                //        lblShippingWarning.Text = "Requires a number";
                //    }
                //}
                ////Checks to see if shipping was selected
                //if (!RadioButton2.Checked)
                //{
                //    //Sets sessions based on result
                //    Session["shipping"] = false;
                //    Session["ShippingAmount"] = 0;
                //}
                //else
                //{
                //    //Sets sessions based on result
                //    Session["shipping"] = true;
                //    Session["ShippingAmount"] = txtShippingAmount.Text;
                //}
                ////Changes to Sales Checkout page
                //Response.Redirect("SalesCheckout.aspx", false);
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

        //This code is still a little bulky
        //Doesn't currently add the same item together, would have seperate rows for the same sku if they were added seperatly
        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInventorySearched_RowCommand";
            try
            {
                lblInvalidQty.Visible = false;
                int index = ((GridViewRow)(((LinkButton)e.CommandSource).NamingContainer)).RowIndex;
                int quantity = 1;
                string qty = ((TextBox)grdInventorySearched.Rows[index].Cells[2].FindControl("quantityToAdd")).Text;
                if (qty != "")
                {
                    quantity = Convert.ToInt32(qty);
                }
                int currentQty = Convert.ToInt32(((Label)grdInventorySearched.Rows[index].Cells[2].FindControl("QuantityInOrder")).Text);
                if (quantity > currentQty)
                {
                    lblInvalidQty.Visible = true;
                    lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                    lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    InvoiceItems selectedSku = new InvoiceItems();
                    selectedSku.sku = Convert.ToInt32(grdInventorySearched.Rows[index].Cells[1].Text);
                    if (selectedSku.sku == 100000)
                    {
                        btnRefreshCart.Visible = true;
                        //Trade In Sku to add in SK
                        string redirect = "<script>window.open('TradeINEntry.aspx?inv=" + Request.QueryString["inv"].ToString() + "');</script>";
                        Response.Write(redirect);
                    }
                    else
                    {

                        double discount = 0;
                        string discountAmount = ((TextBox)grdInventorySearched.Rows[index].Cells[5].FindControl("txtAmountDiscount")).Text;
                        if (discountAmount != "")
                        {
                            discount = Convert.ToDouble(discountAmount);
                        }
                        selectedSku.invoiceNum = Convert.ToInt32(Request.QueryString["inv"][1]);
                        selectedSku.invoiceSubNum = Convert.ToInt32(Request.QueryString["inv"][2]);
                        selectedSku.itemDiscount = discount;
                        selectedSku.description = ((Label)grdInventorySearched.Rows[index].Cells[3].FindControl("Description")).Text;
                        selectedSku.itemRefund = 0;
                        selectedSku.price = double.Parse(((Label)grdInventorySearched.Rows[index].Cells[4].FindControl("rollPrice")).Text, NumberStyles.Currency);
                        selectedSku.cost = double.Parse(((Label)grdInventorySearched.Rows[index].Cells[4].FindControl("rollCost")).Text, NumberStyles.Currency);
                        selectedSku.percentage = ((CheckBox)grdInventorySearched.Rows[index].Cells[5].FindControl("chkDiscountPercent")).Checked;
                        selectedSku.isTradeIn = ((CheckBox)grdInventorySearched.Rows[index].Cells[6].FindControl("chkTradeInSearch")).Checked;
                        selectedSku.typeID = Convert.ToInt32(((Label)grdInventorySearched.Rows[index].Cells[7].FindControl("lblTypeIDSearch")).Text);
                        selectedSku.quantity = quantity;

                        //add item to table and remove the added qty from current inventory
                        IIM.InsertItemIntoSalesCart(selectedSku);
                        IIM.RemoveQTYFromInventoryWithSKU(selectedSku.sku, selectedSku.typeID, (currentQty - quantity));
                        //Process an update of totals

                        //refresh the cart grd from table
                        grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                        grdCartItems.DataBind();
                        //Set an empty variable to bind to the searched items grid view so it is empty
                        List<Cart> nullGrid = new List<Cart>();
                        nullGrid = null;
                        grdInventorySearched.DataSource = nullGrid;
                        grdInventorySearched.DataBind();
                        //Recalculate the new subtotal
                        IM.CalculateNewInvoiceTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                        Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        lblSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
                    }
                }

                //Loops through each item in the cart
                //foreach (var cart in itemsInCart)
                //{
                //Checks to see if the cart sku matches selected sku
                //and that the item hasn't already been added
                //        if (cart.sku == itemKey && !bolAdded)
                //{
                //Queries the remaining quantity in stock of the selected sku
                //            int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                //Checks that the new quantity will exceed the remaing qunatity
                //            if ((cart.quantity + 1) > remainingQTY)
                //            {
                //Advises user that not enough qunatity to sell item

                //            }
                //            else
                //            {
                //There is enough in stock to make sale
                //increase quantity in the cart and remove from stock
                //                cart.quantity += 1;
                //                idu.removeQTYfromInventoryWithSKU(cart.sku, cart.typeID, (remainingQTY - 1));
                //            }
                //            bolAdded = true;
                //}
                //}

                //If the itemKey is between or equal to the ranges, do trade in
                //
                //Cart is empty or sku didn't match any items currently in stock
                //else if (itemsInCart.Count == 0 || !bolAdded)
                //{
                //Returns a club, clothing, accessories based on the selected sku
                //    Clubs c = ssm.singleItemLookUp(itemKey);
                //    Clothing cl = ssm.getClothing(itemKey);
                //    Accessories ac = ssm.getAccessory(itemKey);
                //    int itemType = 0;
                //Checks that club looked up an item
                //    if (c.sku != 0)
                //    {
                //if club has value then set type and pass to object
                //        itemType = c.typeID;
                //        o = c as Object;
                //    }
                //Checks that clothing looked up an item
                //    else if (cl.sku != 0)
                //    {
                //if clothing has value then set type and pass to object
                //        itemType = cl.typeID;
                //        o = cl as Object;
                //    }
                //Checks that accessory looked up an item
                //    else if (ac.sku != 0)
                //    {
                //if accessories has value then set type and pass to object
                //        itemType = ac.typeID;
                //        o = ac as Object;
                //    }
                //Retrieves the quantity in stock of the selected item
                //    int remainingQTY = idu.getquantity(itemKey, itemType);
                //Checks to see if there is 0 of the item in stock
                //    if (1 > remainingQTY)
                //    {
                //Displays to user that item has no quantity
                //        MessageBox.ShowMessage("This item has 0 quantity", this);
                //        lblInvalidQty.Visible = true;
                //    }
                //    else
                //    {
                //Query database for item matching object and add to the current cart
                //        itemsInCart.Add(idu.addingToCart(o));
                //Remove the quantity from amount in stock
                //        idu.removeQTYfromInventoryWithSKU(itemKey, itemType, (remainingQTY - 1));
                //    }
                //}

                //Set items in cart into Session
                //    Session["ItemsInCart"] = itemsInCart;
                //Bind items in cart to grid view
                //    
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
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void UpdateInvoiceTotal()
        {
            grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
            grdCartItems.DataBind();
            IM.CalculateNewInvoiceTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
            Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
            lblSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
        }

        protected void btnRefreshCart_Click(object sender, EventArgs e)
        {
            btnRefreshCart.Visible = false;
            UpdateInvoiceTotal();
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            grdInventorySearched.DataSource = null;
            grdInventorySearched.DataBind();
        }
    }
}