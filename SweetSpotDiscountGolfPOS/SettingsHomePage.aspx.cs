﻿using OfficeOpenXml;
using OfficeOpenXml.Style;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SweetSpotDiscountGolfPOS
{
    public partial class SettingsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();
        EmployeeManager EM = new EmployeeManager();
        Reports R = new Reports();
        TaxManager TM = new TaxManager();
        LocationManager LM = new LocationManager();
        DatabaseCalls dbc = new DatabaseCalls();

        //SweetShopManager ssm = new SweetShopManager();
        internal static readonly Page aspx;
        public int counter;
        public int total;
        public Label lblP;
        public string progress;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SettingsHomePage.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                else
                {
                    lblCurrentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    if (txtDate.Text == "")
                    {
                        txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    }
                    //Checks if the user is an Admin
                    if (CU.jobID != 0)
                    {
                        btnAddNewEmployee.Enabled = false;
                        btnLoadItems.Enabled = false;
                    }
                    if (!IsPostBack)
                    {
                        ddlProvince.DataSource = LM.ReturnProvinceDropDown(0);
                        ddlProvince.DataTextField = "provName";
                        ddlProvince.DataValueField = "provStateID";
                        ddlProvince.DataBind();
                        ddlProvince.SelectedValue = "1";
                        ddlTax.DataSource = TM.ReturnTaxListBasedOnDateAndProvinceForUpdate(1, Convert.ToDateTime(lblCurrentDate.Text));
                        ddlTax.DataTextField = "taxName";
                        ddlTax.DataValueField = "taxID";
                        ddlTax.DataBind();
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
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddNewEmployee_Click";
            try
            {
                //Change to Employee add new page
                Response.Redirect("EmployeeAddNew.aspx?emp=-10", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void grdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdEmployeesSearched_RowCommand";
            try
            {
                //Checks if the string is view profile
                if (e.CommandName == "ViewProfile")
                {
                    //Changes page to Employee Add New
                    Response.Redirect("EmployeeAddNew.aspx?emp=" + e.CommandArgument.ToString(), false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnEmployeeSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEmployeeSearch_Click";
            try
            {
                grdEmployeesSearched.Visible = true;
                //Binds the employee list to grid view
                grdEmployeesSearched.DataSource = EM.ReturnEmployeeBasedOnText(txtSearch.Text);
                grdEmployeesSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Importing
        protected void btnLoadItems_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnLoadItems_Click";
            try
            {
                int error = 0;
                //Verifies file has been selected
                if (fupItemSheet.HasFile)
                {
                    //load the uploaded file into the memorystream
                    using (MemoryStream stream = new MemoryStream(fupItemSheet.FileBytes))
                    //Lets the server know to use the excel package
                    using (ExcelPackage xlPackage = new ExcelPackage(stream))
                    {
                        //Gets the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                        //Gets the row count
                        var rowCnt = worksheet.Dimension.End.Row;
                        //Gets the column count
                        var colCnt = worksheet.Dimension.End.Column;
                        //Beginning the loop for data gathering
                        for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                        {
                            //Array of the cells that will need to be checked
                            int[] cells = { 3, 5, 6, 10, 11, 12, 13, 14, 15, 22 };
                            foreach (int column in cells)
                            {
                                //If there is no value in the column, proceed
                                if (worksheet.Cells[i, column].Value == null)
                                {
                                    worksheet.Cells[i, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, column].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    error = 1;
                                }
                            }
                        }
                        //xlPackage.SaveAs(new FileInfo(@"c:\temp\myFile.xls"));
                        if (error == 1)
                        {
                            //Sets the attributes and writes file
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment; filename=" + fupItemSheet.FileName + "_ErrorsFound" + ".xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(xlPackage.GetAsByteArray());
                            Response.End();
                        }
                        else
                        {
                            //Calls method to import the requested file
                            DataTable errors = new DataTable();
                            errors.Columns.Add("sku");
                            errors.Columns.Add("brandError");
                            errors.Columns.Add("modelError");
                            errors.Columns.Add("identifierError");
                            errors = R.uploadItems(fupItemSheet);
                            if (errors.Rows.Count != 0)
                            {
                                foreach (DataRow row in errors.Rows)
                                {
                                    for (int i = 2; i <= rowCnt; i++)
                                    {
                                        //Column 3 should be the skus

                                        if ((worksheet.Cells[i, 3].Value).ToString().Equals(row[0].ToString()))
                                        {
                                            worksheet.Cells[i, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            //If the brand caused an error
                                            if (Convert.ToInt32(row[1]) == 1)
                                            {
                                                worksheet.Cells[i, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 5].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            //If the model caused an error
                                            if (Convert.ToInt32(row[2]) == 1)
                                            {
                                                worksheet.Cells[i, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            //If the secondary identifier(Destination) caused an error
                                            if (Convert.ToInt32(row[3]) == 1)
                                            {
                                                worksheet.Cells[i, 22].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 22].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                        }
                                    }
                                }
                                worksheet.Cells[1, 26].Value = "Errors Found. The skus that are highlighted in red have an issue with either their brand or model. This could be a spelling mistake or the brand and/or model are not in the database.";
                                //MessageBox.ShowMessage("Errors Found. The skus that are highlighted in red have an issue with either their brand or model. This could be a spelling mistake or the brand and/or model are not in the database.", this);
                                string fileName = fupItemSheet.FileName + "_ErrorsFound";
                                //Sets the attributes and writes file
                                Response.Clear();
                                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.BinaryWrite(xlPackage.GetAsByteArray());
                                Response.End();
                            }
                            else
                            {
                                MessageBox.ShowMessage("Importing Complete", this);
                            }

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
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnImportCustomers_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnImportCustomers_Click";
            try
            {
                //Verifies file has been selected
                if (fupCustomers.HasFile)
                {
                    //Calls method to import the requested file
                    R.importCustomers(fupCustomers);
                }
                //Show that it is done
                MessageBox.ShowMessage("Importing Complete", this);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Exporting
        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportAll_Click";
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllItems - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + "TotalInventory.xlsx");
                //With the craeted file do all intenal code
                R.itemExports("all", newFile, filename);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnExportClubs_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportClubs_Click";
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllClubs - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);
                //With the craeted file do all intenal code
                R.itemExports("clubs", newFile, filename);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = CU.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                ER.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportClothing_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportClothing_Click";
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllClothing - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);
                //With the created file do all intenal code
                R.itemExports("clothing", newFile, filename);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = CU.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                ER.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportAccessories_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportAccessories_Click";
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllAccessories - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);
                //With the craeted file do all intenal code
                R.itemExports("accessories", newFile, filename);

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = CU.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                ER.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportInvoices_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportInvoices_Click";
            try
            {
                //Sets up database connection
                string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connectionString);
                //Selects everything form the invoice table
                DataTable dtim = new DataTable();
                using (var cmd = new SqlCommand("getInvoiceAll", sqlCon)) //Calling the SP   
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtim);
                }
                DataColumnCollection dcimHeaders = dtim.Columns;
                //Selects everything form the invoice item table
                DataTable dtii = new DataTable();
                using (var cmd = new SqlCommand("getInvoiceItemAll", sqlCon)) //Calling the SP   
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtii);
                }
                DataColumnCollection dciiHeaders = dtii.Columns;
                //Selects everything form the invoice mop table
                DataTable dtimo = new DataTable();
                using (var cmd = new SqlCommand("getInvoiceMOPAll", sqlCon)) //Calling the SP   
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dtimo);
                }
                DataColumnCollection dcimoHeaders = dtimo.Columns;
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                FileInfo newFile = new FileInfo(pathDownload + "InvoiceReport.xlsx");
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet invoiceMain = xlPackage.Workbook.Worksheets.Add("Invoice Main");
                    ExcelWorksheet invoiceItems = xlPackage.Workbook.Worksheets.Add("Invoice Items");
                    ExcelWorksheet invoiceMOPS = xlPackage.Workbook.Worksheets.Add("Invoice MOPS");
                    // write to sheet                  

                    //Export main invoice
                    for (int i = 1; i <= dtim.Rows.Count; i++)
                    {
                        for (int j = 1; j < dtim.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                invoiceMain.Cells[i, j].Value = dcimHeaders[j - 1].ToString();
                            }
                            else
                            {
                                invoiceMain.Cells[i, j].Value = dtim.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    //Export item invoice
                    for (int i = 1; i <= dtii.Rows.Count; i++)
                    {
                        for (int j = 1; j < dtii.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                invoiceItems.Cells[i, j].Value = dciiHeaders[j - 1].ToString();
                            }
                            else
                            {
                                invoiceItems.Cells[i, j].Value = dtii.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    //Export mop invoice
                    for (int i = 1; i <= dtimo.Rows.Count; i++)
                    {
                        for (int j = 1; j < dtimo.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                invoiceMOPS.Cells[i, j].Value = dcimoHeaders[j - 1].ToString();
                            }
                            else
                            {
                                invoiceMOPS.Cells[i, j].Value = dtimo.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=InvoiceReport.xlsx");
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
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = "ddlProvince_SelectedIndexChanged";
            try
            {
                ddlTax.DataSource = TM.ReturnTaxListBasedOnDateAndProvinceForUpdate(Convert.ToInt32(ddlProvince.SelectedValue), Convert.ToDateTime(lblCurrentDate.Text));
                ddlTax.DataBind();
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void ddlTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = "ddlTax_SelectedIndexChanged";
            try
            {
                List<Tax> t = TM.ReturnTaxListBasedOnDate(Convert.ToDateTime(lblCurrentDate.Text), Convert.ToInt32(ddlProvince.SelectedValue));
                foreach (var tax in t)
                {
                    if (tax.taxID == Convert.ToInt32(ddlTax.SelectedValue))
                    {
                        lblCurrentDisplay.Text = tax.taxRate.ToString("#0.00");
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
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnSaveTheTax_Click(object sender, EventArgs e)
        {
            string method = "btnSaveTheTax_Click";
            try
            {
                TM.InsertNewTaxRate(Convert.ToInt32(ddlProvince.SelectedValue), Convert.ToInt32(ddlTax.SelectedValue), Convert.ToDateTime(txtDate.Text), Convert.ToDouble(txtNewRate.Text));
                txtDate.Text = "";
                txtNewRate.Text = "";
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }

        public void callJS()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "upl", "UpdateProgressLabel();", true);
        }
        protected void btnExportEmails_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportEmails_Click";
            try
            {
                //Sets up database connection
                string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
                SqlConnection sqlCon = new SqlConnection(connectionString);
                //Selects everything form the invoice table
                DataTable emailTable = new DataTable();
                using (var cmd = new SqlCommand("getCustomerEmailAll", sqlCon)) //Calling the SP   
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(emailTable);
                }
                DataColumnCollection headers = emailTable.Columns;
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                FileInfo newFile = new FileInfo(pathDownload + "Email List.xlsx");
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Creates a seperate sheet for each data table
                    ExcelWorksheet emailExport = xlPackage.Workbook.Worksheets.Add("Email List");
                    // write to sheet                  

                    //Export main invoice
                    for (int i = 1; i < emailTable.Rows.Count; i++)
                    {
                        for (int j = 1; j < emailTable.Columns.Count + 1; j++)
                        {
                            if (i == 1)
                            {
                                emailExport.Cells[i, j].Value = headers[j - 1].ToString();
                            }
                            else
                            {
                                emailExport.Cells[i, j].Value = emailTable.Rows[i - 1][j - 1];
                            }
                        }
                    }
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=Email List.xlsx");
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
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }

        protected void btnAddModel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddModel_Click";
            try
            {
                if (txtModelOne.Text != "" && txtModelTwo.Text != "")
                {
                    if (txtModelOne.Text.Equals(txtModelTwo.Text))
                    {
                        string sqlCmd = "if exists((select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName)) " +
                                            "begin " +                                                
                                                "print '1'; " +
                                            "end " +
                                        "else " +
                                            "begin " +
                                                "Insert into tbl_model values(@modelName) " +
                                                "print '0'; " +
                                             "end";                      
                        object[][] parms =
                        {
                            new object[] {"@modelName", txtModelOne.Text}
                        };
                        dbc.executeInsertQuery(sqlCmd, parms);                        
                    }
                    else
                    {
                        MessageBox.ShowMessage("The models do not match. "
                                + "Please retype the model name again.", this);
                    }
                }
                else
                {
                    MessageBox.ShowMessage("Both fields require user input", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }

        protected void btnAddBrand_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddBrand_Click";
            try
            {
                if (txtBrandOne.Text != "" && txtBrandTwo.Text != "")
                {
                    if (txtBrandOne.Text.Equals(txtBrandTwo.Text))
                    {
                        string sqlCmd = "if exists((select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName)) " +
                                            "begin " +
                                                "print '1'; " +
                                            "end " +
                                        "else " +
                                            "begin " +
                                                "Insert into tbl_brand values(@brandName) " +
                                                "print '0'; " +
                                             "end";
                        object[][] parms =
                        {
                            new object[] {"@brandName", txtBrandOne.Text}
                        };
                        dbc.executeInsertQuery(sqlCmd, parms);
                    }
                    else
                    {
                        MessageBox.ShowMessage("The brands do not match. "
                                + "Please retype the brand name again.", this);
                    }
                }
                else
                {
                    MessageBox.ShowMessage("Both fields require user input", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}