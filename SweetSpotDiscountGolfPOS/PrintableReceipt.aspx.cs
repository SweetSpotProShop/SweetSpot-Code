using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class PrintableReceipt : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        LocationManager LM = new LocationManager();
        InvoiceManager IM = new InvoiceManager();
        private static Invoice invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PrintableReceipt.aspx";
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
                    if (!IsPostBack)
                    {
                        //Store in Customer class
                        invoice = IM.ReturnPurchaseInvoice(Convert.ToInt32(Request.QueryString["receipt"].ToString()), objPageDetails)[0];
                        //display information on receipt
                        lblCustomerName.Text = invoice.customer.varFirstName.ToString() + " " + invoice.customer.varLastName.ToString();
                        lblStreetAddress.Text = invoice.customer.varAddress.ToString();
                        lblPostalAddress.Text = invoice.customer.varCityName.ToString() + ", " + LM.ReturnProvinceName(invoice.customer.intProvinceID, objPageDetails) + " " + invoice.customer.varPostalCode.ToString();
                        lblPhone.Text = invoice.customer.varContactNumber.ToString();
                        lblinvoiceNum.Text = invoice.varInvoiceNumber.ToString();
                        lblDate.Text = invoice.dtmInvoiceDate.ToString("dd/MMM/yy");
                        //Gather transaction type from Session
                        //Display the location information
                        lblSweetShopName.Text = invoice.location.varLocationName.ToString();
                        lblSweetShopStreetAddress.Text = invoice.location.varAddress.ToString();
                        lblSweetShopPostalAddress.Text = invoice.location.varCityName.ToString() + ", " + LM.ReturnProvinceName(invoice.location.intProvinceID, objPageDetails) + " " + invoice.location.varPostalCode.ToString();
                        lblSweetShopPhone.Text = invoice.location.varContactNumber.ToString();

                        //Binds the cart to the grid view
                        grdItemsBoughtList.DataSource = invoice.invoiceItems;
                        grdItemsBoughtList.DataBind();

                        //Displays the total amount ppaid
                        lblSubtotalDisplay.Text = invoice.fltSubTotal.ToString("#0.00");
                        lblTotalPaidDisplay.Text = invoice.fltSubTotal.ToString("#0.00");
                        //Binds the payment methods to a gridview
                        grdMOPS.DataSource = invoice.invoiceMops;
                        grdMOPS.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnHome_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Change to the Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}