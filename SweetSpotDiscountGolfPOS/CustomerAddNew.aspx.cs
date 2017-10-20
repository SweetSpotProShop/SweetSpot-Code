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
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        CurrentUser cu;
        EmployeeManager em = new EmployeeManager();
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "CustomerAddNew.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                //Checks for a Customer Key
                if (Session["key"] != null)
                {
                    if (!IsPostBack)
                    {
                        //Create customer class and fill page with all info based in the customer number 
                        //from the key
                        int custNum = Convert.ToInt32(Session["key"].ToString());
                        Customer c = ssm.GetCustomerbyCustomerNumber(custNum);

                        lblFirstNameDisplay.Text = c.firstName.ToString();
                        lblLastNameDisplay.Text = c.lastName.ToString();
                        lblPrimaryAddressDisplay.Text = c.primaryAddress.ToString();
                        //lblBillingAddressDisplay.Text = c.emailList.ToString();
                        lblSecondaryAddressDisplay.Text = c.secondaryAddress.ToString();
                        lblPrimaryPhoneNumberDisplay.Text = c.primaryPhoneNumber.ToString();
                        lblSecondaryPhoneNumberDisplay.Text = c.secondaryPhoneNumber.ToString();
                        lblEmailDisplay.Text = c.email.ToString();
                        lblCityDisplay.Text = c.city.ToString();
                        lblProvinceDisplay.Text = lm.provinceName(c.province);
                        lblCountryDisplay.Text = lm.countryName(c.country);

                        ddlCountry.SelectedValue = c.country.ToString();
                        dt = em.returnProvinceDropDown(c.country);
                        ddlProvince.DataTextField = "provName";
                        ddlProvince.DataValueField = "provStateID";
                        ddlProvince.DataSource = dt;
                        ddlProvince.DataBind();

                        lblPostalCodeDisplay.Text = c.postalCode.ToString();
                        if (c.emailList == true) { chkEmailList.Checked = true; }
                        else { chkEmailList.Checked = false; }
                    }
                    //Customer invoices
                    List<Invoice> fullInvoices;
                    //Searches through invoices using customer name and date
                    fullInvoices = ssm.getInvoiceFromCustID(Convert.ToInt32(Session["key"].ToString()));

                    List<Invoice> viewInvoices = new List<Invoice>();
                    //Loops through each invoice
                    foreach (var i in fullInvoices)
                    {
                        //Sets customer and employee class for the last invoice
                        Customer c = ssm.GetCustomerbyCustomerNumber(i.customerID);
                        Employee emp = em.getEmployeeByID(i.employeeID);
                        //Uses the classes to set customer name and employee name of each invoice
                        Invoice iv = new Invoice(i.invoiceNum, i.invoiceSub, i.invoiceDate, c.firstName + " " + c.lastName, i.discountAmount, i.tradeinAmount, i.subTotal, i.governmentTax, i.provincialTax, i.balanceDue, emp.firstName + " " + emp.lastName);
                        //Adds each invoice to invoice list
                        viewInvoices.Add(iv);
                    }
                    //Binds invoice list to the grid view
                    grdInvoiceSelection.DataSource = viewInvoices;
                    grdInvoiceSelection.DataBind();
                    //Center the mop grid view
                    foreach (GridViewRow row in grdInvoiceSelection.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            cell.Attributes.CssStyle["text-align"] = "center";
                        }
                    }
                }
                else
                {
                    //Displays text boxes instead of label for customer creation info
                    txtFirstName.Visible = true;
                    lblFirstNameDisplay.Visible = false;

                    txtLastName.Visible = true;
                    lblLastNameDisplay.Visible = false;

                    txtPrimaryAddress.Visible = true;
                    lblPrimaryAddressDisplay.Visible = false;

                    //txtBillingAddress.Visible = true;
                    //lblBillingAddressDisplay.Visible = false;

                    txtSecondaryAddress.Visible = true;
                    lblSecondaryAddressDisplay.Visible = false;

                    txtPrimaryPhoneNumber.Visible = true;
                    lblPrimaryPhoneNumberDisplay.Visible = false;

                    txtSecondaryPhoneNumber.Visible = true;
                    lblSecondaryPhoneNumberDisplay.Visible = false;

                    txtEmail.Visible = true;
                    lblEmailDisplay.Visible = false;

                    txtCity.Visible = true;
                    lblCityDisplay.Visible = false;

                    ddlProvince.Visible = true;
                    lblProvinceDisplay.Visible = false;

                    ddlCountry.Visible = true;
                    lblCountryDisplay.Visible = false;

                    txtPostalCode.Visible = true;
                    lblPostalCodeDisplay.Visible = false;
                    dt = em.returnProvinceDropDown(0);
                    ddlProvince.DataTextField = "provName";
                    ddlProvince.DataValueField = "provStateID";
                    ddlProvince.DataSource = dt;
                    ddlProvince.DataBind();
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                if (chkEmailList.Checked) { c.emailList = true; }
                else { c.emailList = false; }
                c.email = txtEmail.Text;
                c.city = txtCity.Text;
                c.province = Convert.ToInt32(ddlProvince.SelectedValue);
                c.country = Convert.ToInt32(ddlCountry.SelectedValue);
                c.postalCode = txtPostalCode.Text;

                //Process the add and saves the customer into the key.
                Session["key"] = ssm.addCustomer(c);
                Server.Transfer(Request.RawUrl, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnEditCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditCustomer_Click";
            try
            {
                //transfers data from label into textbox for editing
                txtFirstName.Text = lblFirstNameDisplay.Text;
                txtFirstName.Visible = true;
                lblFirstNameDisplay.Visible = false;

                txtLastName.Text = lblLastNameDisplay.Text;
                txtLastName.Visible = true;
                lblLastNameDisplay.Visible = false;

                txtPrimaryAddress.Text = lblPrimaryAddressDisplay.Text;
                txtPrimaryAddress.Visible = true;
                lblPrimaryAddressDisplay.Visible = false;

                //txtBillingAddress.Text = lblBillingAddressDisplay.Text;
                //txtBillingAddress.Visible = true;
                //lblBillingAddressDisplay.Visible = false;

                txtSecondaryAddress.Text = lblSecondaryAddressDisplay.Text;
                txtSecondaryAddress.Visible = true;
                lblSecondaryAddressDisplay.Visible = false;

                txtPrimaryPhoneNumber.Text = lblPrimaryPhoneNumberDisplay.Text;
                txtPrimaryPhoneNumber.Visible = true;
                lblPrimaryPhoneNumberDisplay.Visible = false;

                txtSecondaryPhoneNumber.Text = lblSecondaryPhoneNumberDisplay.Text;
                txtSecondaryPhoneNumber.Visible = true;
                lblSecondaryPhoneNumberDisplay.Visible = false;

                txtEmail.Text = lblEmailDisplay.Text;
                txtEmail.Visible = true;
                lblEmailDisplay.Visible = false;
                chkEmailList.Enabled = true;

                txtCity.Text = lblCityDisplay.Text;
                txtCity.Visible = true;
                lblCityDisplay.Visible = false;

                //transfers data from label into dropdown for editing
                ddlCountry.SelectedValue = (lm.countryID(lblCountryDisplay.Text)).ToString();
                ddlCountry.Visible = true;
                lblCountryDisplay.Visible = false;
                ddlProvince.SelectedValue = (lm.pronvinceID(lblProvinceDisplay.Text)).ToString();
                ddlProvince.Visible = true;
                lblProvinceDisplay.Visible = false;

                txtPostalCode.Text = lblPostalCodeDisplay.Text;
                txtPostalCode.Visible = true;
                lblPostalCodeDisplay.Visible = false;
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                c.customerId = (int)(Convert.ToInt32(Session["key"].ToString()));
                c.firstName = txtFirstName.Text;
                c.lastName = txtLastName.Text;
                c.primaryAddress = txtPrimaryAddress.Text;
                c.secondaryAddress = txtSecondaryAddress.Text;
                c.primaryPhoneNumber = txtPrimaryPhoneNumber.Text;
                c.secondaryPhoneNumber = txtSecondaryPhoneNumber.Text;
                if (chkEmailList.Checked) { c.emailList = true; }
                else { c.emailList = false; }
                c.email = txtEmail.Text;
                c.city = txtCity.Text;
                c.province = Convert.ToInt32(ddlProvince.SelectedValue);
                c.country = Convert.ToInt32(ddlCountry.SelectedValue);
                c.postalCode = txtPostalCode.Text;
                //updates the customer info in tables
                ssm.updateCustomer(c);
                //changes all text boxes and dropdowns to labels
                txtFirstName.Visible = false;
                lblFirstNameDisplay.Visible = true;
                txtLastName.Visible = false;
                lblLastNameDisplay.Visible = true;
                txtPrimaryAddress.Visible = false;
                lblPrimaryAddressDisplay.Visible = true;
                //txtBillingAddress.Visible = false;
                //lblBillingAddressDisplay.Visible = true;
                txtSecondaryAddress.Visible = false;
                lblSecondaryAddressDisplay.Visible = true;
                txtPrimaryPhoneNumber.Visible = false;
                lblPrimaryPhoneNumberDisplay.Visible = true;
                txtSecondaryPhoneNumber.Visible = false;
                lblSecondaryPhoneNumberDisplay.Visible = true;
                txtEmail.Visible = false;
                lblEmailDisplay.Visible = true;
                chkEmailList.Enabled = false;
                txtCity.Visible = false;
                lblCityDisplay.Visible = true;
                ddlProvince.Visible = false;
                lblProvinceDisplay.Visible = true;
                ddlCountry.Visible = false;
                lblCountryDisplay.Visible = true;
                txtPostalCode.Visible = false;
                lblPostalCodeDisplay.Visible = true;
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
                Server.Transfer(Request.RawUrl, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            try
            {
                //no chnages saved and moves to customer home page
                Server.Transfer(Request.RawUrl, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnStartSale_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnStartSale_Click";
            try
            {
                //Sets transaction type as sale
                Session["TranType"] = 1;
                //opens the sales cart page
                Server.Transfer("SalesCart.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnBackToSearch_Click";
            try
            {
                //removes key that was set so no customer is currently selected
                Session["key"] = null;
                //opens the Customer home page
                Server.Transfer("CustomerHomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                int invNum = Convert.ToInt32(strInvoice.Split('-')[0]);
                int invSNum = Convert.ToInt32(strInvoice.Split('-')[1]);
                //Checks that the command name is return invoice
                if (e.CommandName == "returnInvoice")
                {
                    //determines the table to use for queries
                    string table = "";
                    int tran = 3;
                    if (invSNum > 1)
                    {
                        table = "Returns";
                        tran = 4;
                    }
                    //Stores required info into Sessions
                    Invoice rInvoice = ssm.getSingleInvoice(invNum, invSNum);
                    Session["key"] = rInvoice.customerID;
                    Session["Invoice"] = strInvoice;
                    Session["useInvoice"] = true;
                    Session["strDate"] = rInvoice.invoiceDate;
                    Session["ItemsInCart"] = ssm.invoice_getItems(invNum, invSNum, "tbl_invoiceItem" + table);
                    Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invNum, invSNum, "tbl_invoice");
                    Session["MethodsOfPayment"] = ssm.invoice_getMOP(invNum, invSNum, "tbl_invoiceMOP");
                    Session["TranType"] = tran;
                    //Changes to printable invoice page
                    Server.Transfer("PrintableInvoice.aspx", false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlCountry_SelectedIndexChanged";
            try
            {
                dt = em.returnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));

                ddlProvince.DataTextField = "provName";
                ddlProvince.DataValueField = "provStateID";

                ddlProvince.DataSource = dt;
                ddlProvince.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
    }
}