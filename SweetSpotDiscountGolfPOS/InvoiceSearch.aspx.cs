using System;
using SweetShop;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InvoiceSearch : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        InvoiceManager IM = new InvoiceManager();
        LocationManager LM = new LocationManager();
        CurrentUser CU;

        string oldInvoice = string.Empty;
        string newInvoice = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InvoiceSearch.aspx";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                    if (!IsPostBack)
                    {
                        //Sets the calendar and text boxes start and end dates
                        calStartDate.SelectedDate = DateTime.Today;
                        calEndDate.SelectedDate = DateTime.Today;
                        ddlLocation.DataSource = LM.ReturnLocationDropDown(objPageDetails);
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = CU.location.locationID.ToString();
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calStart_SelectionChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try {}
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "calEnd_SelectionChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try {}
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnInvoiceSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInvoiceSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Binds invoice list to the grid view
                grdInvoiceSelection.DataSource = IM.ReturnInvoicesBasedOnSearchCriteria(calStartDate.SelectedDate, calEndDate.SelectedDate, txtInvoiceNum.Text, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                grdInvoiceSelection.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        //Still Needs to be Updated
        protected void grdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdInvoiceSelection_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets the string of the command argument(invoice number
                string strInvoice = Convert.ToString(e.CommandArgument);
                //Checks that the command name is return invoice
                if (e.CommandName == "returnInvoice")
                {
                    //Changes to printable invoice page
                    Response.Redirect("PrintableInvoice.aspx?inv=" + strInvoice, false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void grdInvoiceSelection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Problems with looping
            //Collects current method for error tracking
            string method = "grdSameDaySales_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            //Current method does nothing
            try
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lkbInvoiceNum");
                if (lb != null)
                {
                    oldInvoice = lb.Text;
                    if (oldInvoice == newInvoice)
                    {
                        //Setting the contents of the cells to be blank and to remove them                        
                        for (int l = 2; l < 9; l++)
                        {
                            if (l == 8)
                            {
                                e.Row.Cells[l + 2].Visible = false;
                                e.Row.Cells[l + 2].Text = "";
                            }
                            else
                            {
                                e.Row.Cells[l].Visible = false;
                                e.Row.Cells[l].Text = "";
                            }
                        }
                        e.Row.Cells[0].Text = "";
                        e.Row.Cells[1].Text = "";
                        e.Row.Cells[1].ColumnSpan = 7;
                    }
                    else
                    {
                        newInvoice = oldInvoice;
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}