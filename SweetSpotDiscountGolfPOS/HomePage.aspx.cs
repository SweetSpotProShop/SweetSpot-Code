using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class HomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        Reports R = new Reports();
        LocationManager LM = new LocationManager();
        CurrentUser CU;
        int totalSales = 0;
        double totalDiscounts = 0;
        double totalTradeIns = 0;
        double totalSubtotals = 0;
        double totalGST = 0;
        double totalPST = 0;
        double totalBalancePaid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "HomePage.aspx";
            //Session["prevPage"] = "HomePage.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (!this.IsPostBack)
                {
                    ddlLocation.DataSource = LM.ReturnLocationDropDown();
                    ddlLocation.DataTextField = "locationName";
                    ddlLocation.DataValueField = "locationID";
                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = CU.locationID.ToString();
                }
                //Checks user for admin status
                if (CU.jobID == 0)
                {
                    lbluser.Text = "You have Admin Access";
                    lbluser.Visible = true;
                }
                else/* if (Session["Loc"] != null)*/
                {
                    //If no admin status shows location as label instead of drop down
                    //lblLocation.Text = CU.locationName;
                    //lblLocation.Visible = true;
                    //ddlLocation.Visible = false;
                    ddlLocation.Enabled = false;
                }
                //populate gridview with todays sales
                grdSameDaySales.DataSource = R.getInvoiceBySaleDate(DateTime.Today, DateTime.Today, Convert.ToInt32(ddlLocation.SelectedValue));
                grdSameDaySales.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
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
                //Changes page to display a printable invoice
                Response.Redirect("PrintableInvoice.aspx?inv=" + invoice, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdSameDaySales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdSameDaySales_RowDataBound";
            //Current method does nothing
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    totalSales += 1;
                    totalDiscounts += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discountAmount"));
                    totalTradeIns += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "tradeinAmount"));
                    totalSubtotals += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "subTotal"));
                    totalGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "governmentTax"));
                    totalPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provincialTax"));
                    totalBalancePaid += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "balanceDue"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[1].Text = totalSales.ToString();
                    e.Row.Cells[2].Text = String.Format("{0:C}", totalDiscounts);
                    e.Row.Cells[3].Text = String.Format("{0:C}", totalTradeIns);
                    e.Row.Cells[4].Text = String.Format("{0:C}", totalSubtotals);
                    e.Row.Cells[5].Text = String.Format("{0:C}", totalGST);
                    e.Row.Cells[6].Text = String.Format("{0:C}", totalPST);
                    e.Row.Cells[7].Text = String.Format("{0:C}", totalBalancePaid);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V2.1 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}