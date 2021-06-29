using System;
using System.Web.UI.WebControls;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InvoiceSearch : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly InvoiceManager IM = new InvoiceManager();
        readonly LocationManager LM = new LocationManager();
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
                        CalStartDate.SelectedDate = DateTime.Today;
                        CalEndDate.SelectedDate = DateTime.Today;
                        ddlLocation.DataSource = LM.CallReturnLocationDropDown(objPageDetails);
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = CU.location.intLocationID.ToString();
                    }
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
        protected void CalStart_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "CalStart_SelectionChanged";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try {}
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
        protected void CalEnd_SelectionChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "CalEnd_SelectionChanged";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try {}
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
        protected void BtnInvoiceSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnInvoiceSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Binds invoice list to the grid view
                GrdInvoiceSelection.DataSource = IM.CallReturnInvoicesBasedOnSearchCriteria(CalStartDate.SelectedDate, CalEndDate.SelectedDate, txtInvoiceNum.Text, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                GrdInvoiceSelection.DataBind();
                MergeRows(GrdInvoiceSelection);
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
        protected void GrdInvoiceSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInvoiceSelection_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets the string of the command argument(invoice number
                int invoiceID = Convert.ToInt32(e.CommandArgument.ToString());

                InvoiceManager IM = new InvoiceManager();
                if (IM.InvoiceIsReturn(invoiceID, objPageDetails))
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoiceReturn.aspx?invoice=" + invoiceID.ToString(), false);
                }
                else
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoice.aspx?invoice=" + invoiceID.ToString(), false);
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
        protected void GrdInvoiceSelection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Problems with looping
            //Collects current method for error tracking
            string method = "GrdInvoiceSelection_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void MergeRows(GridView gridView)
        {
            string method = "MergeRows";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow row = gridView.Rows[rowIndex];
                    GridViewRow previousRow = gridView.Rows[rowIndex + 1];
                    //string rowText = row.Cells[1].Text;
                    //string previousRowText = previousRow.Cells[1].Text;
                    string rowInvoice = "";
                    string prevRowInvoice = "";
                    LinkButton lbtnRow = (LinkButton)row.FindControl("lkbInvoiceNum");
                    LinkButton lbtnPrevRow = (LinkButton)previousRow.FindControl("lkbInvoiceNum");
                    if (lbtnRow != null) { rowInvoice = lbtnRow.Text; }
                    if (lbtnPrevRow != null) { prevRowInvoice = lbtnPrevRow.Text; }
                    if (rowInvoice.Equals(prevRowInvoice) || prevRowInvoice.Equals(""))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 // merge the first two cells
                                             : previousRow.Cells[i].RowSpan + 1; //any subsequent merging
                            previousRow.Cells[i].Visible = false;

                        }
                        row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 // merge the first two cells
                                             : previousRow.Cells[10].RowSpan + 1; //any subsequent merging
                        previousRow.Cells[10].Visible = false;
                    }
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
    }
}