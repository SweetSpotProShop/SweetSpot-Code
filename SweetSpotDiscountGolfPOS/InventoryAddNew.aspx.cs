using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetShop;
using SweetSpotProShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using System.Threading;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryAddNew : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        Object o = new Object();
        SweetShopManager ssm = new SweetShopManager();
        Clubs c = new Clubs();
        Accessories a = new Accessories();
        Clothing cl = new Clothing();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();
        CurrentUser cu = new CurrentUser();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InventoryAddNew.aspx";
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
                    //If user is not an admin then disable the edit item button
                    btnEditItem.Enabled = false;
                }
                //Check to see if an item was selected
                if (Session["key"] != null)
                {
                    if (!IsPostBack)
                    {
                        //Using the item number
                        string itemType = Session["itemType"].ToString();
                        int itemSKU = Convert.ToInt32(Session["key"].ToString());
                        lblTypeDisplay.Text = itemType;
                        //Checks the item type
                        if (itemType == "Clubs")
                        {
                            //When a club, create class and populate labels
                            c = ssm.singleItemLookUp(itemSKU);
                            lblSKUDisplay.Text = c.sku.ToString();
                            lblCostDisplay.Text = c.cost.ToString();
                            lblBrandDisplay.Text = idu.brandType(c.brandID);
                            lblPriceDisplay.Text = c.price.ToString();
                            lblQuantityDisplay.Text = c.quantity.ToString();
                            lblLocationDisplay.Text = lm.locationName(c.itemlocation);

                            lblClubTypeDisplay.Text = c.clubType.ToString();
                            lblModelDisplay.Text = idu.modelType(c.modelID);
                            lblShaftDisplay.Text = c.shaft.ToString();
                            lblNumberofClubsDisplay.Text = c.numberOfClubs.ToString();
                            lblClubSpecDisplay.Text = c.clubSpec.ToString();
                            lblShaftSpecDisplay.Text = c.shaftSpec.ToString();
                            lblShaftFlexDisplay.Text = c.shaftFlex.ToString();
                            lblDexterityDisplay.Text = c.dexterity.ToString();
                            chkUsed.Checked = c.used;
                            lblCommentsDisplay.Text = c.comments.ToString();

                        }
                        else if (itemType == "Accessories")
                        {
                            //When accessories, create class and populate labels
                            a = ssm.getAccessory(itemSKU);
                            lblSKUDisplay.Text = a.sku.ToString();
                            lblCostDisplay.Text = a.cost.ToString();
                            lblBrandDisplay.Text = idu.brandType(a.brandID);
                            lblPriceDisplay.Text = a.price.ToString();
                            lblQuantityDisplay.Text = a.quantity.ToString();
                            lblLocationDisplay.Text = lm.locationName(a.locID);
                            lblNumberofClubs.Text = "Accessory Type: ";
                            lblNumberofClubsDisplay.Text = a.accessoryType.ToString();

                            lblClubType.Text = "Size: ";
                            lblClubTypeDisplay.Text = a.size.ToString();
                            lblModel.Visible = true;
                            lblModelDisplay.Text = idu.modelType(a.modelID);
                            lblShaft.Text = "Colour: ";
                            lblShaftDisplay.Text = a.colour.ToString();

                            lblCommentsDisplay.Text = a.comments.ToString();
                            
                            lblClubSpec.Visible = false;
                            lblClubSpecDisplay.Visible = false;
                            lblShaftSpec.Visible = false;
                            lblShaftSpecDisplay.Visible = false;
                            lblShaftFlex.Visible = false;
                            lblShaftFlexDisplay.Visible = false;
                            lblDexterity.Visible = false;
                            lblDexterityDisplay.Visible = false;
                            lblComments.Visible = true;
                            lblCommentsDisplay.Visible = true;
                            chkUsed.Visible = false;

                        }
                        else if (itemType == "Clothing")
                        {
                            //When clothing, create class and populate labels
                            cl = ssm.getClothing(itemSKU);
                            lblSKUDisplay.Text = cl.sku.ToString();
                            lblCostDisplay.Text = cl.cost.ToString();
                            lblBrandDisplay.Text = idu.brandType(cl.brandID);
                            lblPriceDisplay.Text = cl.price.ToString();
                            lblQuantityDisplay.Text = cl.quantity.ToString();
                            lblLocationDisplay.Text = lm.locationName(cl.locID);
                            lblCommentsDisplay.Text = cl.comments.ToString();


                            lblClubType.Text = "Size: ";
                            lblClubTypeDisplay.Text = cl.size.ToString();
                            lblModel.Visible = false;
                            lblModelDisplay.Visible = false;
                            lblShaft.Text = "Colour: ";
                            lblShaftDisplay.Text = cl.colour.ToString();
                            lblNumberofClubs.Visible = false;
                            lblNumberofClubsDisplay.Visible = false;
                            lblClubSpec.Text = "Gender: ";
                            lblClubSpecDisplay.Text = cl.gender.ToString();
                            lblShaftFlex.Text = "Style: ";
                            lblShaftFlexDisplay.Text = cl.style.ToString();
                            lblShaftSpec.Visible = false;
                            lblShaftSpecDisplay.Visible = false;
                            lblDexterity.Visible = false;
                            lblDexterityDisplay.Visible = false;
                            lblComments.Visible = true;
                            lblCommentsDisplay.Visible = true;
                            chkUsed.Visible = false;
                        }
                    }
                }
                else
                {
                    //When no item was selected display drop downs and text boxes
                    ddlType.Visible = true;
                    lblTypeDisplay.Visible = false;

                    txtCost.Visible = true;
                    lblCostDisplay.Visible = false;

                    ddlBrand.Visible = true;
                    lblBrandDisplay.Visible = false;

                    txtPrice.Visible = true;
                    lblPriceDisplay.Visible = false;

                    txtQuantity.Visible = true;
                    lblQuantityDisplay.Visible = false;

                    ddlLocation.Visible = true;
                    lblLocationDisplay.Visible = false;

                    lblClubTypeDisplay.Visible = false;
                    lblModelDisplay.Visible = false;
                    lblShaftDisplay.Visible = false;
                    lblNumberofClubsDisplay.Visible = false;
                    lblClubSpecDisplay.Visible = false;
                    lblShaftSpecDisplay.Visible = false;
                    lblShaftFlexDisplay.Visible = false;
                    lblDexterityDisplay.Visible = false;
                    lblCommentsDisplay.Visible = false;

                    //Accessories
                    if (ddlType.SelectedIndex <= 0)
                    {
                        //adjust labels displaying for accessories
                        lblClubType.Text = "Size: ";
                        txtClubType.Visible = true;
                        lblShaft.Text = "Colour: ";
                        txtShaft.Visible = true;
                        txtClubSpec.Visible = false;
                        txtShaftFlex.Visible = false;
                        chkUsed.Visible = false;
                        lblModel.Visible = true;
                        ddlModel.Visible = true;
                        lblNumberofClubs.Visible = true;
                        txtNumberofClubs.Visible = true;
                        lblClubSpec.Visible = false;
                        lblShaftSpec.Visible = false;
                        txtShaftSpec.Visible = false;
                        lblShaftFlex.Visible = false;
                        lblDexterity.Visible = false;
                        txtDexterity.Visible = false;
                        lblComments.Visible = false;
                        txtComments.Visible = false;
                    }
                    //Clubs
                    else if (ddlType.SelectedIndex == 2)
                    {
                        //adjust labels displaying for clubs
                        txtClubType.Visible = true;
                        ddlModel.Visible = true;
                        txtShaft.Visible = true;
                        txtNumberofClubs.Visible = true;
                        txtClubSpec.Visible = true;
                        txtShaftSpec.Visible = true;
                        txtShaftFlex.Visible = true;
                        txtDexterity.Visible = true;
                        txtComments.Visible = true;
                        chkUsed.Enabled = true;
                        lblClubType.Text = "Club Type: ";
                        lblShaft.Text = "Shaft: ";
                        lblClubSpec.Text = "Club Spec: ";
                        lblClubSpec.Visible = true;
                        lblShaftFlex.Text = "Shaft Flex: ";
                        lblShaftFlex.Visible = true;
                        lblModel.Visible = true;
                        lblNumberofClubs.Visible = true;
                        lblShaftSpec.Visible = true;
                        lblDexterity.Visible = true;
                        lblComments.Visible = true;
                        chkUsed.Visible = true;
                    }
                    //Clothing
                    else if (ddlType.SelectedIndex == 1)
                    {
                        //adjust labels displaying for clubs
                        lblClubType.Text = "Size: ";
                        txtClubType.Visible = true;
                        lblShaft.Text = "Colour: ";
                        txtShaft.Visible = true;
                        lblClubSpec.Text = "Gender: ";
                        lblClubSpec.Visible = true;
                        txtClubSpec.Visible = true;
                        lblShaftFlex.Text = "Style: ";
                        lblShaftFlex.Visible = true;
                        txtShaftFlex.Visible = true;
                        chkUsed.Visible = false;
                        ddlModel.Visible = false;
                        txtNumberofClubs.Visible = false;
                        txtShaftSpec.Visible = false;
                        txtDexterity.Visible = false;
                        txtComments.Visible = false;
                        lblShaftSpec.Visible = false;
                        lblModel.Visible = false;
                        lblNumberofClubs.Visible = false;
                        lblDexterity.Visible = false;
                        lblComments.Visible = true;

                    }

                    //hides and displays the proper buttons for access
                    btnSaveItem.Visible = false;
                    btnAddItem.Visible = true;
                    pnlDefaultButton.DefaultButton = "btnAddItem";
                    btnEditItem.Visible = false;
                    btnCancel.Visible = false;
                    btnBackToSearch.Visible = true;
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
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddItem_Click";
            try
            {
                //Retrieves the type of item that is getting added
                int skuNum;
                string type = idu.typeName(Convert.ToInt32(ddlType.SelectedValue));
                Session["itemType"] = type;
                if (ddlType.SelectedIndex == 2)
                {
                    //Transfers all info into Club class
                    c.sku = idu.maxSku(1);
                    c.cost = Convert.ToDouble(txtCost.Text);
                    c.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    c.price = Convert.ToDouble(txtPrice.Text);
                    c.quantity = Convert.ToInt32(txtQuantity.Text);
                    c.itemlocation = Convert.ToInt32(ddlLocation.SelectedValue);
                    c.clubType = txtClubType.Text;
                    c.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                    c.shaft = txtShaft.Text;
                    c.numberOfClubs = txtNumberofClubs.Text;
                    c.clubSpec = txtClubSpec.Text;
                    c.shaftSpec = txtShaftSpec.Text;
                    c.shaftFlex = txtShaftFlex.Text;
                    c.dexterity = txtDexterity.Text;
                    c.used = chkUsed.Checked;
                    c.comments = txtComments.Text;
                    c.typeID = Convert.ToInt32(ddlType.SelectedValue);
                    //stores club as an object
                    o = c as Object;
                }
                else if (ddlType.SelectedIndex == 0)
                {
                    //Transfers all info into Accessory class
                    a.sku = idu.maxSku(2);
                    a.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    a.cost = Convert.ToDouble(txtCost.Text);
                    a.price = Convert.ToDouble(txtPrice.Text);
                    a.quantity = Convert.ToInt32(txtQuantity.Text);
                    a.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    a.typeID = Convert.ToInt32(ddlType.SelectedValue);
                    a.size = txtClubType.Text;
                    a.colour = txtShaft.Text;
                    a.accessoryType = txtNumberofClubs.Text;
                    a.comments = txtComments.Text;
                    //stores accessory as an object
                    o = a as Object;
                }
                else if (ddlType.SelectedIndex == 1)
                {
                    //Transfers all info into Clothing class
                    cl.sku = idu.maxSku(3);
                    cl.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    cl.cost = Convert.ToDouble(txtCost.Text);
                    cl.price = Convert.ToDouble(txtPrice.Text);
                    cl.quantity = Convert.ToInt32(txtQuantity.Text);
                    cl.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    cl.typeID = Convert.ToInt32(ddlType.SelectedValue);
                    cl.size = txtClubType.Text;
                    cl.colour = txtShaft.Text;
                    cl.gender = txtClubSpec.Text;
                    cl.style = txtShaftFlex.Text;
                    cl.comments = txtComments.Text;
                    //stores clothing as an object
                    o = cl as Object;
                }
                //adds item into the inventory
                skuNum = ssm.addItem(o);
                //store sku into Session
                Session["key"] = skuNum;
                //Refreshes current page
                Server.Transfer(Request.RawUrl, false);
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
        protected void btnEditItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditItem_Click";
            try
            {
                //transfers data from labels into textbox for editing
                txtCost.Text = lblCostDisplay.Text;
                txtCost.Visible = true;
                lblCostDisplay.Visible = false;

                ddlBrand.SelectedValue = idu.brandName(lblBrandDisplay.Text).ToString();
                ddlBrand.Visible = true;
                lblBrandDisplay.Visible = false;

                txtPrice.Text = lblPriceDisplay.Text;
                txtPrice.Visible = true;
                lblPriceDisplay.Visible = false;

                txtQuantity.Text = lblQuantityDisplay.Text;
                txtQuantity.Visible = true;
                lblQuantityDisplay.Visible = false;

                ddlLocation.SelectedValue = lm.locationID(lblLocationDisplay.Text).ToString();
                ddlLocation.Visible = true;
                lblLocationDisplay.Visible = false;

                if (lblTypeDisplay.Text == "Clubs")
                {
                    //transfers specific data if club type item
                    txtClubType.Text = lblClubTypeDisplay.Text;
                    txtClubType.Visible = true;
                    lblClubTypeDisplay.Visible = false;

                    ddlModel.SelectedValue = idu.modelName(lblModelDisplay.Text).ToString();
                    ddlModel.Visible = true;
                    lblModelDisplay.Visible = false;

                    txtShaft.Text = lblShaftDisplay.Text;
                    txtShaft.Visible = true;
                    lblShaftDisplay.Visible = false;

                    txtNumberofClubs.Text = lblNumberofClubsDisplay.Text;
                    txtNumberofClubs.Visible = true;
                    lblNumberofClubsDisplay.Visible = false;

                    txtClubSpec.Text = lblClubSpecDisplay.Text;
                    txtClubSpec.Visible = true;
                    lblClubSpecDisplay.Visible = false;

                    txtShaftSpec.Text = lblShaftSpecDisplay.Text;
                    txtShaftSpec.Visible = true;
                    lblShaftSpecDisplay.Visible = false;

                    txtShaftFlex.Text = lblShaftFlexDisplay.Text;
                    txtShaftFlex.Visible = true;
                    lblShaftFlexDisplay.Visible = false;

                    txtDexterity.Text = lblDexterityDisplay.Text;
                    txtDexterity.Visible = true;
                    lblDexterityDisplay.Visible = false;

                    txtComments.Text = lblCommentsDisplay.Text;
                    txtComments.Visible = true;
                    lblCommentsDisplay.Visible = false;

                    chkUsed.Enabled = true;
                }
                else if (lblTypeDisplay.Text == "Accessories")
                {
                    //transfers specific data if accessories type item
                    ddlModel.SelectedValue = idu.modelName(lblModelDisplay.Text).ToString();
                    ddlModel.Visible = true;
                    lblModelDisplay.Visible = false;

                    txtClubType.Text = lblClubTypeDisplay.Text;
                    txtClubType.Visible = true;
                    lblClubTypeDisplay.Visible = false;

                    txtShaft.Text = lblShaftDisplay.Text;
                    txtShaft.Visible = true;
                    lblShaftDisplay.Visible = false;

                    txtNumberofClubs.Text = lblNumberofClubsDisplay.Text;
                    txtNumberofClubs.Visible = true;
                    lblNumberofClubsDisplay.Visible = false;

                    txtComments.Text = lblCommentsDisplay.Text;
                    txtComments.Visible = true;
                    lblCommentsDisplay.Visible = false;
                }
                else if (lblTypeDisplay.Text == "Clothing")
                {
                    //transfers specific data if clothing type item
                    txtClubType.Text = lblClubTypeDisplay.Text;
                    txtClubType.Visible = true;
                    lblClubTypeDisplay.Visible = false;

                    txtShaft.Text = lblShaftDisplay.Text;
                    txtShaft.Visible = true;
                    lblShaftDisplay.Visible = false;

                    txtClubSpec.Text = lblClubSpecDisplay.Text;
                    txtClubSpec.Visible = true;
                    lblClubSpecDisplay.Visible = false;

                    txtShaftFlex.Text = lblShaftFlexDisplay.Text;
                    txtShaftFlex.Visible = true;
                    lblShaftFlexDisplay.Visible = false;

                    txtComments.Text = lblCommentsDisplay.Text;
                    txtComments.Visible = true;
                    lblCommentsDisplay.Visible = false;
                }
                //hides and displays the proper buttons for access
                btnSaveItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveItem";
                btnEditItem.Visible = false;
                btnAddItem.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
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
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveItem_Click";
            try
            {
                if (lblTypeDisplay.Text == "Clubs")
                {
                    //if item type is club then save as club class
                    c.sku = Convert.ToInt32(Session["key"].ToString());
                    c.cost = Convert.ToDouble(txtCost.Text);
                    c.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    c.price = Convert.ToDouble(txtPrice.Text);
                    c.quantity = Convert.ToInt32(txtQuantity.Text);
                    c.itemlocation = Convert.ToInt32(ddlLocation.SelectedValue);
                    c.clubType = txtClubType.Text;
                    c.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                    c.shaft = txtShaft.Text;
                    c.numberOfClubs = txtNumberofClubs.Text;
                    c.clubSpec = txtClubSpec.Text;
                    c.shaftSpec = txtShaftSpec.Text;
                    c.shaftFlex = txtShaftFlex.Text;
                    c.dexterity = txtDexterity.Text;
                    c.comments = txtComments.Text;
                    c.used = chkUsed.Checked;
                    ssm.updateClub(c);
                    //changes all text boxes and dropdowns to labels
                    txtCost.Visible = false;
                    lblCostDisplay.Visible = true;
                    ddlBrand.Visible = false;
                    lblBrandDisplay.Visible = true;
                    txtPrice.Visible = false;
                    lblPriceDisplay.Visible = true;
                    txtQuantity.Visible = false;
                    lblQuantityDisplay.Visible = true;
                    ddlLocation.Visible = false;
                    lblLocationDisplay.Visible = true;
                    txtClubType.Visible = false;
                    lblClubTypeDisplay.Visible = true;
                    ddlModel.Visible = false;
                    lblModelDisplay.Visible = true;
                    txtShaft.Visible = false;
                    lblShaftDisplay.Visible = true;
                    txtNumberofClubs.Visible = false;
                    lblNumberofClubsDisplay.Visible = true;
                    txtClubSpec.Visible = false;
                    lblClubSpecDisplay.Visible = true;
                    txtShaftSpec.Visible = false;
                    lblShaftSpecDisplay.Visible = true;
                    txtShaftFlex.Visible = false;
                    lblShaftFlexDisplay.Visible = true;
                    txtDexterity.Visible = false;
                    lblDexterityDisplay.Visible = true;
                    txtComments.Visible = false;
                    lblCommentsDisplay.Visible = true;
                    chkUsed.Enabled = false;
                }
                else if (lblTypeDisplay.Text == "Accessories")
                {
                    //if item type is accesory then save as accessory class
                    a.sku = Convert.ToInt32(lblSKUDisplay.Text);
                    a.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    a.cost = Convert.ToDouble(txtCost.Text);
                    a.price = Convert.ToDouble(txtPrice.Text);
                    a.quantity = Convert.ToInt32(txtQuantity.Text);
                    a.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    a.size = txtClubType.Text;
                    a.colour = txtShaft.Text;
                    a.accessoryType = txtNumberofClubs.Text;
                    a.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                    a.comments = txtComments.Text;
                    ssm.updateAccessories(a);

                    //changes all text boxes and dropdowns to labels
                    ddlModel.Visible = false;
                    lblModelDisplay.Visible = true;
                    txtNumberofClubs.Visible = false;
                    lblNumberofClubsDisplay.Visible = true;
                    txtCost.Visible = false;
                    lblCostDisplay.Visible = true;
                    ddlBrand.Visible = false;
                    lblBrandDisplay.Visible = true;
                    txtPrice.Visible = false;
                    lblPriceDisplay.Visible = true;
                    txtQuantity.Visible = false;
                    lblQuantityDisplay.Visible = true;
                    ddlLocation.Visible = false;
                    lblLocationDisplay.Visible = true;
                    txtClubType.Visible = false;
                    lblClubTypeDisplay.Visible = true;
                    txtShaft.Visible = false;
                    lblShaftDisplay.Visible = true;
                    txtComments.Visible = false;
                    lblCommentsDisplay.Visible = true;
                }
                else if (lblTypeDisplay.Text == "Clothing")
                {
                    //if item type is clothing then save as clothing class
                    cl.sku = Convert.ToInt32(lblSKUDisplay.Text);
                    cl.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    cl.cost = Convert.ToDouble(txtCost.Text);
                    cl.price = Convert.ToDouble(txtPrice.Text);
                    cl.quantity = Convert.ToInt32(txtQuantity.Text);
                    cl.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    cl.size = txtClubType.Text;
                    cl.colour = txtShaft.Text;
                    cl.gender = txtClubSpec.Text;
                    cl.style = txtShaftFlex.Text;
                    cl.comments = txtComments.Text;
                    ssm.updateClothing(cl);

                    //changes all text boxes and dropdowns to labels
                    txtCost.Visible = false;
                    lblCostDisplay.Visible = true;
                    ddlBrand.Visible = false;
                    lblBrandDisplay.Visible = true;
                    txtPrice.Visible = false;
                    lblPriceDisplay.Visible = true;
                    txtQuantity.Visible = false;
                    lblQuantityDisplay.Visible = true;
                    ddlLocation.Visible = false;
                    lblLocationDisplay.Visible = true;
                    txtClubType.Visible = false;
                    lblClubTypeDisplay.Visible = true;
                    txtShaft.Visible = false;
                    lblShaftDisplay.Visible = true;
                    txtClubSpec.Visible = false;
                    lblClubSpecDisplay.Visible = true;
                    txtShaftFlex.Visible = false;
                    lblShaftFlexDisplay.Visible = true;
                    txtComments.Visible = false;
                    lblCommentsDisplay.Visible = true;
                }
                //hides and displays the proper buttons for access
                btnSaveItem.Visible = false;
                btnEditItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditItem";
                btnCancel.Visible = false;
                btnAddItem.Visible = false;
                btnBackToSearch.Visible = true;

                Server.Transfer(Request.RawUrl, false);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            try
            {
                //no changes saved, refreshes current page
                Server.Transfer(Request.RawUrl, false);
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnBackToSearch_Click";
            try
            {
                //removes item key and type that was set so no item is currently selected
                Session["itemType"] = null;
                Session["key"] = null;
                //Changes page to the inventory home page
                Server.Transfer("InventoryHomePage.aspx", false);
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
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlType_SelectedIndexChanged";
            try
            {
                //changes the item type, causes post back
                Session["itemType"] = ddlType.SelectedIndex;
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