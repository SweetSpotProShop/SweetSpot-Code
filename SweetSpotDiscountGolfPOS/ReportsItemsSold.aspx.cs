using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsItemsSold : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        LocationManager LM = new LocationManager();

        //ItemDataUtilities idu = new ItemDataUtilities();        
        double tCost;
        double tPrice;
        double tProfit;
        DataTable items = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsItemsSold";
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
                    //Gathering the start and end dates
                    object[] passing = (object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("dd/MMM/yy") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }
                    else
                    {
                        lblDates.Text = "Items sold on: " + startDate.ToString("dd/MMM/yy") + " to " + endDate.ToString("dd/MMM/yy") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                    }

                    //Binding the gridview
                    items = R.returnItemsSold(startDate, endDate, locationID, objPageDetails);
                    //Checking if there are any values
                    if (items.Rows.Count > 0)
                    {
                        grdItems.DataSource = items;
                        grdItems.DataBind();
                    }
                    else
                    {
                        if (startDate == endDate)
                        {
                            lblDates.Text = "There are no items sold for: " + startDate.ToString("d");
                        }
                        else
                        {
                            lblDates.Text = "There are no items sold for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                        }
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "grdItems_RowDataBound";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label percent = (Label)e.Row.FindControl("lblPercentage");
                    Label discount = (Label)e.Row.FindControl("lblTotalDiscount");
                    if (percent.Text.Equals("True"))
                    {
                        discount.Text = discount.Text + "%";
                    }
                    else
                    {
                        if (discount.Text.Equals("0"))
                        {
                            discount.Text = "-";
                        }
                        else
                        {
                            discount.Text = "$" + discount.Text;
                        }
                    }
                    Label lblProfit = (Label)e.Row.FindControl("lblTotalProfit");
                    string profitText = lblProfit.Text;
                    if (profitText.Contains("("))
                    {
                        lblProfit.ForeColor = System.Drawing.Color.Red;
                    }
                    tCost += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "cost"));
                    tPrice += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "price"));
                    tProfit += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "profit"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[2].Text = String.Format("{0:C}", tCost);
                    e.Row.Cells[3].Text = String.Format("{0:C}", tPrice);
                    e.Row.Cells[6].Text = String.Format("{0:C}", tProfit);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Text of the linkbutton
                LinkButton btn = sender as LinkButton;
                string invoice = btn.Text;
                //Changes page to display a printable invoice
                Server.Transfer("PrintableInvoice.aspx?inv=" + invoice, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                object[] passing = (object[])Session["reportInfo"];
                string loc = LM.ReturnLocationName(Convert.ToInt32(passing[1]), objPageDetails);
                string fileName = "Items Sold Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet itemsSoldExport = xlPackage.Workbook.Worksheets.Add("Items Sold");
                    // write to sheet   
                    itemsSoldExport.Cells[1, 1].Value = lblDates.Text;
                    itemsSoldExport.Cells[2, 1].Value = "Invoice Number";
                    itemsSoldExport.Cells[2, 2].Value = "SKU";
                    itemsSoldExport.Cells[2, 3].Value = "Item Cost";
                    itemsSoldExport.Cells[2, 4].Value = "Item Price";
                    itemsSoldExport.Cells[2, 5].Value = "Item Discount";
                    itemsSoldExport.Cells[2, 6].Value = "Item Profit";
                    int recordIndex = 3;
                    foreach (DataRow i in items.Rows) 
                    {
                        //itemsSoldExport.Cells[recordIndex, 1].Value = i.invoice;
                        itemsSoldExport.Cells[recordIndex, 1].Value = i[0].ToString();
                        itemsSoldExport.Cells[recordIndex, 2].Value = i[1].ToString();
                        itemsSoldExport.Cells[recordIndex, 3].Value = Convert.ToDouble(i[2].ToString());
                        itemsSoldExport.Cells[recordIndex, 4].Value = Convert.ToDouble(i[3].ToString());
                        if (Convert.ToBoolean(i[5]))
                        {
                            itemsSoldExport.Cells[recordIndex, 5].Value = i[4].ToString() + "%";
                        }
                        else
                        {
                            itemsSoldExport.Cells[recordIndex, 5].Value = "$" + i[4].ToString();
                        }
                        itemsSoldExport.Cells[recordIndex, 6].Value = Convert.ToDouble(i[6].ToString());
                        recordIndex++;
                    }
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}