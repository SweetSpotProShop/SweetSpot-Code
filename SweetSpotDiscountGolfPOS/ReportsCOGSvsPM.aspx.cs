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
    public partial class ReportsCOGSvsPM : System.Web.UI.Page
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
                        lblDates.Text = "COGs vs PM for: " + startDate.ToString("d");
                    }
                    else
                    {
                        lblDates.Text = "COGs vs PM for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                    }

                    List<Items> items = new List<Items>();
                    //Binding the gridview
                    items = r.returnItemsForCOGS(startDate, endDate, locationID);
                    grdInvoiceSelection.DataSource = items;
                    grdInvoiceSelection.DataBind();
                    //Displaying the total cost
                    lblTotalCostDisplay.Text = r.returnCOGSCost(startDate, endDate, locationID).ToString("C");
                    //Displaying the total price/sold at
                    lblSoldDisplay.Text = r.returnCOGSPrice(startDate, endDate, locationID).ToString("C");
                    //Displaying the profit margin
                    lblProfitMarginDisplay.Text = r.returnCOGSProfitMargin(startDate, endDate, locationID).ToString("C");


















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






















    }
}