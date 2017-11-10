using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsPaymentType : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CustomMessageBox cmb = new CustomMessageBox();
        CurrentUser cu = new CurrentUser();
        LocationManager l = new LocationManager();
        DateTime startDate;
        DateTime endDate;
        double salesCash;
        double salesDebit;
        double salesGiftCard;
        double salesMastercard;
        double salesVisa;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsSales";
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
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " for " + l.locationName(locationID);
                    }
                    else
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                    }
                    DataTable dt = r.returnSalesByPaymentTypeForSelectedDate(passing);
                    grdSalesByDate.DataSource = dt;
                    grdSalesByDate.DataBind();
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
        protected void grdSalesByDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                salesCash += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Cash"));
                salesDebit += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Debit"));
                salesGiftCard += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "GiftCard"));
                salesMastercard += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Mastercard"));
                salesVisa += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Visa"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[1].Text = String.Format("{0:C}", salesCash);
                e.Row.Cells[2].Text = String.Format("{0:C}", salesDebit);
                e.Row.Cells[3].Text = String.Format("{0:C}", salesGiftCard);
                e.Row.Cells[4].Text = String.Format("{0:C}", salesMastercard);
                e.Row.Cells[5].Text = String.Format("{0:C}", salesVisa);
            }
        }
    }
}