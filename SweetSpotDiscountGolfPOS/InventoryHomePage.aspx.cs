using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryHomePage : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        CurrentUser cu = new CurrentUser();
        List<Items> searched = new List<Items>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InventoryHomePage";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }

                if (cu.jobID != 0)
                {
                    //If user is not an admin then disable the add new item button
                    btnAddNewInventory.Enabled = false;
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
        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnInventorySearch_Click";
            try
            {
                ItemDataUtilities idu = new ItemDataUtilities();
                string skuString;
                int skuInt;
                if (txtSearch.Text == "")
                {

                }
                else
                {
                    //If text has been entered to search use it to dislpay relevent items
                    //string loc = Convert.ToString(Session["Loc"]);
                    SweetShopManager ssm = new SweetShopManager();
                    string itemType = ddlInventoryType.SelectedItem.ToString();
                    //determines if the searched text is a sku number
                    if (!int.TryParse(txtSearch.Text, out skuInt))
                    {
                        //If number search through skus for any that match
                        skuString = txtSearch.Text;
                        searched = ssm.GetItemfromSearch(txtSearch.Text, itemType);
                    }
                    else
                    {
                        //If search is text 
                        skuString = txtSearch.Text;
                        // this looks for the item in the database
                        List<Items> i = idu.getItemByID(Convert.ToInt32(skuInt));
                        itemType = idu.typeName(i.ElementAt(0).typeID);
                        //if adding new item
                        if (i != null && i.Count >= 1)
                        {
                            searched.Add(i.ElementAt(0));
                        }
                    }
                    //Sets item type
                    Session["itemType"] = itemType;
                    populateGridview(searched);
                    Session["listItems"] = searched;
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
        protected void btnAddNewInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddNewInventory_Click";
            try
            {
                Session["listItems"] = null;
                //Changes page to the inventory add new page
                Server.Transfer("InventoryAddNew.aspx", false);
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
        protected void btnMakePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnMakePurchase_Click";
            try
            {
                //Sets transaction type to sale
                Session["TranType"] = 5;
                //Sets customer id to guest cust
                Session["key"] = 1;
                Session["listItems"] = null;
                //Changes page to Sales Cart
                Server.Transfer("PurchasesCart.aspx", false);
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
        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string method = "err_grdInventorySearched_RowCommand";
            try
            {
                //Stores sku number of selected item
                string itemKey = e.CommandArgument.ToString();
                if (e.CommandName == "viewItem")
                {
                    //If the command selected is viewItem, store item type 
                    Session["key"] = itemKey;
                    Session["listItems"] = null;
                    //Change to Inventory Add new page to display selected item
                    Server.Transfer("InventoryAddNew.aspx");
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
        protected void grdInventorySearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdInventorySearched.PageIndex = e.NewPageIndex;
            searched = (List<Items>)Session["listItems"];
            populateGridview(searched);
        }
        protected void populateGridview(List<Items> list)
        {
            grdInventorySearched.Visible = true;
            //Binds returned items to gridview for display
            grdInventorySearched.DataSource = list;
            grdInventorySearched.DataBind();
            
        }
    }
}