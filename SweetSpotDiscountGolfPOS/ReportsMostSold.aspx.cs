using OfficeOpenXml;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsMSI : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();

        SweetShopManager ssm = new SweetShopManager();
        Reports r = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager l = new LocationManager();
        CustomMessageBox cmb = new CustomMessageBox();
        DateTime startDate;
        DateTime endDate;

        System.Data.DataTable items = new System.Data.DataTable();
        System.Data.DataTable models = new System.Data.DataTable();
        System.Data.DataTable brands = new System.Data.DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "ReportsMostSold";
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
                    Object[] passing = (Object[])Session["reportInfo"];
                    DateTime[] reportDates = (DateTime[])passing[0];
                    DateTime startDate = reportDates[0];
                    DateTime endDate = reportDates[1];
                    int locationID = (int)passing[1];
                    //Builds string to display in label
                    if (startDate == endDate)
                    {
                        lblDates.Text = "Items sold for: " + startDate.ToString("d") + " for " + l.locationName(locationID);
                    }
                    else
                    {
                        lblDates.Text = "Items sold for: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                    }

                    items.Columns.Add("sku");
                    items.Columns.Add("amountSold");
                    brands.Columns.Add("brand");
                    brands.Columns.Add("amountSold");
                    models.Columns.Add("models");
                    models.Columns.Add("amountSold");

                    //Binding the gridview
                    items = r.mostSoldItemsReport(startDate, endDate, locationID);
                    brands = r.mostSoldBrandsReport(startDate, endDate, locationID);
                    models = r.mostSoldModelsReport(startDate, endDate, locationID);
                    //Checking if there are any values
                    if (items.Rows.Count > 0 && brands.Rows.Count > 0 && models.Rows.Count > 0)
                    {
                        grdItems.DataSource = items;
                        grdItems.DataBind();
                        grdBrands.DataSource = brands;
                        grdBrands.DataBind();
                        grdModels.DataSource = models;
                        grdModels.DataBind();
                    }
                    else
                    {
                        if (startDate == endDate)
                        {
                            lblDates.Text = "There is no data for: " + startDate.ToString("d") + " for " + l.locationName(locationID);
                        }
                        else
                        {
                            lblDates.Text = "There is no data for: " + startDate.ToString("d") + " to " + endDate.ToString("d") + " for " + l.locationName(locationID);
                        }
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                Object[] passing = (Object[])Session["reportInfo"];
                string loc = l.locationName(Convert.ToInt32(passing[1]));
                string fileName = "Top Selling Report - " + loc + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + fileName);
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet itemsPage = xlPackage.Workbook.Worksheets.Add("Items");
                    ExcelWorksheet modelsPage = xlPackage.Workbook.Worksheets.Add("Models");
                    ExcelWorksheet brandsPage = xlPackage.Workbook.Worksheets.Add("Brands");
                    //Writing       
                    itemsPage.Cells[1, 1].Value = lblDates.Text; modelsPage.Cells[1, 1].Value = lblDates.Text; brandsPage.Cells[1, 1].Value = lblDates.Text;
                    itemsPage.Cells[2, 1].Value = "Items"; modelsPage.Cells[2, 1].Value = "Models"; brandsPage.Cells[2, 1].Value = "Brands";
                    itemsPage.Cells[3, 1].Value = "SKU"; itemsPage.Cells[3, 2].Value = "Amount Sold";
                    modelsPage.Cells[3, 1].Value = "Model"; modelsPage.Cells[3, 2].Value = "Times Sold";
                    brandsPage.Cells[3, 1].Value = "Brand"; brandsPage.Cells[3, 2].Value = "Times Sold"; 
                    int recordIndexItems = 4;
                    if (items.Rows.Count > 0)
                    {
                        foreach (DataRow i in items.Rows) 
                        {
                            itemsPage.Cells[recordIndexItems, 1].Value = i[0];
                            itemsPage.Cells[recordIndexItems, 2].Value = i[1];
                            recordIndexItems++;
                        }
                    }
                    int recordIndexModels = 4;
                    if (models.Rows.Count > 0)
                    {
                        foreach (DataRow m in models.Rows) 
                        {
                            modelsPage.Cells[recordIndexModels, 1].Value = m[0];
                            modelsPage.Cells[recordIndexModels, 2].Value = m[1];
                            recordIndexModels++;
                        }
                    }
                    int recordIndexBrands = 4;
                    if (brands.Rows.Count > 0)
                    {
                        foreach (DataRow b in brands.Rows) 
                        {
                            brandsPage.Cells[recordIndexBrands, 1].Value = b[0];
                            brandsPage.Cells[recordIndexBrands, 2].Value = b[1];
                            recordIndexBrands++;
                        }
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}