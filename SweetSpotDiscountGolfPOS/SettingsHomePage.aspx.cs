using OfficeOpenXml;
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
        ErrorReporting er = new ErrorReporting();
        SweetShopManager ssm = new SweetShopManager();
        EmployeeManager em = new EmployeeManager();
        Reports r = new Reports();
        internal static readonly Page aspx;
        CurrentUser cu = new CurrentUser();
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
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                lblCurrentDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                if (txtDate.Text == "")
                {
                    txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                }
                //Checks if the user is an Admin
                //if (cu.jobID != 0)
                //{
                //    btnAddNewEmployee.Enabled = false;
                //    btnLoadCustomers.Enabled = false;
                //    btnLoadEmployees.Enabled = false;
                //    btnLoadItems.Enabled = false;
                //}
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddNewEmployee_Click";
            try
            {
                //Change to Employee add new page
                Server.Transfer("EmployeeAddNew.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void grdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "grdEmployeesSearched_RowCommand";
            try
            {
                //Gets string from the command argument
                string key = e.CommandArgument.ToString();
                //Checks if the string is view profile
                if (e.CommandName == "ViewProfile")
                {
                    //Sets the employee id into a session
                    Session["key"] = key;
                    //Changes page to Employee Add New
                    Server.Transfer("EmployeeAddNew.aspx", false);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnEmployeeSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEmployeeSearch_Click";
            try
            {
                //Queries database and returns a list of emplyees that match the search criteria
                List<Employee> emp = em.GetEmployeefromSearch(txtSearch.Text);
                grdEmployeesSearched.Visible = true;
                //Binds the employee list to grid view
                grdEmployeesSearched.DataSource = emp;
                grdEmployeesSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                            int[] cells = { 3, 10, 11, 12, 13, 14, 15 };
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
                            r.importItems(fupItemSheet);
                        }
                    }
                    //Show that it is done
                    MessageBox.ShowMessage("Importing Complete", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                    r.importCustomers(fupCustomers);
                }
                //Show that it is done
                MessageBox.ShowMessage("Importing Complete", this);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                FileInfo newFile = new FileInfo(pathDownload + "TotalInventory.xlsx");
                //With the craeted file do all intenal code
                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    //Add page to the work book called inventory
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Inventory");
                    // write to sheet
                    DataTable exportTable = r.exportAllItems();
                    //Setting data collection as datatable
                    DataColumnCollection dcCollection = exportTable.Columns;
                    //Loops through each row in the datatable
                    for (int i = 1; i < exportTable.Rows.Count + 2; i++)
                    {
                        //Loops through each column in the data table
                        for (int j = 1; j < exportTable.Columns.Count + 1; j++)
                        {
                            //When the row equals 1 set the headers
                            if (i == 1)
                            {
                                worksheet.Cells[i, j].Value = dcCollection[j - 1].ToString();
                            }
                            else
                                //Set the values in the data table
                                worksheet.Cells[i, j].Value = exportTable.Rows[i - 2][j - 1].ToString();
                        }
                    }
                    //Sets the attributes and writes file
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=TotalInventory.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(xlPackage.GetAsByteArray());
                    Response.End();
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnExportClubs_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnExportClubs_Click";
            try
            {
                //Calls method to export clubs
                r.exportClubs();
                //Displays message
                MessageBox.ShowMessage("Export Complete", this);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
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
                //Calls method to export all clothing
                r.exportClothing();
                //Displays message
                MessageBox.ShowMessage("Export Complete", this);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
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
                //Calls method to export all accessories
                r.exportAccessories();
                //Displays message
                MessageBox.ShowMessage("Export Complete", this);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
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
                    for (int i = 1; i < dtim.Rows.Count; i++)
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
                    for (int i = 1; i < dtii.Rows.Count; i++)
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
                    for (int i = 1; i < dtimo.Rows.Count; i++)
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }

        protected void ddlTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            string method = "ddlTax_SelectedIndexChanged";
            try
            {
                string sel = ddlTax.SelectedItem.ToString();
                List<Tax> t = new List<Tax>();
                t = ssm.getTaxes(Convert.ToInt32(ddlProvince.SelectedValue), Convert.ToDateTime(lblCurrentDate.Text));
                foreach (var tax in t)
                {
                    if (tax.taxName == sel)
                    {
                        lblCurrentDisplay.Text = tax.taxRate.ToString("#0.00");
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnSaveTheTax_Click(object sender, EventArgs e)
        {
            string method = "btnSaveTheTax_Click";
            try
            {
                int psID = Convert.ToInt32(ddlProvince.SelectedValue);
                int tID = Convert.ToInt32(ddlTax.SelectedValue);
                DateTime tDate = Convert.ToDateTime(txtDate.Text);
                double tRate = Convert.ToDouble(txtNewRate.Text);

                ssm.updateNewTaxRate(psID, tDate, tID, tRate);
                txtDate.Text = "";
                txtNewRate.Text = "";

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
    }
}