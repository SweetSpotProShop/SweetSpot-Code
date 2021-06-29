using System;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class PrintableInvoice : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly InvoiceManager IM = new InvoiceManager();
        CurrentUser CU;
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
                        Invoice invoice = IM.CallReturnInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), objPageDetails)[0];

                        //display information on receipt
                        lblCustomerName.Text = invoice.customer.varFirstName.ToString() + " " + invoice.customer.varLastName.ToString();
                        lblStreetAddress.Text = invoice.customer.varAddress.ToString();
                        lblPostalAddress.Text = invoice.customer.varCityName.ToString() + ", " + LM.CallReturnProvinceName(invoice.customer.intProvinceID, objPageDetails) + " " + invoice.customer.varPostalCode.ToString();
                        lblPhone.Text = invoice.customer.varContactNumber.ToString();
                        lblinvoiceNum.Text = invoice.varInvoiceNumber.ToString() + "-" + invoice.intInvoiceSubNumber.ToString();
                        lblDate.Text = invoice.dtmInvoiceDate.ToString("dd/MMM/yy");
                        lblTime.Text = invoice.dtmInvoiceTime.ToString("h:mm tt");

                        //Display the location information
                        lblSweetShopName.Text = invoice.location.varLocationName.ToString();
                        lblSweetShopStreetAddress.Text = invoice.location.varAddress.ToString();
                        lblSweetShopPostalAddress.Text = invoice.location.varCityName.ToString() + ", " + LM.CallReturnProvinceName(invoice.location.intProvinceID, objPageDetails) + " " + invoice.location.varPostalCode.ToString();
                        lblSweetShopPhone.Text = invoice.location.varContactNumber.ToString();
                        lblTaxNum.Text = invoice.location.varTaxNumber.ToString();

                        //Display the totals
                        lblDiscountsDisplay.Text = invoice.fltTotalDiscount.ToString("C");
                        lblTradeInsDisplay.Text = invoice.fltTotalTradeIn.ToString("C");
                        lblShippingDisplay.Text = invoice.fltShippingCharges.ToString("C");

                        double governmentTax = 0 + invoice.fltShippingTaxAmount;
                        double provincialTax = 0;
                        double liquorTax = 0;

                        foreach (var invoiceItem in invoice.invoiceItems)
                        {
                            foreach (var invoiceItemTax in invoiceItem.invoiceItemTaxes)
                            {
                                if (invoiceItemTax.intTaxTypeID == 1 || invoiceItemTax.intTaxTypeID == 3)
                                {
                                    if (invoiceItemTax.bitIsTaxCharged)
                                    {
                                        governmentTax += invoiceItemTax.fltTaxAmount;
                                        lblGovernment.Visible = true;
                                        lblGovernment.Text = invoiceItemTax.varTaxName;
                                        lblGovernmentAmount.Visible = true;
                                    }
                                }
                                else if (invoiceItemTax.intTaxTypeID == 2 || invoiceItemTax.intTaxTypeID == 4 || invoiceItemTax.intTaxTypeID == 5)
                                {
                                    if (invoiceItemTax.bitIsTaxCharged)
                                    {
                                        provincialTax += invoiceItemTax.fltTaxAmount;
                                        lblProvincial.Visible = true;
                                        lblProvincial.Text = invoiceItemTax.varTaxName;
                                        lblProvincialAmount.Visible = true;
                                    }
                                }
                                else if (invoiceItemTax.intTaxTypeID == 6)
                                {
                                    if (invoiceItemTax.bitIsTaxCharged)
                                    {
                                        liquorTax += invoiceItemTax.fltTaxAmount;
                                        lblLiquorTax.Visible = true;
                                        lblLiquorTax.Text = invoiceItemTax.varTaxName;
                                        lblLiquorTaxAmount.Visible = true;
                                    }
                                }
                            }
                        }

                        lblGovernmentAmount.Text = governmentTax.ToString("C");
                        lblProvincialAmount.Text = provincialTax.ToString("C");
                        lblLiquorTaxAmount.Text = liquorTax.ToString("C");

                        double taxAmount = governmentTax + provincialTax + liquorTax;

                        lblSubtotalDisplay.Text = (invoice.fltSubTotal + invoice.fltShippingCharges).ToString("C");
                        lblTotalPaidDisplay.Text = (invoice.fltBalanceDue + taxAmount).ToString("C");

                        object[] amounts = IM.ReturnTotalsForTenderAndChange(invoice);
                        lblTenderDisplay.Text = Convert.ToDouble(amounts[0]).ToString("C");
                        lblChangeDisplay.Text = Convert.ToDouble(amounts[1]).ToString("C");

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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnHome_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnHome_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}