using System;
using System.Web;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class EmployeeAddNew : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly EmployeeManager EM = new EmployeeManager();
        readonly LocationManager LM = new LocationManager();
        CurrentUser CU;
        //private static Employee employee;
        
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
                    if (CU.employee.intJobID != 0)
                    {
                        //If user is not an admin then disable the edit employee button
                        BtnEditEmployee.Enabled = false;
                    }
                    //Check to see if an employee was selected
                    if (Convert.ToInt32(Request.QueryString["employee"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create an employee class
                            Employee employee = EM.CallReturnEmployee(Convert.ToInt32(Request.QueryString["employee"].ToString()), objPageDetails)[0];
                            //Fill asll lables with current selected employee info
                            txtFirstName.Text = employee.varFirstName.ToString();
                            txtLastName.Text = employee.varLastName.ToString();

                            ddlJob.DataSource = EM.CallReturnJobPosition(objPageDetails);
                            ddlJob.DataBind();
                            ddlJob.SelectedValue = employee.intJobID.ToString();

                            ddlLocation.DataSource = LM.CallReturnLocationDropDown(objPageDetails);
                            ddlLocation.DataBind();
                            ddlLocation.SelectedValue = employee.location.intLocationID.ToString();

                            txtEmail.Text = employee.varEmailAddress.ToString();
                            txtPrimaryPhoneNumber.Text = employee.varContactNumber.ToString();
                            txtSecondaryPhoneNumber.Text = employee.secondaryContactNumber.ToString();
                            txtPrimaryAddress.Text = employee.varAddress.ToString();
                            txtSecondaryAddress.Text = employee.secondaryAddress.ToString();
                            txtCity.Text = employee.varCityName.ToString();
                            txtPostalCode.Text = employee.varPostalCode.ToString();
                            ddlProvince.SelectedValue = employee.intProvinceID.ToString();
                            DdlCountry.SelectedValue = employee.intCountryID.ToString();
                            DdlCountry.DataSource = LM.CallReturnCountryDropDown(objPageDetails);
                            DdlCountry.DataBind();
                            ddlProvince.DataSource = LM.CallReturnProvinceDropDown(employee.intCountryID, objPageDetails);
                            ddlProvince.DataBind();
                        }
                    }
                    else
                    {
                        if (!IsPostBack)
                        {
                            ddlJob.DataSource = EM.CallReturnJobPosition(objPageDetails);
                            ddlJob.DataBind();
                            ddlJob.SelectedValue = CU.employee.intJobID.ToString();

                            ddlLocation.DataSource = LM.CallReturnLocationDropDown(objPageDetails);
                            ddlLocation.DataBind();
                            ddlLocation.SelectedValue = CU.location.intLocationID.ToString();
                            
                            DdlCountry.DataSource = LM.CallReturnCountryDropDown(objPageDetails);
                            DdlCountry.DataBind();
                            DdlCountry.SelectedValue = CU.location.intCountryID.ToString();

                            ddlProvince.DataSource = LM.CallReturnProvinceDropDown(0, objPageDetails);
                            ddlProvince.DataBind();
                            ddlProvince.SelectedValue = CU.location.intProvinceID.ToString();
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
                        DdlCountry.Enabled = true;

                        //hides and displays the proper buttons for access
                        BtnSaveEmployee.Visible = false;
                        BtnAddEmployee.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddEmployee";
                        BtnEditEmployee.Visible = false;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddEmployee_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Collects new employee data to add to database
                Employee employee = new Employee(); ;
                employee.varFirstName = txtFirstName.Text;
                employee.varLastName = txtLastName.Text;
                employee.intJobID = Convert.ToInt32(ddlJob.SelectedValue);
                employee.location = LM.CallReturnLocation(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails)[0];
                employee.varEmailAddress = txtEmail.Text;
                employee.varContactNumber = txtPrimaryPhoneNumber.Text;
                employee.secondaryContactNumber = txtSecondaryPhoneNumber.Text;
                employee.varAddress = txtPrimaryAddress.Text;
                employee.secondaryAddress = txtSecondaryAddress.Text;
                employee.varCityName = txtCity.Text;
                employee.varPostalCode = txtPostalCode.Text;
                employee.intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue);
                employee.intCountryID = Convert.ToInt32(DdlCountry.SelectedValue);

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("employee", EM.CallAddEmployee(employee, objPageDetails).ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnEditEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnEditEmployee_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                DdlCountry.Enabled = true;

                //hides and displays the proper buttons for access
                BtnSaveEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveEmployee";
                BtnEditEmployee.Visible = false;
                BtnAddEmployee.Visible = false;
                BtnCancel.Visible = true;
                BtnBackToSearch.Visible = false;
                //Add or Update the password for employee
                lblNewPassword.Visible = true;
                txtNewPassword.Visible = true;
                lblPasswordFormat.Visible = true;
                lblNewPassword2.Visible = true;
                txtNewPassword2.Visible = true;
                BtnSavePassword.Visible = true;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnSaveEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnSaveEmployee_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Collects employee data to add to database
                Employee employee = new Employee
                {
                    intEmployeeID = Convert.ToInt32(Request.QueryString["employee"].ToString()),
                    varFirstName = txtFirstName.Text,
                    varLastName = txtLastName.Text,
                    intJobID = Convert.ToInt32(ddlJob.SelectedValue),
                    location = LM.CallReturnLocation(Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails)[0],
                    varEmailAddress = txtEmail.Text,
                    varContactNumber = txtPrimaryPhoneNumber.Text,
                    secondaryContactNumber = txtSecondaryPhoneNumber.Text,
                    varAddress = txtPrimaryAddress.Text,
                    secondaryAddress = txtSecondaryAddress.Text,
                    varCityName = txtCity.Text,
                    varPostalCode = txtPostalCode.Text,
                    intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue),
                    intCountryID = Convert.ToInt32(DdlCountry.SelectedValue)
                };

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
                DdlCountry.Enabled = false;

                //hides and displays the proper buttons for access
                BtnSaveEmployee.Visible = false;
                BtnEditEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditEmployee";
                BtnCancel.Visible = false;
                BtnAddEmployee.Visible = false;
                BtnBackToSearch.Visible = true;
                lblNewPassword.Visible = false;
                txtNewPassword.Visible = false;
                lblPasswordFormat.Visible = false;
                lblNewPassword2.Visible = false;
                txtNewPassword2.Visible = false;
                BtnSavePassword.Visible = false;

                //reloads current page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("employee", EM.CallUpdateEmployee(employee, objPageDetails).ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
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
                //no changes saved, refreshes current page
                Response.Redirect(Request.RawUrl, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnBackToSearch_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnSavePassword_Click(object sender, EventArgs e)
        {
            string method = "BtnSavePassword_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Compare the 2 passwords entered to make sure they are identical
                if(Convert.ToInt32(txtNewPassword.Text) == Convert.ToInt32(txtNewPassword2.Text))
                {
                    //Call method to add the new password
                    bool bolAdded = EM.CallSaveNewPassword(Convert.ToInt32(Request.QueryString["employee"].ToString()), Convert.ToInt32(txtNewPassword.Text), objPageDetails);
                    //Check if the password was added or not
                    if (!bolAdded)
                    {
                        //The password was not added because it is already in use by employee
                        MessageBox.ShowMessage("The password supplied is not available. Please try another password.", this);
                    }
                    else
                    {
                        //The password was added, advise user and return to employee viewing
                        MessageBox.ShowMessage("New password for employee saved.", this);
                        BtnCancel_Click(sender, e);
                    }
                }
                else
                {
                    //Passwords do not match
                    MessageBox.ShowMessage("The passwords do not match. Please retype the passwords again.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}