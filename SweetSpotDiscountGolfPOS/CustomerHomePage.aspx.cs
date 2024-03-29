﻿using System;
using System.Web;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class CustomerHomePage : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly CustomerManager CM = new CustomerManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "CustomerHomePage";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
        protected void BtnCustomerSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "BtnCustomerSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                GrdCustomersSearched.Visible = true;
                GrdCustomersSearched.DataSource = CM.CallReturnCustomerBasedOnText(txtSearch.Text, objPageDetails);
                GrdCustomersSearched.DataBind();
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
        protected void BtnAddNewCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "BtnAddNewCustomer_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Opens the page to add a new customer
                Response.Redirect("CustomerAddNew.aspx?customer=-10", false);
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
        protected void GrdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "GrdCustomersSearched_RowCommand";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.CommandName == "ViewProfile")
                {
                    //open Add New Customer page
                    Response.Redirect("CustomerAddNew.aspx?customer=" + e.CommandArgument.ToString(), false);
                }
                else if (e.CommandName == "StartSale")
                {
                    InvoiceManager IM = new InvoiceManager();
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", e.CommandArgument.ToString());
                    string invoice = "-10";
                    nameValues.Set("invoice", invoice);
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                    //Changes page to Sales Cart
                    Response.Redirect("SalesCart.aspx?" + nameValues, false);
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
        protected void GrdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "GrdCustomersSearched_PageIndexChanging";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdCustomersSearched.PageIndex = e.NewPageIndex;
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                GrdCustomersSearched.Visible = true;
                GrdCustomersSearched.DataSource = CM.CallReturnCustomerBasedOnText(txtSearch.Text, objPageDetails);
                GrdCustomersSearched.DataBind();
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