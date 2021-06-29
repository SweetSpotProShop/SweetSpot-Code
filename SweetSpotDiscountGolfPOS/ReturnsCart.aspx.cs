using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReturnsCart : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly InvoiceItemsManager IIM = new InvoiceItemsManager();
        CurrentUser CU;
        //private static Invoice returnInvoice;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReturnsCart.aspx";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                        List<Invoice> returnInvoicesCalled = IM.CallReturnInvoice(Convert.ToInt32(Request.QueryString["invoice"]), objPageDetails);
                        Invoice returnInvoice = new Invoice();
                        if (returnInvoicesCalled.Count > 0)
                        {
                            returnInvoice = returnInvoicesCalled[0];
                            returnInvoice.intInvoiceSubNumber = IM.CallCalculateNextInvoiceSubNum(returnInvoice.varInvoiceNumber, objPageDetails);
                            returnInvoice.location = CU.location;
                            returnInvoice.employee = CU.employee;
                            returnInvoice.intTransactionTypeID = 2;
                            returnInvoice = IM.CallCreateInitialTotalsForTable(returnInvoice, objPageDetails)[0];

                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("invoice", returnInvoice.intInvoiceID.ToString());
                            Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                        }
                        else
                        {
                            returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                        }
                        lblCustomerDisplay.Text = returnInvoice.customer.varFirstName.ToString() + " " + returnInvoice.customer.varLastName.ToString();
                        lblInvoiceNumberDisplay.Text = returnInvoice.varInvoiceNumber + "-" + returnInvoice.intInvoiceSubNumber;
                        lblDateDisplay.Text = returnInvoice.dtmInvoiceDate.ToString("dd/MMM/yy");
                        //binds items in cart to gridview
                        GrdInvoicedItems.DataSource = IIM.CallReturnInvoiceItemsFromProcessedSalesForReturn(returnInvoice.varInvoiceNumber.ToString(),returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                        GrdInvoicedItems.DataBind();

                        GrdReturningItems.DataSource = returnInvoice.invoiceItems;
                        GrdReturningItems.DataBind();

                        //IM.CalculateNewInvoiceReturnTotalsToUpdate(returnInvoice, objPageDetails);                        
                        lblReturnSubtotalDisplay.Text = "$ " + returnInvoice.fltSubTotal.ToString("#0.00");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                IIM.LoopThroughTheItemsToReturnToInventory(returnInvoice.intInvoiceID, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                IIM.CallRemoveInitialTotalsForTable(returnInvoice.intInvoiceID, objPageDetails);
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void BtnProceedToReturnCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnProceedToCheckout_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                if(returnInvoice.invoiceItems.Count > 0)
//                if (IIM.CheckForItemsInTransaction(returnInvoice))
                {
                    lblInvalidQty.Visible = false;
                    //Changes page to the returns checkout page
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("invoice", returnInvoice.intInvoiceID.ToString());
                    Response.Redirect("ReturnsCheckout.aspx?" + nameValues, false);
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdInvoicedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInvoicedItems_RowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                //Stores the info about the item in that index
                InvoiceItems selectedSku = IIM.CallReturnInvoiceItemForReturnProcess(Convert.ToInt32(((Label)GrdInvoicedItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text), objPageDetails);

                //if (selectedSku.bitIsDiscountPercent)
                //{
                //    selectedSku.fltItemPrice -= ((selectedSku.fltItemDiscount / 100) * selectedSku.fltItemPrice);
                //}
                //else
                //{
                //    selectedSku.fltItemPrice -= selectedSku.fltItemDiscount;
                //}

                selectedSku.intInvoiceID = Convert.ToInt32(returnInvoice.intInvoiceID);
                if (!IIM.CallItemAlreadyInCart(selectedSku, objPageDetails))
                {
                    int currentQTY = selectedSku.intItemQuantity;
                    string quantityForReturn = ((TextBox)GrdInvoicedItems.Rows[e.RowIndex].Cells[2].FindControl("quantityToReturn")).Text;
                    int quantitySold = Convert.ToInt32(((Label)GrdInvoicedItems.Rows[e.RowIndex].Cells[2].FindControl("quantitySold")).Text);
                    int returnQuantity = 1;
                    if (quantityForReturn != "")
                    {
                        if (int.TryParse(quantityForReturn, out returnQuantity))
                        {
                            returnQuantity = Convert.ToInt32(quantityForReturn);
                        }
                    }
                    if (returnQuantity > quantitySold || returnQuantity < 1)
                    {
                        lblInvalidQty.Visible = true;
                    }
                    else
                    {
                        double returnDollars = 0;
                        string returnAmount = ((TextBox)GrdInvoicedItems.Rows[e.RowIndex].Cells[7].FindControl("txtReturnAmount")).Text;
                        if(returnAmount != "")
                        {
                            if(double.TryParse(returnAmount, out returnDollars))
                            {
                                returnDollars = Convert.ToDouble(returnAmount);
                            }
                        }
                        IIM.CallRemoveQTYFromInventoryWithSKU(selectedSku.intInventoryID, selectedSku.intItemTypeID, (currentQTY + returnQuantity), objPageDetails);
                        selectedSku.intItemQuantity = returnQuantity;
                        selectedSku.fltItemRefund = -1 * returnDollars;
                        selectedSku.fltItemCost *= -1;
                        IIM.CallInsertItemIntoSalesCart(selectedSku, returnInvoice.intTransactionTypeID, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                        //deselect the indexed item
                        GrdInvoicedItems.EditIndex = -1;
                        //store items available for return in session
                        GrdInvoicedItems.DataSource = IIM.CallReturnInvoiceItemsFromProcessedSalesForReturn(returnInvoice.varInvoiceNumber.ToString(), returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                        GrdInvoicedItems.DataBind();

                        IM.CalculateNewInvoiceReturnTotalsToUpdate(IM.CallReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                        returnInvoice = IM.CallReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];

                        GrdReturningItems.DataSource = returnInvoice.invoiceItems;
                        GrdReturningItems.DataBind();

                        //recalculate the return total
                        lblReturnSubtotalDisplay.Text = "$ " + returnInvoice.fltSubTotal.ToString("#0.00");
                    }
                }
                else
                {
                    MessageBoxCustom.ShowMessage("Same item cannot be returned for a different amount. "
                         + "Either cancel item to set both at new return amount or process a second return.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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
        protected void GrdReturningItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdReturningItems_RowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                //Gathers index from selected line item
                int index = e.RowIndex;
                //Stores the info about the item in that index
                int invoiceItemReturnID = Convert.ToInt32(((Label)GrdReturningItems.Rows[index].Cells[0].FindControl("lblInvoiceItemReturnID")).Text);
                InvoiceItems selectedSku = IIM.ReturnSkuFromCurrentSalesUsingSKU(invoiceItemReturnID, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);

                //add item to table and remove the added qty from current inventory
                IIM.DoNotReturnTheItemOnReturn(selectedSku, objPageDetails);
                IIM.CallRemoveQTYFromInventoryWithSKU(selectedSku.intInventoryID, selectedSku.intItemTypeID, (IIM.ReturnCurrentQuantityOfItem(selectedSku, objPageDetails) - selectedSku.intItemQuantity), objPageDetails);
                //deselect the indexed item
                GrdReturningItems.EditIndex = -1;
                //Check if the marked for returns cart has any items in it
                GrdInvoicedItems.DataSource = IIM.CallReturnInvoiceItemsFromProcessedSalesForReturn(returnInvoice.varInvoiceNumber, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                GrdInvoicedItems.DataBind();

                returnInvoice = IM.CallReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
                IM.CalculateNewInvoiceReturnTotalsToUpdate(returnInvoice, objPageDetails);

                GrdReturningItems.DataSource = returnInvoice.invoiceItems;
                GrdReturningItems.DataBind();

                //recalculate the return total
                lblReturnSubtotalDisplay.Text = "$ " + returnInvoice.fltSubTotal.ToString("#0.00");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
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