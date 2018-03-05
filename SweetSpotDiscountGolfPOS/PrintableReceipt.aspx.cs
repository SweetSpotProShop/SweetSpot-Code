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

        SweetShopManager ssm = new SweetShopManager();
        List<Mops> mopList = new List<Mops>();
        List<Cart> cart = new List<Cart>();
        CheckoutManager ckm = new CheckoutManager();
        int tranType;
        double dblAmountPaid;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PrintableReceipt.aspx";
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
                    //get current customer from customer number session
                    int custNum = (Convert.ToInt32(Session["key"].ToString()));
                    //Store in Customer class
                    Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                    //display information on receipt
                    lblCustomerName.Text = c.firstName.ToString() + " " + c.lastName.ToString();
                    lblStreetAddress.Text = c.primaryAddress.ToString();
                    lblPostalAddress.Text = c.city.ToString() + ", " + LM.ReturnProvinceName(c.province) + " " + c.postalCode.ToString();
                    lblPhone.Text = c.primaryPhoneNumber.ToString();
                    lblinvoiceNum.Text = Convert.ToString(Session["Invoice"]);
                    lblDate.Text = Convert.ToDateTime(Session["strDate"]).ToString("yyyy-MM-dd");
                    //Gather transaction type from Session
                    //Determins the session to get the cart items from
                    cart = (List<Cart>)Session["ItemsInCart"];
                    //Use current location to display on invoice
                    List<Location> l = LM.ReturnLocation(CU.locationID);

                    //Display the location information
                    lblSweetShopName.Text = l[0].locationName.ToString();
                    lblSweetShopStreetAddress.Text = l[0].address.ToString();
                    lblSweetShopPostalAddress.Text = l[0].city.ToString() + ", " + LM.ReturnProvinceName(l[0].provID) + " " + l[0].postal.ToString();
                    lblSweetShopPhone.Text = l[0].primaryPhone.ToString();

                    //Gathers stored totals
                    ckm = (CheckoutManager)Session["CheckOutTotals"];
                    //Gathers stored payment methods
                    mopList = (List<Mops>)Session["MethodsofPayment"];
                    //Displays subtotal
                    lblSubtotalDisplay.Text = "$ " + ckm.dblTotal.ToString("#0.00");
                    //Loops through each payment method and totlas them
                    foreach (var mop in mopList)
                    {
                        dblAmountPaid += mop.amountPaid;
                    }

                    //Binds the cart to the grid view
                    grdItemsBoughtList.DataSource = cart;
                    grdItemsBoughtList.DataBind();

                    //Displays the total amount ppaid
                    lblTotalPaidDisplay.Text = "$ " + dblAmountPaid.ToString("#0.00");
                    //Binds the payment methods to a gridview
                    grdMOPS.DataSource = mopList;
                    grdMOPS.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}