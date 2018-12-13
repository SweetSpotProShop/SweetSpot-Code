﻿using SweetShop;
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
                    if (Convert.ToInt32(Request.QueryString["cust"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create customer class and fill page with all info based in the customer number 
                            Customer customer = CM.ReturnCustomerWithInvoiceList(Convert.ToInt32(Request.QueryString["cust"].ToString()), objPageDetails)[0];

                            txtFirstName.Text = customer.firstName.ToString();
                            txtLastName.Text = customer.lastName.ToString();
                            txtPrimaryAddress.Text = customer.primaryAddress.ToString();
                            txtSecondaryAddress.Text = customer.secondaryAddress.ToString();
                            txtPrimaryPhoneNumber.Text = customer.primaryPhoneNumber.ToString();
                            txtSecondaryPhoneNumber.Text = customer.secondaryPhoneNumber.ToString();
                            txtEmail.Text = customer.email.ToString();
                            txtCity.Text = customer.city.ToString();

                            ddlCountry.DataSource = LM.ReturnCountryDropDown(objPageDetails);
                            ddlCountry.DataBind();
                            ddlCountry.SelectedValue = customer.country.ToString();
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(customer.country, objPageDetails);
                            ddlProvince.SelectedValue = customer.province.ToString();
                            ddlProvince.DataBind();

                            txtPostalCode.Text = customer.postalCode.ToString();
                            chkEmailList.Checked = customer.emailList;

                            //Binds invoice list to the grid view
                            grdInvoiceSelection.DataSource = customer.invoices;
                            grdInvoiceSelection.DataBind();
                        }
                    }
                    else
                    {
                        //no cust number
                        if (!IsPostBack)
                        {
                            ddlCountry.DataSource = LM.ReturnCountryDropDown(objPageDetails);
                            ddlCountry.DataBind();
                            ddlCountry.SelectedValue = CU.location.countryID.ToString();

                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(0, objPageDetails);
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
                        ddlCountry.Enabled = true;
                        chkEmailList.Enabled = true;
                        txtPostalCode.Enabled = true;
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
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                nameValues.Set("cust", CM.addCustomer(c, objPageDetails).ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void btnEditCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ddlCountry.Enabled = true;
                ddlProvince.Enabled = true;
                txtPostalCode.Enabled = true;
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveCustomer_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                CM.updateCustomer(c, objPageDetails);
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
                ddlCountry.Enabled = false;
                txtPostalCode.Enabled = false;
                //hides and displays the proper buttons for access
                pnlDefaultButton.DefaultButton = "btnEditCustomer";
                btnAddCustomer.Visible = false;
                btnBackToSearch.Visible = true;
                btnCancel.Visible = false;
                btnEditCustomer.Visible = true;
                btnSaveCustomer.Visible = false;
                btnStartSale.Visible = true;
                
                //reloads current page
                Response.Redirect(Request.RawUrl, false);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnStartSale_Click(object sender, EventArgs e)
        {
            //****Still need this updated with new process****
            //Collects current method and page for error tracking
            string method = "btnStartSale_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceManager IM = new InvoiceManager();
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("cust", Request.QueryString["cust"].ToString());
                string invoice = CU.locationName + "-" + IM.ReturnNextInvoiceNumber(objPageDetails) + "-1";
                nameValues.Set("inv", invoice);
                //Changes page to Sales Cart
                Response.Redirect("SalesCart.aspx?" + nameValues, false);
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnBackToSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoiceSelection_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlCountry_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue), objPageDetails);
                ddlProvince.DataBind();
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