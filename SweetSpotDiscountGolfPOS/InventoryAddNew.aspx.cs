using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class InventoryAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        ItemChangeTracking changeItem;
        ItemDataUtilities IDU = new ItemDataUtilities();
        ItemsManager IM = new ItemsManager();
        LocationManager LM = new LocationManager();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "InventoryAddNew.aspx";
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
                    if (CU.employee.intJobID != 0)
                    {
                        //If user is not an admin then disable the edit item button
                        btnEditItem.Enabled = false;
                    }
                    //Check to see if an item was selected
                    if (Convert.ToInt32(Request.QueryString["inventory"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            ItemChangeTracking tempItem = new ItemChangeTracking();
                            //Grabs a list of objects that match the sku in query string. There should only ever be 1 that is returned
                            List<Object> o = IDU.CallReturnListOfObjectsFromThreeTablesForInventoryAddNew(Convert.ToInt32(Request.QueryString["inventory"].ToString()), objPageDetails, DateTime.Now, CU.location.intProvinceID);
                            ddlBrand.DataSource = IM.CallReturnDropDownForBrand(objPageDetails);
                            ddlBrand.DataBind();
                            ddlLocation.DataSource = LM.CallReturnLocationDropDown(objPageDetails);
                            ddlLocation.DataBind();
                            ddlType.DataSource = IM.CallReturnDropDownForItemType(objPageDetails);
                            ddlType.DataBind();
                            ddlModel.DataSource = IM.CallReturnDropDownForModel(objPageDetails);
                            ddlModel.DataBind();
                            if (o[0] is Clubs)
                            {
                                //When a club, pass class and populate DropDowns and TextBoxes
                                Clubs club = o[0] as Clubs;
                                ddlType.SelectedValue = club.intItemTypeID.ToString();
                                lblSKUDisplay.Text = club.varSku.ToString();
                                txtCost.Text = club.fltCost.ToString();
                                ddlBrand.SelectedValue = club.intBrandID.ToString();
                                txtPrice.Text = club.fltPrice.ToString();
                                txtQuantity.Text = club.intQuantity.ToString();
                                ddlLocation.SelectedValue = club.intLocationID.ToString();

                                txtClubType.Text = club.varTypeOfClub.ToString();
                                ddlModel.SelectedValue = club.intModelID.ToString();
                                txtShaft.Text = club.varShaftType.ToString();
                                txtNumberofClubs.Text = club.varNumberOfClubs.ToString();
                                txtClubSpec.Text = club.varClubSpecification.ToString();
                                txtShaftSpec.Text = club.varShaftSpecification.ToString();
                                txtShaftFlex.Text = club.varShaftFlexability.ToString();
                                txtDexterity.Text = club.varClubDexterity.ToString();
                                chkUsed.Checked = club.bitIsUsedProduct;
                                txtComments.Text = club.varAdditionalInformation.ToString();

                                grdInventoryTaxes.DataSource = club.lstTaxTypePerInventoryItem;
                                grdInventoryTaxes.DataBind();

                                tempItem.intInventoryID = club.intInventoryID;
                                tempItem.fltOriginalCost = club.fltCost;
                                tempItem.fltOriginalPrice = club.fltPrice;
                                tempItem.intOriginalQuantity = club.intQuantity;
                                tempItem.varOriginalDescription = "Location ID: " + club.intLocationID.ToString() + "; Brand ID: " + club.intBrandID.ToString()
                                    + "; Model ID: " + club.intModelID.ToString() + "; Club Type: " + club.varTypeOfClub.ToString() + "; Shaft: " + club.varShaftType.ToString()
                                    + "; Number of Clubs: " + club.varNumberOfClubs.ToString() + "; Club Spec: " + club.varClubSpecification.ToString() + "; Shaft Spec: " 
                                    + club.varShaftSpecification.ToString() + "; Shaft Flex: " + club.varShaftFlexability.ToString() + "; Dexterity: " + club.varClubDexterity.ToString()
                                    + "; Used: " + club.bitIsUsedProduct.ToString() + "; Comments: " + club.varAdditionalInformation.ToString();
                            }
                            else if (o[0] is Accessories)
                            {
                                //When accessories, pass class and populate DropDowns and TextBoxes
                                Accessories accessory = o[0] as Accessories;
                                ddlType.SelectedValue = accessory.intItemTypeID.ToString();
                                lblSKUDisplay.Text = accessory.varSku.ToString();
                                txtCost.Text = accessory.fltCost.ToString();
                                ddlBrand.SelectedValue = accessory.intBrandID.ToString();
                                txtPrice.Text = accessory.fltPrice.ToString();
                                txtQuantity.Text = accessory.intQuantity.ToString();
                                ddlLocation.SelectedValue = accessory.intLocationID.ToString();
                                lblClubType.Text = "Accessory Type:";
                                txtClubType.Text = accessory.varTypeOfAccessory.ToString();

                                lblNumberofClubs.Text = "Size:";
                                txtNumberofClubs.Text = accessory.varSize.ToString();
                                ddlModel.SelectedValue = accessory.intModelID.ToString();
                                lblShaft.Text = "Colour:";
                                txtShaft.Text = accessory.varColour.ToString();
                                txtComments.Text = accessory.varAdditionalInformation.ToString();

                                lblClubSpec.Visible = false;
                                txtClubSpec.Visible = false;
                                lblShaftSpec.Visible = false;
                                txtShaftSpec.Visible = false;
                                lblShaftFlex.Visible = false;
                                txtShaftFlex.Visible = false;
                                lblDexterity.Visible = false;
                                txtDexterity.Visible = false;
                                chkUsed.Visible = false;

                                grdInventoryTaxes.DataSource = accessory.lstTaxTypePerInventoryItem;
                                grdInventoryTaxes.DataBind();

                                tempItem.intInventoryID = accessory.intInventoryID;
                                tempItem.fltOriginalCost = accessory.fltCost;
                                tempItem.fltOriginalPrice = accessory.fltPrice;
                                tempItem.intOriginalQuantity = accessory.intQuantity;
                                tempItem.varOriginalDescription = "Location ID: " + accessory.intLocationID.ToString() + "; Brand ID: " + accessory.intBrandID.ToString()
                                    + "; Model ID: " + accessory.intModelID.ToString() + "; Size: " + accessory.varSize.ToString() + "; Colour: " + accessory.varColour.ToString()
                                    + "; Accessory Type: " + accessory.varTypeOfAccessory.ToString() + "; Comments: " + accessory.varAdditionalInformation.ToString();
                            }
                            else if (o[0] is Clothing)
                            {
                                //When clothing, pass class and populate DropDowns and TextBoxes
                                Clothing clothing = o[0] as Clothing;
                                ddlType.SelectedValue = clothing.intItemTypeID.ToString();
                                lblSKUDisplay.Text = clothing.varSku.ToString();
                                txtCost.Text = clothing.fltCost.ToString();
                                ddlBrand.SelectedValue = clothing.intBrandID.ToString();
                                txtPrice.Text = clothing.fltPrice.ToString();
                                txtQuantity.Text = clothing.intQuantity.ToString();
                                ddlLocation.SelectedValue = clothing.intLocationID.ToString();
                                txtComments.Text = clothing.varAdditionalInformation.ToString();

                                lblNumberofClubs.Text = "Size:";
                                txtNumberofClubs.Text = clothing.varSize.ToString();
                                lblModel.Visible = false;
                                ddlModel.Visible = false;
                                lblShaft.Text = "Colour:";
                                txtShaft.Text = clothing.varColour.ToString();
                                lblShaftFlex.Visible = false;
                                txtShaftFlex.Visible = false;
                                lblClubSpec.Text = "Gender:";
                                txtClubSpec.Text = clothing.varGender.ToString();
                                lblClubType.Text = "Style:";
                                txtClubType.Text = clothing.varStyle.ToString();
                                lblShaftSpec.Visible = false;
                                txtShaftSpec.Visible = false;
                                lblDexterity.Visible = false;
                                txtDexterity.Visible = false;
                                chkUsed.Visible = false;

                                grdInventoryTaxes.DataSource = clothing.lstTaxTypePerInventoryItem;
                                grdInventoryTaxes.DataBind();

                                tempItem.intInventoryID = clothing.intInventoryID;
                                tempItem.fltOriginalCost = clothing.fltCost;
                                tempItem.fltOriginalPrice = clothing.fltPrice;
                                tempItem.intOriginalQuantity = clothing.intQuantity;
                                tempItem.varOriginalDescription = "Location ID: " + clothing.intLocationID.ToString() + "; Brand ID: " + clothing.intBrandID.ToString()
                                    + "; Size: " + clothing.varSize.ToString() + "; Colour: " + clothing.varColour.ToString() + "; Gender: " + clothing.varGender.ToString()
                                    + "; Style: " + clothing.varStyle.ToString() + "; Comments: " + clothing.varAdditionalInformation.ToString();
                            }
                            btnCreateSimilar.Visible = true;
                            changeItem = tempItem;
                            Session["changer"] = changeItem;
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
                            ddlBrand.DataSource = IM.CallReturnDropDownForBrand(objPageDetails);
                            ddlBrand.DataBind();
                            ddlLocation.DataSource = LM.CallReturnLocationDropDown(objPageDetails);
                            ddlLocation.DataBind();
                            ddlType.DataSource = IM.CallReturnDropDownForItemType(objPageDetails);
                            ddlType.DataBind();
                            ddlModel.DataSource = IM.CallReturnDropDownForModel(objPageDetails);
                            ddlModel.DataBind();
                            ddlLocation.SelectedValue = CU.location.intLocationID.ToString();
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
                                lblNumberofClubs.Text = "Size:";

                                lblModel.Visible = true;
                                ddlModel.Visible = true;
                                ddlModel.Enabled = true;

                                lblShaft.Text = "Colour:";

                                lblClubType.Text = "Accessory Type:";
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
                                lblNumberofClubs.Text = "Size:";

                                lblModel.Visible = false;
                                ddlModel.Visible = false;
                                ddlModel.Enabled = false;

                                lblShaft.Text = "Colour:";

                                lblNumberofClubs.Visible = true;
                                txtNumberofClubs.Visible = true;
                                txtNumberofClubs.Enabled = true;

                                lblClubSpec.Text = "Gender:";
                                lblClubSpec.Visible = true;
                                txtClubSpec.Visible = true;
                                txtClubSpec.Enabled = true;

                                lblShaftSpec.Visible = false;
                                txtShaftSpec.Visible = false;
                                txtShaftSpec.Enabled = false;

                                lblClubType.Text = "Style:";
                                lblShaftFlex.Visible = false;
                                txtShaftFlex.Visible = false;
                                txtShaftFlex.Enabled = false;

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
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnAddItem_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Retrieves the type of item that is getting added
                Object o = new Object();
                int typeID = Convert.ToInt32(ddlType.SelectedValue);
                if (typeID == 1)
                {
                    Clubs club = new Clubs();
                    //Transfers all info into Club class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(typeID, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    club.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    club.varSku = inventoryInfo[0].ToString();
                    club.fltCost = Convert.ToDouble(txtCost.Text);
                    club.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    club.fltPrice = Convert.ToDouble(txtPrice.Text);
                    club.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    club.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    club.varTypeOfClub = txtClubType.Text;
                    club.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                    club.varShaftType = txtShaft.Text;
                    club.varNumberOfClubs = txtNumberofClubs.Text;
                    club.varClubSpecification = txtClubSpec.Text;
                    club.varShaftSpecification = txtShaftSpec.Text;
                    club.varShaftFlexability = txtShaftFlex.Text;
                    club.varClubDexterity = txtDexterity.Text;
                    club.bitIsUsedProduct = chkUsed.Checked;
                    club.varAdditionalInformation = txtComments.Text;
                    club.intItemTypeID = typeID;
                    //stores club as an object
                    o = club as Object;
                }
                else if (typeID == 2)
                {
                    Accessories accessory = new Accessories();
                    //Transfers all info into Accessory class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(typeID, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    accessory.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    accessory.varSku = inventoryInfo[0].ToString();
                    accessory.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    accessory.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                    accessory.fltCost = Convert.ToDouble(txtCost.Text);
                    accessory.fltPrice = Convert.ToDouble(txtPrice.Text);
                    accessory.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    accessory.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    accessory.intItemTypeID = typeID;
                    accessory.varSize = txtNumberofClubs.Text;
                    accessory.varColour = txtShaft.Text;
                    accessory.varTypeOfAccessory = txtClubType.Text;
                    accessory.varAdditionalInformation = txtComments.Text;
                    //stores accessory as an object
                    o = accessory as Object;
                }
                else if (typeID == 3)
                {
                    Clothing clothing = new Clothing();
                    //Transfers all info into Clothing class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(typeID, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    clothing.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    clothing.varSku = inventoryInfo[0].ToString();
                    clothing.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    clothing.fltCost = Convert.ToDouble(txtCost.Text);
                    clothing.fltPrice = Convert.ToDouble(txtPrice.Text);
                    clothing.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    clothing.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    clothing.intItemTypeID = typeID;
                    clothing.varSize = txtNumberofClubs.Text;
                    clothing.varColour = txtShaft.Text;
                    clothing.varGender = txtClubSpec.Text;
                    clothing.varStyle = txtClubType.Text;
                    clothing.varAdditionalInformation = txtComments.Text;
                    //stores clothing as an object
                    o = clothing as Object;
                }

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inventory", IDU.AddNewItemToDatabase(o, objPageDetails).ToString());
                //Refreshes current page
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void btnEditItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnEditItem_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                    txtNumberofClubs.Enabled = true;
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnSaveItem_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                changeItem = (ItemChangeTracking)Session["changer"];
                object o = new object();
                if (Convert.ToInt32(ddlType.SelectedValue) == 1)
                {
                    Clubs club = new Clubs();
                    //if item type is club then save as club class
                    club.intInventoryID = Convert.ToInt32(Request.QueryString["inventory"].ToString());
                    club.fltCost = Convert.ToDouble(txtCost.Text);
                    club.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    club.fltPrice = Convert.ToDouble(txtPrice.Text);
                    club.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    club.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    club.varTypeOfClub = txtClubType.Text;
                    club.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                    club.varShaftType = txtShaft.Text;
                    club.varNumberOfClubs = txtNumberofClubs.Text;
                    club.varClubSpecification = txtClubSpec.Text;
                    club.varShaftSpecification = txtShaftSpec.Text;
                    club.varShaftFlexability = txtShaftFlex.Text;
                    club.varClubDexterity = txtDexterity.Text;
                    club.varAdditionalInformation = txtComments.Text;
                    club.bitIsUsedProduct = chkUsed.Checked;
                    o = club as object;

                    changeItem.fltNewCost = club.fltCost;
                    changeItem.fltNewPrice = club.fltPrice;
                    changeItem.intNewQuantity = club.intQuantity;
                    changeItem.varNewDescription = "Location ID: " + club.intLocationID.ToString() + "; Brand ID: " + club.intBrandID.ToString() + "; Model ID: "
                        + club.intModelID.ToString() + "; Club Type: " + club.varTypeOfClub.ToString() + "; Shaft: " + club.varShaftType.ToString() + ";  Number "
                        + "of Clubs: " + club.varNumberOfClubs.ToString() + "; Club Spec: " + club.varClubSpecification.ToString() + "; Shaft Spec: " 
                        + club.varShaftSpecification.ToString() + "; Shaft Flex: " + club.varShaftFlexability.ToString() + "; Dexterity: " 
                        + club.varClubDexterity.ToString() + "; Used: " + club.bitIsUsedProduct.ToString() + "; Comments: " + club.varAdditionalInformation.ToString();
                    
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
                    Accessories accessory = new Accessories();
                    //if item type is accesory then save as accessory class
                    accessory.intInventoryID = Convert.ToInt32(Request.QueryString["inventory"].ToString());
                    accessory.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    accessory.fltCost = Convert.ToDouble(txtCost.Text);
                    accessory.fltPrice = Convert.ToDouble(txtPrice.Text);
                    accessory.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    accessory.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    accessory.varSize = txtNumberofClubs.Text;
                    accessory.varColour = txtShaft.Text;
                    accessory.varTypeOfAccessory = txtClubType.Text;
                    accessory.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                    accessory.varAdditionalInformation = txtComments.Text;
                    o = accessory as object;

                    changeItem.fltNewCost = accessory.fltCost;
                    changeItem.fltNewPrice = accessory.fltPrice;
                    changeItem.intNewQuantity = accessory.intQuantity;
                    changeItem.varNewDescription = "Location ID: " + accessory.intLocationID.ToString() + "; Brand ID: " + accessory.intBrandID.ToString() + "; Model ID: "
                        + accessory.intModelID.ToString() + "; Size: " + accessory.varSize.ToString() + "; Colour: " + accessory.varColour.ToString() + ";  Accessory Type: "
                        + accessory.varTypeOfAccessory.ToString() + "; Comments: " + accessory.varAdditionalInformation.ToString();

                    //changes all text boxes and dropdowns to labels
                    ddlModel.Enabled = false;
                    txtNumberofClubs.Enabled = false;
                }
                else if (Convert.ToInt32(ddlType.SelectedValue) == 3)
                {
                    Clothing clothing = new Clothing();
                    //if item type is clothing then save as clothing class
                    clothing.intInventoryID = Convert.ToInt32(Request.QueryString["inventory"].ToString());
                    clothing.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    clothing.fltCost = Convert.ToDouble(txtCost.Text);
                    clothing.fltPrice = Convert.ToDouble(txtPrice.Text);
                    clothing.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    clothing.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    clothing.varSize = txtNumberofClubs.Text;
                    clothing.varColour = txtShaft.Text;
                    clothing.varGender = txtClubSpec.Text;
                    clothing.varStyle = txtClubType.Text;
                    clothing.varAdditionalInformation = txtComments.Text;
                    o = clothing as object;

                    changeItem.fltNewCost = clothing.fltCost;
                    changeItem.fltNewPrice = clothing.fltPrice;
                    changeItem.intNewQuantity = clothing.intQuantity;
                    changeItem.varNewDescription = "Location ID: " + clothing.intLocationID.ToString() + "; Brand ID: " + clothing.intBrandID.ToString() + "; Size: "
                        + clothing.varSize.ToString() + "; Colour: " + clothing.varColour.ToString() + ";  Gender: " + clothing.varGender.ToString() + "; Style: " 
                        + clothing.varStyle.ToString() + "; Comments: " + clothing.varAdditionalInformation.ToString();

                    //changes all text boxes and dropdowns to labels
                    txtClubSpec.Enabled = false;
                    txtNumberofClubs.Enabled = false;
                }
                //hides and displays the proper buttons for access
                btnSaveItem.Visible = false;
                btnEditItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditItem";
                btnCancel.Visible = false;
                btnAddItem.Visible = false;
                btnBackToSearch.Visible = true;
                btnCreateSimilar.Visible = true;

                object[] extra = { CU.employee.intEmployeeID, CU.location.intLocationID };
                IDU.SaveInventoryChanges(changeItem, extra, objPageDetails);

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inventory", IDU.UpdateItemInDatabase(o, objPageDetails).ToString());
                //Refreshes current page
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCancel_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnBackToSearch_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "ddlType_SelectedIndexChanged";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
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
        protected void btnCreateSimilar_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCreateSimilar_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Retrieves the type of item that is getting added
                int typeID = Convert.ToInt32(ddlType.SelectedValue);
                Object o = new Object();
                if (typeID == 1)
                {
                    Clubs club = new Clubs();
                    //Transfers all info into Club class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(typeID, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    club.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    club.varSku = inventoryInfo[0].ToString();
                    club.fltCost = Convert.ToDouble(txtCost.Text);
                    club.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    club.fltPrice = Convert.ToDouble(txtPrice.Text);
                    club.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    club.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    club.varTypeOfClub = txtClubType.Text;
                    club.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                    club.varShaftType = txtShaft.Text;
                    club.varNumberOfClubs = txtNumberofClubs.Text;
                    club.varClubSpecification = txtClubSpec.Text;
                    club.varShaftSpecification = txtShaftSpec.Text;
                    club.varShaftFlexability = txtShaftFlex.Text;
                    club.varClubDexterity = txtDexterity.Text;
                    club.bitIsUsedProduct = chkUsed.Checked;
                    club.varAdditionalInformation = txtComments.Text;
                    club.intItemTypeID = typeID;
                    //stores club as an object
                    o = club as Object;
                }
                else if (typeID == 2)
                {
                    Accessories accessory = new Accessories();
                    //Transfers all info into Accessory class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(typeID, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    accessory.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    accessory.varSku = inventoryInfo[0].ToString();
                    accessory.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    accessory.fltCost = Convert.ToDouble(txtCost.Text);
                    accessory.fltPrice = Convert.ToDouble(txtPrice.Text);
                    accessory.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    accessory.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    accessory.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                    accessory.intItemTypeID = typeID;
                    accessory.varSize = txtNumberofClubs.Text;
                    accessory.varColour = txtShaft.Text;
                    accessory.varTypeOfAccessory = txtClubType.Text;
                    accessory.varAdditionalInformation = txtComments.Text;
                    //stores accessory as an object
                    o = accessory as Object;
                }
                else if (typeID == 3)
                {
                    Clothing clothing = new Clothing();
                    //Transfers all info into Clothing class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(typeID, Convert.ToInt32(ddlLocation.SelectedValue), objPageDetails);
                    clothing.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    clothing.varSku = inventoryInfo[0].ToString();
                    clothing.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                    clothing.fltCost = Convert.ToDouble(txtCost.Text);
                    clothing.fltPrice = Convert.ToDouble(txtPrice.Text);
                    clothing.intQuantity = Convert.ToInt32(txtQuantity.Text);
                    clothing.intLocationID = Convert.ToInt32(ddlLocation.SelectedValue);
                    clothing.intItemTypeID = typeID;
                    clothing.varSize = txtNumberofClubs.Text;
                    clothing.varColour = txtShaft.Text;
                    clothing.varGender = txtClubSpec.Text;
                    clothing.varStyle = txtClubType.Text;
                    clothing.varAdditionalInformation = txtComments.Text;
                    //stores clothing as an object
                    o = clothing as Object;
                }

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inventory", IDU.AddNewItemToDatabase(o, objPageDetails).ToString());
                //Refreshes current page
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
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
        protected void grdInventoryTaxes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "bitChargeTax")))
                {
                    LinkButton lCharge = (LinkButton)e.Row.FindControl("lbtnChangeCharged");
                    lCharge.Text = "Charge";
                }
            }
        }
        protected void grdInventoryTaxes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string method = "grdInventoryTaxes_RowCommand";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
            bool chargeTax = Convert.ToBoolean(((CheckBox)grdInventoryTaxes.Rows[index].Cells[3].FindControl("chkChargeTax")).Checked);
            if (chargeTax)
            {
                chargeTax = false;
            }
            else
            {
                chargeTax = true;
            }
            TaxManager TM = new TaxManager();
            TM.UpdateTaxChargedForInventory(Convert.ToInt32(Request.QueryString["inventory"].ToString()), Convert.ToInt32(e.CommandArgument.ToString()), chargeTax, objPageDetails);
            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            nameValues.Set("inventory", Request.QueryString["inventory"].ToString());
            //Refreshes current page
            Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, true);
        }
    }
}