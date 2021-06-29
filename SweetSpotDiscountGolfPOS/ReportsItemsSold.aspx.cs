using OfficeOpenXml;
using System;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsItemsSold : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly Reports R = new Reports();
        CurrentUser CU;

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
                    ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                    //Builds string to display in label
                    lblDates.Text = "Items sold on: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString() + " for " + repInfo.varLocationName;
                    //Binding the gridview
                    //items = R.CallReturnItemsSold(repInfo, objPageDetails);
                    //Checking if there are any values
                    if (items.Rows.Count > 0)
                    {
                        GrdItems.DataSource = items;
                        GrdItems.DataBind();
                    }
                    else
                    {
                        lblDates.Text = "There are no items sold for: " + repInfo.dtmStartDate.ToShortDateString() + " to " + repInfo.dtmEndDate.ToShortDateString();
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
        protected void GrdItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string method = "GrdItems_RowDataBound";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label percent = (Label)e.Row.FindControl("lblPercentage");
                    Label discount = (Label)e.Row.FindControl("lblTotalDiscount");
                    if (percent.Text.Equals("True"))
                    {
                        discount.Text += "%";
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void LbtnInvoiceNumber_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "LbtnInvoiceNumber_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnDownload_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                ReportInformation repInfo = (ReportInformation)Session["reportInfo"];
                string fileName = "Items Sold Report - " + repInfo.varLocationName + ".xlsx";
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}