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
    public partial class PrintableInvoice : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;

        LocationManager LM = new LocationManager();
        InvoiceManager IM = new InvoiceManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PrintableInvoice.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (!IsPostBack)
                {
                    List<Invoice> invoices = IM.ReturnInvoice(Request.QueryString["inv"]);

                    //display information on receipt
                    lblCustomerName.Text = invoices[0].customer.firstName.ToString() + " " + invoices[0].customer.lastName.ToString();
                    lblStreetAddress.Text = invoices[0].customer.primaryAddress.ToString();
                    lblPostalAddress.Text = invoices[0].customer.city.ToString() + ", " + LM.ReturnProvinceName(invoices[0].customer.province) + " " + invoices[0].customer.postalCode.ToString();
                    lblPhone.Text = invoices[0].customer.primaryPhoneNumber.ToString();
                    lblinvoiceNum.Text = invoices[0].invoiceNum.ToString() + "-" + invoices[0].invoiceSub.ToString();
                    lblDate.Text = invoices[0].invoiceDate.ToShortDateString();
                    lblTime.Text = invoices[0].invoiceTime.ToString("h:mm tt");

                    //Display the location information
                    lblSweetShopName.Text = invoices[0].location.locationName.ToString();
                    lblSweetShopStreetAddress.Text = invoices[0].location.address.ToString();
                    lblSweetShopPostalAddress.Text = invoices[0].location.city.ToString() + ", " + LM.ReturnProvinceName(invoices[0].location.provID) + " " + invoices[0].location.postal.ToString();
                    lblSweetShopPhone.Text = invoices[0].location.primaryPhone.ToString();
                    lblTaxNum.Text = invoices[0].location.taxNumber.ToString();

                    //Display the totals
                    lblDiscountsDisplay.Text = invoices[0].discountAmount.ToString("#0.00");
                    lblTradeInsDisplay.Text = invoices[0].tradeinAmount.ToString("#0.00");
                    lblShippingDisplay.Text = invoices[0].shippingAmount.ToString("#0.00");
                    lblGSTDisplay.Text = invoices[0].governmentTax.ToString("#0.00");
                    lblPSTDisplay.Text = invoices[0].provincialTax.ToString("#0.00");
                    lblSubtotalDisplay.Text = invoices[0].subTotal.ToString("#0.00");
                    lblTotalPaidDisplay.Text = invoices[0].balanceDue.ToString("#0.00");

                    if (invoices[0].invoiceSub > 1)
                    {
                    //    //Changes headers if the invoice is return
                        grdItemsSoldList.Columns[2].HeaderText = "Sold At";
                        grdItemsSoldList.Columns[3].HeaderText = "Non Refundable";
                        grdItemsSoldList.Columns[5].HeaderText = "Returned At";
                    }
                    //Binds the cart to the grid view
                    grdItemsSoldList.DataSource = invoices[0].soldItems;
                    grdItemsSoldList.DataBind();
                    
                    //Displays the total amount ppaid
                    //Binds the payment methods to a gridview
                    grdMOPS.DataSource = invoices[0].usedMops;
                    grdMOPS.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnHome_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnHome_Click";
            try
            {
                //Nulls used stored sessions 
                Session["useInvoice"] = null;
                Session["Invoice"] = null;
                Session["key"] = null;
                Session["ItemsInCart"] = null;
                Session["returnedCart"] = null;
                Session["TranType"] = null;
                Session["CheckOutTotals"] = null;
                Session["MethodsofPayment"] = null;
                Session["strDate"] = null;
                //Change to the Home Page
                Response.Redirect("HomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}