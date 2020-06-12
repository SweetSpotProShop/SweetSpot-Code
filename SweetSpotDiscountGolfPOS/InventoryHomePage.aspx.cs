using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryHomePage : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly ItemsManager IM = new ItemsManager();
        //InvoiceManager InM = new InvoiceManager();
        List<InvoiceItems> searched = new List<InvoiceItems>();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InventoryHomePage";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                    if (CU.employee.intJobID != 0)
                    {
                        //If user is not an admin then disable the add new item button
                        BtnAddNewInventory.Enabled = false;
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
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnInventorySearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                string[] headers = { "SKU", "Description ▼", "Store ▼", "Quantity ▼", "Price ▼", "Cost ▼", "Comments ▼" };
                ViewState["headers"] = headers;

                searched = IM.CallReturnInventoryFromSearchStringAndQuantity(txtSearch.Text, chkIncludeZero.Checked, objPageDetails);
                ViewState["listItems"] = searched;
                PopulateGridview(searched);
                GrdInventorySearched.PageIndex = 0;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233086)
                {
                    MessageBox.ShowMessage("You have searched for an invalid SKU number. "
                       + "Please verify the SKU number you are looking for and try again. ", this);
                }
                else
                {
                    //Log all info into error table
                    ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                    //Display message box
                    MessageBox.ShowMessage("An Error has occurred and been logged. "
                        + "If you continue to receive this message please contact "
                        + "your system administrator.", this);
                }
            }
        }
        protected void BtnAddNewInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnAddNewInventory_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Changes page to the inventory add new page
                Response.Redirect("InventoryAddNew.aspx?inventory=-10", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnMakePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnMakePurchase_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", "1");
                //string receipt = CU.location.varLocationName + "-" + InM.ReturnNextReceiptNumber(objPageDetails) + "-1";
                nameValues.Set("receipt", "-10");
                Response.Redirect("PurchasesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "GrdInventorySearched_RowCommand";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (e.CommandName == "viewItem")
                {
                    //Change to Inventory Add new page to display selected item
                    Response.Redirect("InventoryAddNew.aspx?inventory=" + e.CommandArgument.ToString());
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void GrdInventorySearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string method = "GrdInventorySearched_PageIndexChanging";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdInventorySearched.PageIndex = e.NewPageIndex;
                searched = (List<InvoiceItems>)ViewState["listItems"];
                PopulateGridview(searched);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void PopulateGridview(List<InvoiceItems> list)
        {
            string method = "populateGridview";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                GrdInventorySearched.Visible = true;
                //Binds returned items to gridview for display
                GrdInventorySearched.DataSource = list;
                GrdInventorySearched.DataBind();
                //grdInventorySearched.PageIndex = 0;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Sorting Skus
        protected void BtnSKU_Click(object sender, EventArgs e)
        {
            string method = "BtnSKU_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button sku = GrdInventorySearched.HeaderRow.FindControl("btnSKU") as Button;
                string sort = sku.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "SKU":
                        headers[0] = "SKU ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varSku.CompareTo(y.varSku);
                        });
                        break;
                    case "SKU ▼":
                        headers[0] = "SKU ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varSku.CompareTo(y.varSku);
                        });
                        break;
                    case "SKU ▲":
                        headers[0] = "SKU ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.varSku.CompareTo(x.varSku);
                        });
                        break;
                }
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                headers[6] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                                + "If you continue to receive this message please contact "
                                + "your system administrator.", this);
            }
        }
        protected void BtnDescription_Click(object sender, EventArgs e)
        {
            string method = "BtnDescription_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button desc = GrdInventorySearched.HeaderRow.FindControl("btnDescription") as Button;
                string sort = desc.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Description":
                        headers[1] = "Description ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varItemDescription.CompareTo(y.varItemDescription);
                        });
                        break;
                    case "Description ▼":
                        headers[1] = "Description ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varItemDescription.CompareTo(y.varItemDescription);
                        });
                        break;
                    case "Description ▲":
                        headers[1] = "Description ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.varItemDescription.CompareTo(x.varItemDescription);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                headers[6] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnStore_Click(object sender, EventArgs e)
        {
            string method = "BtnStore_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button store = GrdInventorySearched.HeaderRow.FindControl("btnStore") as Button;
                string sort = store.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Store":
                        headers[2] = "Store ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varLocationName.CompareTo(y.varLocationName);
                        });
                        break;
                    case "Store ▼":
                        headers[2] = "Store ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varLocationName.CompareTo(y.varLocationName);
                        });
                        break;
                    case "Store ▲":
                        headers[2] = "Store ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.varLocationName.CompareTo(x.varLocationName);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                headers[6] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnQuantity_Click(object sender, EventArgs e)
        {
            string method = "BtnQuantity_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button quantity = GrdInventorySearched.HeaderRow.FindControl("btnQuantity") as Button;
                string sort = quantity.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Quantity":
                        headers[3] = "Quantity ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.intItemQuantity.CompareTo(y.intItemQuantity);
                        });
                        break;
                    case "Quantity ▼":
                        headers[3] = "Quantity ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.intItemQuantity.CompareTo(y.intItemQuantity);
                        });
                        break;
                    case "Quantity ▲":
                        headers[3] = "Quantity ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.intItemQuantity.CompareTo(x.intItemQuantity);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[4] = "Price";
                headers[5] = "Cost";
                headers[6] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnPrice_Click(object sender, EventArgs e)
        {
            string method = "BtnPrice_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button price = GrdInventorySearched.HeaderRow.FindControl("btnPrice") as Button;
                string sort = price.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Price":
                        headers[4] = "Price ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.fltItemPrice.CompareTo(y.fltItemPrice);
                        });
                        break;
                    case "Price ▼":
                        headers[4] = "Price ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.fltItemPrice.CompareTo(y.fltItemPrice);
                        });
                        break;
                    case "Price ▲":
                        headers[4] = "Price ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.fltItemPrice.CompareTo(x.fltItemPrice);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[5] = "Cost";
                headers[6] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnCost_Click(object sender, EventArgs e)
        {
            string method = "BtnCost_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button cost = GrdInventorySearched.HeaderRow.FindControl("btnCost") as Button;
                string sort = cost.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Cost":
                        headers[5] = "Cost ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.fltItemCost.CompareTo(y.fltItemCost);
                        });
                        break;
                    case "Cost ▼":
                        headers[5] = "Cost ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.fltItemCost.CompareTo(y.fltItemCost);
                        });
                        break;
                    case "Cost ▲":
                        headers[5] = "Cost ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.fltItemCost.CompareTo(x.fltItemCost);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[6] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnComments_Click(object sender, EventArgs e)
        {
            string method = "BtnComments_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Grabbing the list
                searched = (List<InvoiceItems>)ViewState["listItems"];
                Button comment = GrdInventorySearched.HeaderRow.FindControl("btnComments") as Button;
                string sort = comment.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Comments":
                        headers[6] = "Comments ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varAdditionalInformation.CompareTo(y.varAdditionalInformation);
                        });
                        break;
                    case "Comments ▼":
                        headers[6] = "Comments ▲";
                        //Ascending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return x.varAdditionalInformation.CompareTo(y.varAdditionalInformation);
                        });
                        break;
                    case "Comments ▲":
                        headers[6] = "Comments ▼";
                        //Descending Order
                        searched.Sort(delegate (InvoiceItems x, InvoiceItems y)
                        {
                            return y.varAdditionalInformation.CompareTo(x.varAdditionalInformation);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                PopulateGridview(searched);
                UpdateButtonText(headers);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void UpdateButtonText(string[] headers)
        {
            string method = "UpdateButtonText";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                (GrdInventorySearched.HeaderRow.FindControl("btnSKU") as Button).Text = headers[0];
                (GrdInventorySearched.HeaderRow.FindControl("btnDescription") as Button).Text = headers[1];
                (GrdInventorySearched.HeaderRow.FindControl("btnStore") as Button).Text = headers[2];
                (GrdInventorySearched.HeaderRow.FindControl("btnQuantity") as Button).Text = headers[3];
                (GrdInventorySearched.HeaderRow.FindControl("btnPrice") as Button).Text = headers[4];
                (GrdInventorySearched.HeaderRow.FindControl("btnCost") as Button).Text = headers[5];
                (GrdInventorySearched.HeaderRow.FindControl("btnComments") as Button).Text = headers[6];
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Downloading the search
        protected void BtnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnDownload_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                if (ViewState["listItems"] != null)
                {
                    searched = ViewState["listItems"] as List<InvoiceItems>;
                    //Sets path and file name to download report to
                    string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string pathDownload = (pathUser + "\\Downloads\\");
                    string loc = CU.location.varLocationName;
                    string fileName = "Item Search - " + txtSearch.Text + ".xlsx";
                    FileInfo newFile = new FileInfo(pathDownload + fileName);
                    using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                    {
                        //Creates a seperate sheet for each data table
                        ExcelWorksheet searchExport = xlPackage.Workbook.Worksheets.Add("Items");
                        // write to sheet     
                        searchExport.Cells[1, 1].Value = "SKU";
                        searchExport.Cells[1, 2].Value = "Description";
                        searchExport.Cells[1, 3].Value = "Store";
                        searchExport.Cells[1, 4].Value = "Quantity";
                        searchExport.Cells[1, 5].Value = "Price";
                        searchExport.Cells[1, 6].Value = "Cost";
                        searchExport.Cells[1, 7].Value = "Comments";
                        int recordIndex = 2;
                        foreach (InvoiceItems item in searched)
                        {
                            searchExport.Cells[recordIndex, 1].Value = item.varSku;
                            searchExport.Cells[recordIndex, 2].Value = item.varItemDescription;
                            searchExport.Cells[recordIndex, 3].Value = item.varLocationName;
                            searchExport.Cells[recordIndex, 4].Value = item.intItemQuantity;
                            searchExport.Cells[recordIndex, 5].Value = item.fltItemPrice;
                            searchExport.Cells[recordIndex, 6].Value = item.fltItemCost;
                            searchExport.Cells[recordIndex, 7].Value = item.varAdditionalInformation;
                            recordIndex++;
                        }
                        searchExport.Cells[searchExport.Dimension.Address].AutoFitColumns();
                        Response.Clear();
                        Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.BinaryWrite(xlPackage.GetAsByteArray());
                        Response.End();
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
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}