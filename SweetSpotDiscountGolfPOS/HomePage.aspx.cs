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
        double totalMOPAmount = 0;
        string oldInvoice = string.Empty;
        string newInvoice = string.Empty;
        int currentRow = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "HomePage.aspx";
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
                        ddlLocation.DataSource = LM.ReturnLocationDropDown();
                        ddlLocation.DataTextField = "locationName";
                        ddlLocation.DataValueField = "locationID";
                        ddlLocation.DataBind();
                        ddlLocation.SelectedValue = CU.location.locationID.ToString();
                    }
                    //Checks user for admin status
                    if (CU.jobID == 0)
                    {
                        lbluser.Text = "You have Admin Access";
                        lbluser.Visible = true;
                    }
                    else
                    {
                        //If no admin status shows location as label instead of drop down
                        ddlLocation.Enabled = false;
                    }
                    //populate gridview with todays sales
                    grdSameDaySales.DataSource = R.getInvoiceBySaleDate(DateTime.Today, DateTime.Today, Convert.ToInt32(ddlLocation.SelectedValue));
                    grdSameDaySales.DataBind();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdSameDaySales_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //Problems with looping 
            //Collects current method for error tracking
            string method = "grdSameDaySales_RowDataBound";
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
                            totalDiscounts += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "discountAmount"));
                            totalTradeIns += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "tradeinAmount"));
                            totalSubtotals += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "subTotal"));
                            totalGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "governmentTax"));
                            totalPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "provincialTax"));
                            totalBalancePaid += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "balanceDue"));
                        }
                        newInvoice = oldInvoice;
                    }
                }
                //This is separate because every line will have a MOP on it
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    totalMOPAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "amountPaid"));
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
                currentRow++;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void MergeRows(GridView gridView)
        {
            string method = "MergeRows";
            try
            {
                for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow row = gridView.Rows[rowIndex];
                    GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                    string rowText = row.Cells[1].Text;
                    string previousRowText = previousRow.Cells[1].Text;
                    string rowInvoice;
                    string prevRowInvoice;
                    LinkButton lbtnRow = (LinkButton)row.FindControl("lbtnInvoiceNumber");
                    LinkButton lbtnPrevRow = (LinkButton)previousRow.FindControl("lbtnInvoiceNumber");
                    if (lbtnRow != null)
                        rowInvoice = lbtnRow.Text;
                    else
                        rowInvoice = "";
                    if (lbtnPrevRow != null)
                        prevRowInvoice = lbtnPrevRow.Text;
                    else
                        prevRowInvoice = "";
                    if (rowText.Equals(previousRowText) && !rowText.isNumber() && !previousRowText.isNumber())
                    {
                        row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2
                                         ? 2 // merge the first two cells
                                         : previousRow.Cells[1].RowSpan + 1; //any subsequent merging
                        previousRow.Cells[1].Visible = false;
                    }
                    if (rowInvoice.Equals(prevRowInvoice) || prevRowInvoice.Equals(""))
                    {
                        row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2
                                         ? 2 // merge the first two cells
                                         : previousRow.Cells[0].RowSpan + 1; //any subsequent merging
                        previousRow.Cells[0].Visible = false;
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}