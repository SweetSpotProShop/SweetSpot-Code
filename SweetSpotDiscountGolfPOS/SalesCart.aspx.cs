using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesCart : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly CustomerManager CM = new CustomerManager();
        readonly LocationManager LM = new LocationManager();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly ItemsManager ITM = new ItemsManager();
        readonly InvoiceItemsManager IIM = new InvoiceItemsManager();
        readonly TaxManager TM = new TaxManager();
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
                        if (Request.QueryString["invoice"].ToString() == "-10")
                        {
                            Invoice newInvoice = new Invoice
                            {
                                varInvoiceNumber = IM.CallReturnNextInvoiceNumberForNewInvoice(CU, objPageDetails),
                                intInvoiceSubNumber = 1,
                                customer = CM.CallReturnCustomer(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0],
                                employee = CU.employee,
                                location = CU.location,
                                fltGovernmentTaxAmount = 0,
                                fltProvincialTaxAmount = 0,
                                fltLiquorTaxAmount = 0,
                                bitIsShipping = false,
                                //newInvoice.bitChargeGST = true;
                                //newInvoice.bitChargePST = true;
                                //newInvoice.bitChargeLCT = true;
                                intTransactionTypeID = 1,
                                varAdditionalInformation = ""
                            };
                            invoice = IM.CallCreateInitialTotalsForTable(newInvoice, objPageDetails)[0];

                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                            nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                            Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                        }
                        else
                        {
                            invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                        }

                        DdlShippingProvince.DataSource = LM.CallReturnProvinceDropDown(invoice.location.intCountryID, objPageDetails);
                        DdlShippingProvince.SelectedValue = invoice.intShippingProvinceID.ToString();
                        DdlShippingProvince.DataBind();

                        txtCustomer.Text = invoice.customer.varFirstName + " " + invoice.customer.varLastName;
                        lblDateDisplay.Text = DateTime.Today.ToString("dd/MMM/yy");
                        lblInvoiceNumberDisplay.Text = invoice.varInvoiceNumber + "-" + invoice.intInvoiceSubNumber;
                        //change to gather the items from table
                        GrdCartItems.DataSource = invoice.invoiceItems;
                        GrdCartItems.DataBind();
                        if (invoice.bitIsShipping)
                        {
                            RdbShipping.Checked = true;
                            DdlShippingProvince.Visible = true;
                            DdlShippingProvince.Enabled = true;
                        }
                        else
                        {
                            RdbInStorePurchase.Checked = true;
                            DdlShippingProvince.Visible = false;
                            DdlShippingProvince.Enabled = false;
                        }
                        txtShippingAmount.Text = invoice.fltShippingCharges.ToString();
                        lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("C");
                        Session["currentInvoice"] = invoice;
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }


        //these are page processes
        protected void BtnCustomerSelect_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCustomerSelect_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (BtnCustomerSelect.Text == "Cancel")
                {
                    BtnCustomerSelect.Text = "Change Customer";
                    GrdCustomersSearched.Visible = false;
                }
                else
                {
                    GrdCustomersSearched.Visible = true;
                    GrdCustomersSearched.DataSource = CM.CallReturnCustomerBasedOnText(txtCustomer.Text, objPageDetails);
                    GrdCustomersSearched.DataBind();
                    if (GrdCustomersSearched.Rows.Count > 0)
                    {
                        BtnCustomerSelect.Text = "Cancel";
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string method = "GrdCustomersSearched_PageIndexChanging";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdCustomersSearched.PageIndex = e.NewPageIndex;
                GrdCustomersSearched.Visible = true;
                GrdCustomersSearched.DataSource = CM.CallReturnCustomerBasedOnText(txtCustomer.Text, objPageDetails);
                GrdCustomersSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnInventorySearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtSearch.Text != "")
                {
                    lblInvalidQty.Visible = false;
                    GrdInventorySearched.DataSource = ITM.CallReturnInvoiceItemsFromSearchStringForSale(txtSearch.Text, objPageDetails);
                    GrdInventorySearched.DataBind();
                    txtSearch.Text = "";
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowEditing";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                //lblInvalidQty.Visible = false;
                GrdCartItems.DataSource = invoice.invoiceItems;
                GrdCartItems.EditIndex = e.NewEditIndex;
                GrdCartItems.DataBind();
                ((CheckBoxList)GrdCartItems.Rows[e.NewEditIndex].Cells[9].FindControl("cblTaxes")).Enabled = true;

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "ORowCanceling";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                //lblInvalidQty.Visible = false;
                ((CheckBoxList)GrdCartItems.Rows[GrdCartItems.EditIndex].Cells[9].FindControl("cblTaxes")).Enabled = false;
                //Clears the indexed row
                GrdCartItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                GrdCartItems.DataSource = invoice.invoiceItems;
                GrdCartItems.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                IIM.LoopThroughTheItemsToReturnToInventory(invoice.intInvoiceID, invoice.dtmInvoiceDate, Convert.ToInt32(DdlShippingProvince.SelectedValue), objPageDetails);
                IIM.CallRemoveInitialTotalsForTable(invoice.intInvoiceID, objPageDetails);
                Response.Redirect("SalesHomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnExitSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExitSale_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Response.Redirect("SalesHomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnProceedToCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnProceedToCheckout_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                if (invoice.invoiceItems.Count > 0)
                {
                    //UpdateInvoiceTotal();
                    lblInvalidQty.Visible = false;
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                    nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                    Response.Redirect("SalesCheckout.aspx?" + nameValues, false);
                }
                else
                {
                    MessageBoxCustom.ShowMessage("There are no items on this transaction.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnJumpToInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnJumpToInventory_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnClearSearch_Click(object sender, EventArgs e)
        {
            string method = "BtnClearSearch_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdInventorySearched.DataSource = null;
                GrdInventorySearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdCartItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBoxList checkboxTaxes = (CheckBoxList)e.Row.FindControl("cblTaxes");
                foreach (ListItem item in checkboxTaxes.Items)
                {
                    item.Selected = bool.Parse(item.Value);
                }
            }

        }
        protected void BtnAddTradeIn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddTradeIn_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                BtnRefreshCart.Visible = true;
                //Trade In Sku to add in SK
                string redirect = "<script>window.open('TradeINEntry.aspx?invoice=" + Request.QueryString["invoice"].ToString() + "');</script>";
                Response.Write(redirect);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }


        //These update the invoice        
        protected void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            string method = "BtnAddCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                Customer customer = new Customer
                {
                    varFirstName = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text,
                    varLastName = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtLastName")).Text,
                    varAddress = "",
                    secondaryAddress = "",
                    varContactNumber = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text,
                    secondaryPhoneNumber = "",
                    bitSendMarketing = ((CheckBox)GrdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked,
                    varEmailAddress = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtEmail")).Text,
                    billingAddress = "",
                    varCityName = "",
                    intProvinceID = CU.location.intProvinceID,
                    intCountryID = CU.location.intCountryID,
                    varPostalCode = ""
                };
                int custNum = CM.CallAddCustomer(customer, objPageDetails);                
                customer.intCustomerID = custNum;
                invoice.customer = customer;
                IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
                UpdateInvoiceTotals();
                Session["currentInvoice"] = invoice;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "GrdCustomersSearched_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //grabs the command argument for the command pressed 
                if (e.CommandName == "SwitchCustomer")
                {
                    Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                    Customer C = CM.CallReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    invoice.customer = C;
                    IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
                    UpdateInvoiceTotals();
                    Session["currentInvoice"] = invoice;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }


        //These update the invoice total
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                lblInvalidQty.Visible = false;
                int currentInvoiceItemID = Convert.ToInt32(((Label)GrdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                IIM.ReturnQTYToInventory(currentInvoiceItemID, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                //Remove the indexed pointer
                GrdCartItems.EditIndex = -1;
                IM.CalculateNewInvoiceTotalsToUpdate(IM.CallReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0], objPageDetails);
                invoice = IM.CallReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];

                //bind items back to grid view
                GrdCartItems.DataSource = invoice.invoiceItems;
                GrdCartItems.DataBind();
                //Calculate new subtotal
                lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("C");
                Session["currentInvoice"] = invoice;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowUpdating";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                lblInvalidQty.Visible = false;
                //Stores all the data for each element in the row
#pragma warning disable IDE0017 // Simplify object initialization
                InvoiceItems newItemInfo = new InvoiceItems();
#pragma warning restore IDE0017 // Simplify object initialization
                newItemInfo.intInvoiceID = invoice.intInvoiceID;
                newItemInfo.intInvoiceItemID = Convert.ToInt32(((Label)GrdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                newItemInfo.intInventoryID = IIM.CallReturnInventoryIDFromInvoiceItemID(newItemInfo.intInvoiceItemID, objPageDetails);
                //newItemInfo.fltItemPrice = Convert.ToDouble(((Label)grdCartItems.Rows[e.RowIndex].Cells[5].FindControl("price")).Text.Replace("$",""));
                newItemInfo.fltItemDiscount = Convert.ToDouble(((TextBox)GrdCartItems.Rows[e.RowIndex].Cells[6].FindControl("txtAmnt")).Text);
                newItemInfo.intItemQuantity = Convert.ToInt32(((TextBox)GrdCartItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                newItemInfo.bitIsDiscountPercent = ((CheckBox)GrdCartItems.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked;
                newItemInfo.bitIsClubTradeIn = ((CheckBox)GrdCartItems.Rows[e.RowIndex].Cells[7].FindControl("chkTradeIn")).Checked;
                newItemInfo.intItemTypeID = Convert.ToInt32(((Label)GrdCartItems.Rows[e.RowIndex].Cells[8].FindControl("lblTypeID")).Text);
                newItemInfo.invoiceItemTaxes = new List<InvoiceItemTax>();

                CheckBoxList checkboxTaxes = (CheckBoxList)GrdCartItems.Rows[e.RowIndex].Cells[9].FindControl("cblTaxes");

                foreach (ListItem item in checkboxTaxes.Items)
                {
#pragma warning disable IDE0017 // Simplify object initialization
                    InvoiceItemTax iit = new InvoiceItemTax();
#pragma warning restore IDE0017 // Simplify object initialization
                    iit.intInvoiceItemID = newItemInfo.intInvoiceItemID;
                    iit.varTaxName = item.Text;
                    iit.bitIsTaxCharged = Convert.ToBoolean(item.Selected);
                    foreach (InvoiceItems itemCheck in invoice.invoiceItems)
                    {
                        foreach (InvoiceItemTax itemTaxCheck in itemCheck.invoiceItemTaxes)
                        {
                            if (itemCheck.intInvoiceItemID == iit.intInvoiceItemID)
                            {
                                if (itemTaxCheck.varTaxName == iit.varTaxName)
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
                GrdCartItems.EditIndex = -1;
                //Recalculates the new subtotal and Binds cart items to grid view
                //Session["currentInvoice"] = invoice;
                UpdateInvoiceTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInventorySearched_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                lblInvalidQty.Visible = false;
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int quantity = 1;
                string qty = ((TextBox)GrdInventorySearched.Rows[index].Cells[2].FindControl("quantityToAdd")).Text;
                if (qty != "")
                {
                    if (int.TryParse(qty, out quantity))
                    {
                        quantity = Convert.ToInt32(qty);
                    }
                }
                int currentQty = Convert.ToInt32(((Label)GrdInventorySearched.Rows[index].Cells[2].FindControl("QuantityInOrder")).Text);
                if (quantity > currentQty || quantity < 1)
                {
                    lblInvalidQty.Visible = true;
                }
                else
                {
#pragma warning disable IDE0017 // Simplify object initialization
                    InvoiceItems selectedSku = new InvoiceItems();
#pragma warning restore IDE0017 // Simplify object initialization
                    selectedSku.intInventoryID = Convert.ToInt32(e.CommandArgument);
                    selectedSku.intInvoiceID = invoice.intInvoiceID;
                    if (!IIM.CallItemAlreadyInCart(selectedSku, objPageDetails))
                    {
                        double discount = 0;
                        string discountAmount = ((TextBox)GrdInventorySearched.Rows[index].Cells[5].FindControl("txtAmountDiscount")).Text;
                        if (discountAmount != "")
                        {
                            if (double.TryParse(discountAmount, out discount))
                            {
                                discount = Convert.ToDouble(discountAmount);
                            }
                        }
                        selectedSku.fltItemDiscount = discount;
                        selectedSku.varItemDescription = ((Label)GrdInventorySearched.Rows[index].Cells[3].FindControl("Description")).Text;
                        selectedSku.fltItemRefund = 0;
                        selectedSku.fltItemPrice = double.Parse(((Label)GrdInventorySearched.Rows[index].Cells[4].FindControl("rollPrice")).Text, NumberStyles.Currency);
                        selectedSku.fltItemCost = double.Parse(((Label)GrdInventorySearched.Rows[index].Cells[4].FindControl("rollCost")).Text, NumberStyles.Currency);
                        selectedSku.bitIsDiscountPercent = ((CheckBox)GrdInventorySearched.Rows[index].Cells[5].FindControl("chkDiscountPercent")).Checked;
                        selectedSku.bitIsClubTradeIn = ((CheckBox)GrdInventorySearched.Rows[index].Cells[6].FindControl("chkTradeInSearch")).Checked;
                        selectedSku.intItemTypeID = Convert.ToInt32(((Label)GrdInventorySearched.Rows[index].Cells[7].FindControl("lblTypeIDSearch")).Text);
                        selectedSku.intItemQuantity = quantity;

                        //add item to table and remove the added qty from current inventory
                        IIM.CallInsertItemIntoSalesCart(selectedSku, invoice.intTransactionTypeID, invoice.dtmInvoiceDate, Convert.ToInt32(DdlShippingProvince.SelectedValue), objPageDetails);
                        IIM.CallRemoveQTYFromInventoryWithSKU(selectedSku.intInventoryID, selectedSku.intItemTypeID, (currentQty - quantity), objPageDetails);

                        invoice = IM.CallReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];
                        //Set an empty variable to bind to the searched items grid view so it is empty
                        GrdInventorySearched.DataSource = null;
                        GrdInventorySearched.DataBind();
                        Session["currentInvoice"] = invoice;
                        //Recalculate the new subtotal
                        UpdateInvoiceTotals();
                    }
                    else
                    {
                        MessageBoxCustom.ShowMessage("Item is already in the cart. Please update item in cart or process a second sale.", this);
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnRefreshCart_Click(object sender, EventArgs e)
        {
            string method = "BtnRefreshCart_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                BtnRefreshCart.Visible = false;
                UpdateInvoiceTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void RdbInStorePurchase_CheckedChanged(object sender, EventArgs e)
        {
            string method = "RdbInStorePurchase_CheckedChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (RdbShipping.Checked)
                {
                    DdlShippingProvince.Enabled = true;
                    DdlShippingProvince.Visible = true;
                }
                else
                {
                    DdlShippingProvince.Enabled = false;
                    DdlShippingProvince.Visible = false;

                    Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                    invoice.intShippingProvinceID = CU.location.intProvinceID;
                    invoice.fltShippingCharges = 0;
                    txtShippingAmount.Text = invoice.fltShippingCharges.ToString();
                    TM.ChangeProvinceTaxesBasedOnShipping(Convert.ToInt32(Request.QueryString["invoice"].ToString()), invoice.intShippingProvinceID, objPageDetails);
                    //Session["currentInvoice"] = invoice;
                    DdlShippingProvince.SelectedValue = CU.location.intProvinceID.ToString();
                }
                UpdateInvoiceTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void RdbShipping_CheckedChanged(object sender, EventArgs e)
        {
            string method = "RdbShipping_CheckedChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (RdbShipping.Checked)
                {
                    DdlShippingProvince.Enabled = true;
                    DdlShippingProvince.Visible = true;
                }
                else
                {
                    Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                    DdlShippingProvince.Enabled = false;
                    DdlShippingProvince.Visible = false;                    
                    invoice.intShippingProvinceID = CU.location.intProvinceID;
                    invoice.fltShippingCharges = 0;
                    txtShippingAmount.Text = invoice.fltShippingCharges.ToString();
                    TM.ChangeProvinceTaxesBasedOnShipping(Convert.ToInt32(Request.QueryString["invoice"].ToString()), invoice.intShippingProvinceID, objPageDetails);
                    //Session["currentInvoice"] = invoice;
                    DdlShippingProvince.SelectedValue = CU.location.intProvinceID.ToString();
                }                
                UpdateInvoiceTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void DdlShippingProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = "DdlShippingProvince_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //needs to go through and remove all current taxes then apply taxes for new delected province
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                invoice.intShippingProvinceID = Convert.ToInt32(DdlShippingProvince.SelectedValue);
                TM.ChangeProvinceTaxesBasedOnShipping(Convert.ToInt32(Request.QueryString["invoice"].ToString()), invoice.intShippingProvinceID, objPageDetails);
                IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
                UpdateInvoiceTotals();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void UpdateInvoiceTotals()
        {
            string method = "UpdateInvoiceTotal";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];
                invoice.fltShippingCharges = Convert.ToDouble(txtShippingAmount.Text);
                bool bolShip = false;
                if (RdbShipping.Checked)
                {
                    bolShip = true;
                }
                invoice.bitIsShipping = bolShip;
                IM.CalculateNewInvoiceTotalsToUpdate(invoice, objPageDetails);
                invoice = IM.CallReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];
                lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("C");
                GrdCartItems.DataSource = invoice.invoiceItems;
                GrdCartItems.DataBind();
                //Session["currentInvoice"] = invoice;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}