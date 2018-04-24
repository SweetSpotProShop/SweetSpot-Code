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
                        Invoice invoices = IM.ReturnInvoice(Request.QueryString["inv"])[0];

                        //display information on receipt
                        lblCustomerName.Text = invoices.customer.firstName.ToString() + " " + invoices.customer.lastName.ToString();
                        lblStreetAddress.Text = invoices.customer.primaryAddress.ToString();
                        lblPostalAddress.Text = invoices.customer.city.ToString() + ", " + LM.ReturnProvinceName(invoices.customer.province) + " " + invoices.customer.postalCode.ToString();
                        lblPhone.Text = invoices.customer.primaryPhoneNumber.ToString();
                        lblinvoiceNum.Text = invoices.invoiceNum.ToString() + "-" + invoices.invoiceSub.ToString();
                        lblDate.Text = invoices.invoiceDate.ToShortDateString();
                        lblTime.Text = invoices.invoiceTime.ToString("h:mm tt");

                        //Display the location information
                        lblSweetShopName.Text = invoices.location.locationName.ToString();
                        lblSweetShopStreetAddress.Text = invoices.location.address.ToString();
                        lblSweetShopPostalAddress.Text = invoices.location.city.ToString() + ", " + LM.ReturnProvinceName(invoices.location.provID) + " " + invoices.location.postal.ToString();
                        lblSweetShopPhone.Text = invoices.location.primaryPhone.ToString();
                        lblTaxNum.Text = invoices.location.taxNumber.ToString();

                        //Display the totals
                        lblDiscountsDisplay.Text = invoices.discountAmount.ToString("#0.00");
                        lblTradeInsDisplay.Text = invoices.tradeinAmount.ToString("#0.00");
                        lblShippingDisplay.Text = invoices.shippingAmount.ToString("#0.00");
                        lblGSTDisplay.Text = invoices.governmentTax.ToString("#0.00");
                        lblPSTDisplay.Text = invoices.provincialTax.ToString("#0.00");
                        lblSubtotalDisplay.Text = invoices.subTotal.ToString("#0.00");
                        lblTotalPaidDisplay.Text = invoices.balanceDue.ToString("#0.00");

                        object[] amounts = IM.ReturnTotalsForTenderAndChange(invoices);
                        lblTenderDisplay.Text = Convert.ToDouble(amounts[0]).ToString("#0.00");
                        lblChangeDisplay.Text = Convert.ToDouble(amounts[1]).ToString("#0.00");

                        if (invoices.invoiceSub > 1)
                        {
                            //    //Changes headers if the invoice is return
                            grdItemsSoldList.Columns[2].HeaderText = "Sold At";
                            grdItemsSoldList.Columns[3].HeaderText = "Non Refundable";
                            grdItemsSoldList.Columns[5].HeaderText = "Returned At";
                        }

                        //Binds the cart to the grid view
                        grdItemsSoldList.DataSource = invoices.soldItems;
                        grdItemsSoldList.DataBind();

                        //Displays the total amount ppaid
                        //Binds the payment methods to a gridview
                        grdMOPS.DataSource = invoices.usedMops;
                        grdMOPS.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}