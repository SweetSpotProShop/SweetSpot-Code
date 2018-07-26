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
                        Invoice I = IM.ReturnInvoiceForReturns(Request.QueryString["inv"].ToString());

                        if (!IM.ReturnBolInvoiceExists(Request.QueryString["inv"].ToString()))
                        {
                            I.invoiceSub = Convert.ToInt32((Request.QueryString["inv"].ToString()).Split('-')[2]);
                            I.shippingAmount = 0;
                            I.subTotal = 0;
                            I.discountAmount = 0;
                            I.tradeinAmount = 0;
                            I.balanceDue = 0;
                            I.location = CU.location;
                            I.employee = CU.emp;
                            IM.CreateInitialTotalsForTable(I);
                        }
                        lblCustomerDisplay.Text = I.customer.firstName.ToString() + " " + I.customer.lastName.ToString();
                        lblInvoiceNumberDisplay.Text = Request.QueryString["inv"].ToString();
                        lblDateDisplay.Text = DateTime.Today.ToString("yyyy-MM-dd");
                        //binds items in cart to gridview
                        grdInvoicedItems.DataSource = IIM.ReturnInvoiceItemsFromProcessedSalesForReturn(Request.QueryString["inv"].ToString());
                        grdInvoicedItems.DataBind();

                        grdReturningItems.DataSource = IIM.ReturnItemsInTheReturnCart(Request.QueryString["inv"].ToString());
                        grdReturningItems.DataBind();

                        IM.CalculateNewInvoiceReturnTotalsToUpdate(IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0]);
                        I = IM.ReturnCurrentInvoice(Request.QueryString["inv"].ToString())[0];
                        lblReturnSubtotalDisplay.Text = "$ " + I.subTotal.ToString("#0.00");
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
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            try
            {
                lblInvalidQty.Visible = false;
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                selectedSku.invoiceSubNum = Convert.ToInt32(Request.QueryString["inv"].Split('-')[2].ToString());
                if (!IIM.ItemAlreadyInCart(selectedSku))
                {
                    int currentQTY = selectedSku.quantity;
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
                        IIM.RemoveQTYFromInventoryWithSKU(selectedSku.sku, selectedSku.typeID, (currentQTY + returnQuantity));
                        selectedSku.quantity = returnQuantity;
                        selectedSku.itemRefund = -1 * returnDollars;
                        selectedSku.cost = selectedSku.cost * -1;
                        IIM.InsertItemIntoSalesCart(selectedSku);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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