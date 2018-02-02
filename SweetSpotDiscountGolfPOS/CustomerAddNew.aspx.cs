using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        CustomerManager CM = new CustomerManager();
        EmployeeManager EM = new EmployeeManager();
        CurrentUser CU;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "CustomerAddNew.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                //Checks for a Customer Key
                if (Convert.ToInt32(Request.QueryString["cust"].ToString()) != -10)
                {
                    if (!IsPostBack)
                    {
                        //Create customer class and fill page with all info based in the customer number 
                        List<Customer> customer = CM.ReturnCustomerWithInvoiceList(Convert.ToInt32(Request.QueryString["cust"].ToString()));

                        txtFirstName.Text = customer[0].firstName.ToString();
                        txtLastName.Text = customer[0].lastName.ToString();
                        txtPrimaryAddress.Text = customer[0].primaryAddress.ToString();
                        txtSecondaryAddress.Text = customer[0].secondaryAddress.ToString();
                        txtPrimaryPhoneNumber.Text = customer[0].primaryPhoneNumber.ToString();
                        txtSecondaryPhoneNumber.Text = customer[0].secondaryPhoneNumber.ToString();
                        txtEmail.Text = customer[0].email.ToString();
                        txtCity.Text = customer[0].city.ToString();
                        ddlProvince.SelectedValue = customer[0].province.ToString();
                        ddlCountry.SelectedValue = customer[0].country.ToString();

                        ddlCountry.DataSource = LM.ReturnCountryDropDown();
                        ddlCountry.DataTextField = "countryDesc";
                        ddlCountry.DataValueField = "countryID";
                        ddlCountry.DataBind();
                        ddlCountry.SelectedValue = customer[0].country.ToString();
                        ddlProvince.DataSource = LM.ReturnProvinceDropDown(customer[0].country);
                        ddlProvince.DataTextField = "provName";
                        ddlProvince.DataValueField = "provStateID";
                        ddlProvince.SelectedValue = customer[0].province.ToString();
                        ddlProvince.DataBind();

                        txtPostalCode.Text = customer[0].postalCode.ToString();
                        chkEmailList.Checked = customer[0].emailList;

                        //Binds invoice list to the grid view
                        grdInvoiceSelection.DataSource = customer[0].invoices;
                        grdInvoiceSelection.DataBind();

                    }
                }
                else
                {
                    //no cust number
                    if (!IsPostBack)
                    {
                        ddlCountry.DataSource = LM.ReturnCountryDropDown();
                        ddlCountry.DataTextField = "countryDesc";
                        ddlCountry.DataValueField = "countryID";
                        ddlCountry.DataBind();
                        ddlCountry.SelectedValue = 0.ToString();

                        ddlProvince.DataTextField = "provName";
                        ddlProvince.DataValueField = "provStateID";
                        ddlProvince.DataSource = LM.ReturnProvinceDropDown(0);
                        ddlProvince.DataBind();
                    }
                    //Displays text boxes instead of label for customer creation info
                    txtFirstName.Enabled = true;
                    //lblFirstNameDisplay.Visible = false;

                    txtLastName.Enabled = true;
                    //lblLastNameDisplay.Visible = false;

                    txtPrimaryAddress.Enabled = true;
                    //lblPrimaryAddressDisplay.Visible = false;

                    txtSecondaryAddress.Enabled = true;
                    //lblSecondaryAddressDisplay.Visible = false;

                    txtPrimaryPhoneNumber.Enabled = true;
                    //lblPrimaryPhoneNumberDisplay.Visible = false;

                    txtSecondaryPhoneNumber.Enabled = true;
                    //lblSecondaryPhoneNumberDisplay.Visible = false;

                    txtEmail.Enabled = true;
                    //lblEmailDisplay.Visible = false;

                    txtCity.Enabled = true;
                    //lblCityDisplay.Visible = false;

                    ddlProvince.Enabled = true;
                    //lblProvinceDisplay.Visible = false;

                    ddlCountry.Enabled = true;
                    //lblCountryDisplay.Visible = false;

                    chkEmailList.Enabled = true;

                    txtPostalCode.Enabled = true;
                    //lblPostalCodeDisplay.Visible = false;

                    //hides and displays the proper buttons for access
                    btnSaveCustomer.Visible = false;
                    btnAddCustomer.Visible = true;
                    pnlDefaultButton.DefaultButton = "btnAddCustomer";
                    btnEditCustomer.Visible = false;
                    btnStartSale.Visible = false;
                    btnCancel.Visible = false;
                    btnBackToSearch.Visible = true;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddCustomer_Click";
            try
            {
                //Collects new customer data to add to database
                Customer c = new Customer();
                c.firstName = txtFirstName.Text;
                c.lastName = txtLastName.Text;
                c.primaryAddress = txtPrimaryAddress.Text;
                c.secondaryAddress = txtSecondaryAddress.Text;
                c.primaryPhoneNumber = txtPrimaryPhoneNumber.Text;
                c.secondaryPhoneNumber = txtSecondaryPhoneNumber.Text;
                c.emailList = chkEmailList.Checked;
                c.email = txtEmail.Text;
                c.city = txtCity.Text;
                c.province = Convert.ToInt32(ddlProvince.SelectedValue);
                c.country = Convert.ToInt32(ddlCountry.SelectedValue);
                c.postalCode = txtPostalCode.Text;

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", CM.addCustomer(c).ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnEditCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditCustomer_Click";
            try
            {
                //transfers data from label into textbox for editing
                //txtFirstName.Text = lblFirstNameDisplay.Text;
                txtFirstName.Enabled = true;
                //lblFirstNameDisplay.Visible = false;

                //txtLastName.Text = lblLastNameDisplay.Text;
                txtLastName.Enabled = true;
                //lblLastNameDisplay.Visible = false;

                //txtPrimaryAddress.Text = lblPrimaryAddressDisplay.Text;
                txtPrimaryAddress.Enabled = true;
                //lblPrimaryAddressDisplay.Visible = false;

                //txtSecondaryAddress.Text = lblSecondaryAddressDisplay.Text;
                txtSecondaryAddress.Enabled = true;
                //lblSecondaryAddressDisplay.Visible = false;

                //txtPrimaryPhoneNumber.Text = lblPrimaryPhoneNumberDisplay.Text;
                txtPrimaryPhoneNumber.Enabled = true;
                //lblPrimaryPhoneNumberDisplay.Visible = false;

                //txtSecondaryPhoneNumber.Text = lblSecondaryPhoneNumberDisplay.Text;
                txtSecondaryPhoneNumber.Enabled = true;
                //lblSecondaryPhoneNumberDisplay.Visible = false;

                //txtEmail.Text = lblEmailDisplay.Text;
                txtEmail.Enabled = true;
                //lblEmailDisplay.Visible = false;
                chkEmailList.Enabled = true;

                //txtCity.Text = lblCityDisplay.Text;
                txtCity.Enabled = true;
                //lblCityDisplay.Visible = false;

                ddlCountry.Enabled = true;
                //lblCountryDisplay.Visible = false;

                ddlProvince.Enabled = true;
                //lblProvinceDisplay.Visible = false;

                //txtPostalCode.Text = lblPostalCodeDisplay.Text;
                txtPostalCode.Enabled = true;
                //lblPostalCodeDisplay.Visible = false;
                //hides and displays the proper buttons for access
                btnSaveCustomer.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveCustomer";
                btnEditCustomer.Visible = false;
                btnAddCustomer.Visible = false;
                btnStartSale.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveCustomer_Click";
            try
            {
                //Collects customer data to add to database
                Customer c = new Customer();
                c.customerId = Convert.ToInt32(Request.QueryString["cust"].ToString());
                c.firstName = txtFirstName.Text;
                c.lastName = txtLastName.Text;
                c.primaryAddress = txtPrimaryAddress.Text;
                c.secondaryAddress = txtSecondaryAddress.Text;
                c.primaryPhoneNumber = txtPrimaryPhoneNumber.Text;
                c.secondaryPhoneNumber = txtSecondaryPhoneNumber.Text;
                c.emailList = chkEmailList.Checked;
                c.email = txtEmail.Text;
                c.city = txtCity.Text;
                c.province = Convert.ToInt32(ddlProvince.SelectedValue);
                c.country = Convert.ToInt32(ddlCountry.SelectedValue);
                c.postalCode = txtPostalCode.Text;
                //updates the customer info in tables
                CM.updateCustomer(c);
                //changes all text boxes and dropdowns to labels
                txtFirstName.Enabled = false;
                //lblFirstNameDisplay.Visible = true;
                txtLastName.Enabled = false;
                //lblLastNameDisplay.Visible = true;
                txtPrimaryAddress.Enabled = false;
                //lblPrimaryAddressDisplay.Visible = true;
                txtSecondaryAddress.Enabled = false;
                //lblSecondaryAddressDisplay.Visible = true;
                txtPrimaryPhoneNumber.Enabled = false;
                //lblPrimaryPhoneNumberDisplay.Visible = true;
                txtSecondaryPhoneNumber.Enabled = false;
                //lblSecondaryPhoneNumberDisplay.Visible = true;
                txtEmail.Enabled = false;
                //lblEmailDisplay.Visible = true;
                chkEmailList.Enabled = false;
                txtCity.Enabled = false;
                //lblCityDisplay.Visible = true;
                ddlProvince.Enabled = false;
                //lblProvinceDisplay.Visible = true;
                ddlCountry.Enabled = false;
                //lblCountryDisplay.Visible = true;
                txtPostalCode.Enabled = false;
                //lblPostalCodeDisplay.Visible = true;
                //hides and displays the proper buttons for access
                btnSaveCustomer.Visible = false;
                btnEditCustomer.Visible = true;
                btnCancel.Visible = false;
                btnAddCustomer.Visible = false;
                btnBackToSearch.Visible = true;
                btnSaveCustomer.Visible = false;
                btnEditCustomer.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditCustomer";
                btnCancel.Visible = false;
                btnStartSale.Visible = true;
                btnAddCustomer.Visible = false;
                btnBackToSearch.Visible = true;
                //reloads current page
                Response.Redirect(Request.RawUrl, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnStartSale_Click(object sender, EventArgs e)
        {
            //****Still need this updated with new process****
            //Collects current method and page for error tracking
            string method = "btnStartSale_Click";
            try
            {
                Session["ItemsInCart"] = null;
                //Sets transaction type to sale
                Session["TranType"] = 1;
                //Sets customer id to guest cust
                Session["key"] = Convert.ToInt32(Request.QueryString["cust"].ToString());
                //opens the sales cart page
                Response.Redirect("SalesCart.aspx?cust=" + Request.QueryString["cust"].ToString(), false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnBackToSearch_Click";
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoiceSelection_RowCommand";
            try
            {
                //Sets the string of the command argument(invoice number
                string strInvoice = Convert.ToString(e.CommandArgument);
                //Splits the invoice string into numbers
                //Checks that the command name is return invoice
                Response.Redirect("PrintableInvoice.aspx?inv=" + strInvoice, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlCountry_SelectedIndexChanged";
            try
            {
                ddlProvince.DataTextField = "provName";
                ddlProvince.DataValueField = "provStateID";
                ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlProvince.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}