using SweetShop;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class ImportExport
    {
        //DatabaseCalls DBC = new DatabaseCalls();
        public void ImportNewItem(DataRow row, CurrentUser cu, object[] objPageDetails)
        {
            ItemDataUtilities IDU = new ItemDataUtilities();
            int inventoryID = 0;
            int itemTypeID = Convert.ToInt32(row[14]);
            string sku = row[0].ToString();
            Object o = new Object();

            inventoryID = IDU.CheckIfSkuAlreadyInDatabase(sku, itemTypeID, objPageDetails);

            if (inventoryID > 0)
            {
                //Check for item in table
                if (itemTypeID == 1)
                {
                    Clubs club = new Clubs();
                    //if item type is club then save as club class
                    club.intInventoryID = inventoryID;
                    club.fltCost = Convert.ToDouble(row[7]);
                    club.intBrandID = Convert.ToInt32(row[1]);
                    club.fltPrice = Convert.ToDouble(row[8]);
                    club.intQuantity = Convert.ToInt32(row[9]);
                    club.intLocationID = Convert.ToInt32(row[15]);
                    club.varTypeOfClub = row[3].ToString();
                    club.intModelID = Convert.ToInt32(row[2]);
                    club.varShaftType = row[4].ToString();
                    club.varNumberOfClubs = row[5].ToString();
                    club.varClubSpecification = row[10].ToString();
                    club.varShaftSpecification = row[11].ToString();
                    club.varShaftFlexability = row[12].ToString();
                    club.varClubDexterity = row[13].ToString();
                    club.varAdditionalInformation = row[16].ToString();
                    club.bitIsUsedProduct = Convert.ToBoolean(row[17]);
                    o = club as Object;
                }
                else if (itemTypeID == 2)
                {
                    Accessories accessory = new Accessories();
                    //if item type is accesory then save as accessory class
                    accessory.intInventoryID = inventoryID;
                    accessory.intBrandID = Convert.ToInt32(row[1]);
                    accessory.fltCost = Convert.ToDouble(row[7]);
                    accessory.fltPrice = Convert.ToDouble(row[8]);
                    accessory.intQuantity = Convert.ToInt32(row[9]);
                    accessory.intLocationID = Convert.ToInt32(row[15]);
                    accessory.varSize = row[5].ToString();
                    accessory.varColour = row[4].ToString();
                    accessory.varTypeOfAccessory = row[3].ToString();
                    accessory.intModelID = Convert.ToInt32(row[2]);
                    accessory.varAdditionalInformation = row[16].ToString();
                    o = accessory as Object;
                }
                else if(itemTypeID == 3)
                {
                    Clothing clothing = new Clothing();
                    //if item type is clothing then save as clothing class
                    clothing.intInventoryID = inventoryID;
                    clothing.intBrandID = Convert.ToInt32(row[1]);
                    clothing.fltCost = Convert.ToDouble(row[7]);
                    clothing.fltPrice = Convert.ToDouble(row[8]);
                    clothing.intQuantity = Convert.ToInt32(row[9]);
                    clothing.intLocationID = Convert.ToInt32(row[15]);
                    clothing.varSize = row[5].ToString();
                    clothing.varColour = row[4].ToString();
                    clothing.varGender = row[10].ToString();
                    clothing.varStyle = row[3].ToString();
                    clothing.varAdditionalInformation = row[16].ToString();
                    o = clothing as Object;
                }
                IDU.UpdateItemInDatabase(o, objPageDetails);
            }
            else
            {
                if (itemTypeID == 1)
                {
                    Clubs club = new Clubs();
                    //Transfers all info into Club class
                    string[] inventoryInfo = IDU.ReturnMaxSku(itemTypeID, cu.location.intLocationID, objPageDetails);
                    club.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    club.varSku = sku;
                    club.fltCost = Convert.ToDouble(row[7]);
                    club.intBrandID = Convert.ToInt32(row[1]);                    
                    club.fltPrice = Convert.ToDouble(row[8]);
                    club.intQuantity = Convert.ToInt32(row[9]);
                    club.intLocationID = Convert.ToInt32(row[15]);
                    club.varTypeOfClub = row[3].ToString();
                    club.intModelID = Convert.ToInt32(row[2]);
                    club.varShaftType = row[4].ToString();
                    club.varNumberOfClubs = row[5].ToString();
                    club.varClubSpecification = row[10].ToString();
                    club.varShaftSpecification = row[11].ToString();
                    club.varShaftFlexability = row[12].ToString();
                    club.varClubDexterity = row[13].ToString();
                    club.bitIsUsedProduct = Convert.ToBoolean(row[17]);
                    club.varAdditionalInformation = row[16].ToString();
                    club.intItemTypeID = itemTypeID;
                    //stores club as an object
                    o = club as Object;
                }
                else if (itemTypeID == 2)
                {
                    Accessories accessory = new Accessories();
                    //Transfers all info into Accessory class
                    string[] inventoryInfo = IDU.ReturnMaxSku(itemTypeID, cu.location.intLocationID, objPageDetails);
                    accessory.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    accessory.varSku = sku;
                    accessory.intBrandID = Convert.ToInt32(row[1]);
                    accessory.intModelID = Convert.ToInt32(row[2]);
                    accessory.fltCost = Convert.ToDouble(row[7]);
                    accessory.fltPrice = Convert.ToDouble(row[8]);
                    accessory.intQuantity = Convert.ToInt32(row[9]);
                    accessory.intLocationID = Convert.ToInt32(row[15]);
                    accessory.intItemTypeID = itemTypeID;
                    accessory.varSize = row[5].ToString();
                    accessory.varColour = row[4].ToString();
                    accessory.varTypeOfAccessory = row[3].ToString();
                    accessory.varAdditionalInformation = row[16].ToString();
                    //stores accessory as an object
                    o = accessory as Object;
                }
                else if (itemTypeID == 3)
                {
                    Clothing clothing = new Clothing();
                    //Transfers all info into Clothing class
                    string[] inventoryInfo = IDU.ReturnMaxSku(itemTypeID, cu.location.intLocationID, objPageDetails);
                    clothing.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    clothing.varSku = sku;
                    clothing.intBrandID = Convert.ToInt32(row[1]);
                    clothing.fltCost = Convert.ToDouble(row[7]);
                    clothing.fltPrice = Convert.ToDouble(row[8]);
                    clothing.intQuantity = Convert.ToInt32(row[9]);
                    clothing.intLocationID = Convert.ToInt32(row[15]);
                    clothing.intItemTypeID = itemTypeID;
                    clothing.varSize = row[5].ToString();
                    clothing.varColour = row[4].ToString();
                    clothing.varGender = row[10].ToString();
                    clothing.varStyle = row[3].ToString();
                    clothing.varAdditionalInformation = row[16].ToString();
                    //stores clothing as an object
                    o = clothing as Object;
                }
                IDU.AddNewItemToDatabase(o, objPageDetails);
            }
        }
    }
}