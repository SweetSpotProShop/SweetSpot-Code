﻿using System;
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
        //private static Invoice invoice;
        CurrentUser CU;

        //Still need to account for a duplicate item being added
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCart.aspx";
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
                    lblInvalidQty.Visible = false;
                    if (!Page.IsPostBack)
                    {
                        txtSearch.Focus();
                        //Set name in text box
                        Invoice invoice = new Invoice();
                        if(Request.QueryString["invoice"].ToString() == "-10")
                        {
                            Invoice newInvoice = new Invoice();
                            newInvoice.varInvoiceNumber = IM.ReturnNextInvoiceNumberForNewInvoice(CU, objPageDetails);
                            newInvoice.intInvoiceSubNumber = 1;
                            newInvoice.customer = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0];
                            newInvoice.employee = CU.employee;
                            newInvoice.location = CU.location;
                            newInvoice.fltGovernmentTaxAmount = 0;
                            newInvoice.fltProvincialTaxAmount = 0;
                            newInvoice.fltLiquorTaxAmount = 0;
                            newInvoice.bitChargeGST = true;
                            newInvoice.bitChargePST = true;
                            newInvoice.bitChargeLCT = true;
                            newInvoice.intTransactionTypeID = 1;
                            newInvoice.varAdditionalInformation = "";
                            invoice = IM.CreateInitialTotalsForTable(newInvoice, objPageDetails)[0];

                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                            nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                            Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                        }
                        else
                        {
                            invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                        }

                        txtCustomer.Text = invoice.customer.varFirstName + " " + invoice.customer.varLastName;
                        lblDateDisplay.Text = DateTime.Today.ToString("dd/MMM/yy");
                        lblInvoiceNumberDisplay.Text = invoice.varInvoiceNumber + "-" + invoice.intInvoiceSubNumber;
                        //change to gather the items from table
                        grdCartItems.DataSource = invoice.invoiceItems;
                        grdCartItems.DataBind();
                        txtShippingAmount.Text = invoice.fltShippingCharges.ToString();
                        lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("C");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }        
        protected void txtShippingAmount_TextChanged(object sender, EventArgs e)
        {
            string method = "txtShippingAmount_TextChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //call the invoice totals
                //List<Invoice> i = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails);
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
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                invoice.fltShippingCharges = shipAmount;
                //send back to update
                IM.UpdateCurrentInvoice(invoice, objPageDetails);
            }
            //Exception catch
            catch(ThreadAbortException tae) { }
            catch(Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                    grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text, objPageDetails);
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnSearchCustomers_Click(object sender, EventArgs e)
        {
            string method = "btnSearchCustomers_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text, objPageDetails);
                grdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string method = "btnAddCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                Customer customer = new Customer();
                customer.varFirstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text;
                customer.varLastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text;
                customer.varAddress = "";
                customer.secondaryAddress = "";
                customer.varContactNumber = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text;
                customer.secondaryPhoneNumber = "";
                customer.bitSendMarketing = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked;
                customer.varEmailAddress = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmail")).Text;
                customer.billingAddress = "";
                customer.varCityName = "";
                customer.intProvinceID = CU.location.intProvinceID;
                customer.intCountryID = CU.location.intCountryID;
                customer.varPostalCode = "";
                int custNum = CM.addCustomer(customer, objPageDetails);
                //List<Invoice> i = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails);
                customer.intCustomerID = custNum;
                invoice.customer = customer;
                IM.UpdateCurrentInvoice(invoice, objPageDetails);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string method = "grdCustomersSearched_PageIndexChanging";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                grdCustomersSearched.PageIndex = e.NewPageIndex;
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text, objPageDetails);
                grdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //grabs the command argument for the command pressed 
                if (e.CommandName == "SwitchCustomer")
                {
                    Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    invoice.customer = C;
                    IM.UpdateCurrentInvoice(invoice, objPageDetails);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                    nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtSearch.Text != "")
                {
                    //if (IM.InventorySearchReturnsTradeIn(txtSearch.Text, CU, objPageDetails))
                    //{
                    //    grdInventorySearched.DataSource = ITM.ReturnTradeInSku(objPageDetails);
                    //}
                    //else
                    //{
                        grdInventorySearched.DataSource = ITM.ReturnInvoiceItemsFromSearchStringForSale(txtSearch.Text, objPageDetails);
                    //}
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                int currentInvoiceItemID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                IIM.ReturnQTYToInventory(currentInvoiceItemID, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                //Remove the indexed pointer
                grdCartItems.EditIndex = -1;

                IM.CalculateNewInvoiceTotalsToUpdate(IM.ReturnCurrentInvoice(invoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                invoice = IM.ReturnCurrentInvoice(invoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];

                //bind items back to grid view
                grdCartItems.DataSource = invoice.invoiceItems;
                grdCartItems.DataBind();
                //Calculate new subtotal
                lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("C");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                grdCartItems.DataSource = invoice.invoiceItems;
                grdCartItems.EditIndex = e.NewEditIndex;
                grdCartItems.DataBind();
                ((CheckBoxList)grdCartItems.Rows[e.NewEditIndex].Cells[9].FindControl("cblTaxes")).Enabled = true;

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                ((CheckBoxList)grdCartItems.Rows[grdCartItems.EditIndex].Cells[9].FindControl("cblTaxes")).Enabled = false;
                //Clears the indexed row
                grdCartItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdCartItems.DataSource = invoice.invoiceItems;
                grdCartItems.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                //Stores all the data for each element in the row
                InvoiceItems newItemInfo = new InvoiceItems();
                newItemInfo.intInvoiceID = invoice.intInvoiceID;
                newItemInfo.intInvoiceItemID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                newItemInfo.intInventoryID = IIM.ReturnInventoryIDFromInvoiceItemID(newItemInfo.intInvoiceItemID, objPageDetails);
                //newItemInfo.fltItemPrice = Convert.ToDouble(((Label)grdCartItems.Rows[e.RowIndex].Cells[5].FindControl("price")).Text.Replace("$",""));
                newItemInfo.fltItemDiscount = Convert.ToDouble(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("txtAmnt")).Text);
                newItemInfo.intItemQuantity = Convert.ToInt32(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                newItemInfo.bitIsDiscountPercent = ((CheckBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked;
                newItemInfo.bitIsClubTradeIn = ((CheckBox)grdCartItems.Rows[e.RowIndex].Cells[7].FindControl("chkTradeIn")).Checked;
                newItemInfo.intItemTypeID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[8].FindControl("lblTypeID")).Text);
                newItemInfo.invoiceItemTaxes = new List<InvoiceItemTax>();

                CheckBoxList checkboxTaxes = (CheckBoxList)grdCartItems.Rows[e.RowIndex].Cells[9].FindControl("cblTaxes");

                foreach (ListItem item in checkboxTaxes.Items)
                {
                    InvoiceItemTax iit = new InvoiceItemTax();
                    iit.intInvoiceItemID = newItemInfo.intInvoiceItemID;
                    iit.varTaxName = item.Text;
                    iit.bitIsTaxCharged = Convert.ToBoolean(item.Selected);
                    foreach(InvoiceItems itemCheck in invoice.invoiceItems)
                    {
                        foreach(InvoiceItemTax itemTaxCheck in itemCheck.invoiceItemTaxes)
                        {
                            if(itemCheck.intInvoiceItemID == iit.intInvoiceItemID)
                            {
                                if(itemTaxCheck.varTaxName == iit.varTaxName)
                                {
                                    itemTaxCheck.bitIsTaxCharged = iit.bitIsTaxCharged;
                                    newItemInfo.invoiceItemTaxes.Add(itemTaxCheck);
                                    newItemInfo.fltItemPrice = itemCheck.fltItemPrice;
                                }
                            }
                        }
                    }
                }

                if (!IIM.ValidQTY(newItemInfo, objPageDetails))
                {
                    if (newItemInfo.bitIsClubTradeIn)
                    {
                        IIM.UpdateItemFromCurrentSalesTable(newItemInfo, invoice.intTransactionTypeID, objPageDetails);
                    }
                    else
                    { 
                        //if it is less than 0 then there is not enough in invenmtory to sell
                        lblInvalidQty.Visible = true;
                        //Display error message
                        lblInvalidQty.Text = "Quantity Exceeds the Remaining Inventory";
                        lblInvalidQty.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    IIM.UpdateItemFromCurrentSalesTable(newItemInfo, invoice.intTransactionTypeID, objPageDetails);
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                IIM.LoopThroughTheItemsToReturnToInventory(invoice.intInvoiceID, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                IIM.RemoveInitialTotalsForTable(invoice.intInvoiceID, objPageDetails);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                invoice.intTransactionTypeID = 1;
                IM.UpdateCurrentInvoice(invoice, objPageDetails);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Invoice I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString(), objPageDetails)[0];
                //I.transactionType = 6;
                //IM.UpdateCurrentInvoice(I, objPageDetails);
                //Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                if (IIM.CheckForItemsInTransaction(invoice))
                {
                    UpdateInvoiceTotal();
                    lblInvalidQty.Visible = false;
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                    nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                    Response.Redirect("SalesCheckout.aspx?" + nameValues, false);
                }
                else
                {
                    MessageBox.ShowMessage("There are no items on this transaction.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
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
                    selectedSku.intInventoryID = Convert.ToInt32(e.CommandArgument);
                    selectedSku.intInvoiceID = invoice.intInvoiceID;
                    if (!IIM.ItemAlreadyInCart(selectedSku, objPageDetails))
                    {
                        //ToDo this check needs to look for the actual ID of the trade in sku
                        //13040 is the testing trade in sku
                        //13240 is the live trade in sku
                        //if (IM.InventoryIsInTradeInList(selectedSku.intInventoryID, CU, objPageDetails))
                        //{
                            
                        //}
                        //else
                        //{

                            double discount = 0;
                            string discountAmount = ((TextBox)grdInventorySearched.Rows[index].Cells[5].FindControl("txtAmountDiscount")).Text;
                            if (discountAmount != "")
                            {
                                if (double.TryParse(discountAmount, out discount))
                                {
                                    discount = Convert.ToDouble(discountAmount);
                                }
                            }
                            selectedSku.fltItemDiscount = discount;
                            selectedSku.varItemDescription = ((Label)grdInventorySearched.Rows[index].Cells[3].FindControl("Description")).Text;
                            selectedSku.fltItemRefund = 0;
                            selectedSku.fltItemPrice = double.Parse(((Label)grdInventorySearched.Rows[index].Cells[4].FindControl("rollPrice")).Text, NumberStyles.Currency);
                            selectedSku.fltItemCost = double.Parse(((Label)grdInventorySearched.Rows[index].Cells[4].FindControl("rollCost")).Text, NumberStyles.Currency);
                            selectedSku.bitIsDiscountPercent = ((CheckBox)grdInventorySearched.Rows[index].Cells[5].FindControl("chkDiscountPercent")).Checked;
                            selectedSku.bitIsClubTradeIn = ((CheckBox)grdInventorySearched.Rows[index].Cells[6].FindControl("chkTradeInSearch")).Checked;
                            selectedSku.intItemTypeID = Convert.ToInt32(((Label)grdInventorySearched.Rows[index].Cells[7].FindControl("lblTypeIDSearch")).Text);
                            selectedSku.intItemQuantity = quantity;

                            //add item to table and remove the added qty from current inventory
                            IIM.InsertItemIntoSalesCart(selectedSku, invoice.intTransactionTypeID, invoice.dtmInvoiceDate, CU, objPageDetails);
                            IIM.RemoveQTYFromInventoryWithSKU(selectedSku.intInventoryID, selectedSku.intItemTypeID, (currentQty - quantity), objPageDetails);
                            
                            //Set an empty variable to bind to the searched items grid view so it is empty
                            grdInventorySearched.DataSource = null;
                            grdInventorySearched.DataBind();
                            //Recalculate the new subtotal
                            UpdateInvoiceTotal();
                        //}
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void UpdateInvoiceTotal()
        {
            string method = "UpdateInvoiceTotal";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                IM.CalculateNewInvoiceTotalsToUpdate(invoice, objPageDetails);
                invoice = IM.ReturnCurrentInvoice(invoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
                lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("C");
                grdCartItems.DataSource = invoice.invoiceItems;
                grdCartItems.DataBind();
            }
            //Exception catch
            catch(ThreadAbortException tae) { }
            catch(Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnRefreshCart_Click(object sender, EventArgs e)
        {
            string method = "btnRefreshCart_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            string method = "btnClearSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void grdCartItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBoxList checkboxTaxes = (CheckBoxList)e.Row.FindControl("cblTaxes");
                foreach (ListItem item in checkboxTaxes.Items)
                {
                    item.Selected = bool.Parse(item.Value);
                }
            }
             
        }

        protected void btnAddTradeIn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInventorySearched_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                btnRefreshCart.Visible = true;
            //Trade In Sku to add in SK
            string redirect = "<script>window.open('TradeINEntry.aspx?invoice=" + Request.QueryString["invoice"].ToString() + "');</script>";
            Response.Write(redirect);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}