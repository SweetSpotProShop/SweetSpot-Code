using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System.Threading;
using System.Data;

namespace SweetSpotDiscountGolfPOS
{
    public partial class EmployeeAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        EmployeeManager EM = new EmployeeManager();
        LocationManager LM = new LocationManager();
        CurrentUser CU;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "EmployeeAddNew.aspx";
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
                    if (CU.jobID != 0)
                    {
                        //If user is not an admin then disable the edit employee button
                        btnEditEmployee.Enabled = false;
                    }
                    //Check to see if an employee was selected
                    if (Convert.ToInt32(Request.QueryString["emp"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create an employee class
                            Employee employee = EM.ReturnEmployee(Convert.ToInt32(Request.QueryString["emp"].ToString()), objPageDetails)[0];
                            //Fill asll lables with current selected employee info
                            txtFirstName.Text = employee.firstName.ToString();
                            txtLastName.Text = employee.lastName.ToString();
                            ddlJob.SelectedValue = employee.jobID.ToString();
                            ddlLocation.SelectedValue = employee.location.locationID.ToString();
                            txtEmail.Text = employee.emailAddress.ToString();
                            txtPrimaryPhoneNumber.Text = employee.primaryContactNumber.ToString();
                            txtSecondaryPhoneNumber.Text = employee.secondaryContactNumber.ToString();
                            txtPrimaryAddress.Text = employee.primaryAddress.ToString();
                            txtSecondaryAddress.Text = employee.secondaryAddress.ToString();
                            txtCity.Text = employee.city.ToString();
                            txtPostalCode.Text = employee.postZip.ToString();
                            ddlProvince.SelectedValue = employee.provState.ToString();
                            ddlCountry.SelectedValue = employee.country.ToString();
                            ddlProvince.DataTextField = "provName";
                            ddlProvince.DataValueField = "provStateID";
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(employee.country, objPageDetails);
                            ddlProvince.DataBind();
                        }
                    }
                    else
                    {
                        if (!IsPostBack)
                        {
                            ddlLocation.SelectedValue = CU.location.locationID.ToString();
                            ddlProvince.DataTextField = "provName";
                            ddlProvince.DataValueField = "provStateID";
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(0, objPageDetails);
                            ddlProvince.DataBind();
                        }
                        //With no employee selected display text boxes and drop downs to add employee
                        txtFirstName.Enabled = true;
                        txtLastName.Enabled = true;
                        ddlJob.Enabled = true;
                        ddlLocation.Enabled = true;

                        txtEmail.Enabled = true;
                        txtPrimaryPhoneNumber.Enabled = true;
                        txtSecondaryPhoneNumber.Enabled = true;
                        txtPrimaryAddress.Enabled = true;
                        txtSecondaryAddress.Enabled = true;
                        txtCity.Enabled = true;
                        txtPostalCode.Enabled = true;
                        ddlProvince.Enabled = true;
                        ddlCountry.Enabled = true;

                        //hides and displays the proper buttons for access
                        btnSaveEmployee.Visible = false;
                        btnAddEmployee.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddEmployee";
                        btnEditEmployee.Visible = false;
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
        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddEmployee_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Collects new employee data to add to database
                Employee em = new Employee();
                em.firstName = txtFirstName.Text;
                em.lastName = txtLastName.Text;
                em.jobID = Convert.ToInt32(ddlJob.SelectedValue);
                em.location = LM.ReturnLocation(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails)[0];
                em.emailAddress = txtEmail.Text;
                em.primaryContactNumber = txtPrimaryPhoneNumber.Text;
                em.secondaryContactNumber = txtSecondaryPhoneNumber.Text;
                em.primaryAddress = txtPrimaryAddress.Text;
                em.secondaryAddress = txtSecondaryAddress.Text;
                em.city = txtCity.Text;
                em.postZip = txtPostalCode.Text;
                em.provState = Convert.ToInt32(ddlProvince.SelectedValue);
                em.country = Convert.ToInt32(ddlCountry.SelectedValue);

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("emp", EM.AddEmployee(em, objPageDetails).ToString());
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
        protected void btnEditEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditEmployee_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //transfers data from label into textbox for editing
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtEmail.Enabled = true;
                txtPrimaryPhoneNumber.Enabled = true;
                txtSecondaryPhoneNumber.Enabled = true;
                txtPrimaryAddress.Enabled = true;
                txtSecondaryAddress.Enabled = true;
                txtCity.Enabled = true;
                txtPostalCode.Enabled = true;
                ddlJob.Enabled = true;
                ddlLocation.Enabled = true;
                ddlProvince.Enabled = true;
                ddlCountry.Enabled = true;

                //hides and displays the proper buttons for access
                btnSaveEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveEmployee";
                btnEditEmployee.Visible = false;
                btnAddEmployee.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
                //Add or Update the password for employee
                lblNewPassword.Visible = true;
                txtNewPassword.Visible = true;
                lblPasswordFormat.Visible = true;
                lblNewPassword2.Visible = true;
                txtNewPassword2.Visible = true;
                btnSavePassword.Visible = true;
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
        protected void btnSaveEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveEmployee_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Collects employee data to add to database
                Employee em = new Employee();
                em.employeeID = Convert.ToInt32(Convert.ToInt32(Request.QueryString["emp"].ToString()));
                em.firstName = txtFirstName.Text;
                em.lastName = txtLastName.Text;
                em.jobID = Convert.ToInt32(ddlJob.SelectedValue);
                em.location = LM.ReturnLocation(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails)[0];
                em.emailAddress = txtEmail.Text;
                em.primaryContactNumber = txtPrimaryPhoneNumber.Text;
                em.secondaryContactNumber = txtSecondaryPhoneNumber.Text;
                em.primaryAddress = txtPrimaryAddress.Text;
                em.secondaryAddress = txtSecondaryAddress.Text;
                em.city = txtCity.Text;
                em.postZip = txtPostalCode.Text;
                em.provState = Convert.ToInt32(ddlProvince.SelectedValue);
                em.country = Convert.ToInt32(ddlCountry.SelectedValue);
                
                //changes all text boxes and dropdowns to labels
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                ddlJob.Enabled = false;
                ddlLocation.Enabled = false;
                txtEmail.Enabled = false;
                txtPrimaryPhoneNumber.Enabled = false;
                txtSecondaryPhoneNumber.Enabled = false;
                txtPrimaryAddress.Enabled = false;
                txtSecondaryAddress.Enabled = false;
                txtCity.Enabled = false;
                txtPostalCode.Enabled = false;
                ddlProvince.Enabled = false;
                ddlCountry.Enabled = false;

                //hides and displays the proper buttons for access
                btnSaveEmployee.Visible = false;
                btnEditEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditEmployee";
                btnCancel.Visible = false;
                btnAddEmployee.Visible = false;
                btnBackToSearch.Visible = true;
                lblNewPassword.Visible = false;
                txtNewPassword.Visible = false;
                lblPasswordFormat.Visible = false;
                lblNewPassword2.Visible = false;
                txtNewPassword2.Visible = false;
                btnSavePassword.Visible = false;

                //reloads current page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("emp", EM.UpdateEmployee(em, objPageDetails).ToString());
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //no changes saved, refreshes current page
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnBackToSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Changes page to the settings page
                Response.Redirect("SettingsHomePage.aspx", false);
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
        protected void btnSavePassword_Click(object sender, EventArgs e)
        {
            string method = "btnSavePassword_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Compare the 2 passwords entered to make sure they are identical
                if(Convert.ToInt32(txtNewPassword.Text) == Convert.ToInt32(txtNewPassword2.Text))
                {
                    //Call method to add the new password
                    bool bolAdded = EM.saveNewPassword(Convert.ToInt32(Request.QueryString["emp"].ToString()), Convert.ToInt32(txtNewPassword.Text), objPageDetails);
                    //Check if the password was added or not
                    if (!bolAdded)
                    {
                        //The password was not added because it is already in use by employee
                        MessageBox.ShowMessage("The password supplied is not available. "
                            + "Please try another password.", this);
                    }
                    else
                    {
                        //The password was added, advise user and return to employee viewing
                        MessageBox.ShowMessage("New password for employee saved.", this);
                        btnCancel_Click(sender, e);
                    }
                }
                else
                {
                    //Passwords do not match
                    MessageBox.ShowMessage("The passwords do not match. "
                            + "Please retype the passwords again.", this);
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
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlCountry_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                ddlProvince.DataTextField = "provName";
                ddlProvince.DataValueField = "provStateID";
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