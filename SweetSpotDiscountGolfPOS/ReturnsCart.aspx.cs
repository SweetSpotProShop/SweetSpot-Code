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

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReturnsCart : System.Web.UI.Page
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
        Object o = new Object();
        CurrentUser cu;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCart.aspx";
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
                    //Retrieves transaction type from Session
                    int tranType = Convert.ToInt32(Session["TranType"]);
                    if (tranType == 2)
                    {
                        //Retrieves Invoice from Session
                        Invoice rInvoice = (Invoice)Session["searchReturnInvoices"];
                        //Checks to see if the returned cart is empty
                        if (Session["returnedCart"] != null)
                        {
                            //When not empty passes in 2 sessions
                            itemsInCart = (List<Cart>)Session["ItemsInCart"];
                            returnedCart = (List<Cart>)Session["returnedCart"];
                            //binds returned cart to the grid view
                            grdReturningItems.DataSource = returnedCart;
                            grdReturningItems.DataBind();
                            //displays subtotal based on the returned cart
                            lblReturnSubtotalDisplay.Text = "$ " + ssm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
                        }
                        else
                        {
                            //Whjen session is empty gathers a cart from the stored invoice number
                            temp = ssm.returningItems(rInvoice.invoiceNum, rInvoice.invoiceSub);
                            foreach (var item in temp)
                            {
                                //Checks each item to make sure it is not a trade in
                                if (item.typeID != 0)
                                {
                                    itemsInCart.Add(item);
                                }
                            }
                        }
                        //populates current customer info
                        lblCustomerDisplay.Text = rInvoice.customerName.ToString();
                        lblInvoiceNumberDisplay.Text = cu.locationName + "-" + rInvoice.invoiceNum.ToString() + "-" + idu.getNextInvoiceSubNum(rInvoice.invoiceNum).ToString();
                        Session["Invoice"] = lblInvoiceNumberDisplay.Text;
                        lblDateDisplay.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        Session["ItemsInCart"] = itemsInCart;
                        //binds items in cart to gridview
                        grdInvoicedItems.DataSource = itemsInCart;
                        grdInvoicedItems.DataBind();
                    }
                }
                //Sets Date session
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
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                int tranType = Convert.ToInt32(Session["TranType"]);
                if (tranType == 2)
                {
                    if (Session["returnedCart"] != null)
                    {
                        itemsInCart = (List<Cart>)Session["returnedCart"];
                    }
                    foreach (var cart in itemsInCart)
                    {
                        int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                        idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY - cart.quantity));
                    }
                }
                //* *update * *to null any new seesions btnCancelReturn_Click;
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
        protected void btnProceedToReturnCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProceedToCheckout_Click";
            try
            {
                //Changes page to the returns checkout page
                Server.Transfer("ReturnsCheckout.aspx", false);
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
        protected void grdInvoicedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoicedItems_RowDeleting";
            try
            {
                //Gathers index from selected line item
                int index = e.RowIndex;
                //Stores the info about the item in that index
                int sku = Convert.ToInt32(grdInvoicedItems.Rows[index].Cells[1].Text);
                int quantity = Convert.ToInt32(grdInvoicedItems.Rows[index].Cells[2].Text);
                int itemType = Convert.ToInt32(((Label)grdInvoicedItems.Rows[index].Cells[6].FindControl("lblReturnTypeID")).Text);
                double returnAmount = Convert.ToDouble(((TextBox)grdInvoicedItems.Rows[index].Cells[7].FindControl("txtReturnAmount")).Text);
                //Calls current quantity of the sku that is in stock
                int inStockQTY = idu.getquantity(sku, itemType);
                int remainingQTY = 0;
                //Transfers Session of items that could be returned into a duplicate cart
                List<Cart> duplicateCart = (List<Cart>)Session["ItemsInCart"];
                Cart returnedItem = new Cart();
                //Create a second duplicate cart for those items that have already been
                //marked for return
                List<Cart> duplicateReturnedCart = new List<Cart>();
                bool bolAdded = false;
                bool bolCart = true;
                //Checks if there are already items in the cart marked for return
                if (Session["returnedCart"] != null)
                {
                    //if there are then the session is set to the duplicate of the return cart
                    duplicateReturnedCart = (List<Cart>)Session["returnedCart"];
                    bolCart = false;
                }
                //Loops through each cart item that could be returned
                foreach (var cart in duplicateCart)
                {
                    //Matches the selected sku with cart item that could be returned
                    if (cart.sku == sku)
                    {
                        //Checks if there are more than 1 of that item that is avail for return
                        if (cart.quantity > 1)
                        {
                            //If there are then reduce the quantity of that item by 1
                            remainingQTY = quantity - 1;
                            cart.quantity = remainingQTY;
                            //Add it into the cart of item that could be returned at
                            //this lower quantity
                            itemsInCart.Add(cart);
                        }
                        //Checks if there are already items in the cart marked for return
                        if (!bolCart)
                        {
                            //When there are need to loop through that cart to see if
                            //the item now being returned already has a quantity in
                            //the marked for return cart
                            foreach (var retCart in duplicateReturnedCart)
                            {
                                //checks to see if the skus match
                                if (retCart.sku == cart.sku)
                                {
                                    //When skus match increase the quantity for that sku
                                    //in the marked for return cart
                                    returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.returnAmount, retCart.tradeIn, retCart.typeID);
                                    //Add that item back into stock so that it could be sold again
                                    idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                                    //Trigger that the selected sku has now been added to marked return cart
                                    bolAdded = true;
                                }
                                else
                                {
                                    //If the sku doesn't match then item we checked against
                                    //needs to be added back into the cart
                                    returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.returnAmount, retCart.tradeIn, retCart.typeID);
                                }
                                //This completes the add of the item from the if statement
                                returnedCart.Add(returnedItem);
                            }
                            //Triggers if the selected sku didn't match any sku in the marked
                            //for return cart
                            if (!bolAdded)
                            {
                                int multi;
                                //checks if there was a percentage discount or dollar discount
                                //on the sku
                                if (cart.percentage) { multi = 1; } else { multi = -1; }
                                //Adds sku in the cart of items marked for return
                                returnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, -1 * returnAmount, cart.tradeIn, cart.typeID);
                                //Adds the new quantity back into stock
                                idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                                returnedCart.Add(returnedItem);
                            }
                        }
                        else
                        {
                            //The marked for return cart was empty no checks needed on item just add
                            int multi;
                            //checks if there was a percentage discount or dollar discount
                            //on the sku
                            if (cart.percentage) { multi = 1; } else { multi = -1; }
                            //Adds sku in the cart of items marked for return
                            returnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, -1 * returnAmount, cart.tradeIn, cart.typeID);
                            //Adds the new quantity back into stock
                            idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                            returnedCart.Add(returnedItem);
                        }
                    }
                    else if (cart.sku != sku)
                    {
                        //sku was not the selected sku add it back into the cart of items
                        //available for return
                        itemsInCart.Add(cart);
                    }
                }
                //deselect the indexed item
                grdInvoicedItems.EditIndex = -1;
                //store items available for return in session
                Session["ItemsInCart"] = itemsInCart;
                //bind items to grid view
                grdInvoicedItems.DataSource = itemsInCart;
                grdInvoicedItems.DataBind();
                //Check if the marked for returns cart has any items in it
                if (returnedCart.Count > 0)
                {
                    //If yes then store in session
                    Session["returnedCart"] = returnedCart;
                }
                else
                {
                    //If no then null the session
                    Session["returnedCart"] = null;
                }
                //bind marked for return items to grid view
                grdReturningItems.DataSource = returnedCart;
                grdReturningItems.DataBind();
                //recalculate the return total
                lblReturnSubtotalDisplay.Text = "$ " + ssm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
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
        protected void grdReturningItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdReturningItems_RowDeleting";
            try
            {
                //Gathers index from selected line item
                int index = e.RowIndex;
                //Stores the info about the item in that index
                int sku = Convert.ToInt32(grdReturningItems.Rows[index].Cells[1].Text);
                int quantity = Convert.ToInt32(grdReturningItems.Rows[index].Cells[2].Text);
                int itemType = Convert.ToInt32(((Label)grdReturningItems.Rows[index].Cells[6].FindControl("lblReturnedTypeID")).Text);
                //Calls current quantity of the sku that is in stock
                int inStockQTY = idu.getquantity(sku, itemType);
                int remainingQTY = 0;
                //Transfers Session of items that have been marked for return
                List<Cart> duplicateCart = (List<Cart>)Session["returnedCart"];
                Cart cancelReturnedItem = new Cart();
                //Transfers Session of items that could be returned into a duplicate cart
                List<Cart> duplicateReturnedCart = (List<Cart>)Session["ItemsInCart"];
                bool bolAdded = false;
                bool bolCart = true;
                //Checks if there are already items in the cart that could be returned
                if (duplicateReturnedCart.Count > 0)
                {
                    bolCart = false;
                }
                //Loops through each cart item that has been marked for return
                foreach (var cart in duplicateCart)
                {
                    //Matches the selected sku with cart item that has been marked for return
                    if (cart.sku == sku)
                    {
                        //Checks if there are more than 1 of that item that is marked for return
                        if (quantity > 1)
                        {
                            //If there are then reduce the quantity of that item by 1
                            remainingQTY = quantity - 1;
                            cart.quantity = remainingQTY;
                            //Add it into the cart of item that are marked for return
                            //at this lower quantity
                            returnedCart.Add(cart);
                        }
                        //Checks if there are already items in the returnable items cart
                        if (!bolCart)
                        {
                            //When there are need to loop through that cart to see if
                            //the item now being updated already has a quantity in
                            //the available for return cart
                            foreach (var retCart in duplicateReturnedCart)
                            {
                                //checks to see if the skus match
                                if (retCart.sku == cart.sku)
                                {
                                    //When skus match increase the quantity for that sku
                                    //in the returnable items cart
                                    cancelReturnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.tradeIn, retCart.typeID);
                                    //Remove that item from stock so that it can not be sold again
                                    idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                                    //Trigger that the selected sku has now been added into the returnable items cart
                                    bolAdded = true;
                                }
                                else
                                {
                                    //If the sku doesn't match then item we checked against
                                    //needs to be added back into the marked for return cart
                                    cancelReturnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity, retCart.price, retCart.cost, retCart.discount, retCart.percentage, retCart.tradeIn, retCart.typeID);
                                }
                                //This completes the add of the item from the if statement
                                itemsInCart.Add(cancelReturnedItem);
                            }
                            //Triggers if the selected sku didn't match any sku in the returnable
                            //items cart
                            if (!bolAdded)
                            {
                                int multi;
                                //checks if there was a percentage discount or dollar discount
                                //on the sku
                                if (cart.percentage) { multi = 1; } else { multi = -1; }
                                //Adds sku in the returnable items cart
                                cancelReturnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                                //Removes the new quantity from stock
                                idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                                itemsInCart.Add(cancelReturnedItem);
                            }
                        }
                        //The returnable items cart was empty no checks needed on item just add
                        else
                        {
                            int multi;
                            //checks if there was a percentage discount or dollar discount
                            //on the sku
                            if (cart.percentage) { multi = 1; } else { multi = -1; }
                            //Adds sku in the returnable items cart
                            cancelReturnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.discount, cart.percentage, cart.tradeIn, cart.typeID);
                            //Removes the new quantity from stock
                            idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                            itemsInCart.Add(cancelReturnedItem);
                        }
                    }
                    else if (cart.sku != sku)
                    {
                        //sku was not the selected sku add it back into the marked for
                        //return cart
                        returnedCart.Add(cart);
                    }
                }
                //deselect the indexed item
                grdReturningItems.EditIndex = -1;
                //Check if the marked for returns cart has any items in it
                if (returnedCart.Count > 0)
                {
                    //If yes then store in session
                    Session["returnedCart"] = returnedCart;
                }
                else
                {
                    //If no then null the session
                    Session["returnedCart"] = null;
                }
                //bind marked for return items to grid view
                grdReturningItems.DataSource = returnedCart;
                grdReturningItems.DataBind();
                //store items available for return in session
                Session["ItemsInCart"] = itemsInCart;
                //bind items to grid view
                grdInvoicedItems.DataSource = itemsInCart;
                grdInvoicedItems.DataBind();
                //recalculate the return total
                lblReturnSubtotalDisplay.Text = "$ " + ssm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
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