using System;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class PurchasesCart : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly CustomerManager CM = new CustomerManager();
        readonly InvoiceItemsManager IIM = new InvoiceItemsManager();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly ItemsManager ItM = new ItemsManager();
        //LocationManager LM = new LocationManager();
        CurrentUser CU;
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
                        if (!IM.CallReturnBolInvoiceExists(Convert.ToInt32(Request.QueryString["receipt"].ToString()), objPageDetails))
                        {
                            receipt.varInvoiceNumber = IM.CallReturnNextReceiptNumber(CU, objPageDetails);
                            receipt.intInvoiceSubNumber = 1;
                            //receipt.customer = CM.CallReturnCustomer(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0];
                            //receipt.employee = CU.employee;
                            //receipt.location = CU.location;
                            receipt.intCustomerID = Convert.ToInt32(Request.QueryString["customer"].ToString());
                            receipt.intEmployeeID = CU.employee.intEmployeeID;
                            receipt.intLocationID = CU.location.intLocationID;
                            receipt.fltGovernmentTaxAmount = 0;
                            receipt.fltProvincialTaxAmount = 0;
                            receipt.intTransactionTypeID = 5;
                            //receipt.bitChargeGST = false;
                            //receipt.bitChargePST = false;
                            IM.CallCreateInitialTotalsForTable(receipt, objPageDetails);
                        }
                        else
                        {
                            receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                        }

                        //Checks if there is a Customer Number stored in the Session
                        Customer cust = CM.CallReturnCustomer(receipt.intCustomerID, objPageDetails)[0];
                        //Set name in text box
                        //txtCustomer.Text = receipt.customer.varFirstName + " " + receipt.customer.varLastName;
                        txtCustomer.Text = cust.varFirstName + " " + cust.varLastName;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
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
                    Customer C = CM.CallReturnCustomer(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0];
                    //Set name in text box
                    txtCustomer.Text = C.varFirstName + " " + C.varLastName;
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
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            string method = "BtnAddCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                Customer C = new Customer
                {
                    varFirstName = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text,
                    varLastName = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtLastName")).Text,
                    varAddress = "",
                    secondaryAddress = "",
                    varContactNumber = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text,
                    secondaryPhoneNumber = "",
                    bitSendMarketing = ((CheckBox)GrdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked,
                    varEmailAddress = ((TextBox)GrdCustomersSearched.FooterRow.FindControl("txtEmail")).Text,
                    varCityName = "",
                    intProvinceID = CU.location.intProvinceID,
                    intCountryID = CU.location.intCountryID,
                    varPostalCode = ""
                };
                receipt.intCustomerID = CM.CallAddCustomer(C, objPageDetails);
                //receipt.customer = C;
                IM.CallUpdateCurrentInvoice(receipt, objPageDetails);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", receipt.intInvoiceID.ToString());
                nameValues.Set("customer", receipt.intCustomerID.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                //grabs the command argument for the command pressed 
                if (e.CommandName == "SwitchCustomer")
                {
                    //if command argument is SwitchCustomer, set the new key
                    receipt = IM.CallReturnCurrentInvoice(receipt.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
                    receipt.intCustomerID = Convert.ToInt32(e.CommandArgument.ToString());
                    //receipt.customer = CM.CallReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    IM.CallUpdateCurrentInvoice(receipt, objPageDetails);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", receipt.intCustomerID.ToString());
                    nameValues.Set("receipt", receipt.intInvoiceID.ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
                BtnCustomerSelect.Text = "Change Customer";
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void BtnAddPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method error tracking
            string method = "BtnAddPurchase_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                string[] inventoryInfo = ItM.CallReserveTradeInSKU(CU, objPageDetails);
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

                IIM.CallInsertItemIntoSalesCart(purchItem, receipt.intTransactionTypeID, receipt.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                //Bind items in cart to grid view
                UpdateReceiptTotal();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"]), CU.location.intProvinceID, objPageDetails)[0];
                //creates a temp item with the new updates
                InvoiceItems purchItem = new InvoiceItems
                {
                    intInvoiceID = receipt.intInvoiceID,
                    intInvoiceItemID = Convert.ToInt32(((Label)grdPurchasedItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text),
                    fltItemCost = Convert.ToDouble(((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text),
                    varItemDescription = ((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[2].Controls[0]).Text
                };

                IIM.CallUpdateItemFromCurrentSalesTableActualQueryForPurchases(purchItem, objPageDetails);

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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.CancellingReceipt(IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnProceedToPayOut_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnProceedToCheckout_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                IM.CalculateNewReceiptTotalsToUpdate(IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                Invoice receipt = IM.CallReturnCurrentPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.location.intProvinceID, objPageDetails)[0];
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}