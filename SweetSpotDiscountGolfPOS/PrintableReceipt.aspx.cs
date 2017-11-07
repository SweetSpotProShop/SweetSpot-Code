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
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        LocationManager lm = new LocationManager();
        List<Checkout> mopList = new List<Checkout>();
        List<Cart> cart = new List<Cart>();
        CheckoutManager ckm = new CheckoutManager();
        CurrentUser cu;
        int tranType;
        double dblAmountPaid;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "PrintableReceipt.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                //get current customer from customer number session
                int custNum = (Convert.ToInt32(Session["key"].ToString()));
                //Store in Customer class
                Customer c = ssm.GetCustomerbyCustomerNumber(custNum);
                //display information on receipt
                lblCustomerName.Text = c.firstName.ToString() + " " + c.lastName.ToString();
                lblStreetAddress.Text = c.primaryAddress.ToString();
                lblPostalAddress.Text = c.city.ToString() + ", " + lm.provinceName(c.province) + " " + c.postalCode.ToString();
                lblPhone.Text = c.primaryPhoneNumber.ToString();
                lblinvoiceNum.Text = Convert.ToString(Session["Invoice"]);
                lblDate.Text = Convert.ToDateTime(Session["strDate"]).ToString("yyyy-MM-dd");
                Location l = new Location();

                //Use current location to display on invoice
                l = lm.returnLocationForInvoice(cu.locationName);

                //Display the location information
                lblSweetShopName.Text = l.location.ToString();
                lblSweetShopStreetAddress.Text = l.address.ToString();
                lblSweetShopPostalAddress.Text = l.city.ToString() + ", " + lm.provinceName(l.provID) + " " + l.postal.ToString();
                lblSweetShopPhone.Text = l.phone.ToString();

                //Gather transaction type from Session
                tranType = Convert.ToInt32(Session["TranType"]);
                //Determins the session to get the cart items from
                if (tranType == 5) { cart = (List<Cart>)Session["ItemsInCart"]; }
                //Gathers stored totals
                ckm = (CheckoutManager)Session["CheckOutTotals"];
                //Gathers stored payment methods
                mopList = (List<Checkout>)Session["MethodsofPayment"];
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

                foreach (GridViewRow row in grdItemsBoughtList.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
                }

                //Displays the total amount ppaid
                lblTotalPaidDisplay.Text = "$ " + dblAmountPaid.ToString("#0.00");
                //Binds the payment methods to a gridview
                grdMOPS.DataSource = mopList;
                grdMOPS.DataBind();
                foreach (GridViewRow row in grdMOPS.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        cell.Attributes.CssStyle["text-align"] = "center";
                    }
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
                Server.Transfer("HomePage.aspx", false);
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