using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        Customer C = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["cust"].ToString()))[0];
                        //Set name in text box
                        txtCustomer.Text = C.firstName + " " + C.lastName;
                        //display system time in Sales Page
                        DateTime today = DateTime.Today;
                        lblDateDisplay.Text = today.ToString("yyyy-MM-dd");
                        lblReceiptNumberDisplay.Text = Request.QueryString["receipt"].ToString();

                        Invoice R = new Invoice(Convert.ToInt32(Request.QueryString["receipt"].ToString().Split('-')[1]), 1, DateTime.Now, DateTime.Now, C, CU.emp, CU.location, 0, 0, 0, 0, 0, 0, 0, 5, "");
                        if (!IM.ReturnBolInvoiceExists(Request.QueryString["receipt"].ToString()))
                        {
                            IM.CreateInitialTotalsForTable(R);
                        }
                        UpdateReceiptTotal();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                if (btnCustomerSelect.Text == "Cancel")
                {
                    btnCustomerSelect.Text = "Change Customer";
                    grdCustomersSearched.Visible = false;
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(Request.QueryString["cust"].ToString()))[0];
                    //Set name in text box
                    txtCustomer.Text = C.firstName + " " + C.lastName;
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
            try
            {
                Location L = LM.ReturnLocation(CU.location.locationID)[0];
                Customer C = new Customer();
                C.firstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text;
                C.lastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text;
                C.primaryAddress = "";
                C.secondaryAddress = "";
                C.primaryPhoneNumber = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text;
                C.secondaryPhoneNumber = "";
                C.emailList = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked;
                C.email = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmail")).Text;
                C.city = "";
                C.province = L.provID;
                C.country = L.countryID;
                C.postalCode = "";
                C.customerId = CM.addCustomer(C);
                Invoice R = IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString())[0];
                R.customer = C;
                IM.UpdateCurrentInvoice(R);
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                nameValues.Set("cust", R.customer.customerId.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                //grabs the command argument for the command pressed 
                if (e.CommandName == "SwitchCustomer")
                {
                    //if command argument is SwitchCustomer, set the new key
                    Customer C = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()))[0];
                    Invoice R = IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString() + "-1")[0];
                    R.customer = C;
                    IM.UpdateCurrentInvoice(R);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("cust", C.customerId.ToString());
                    nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                }
                btnCustomerSelect.Text = "Change Customer";
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                InvoiceItems purchItem = new InvoiceItems();

                purchItem.sku = ItM.ReserveTradeInSKU(CU.location.locationID);
                purchItem.quantity = 1;
                purchItem.description = "";
                purchItem.cost = 0.00;
                purchItem.invoiceNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[1]);
                purchItem.invoiceSubNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[2]);
                purchItem.itemDiscount = 0;
                purchItem.itemRefund = 0;
                purchItem.price = 0;
                purchItem.percentage = false;
                purchItem.isTradeIn = false;
                purchItem.typeID = 1;

                IIM.InsertItemIntoSalesCart(purchItem);
                //Bind items in cart to grid view
                UpdateReceiptTotal();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                //it's available columns
                grdPurchasedItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["receipt"].ToString() + "-1");
                grdPurchasedItems.EditIndex = e.NewEditIndex;
                grdPurchasedItems.DataBind();
                //Recalculates subtotal
                //lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount(itemsInCart).ToString();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdPurchasedItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["receipt"].ToString() + "-1");
                grdPurchasedItems.DataBind();
                //Recalcluate subtotal
                //lblPurchaseAmountDisplay.Text = "$ " + scm.returnPurchaseAmount((List<Cart>)Session["ItemsInCart"]).ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                //creates a temp item with the new updates
                InvoiceItems purchItem = new InvoiceItems();
                purchItem.invoiceNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[1]);
                purchItem.invoiceSubNum = Convert.ToInt32((Request.QueryString["receipt"].ToString()).Split('-')[2]);
                purchItem.sku = Convert.ToInt32(grdPurchasedItems.Rows[e.RowIndex].Cells[1].Text);
                purchItem.cost = Convert.ToDouble(((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                purchItem.description = ((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[2].Controls[0]).Text;

                IIM.UpdateItemFromCurrentSalesTableActualQueryForPurchases(purchItem);

                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds cart items to grid view
                UpdateReceiptTotal();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                IM.CancellingReceipt(IM.ReturnCurrentInvoice(Request.QueryString["receipt"].ToString() + "-1")[0]);
                //Change to Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", Request.QueryString["receipt"].ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                //Changes to Sales Checkout page
                Response.Redirect("PurchasesCheckout.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
            IM.CalculateNewReceiptTotalsToUpdate(IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString())[0]);
            Invoice R = IM.ReturnCurrentPurchaseInvoice(Request.QueryString["receipt"].ToString())[0];
            grdPurchasedItems.DataSource = IIM.ReturnItemsInTheCart(Request.QueryString["receipt"].ToString());
            grdPurchasedItems.DataBind();
            //Recalculates the new subtotal
            lblPurchaseAmountDisplay.Text = "$ " + R.subTotal.ToString("#0.00");
        }
    }
}