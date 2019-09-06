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
    public partial class LoungeSalesCart : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CustomerManager CM = new CustomerManager();
        LocationManager LM = new LocationManager();
        ItemsManager IM = new ItemsManager();
        InvoiceManager InM = new InvoiceManager();
        InvoiceItemsManager IIM = new InvoiceItemsManager();
        CurrentUser CU;
        private static Invoice invoice;
        private static List<InvoiceItems> programmed;

        //Still need to account for a duplicate item being added
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "LoungeSalesCart.aspx";
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
                        mvApplicableServices.ActiveViewIndex = 0;
                        programmed = IM.ReturnProgrammedSaleableItems();
                        ButtonCheck(Page);

                        List<Invoice> invoices = InM.GatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), objPageDetails);
                        if (invoices.Count == 0)
                        {
                            InM.CreateNewInvoiceAtTable(Request.QueryString["pressedBTN"].ToString(), CU, objPageDetails);
                            invoices = InM.GatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), objPageDetails);
                        }
                        invoice = invoices[0];
                        UpdateInvoiceTotal();
                        txtCustomer.Text = invoice.customer.varFirstName + " " + invoice.customer.varLastName;
                        lblInvoiceNumberDisplay.Text = invoice.intInvoiceID + "-" + invoice.intInvoiceSubNumber;
                        lblDateDisplay.Text = invoice.dtmInvoiceDate.ToShortDateString() + " " + invoice.dtmInvoiceTime.ToShortTimeString();
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
                Location location = LM.ReturnLocation(CU.location.intLocationID, objPageDetails)[0];
                Customer customer = new Customer();
                customer.varFirstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text;
                customer.varLastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text;
                customer.varAddress = "";
                customer.secondaryAddress = "";
                customer.varContactNumber = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtPhoneNumber")).Text;
                customer.secondaryPhoneNumber = "";
                customer.bitSendMarketing = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkMarketingEnrollment")).Checked;
                customer.varEmailAddress = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmail")).Text;
                customer.varCityName = "";
                customer.intProvinceID = location.intProvinceID;
                customer.intCountryID = location.intCountryID;
                customer.varPostalCode = "";
                int customerNumber = CM.addCustomer(customer, objPageDetails);
                customer.intCustomerID = customerNumber;
                invoice.customer = customer;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("pressedBTN", Request.QueryString["pressedBTN"].ToString());
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
                    Customer customer = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    invoice.customer = customer;
                    InM.UpdateCurrentInvoice(invoice, objPageDetails);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("pressedBTN", Request.QueryString["pressedBTN"].ToString());
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
        //Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowDeleting";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {                
                int sku = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[2].FindControl("")).Text);
                //Remove the item from table;
                IIM.RemoveFromLoungeSimCart(sku, (CU.location.varLocationName + "-" + invoice.varInvoiceNumber + "-" + invoice.intInvoiceSubNumber).ToString(), objPageDetails);
                //Remove the indexed pointer
                grdCartItems.EditIndex = -1;
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
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowEditing";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                grdCartItems.EditIndex = e.NewEditIndex;
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
        //Currently used for cancelling the edit
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "ORowCanceling";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Clears the indexed row
                grdCartItems.EditIndex = -1;
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
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowUpdating";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Stores all the data for each element in the row
                InvoiceItems newItemInfo = new InvoiceItems();
                newItemInfo.intInvoiceID = Convert.ToInt32(invoice.intInvoiceID);                
                newItemInfo.intInventoryID = Convert.ToInt32(grdCartItems.Rows[e.RowIndex].Cells[2].Text);
                newItemInfo.fltItemDiscount = Convert.ToDouble(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("txtAmnt")).Text);
                newItemInfo.intItemQuantity = Convert.ToInt32(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                newItemInfo.bitIsDiscountPercent = ((CheckBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked;
                newItemInfo.intItemTypeID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[8].FindControl("lblTypeID")).Text);
                //Clears the indexed row
                grdCartItems.EditIndex = -1;
                IIM.UpdateItemFromCurrentSalesTable(newItemInfo, objPageDetails);
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

        protected void UpdateInvoiceTotal()
        {
            string method = "UpdateInvoiceTotal";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InM.CalculateNewInvoiceTotalsToUpdate(InM.ReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0], objPageDetails);
                invoice = InM.ReturnCurrentInvoice(invoice.intInvoiceID, objPageDetails)[0];
                grdCartItems.DataSource = invoice.invoiceItems;
                grdCartItems.DataBind();
                lblSubtotalDisplay.Text = "$ " + invoice.fltSubTotal.ToString("#0.00");
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

        protected void btnItemSelectionPageOne_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageOne_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                mvApplicableServices.ActiveViewIndex = 1;
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

        protected void btnItemSelectionPageTwo_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageTwo_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                mvApplicableServices.ActiveViewIndex = 2;
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

        protected void btnItemSelectionPageThree_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageThree_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                mvApplicableServices.ActiveViewIndex = 3;
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

        protected void btnItemSelectionPageFour_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageFour_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                mvApplicableServices.ActiveViewIndex = 4;
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

        protected void btnItemSelectionPageFive_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageFive_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                mvApplicableServices.ActiveViewIndex = 5;
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

        protected void btnItemSelectionPageSix_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageSix_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                mvApplicableServices.ActiveViewIndex = 6;
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

        protected void btnExit_Click(object sender, EventArgs e)
        {
            string method = "btnExitSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Response.Redirect("LoungeSims.aspx", false);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string method = "btnBack_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Session["selectedProgramButton"] = null;
                tblSelectItem.Visible = false;
                txtSearchText.Visible = false;
                txtSearchText.Text = "";
                btnSearchText.Visible = false;
                grdSelectItem.Visible = false;
                mvApplicableServices.ActiveViewIndex = 0;
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

        protected void btnSelectedItem_Click(object sender, EventArgs e)
        {
            string method = "btnSelectedItem_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Button selectedItem = (Button)sender;
                if (selectedItem.Text == "Program Button")
                {
                    //View Item Selection to set
                    Session["selectedProgramButton"] = selectedItem.ID.ToString();
                    tblSelectItem.Visible = true;
                    txtSearchText.Visible = true;
                    btnSearchText.Visible = true;
                    grdSelectItem.Visible = true;
                }
                else
                {
                    //gather text of item
                    //use text to make db call
                    //this will return the item number
                    //once item number returned can be added to sale
                    InvoiceItems selectedSku = IIM.GatherLoungeSimToAddToInvoice(selectedItem.ID.ToString(), Convert.ToInt32(invoice.intInvoiceID));
                    //add item to table and remove the added qty from current inventory
                    IIM.InsertItemIntoSalesCart(selectedSku, objPageDetails);
                    UpdateInvoiceTotal();
                    mvApplicableServices.ActiveViewIndex = 0;
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

        protected void btnSearchText_Click(object sender, EventArgs e)
        {
            string method = "btnSearchText_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                List<InvoiceItems> searched = IM.ReturnInvoiceItemsFromForLoungeSim(txtSearchText.Text, objPageDetails);
                grdSelectItem.DataSource = searched;
                grdSelectItem.DataBind();
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

        protected void grdSelectItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "grdSelectItem_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.ProgramLoungeSimButton(Convert.ToInt32(e.CommandArgument.ToString()), Session["selectedProgramButton"].ToString());
                Session["selectedProgramButton"] = null;
                tblSelectItem.Visible = false;
                txtSearchText.Visible = false;
                txtSearchText.Text = "";
                btnSearchText.Visible = false;
                grdSelectItem.Visible = false;
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
        private void ButtonCheck(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is Button)
                {
                    Button button = (Button)c;
                    foreach (InvoiceItems i in programmed)
                    {
                        if (button.ID.ToString() == i.varAdditionalInformation.ToString())
                        {
                            button.Text = i.varItemDescription;
                        }
                    }
                }
                else if (c.HasControls())
                {
                    ButtonCheck(c);
                }
            }
        }
    }
}