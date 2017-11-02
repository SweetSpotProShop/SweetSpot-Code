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
    public partial class ReportsItemsSold : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CustomMessageBox cmb = new CustomMessageBox();
        CurrentUser cu = new CurrentUser();
        DateTime startDate;
        DateTime endDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsHomePage";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                if (!IsPostBack)
                {
                    //Gathering the start and end dates
                    Object[] passing = (Object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold for: " + startDate.ToString("d");
                    }
                    else
                    {
                        lblDates.Text = "Items sold for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                    }

                    List<Items> items = new List<Items>();
                    //Binding the gridview
                    items = r.returnItemsSold(startDate, endDate, locationID);
                    //Checking if there are any values
                    if (items.Count > 0)
                    {
                        grdItems.DataSource = items;
                        grdItems.DataBind();
                    }
                    else
                    {
                        if (startDate == endDate)
                        {
                            lblDates.Text = "There are no items sold for: " + startDate.ToString("d");
                        }
                        else
                        {
                            lblDates.Text = "There are no items sold for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                        }
                    }
                }

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                er.logError(ex, cu.empID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label percent = (Label)e.Row.FindControl("lblPercentage");
                Label discount = (Label)e.Row.FindControl("lblTotalDiscount");
                if (percent.Text.Equals("True"))
                {
                    discount.Text = discount.Text + "%";
                }
                else
                {
                    if (discount.Text.Equals("0"))
                    {
                        discount.Text = "-";
                    }
                    else
                    {
                        discount.Text = "$" + discount.Text;
                    }
                }

                Label lblProfit = (Label)e.Row.FindControl("lblTotalProfit");
                string profitText = lblProfit.Text;
                if (profitText.Contains("("))
                {
                    lblProfit.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void lbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "lbtnInvoiceNumber_Click";
            try
            {
                //Text of the linkbutton
                LinkButton btn = sender as LinkButton;
                string invoice = btn.Text;
                //Parsing into invoiceNum and invoiceSubNum
                char[] splitchar = { '-' };
                string[] invoiceSplit = invoice.Split(splitchar);
                int invNum = Convert.ToInt32(invoiceSplit[0]);
                int invSNum = Convert.ToInt32(invoiceSplit[1]);
                //determines the table to use for queries
                string table = "";
                int tran = 3;
                if (invSNum > 1)
                {
                    table = "Returns";
                    tran = 4;
                }
                //Stores required info into Sessions
                Invoice rInvoice = ssm.getSingleInvoice(invNum, invSNum);
                //Session["key"] = rInvoice.customerID;
                //Session["Invoice"] = invoice;
                Session["actualInvoiceInfo"] = rInvoice;
                Session["useInvoice"] = true;
                //Session["strDate"] = rInvoice.invoiceDate;
                Session["ItemsInCart"] = ssm.invoice_getItems(invNum, invSNum, "tbl_invoiceItem" + table);
                Session["CheckOutTotals"] = ssm.invoice_getCheckoutTotals(invNum, invSNum, "tbl_invoice");
                Session["MethodsOfPayment"] = ssm.invoice_getMOP(invNum, invSNum, "tbl_invoiceMOP");
                Session["TranType"] = tran;
                //Changes page to display a printable invoice
                Server.Transfer("PrintableInvoice.aspx", false);
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