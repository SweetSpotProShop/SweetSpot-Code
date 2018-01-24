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

        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
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
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                //get current customer from customer number session
                int custNum = (Convert.ToInt32(Session["key"].ToString()));
                //Store in Customer class
                Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                //display information on receipt
                lblCustomerName.Text = c.firstName.ToString() + " " + c.lastName.ToString();
                lblStreetAddress.Text = c.primaryAddress.ToString();
                //lblPostalAddress.Text = c.city.ToString() + ", " + lm.provinceName(c.province) + " " + c.postalCode.ToString();
                lblPhone.Text = c.primaryPhoneNumber.ToString();
                lblinvoiceNum.Text = Convert.ToString(Session["Invoice"]);
                lblDate.Text = Convert.ToDateTime(Session["strDate"]).ToString("yyyy-MM-dd");
                Location l = new Location();
                //Gather transaction type from Session
                
                int ln = 0;
                //Determins the session to get the cart items from
                cart = (List<Cart>)Session["ItemsInCart"];
                ln = CU.locationID;
                //else if(tranType == 6){ cart = (List<Cart>)Session["ItemsInCart"]; ln = lm.returnlocationIDFromReceiptNumber(Convert.ToInt32(Session["Invoice"])); }
                //Use current location to display on invoice
                //l = lm.returnLocationForReceiptFromID(ln);

                //Display the location information
                //lblSweetShopName.Text = l.location.ToString();
                lblSweetShopStreetAddress.Text = l.address.ToString();
                //lblSweetShopPostalAddress.Text = l.city.ToString() + ", " + lm.provinceName(l.provID) + " " + l.postal.ToString();
                //lblSweetShopPhone.Text = l.phone.ToString();
                
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