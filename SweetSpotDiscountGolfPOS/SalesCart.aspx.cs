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
                        txtSearch.Focus();
                        Customer C = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["cust"].ToString()))[0];
                        //Set name in text box
                        txtCustomer.Text = C.firstName + " " + C.lastName;
                        lblDateDisplay.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        lblInvoiceNumberDisplay.Text = Request.QueryString["inv"].ToString();
                        Invoice I = new Invoice(Convert.ToInt32(Request.QueryString["inv"].ToString().Split('-')[1]), Convert.ToInt32(Request.QueryString["inv"].ToString().Split('-')[2]), DateTime.Now, DateTime.Now, C, CU.emp, CU.location, 0, 0, 0, 0, 0, 0, 0, 1, "");
                        if (!IM.ReturnBolInvoiceExists(Request.QueryString["inv"].ToString()))
                        {
                            IM.CreateInitialTotalsForTable(I);
                        }
                        //change to gather the items from table
                        grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                        grdCartItems.DataBind();
                        I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        
        protected void txtShippingAmount_TextChanged(object sender, EventArgs e)
        {
            string method = "txtShippingAmount_TextChanged";
            try
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
            //Exception catch
            catch(ThreadAbortException tae) { }
            catch(Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                Location L = LM.ReturnLocation(CU.location.locationID)[0];
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
                c.province = L.provID;
                c.country = L.countryID;
                c.postalCode = "";
                int custNum = CM.addCustomer(c);
                List<Invoice> i = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString());
                c.customerId = custNum;
                i[0].customer = c;
                IM.UpdateCurrentInvoice(i[0]);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inv", Request.QueryString["inv"].ToString());
                nameValues.Set("cust", custNum.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text);
                grdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (e.CommandName == "SwitchCustomer")
                {
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()))[0];
                    Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                    I.customer = C;
                    IM.UpdateCurrentInvoice(I);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("cust", C.customerId.ToString());
                    nameValues.Set("receipt", Request.QueryString["inv"].ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                if (txtSearch.Text != "")
                {
                    if (txtSearch.Text.Equals("100000"))
                    {
                        grdInventorySearched.DataSource = ITM.ReturnTradeInSku();
                    }
                    else
                    {
                        grdInventorySearched.DataSource = ITM.ReturnInvoiceItemsFromSearchStringForSale(txtSearch.Text);
                    }
                    lblInvalidQty.Visible = false;
                    //Binds list to the grid view

                    grdInventorySearched.DataBind();
                    //Clears search text box
                    txtSearch.Text = "";
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                int sku = Convert.ToInt32(grdCartItems.Rows[e.RowIndex].Cells[2].Text);
                IIM.ReturnQTYToInventory(sku, Request.QueryString["inv"].ToString());
                //Remove the indexed pointer
                grdCartItems.EditIndex = -1;
                //bind items back to grid view
                grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                grdCartItems.DataBind();
                //Calculate new subtotal
                IM.CalculateNewInvoiceTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                lblSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                grdCartItems.EditIndex = e.NewEditIndex;
                grdCartItems.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                IIM.LoopThroughTheItemsToReturnToInventory(Request.QueryString["inv"].ToString());
                IIM.RemoveInitialTotalsForTable(Request.QueryString["inv"].ToString());
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                UpdateInvoiceTotal();
                lblInvalidQty.Visible = false;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                nameValues.Set("inv", Request.QueryString["inv"].ToString());
                Response.Redirect("SalesCheckout.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int quantity = 1;
                string qty = ((TextBox)grdInventorySearched.Rows[index].Cells[2].FindControl("quantityToAdd")).Text;
                if (qty != "")
                {
                    if (int.TryParse(qty, out quantity))
                    {
                        quantity = Convert.ToInt32(qty);
                    }
                }
                int currentQty = Convert.ToInt32(((Label)grdInventorySearched.Rows[index].Cells[2].FindControl("QuantityInOrder")).Text);
                if (quantity > currentQty || quantity < 1)
                {
                    lblInvalidQty.Visible = true;
                }
                else
                {
                    InvoiceItems selectedSku = new InvoiceItems();
                    selectedSku.sku = Convert.ToInt32(grdInventorySearched.Rows[index].Cells[1].Text);
                    selectedSku.invoiceNum = Convert.ToInt32((Request.QueryString["inv"].ToString()).Split('-')[1]);
                    selectedSku.invoiceSubNum = Convert.ToInt32((Request.QueryString["inv"].ToString()).Split('-')[2]);
                    if (!IIM.ItemAlreadyInCart(selectedSku))
                    {
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
                                if (double.TryParse(discountAmount, out discount))
                                {
                                    discount = Convert.ToDouble(discountAmount);
                                }
                            }
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
                    else
                    {
                        MessageBox.ShowMessage("Item is already in the cart. Please update item in "
                         + "cart or process a second sale.", this);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void UpdateInvoiceTotal()
        {
            string method = "UpdateInvoiceTotal";
            try
            {
                grdCartItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["inv"].ToString());
                grdCartItems.DataBind();
                IM.CalculateNewInvoiceTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                lblSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
            }
            //Exception catch
            catch(ThreadAbortException tae) { }
            catch(Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnRefreshCart_Click(object sender, EventArgs e)
        {
            string method = "btnRefreshCart_Click";
            try
            {
                btnRefreshCart.Visible = false;
                UpdateInvoiceTotal();
            }
            //Exception catch
            catch(ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            string method = "btnClearSearch_Click";
            try
            {
                grdInventorySearched.DataSource = null;
                grdInventorySearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}