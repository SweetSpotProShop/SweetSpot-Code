using System;
using System.Data;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsCostOfInventory : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly Reports R = new Reports();
        CurrentUser CU;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsCostOfInventory";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    //Binding the gridview
#pragma warning disable IDE0067 // Dispose objects before losing scope
                    DataTable list = new DataTable();
#pragma warning restore IDE0067 // Dispose objects before losing scope
                    list = R.CallCostOfInventoryReport(objPageDetails);
                    //Checking if there are any values
                    if (list.Rows.Count > 0)
                    {
                        grdCostOfInventory.DataSource = list;
                        grdCostOfInventory.DataBind();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}