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

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();
        ItemsManager IM = new ItemsManager();
        List<Items> searched = new List<Items>();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InventoryHomePage";
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
                    if (CU.jobID != 0)
                    {
                        //If user is not an admin then disable the add new item button
                        btnAddNewInventory.Enabled = false;
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
        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInventorySearch_Click";
            try
            {
                string[] headers = { "SKU", "Description ▼", "Store ▼", "Quantity ▼", "Price ▼", "Cost ▼" };
                ViewState["headers"] = headers;

                searched = IM.ReturnInvoiceItemsFromSearchStringAndQuantity(txtSearch.Text, chkIncludeZero.Checked);
                ViewState["listItems"] = searched;
                populateGridview(searched);
                grdInventorySearched.PageIndex = 0;
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
                    ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                    //Display message box
                    MessageBox.ShowMessage("An Error has occurred and been logged. "
                        + "If you continue to receive this message please contact "
                        + "your system administrator.", this);
                }
            }
        }
        protected void btnAddNewInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddNewInventory_Click";
            try
            {
                //Changes page to the inventory add new page
                Response.Redirect("InventoryAddNew.aspx?sku=-10", false);
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

        //Still Needs Work
        protected void btnMakePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnMakePurchase_Click";
            try
            {
                //Sets customer id to guest cust
                Session["key"] = 1;
                //Changes page to Sales Cart
                Response.Redirect("PurchasesCart.aspx", false);
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

        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "err_grdInventorySearched_RowCommand";
            try
            {
                if (e.CommandName == "viewItem")
                {
                    //Change to Inventory Add new page to display selected item
                    Response.Redirect("InventoryAddNew.aspx?sku=" + e.CommandArgument.ToString());
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
        protected void grdInventorySearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string method = "grdInventorySearched_PageIndexChanging";
            try
            {
                grdInventorySearched.PageIndex = e.NewPageIndex;
                searched = (List<Items>)ViewState["listItems"];
                populateGridview(searched);
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
        protected void populateGridview(List<Items> list)
        {
            string method = "populateGridview";
            try
            {
                grdInventorySearched.Visible = true;
                //Binds returned items to gridview for display
                grdInventorySearched.DataSource = list;
                grdInventorySearched.DataBind();
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
        //Sorting Skus
        protected void lbtnSKU_Click(object sender, EventArgs e)
        {
            string method = "lbtnSKU_Click";
            try
            {
                //Grabbing the list
                searched = (List<Items>)ViewState["listItems"];
                Button sku = grdInventorySearched.HeaderRow.FindControl("btnSKU") as Button;
                string sort = sku.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "SKU":
                        headers[0] = "SKU ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.sku.CompareTo(y.sku);
                        });
                        break;
                    case "SKU ▼":
                        headers[0] = "SKU ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.sku.CompareTo(y.sku);
                        });
                        break;
                    case "SKU ▲":
                        headers[0] = "SKU ▼";
                        //Descending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return y.sku.CompareTo(x.sku);
                        });
                        break;
                }
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnDescription_Click(object sender, EventArgs e)
        {
            string method = "btnDescription_Click";
            try
            {
                //Grabbing the list
                searched = (List<Items>)ViewState["listItems"];
                Button desc = grdInventorySearched.HeaderRow.FindControl("btnDescription") as Button;
                string sort = desc.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Description":
                        headers[1] = "Description ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.description.CompareTo(y.description);
                        });
                        break;
                    case "Description ▼":
                        headers[1] = "Description ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.description.CompareTo(y.description);
                        });
                        break;
                    case "Description ▲":
                        headers[1] = "Description ▼";
                        //Descending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return y.description.CompareTo(x.description);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnStore_Click(object sender, EventArgs e)
        {
            string method = "btnStore_Click";
            try
            {
                //Grabbing the list
                searched = (List<Items>)ViewState["listItems"];
                Button store = grdInventorySearched.HeaderRow.FindControl("btnStore") as Button;
                string sort = store.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Store":
                        headers[2] = "Store ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.location.CompareTo(y.location);
                        });
                        break;
                    case "Store ▼":
                        headers[2] = "Store ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.location.CompareTo(y.location);
                        });
                        break;
                    case "Store ▲":
                        headers[2] = "Store ▼";
                        //Descending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return y.location.CompareTo(x.location);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[3] = "Quantity";
                headers[4] = "Price";
                headers[5] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnQuantity_Click(object sender, EventArgs e)
        {
            string method = "btnQuantity_Click";
            try
            {
                //Grabbing the list
                searched = (List<Items>)ViewState["listItems"];
                Button quantity = grdInventorySearched.HeaderRow.FindControl("btnQuantity") as Button;
                string sort = quantity.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Quantity":
                        headers[3] = "Quantity ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.quantity.CompareTo(y.quantity);
                        });
                        break;
                    case "Quantity ▼":
                        headers[3] = "Quantity ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.quantity.CompareTo(y.quantity);
                        });
                        break;
                    case "Quantity ▲":
                        headers[3] = "Quantity ▼";
                        //Descending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return y.quantity.CompareTo(x.quantity);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[4] = "Price";
                headers[5] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnPrice_Click(object sender, EventArgs e)
        {
            string method = "btnPrice_Click";
            try
            {
                //Grabbing the list
                searched = (List<Items>)ViewState["listItems"];
                Button price = grdInventorySearched.HeaderRow.FindControl("btnPrice") as Button;
                string sort = price.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Price":
                        headers[4] = "Price ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.price.CompareTo(y.price);
                        });
                        break;
                    case "Price ▼":
                        headers[4] = "Price ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.price.CompareTo(y.price);
                        });
                        break;
                    case "Price ▲":
                        headers[4] = "Price ▼";
                        //Descending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return y.price.CompareTo(x.price);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[5] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnCost_Click(object sender, EventArgs e)
        {
            string method = "btnCost_Click";
            try
            {
                //Grabbing the list
                searched = (List<Items>)ViewState["listItems"];
                Button cost = grdInventorySearched.HeaderRow.FindControl("btnCost") as Button;
                string sort = cost.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Cost":
                        headers[5] = "Cost ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.cost.CompareTo(y.cost);
                        });
                        break;
                    case "Cost ▼":
                        headers[5] = "Cost ▲";
                        //Ascending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return x.cost.CompareTo(y.cost);
                        });
                        break;
                    case "Cost ▲":
                        headers[5] = "Cost ▼";
                        //Descending Order
                        searched.Sort(delegate (Items x, Items y)
                        {
                            return y.cost.CompareTo(x.cost);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Price";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void updateButtonText(string[] headers)
        {
            string method = "updateButtonText";
            try
            {
                (grdInventorySearched.HeaderRow.FindControl("btnSKU") as Button).Text = headers[0];
                (grdInventorySearched.HeaderRow.FindControl("btnDescription") as Button).Text = headers[1];
                (grdInventorySearched.HeaderRow.FindControl("btnStore") as Button).Text = headers[2];
                (grdInventorySearched.HeaderRow.FindControl("btnQuantity") as Button).Text = headers[3];
                (grdInventorySearched.HeaderRow.FindControl("btnPrice") as Button).Text = headers[4];
                (grdInventorySearched.HeaderRow.FindControl("btnCost") as Button).Text = headers[5];
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
        //Downloading the search
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnDownload_Click";
            try
            {
                searched = ViewState["listItems"] as List<Items>;
                //Sets path and file name to download report to
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = (pathUser + "\\Downloads\\");
                string loc = CU.locationName;
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
                    int recordIndex = 2;
                    foreach (Items item in searched)
                    {
                        searchExport.Cells[recordIndex, 1].Value = item.sku;
                        searchExport.Cells[recordIndex, 2].Value = item.description;
                        searchExport.Cells[recordIndex, 3].Value = item.location;
                        searchExport.Cells[recordIndex, 4].Value = item.quantity;
                        searchExport.Cells[recordIndex, 5].Value = item.price;
                        searchExport.Cells[recordIndex, 6].Value = item.cost;
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