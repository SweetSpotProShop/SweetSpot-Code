using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class LoungeSalesCart : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly CustomerManager CM = new CustomerManager();
        //readonly LocationManager LM = new LocationManager();
        readonly ItemsManager IM = new ItemsManager();
        readonly InvoiceManager InM = new InvoiceManager();
        readonly InvoiceItemsManager IIM = new InvoiceItemsManager();
        CurrentUser CU;
        //private static List<Invoice> invoices;
        //private static List<InvoiceItems> programmed;

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
                        List<InvoiceItems> programmed = IM.ReturnProgrammedSaleableItems(objPageDetails);
                        Session["programButtons"] = programmed;
                        ButtonCheck(Page);

                        if (!CU.isSimEditMode)
                        {

                            List<Invoice> invoices = InM.CallGatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), CU.location.intProvinceID, objPageDetails);

                            if (Convert.ToBoolean(Request.QueryString["MTI"].ToString()))
                            {
                                if (invoices.Count < 2)
                                {
                                    //Create master
                                    InM.CreateNewInvoiceAtTable(Request.QueryString["pressedBTN"].ToString(), CU, objPageDetails);
                                    //and first invoice
                                    //change pressed btn to seat one
                                    string[] btns = InM.GatherTableForFirstPlayerInvoice(Request.QueryString["pressedBTN"].ToString());
                                    string one = "One";
                                    if (btns.Contains("Sim"))
                                    {
                                        one = "Player" + one;
                                    }
                                    else
                                    {
                                        one = "Seat" + one;
                                    }
                                    InM.CreateNewInvoiceAtTable(btns[0] + one, CU, objPageDetails);

                                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                                    nameValues.Set("pressedBTN", one);
                                    nameValues.Set("MTI", "false");
                                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);

                                    //display first invoice as single
                                    //if (!CM.isGuestCustomer(invoices[0].customer.intCustomerID, invoices[0].location.intLocationID, objPageDetails))
                                    //{
                                    //    txtCustomer.Text = invoices[0].customer.varFirstName + " " + invoices[0].customer.varLastName;
                                    //}
                                    //else
                                    //{
                                    //    txtCustomerDescription.Text = invoices[0].varAdditionalInformation;
                                    //    txtCustomer.Visible = false;
                                    //    lblCustomer.Visible = false;
                                    //    txtCustomerDescription.Visible = true;
                                    //    lblCustomerDescription.Visible = true;
                                    //}

                                    //lblInvoiceNumberDisplay.Text = invoices[0].varInvoiceNumber.ToString();
                                    //lblDateDisplay.Text = invoices[0].dtmInvoiceDate.ToShortDateString() + " " + invoices[0].dtmInvoiceTime.ToShortTimeString();

                                    //cellMaster.Visible = false;
                                    //cellEachInvoice.Visible = false;
                                    //cellSingleInvoice.Visible = true;

                                }
                                else
                                {
                                    //remove master from list
                                    //display master
                                    //display invoice list

                                    //LocateMaster
                                    int invoiceID = InM.ReturnMasterInvoice(Request.QueryString["pressedBTN"].ToString(), objPageDetails);
                                    Invoice masterInvoice = invoices.SingleOrDefault(r => r.intInvoiceID == invoiceID);
                                    if (masterInvoice != null)
                                    {
                                        invoices.Remove(masterInvoice);
                                    }

                                    GrdMasterCartItems.DataSource = masterInvoice.invoiceItems;
                                    GrdMasterCartItems.DataBind();
                                    cellMaster.Visible = true;
                                    cellEachInvoice.Visible = true;
                                    cellSingleInvoice.Visible = false;

                                    Session["masterInvoice"] = masterInvoice;
                                    Session["invoiceList"] = invoices;
                                }
                            }
                            else
                            {
                                if (invoices.Count == 0)
                                {
                                    InM.CreateNewInvoiceAtTable(Request.QueryString["pressedBTN"].ToString(), CU, objPageDetails);
                                    invoices = InM.CallGatherInvoicesFromTable(Request.QueryString["pressedBTN"].ToString(), CU.location.intProvinceID, objPageDetails);
                                }

                                txtCustomer.Text = invoices[0].customer.varFirstName + " " + invoices[0].customer.varLastName;
                                txtCustomerDescription.Text = invoices[0].varAdditionalInformation;

                                if (CM.IsGuestCustomer(invoices[0].customer.intCustomerID, invoices[0].location.intLocationID, objPageDetails))
                                {
                                    txtCustomer.Visible = false;
                                    lblCustomer.Visible = false;
                                    txtCustomerDescription.Visible = true;
                                    lblCustomerDescription.Visible = true;
                                }

                                lblInvoiceNumberDisplay.Text = invoices[0].varInvoiceNumber.ToString();
                                lblDateDisplay.Text = invoices[0].dtmInvoiceDate.ToShortDateString() + " " + invoices[0].dtmInvoiceTime.ToShortTimeString();

                                Session["invoiceList"] = invoices;
                                cellMaster.Visible = false;
                                cellEachInvoice.Visible = false;
                                cellSingleInvoice.Visible = true;
                            }
                            UpdateInvoiceTotal(invoices);
                        }
                        else
                        {
                            BtnCancelSale.Visible = false;
                            BtnExitSale.Visible = false;
                            BtnPrintBill.Visible = false;
                            BtnProcessPayment.Visible = false;
                            BtnExit.Visible = true;
                            btnExitFromPageOne.Visible = true;
                            btnExitFromPageTwo.Visible = true;
                            btnExitFromPageThree.Visible = true;
                            btnExitFromPageFour.Visible = true;
                            btnExitFromPageFive.Visible = true;
                            btnExitFromPageSix.Visible = true;
                        }
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
        protected void BtnCustomerSelect_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCustomerSelect_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                if (BtnCustomerSelect.Text == "Cancel")
                {
                    BtnCustomerSelect.Text = "Change Customer";
                    GrdCustomersSearched.Visible = false;
                }
                else
                {
                    GrdCustomersSearched.Visible = true;
                    string searchText = txtCustomer.Text;
                    if (CM.IsGuestCustomer(invoices[0].customer.intCustomerID, invoices[0].location.intLocationID, objPageDetails))
                    {
                        searchText = txtCustomerDescription.Text;
                    }
                    GrdCustomersSearched.DataSource = CM.CallReturnCustomerBasedOnText(searchText, objPageDetails);
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
        //protected void btnSearchCustomers_Click(object sender, EventArgs e)
        //{
        //    string method = "btnSearchCustomers_Click";
        //    object[] objPageDetails = { Session["currPage"].ToString(), method };
        //    try
        //    {
        //        grdCustomersSearched.Visible = true;
        //        grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text, objPageDetails);
        //        grdCustomersSearched.DataBind();
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
        //        //Display message box
        //        MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        protected void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            string method = "BtnAddCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Location location = LM.ReturnLocation(CU.location.intLocationID, objPageDetails)[0];
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
                    varCityName = "",
                    intProvinceID = CU.location.intProvinceID,
                    intCountryID = CU.location.intCountryID,
                    varPostalCode = ""
                };
                int customerNumber = CM.CallAddCustomer(customer, objPageDetails);
                customer.intCustomerID = customerNumber;

                List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                invoices[0].customer = customer;
                InM.CallUpdateCurrentInvoice(invoices[0], objPageDetails);
                Session["invoiceList"] = invoices;

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("pressedBTN", Request.QueryString["pressedBTN"].ToString());
                nameValues.Set("MTI", Request.QueryString["MTI"].ToString());
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
                    List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                    Customer customer = CM.CallReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails)[0];
                    invoices[0].customer = customer;
                    InM.CallUpdateCurrentInvoice(invoices[0], objPageDetails);
                    Session["invoiceList"] = invoices;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
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
                List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                int invoiceItemID = Convert.ToInt32(((Label)GrdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                //Remove the item from table;
                IIM.RemoveFromLoungeSimCart(invoiceItemID, invoices[0].dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                //Remove the indexed pointer
                GrdCartItems.EditIndex = -1;
                UpdateInvoiceTotal(invoices);
                Session["invoiceList"] = invoices;
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
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowEditing";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdCartItems.EditIndex = e.NewEditIndex;
                //UpdateInvoiceTotal();
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
        //Currently used for cancelling the edit
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string method = "ORowCanceling";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Clears the indexed row
                GrdCartItems.EditIndex = -1;
                //UpdateInvoiceTotal();
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
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string method = "OnRowUpdating";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                //Stores all the data for each element in the row
                InvoiceItems newItemInfo = new InvoiceItems
                {
                    intInvoiceID = Convert.ToInt32(invoices[0].intInvoiceID),
                    intInvoiceItemID = Convert.ToInt32(((Label)GrdCartItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text),
                    fltItemDiscount = Convert.ToDouble(((TextBox)GrdCartItems.Rows[e.RowIndex].Cells[6].FindControl("txtAmnt")).Text),
                    intItemQuantity = Convert.ToInt32(((TextBox)GrdCartItems.Rows[e.RowIndex].Cells[3].Controls[0]).Text),
                    bitIsDiscountPercent = ((CheckBox)GrdCartItems.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked,
                    intItemTypeID = Convert.ToInt32(((Label)GrdCartItems.Rows[e.RowIndex].Cells[8].FindControl("lblTypeID")).Text)
                };
                //Clears the indexed row
                GrdCartItems.EditIndex = -1;

                IIM.CallUpdateSimItemFromCurrentSalesTableActualQuery(newItemInfo, objPageDetails);
                //Recalculates the new subtotal and Binds cart items to grid view
                UpdateInvoiceTotal(invoices);

                Session["invoiceList"] = invoices;
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
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (!Convert.ToBoolean(Request.QueryString["MTI"].ToString()))
                {
                    List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                    invoices[0].varAdditionalInformation = txtCustomerDescription.Text;
                    invoices[0].intTransactionTypeID = 7;
                    InM.CallUpdateCurrentInvoice(invoices[0], objPageDetails);
                }
                CU.isSimEditMode = false;
                Session["currentUser"] = CU;
                Response.Redirect("LoungeSims.aspx", false);
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
        protected void BtnPrintBill_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnPrintBill_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //need access to printer to see how to code a small bill print for this
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
        protected void BtnProcessPayment_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnProcessPayment_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Not sure if this should then go to the checkout screen or just have an option to select payment
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
        protected void UpdateInvoiceTotal(List<Invoice> invoices)
        {
            string method = "UpdateInvoiceTotal";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (Convert.ToBoolean(Request.QueryString["MTI"].ToString()))
                {
                    RptEachInvoice.DataSource = invoices;
                    RptEachInvoice.DataBind();
                }
                else
                {
                    GrdCartItems.DataSource = invoices[0].invoiceItems;
                    GrdCartItems.DataBind();
                }
                for (int i = 0; i <= invoices.Count - 1; ++i)
                {
                    InM.CalculateNewInvoiceTotalsToUpdate(InM.CallReturnCurrentInvoice(invoices[i].intInvoiceID, CU.location.intProvinceID, objPageDetails)[0], objPageDetails);
                    invoices[i] = InM.CallReturnCurrentInvoice(invoices[i].intInvoiceID, CU.location.intProvinceID, objPageDetails)[0];
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnItemSelectionPageOne_Click(object sender, EventArgs e)
        {
            string method = "BtnItemSelectionPageOne_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 1);
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
        protected void BtnItemSelectionPageTwo_Click(object sender, EventArgs e)
        {
            string method = "BtnItemSelectionPageTwo_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 2);
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
        protected void BtnItemSelectionPageThree_Click(object sender, EventArgs e)
        {
            string method = "BtnItemSelectionPageThree_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 3);
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
        protected void BtnItemSelectionPageFour_Click(object sender, EventArgs e)
        {
            string method = "BtnItemSelectionPageFour_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 4);
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
        protected void BtnItemSelectionPageFive_Click(object sender, EventArgs e)
        {
            string method = "BtnItemSelectionPageFive_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 5);
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
        protected void BtnItemSelectionPageSix_Click(object sender, EventArgs e)
        {
            string method = "BtnItemSelectionPageSix_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                PageButtonCheck((Button)sender, 6);
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
        protected void BtnExit_Click(object sender, EventArgs e)
        {
            string method = "BtnExitSale_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                CU.isSimEditMode = false;
                Session["currentUser"] = CU;
                Response.Redirect("LoungeSims.aspx", false);
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
        protected void BtnBack_Click(object sender, EventArgs e)
        {
            string method = "BtnBack_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Session["selectedProgramButton"] = null;
                tblSelectItem.Visible = false;
                txtSearchText.Visible = false;
                txtSearchText.Text = "";
                BtnSearchText.Visible = false;
                GrdSelectItem.Visible = false;
                mvApplicableServices.ActiveViewIndex = 0;
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
        protected void BtnSelectedItem_Click(object sender, EventArgs e)
        {
            string method = "BtnSelectedItem_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                Button selectedItem = (Button)sender;
                //if (selectedItem.Text == "Program Button")
                if (CU.isSimEditMode)
                {
                    //View Item Selection to set
                    Session["selectedProgramButton"] = selectedItem.ID.ToString();
                    tblSelectItem.Visible = true;
                    txtSearchText.Visible = true;
                    BtnSearchText.Visible = true;
                    GrdSelectItem.Visible = true;
                }
                else
                {
                    //gather text of item
                    //use text to make db call
                    //this will return the item number
                    //once item number returned can be added to sale

                    // if master is true add to master invoice
                    List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                    InvoiceItems selectedSku = IIM.CallGatherLoungeSimToAddToInvoice(selectedItem.ID.ToString(), Convert.ToInt32(invoices[0].intInvoiceID), objPageDetails);
                    selectedSku.intInvoiceID = invoices[0].intInvoiceID;
                    selectedSku.fltItemDiscount = 0;
                    selectedSku.fltItemRefund = 0;
                    selectedSku.bitIsDiscountPercent = false;
                    selectedSku.bitIsClubTradeIn = false;
                    //add item to table and remove the added qty from current inventory
                    //IIM.InsertItemIntoSalesCart(selectedSku, invoices[0].intTransactionTypeID, invoices[0].dtmInvoiceDate, CU, objPageDetails);
                    UpdateInvoiceTotal(invoices);
                    Session["invoiceList"] = invoices;
                    mvApplicableServices.ActiveViewIndex = 0;
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
        protected void BtnSearchText_Click(object sender, EventArgs e)
        {
            string method = "BtnSearchText_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                List<InvoiceItems> searched = IM.CallReturnInvoiceItemsFromForLoungeSim(txtSearchText.Text, objPageDetails);
                GrdSelectItem.DataSource = searched;
                GrdSelectItem.DataBind();
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
        protected void GrdSelectItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "GrdSelectItem_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.ProgramLoungeSimButton(Convert.ToInt32(e.CommandArgument.ToString()), "", Session["selectedProgramButton"].ToString(), objPageDetails);
                Session["selectedProgramButton"] = null;
                tblSelectItem.Visible = false;
                txtSearchText.Visible = false;
                txtSearchText.Text = "";
                BtnSearchText.Visible = false;
                GrdSelectItem.Visible = false;
                GrdSelectItem.DataSource = null;
                GrdSelectItem.DataBind();
                List<InvoiceItems> programmed = IM.ReturnProgrammedSaleableItems(objPageDetails);
                Session["programButtons"] = programmed;
                ButtonCheck(Page);
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
        private void ButtonCheck(Control control)
        {
            List<InvoiceItems> programmed = (List<InvoiceItems>)Session["programButtons"];
            foreach (Control c in control.Controls)
            {
                if (c is Button)
                {
#pragma warning disable IDE0020 // Use pattern matching
                    Button btn = (Button)c;
#pragma warning restore IDE0020 // Use pattern matching
                    foreach (InvoiceItems i in programmed)
                    {
                        if (btn.ID.ToString() == i.varAdditionalInformation.ToString())
                        {
                            btn.Text = i.varItemDescription;
                        }
                    }
                    if (!CU.isSimEditMode)
                    {
                        if (btn.Text == "Program Button")
                        {
                            btn.Visible = false;
                        }
                    }
                }
                else if (c.HasControls())
                {
                    ButtonCheck(c);
                }
            }
        }
        protected void BtnEditButtonText_Click(object sender, EventArgs e)
        {
            string method = "BtnEditButtonText_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                IM.ProgramLoungeSimButton(0, txtButtonText.Text, Session["selectedProgramButton"].ToString(), objPageDetails);
                Session["selectedProgramButton"] = null;
                tblSelectItem.Visible = false;
                txtButtonText.Visible = false;
                txtButtonText.Text = "";
                BtnEditButtonText.Visible = false;
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
                    BtnEditButtonText.Visible = true;
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
        protected void GrdEachCartItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "GrdEachCartItems_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                List<Invoice> invoices = (List<Invoice>)Session["invoiceList"];
                if (e.CommandName.ToString() == "R")
                {
                    IIM.DeleteItemFromCurrentSalesTable(Convert.ToInt32(e.CommandArgument.ToString()), invoices[0].dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);
                }
                UpdateInvoiceTotal(invoices);
                Session["invoiceList"] = invoices;
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
        protected void GrdMasterCartItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "GrdMasterCartItems_RowCommand";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {

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
        protected void RptEachInvoice_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string method = "RptEachInvoice_ItemDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (CM.IsGuestCustomer(Convert.ToInt32(((HiddenField)(e.Item.FindControl("hidRepeatCustomerID"))).Value), CU.location.intLocationID, objPageDetails))
                {
                    txtCustomer.Visible = false;
                    lblCustomer.Visible = false;
                    txtCustomerDescription.Visible = true;
                    lblCustomerDescription.Visible = true;
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
    }
}
