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
        private static List<Invoice> invoices;
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
                        programmed = IM.ReturnProgrammedSaleableItems(objPageDetails);
                        ButtonCheck(Page);

                        if (!CU.isSimEditMode)
                        {
                            invoices = InM.GatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), CU.location.intProvinceID, objPageDetails);
                            if (invoices.Count == 0)
                            {
                                InM.CreateNewInvoiceAtTable(Request.QueryString["pressedBTN"].ToString(), CU, objPageDetails);
                                invoices = InM.GatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), CU.location.intProvinceID, objPageDetails);
                                //txtCustomer.Text = invoices[0].customer.varFirstName + " " + invoices[0].customer.varLastName;
                                //lblInvoiceNumberDisplay.Text = invoices[0].varInvoiceNumber.ToString();
                                //lblDateDisplay.Text = invoices[0].dtmInvoiceDate.ToShortDateString() + " " + invoices[0].dtmInvoiceTime.ToShortTimeString();
                                cellMaster.Visible = false;
                                cellEachInvoice.Visible = false;
                                cellSingleInvoice.Visible = true;
                            }
                            else if (invoices.Count == 1)
                            {
                                //invoices = InM.GatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), CU.location.intProvinceID, objPageDetails);
                                if (!CM.isGuestCustomer(invoices[0].customer.intCustomerID, invoices[0].location.intLocationID, objPageDetails))
                                {
                                    txtCustomer.Text = invoices[0].customer.varFirstName + " " + invoices[0].customer.varLastName;
                                }
                                else
                                {
                                    txtCustomerDescription.Text = invoices[0].varAdditionalInformation;
                                    txtCustomer.Visible = false;
                                    lblCustomer.Visible = false;
                                    txtCustomerDescription.Visible = true;
                                    lblCustomerDescription.Visible = true;
                                }

                                lblInvoiceNumberDisplay.Text = invoices[0].varInvoiceNumber.ToString();
                                lblDateDisplay.Text = invoices[0].dtmInvoiceDate.ToShortDateString() + " " + invoices[0].dtmInvoiceTime.ToShortTimeString();

                                cellMaster.Visible = false;
                                cellEachInvoice.Visible = false;
                                cellSingleInvoice.Visible = true;
                            }
                            else
                            {
                                cellMaster.Visible = true;
                                cellEachInvoice.Visible = true;
                                cellSingleInvoice.Visible = false;
                            }
                            UpdateInvoiceTotal();
                        }
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
                    string searchText = txtCustomer.Text;
                    if (CM.isGuestCustomer(invoices[0].customer.intCustomerID, invoices[0].location.intLocationID, objPageDetails))
                    {
                        searchText = txtCustomerDescription.Text;
                    }
                    grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(searchText, objPageDetails);
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
                invoices[0].customer = customer;
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
                    invoices[0].customer = customer;
                    InM.UpdateCurrentInvoice(invoices[0], objPageDetails);
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
                int invoiceItemID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                //Remove the item from table;
                IIM.RemoveFromLoungeSimCart(invoiceItemID, invoices[0].dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
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
                newItemInfo.intInvoiceID = Convert.ToInt32(invoices[0].intInvoiceID);
                newItemInfo.intInvoiceItemID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                newItemInfo.fltItemDiscount = Convert.ToDouble(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("txtAmnt")).Text);
                newItemInfo.intItemQuantity = Convert.ToInt32(((TextBox)grdCartItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                newItemInfo.bitIsDiscountPercent = ((CheckBox)grdCartItems.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked;
                newItemInfo.intItemTypeID = Convert.ToInt32(((Label)grdCartItems.Rows[e.RowIndex].Cells[8].FindControl("lblTypeID")).Text);
                //Clears the indexed row
                grdCartItems.EditIndex = -1;

                IIM.UpdateSimItemFromCurrentSalesTableActualQuery(newItemInfo, objPageDetails);
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
                CU.isSimEditMode = false;
                Session["currentUser"] = CU;
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
                invoices[0].varAdditionalInformation = txtCustomerDescription.Text;
                invoices[0].intTransactionTypeID = 7;
                InM.UpdateCurrentInvoice(invoices[0], objPageDetails);
                CU.isSimEditMode = false;
                Session["currentUser"] = CU;
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
        protected void btnPrintBill_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //need access to printer to see how to code a small bill print for this
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
        protected void btnProcessPayment_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancelSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Not sure if this should then go to the checkout screen or just have an option to select payment
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
                rptEachInvoice.DataSource = invoices;
                rptEachInvoice.DataBind();
                for (int i = 0; i <= invoices.Count - 1; ++i)
                {
                    InM.CalculateNewInvoiceTotalsToUpdate(InM.ReturnCurrentInvoice(invoices[i].intInvoiceID, CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                    invoices[i] = InM.ReturnCurrentInvoice(invoices[i].intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
                    grdCartItems.DataSource = invoices[i].invoiceItems;
                    grdCartItems.DataBind();
                    lblLiquorTaxDisplay.Text = invoices[i].fltLiquorTaxAmount.ToString("C");
                    lblGovernmentTaxDisplay.Text = invoices[i].fltGovernmentTaxAmount.ToString("C");
                    lblProvincialTaxDisplay.Text = invoices[i].fltProvincialTaxAmount.ToString("C");
                    lblSubtotalDisplay.Text = "$ " + invoices[i].fltSubTotal.ToString("#0.00");
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

        protected void btnItemSelectionPageOne_Click(object sender, EventArgs e)
        {
            string method = "btnItemSelectionPageOne_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 1);
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
                PageButtonCheck((Button)sender, 2);
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
                PageButtonCheck((Button)sender, 3);
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
                PageButtonCheck((Button)sender, 4);
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
                PageButtonCheck((Button)sender, 5);
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
                PageButtonCheck((Button)sender, 6);
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

        //protected void btnExit_Click(object sender, EventArgs e)
        //{
        //    string method = "btnExitSale_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        CU.isSimEditMode = false;
        //        Session["currentUser"] = CU;
        //        Response.Redirect("LoungeSims.aspx", false);
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}

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
                //if (selectedItem.Text == "Program Button")
                if(CU.isSimEditMode)
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
                    InvoiceItems selectedSku = IIM.GatherLoungeSimToAddToInvoice(selectedItem.ID.ToString(), Convert.ToInt32(invoices[0].intInvoiceID), objPageDetails);
                    selectedSku.intInvoiceID = invoices[0].intInvoiceID;
                    selectedSku.fltItemDiscount = 0;
                    selectedSku.fltItemRefund = 0;
                    selectedSku.bitIsDiscountPercent = false;
                    selectedSku.bitIsClubTradeIn = false;
                    //add item to table and remove the added qty from current inventory
                    IIM.InsertItemIntoSalesCart(selectedSku, invoices[0].intTransactionTypeID, invoices[0].dtmInvoiceDate, CU, objPageDetails);
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
                IM.ProgramLoungeSimButton(Convert.ToInt32(e.CommandArgument.ToString()), "", Session["selectedProgramButton"].ToString(), objPageDetails);
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
                    if (!CU.isSimEditMode)
                    {
                        if (button.Text == "Program Button")
                        {
                            button.Visible = false;
                        }
                    }
                }
                else if (c.HasControls())
                {
                    ButtonCheck(c);
                }
            }
        }
        protected void btnEditButtonText_Click(object sender, EventArgs e)
        {
            string method = "btnEditButtonText_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.ProgramLoungeSimButton(0, txtButtonText.Text, Session["selectedProgramButton"].ToString(), objPageDetails);
                Session["selectedProgramButton"] = null;
                tblSelectItem.Visible = false;
                txtButtonText.Visible = false;
                txtButtonText.Text = "";
                btnEditButtonText.Visible = false;
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
        private void PageButtonCheck(Button selectedItem, int activeWindow)
        {
             if (CU.isSimEditMode)
                {
                    if (selectedItem.Text == "Program Button")
                    {
                        //View Item Selection to set
                        Session["selectedProgramButton"] = selectedItem.ID.ToString();
                        tblSelectItem.Visible = true;
                        txtButtonText.Visible = true;
                        btnEditButtonText.Visible = true;
                    }
                    else
                    {
                        mvApplicableServices.ActiveViewIndex = activeWindow;
                    }
                }
                else
                {
                    mvApplicableServices.ActiveViewIndex = activeWindow;
                }
        }

        protected void grdEachCartItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "grdEachCartItems_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if(e.CommandName.ToString() == "R")
                {
                    IIM.DeleteItemFromCurrentSalesTable(Convert.ToInt32(e.CommandArgument.ToString()), invoices[0].dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                }
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
    }
}