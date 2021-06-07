using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;


namespace SweetSpotDiscountGolfPOS
{
    public partial class HomePage : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

        int totalSales = 0;
        double totalDiscounts = 0;
        double totalTradeIns = 0;
        double totalSubtotals = 0;
        double totalGST = 0;
        double totalPST = 0;
        double totalBalancePaid = 0;
        double totalMOPAmount = 0;
        string oldInvoice = string.Empty;
        string newInvoice = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "HomePage.aspx";
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
                        ddlLocation.DataSource = LM.CallReturnLocationDropDown(objPageDetails);
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = CU.location.intLocationID.ToString();
                    }
                    //Checks user for admin status
                    if (CU.employee.intJobID == 0)
                    {
                        lbluser.Visible = true;
                        ddlLocation.Enabled = true;
                    }
                    //populate gridview with todays sales
                    ReportInformation repInfo = new ReportInformation();
                    repInfo.dtmStartDate = DateTime.Today;
                    repInfo.dtmEndDate = DateTime.Today;
                    repInfo.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);

                    GrdSameDaySales.DataSource = R.CallGetInvoiceBySaleDate(repInfo, objPageDetails);
                    GrdSameDaySales.DataBind();
                    MergeRows(GrdSameDaySales);
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
        protected void GrdSameDaySales_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //Problems with looping 
            //Collects current method for error tracking
            string method = "GrdSameDaySales_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            //Current method does nothing
            try
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lbtnInvoiceNumber");
                if (lb != null)
                {
                    oldInvoice = lb.Text;
                    if (oldInvoice == newInvoice)
                    {
                        //Setting the contents of the cells to be blank and to remove them                        
                        for (int l = 2; l < 9; l++)
                        {
                            e.Row.Cells[l].Visible = false;
                            e.Row.Cells[l].Text = "";
                        }
                        e.Row.Cells[0].Text = "";
                        e.Row.Cells[1].Text = "";
                        e.Row.Cells[1].ColumnSpan = 8;
                    }
                    else if (oldInvoice != newInvoice)
                    {
                        //This is where the totals at the bottom are calculated
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //This triggers when the row is not the footer(Not the end)
                            //Need to determine if the cell is empty
                            totalSales += 1;
                            totalDiscounts += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalDiscount"));
                            totalTradeIns += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTotalTradeIn"));
                            totalSubtotals += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSubTotal"));
                            totalGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTaxAmount"));
                            totalPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTaxAmount"));
                            totalBalancePaid += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltBalanceDue"));
                        }
                        newInvoice = oldInvoice;
                    }
                }
                //This is separate because every line will have a MOP on it
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    totalMOPAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltAmountPaid"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Triggers when the row is the footer(End)
                    e.Row.Cells[1].Text = totalSales.ToString();
                    e.Row.Cells[3].Text = String.Format("{0:C}", totalDiscounts);
                    e.Row.Cells[4].Text = String.Format("{0:C}", totalTradeIns);
                    e.Row.Cells[5].Text = String.Format("{0:C}", totalSubtotals);
                    e.Row.Cells[6].Text = String.Format("{0:C}", totalGST);
                    e.Row.Cells[7].Text = String.Format("{0:C}", totalPST);
                    e.Row.Cells[8].Text = String.Format("{0:C}", totalBalancePaid);
                    e.Row.Cells[10].Text = String.Format("{0:C}", totalMOPAmount);
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
                    LinkButton lbtnRow = (LinkButton)row.FindControl("lbtnInvoiceNumber");
                    LinkButton lbtnPrevRow = (LinkButton)previousRow.FindControl("lbtnInvoiceNumber");
                    if (lbtnRow != null) { rowInvoice = lbtnRow.Text; }                    
                    if (lbtnPrevRow != null) { prevRowInvoice = lbtnPrevRow.Text; }
                    if (rowInvoice.Equals(prevRowInvoice) || prevRowInvoice.Equals(""))
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 // merge the first two cells
                                             : previousRow.Cells[i].RowSpan + 1; //any subsequent merging
                            previousRow.Cells[i].Visible = false;

                        }
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
        protected void GrdSameDaySales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdSameDaySales_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                InvoiceManager IM = new InvoiceManager();
                if (IM.InvoiceIsReturn(Convert.ToInt32(e.CommandArgument.ToString()), objPageDetails))
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoiceReturn.aspx?invoice=" + e.CommandArgument.ToString(), false);
                }
                else
                {
                    //Changes page to display a printable invoice
                    Response.Redirect("PrintableInvoice.aspx?invoice=" + e.CommandArgument.ToString(), false);
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