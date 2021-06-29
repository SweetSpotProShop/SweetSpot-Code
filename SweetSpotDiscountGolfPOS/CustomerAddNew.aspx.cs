using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;
using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerAddNew : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly CustomerManager CM = new CustomerManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "CustomerAddNew.aspx";
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
                    //Checks for a Customer Key
                    if (Convert.ToInt32(Request.QueryString["customer"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create customer class and fill page with all info based in the customer number 
                            Customer customer = CM.CallReturnCustomerWithInvoiceList(Convert.ToInt32(Request.QueryString["customer"].ToString()), objPageDetails)[0];

                            txtFirstName.Text = customer.varFirstName.ToString();
                            txtLastName.Text = customer.varLastName.ToString();
                            txtPrimaryAddress.Text = customer.varAddress.ToString();
                            txtSecondaryAddress.Text = customer.secondaryAddress.ToString();
                            txtPrimaryPhoneNumber.Text = customer.varContactNumber.ToString();
                            txtSecondaryPhoneNumber.Text = customer.secondaryPhoneNumber.ToString();
                            txtEmail.Text = customer.varEmailAddress.ToString();
                            txtCity.Text = customer.varCityName.ToString();

                            DdlCountry.DataSource = LM.CallReturnCountryDropDown(objPageDetails);
                            DdlCountry.DataBind();
                            DdlCountry.SelectedValue = customer.intCountryID.ToString();
                            ddlProvince.DataSource = LM.CallReturnProvinceDropDown(customer.intCountryID, objPageDetails);
                            ddlProvince.SelectedValue = customer.intProvinceID.ToString();
                            ddlProvince.DataBind();

                            txtPostalCode.Text = customer.varPostalCode.ToString();
                            chkEmailList.Checked = customer.bitSendMarketing;

                            //Binds invoice list to the grid view
                            GrdInvoiceSelection.DataSource = customer.invoices;
                            GrdInvoiceSelection.DataBind();
                        }
                    }
                    else
                    {
                        //no cust number
                        if (!IsPostBack)
                        {
                            DdlCountry.DataSource = LM.CallReturnCountryDropDown(objPageDetails);
                            DdlCountry.DataBind();
                            DdlCountry.SelectedValue = CU.location.intCountryID.ToString();

                            ddlProvince.DataSource = LM.CallReturnProvinceDropDown(0, objPageDetails);
                            ddlProvince.DataBind();
                        }
                        //Displays text boxes instead of label for customer creation info
                        txtFirstName.Enabled = true;
                        txtLastName.Enabled = true;
                        txtPrimaryAddress.Enabled = true;
                        txtSecondaryAddress.Enabled = true;
                        txtPrimaryPhoneNumber.Enabled = true;
                        txtSecondaryPhoneNumber.Enabled = true;
                        txtEmail.Enabled = true;
                        txtCity.Enabled = true;
                        ddlProvince.Enabled = true;
                        DdlCountry.Enabled = true;
                        chkEmailList.Enabled = true;
                        txtPostalCode.Enabled = true;
                        //hides and displays the proper buttons for access
                        BtnSaveCustomer.Visible = false;
                        BtnAddCustomer.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddCustomer";
                        BtnEditCustomer.Visible = false;
                        BtnStartSale.Visible = false;
                        BtnCancel.Visible = false;
                        BtnBackToSearch.Visible = true;
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
        protected void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Collects new customer data to add to database
                Customer customer = new Customer
                {
                    varFirstName = txtFirstName.Text,
                    varLastName = txtLastName.Text,
                    varAddress = txtPrimaryAddress.Text,
                    secondaryAddress = txtSecondaryAddress.Text,
                    varContactNumber = txtPrimaryPhoneNumber.Text,
                    secondaryPhoneNumber = txtSecondaryPhoneNumber.Text,
                    bitSendMarketing = chkEmailList.Checked,
                    varEmailAddress = txtEmail.Text,
                    varCityName = txtCity.Text,
                    intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue),
                    intCountryID = Convert.ToInt32(DdlCountry.SelectedValue),
                    varPostalCode = txtPostalCode.Text
                };

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", CM.CallAddCustomer(customer, objPageDetails).ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void BtnEditCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnEditCustomer_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //transfers data from label into textbox for editing
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtPrimaryAddress.Enabled = true;
                txtSecondaryAddress.Enabled = true;
                txtPrimaryPhoneNumber.Enabled = true;
                txtSecondaryPhoneNumber.Enabled = true;
                txtEmail.Enabled = true;
                chkEmailList.Enabled = true;
                txtCity.Enabled = true;
                DdlCountry.Enabled = true;
                ddlProvince.Enabled = true;
                txtPostalCode.Enabled = true;
                //hides and displays the proper buttons for access
                BtnSaveCustomer.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveCustomer";
                BtnEditCustomer.Visible = false;
                BtnAddCustomer.Visible = false;
                BtnStartSale.Visible = false;
                BtnCancel.Visible = true;
                BtnBackToSearch.Visible = false;
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
        protected void BtnSaveCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnSaveCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Collects customer data to add to database
                Customer customer = new Customer
                {
                    intCustomerID = Convert.ToInt32(Request.QueryString["customer"].ToString()),
                    varFirstName = txtFirstName.Text,
                    varLastName = txtLastName.Text,
                    varAddress = txtPrimaryAddress.Text,
                    secondaryAddress = txtSecondaryAddress.Text,
                    varContactNumber = txtPrimaryPhoneNumber.Text,
                    secondaryPhoneNumber = txtSecondaryPhoneNumber.Text,
                    bitSendMarketing = chkEmailList.Checked,
                    varEmailAddress = txtEmail.Text,
                    varCityName = txtCity.Text,
                    intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue),
                    intCountryID = Convert.ToInt32(DdlCountry.SelectedValue),
                    varPostalCode = txtPostalCode.Text
                };
                //updates the customer info in tables
                CM.CallUpdateCustomer(customer, objPageDetails);
                //changes all text boxes and dropdowns to labels
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtPrimaryAddress.Enabled = false;
                txtSecondaryAddress.Enabled = false;
                txtPrimaryPhoneNumber.Enabled = false;
                txtSecondaryPhoneNumber.Enabled = false;
                txtEmail.Enabled = false;
                chkEmailList.Enabled = false;
                txtCity.Enabled = false;
                ddlProvince.Enabled = false;
                DdlCountry.Enabled = false;
                txtPostalCode.Enabled = false;
                //hides and displays the proper buttons for access
                pnlDefaultButton.DefaultButton = "btnEditCustomer";
                BtnAddCustomer.Visible = false;
                BtnBackToSearch.Visible = true;
                BtnCancel.Visible = false;
                BtnEditCustomer.Visible = true;
                BtnSaveCustomer.Visible = false;
                BtnStartSale.Visible = true;
                
                //reloads current page
                Response.Redirect(Request.RawUrl, false);
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
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCancel_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //no chnages saved and moves to customer home page
                Response.Redirect(Request.RawUrl, false);
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
        protected void BtnStartSale_Click(object sender, EventArgs e)
        {
            //****Still need this updated with new process****
            //Collects current method and page for error tracking
            string method = "BtnStartSale_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", Request.QueryString["customer"].ToString());
                string invoice = "-10";
                nameValues.Set("invoice", invoice);
                //Changes page to Sales Cart
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
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
        protected void BtnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "BtnBackToSearch_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //opens the Customer home page
                Response.Redirect("CustomerHomePage.aspx", false);
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
        protected void GrdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInvoiceSelection_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets the string of the command argument(invoice number
                int invoiceID = Convert.ToInt32(e.CommandArgument.ToString());
                InvoiceManager IM = new InvoiceManager();
                if (IM.InvoiceIsReturn(invoiceID, objPageDetails))
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoiceReturn.aspx?invoice=" + invoiceID.ToString(), false);
                }
                else
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoice.aspx?invoice=" + invoiceID.ToString(), false);
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
        protected void DdlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "DdlCountry_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                ddlProvince.DataSource = LM.CallReturnProvinceDropDown(Convert.ToInt32(DdlCountry.SelectedValue), objPageDetails);
                ddlProvince.DataBind();
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