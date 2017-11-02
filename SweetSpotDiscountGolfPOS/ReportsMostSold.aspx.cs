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
    public partial class ReportsMSI : System.Web.UI.Page
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
                    List<Items> models = new List<Items>();
                    List<Items> brands = new List<Items>();
                    //Binding the gridview
                    items = r.mostSoldItemsReport(startDate, endDate, locationID);
                    brands = r.mostSoldBrandsReport(startDate, endDate, locationID);
                    models = r.mostSoldModelsReport(startDate, endDate, locationID);
                    //Checking if there are any values
                    if (items.Count > 0 && brands.Count > 0 && models.Count > 0)
                    {
                        grdItems.DataSource = items;
                        grdItems.DataBind();
                        grdBrands.DataSource = brands;
                        grdBrands.DataBind();
                        grdModels.DataSource = models;
                        grdModels.DataBind();
                    }
                    else
                    {
                        if (startDate == endDate)
                        {
                            lblDates.Text = "There is no data for: " + startDate.ToString("d");
                        }
                        else
                        {
                            lblDates.Text = "There is no data for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
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
    }
}