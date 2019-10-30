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
    public partial class PrintableInvoiceReturn : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        LocationManager LM = new LocationManager();
        InvoiceManager IM = new InvoiceManager();
        //private static Invoice invoice;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PrintableInvoice.aspx";
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
                        Invoice invoice = IM.ReturnInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.location.intProvinceID, objPageDetails)[0];

                        //display information on receipt
                        lblCustomerName.Text = invoice.customer.varFirstName.ToString() + " " + invoice.customer.varLastName.ToString();
                        lblStreetAddress.Text = invoice.customer.varAddress.ToString();
                        lblPostalAddress.Text = invoice.customer.varCityName.ToString() + ", " + LM.ReturnProvinceName(invoice.customer.intProvinceID, objPageDetails) + " " + invoice.customer.varPostalCode.ToString();
                        lblPhone.Text = invoice.customer.varContactNumber.ToString();
                        lblinvoiceNum.Text = invoice.varInvoiceNumber.ToString() + "-" + invoice.intInvoiceSubNumber.ToString();
                        lblDate.Text = invoice.dtmInvoiceDate.ToString("dd/MMM/yy");
                        lblTime.Text = invoice.dtmInvoiceTime.ToString("h:mm tt");

                        //Display the location information
                        lblSweetShopName.Text = invoice.location.varLocationName.ToString();
                        lblSweetShopStreetAddress.Text = invoice.location.varAddress.ToString();
                        lblSweetShopPostalAddress.Text = invoice.location.varCityName.ToString() + ", " + LM.ReturnProvinceName(invoice.location.intProvinceID, objPageDetails) + " " + invoice.location.varPostalCode.ToString();
                        lblSweetShopPhone.Text = invoice.location.varContactNumber.ToString();
                        lblTaxNum.Text = invoice.location.varTaxNumber.ToString();

                        //Display the totals
                        lblDiscountsDisplay.Text = invoice.fltTotalDiscount.ToString("#0.00");
                        lblTradeInsDisplay.Text = invoice.fltTotalTradeIn.ToString("#0.00");
                        lblShippingDisplay.Text = invoice.fltShippingCharges.ToString("#0.00");
                        lblGSTDisplay.Text = invoice.fltGovernmentTaxAmount.ToString("#0.00");
                        lblPSTDisplay.Text = invoice.fltProvincialTaxAmount.ToString("#0.00");
                        lblSubtotalDisplay.Text = (invoice.fltSubTotal + invoice.fltShippingCharges).ToString("#0.00");
                        lblTotalPaidDisplay.Text = (invoice.fltBalanceDue + invoice.fltGovernmentTaxAmount + invoice.fltProvincialTaxAmount).ToString("#0.00");

                        object[] amounts = IM.ReturnTotalsForTenderAndChange(invoice);
                        lblTenderDisplay.Text = Convert.ToDouble(amounts[0]).ToString("#0.00");
                        lblChangeDisplay.Text = Convert.ToDouble(amounts[1]).ToString("#0.00");

                        if (invoice.intInvoiceSubNumber > 1)
                        {
                            //    //Changes headers if the invoice is return
                            grdItemsSoldList.Columns[2].HeaderText = "Sold At";
                            grdItemsSoldList.Columns[3].HeaderText = "Non Refundable";
                            grdItemsSoldList.Columns[5].HeaderText = "Returned At";
                        }

                        //Binds the cart to the grid view
                        grdItemsSoldList.DataSource = invoice.invoiceItems;
                        grdItemsSoldList.DataBind();

                        //Displays the total amount ppaid
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}