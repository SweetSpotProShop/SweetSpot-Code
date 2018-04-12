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
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();
        InvoiceManager IM = new InvoiceManager();
        InvoiceItemsManager IIM = new InvoiceItemsManager();
        //SalesCalculationManager SCM = new SalesCalculationManager();

        //public string skuString;
        //public int skuInt;
        //public int invNum;
        //SweetShopManager ssm = new SweetShopManager();
        //ItemDataUtilities idu = new ItemDataUtilities();
        //List<Cart> invoiceItems = new List<Cart>();
        //List<Cart> itemsInCart = new List<Cart>();
        //List<Cart> returnedCart = new List<Cart>();
        //List<Cart> temp = new List<Cart>();
        //LocationManager lm = new LocationManager();
        //Object o = new Object();


        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCart.aspx";
            try
            {
                lblInvalidQty.Visible = false;
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
                        //Retrieves transaction type from Session
                        //int tranType = Convert.ToInt32(Session["TranType"]);
                        //if (tranType == 2)
                        //{
                        //Retrieves Invoice from Session
                        Invoice I = IM.ReturnInvoiceForReturns(Request.QueryString["inv"].ToString());

                        if (!IM.ReturnBolInvoiceExists(Request.QueryString["inv"].ToString()))
                        {
                            I.invoiceSub = Convert.ToInt32((Request.QueryString["inv"].ToString()).Split('-')[2]);
                            I.subTotal = 0;
                            I.discountAmount = 0;
                            I.tradeinAmount = 0;
                            I.balanceDue = 0;
                            I.location = CU.location;
                            I.employee = CU.emp;
                            IM.CreateInitialTotalsForTable(I);
                        }

                        //Checks to see if the returned cart is empty
                        //if (Session["returnedCart"] != null)
                        //{
                        //    //When not empty passes in 2 sessions
                        //    itemsInCart = ReturnInvoiceItemsFromProcessedSalesForReturn(Request.QueryString["inv"].ToString()[0]);
                        //    returnedCart = (List<Cart>)Session["returnedCart"];
                        //    //binds returned cart to the grid view
                        //    grdReturningItems.DataSource = returnedCart;
                        //    grdReturningItems.DataBind();
                        //    //displays subtotal based on the returned cart
                        //    //lblReturnSubtotalDisplay.Text = "$ " + cm.returnRefundSubtotalAmount(returnedCart).ToString("#0.00");
                        //}
                        //else
                        //{
                        //    //When session is empty gathers a cart from the stored invoice number
                        //    temp = ssm.returningItems(rInvoice.invoiceNum, rInvoice.invoiceSub);
                        //    foreach (var item in temp)
                        //    {
                        //        //Checks each item to make sure it is not a trade in
                        //        if (item.typeID != 0)
                        //        {
                        //            itemsInCart.Add(item);
                        //        }
                        //    }
                        //}
                        //populates current customer info
                        lblCustomerDisplay.Text = I.customer.firstName.ToString() + " " + I.customer.lastName.ToString();
                        lblInvoiceNumberDisplay.Text = Request.QueryString["inv"].ToString();
                        lblDateDisplay.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        //binds items in cart to gridview
                        grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(Request.QueryString["inv"].ToString());
                        grdInvoicedItems.DataBind();

                        grdReturningItems.DataSource = IIM.ReturnItemsInTheReturnCart(Request.QueryString["inv"].ToString());
                        grdReturningItems.DataBind();
                        //}

                        IM.CalculateNewInvoiceReturnTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                        I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        lblReturnSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");

                        //lblReturnSubtotalDisplay.Text = "$ " + SCM.returnRefundSubtotalAmount(Request.QueryString["inv"].ToString()).ToString("#0.00");
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
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                lblInvalidQty.Visible = false;
                IIM.LoopThroughTheItemsToReturnToInventory(Request.QueryString["inv"].ToString());
                IIM.RemoveInitialTotalsForTable(Request.QueryString["inv"].ToString());

                //int tranType = Convert.ToInt32(Session["TranType"]);
                //if (tranType == 2)
                //{
                //    if (Session["returnedCart"] != null)
                //    {
                //        itemsInCart = (List<Cart>)Session["returnedCart"];
                //    }
                //    foreach (var cart in itemsInCart)
                //    {
                //        int remainingQTY = idu.getquantity(cart.sku, cart.typeID);
                //        idu.updateQuantity(cart.sku, cart.typeID, (remainingQTY - cart.quantity));
                //    }
                //}
                ////* *update * *to null any new seesions btnCancelReturn_Click;
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
        protected void btnProceedToReturnCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProceedToCheckout_Click";
            try
            {
                lblInvalidQty.Visible = false;
                //Changes page to the returns checkout page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inv", Request.QueryString["inv"].ToString());
                Response.Redirect("ReturnsCheckout.aspx?" + nameValues, false);
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
        protected void grdInvoicedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoicedItems_RowDeleting";
            try
            {
                lblInvalidQty.Visible = false;
                //Stores the info about the item in that index
                InvoiceItems selectedSku = IIM.ReturnInvoiceItemForReturnProcess(Convert.ToInt32(grdInvoicedItems.Rows[e.RowIndex].Cells[1].Text), Request.QueryString["inv"].ToString());
                int currentQTY = selectedSku.quantity;
                string quantityForReturn = ((TextBox)grdInvoicedItems.Rows[e.RowIndex].Cells[2].FindControl("quantityToReturn")).Text;
                int quantitySold = Convert.ToInt32(((Label)grdInvoicedItems.Rows[e.RowIndex].Cells[2].FindControl("quantitySold")).Text);
                int returnQuantity = 1;
                if (quantityForReturn != "")
                {
                    returnQuantity = Convert.ToInt32(quantityForReturn);
                }
                if (returnQuantity > quantitySold)
                {
                    lblInvalidQty.Visible = true;
                    lblInvalidQty.Text = "Quantity Exceeds the Quantity Sold";
                    lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    double returnAmount = Convert.ToDouble(((TextBox)grdInvoicedItems.Rows[e.RowIndex].Cells[7].FindControl("txtReturnAmount")).Text);
                    IIM.RemoveQTYFromInventoryWithSKU(selectedSku.sku, selectedSku.typeID, (currentQTY + returnQuantity));
                    selectedSku.invoiceSubNum = Convert.ToInt32(Request.QueryString["inv"].Split('-')[2].ToString());
                    selectedSku.quantity = returnQuantity;
                    selectedSku.itemRefund = returnAmount;

                    IIM.InsertItemIntoSalesCart(selectedSku);

                    //Checks if there are already items in the cart marked for return
                    //if (Session["returnedCart"] != null)
                    //{
                    //    //if there are then the session is set to the duplicate of the return cart
                    //    duplicateReturnedCart = (List<Cart>)Session["returnedCart"];
                    //    bolCart = false;
                    //}
                    ////Loops through each cart item that could be returned
                    //foreach (var cart in duplicateCart)
                    //{
                    //    //Matches the selected sku with cart item that could be returned
                    //    if (cart.sku == sku)
                    //    {
                    //        //Checks if there are more than 1 of that item that is avail for return
                    //        if (cart.quantity > 1)
                    //        {
                    //            //If there are then reduce the quantity of that item by 1
                    //            remainingQTY = quantity - 1;
                    //            cart.quantity = remainingQTY;
                    //            //Add it into the cart of item that could be returned at
                    //            //this lower quantity
                    //            //itemsInCart.Add(cart);
                    //        }
                    //        //Checks if there are already items in the cart marked for return
                    //        if (!bolCart)
                    //        {
                    //            //When there are need to loop through that cart to see if
                    //            //the item now being returned already has a quantity in
                    //            //the marked for return cart
                    //            foreach (var retCart in duplicateReturnedCart)
                    //            {
                    //                //checks to see if the skus match
                    //                if (retCart.sku == cart.sku)
                    //                {
                    //                    //When skus match increase the quantity for that sku
                    //                    //in the marked for return cart
                    //                    returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.itemDiscount, retCart.percentage, retCart.returnAmount, retCart.isTradeIn, retCart.typeID);
                    //                    //Add that item back into stock so that it could be sold again
                    //                    idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                    //                    //Trigger that the selected sku has now been added to marked return cart
                    //                    bolAdded = true;
                    //                }
                    //                else
                    //                {
                    //                    //If the sku doesn't match then item we checked against
                    //                    //needs to be added back into the cart
                    //                    returnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity, retCart.price, retCart.cost, retCart.itemDiscount, retCart.percentage, retCart.returnAmount, retCart.isTradeIn, retCart.typeID);
                    //                }
                    //                //This completes the add of the item from the if statement
                    //                //returnedCart.Add(returnedItem);
                    //            }
                    //            //Triggers if the selected sku didn't match any sku in the marked
                    //            //for return cart
                    //            if (!bolAdded)
                    //            {
                    //                int multi;
                    //                //checks if there was a percentage discount or dollar discount
                    //                //on the sku
                    //                if (cart.percentage) { multi = 1; } else { multi = -1; }
                    //                //Adds sku in the cart of items marked for return
                    //                returnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.itemDiscount, cart.percentage, -1 * returnAmount, cart.isTradeIn, cart.typeID);
                    //                //Adds the new quantity back into stock
                    //                idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                    //                //returnedCart.Add(returnedItem);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //The marked for return cart was empty no checks needed on item just add
                    //            int multi;
                    //            //checks if there was a percentage discount or dollar discount
                    //            //on the sku
                    //            if (cart.percentage) { multi = 1; } else { multi = -1; }
                    //            //Adds sku in the cart of items marked for return
                    //            returnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.itemDiscount, cart.percentage, -1 * returnAmount, cart.isTradeIn, cart.typeID);
                    //            //Adds the new quantity back into stock
                    //            idu.removeQTYfromInventoryWithSKU(returnedItem.sku, returnedItem.typeID, inStockQTY + 1);
                    //            //returnedCart.Add(returnedItem);
                    //        }
                    //    }
                    //    else if (cart.sku != sku)
                    //    {
                    //        //sku was not the selected sku add it back into the cart of items
                    //        //available for return
                    //        //itemsInCart.Add(cart);
                    //    }
                    //}

                    //deselect the indexed item
                    grdInvoicedItems.EditIndex = -1;
                    //store items available for return in session
                    grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(Request.QueryString["inv"].ToString());
                    grdInvoicedItems.DataBind();

                    grdReturningItems.DataSource = IIM.ReturnItemsInTheReturnCart(Request.QueryString["inv"].ToString());
                    grdReturningItems.DataBind();

                    //recalculate the return total
                    IM.CalculateNewInvoiceReturnTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                    Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                    lblReturnSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");

                    //lblReturnSubtotalDisplay.Text = "$ " + SCM.returnRefundSubtotalAmount(Request.QueryString["inv"].ToString()).ToString("#0.00");
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
        protected void grdReturningItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdReturningItems_RowDeleting";
            try
            {
                lblInvalidQty.Visible = false;
                //Gathers index from selected line item
                int index = e.RowIndex;
                //Stores the info about the item in that index
                int sku = Convert.ToInt32(grdReturningItems.Rows[index].Cells[1].Text);
                InvoiceItems selectedSku = IIM.ReturnSkuFromCurrentSalesUsingSKU(sku, Request.QueryString["inv"].ToString());
                
                //add item to table and remove the added qty from current inventory
                IIM.DoNotReturnTheItemOnReturn(selectedSku);
                IIM.RemoveQTYFromInventoryWithSKU(selectedSku.sku, selectedSku.typeID, (IIM.ReturnCurrentQuantityOfItem(selectedSku) - selectedSku.quantity));



                //int quantity = Convert.ToInt32(grdReturningItems.Rows[index].Cells[2].Text);
                //int itemType = Convert.ToInt32(((Label)grdReturningItems.Rows[index].Cells[6].FindControl("lblReturnedTypeID")).Text);
                ////Calls current quantity of the sku that is in stock
                //int inStockQTY = idu.getquantity(sku, itemType);
                //int remainingQTY = 0;
                ////Transfers Session of items that have been marked for return
                //List<Cart> duplicateCart = (List<Cart>)Session["returnedCart"];
                //Cart cancelReturnedItem = new Cart();
                ////Transfers Session of items that could be returned into a duplicate cart
                //List<Cart> duplicateReturnedCart = (List<Cart>)Session["ItemsInCart"];
                //bool bolAdded = false;
                //bool bolCart = true;
                ////Checks if there are already items in the cart that could be returned
                //if (duplicateReturnedCart.Count > 0)
                //{
                //    bolCart = false;
                //}
                ////Loops through each cart item that has been marked for return
                //foreach (var cart in duplicateCart)
                //{
                //    //Matches the selected sku with cart item that has been marked for return
                //    if (cart.sku == sku)
                //    {
                //        //Checks if there are more than 1 of that item that is marked for return
                //        if (quantity > 1)
                //        {
                //            //If there are then reduce the quantity of that item by 1
                //            remainingQTY = quantity - 1;
                //            cart.quantity = remainingQTY;
                //            //Add it into the cart of item that are marked for return
                //            //at this lower quantity
                //            //returnedCart.Add(cart);
                //        }
                //        //Checks if there are already items in the returnable items cart
                //        if (!bolCart)
                //        {
                //            //When there are need to loop through that cart to see if
                //            //the item now being updated already has a quantity in
                //            //the available for return cart
                //            foreach (var retCart in duplicateReturnedCart)
                //            {
                //                //checks to see if the skus match
                //                if (retCart.sku == cart.sku)
                //                {
                //                    //When skus match increase the quantity for that sku
                //                    //in the returnable items cart
                //                    cancelReturnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity + 1, retCart.price, retCart.cost, retCart.itemDiscount, retCart.percentage, 0, retCart.isTradeIn, retCart.typeID);
                //                    //Remove that item from stock so that it can not be sold again
                //                    idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                //                    //Trigger that the selected sku has now been added into the returnable items cart
                //                    bolAdded = true;
                //                }
                //                else
                //                {
                //                    //If the sku doesn't match then item we checked against
                //                    //needs to be added back into the marked for return cart
                //                    cancelReturnedItem = new Cart(retCart.sku, retCart.description, retCart.quantity, retCart.price, retCart.cost, retCart.itemDiscount, retCart.percentage, 0, retCart.isTradeIn, retCart.typeID);
                //                }
                //                //This completes the add of the item from the if statement
                //                //itemsInCart.Add(cancelReturnedItem);
                //            }
                //            //Triggers if the selected sku didn't match any sku in the returnable
                //            //items cart
                //            if (!bolAdded)
                //            {
                //                int multi;
                //                //checks if there was a percentage discount or dollar discount
                //                //on the sku
                //                if (cart.percentage) { multi = 1; } else { multi = -1; }
                //                //Adds sku in the returnable items cart
                //                cancelReturnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.itemDiscount, cart.percentage, 0, cart.isTradeIn, cart.typeID);
                //                //Removes the new quantity from stock
                //                idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                //                //itemsInCart.Add(cancelReturnedItem);
                //            }
                //        }
                //        //The returnable items cart was empty no checks needed on item just add
                //        else
                //        {
                //            int multi;
                //            //checks if there was a percentage discount or dollar discount
                //            //on the sku
                //            if (cart.percentage) { multi = 1; } else { multi = -1; }
                //            //Adds sku in the returnable items cart
                //            cancelReturnedItem = new Cart(cart.sku, cart.description, 1, -1 * cart.price, cart.cost, multi * cart.itemDiscount, cart.percentage, 0, cart.isTradeIn, cart.typeID);
                //            //Removes the new quantity from stock
                //            idu.removeQTYfromInventoryWithSKU(cancelReturnedItem.sku, cancelReturnedItem.typeID, inStockQTY - 1);
                //            //itemsInCart.Add(cancelReturnedItem);
                //        }
                //    }
                //    else if (cart.sku != sku)
                //    {
                //        //sku was not the selected sku add it back into the marked for
                //        //return cart
                //        //returnedCart.Add(cart);
                //    }
                //}
                //deselect the indexed item
                grdReturningItems.EditIndex = -1;
                //Check if the marked for returns cart has any items in it
                grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(Request.QueryString["inv"].ToString());
                grdInvoicedItems.DataBind();

                grdReturningItems.DataSource = IIM.ReturnItemsInTheReturnCart(Request.QueryString["inv"].ToString());
                grdReturningItems.DataBind();

                //recalculate the return total
                IM.CalculateNewInvoiceReturnTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                lblReturnSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");

                //lblReturnSubtotalDisplay.Text = "$ " + SCM.returnRefundSubtotalAmount(Request.QueryString["inv"].ToString()).ToString("#0.00");
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
    }
}