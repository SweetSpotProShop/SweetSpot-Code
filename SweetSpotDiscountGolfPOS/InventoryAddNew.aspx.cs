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
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();
        ItemDataUtilities IDU = new ItemDataUtilities();
        ItemsManager IM = new ItemsManager();
        LocationManager LM = new LocationManager();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InventoryAddNew.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                if (CU.jobID != 0)
                {
                    //If user is not an admin then disable the edit item button
                    btnEditItem.Enabled = false;
                }
                //Check to see if an item was selected
                if (Convert.ToInt32(Request.QueryString["sku"].ToString()) != -10)
                {
                    if (!IsPostBack)
                    {
                        //Grabs a list of objects that match the sku in query string. There should only ever be 1 that is returned
                        List<Object> o = IDU.ReturnListOfObjectsFromThreeTables(Convert.ToInt32(Request.QueryString["sku"].ToString()));
                        ddlBrand.DataSource = IM.ReturnDropDownForBrand();
                        ddlBrand.DataTextField = "brandName";
                        ddlBrand.DataValueField = "brandID";
                        ddlBrand.DataBind();
                        ddlLocation.DataSource = LM.ReturnLocationDropDownAll();
                        ddlLocation.DataTextField = "locationName";
                        ddlLocation.DataValueField = "locationID";
                        ddlLocation.DataBind();
                        ddlType.DataSource = IM.ReturnDropDownForItemType();
                        ddlType.DataTextField = "typeDescription";
                        ddlType.DataValueField = "typeID";
                        ddlType.DataBind();
                        ddlModel.DataSource = IM.ReturnDropDownForModel();
                        ddlModel.DataTextField = "modelName";
                        ddlModel.DataValueField = "modelID";
                        ddlModel.DataBind();
                        if (o[0] is Clubs)
                        {
                            //When a club, pass class and populate DropDowns and TextBoxes
                            Clubs c = o[0] as Clubs;
                            ddlType.SelectedValue = c.typeID.ToString();
                            lblSKUDisplay.Text = c.sku.ToString();
                            txtCost.Text = c.cost.ToString();
                            ddlBrand.SelectedValue = c.brandID.ToString();
                            txtPrice.Text = c.price.ToString();
                            txtQuantity.Text = c.quantity.ToString();
                            ddlLocation.SelectedValue = c.itemlocation.ToString();

                            txtClubType.Text = c.clubType.ToString();
                            ddlModel.SelectedValue = c.modelID.ToString();
                            txtShaft.Text = c.shaft.ToString();
                            txtNumberofClubs.Text = c.numberOfClubs.ToString();
                            txtClubSpec.Text = c.clubSpec.ToString();
                            txtShaftSpec.Text = c.shaftSpec.ToString();
                            txtShaftFlex.Text = c.shaftFlex.ToString();
                            txtDexterity.Text = c.dexterity.ToString();
                            chkUsed.Checked = c.used;
                            txtComments.Text = c.comments.ToString();
                        }
                        else if (o[0] is Accessories)
                        {
                            //When accessories, pass class and populate DropDowns and TextBoxes
                            Accessories a = o[0] as Accessories;
                            ddlType.SelectedValue = a.typeID.ToString();
                            lblSKUDisplay.Text = a.sku.ToString();
                            txtCost.Text = a.cost.ToString();
                            ddlBrand.SelectedValue = a.brandID.ToString();
                            txtPrice.Text = a.price.ToString();
                            txtQuantity.Text = a.quantity.ToString();
                            ddlLocation.SelectedValue = a.locID.ToString();
                            lblNumberofClubs.Text = "Accessory Type:";
                            txtNumberofClubs.Text = a.accessoryType.ToString();

                            lblClubType.Text = "Size:";
                            txtClubType.Text = a.size.ToString();
                            ddlModel.SelectedValue = a.modelID.ToString();
                            lblShaft.Text = "Colour:";
                            txtShaft.Text = a.colour.ToString();
                            txtComments.Text = a.comments.ToString();

                            lblClubSpec.Visible = false;
                            txtClubSpec.Visible = false;
                            lblShaftSpec.Visible = false;
                            txtShaftSpec.Visible = false;
                            lblShaftFlex.Visible = false;
                            txtShaftFlex.Visible = false;
                            lblDexterity.Visible = false;
                            txtDexterity.Visible = false;
                            chkUsed.Visible = false;
                        }
                        else if (o[0] is Clothing)
                        {
                            //When clothing, pass class and populate DropDowns and TextBoxes
                            Clothing cl = o[0] as Clothing;
                            ddlType.SelectedValue = cl.typeID.ToString();
                            lblSKUDisplay.Text = cl.sku.ToString();
                            txtCost.Text = cl.cost.ToString();
                            ddlBrand.SelectedValue = cl.brandID.ToString();
                            txtPrice.Text = cl.price.ToString();
                            txtQuantity.Text = cl.quantity.ToString();
                            ddlLocation.SelectedValue = cl.locID.ToString();
                            txtComments.Text = cl.comments.ToString();

                            lblClubType.Text = "Size:";
                            txtClubType.Text = cl.size.ToString();
                            lblModel.Visible = false;
                            ddlModel.Visible = false;
                            lblShaft.Text = "Colour:";
                            txtShaft.Text = cl.colour.ToString();
                            lblNumberofClubs.Visible = false;
                            txtNumberofClubs.Visible = false;
                            lblClubSpec.Text = "Gender:";
                            txtClubSpec.Text = cl.gender.ToString();
                            lblShaftFlex.Text = "Style:";
                            txtShaftFlex.Text = cl.style.ToString();
                            lblShaftSpec.Visible = false;
                            txtShaftSpec.Visible = false;
                            lblDexterity.Visible = false;
                            txtDexterity.Visible = false;
                            chkUsed.Visible = false;
                        }
                        btnCreateSimilar.Visible = true;
                    }
                }
                else
                {
                    //When no item was selected display drop downs and text boxes
                    ddlType.Enabled = true;
                    txtCost.Enabled = true;
                    ddlBrand.Enabled = true;
                    txtPrice.Enabled = true;
                    txtQuantity.Enabled = true;
                    ddlLocation.Enabled = true;

                    txtClubType.Enabled = true;
                    txtShaft.Enabled = true;
                    txtComments.Enabled = true;

                    btnCreateSimilar.Visible = false;
                    if (!IsPostBack)
                    {
                        ddlBrand.DataSource = IM.ReturnDropDownForBrand();
                        ddlBrand.DataTextField = "brandName";
                        ddlBrand.DataValueField = "brandID";
                        ddlBrand.DataBind();
                        ddlLocation.DataSource = LM.ReturnLocationDropDownAll();
                        ddlLocation.DataTextField = "locationName";
                        ddlLocation.DataValueField = "locationID";
                        ddlLocation.DataBind();
                        ddlType.DataSource = IM.ReturnDropDownForItemType();
                        ddlType.DataTextField = "typeDescription";
                        ddlType.DataValueField = "typeID";
                        ddlType.DataBind();
                        ddlModel.DataSource = IM.ReturnDropDownForModel();
                        ddlModel.DataTextField = "modelName";
                        ddlModel.DataValueField = "modelID";
                        ddlModel.DataBind();
                        ddlLocation.SelectedValue = CU.locationID.ToString();
                        ddlType.SelectedValue = "1";
                        ddlModel.Enabled = true;
                        txtNumberofClubs.Enabled = true;
                        txtClubSpec.Enabled = true;
                        txtShaftSpec.Enabled = true;
                        txtShaftFlex.Enabled = true;
                        txtDexterity.Enabled = true;
                        chkUsed.Enabled = true;
                    }
                    else
                    {
                        //Clubs
                        if (Convert.ToInt32(ddlType.SelectedValue) == 1)
                        {
                            //adjust labels displaying for clubs
                            lblClubType.Text = "Club Type:";

                            lblModel.Visible = true;
                            ddlModel.Visible = true;
                            ddlModel.Enabled = true;

                            lblShaft.Text = "Shaft:";

                            lblNumberofClubs.Text = "Number of Clubs:";
                            lblNumberofClubs.Visible = true;
                            txtNumberofClubs.Visible = true;
                            txtNumberofClubs.Enabled = true;

                            lblClubSpec.Text = "Club Spec:";
                            lblClubSpec.Visible = true;
                            txtClubSpec.Visible = true;
                            txtClubSpec.Enabled = true;

                            lblShaftSpec.Visible = true;
                            txtShaftSpec.Visible = true;
                            txtShaftSpec.Enabled = true;

                            lblShaftFlex.Text = "Shaft Flex:";
                            lblShaftFlex.Visible = true;
                            txtShaftFlex.Visible = true;
                            txtShaftFlex.Enabled = true;

                            lblDexterity.Visible = true;
                            txtDexterity.Visible = true;
                            txtDexterity.Enabled = true;

                            chkUsed.Visible = true;
                            chkUsed.Enabled = true;
                        }
                        //Accessories
                        else if (Convert.ToInt32(ddlType.SelectedValue) == 2)
                        {
                            //adjust labels displaying for accessories
                            lblClubType.Text = "Size:";

                            lblModel.Visible = true;
                            ddlModel.Visible = true;
                            ddlModel.Enabled = true;

                            lblShaft.Text = "Colour:";

                            lblNumberofClubs.Text = "Accessory Type:";
                            lblNumberofClubs.Visible = true;
                            txtNumberofClubs.Visible = true;
                            txtNumberofClubs.Enabled = true;

                            lblClubSpec.Visible = false;
                            txtClubSpec.Visible = false;
                            txtClubSpec.Enabled = false;

                            lblShaftSpec.Visible = false;
                            txtShaftSpec.Visible = false;
                            txtShaftSpec.Enabled = false;

                            lblShaftFlex.Visible = false;
                            txtShaftFlex.Visible = false;
                            txtShaftFlex.Enabled = false;

                            lblDexterity.Visible = false;
                            txtDexterity.Visible = false;
                            txtDexterity.Enabled = false;

                            chkUsed.Visible = false;
                            chkUsed.Enabled = false;
                        }
                        //Clothing
                        else if (Convert.ToInt32(ddlType.SelectedValue) == 3)
                        {
                            //adjust labels displaying for clubs
                            lblClubType.Text = "Size:";

                            lblModel.Visible = false;
                            ddlModel.Visible = false;
                            ddlModel.Enabled = false;

                            lblShaft.Text = "Colour:";

                            lblNumberofClubs.Visible = false;
                            txtNumberofClubs.Visible = false;
                            txtNumberofClubs.Enabled = false;

                            lblClubSpec.Text = "Gender:";
                            lblClubSpec.Visible = true;
                            txtClubSpec.Visible = true;
                            txtClubSpec.Enabled = true;

                            lblShaftSpec.Visible = false;
                            txtShaftSpec.Visible = false;
                            txtShaftSpec.Enabled = false;

                            lblShaftFlex.Text = "Style:";
                            lblShaftFlex.Visible = true;
                            txtShaftFlex.Visible = true;
                            txtShaftFlex.Enabled = true;

                            lblDexterity.Visible = false;
                            txtDexterity.Visible = false;
                            txtDexterity.Enabled = false;

                            chkUsed.Visible = false;
                            chkUsed.Enabled = false;
                        }
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
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddItem_Click";
            try
            {
                //Retrieves the type of item that is getting added
                Object o = new Object();
                int typeID = Convert.ToInt32(ddlType.SelectedValue);
                if (typeID == 1)
                {
                    Clubs c = new Clubs();
                    //Transfers all info into Club class
                    c.sku = IDU.ReturnMaxSku(typeID);
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
                    c.typeID = typeID;
                    //stores club as an object
                    o = c as Object;
                }
                else if (typeID == 2)
                {
                    Accessories a = new Accessories();
                    //Transfers all info into Accessory class
                    a.sku = IDU.ReturnMaxSku(typeID);
                    a.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    a.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                    a.cost = Convert.ToDouble(txtCost.Text);
                    a.price = Convert.ToDouble(txtPrice.Text);
                    a.quantity = Convert.ToInt32(txtQuantity.Text);
                    a.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    a.typeID = typeID;
                    a.size = txtClubType.Text;
                    a.colour = txtShaft.Text;
                    a.accessoryType = txtNumberofClubs.Text;
                    a.comments = txtComments.Text;
                    //stores accessory as an object
                    o = a as Object;
                }
                else if (typeID == 3)
                {
                    Clothing cl = new Clothing();
                    //Transfers all info into Clothing class
                    cl.sku = IDU.ReturnMaxSku(typeID);
                    cl.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    cl.cost = Convert.ToDouble(txtCost.Text);
                    cl.price = Convert.ToDouble(txtPrice.Text);
                    cl.quantity = Convert.ToInt32(txtQuantity.Text);
                    cl.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    cl.typeID = typeID;
                    cl.size = txtClubType.Text;
                    cl.colour = txtShaft.Text;
                    cl.gender = txtClubSpec.Text;
                    cl.style = txtShaftFlex.Text;
                    cl.comments = txtComments.Text;
                    //stores clothing as an object
                    o = cl as Object;
                }

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("sku", IDU.AddNewItemToDatabase(o).ToString());
                //Refreshes current page
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnEditItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditItem_Click";
            try
            {
                txtCost.Enabled = true;
                ddlBrand.Enabled = true;
                txtPrice.Enabled = true;
                txtQuantity.Enabled = true;
                ddlLocation.Enabled = true;
                txtClubType.Enabled = true;
                txtShaft.Enabled = true;
                txtComments.Enabled = true;

                if (Convert.ToInt32(ddlType.SelectedValue) == 1)
                {
                    ddlModel.Enabled = true;
                    txtNumberofClubs.Enabled = true;
                    txtClubSpec.Enabled = true;
                    txtShaftSpec.Enabled = true;
                    txtShaftFlex.Enabled = true;
                    txtDexterity.Enabled = true;
                    chkUsed.Enabled = true;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 2)
                {
                    ddlModel.Enabled = true;
                    txtNumberofClubs.Enabled = true;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 3)
                {
                    txtClubSpec.Enabled = true;
                    txtShaftFlex.Enabled = true;
                }
                //hides and displays the proper buttons for access
                btnSaveItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveItem";
                btnEditItem.Visible = false;
                btnAddItem.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
                btnCreateSimilar.Visible = false;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveItem_Click";
            try
            {
                txtCost.Enabled = false;
                ddlBrand.Enabled = false;
                txtPrice.Enabled = false;
                txtQuantity.Enabled = false;
                ddlLocation.Enabled = false;

                txtClubType.Enabled = false;
                txtShaft.Enabled = false;
                txtComments.Enabled = false;

                Object o = new Object();
                if (Convert.ToInt32(ddlType.SelectedValue) == 1)
                {
                    Clubs c = new Clubs();
                    //if item type is club then save as club class
                    c.sku = Convert.ToInt32(Request.QueryString["sku"].ToString());
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
                    o = c as Object;

                    //changes all text boxes and dropdowns to labels
                    ddlModel.Enabled = false;
                    txtNumberofClubs.Enabled = false;
                    txtClubSpec.Enabled = false;
                    txtShaftSpec.Enabled = false;
                    txtShaftFlex.Enabled = false;
                    txtDexterity.Enabled = false;
                    chkUsed.Enabled = false;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 2)
                {
                    Accessories a = new Accessories();
                    //if item type is accesory then save as accessory class
                    a.sku = Convert.ToInt32(Request.QueryString["sku"].ToString());
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
                    o = a as Object;

                    //changes all text boxes and dropdowns to labels
                    ddlModel.Enabled = false;
                    txtNumberofClubs.Enabled = false;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 3)
                {
                    Clothing cl = new Clothing();
                    //if item type is clothing then save as clothing class
                    cl.sku = Convert.ToInt32(Request.QueryString["sku"].ToString());
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
                    o = cl as Object;

                    //changes all text boxes and dropdowns to labels
                    txtClubSpec.Enabled = false;
                    txtShaftFlex.Enabled = false;
                }
                //hides and displays the proper buttons for access
                btnSaveItem.Visible = false;
                btnEditItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditItem";
                btnCancel.Visible = false;
                btnAddItem.Visible = false;
                btnBackToSearch.Visible = true;
                btnCreateSimilar.Visible = true;

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("sku", IDU.UpdateItemInDatabase(o).ToString());
                //Refreshes current page
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            try
            {
                //no changes saved, refreshes current page
                Response.Redirect(Request.RawUrl, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnBackToSearch_Click";
            try
            {
                //Changes page to the inventory home page
                Response.Redirect("InventoryHomePage.aspx", false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlType_SelectedIndexChanged";
            try
            {
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        protected void btnCreateSimilar_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCreateSimilar_Click";
            try
            {
                //Retrieves the type of item that is getting added
                int typeID = Convert.ToInt32(ddlType.SelectedValue);
                Object o = new Object();
                if (typeID == 1)
                {
                    Clubs c = new Clubs();
                    //Transfers all info into Club class
                    c.sku = IDU.ReturnMaxSku(typeID);
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
                    c.typeID = typeID;
                    //stores club as an object
                    o = c as Object;
                }
                else if (typeID == 2)
                {
                    Accessories a = new Accessories();
                    //Transfers all info into Accessory class
                    a.sku = IDU.ReturnMaxSku(typeID);
                    a.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    a.cost = Convert.ToDouble(txtCost.Text);
                    a.price = Convert.ToDouble(txtPrice.Text);
                    a.quantity = Convert.ToInt32(txtQuantity.Text);
                    a.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    a.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                    a.typeID = typeID;
                    a.size = txtClubType.Text;
                    a.colour = txtShaft.Text;
                    a.accessoryType = txtNumberofClubs.Text;
                    a.comments = txtComments.Text;
                    //stores accessory as an object
                    o = a as Object;
                }
                else if (typeID == 3)
                {
                    Clothing cl = new Clothing();
                    //Transfers all info into Clothing class
                    cl.sku = IDU.ReturnMaxSku(typeID);
                    cl.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    cl.cost = Convert.ToDouble(txtCost.Text);
                    cl.price = Convert.ToDouble(txtPrice.Text);
                    cl.quantity = Convert.ToInt32(txtQuantity.Text);
                    cl.locID = Convert.ToInt32(ddlLocation.SelectedValue);
                    cl.typeID = typeID;
                    cl.size = txtClubType.Text;
                    cl.colour = txtShaft.Text;
                    cl.gender = txtClubSpec.Text;
                    cl.style = txtShaftFlex.Text;
                    cl.comments = txtComments.Text;
                    //stores clothing as an object
                    o = cl as Object;
                }

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("sku", IDU.AddNewItemToDatabase(o).ToString());
                //Refreshes current page
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3 Test", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}