using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
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
        InvoiceItemsManager IIM = new InvoiceItemsManager();
        InvoiceManager IM = new InvoiceManager();
        LocationManager LM = new LocationManager();
        ItemsManager ItM = new ItemsManager();
        //private static Invoice receipt;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PurchasesCart.aspx";
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
                    Invoice receipt = new Invoice();
                    CU = (CurrentUser)Session["currentUser"];
                    if (!Page.IsPostBack)
                    {
                        if (!IM.ReturnBolInvoiceExists(Convert.ToInt32(Request.QueryString["receipt"].ToString()), objPageDetails))
                        {
                            receipt.varInvoiceNumber = IM.ReturnNextReceiptNumber(CU, objPageDetails);
                            receipt.intInvoiceSubNumber = 1;
                            receipt.customer = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0];
                            receipt.employee = CU.employee;
                            receipt.location = CU.location;
                            receipt.fltGovernmentTaxAmount = 0;
                            receipt.fltProvincialTaxAmount = 0;
                            receipt.intTransactionTypeID = 5;
                            //receipt.bitChargeGST = false;
                            //receipt.bitChargePST = false;
                            IM.CreateInitialTotalsForTable(receipt, objPageDetails);
                        }
                        else
                        {
                            receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                        }

                        //Checks if there is a Customer Number stored in the Session

                        //Set name in text box
                        txtCustomer.Text = receipt.customer.varFirstName + " " + receipt.customer.varLastName;
                        //display system time in Sales Page
                        DateTime today = DateTime.Today;
                        lblDateDisplay.Text = today.ToString("dd/MMM/yy");
                        lblReceiptNumberDisplay.Text = receipt.varInvoiceNumber.ToString();
                        UpdateReceiptTotal();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0];
                    //Set name in text box
                    txtCustomer.Text = C.varFirstName + " " + C.varLastName;
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
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                Customer C = new Customer
                {
                    varFirstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text,
                    varLastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text,
                    varAddress = "",
                    secondaryAddress = "",
                    varContactNumber = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text,
                    secondaryPhoneNumber = "",
                    bitSendMarketing = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked,
                    varEmailAddress = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmail")).Text,
                    varCityName = "",
                    intProvinceID = CU.location.intProvinceID,
                    intCountryID = CU.location.intCountryID,
                    varPostalCode = ""
                };
                C.intCustomerID = CM.addCustomer(C, objPageDetails);
                receipt.customer = C;
                IM.UpdateCurrentInvoice(receipt, objPageDetails);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", receipt.intInvoiceID.ToString());
                nameValues.Set("customer", receipt.customer.intCustomerID.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                //grabs the command argument for the command pressed 
                if (e.CommandName == "SwitchCustomer")
                {
                    //if command argument is SwitchCustomer, set the new key
                    receipt = IM.ReturnCurrentInvoice(receipt.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
                    receipt.customer = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    IM.UpdateCurrentInvoice(receipt, objPageDetails);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", receipt.customer.intCustomerID.ToString());
                    nameValues.Set("receipt", receipt.intInvoiceID.ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
                btnCustomerSelect.Text = "Change Customer";
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                string[] inventoryInfo = ItM.ReserveTradeInSKU(CU, objPageDetails);
                InvoiceItems purchItem = new InvoiceItems
                {
                    intInventoryID = Convert.ToInt32(inventoryInfo[1]),
                    varSku = inventoryInfo[0].ToString(),
                    intItemQuantity = 1,
                    varItemDescription = "",
                    fltItemCost = 0.00,
                    intInvoiceID = receipt.intInvoiceID,                   
                    fltItemDiscount = 0,
                    fltItemRefund = 0,
                    fltItemPrice = 0,
                    bitIsDiscountPercent = false,
                    bitIsClubTradeIn = false,
                    intItemTypeID = 1
                };

                IIM.InsertItemIntoSalesCart(purchItem, receipt.intTransactionTypeID, receipt.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                //Bind items in cart to grid view
                UpdateReceiptTotal();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                //it's available columns
                grdPurchasedItems.DataSource = receipt.invoiceItems;
                grdPurchasedItems.EditIndex = e.NewEditIndex;
                grdPurchasedItems.DataBind();
                //Recalculates subtotal
                //lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount(itemsInCart).ToString();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Currently used for cancelling the edit
        protected void OnRowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "ORowCanceling";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdPurchasedItems.DataSource = receipt.invoiceItems;
                grdPurchasedItems.DataBind();
                //Recalcluate subtotal
                //lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                //creates a temp item with the new updates
                InvoiceItems purchItem = new InvoiceItems
                {
                    intInvoiceID = receipt.intInvoiceID,
                    intInvoiceItemID = Convert.ToInt32(((Label)grdPurchasedItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text),
                    fltItemCost = Convert.ToDouble(((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text),
                    varItemDescription = ((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[2].Controls[0]).Text
                };

                IIM.UpdateItemFromCurrentSalesTableActualQueryForPurchases(purchItem, objPageDetails);

                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds cart items to grid view
                UpdateReceiptTotal();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.CancellingReceipt(IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                nameValues.Set("customer", Request.QueryString["customer"].ToString());
                //Changes to Sales Checkout page
                Response.Redirect("PurchasesCheckout.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void UpdateReceiptTotal()
        {
            string method = "UpdateReceiptTotal";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.CalculateNewReceiptTotalsToUpdate(IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                Invoice receipt = IM.ReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
                grdPurchasedItems.DataSource = receipt.invoiceItems;
                grdPurchasedItems.DataBind();
                //Recalculates the new subtotal
                lblPurchaseAmountDisplay.Text = "$ " + receipt.fltSubTotal.ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}