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
                    CU = (CurrentUser)Session["currentUser"];
                    if (!Page.IsPostBack)
                    {
                        //Checks if there is a Customer Number stored in the Session
                        Customer C = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["cust"].ToString()), objPageDetails)[0];
                        //Set name in text box
                        txtCustomer.Text = C.firstName + " " + C.lastName;
                        //display system time in Sales Page
                        DateTime today = DateTime.Today;
                        lblDateDisplay.Text = today.ToString("dd/MMM/yy");
                        lblReceiptNumberDisplay.Text = Request.QueryString["receipt"].ToString();

                        Invoice R = new Invoice(Convert.ToInt32(Request.QueryString["receipt"].ToString().Split('-')[1]), 1, DateTime.Now, DateTime.Now, C, CU.emp, CU.location, 0, 0, 0, 0, 0, 0, 0, 5, "");
                        if (!IM.ReturnBolInvoiceExists(Request.QueryString["receipt"].ToString(), objPageDetails))
                        {
                            IM.CreateInitialTotalsForTable(R, objPageDetails);
                        }
                        UpdateReceiptTotal();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["cust"].ToString()), objPageDetails)[0];
                    //Set name in text box
                    txtCustomer.Text = C.firstName + " " + C.lastName;
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                Location L = LM.ReturnLocation(CU.location.locationID, objPageDetails)[0];
                Customer C = new Customer
                {
                    firstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text,
                    lastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text,
                    primaryAddress = "",
                    secondaryAddress = "",
                    primaryPhoneNumber = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text,
                    secondaryPhoneNumber = "",
                    emailList = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked,
                    email = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmail")).Text,
                    city = "",
                    province = L.provID,
                    country = L.countryID,
                    postalCode = ""
                };
                C.customerId = CM.addCustomer(C, objPageDetails);
                Invoice R = IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString(), objPageDetails)[0];
                R.customer = C;
                IM.UpdateCurrentInvoice(R, objPageDetails);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                nameValues.Set("cust", R.customer.customerId.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                    //if command argument is SwitchCustomer, set the new key
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    Invoice R = IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString() + "-1", objPageDetails)[0];
                    R.customer = C;
                    IM.UpdateCurrentInvoice(R, objPageDetails);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("cust", C.customerId.ToString());
                    nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
                btnCustomerSelect.Text = "Change Customer";
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                InvoiceItems purchItem = new InvoiceItems
                {
                    sku = ItM.ReserveTradeInSKU(CU.location.locationID, objPageDetails),
                    quantity = 1,
                    description = "",
                    cost = 0.00,
                    invoiceNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[1]),
                    invoiceSubNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[2]),
                    itemDiscount = 0,
                    itemRefund = 0,
                    price = 0,
                    percentage = false,
                    isTradeIn = false,
                    typeID = 1
                };

                IIM.InsertItemIntoSalesCart(purchItem, objPageDetails);
                //Bind items in cart to grid view
                UpdateReceiptTotal();
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                //it's available columns
                grdPurchasedItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["receipt"].ToString() + "-1", objPageDetails);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdPurchasedItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["receipt"].ToString() + "-1", objPageDetails);
                grdPurchasedItems.DataBind();
                //Recalcluate subtotal
                //lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                //creates a temp item with the new updates
                InvoiceItems purchItem = new InvoiceItems
                {
                    invoiceNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[1]),
                    invoiceSubNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[2]),
                    sku = Convert.ToInt32(grdPurchasedItems.Rows[e.RowIndex].Cells[1].Text),
                    cost = Convert.ToDouble(((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text),
                    description = ((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[2].Controls[0]).Text
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                IM.CancellingReceipt(IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString() + "-1", objPageDetails)[0], objPageDetails);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                //Changes to Sales Checkout page
                Response.Redirect("PurchasesCheckout.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                IM.CalculateNewReceiptTotalsToUpdate(IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString(), objPageDetails)[0], objPageDetails);
                Invoice R = IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString(), objPageDetails)[0];
                grdPurchasedItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["receipt"].ToString(), objPageDetails);
                grdPurchasedItems.DataBind();
                //Recalculates the new subtotal
                lblPurchaseAmountDisplay.Text = "$ " + R.subTotal.ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}