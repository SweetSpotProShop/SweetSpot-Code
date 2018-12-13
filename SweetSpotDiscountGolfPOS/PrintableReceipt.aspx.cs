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
                        Invoice I = IM.ReturnPurchaseInvoice(Request.QueryString["receipt"].ToString(), objPageDetails)[0];
                        //display information on receipt
                        lblCustomerName.Text = I.customer.firstName.ToString() + " " + I.customer.lastName.ToString();
                        lblStreetAddress.Text = I.customer.primaryAddress.ToString();
                        lblPostalAddress.Text = I.customer.city.ToString() + ", " + LM.ReturnProvinceName(I.customer.province, objPageDetails) + " " + I.customer.postalCode.ToString();
                        lblPhone.Text = I.customer.primaryPhoneNumber.ToString();
                        lblinvoiceNum.Text = I.invoiceNum.ToString();
                        lblDate.Text = I.invoiceDate.ToString("dd/MMM/yy");
                        //Gather transaction type from Session
                        //Display the location information
                        lblSweetShopName.Text = I.location.locationName.ToString();
                        lblSweetShopStreetAddress.Text = I.location.address.ToString();
                        lblSweetShopPostalAddress.Text = I.location.city.ToString() + ", " + LM.ReturnProvinceName(I.location.provID, objPageDetails) + " " + I.location.postal.ToString();
                        lblSweetShopPhone.Text = I.location.primaryPhone.ToString();

                        //Binds the cart to the grid view
                        grdItemsBoughtList.DataSource = I.soldItems;
                        grdItemsBoughtList.DataBind();

                        //Displays the total amount ppaid
                        lblSubtotalDisplay.Text = I.subTotal.ToString("#0.00");
                        lblTotalPaidDisplay.Text = I.subTotal.ToString("#0.00");
                        //Binds the payment methods to a gridview
                        grdMOPS.DataSource = I.usedMops;
                        grdMOPS.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}