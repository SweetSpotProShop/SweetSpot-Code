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
        CurrentUser CU;
        InvoiceManager IM = new InvoiceManager();
        InvoiceItemsManager IIM = new InvoiceItemsManager();
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
                        List<Invoice> returnInvoicesCalled = IM.ReturnInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails);
                        Invoice returnInvoice = new Invoice();
                        if (returnInvoicesCalled.Count > 0)
                        {
                            returnInvoice = returnInvoicesCalled[0];
                            returnInvoice.intInvoiceSubNumber = IM.CalculateNextInvoiceSubNum(returnInvoice.varInvoiceNumber, objPageDetails);
                            returnInvoice.location = CU.location;
                            returnInvoice.employee = CU.employee;
                            returnInvoice.intTransactionTypeID = 2;
                            returnInvoice = IM.CreateInitialTotalsForTable(returnInvoice, objPageDetails)[0];

                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("invoice", returnInvoice.intInvoiceID.ToString());
                            Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                        }
                        else
                        {
                            returnInvoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                        }
                        lblCustomerDisplay.Text = returnInvoice.customer.varFirstName.ToString() + " " + returnInvoice.customer.varLastName.ToString();
                        lblInvoiceNumberDisplay.Text = returnInvoice.varInvoiceNumber + "-" + returnInvoice.intInvoiceSubNumber;
                        lblDateDisplay.Text = returnInvoice.dtmInvoiceDate.ToString("dd/MMM/yy");
                        //binds items in cart to gridview
                        grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(returnInvoice.varInvoiceNumber.ToString(),returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                        grdInvoicedItems.DataBind();

                        grdReturningItems.DataSource = returnInvoice.invoiceItems;
                        grdReturningItems.DataBind();

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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                IIM.LoopThroughTheItemsToReturnToInventory(returnInvoice.intInvoiceID, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                IIM.RemoveInitialTotalsForTable(returnInvoice.intInvoiceID, objPageDetails);
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
        protected void btnProceedToReturnCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProceedToCheckout_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                if (IIM.CheckForItemsInTransaction(IM.ReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0]))
                {
                    lblInvalidQty.Visible = false;
                    //Changes page to the returns checkout page
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("invoice", returnInvoice.intInvoiceID.ToString());
                    Response.Redirect("ReturnsCheckout.aspx?" + nameValues, false);
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
        protected void grdInvoicedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoicedItems_RowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                //Stores the info about the item in that index
                InvoiceItems selectedSku = IIM.ReturnInvoiceItemForReturnProcess(Convert.ToInt32(((Label)grdInvoicedItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text), objPageDetails);
                selectedSku.intInvoiceID = Convert.ToInt32(returnInvoice.intInvoiceID);
                if (!IIM.ItemAlreadyInCart(selectedSku, objPageDetails))
                {
                    int currentQTY = selectedSku.intItemQuantity;
                    string quantityForReturn = ((TextBox)grdInvoicedItems.Rows[e.RowIndex].Cells[2].FindControl("quantityToReturn")).Text;
                    int quantitySold = Convert.ToInt32(((Label)grdInvoicedItems.Rows[e.RowIndex].Cells[2].FindControl("quantitySold")).Text);
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
                        string returnAmount = ((TextBox)grdInvoicedItems.Rows[e.RowIndex].Cells[7].FindControl("txtReturnAmount")).Text;
                        if(returnAmount != "")
                        {
                            if(double.TryParse(returnAmount, out returnDollars))
                            {
                                returnDollars = Convert.ToDouble(returnAmount);
                            }
                        }
                        IIM.RemoveQTYFromInventoryWithSKU(selectedSku.intInventoryID, selectedSku.intItemTypeID, (currentQTY + returnQuantity), objPageDetails);
                        selectedSku.intItemQuantity = returnQuantity;
                        selectedSku.fltItemRefund = -1 * returnDollars;
                        selectedSku.fltItemCost = selectedSku.fltItemCost * -1;
                        IIM.InsertItemIntoSalesCart(selectedSku, returnInvoice.intTransactionTypeID, returnInvoice.dtmInvoiceDate, CU, objPageDetails);
                        //deselect the indexed item
                        grdInvoicedItems.EditIndex = -1;
                        //store items available for return in session
                        grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(returnInvoice.varInvoiceNumber.ToString(), returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                        grdInvoicedItems.DataBind();

                        IM.CalculateNewInvoiceReturnTotalsToUpdate(IM.ReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                        returnInvoice = IM.ReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];

                        grdReturningItems.DataSource = returnInvoice.invoiceItems;
                        grdReturningItems.DataBind();

                        //recalculate the return total
                        lblReturnSubtotalDisplay.Text = "$ " + returnInvoice.fltSubTotal.ToString("#0.00");
                    }
                }
                else
                {
                    MessageBox.ShowMessage("Same item cannot be returned for a different amount. "
                         + "Either cancel item to set both at new return amount or process a second return.", this);
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
        protected void grdReturningItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdReturningItems_RowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Invoice returnInvoice = IM.ReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];
                lblInvalidQty.Visible = false;
                //Gathers index from selected line item
                int index = e.RowIndex;
                //Stores the info about the item in that index
                int invoiceItemReturnID = Convert.ToInt32(((Label)grdReturningItems.Rows[index].Cells[0].FindControl("lblInvoiceItemReturnID")).Text);
                InvoiceItems selectedSku = IIM.ReturnSkuFromCurrentSalesUsingSKU(invoiceItemReturnID, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);

                //add item to table and remove the added qty from current inventory
                IIM.DoNotReturnTheItemOnReturn(selectedSku, objPageDetails);
                IIM.RemoveQTYFromInventoryWithSKU(selectedSku.intInventoryID, selectedSku.intItemTypeID, (IIM.ReturnCurrentQuantityOfItem(selectedSku, objPageDetails) - selectedSku.intItemQuantity), objPageDetails);
                //deselect the indexed item
                grdReturningItems.EditIndex = -1;
                //Check if the marked for returns cart has any items in it
                grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(returnInvoice.varInvoiceNumber, returnInvoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                grdInvoicedItems.DataBind();

                returnInvoice = IM.ReturnCurrentInvoice(returnInvoice.intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
                IM.CalculateNewInvoiceReturnTotalsToUpdate(returnInvoice, objPageDetails);

                grdReturningItems.DataSource = returnInvoice.invoiceItems;
                grdReturningItems.DataBind();

                //recalculate the return total
                lblReturnSubtotalDisplay.Text = "$ " + returnInvoice.fltSubTotal.ToString("#0.00");
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