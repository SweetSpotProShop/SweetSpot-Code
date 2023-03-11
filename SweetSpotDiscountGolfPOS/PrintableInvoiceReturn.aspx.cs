using System;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class PrintableInvoiceReturn : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly CustomerManager CM = new CustomerManager();
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

                        Customer cust = new Customer();
                        cust = CM.CallReturnCustomer(invoice.intCustomerID, objPageDetails)[0];
                        //display information on receipt
                        lblCustomerName.Text = cust.varFirstName.ToString() + " " + cust.varLastName.ToString();
                        lblStreetAddress.Text = cust.varAddress.ToString();
                        lblPostalAddress.Text = cust.varCityName.ToString() + ", " + LM.CallReturnProvinceName(cust.intProvinceID, objPageDetails) + " " + cust.varPostalCode.ToString();
                        lblPhone.Text = cust.varContactNumber.ToString();
                        lblinvoiceNum.Text = invoice.varInvoiceNumber.ToString() + "-" + invoice.intInvoiceSubNumber.ToString();
                        lblDate.Text = invoice.dtmInvoiceDate.ToString("dd/MMM/yy");
                        lblTime.Text = invoice.dtmInvoiceTime.ToString("h:mm tt");

                        Location loco = new Location();
                        loco = LM.CallReturnLocation(invoice.intLocationID, objPageDetails)[0];
                        //Display the location information
                        lblSweetShopName.Text = loco.varLocationName.ToString();
                        lblSweetShopStreetAddress.Text = loco.varAddress.ToString();
                        lblSweetShopPostalAddress.Text = loco.varCityName.ToString() + ", " + LM.CallReturnProvinceName(loco.intProvinceID, objPageDetails) + " " + loco.varPostalCode.ToString();
                        lblSweetShopPhone.Text = loco.varContactNumber.ToString();
                        lblTaxNum.Text = loco.varTaxNumber.ToString();

                        double governmentTax = 0;
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
                                        lblGST.Visible = true;
                                        lblGST.Text = invoiceItemTax.varTaxName;
                                        lblGSTDisplay.Visible = true;
                                    }
                                }
                                else if (invoiceItemTax.intTaxTypeID == 2 || invoiceItemTax.intTaxTypeID == 4 || invoiceItemTax.intTaxTypeID == 5)
                                {
                                    if (invoiceItemTax.bitIsTaxCharged)
                                    {
                                        provincialTax += invoiceItemTax.fltTaxAmount;
                                        lblPST.Visible = true;
                                        lblPST.Text = invoiceItemTax.varTaxName;
                                        lblPSTDisplay.Visible = true;
                                    }
                                }
                                else if (invoiceItemTax.intTaxTypeID == 6)
                                {
                                    if (invoiceItemTax.bitIsTaxCharged)
                                    {
                                        liquorTax += invoiceItemTax.fltTaxAmount;
                                        lblLCT.Visible = true;
                                        lblLCT.Text = invoiceItemTax.varTaxName;
                                        lblLCTDisplay.Visible = true;
                                    }
                                }
                            }
                        }

                        lblGSTDisplay.Text = governmentTax.ToString("C");
                        lblPSTDisplay.Text = provincialTax.ToString("C");
                        lblLCTDisplay.Text = liquorTax.ToString("C");

                        double taxAmount = governmentTax + provincialTax + liquorTax;

                        //Display the totals
                        lblDiscountsDisplay.Text = invoice.fltTotalDiscount.ToString("C");
                        lblTradeInsDisplay.Text = invoice.fltTotalTradeIn.ToString("C");
                        lblShippingDisplay.Text = invoice.fltShippingCharges.ToString("C");
                        
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

                        //Displays the total amount paid
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