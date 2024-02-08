using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;


namespace SweetSpotDiscountGolfPOS
{
    public partial class SettingsHomePage : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly EmployeeManager EM = new EmployeeManager();
        readonly ImportExport IE = new ImportExport();
        readonly TaxManager TM = new TaxManager();
        readonly LocationManager LM = new LocationManager();
        readonly DatabaseCalls DBC = new DatabaseCalls();
        readonly CustomerManager CM = new CustomerManager();
        CurrentUser CU;

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
                    lblCurrentDate.Text = DateTime.Today.ToString("dd/MMM/yy");
                    if (txtDate.Text == "")
                    {
                        txtDate.Text = DateTime.Today.ToString("dd/MMM/yy");
                    }
                    //Checks if the user is an Admin
                    if (CU.employee.intJobID != 0)
                    {
                        BtnAddNewEmployee.Enabled = false;
                        BtnLoadItems.Enabled = false;
                    }
                    if (!IsPostBack)
                    {
                        DdlProvince.DataSource = LM.CallReturnProvinceDropDown(0, objPageDetails);
                        DdlProvince.DataBind();
                        DdlProvince.SelectedValue = CU.location.intProvinceID.ToString();
                        DdlTax.DataSource = TM.GatherTaxListFromDateAndProvince(CU.location.intProvinceID, Convert.ToDateTime(lblCurrentDate.Text), objPageDetails);
                        DdlTax.DataBind();
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
        protected void BtnAddNewEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddNewEmployee_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Change to Employee add new page
                Response.Redirect("EmployeeAddNew.aspx?employee=-10", false);
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
        protected void GrdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdEmployeesSearched_RowCommand";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Checks if the string is view profile
                if (e.CommandName == "ViewProfile")
                {
                    //Changes page to Employee Add New
                    Response.Redirect("EmployeeAddNew.aspx?employee=" + e.CommandArgument.ToString(), false);
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
        protected void BtnEmployeeSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnEmployeeSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdEmployeesSearched.Visible = true;
                //Binds the employee list to grid view
                GrdEmployeesSearched.DataSource = EM.CallReturnEmployeeBasedOnText(txtSearch.Text, objPageDetails);
                GrdEmployeesSearched.DataBind();
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

        //Importing
        protected void BtnLoadItems_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnLoadItems_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Trade-in_Detail_Report_"];
                        //Gets the row count
                        var rowCnt = worksheet.Dimension.End.Row;
                        //Gets the column count
                        var colCnt = worksheet.Dimension.End.Column;
                        //Beginning the loop for data gathering
                        for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                        {
                            //Array of the cells that will need to be checked
                            int[] cells = { 4, 6, 7, 13, 14, 16, 23 };
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
#pragma warning disable IDE0067 // Dispose objects before losing scope
                            DataTable errors = new DataTable();
#pragma warning restore IDE0067 // Dispose objects before losing scope
                            errors = IE.CallUploadItems(fupItemSheet, CU, objPageDetails);
                            if (errors.Rows.Count != 0)
                            {
                                //Loops through the errors datatable pulling the sku's from there and entering them into an array
                                //Then loops through each row on the excel sheet and checks to compare against sku array
                                //If it is not in there, that row is deleted
                                object[,] errorList = new object[errors.Rows.Count, 4];
                                ArrayList errorSkus = new ArrayList();
                                for (int i = 0; i < errors.Rows.Count; i++)
                                {
                                    errorSkus.Add(errors.Rows[i][0]); //SKUs used for row deletion
                                    errorList[i, 0] = errors.Rows[i][0].ToNullSafeString(); //SKU
                                    errorList[i, 1] = Convert.ToInt32(errors.Rows[i][1]); //Brand
                                    errorList[i, 2] = Convert.ToInt32(errors.Rows[i][2]); //Model
                                    errorList[i, 3] = Convert.ToInt32(errors.Rows[i][3]); //Secondary Identifier
                                }
                                //Loop through the Excel sheet
                                for (int i = rowCnt; i >= 2; i--)
                                {
                                    //Loop through the error array
                                    for (int j = 0; j < errorList.Length/4; j++) 
                                    {
                                        //Column 4 = SKU
                                        if ((worksheet.Cells[i, 4].Value).ToString().Equals(errorList[j,0].ToString()))
                                        {
                                            worksheet.Cells[i, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[i, 4].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            //If brand caused an error
                                            if (Convert.ToInt32(errorList[j,1]) == 1)
                                            {
                                                worksheet.Cells[i, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            //If model caused an error
                                            if(Convert.ToInt32(errorList[j, 2]) == 1)
                                            {
                                                worksheet.Cells[i, 7].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 7].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            //If secondary identifier(Destination) caused an error
                                            if (Convert.ToInt32(errorList[j,3]) == 1)
                                            {
                                                worksheet.Cells[i, 23].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 23].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            break;
                                        }                                        
                                    }
                                    //Check to see if the cell's sku is in the array, if it is not, delete the row
                                    bool isInArray = errorSkus.IndexOf((worksheet.Cells[i, 4].Value).ToString()) != -1;
                                    if (!isInArray)
                                    {
                                        worksheet.DeleteRow(i);
                                    }
                                }
                                worksheet.Cells[1, 28].Value = "Errors Found. The skus that are highlighted in red have an issue with either their Brand, Model, or Destination. This could be a spelling mistake or the Brand, Model and/or Destination are not in the database.";
                                //MessageBoxCustom.ShowMessage("Errors Found. The skus that are highlighted in red have an issue with either their brand or model. This could be a spelling mistake or the brand and/or model are not in the database.", this);
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
                                MessageBoxCustom.ShowMessage("Importing Complete", this);
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
            imgLoadingItemImport.Visible = false;
        }
        protected void btnSpecialUpdateTool_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSpecialUpdateTool_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                int error = 0;
                //Verifies file has been selected
                if (fupSpecialUpdate.HasFile)
                {
                    //load the uploaded file into the memorystream
                    using (MemoryStream stream = new MemoryStream(fupSpecialUpdate.FileBytes))
                    //Lets the server know to use the excel package
                    using (ExcelPackage xlPackage = new ExcelPackage(stream))
                    {
                        //Gets the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Special_Update"];
                        //Gets the row count
                        var rowCnt = worksheet.Dimension.End.Row;
                        //Gets the column count
                        var colCnt = worksheet.Dimension.End.Column;
                        //Beginning the loop for data gathering
                        for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                        {
                            //Array of the cells that will need to be checked
                            int[] cells = { 1, 2 };
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
                            Response.AddHeader("content-disposition", "attachment; filename=" + fupSpecialUpdate.FileName + "_ErrorsFound" + ".xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(xlPackage.GetAsByteArray());
                            Response.End();
                        }
                        else
                        {
                            //Calls method to import the requested file
#pragma warning disable IDE0067 // Dispose objects before losing scope
                            DataTable errors = new DataTable();
#pragma warning restore IDE0067 // Dispose objects before losing scope
                            string strReferenceColumn = ddlReferenceColumn.SelectedItem.Text;
                            string strUpdateColumn = ddlSpecialUpdateColumn.SelectedItem.Text;
                            errors = IE.CallSpecialUpdateTool(fupSpecialUpdate, strReferenceColumn, strUpdateColumn, CU, objPageDetails);
                            if (errors.Rows.Count != 0)
                            {
                                //Loops through the errors datatable pulling the sku's from there and entering them into an array
                                //Then loops through each row on the excel sheet and checks to compare against sku array
                                //If it is not in there, that row is deleted
                                object[,] errorList = new object[errors.Rows.Count, 4];
                                ArrayList errorSkus = new ArrayList();
                                for (int i = 0; i < errors.Rows.Count; i++)
                                {
                                    errorSkus.Add(errors.Rows[i][0]); //SKUs used for row deletion
                                    errorList[i, 0] = errors.Rows[i][0].ToNullSafeString(); //SKU
                                    errorList[i, 1] = errors.Rows[i][1].ToNullSafeString(); //SpecialUpdate
                                }
                                //Loop through the Excel sheet
                                for (int i = rowCnt; i >= 2; i--)
                                {
                                    //Loop through the error array
                                    for (int j = 0; j < errorList.Length / 4; j++)
                                    {
                                        //Column 4 = SKU
                                        if ((worksheet.Cells[i, 1].Value).ToString().Equals(errorList[j, 0].ToString()))
                                        {
                                            //If brand caused an error
                                            if (Convert.ToInt32(errorList[j, 1]) == 1)
                                            {
                                                worksheet.Cells[i, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 2].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            break;
                                        }
                                    }
                                    //Check to see if the cell's sku is in the array, if it is not, delete the row
                                    bool isInArray = errorSkus.IndexOf((worksheet.Cells[i, 1].Value).ToString()) != -1;
                                    if (!isInArray)
                                    {
                                        worksheet.DeleteRow(i);
                                    }
                                }
                                worksheet.Cells[1, 3].Value = "Sku is not in database for update";
                                //MessageBoxCustom.ShowMessage("Errors Found. The skus that are highlighted in red have an issue with either their brand or model. This could be a spelling mistake or the brand and/or model are not in the database.", this);
                                string fileName = fupSpecialUpdate.FileName + "_ErrorsFound";
                                //Sets the attributes and writes file
                                Response.Clear();
                                Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.BinaryWrite(xlPackage.GetAsByteArray());
                                Response.End();
                            }
                            else
                            {
                                MessageBoxCustom.ShowMessage("Special Update Complete", this);
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
            imgLoadingItemImport2.Visible = false;
        }

        //Exporting
        protected void BtnExportAll_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExportAll_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllItems - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + "TotalInventory.xlsx");
                //With the craeted file do all intenal code
                IE.CallItemExports("all", newFile, filename, objPageDetails);
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
        protected void BtnExportClubs_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExportClubs_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllClubs - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);
                //With the craeted file do all intenal code
                IE.CallItemExports("clubs", newFile, filename, objPageDetails);
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
        protected void BtnExportClothing_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExportClothing_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllClothing - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);
                //With the created file do all intenal code
                IE.CallItemExports("clothing", newFile, filename, objPageDetails);
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
        protected void BtnExportAccessories_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExportAccessories_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Sets path and file name to save the export to 
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string filename = "AllAccessories - " + DateTime.Now.ToString("dd.MM.yyyy") + ".xlsx";
                FileInfo newFile = new FileInfo(pathDownload + filename);
                //With the craeted file do all intenal code
                IE.CallItemExports("accessories", newFile, filename, objPageDetails);
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
        protected void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = "DdlProvince_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                DdlTax.DataSource = TM.GatherTaxListFromDateAndProvince(Convert.ToInt32(DdlProvince.SelectedValue), Convert.ToDateTime(lblCurrentDate.Text), objPageDetails);
                DdlTax.DataBind();
            }
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
        protected void DdlTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = "DdlTax_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                List<Tax> taxes = TM.ReturnTaxListBasedOnDate(Convert.ToDateTime(lblCurrentDate.Text), Convert.ToInt32(DdlProvince.SelectedValue), objPageDetails);
                foreach (var tax in taxes)
                {
                    if (tax.intTaxID == Convert.ToInt32(DdlTax.SelectedValue))
                    {
                        lblCurrentDisplay.Text = tax.fltTaxRate.ToString("#0.00");
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
        protected void BtnSaveTheTax_Click(object sender, EventArgs e)
        {
            string method = "BtnSaveTheTax_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                TM.CallInsertNewTaxRate(Convert.ToInt32(DdlProvince.SelectedValue), Convert.ToInt32(DdlTax.SelectedValue), Convert.ToDateTime(txtDate.Text), Convert.ToDouble(txtNewRate.Text), objPageDetails);
                txtDate.Text = "";
                txtNewRate.Text = "";
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

        public void CallJS()
        {
            string method = "CallJS";
            try
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "upl", "UpdateProgressLabel();", true);
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
        protected void BtnExportEmails_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnExportEmails_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                DataTable emailTable = CM.CallReturnCustomerEmailAddresses(objPageDetails);
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }

        protected void BtnAddModel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddModel_Click";
            string strQueryName = "btnAddModel_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtModelOne.Text != "" && txtModelTwo.Text != "")
                {
                    if (txtModelOne.Text.Equals(txtModelTwo.Text))
                    {
                        string sqlCmd = "IF EXISTS((SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName)) "
                            + "BEGIN PRINT '1'; END ELSE BEGIN INSERT INTO tbl_model VALUES(@varModelName) PRINT '0'; END";
                        object[][] parms =
                        {
                            new object[] {"@varModelName", txtModelOne.Text}
                        };
                        DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
                        txtModelOne.Text = "";
                        txtModelTwo.Text = "";
                        //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
                    }
                    else
                    {
                        MessageBoxCustom.ShowMessage("The models do not match. "
                                + "Please retype the model name again.", this);
                    }
                }
                else
                {
                    MessageBoxCustom.ShowMessage("Both fields require user input", this);
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
        protected void BtnAddBrand_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddBrand_Click";
            string strQueryName = "btnAddBrand_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (txtBrandOne.Text != "" && txtBrandTwo.Text != "")
                {
                    if (txtBrandOne.Text.Equals(txtBrandTwo.Text))
                    {
                        string sqlCmd = "IF EXISTS((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName)) "
                            + "BEGIN PRINT '1'; END ELSE BEGIN INSERT INTO tbl_brand VALUES(@varBrandName) PRINT '0'; END";
                        object[][] parms =
                        {
                            new object[] {"@varBrandName", txtBrandOne.Text}
                        };
                        DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
                        //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
                        txtBrandOne.Text = "";
                        txtBrandTwo.Text = "";
                    }
                    else
                    {
                        MessageBoxCustom.ShowMessage("The brands do not match. "
                                + "Please retype the brand name again.", this);
                    }
                }
                else
                {
                    MessageBoxCustom.ShowMessage("Both fields require user input", this);
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