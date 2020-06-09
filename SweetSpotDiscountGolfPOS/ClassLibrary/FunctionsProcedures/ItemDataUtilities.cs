using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Configuration;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS.FP
{
    //This class is used for way too much...
    public class ItemDataUtilities
    {
        DatabaseCalls DBC = new DatabaseCalls();
        public ItemDataUtilities() { }
        
        //Converters
        private List<Accessories> ConvertFromDataTableToAccessoriesForInventoryAddNew(DataTable dt, DateTime currentDate, int provinceID, object[] objPageDetails)
        {
            List<Accessories> accessories = dt.AsEnumerable().Select(row =>
            new Accessories
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                varSize = row.Field<string>("varSize"),
                varColour = row.Field<string>("varColour"),
                fltPrice = row.Field<double>("fltPrice"),
                fltCost = row.Field<double>("fltCost"),
                intBrandID = row.Field<int>("intBrandID"),
                intModelID = row.Field<int>("intModelID"),
                varTypeOfAccessory = row.Field<string>("varTypeOfAccessory"),
                intQuantity = row.Field<int>("intQuantity"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                intLocationID = row.Field<int>("intLocationID"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            foreach (Accessories a in accessories)
            {
                a.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(currentDate, provinceID, a.intInventoryID, objPageDetails);
            }
            return accessories;
        }
        private List<Clothing> ConvertFromDataTableToClothingForInventoryAddNew(DataTable dt, DateTime currentDate, int provinceID, object[] objPageDetails)
        {
            List<Clothing> clothing = dt.AsEnumerable().Select(row =>
            new Clothing
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                varSize = row.Field<string>("varSize"),
                varColour = row.Field<string>("varColour"),
                varGender = row.Field<string>("varGender"),
                varStyle = row.Field<string>("varStyle"),
                fltPrice = row.Field<double>("fltPrice"),
                fltCost = row.Field<double>("fltCost"),
                intBrandID = row.Field<int>("intBrandID"),
                intQuantity = row.Field<int>("intQuantity"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                intLocationID = row.Field<int>("intLocationID"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            foreach (Clothing c in clothing)
            {
                c.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(currentDate, provinceID, c.intInventoryID, objPageDetails);
            }
            return clothing;
        }
        private List<Clubs> ConvertFromDataTableToClubsForInventoryAddNew(DataTable dt, DateTime currentDate, int provinceID, object[] objPageDetails)
        {
            List<Clubs> clubs = dt.AsEnumerable().Select(row =>
            new Clubs
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                intBrandID = row.Field<int>("intBrandID"),
                intModelID = row.Field<int>("intModelID"),
                varTypeOfClub = row.Field<string>("varTypeOfClub"),
                varShaftType = row.Field<string>("varShaftType"),
                varNumberOfClubs = row.Field<string>("varNumberOfClubs"),
                fltPremiumCharge = row.Field<double>("fltPremiumCharge"),
                fltCost = row.Field<double>("fltCost"),
                fltPrice = row.Field<double>("fltPrice"),
                intQuantity = row.Field<int>("intQuantity"),
                varClubSpecification = row.Field<string>("varClubSpecification"),
                varShaftSpecification = row.Field<string>("varShaftSpecification"),
                varShaftFlexability = row.Field<string>("varShaftFlexability"),
                varClubDexterity = row.Field<string>("varClubDexterity"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                intLocationID = row.Field<int>("intLocationID"),
                bitIsUsedProduct = row.Field<bool>("bitIsUsedProduct"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            foreach (Clubs c in clubs)
            {
                c.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(currentDate, provinceID, c.intInventoryID, objPageDetails);
            }
            return clubs;
        }
        private List<TaxTypePerInventoryItem> ConvertFromDataTableToTaxTypePerInventoryItem(DataTable dt)
        {
            List<TaxTypePerInventoryItem> taxTypePerInventoryItem = dt.AsEnumerable().Select(row =>
            new TaxTypePerInventoryItem
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                intTaxID = row.Field<int>("intTaxID"),
                varTaxName = row.Field<string>("varTaxName"),
                fltTaxRate = row.Field<double>("fltTaxRate"),
                bitChargeTax = row.Field<bool>("bitChargeTax")
            }).ToList();
            return taxTypePerInventoryItem;
        }


        //DB Calls
        private List<object> ReturnListOfObjectsFromThreeTablesForInventoryAddNew(int inventoryID, object[] objPageDetails, DateTime currentDate, int provinceID)
        {
            string strQueryName = "ReturnListOfObjectsFromThreeTables";
            string sqlCmd = "SELECT intInventoryID, varSku, intBrandID, intModelID, varTypeOfClub, varShaftType, varNumberOfClubs, "
                + "fltPremiumCharge, fltCost, fltPrice, intQuantity, varClubSpecification, varShaftSpecification, varShaftFlexability, "
                + "varClubDexterity, intItemTypeID, intLocationID, bitIsUsedProduct, varAdditionalInformation FROM tbl_clubs WHERE "
                + "intInventoryID = @intInventoryID";

            object[][] parms =
            {
                 new object[] { "@intInventoryID", inventoryID }
            };

            List<Clubs> c = ConvertFromDataTableToClubsForInventoryAddNew(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), currentDate, provinceID, objPageDetails);
            //List<Clubs> c = ConvertFromDataTableToClubs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            sqlCmd = "SELECT intInventoryID, varSku, varSize, varColour, varGender, varStyle, fltPrice, fltCost, intBrandID, intQuantity, "
                + "intItemTypeID, intLocationID, varAdditionalInformation FROM tbl_clothing WHERE intInventoryID = @intInventoryID";

            List<Clothing> cl = ConvertFromDataTableToClothingForInventoryAddNew(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), currentDate, provinceID, objPageDetails);
            //List<Clothing> cl = ConvertFromDataTableToClothing(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            sqlCmd = "SELECT intInventoryID, varSku, varSize, varColour, fltPrice, fltCost, intBrandID, intModelID, varTypeOfAccessory, "
                + "intQuantity, intItemTypeID, intLocationID, varAdditionalInformation FROM tbl_accessories WHERE intInventoryID = @intInventoryID";

            List<Accessories> a = ConvertFromDataTableToAccessoriesForInventoryAddNew(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), currentDate, provinceID, objPageDetails);
            //List<Accessories> a = ConvertFromDataTableToAccessories(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            List<object> o = new List<object>();
            o.AddRange(a);
            o.AddRange(cl);
            o.AddRange(c);
            return o;
        }
        private List<TaxTypePerInventoryItem> ReturnTaxTypePerInventoryItem(DateTime currentDate, int provinceID, int inventoryID, object[] objPageDetails)
        {
            string strQueryName = "RetuernTaxTypePerInventoryItem";
            string sqlCmd = "SELECT intInventoryID, TR.intTaxID, varTaxName, fltTaxRate, bitChargeTax FROM tbl_taxRate TR JOIN tbl_taxTypePerInventoryItem "
                + "TTPII ON TTPII.intTaxID = TR.intTaxID JOIN tbl_taxType TT ON TT.intTaxID = TTPII.intTaxID JOIN(SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD "
                + "FROM tbl_taxRate WHERE dtmTaxEffectiveDate <= @dtmTaxEffectiveDate AND intProvinceID = @intProvinceID GROUP BY intTaxID) TRBP ON TRBP.intTaxID = "
                + "TTPII.intTaxID WHERE TR.dtmTaxEffectiveDate = TRBP.MTD AND intInventoryID = @intInventoryID AND intProvinceID = @intProvinceID";
            object[][] parms =
            {
                new object[] { "@dtmTaxEffectiveDate", currentDate.ToShortDateString() },
                new object[] { "@intProvinceID", provinceID },
                new object[] { "@intInventoryID", inventoryID }
            };

            return ConvertFromDataTableToTaxTypePerInventoryItem(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        //Return Model string created by Nathan and Tyler **getModelName
        private string ReturnModelNameFromModelID(int modelID, object[] objPageDetails)
        {
            string strQueryName = "ReturnModelNameFromModelID";
            string sqlCmd = "Select modelName from tbl_model where modelID = @modelID";

            object[][] parms =
            {
                 new object[] { "@modelID", modelID }
            };
            //Returns the model name
            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Return Brand string created by Nathan and Tyler **getBrandName
        private string ReturnBrandNameFromBrandID(int brandID, object[] objPageDetails)
        {
            string strQueryName = "ReturnBrandNameFromBrandID";
            string sqlCmd = "SELECT brandName FROM tbl_brand WHERE brandID = @brandID";
            object[][] parms =
            {
                 new object[] { "@brandID", brandID }
            };
            //Returns the brand name
            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Returns max sku from the skuNumber table based on itemType and directs code to store it
        private string[] ReturnMaxSku(int itemTypeID, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnMaxSku";
            string sqlCmd = "SELECT CONCAT(varStoreCode, CASE WHEN @intItemTypeID = 1 THEN varClubCode WHEN @intItemTypeID = 2 THEN varAccessoryCode "
                + "WHEN @intItemTypeID = 3 THEN varClothingCode END, CASE WHEN LEN(CAST(intSetInventoryNumber AS INT)) < 6 THEN RIGHT(RTRIM("
                + "'000000' + CAST(intSetInventoryNumber AS VARCHAR(6))),6) ELSE CAST(intSetInventoryNumber AS VARCHAR(MAX)) END) AS "
                + "varInventorySKU FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                 new object[] { "@intItemTypeID", itemTypeID },
                 new object[] { "@intLocationID", locationID }
            };

            string sqlCmd2 = "SELECT intInventoryIDTracking FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";

            object[][] parms2 =
            {
                 new object[] { "@intLocationID", locationID }
            };

            string inventorySku = DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            string inventoryID = DBC.MakeDataBaseCallToReturnString(sqlCmd2, parms2, objPageDetails, strQueryName);
            string[] inventory = { inventorySku, inventoryID };
            StoreMaxSku(itemTypeID, locationID, objPageDetails);
            //Returns the new max sku
            return inventory;
        }
        //Stores the max sku in the skuNumber table
        private void StoreMaxSku(int itemTypeID, int locationID, object[] objPageDetails)
        {
            string strQueryName = "StoreMaxSku";
            //This method stores the max sku along with its item type
            string sqlCmd = "UPDATE tbl_storedStoreNumbers SET intSetInventoryNumber = intSetInventoryNumber + 1 WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                 new object[] { "@intLocationID", locationID }
            };

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            string sqlCmd2 = "UPDATE tbl_storedStoreNumbers SET intInventoryIDTracking = intInventoryIDTracking + 1";
            object[][] parms2 = { };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd2, parms2, objPageDetails, strQueryName);
        }
        private int ReturnInventoryIDFromSKU(string sku, int itemTypeID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInventoryIDFromSKU";
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            string sqlCmd = "SELECT intInventoryID FROM tbl_" + IIM.CallReturnTableNameFromTypeID(itemTypeID, objPageDetails) + " WHERE varSku = @varSku";
            object[][] parms =
            {
                new object[] { "@varSku", sku }
            };
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void SaveTaxIDForNewInventoryItem(int inventoryID, int taxID, bool chargeTax, object[] objPageDetails)
        {
            string strQueryName = "SaveTaxIDForNewInventoryItem";
            string sqlCmd = "INSERT INTO tbl_taxTypePerInventoryItem VALUES("
                + "@intInventoryID, @intTaxID, @bitChargeTax)";
            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intTaxID", taxID },
                new object[] { "@bitChargeTax", chargeTax }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //These three actully add the item to specific tables Nathan created
        private void AddClubToDatabase(Clubs club, object[] objPageDetails)
        {
            string strQueryName = "AddClubToDatabase";
            string sqlCmd = "INSERT INTO tbl_clubs VALUES(@intInventoryID, @varSku, @intBrandID, @intModelID, @varTypeOfClub, @varShaftType, @varNumberOfClubs, "
                + "@fltPremiumCharge, @fltCost, @fltPrice, @intQuantity, @varClubSpecification, @varShaftSpecification, @varShaftFlexability, "
                + "@varClubDexterity, @intItemTypeID, @intLocationID, @bitIsUsedProduct, @varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@intInventoryID", club.intInventoryID},
                new object[] { "@varSku", club.varSku },
                new object[] { "@intBrandID", club.intBrandID },
                new object[] { "@intModelID", club.intModelID },
                new object[] { "@varTypeOfClub", club.varTypeOfClub },
                new object[] { "@varShaftType", club.varShaftType },
                new object[] { "@varNumberOfClubs", club.varNumberOfClubs },
                new object[] { "@fltPremiumCharge", club.fltPremiumCharge },
                new object[] { "@fltCost", club.fltCost },
                new object[] { "@fltPrice", club.fltPrice },
                new object[] { "@intQuantity", club.intQuantity },
                new object[] { "@varClubSpecification", club.varClubSpecification },
                new object[] { "@varShaftSpecification", club.varShaftSpecification },
                new object[] { "@varShaftFlexability", club.varShaftFlexability },
                new object[] { "@varClubDexterity", club.varClubDexterity },
                new object[] { "@intItemTypeID", club.intItemTypeID },
                new object[] { "@intLocationID", club.intLocationID },
                new object[] { "@bitIsUsedProduct", club.bitIsUsedProduct },
                new object[] { "@varAdditionalInformation", club.varAdditionalInformation }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //return ReturnClubIDFromClubStats(parms, objPageDetails);
        }
        private void AddAccessoryToDatabase(Accessories accessory, object[] objPageDetails)
        {
            string strQueryName = "AddAccessoryToDatabase";
            string sqlCmd = "INSERT INTO tbl_accessories VALUES(@intInventoryID, @varSku, @varSize, @varColour, @fltPrice, @fltCost, @intBrandID, @intModelID, "
                + "@varTypeOfAccessory, @intQuantity, @intItemTypeID, @intLocationID, @varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@intInventoryID", accessory.intInventoryID },
                new object[] { "@varSku", accessory.varSku },
                new object[] { "@varSize", accessory.varSize },
                new object[] { "@varColour", accessory.varColour },
                new object[] { "@fltPrice", accessory.fltPrice },
                new object[] { "@fltCost", accessory.fltCost },
                new object[] { "@intBrandID", accessory.intBrandID },
                new object[] { "@intModelID", accessory.intModelID },
                new object[] { "@varTypeOfAccessory", accessory.varTypeOfAccessory },
                new object[] { "@intQuantity", accessory.intQuantity },
                new object[] { "@intItemTypeID", accessory.intItemTypeID },
                new object[] { "@intLocationID", accessory.intLocationID },
                new object[] { "@varAdditionalInformation", accessory.varAdditionalInformation }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //return ReturnAccessoryIDFromAccessoryStats(parms, objPageDetails);
        }
        private void AddClothingToDatabase(Clothing clothing, object[] objPageDetails)
        {
            string strQueryName = "AddClothingToDatabase";
            string sqlCmd = "INSERT INTO tbl_clothing VALUES(@intInventoryID, @varSku, @varSize, @varColour, @varGender, @varStyle, @fltPrice, @fltCost, "
                + "@intBrandID, @intQuantity, @intItemTypeID, @intLocationID, @varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@intInventoryID", clothing.intInventoryID },
                new object[] { "@varSku", clothing.varSku },
                new object[] { "@varSize", clothing.varSize },
                new object[] { "@varColour", clothing.varColour },
                new object[] { "@varGender", clothing.varGender },
                new object[] { "@varStyle", clothing.varStyle },
                new object[] { "@fltPrice", clothing.fltPrice },
                new object[] { "@fltCost", clothing.fltCost },
                new object[] { "@intBrandID", clothing.intBrandID },
                new object[] { "@intQuantity", clothing.intQuantity },
                new object[] { "@intItemTypeID", clothing.intItemTypeID },
                new object[] { "@intLocationID", clothing.intLocationID },
                new object[] { "@varAdditionalInformation", clothing.varAdditionalInformation }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //return ReturnClothingIDFromClothingStats(parms, objPageDetails);
        }
        private int ReturnClubIDFromClubStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnClubIDFromClubStats";
            string sqlCmd = "SELECT intInventoryID FROM tbl_clubs WHERE varSku = @varSku AND intBrandID = @intBrandID AND intModelID = @intModelID "
                + "AND varTypeOfClub = @varTypeOfClub AND varShaftType = @varShaftType AND varNumberOfClubs = @varNumberOfClubs AND fltPremiumCharge "
                + "= @fltPremiumCharge AND fltCost = @fltCost AND fltPrice = @fltPrice AND intQuantity = @intQuantity AND varClubSpecification = "
                + "@varClubSpecification AND varShaftSpecification = @varShaftSpecification AND varShaftFlexability = @ varShaftFlexability AND "
                + "varClubDexterity = @varClubDexterity AND intItemTypeID = @intItemTypeID AND intLocationID = @intLocationID AND bitIsUsedProduct = "
                + "@bitIsUsedProduct AND varAdditionalInformation = @varAdditionalInformation";

            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnAccessoryIDFromAccessoryStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnAccessoryIDFromAccessoryStats";
            string sqlCmd = "SELECT intInventoryID FROM tbl_accessories WHERE varSku = @varSku AND varSize = @varSize AND varColour = @varColour AND "
                + "fltCost = @fltCost AND fltPrice = @fltPrice AND intBrandID = @intBrandID AND intModelID = @intModelID AND varTypeOfAccessory = "
                + "@varTypeOfAccessory AND intQuantity = @intQuantity AND intItemTypeID = @intItemTypeID AND intLocationID = @intLocationID AND "
                + "varAdditionalInformation = @varAdditionalInformation";

            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnClothingIDFromClothingStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnClothingIDFromClothingStats";
            string sqlCmd = "SELECT intInventoryID FROM tbl_clothing WHERE varSku = @varSku AND varSize = @varSize AND varColour = @varColour AND "
                + "varGender = @varGender AND varStyle = @varStyle AND fltCost = @fltCost AND fltPrice = @fltPrice AND intBrandID = @intBrandID "
                + "AND intQuantity = @intQuantity AND intItemTypeID = @intItemTypeID AND intLocationID = @intLocationID AND "
                + "varAdditionalInformation = @varAdditionalInformation";

            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //These three actully update the item in their specific tables Nathan created
        private void UpdateClubInDatabase(Clubs club, object[] objPageDetails)
        {
            string strQueryName = "UpdateClubInDatabase";
            string sqlCmd = "UPDATE tbl_clubs SET intBrandID = @intBrandID, intModelID = @intModelID, varTypeOfClub = @varTypeOfClub, "
                + "varShaftType = @varShaftType, varNumberOfClubs = @varNumberOfClubs, fltPremiumCharge = @fltPremiumCharge, fltCost "
                + "= @fltCost, fltPrice = @fltPrice, intQuantity = @intQuantity, varClubSpecification = @varClubSpecification, "
                + "varShaftSpecification = @varShaftSpecification, varShaftFlexability = @varShaftFlexability, varClubDexterity = "
                + "@varClubDexterity, intLocationID = @intLocationID, bitIsUsedProduct = @bitIsUsedProduct, varAdditionalInformation "
                + "= @varAdditionalInformation WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                 new object[] { "@intInventoryID", club.intInventoryID },
                 new object[] { "@intBrandID", club.intBrandID },
                 new object[] { "@intModelID", club.intModelID },
                 new object[] { "@varTypeOfClub", club.varTypeOfClub },
                 new object[] { "@varShaftType", club.varShaftType },
                 new object[] { "@varNumberOfClubs", club.varNumberOfClubs },
                 new object[] { "@fltPremiumCharge", club.fltPremiumCharge },
                 new object[] { "@fltCost", club.fltCost },
                 new object[] { "@fltPrice", club.fltPrice },
                 new object[] { "@intQuantity", club.intQuantity },
                 new object[] { "@varClubSpecification", club.varClubSpecification },
                 new object[] { "@varShaftSpecification", club.varShaftSpecification },
                 new object[] { "@varShaftFlexability", club.varShaftFlexability },
                 new object[] { "@varClubDexterity", club.varClubDexterity },
                 new object[] { "@intLocationID", club.intLocationID },
                 new object[] { "@bitIsUsedProduct", club.bitIsUsedProduct },
                 new object[] { "@varAdditionalInformation", club.varAdditionalInformation }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateAccessoryInDatabase(Accessories accessory, object[] objPageDetails)
        {
            string strQueryName = "UpdateAccessoryInDatabase";
            string sqlCmd = "UPDATE tbl_accessories SET varSize = @varSize, varColour = @varColour, fltPrice = @fltPrice, fltCost = @fltCost, "
                + "intBrandID = @intBrandID, intModelID = @intModelID, varTypeOfAccessory = @varTypeOfAccessory, intQuantity = @intQuantity, "
                + "intLocationID = @intLocationID, varAdditionalInformation = @varAdditionalInformation WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                 new object[] { "@intInventoryID", accessory.intInventoryID },
                 new object[] { "@varSize", accessory.varSize },
                 new object[] { "@varColour", accessory.varColour },
                 new object[] { "@fltPrice", accessory.fltPrice },
                 new object[] { "@fltCost", accessory.fltCost },
                 new object[] { "@intBrandID", accessory.intBrandID },
                 new object[] { "@intModelID", accessory.intModelID },
                 new object[] { "@varTypeOfAccessory", accessory.varTypeOfAccessory },
                 new object[] { "@intQuantity", accessory.intQuantity },
                 new object[] { "@intLocationID", accessory.intLocationID },
                 new object[] { "@varAdditionalInformation", accessory.varAdditionalInformation }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateClothingInDatabase(Clothing clothing, object[] objPageDetails)
        {
            string strQueryName = "UpdateClothingInDatabase";
            string sqlCmd = "UPDATE tbl_clothing SET varSize = @varSize, varColour = @varColour, varGender = @varGender, varStyle = @varStyle, "
                + "fltPrice = @fltPrice, fltCost = @fltCost, intBrandID = @intBrandID, intQuantity = @intQuantity, intLocationID = @intLocationID, "
                + "varAdditionalInformation = @varAdditionalInformation WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                 new object[] { "@intInventoryID", clothing.intInventoryID },
                 new object[] { "@varSize", clothing.varSize },
                 new object[] { "@varColour", clothing.varColour },
                 new object[] { "@varGender", clothing.varGender },
                 new object[] { "@varStyle", clothing.varStyle },
                 new object[] { "@fltPrice", clothing.fltPrice },
                 new object[] { "@fltCost", clothing.fltCost },
                 new object[] { "@intBrandID", clothing.intBrandID },
                 new object[] { "@intQuantity", clothing.intQuantity },
                 new object[] { "@intLocationID", clothing.intLocationID },
                 new object[] { "@varAdditionalInformation", clothing.varAdditionalInformation }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void StoreNewAndOldInventoryChanges(ItemChangeTracking changeItem, object[] extra, object[] objPageDetails)
        {
            string strQueryName = "StoreNewAndOldInventoryChanges";
            string sqlCmd = "INSERT INTO tbl_itemChangeTracking VALUES(@dtmChangeDate, @dtmChangeTime, @intEmployeeID, @intLocationID, "
                + "@intInventoryID, @fltOriginalCost, @fltNewCost, @fltOriginalPrice, @fltNewPrice, @intOriginalQuantity, "
                + "@intNewQuantity, @varOriginalDescription, @varNewDescription)";
            object[][] parms =
            {
                new object[] { "@dtmChangeDate", DateTime.Now },
                new object[] { "@dtmChangeTime", DateTime.Now },
                new object[] { "@intEmployeeID", extra[0] },
                new object[] { "@intLocationID", extra[1] },
                new object[] { "@intInventoryID", changeItem.intInventoryID },
                new object[] { "@fltOriginalCost", changeItem.fltOriginalCost },
                new object[] { "@fltNewCost", changeItem.fltNewCost },
                new object[] { "@fltOriginalPrice", changeItem.fltOriginalPrice },
                new object[] { "@fltNewPrice", changeItem.fltNewPrice },
                new object[] { "@intOriginalQuantity", changeItem.intOriginalQuantity },
                new object[] { "@intNewQuantity", changeItem.intNewQuantity },
                new object[] { "@varOriginalDescription", changeItem.varOriginalDescription },
                new object[] { "@varNewDescription", changeItem.varNewDescription }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }



        //Public calls
        public int CheckIfSkuAlreadyInDatabase(string sku, int itemTypeID, object[] objPageDetails)
        {
            return ReturnInventoryIDFromSKU(sku, itemTypeID, objPageDetails);
        }
        //**Add Item**
        //Adds new Item to tables Nathan created
        public int AddNewItemToDatabase(object o, object[] objPageDetails)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            int inventoryID = -10;
            if (o is Clubs)
            {
                Clubs club = o as Clubs;
                AddClubToDatabase(club, objPageDetails);
                inventoryID = club.intInventoryID;
            }
            else if (o is Accessories)
            {
                Accessories accessory = o as Accessories;
                AddAccessoryToDatabase(accessory, objPageDetails);
                inventoryID = accessory.intInventoryID;
            }
            else if (o is Clothing)
            {
                Clothing clothing = o as Clothing;
                AddClothingToDatabase(clothing, objPageDetails);
                inventoryID = clothing.intInventoryID;
            }
            //Returns the sku of the new item
            SetTaxesForNewInventory(inventoryID, objPageDetails);
            return inventoryID;
        }
        public void SetTaxesForNewInventory(int inventoryID, object[] objPageDetails)
        {
            TaxManager TM = new TaxManager();
            DataTable lTax = TM.ReturnTaxList(objPageDetails);
            foreach (DataRow TR in lTax.Rows)
            {
                bool chargeTax = true;
                if (TM.CallCheckForLiquorTax(Convert.ToInt32(TR[0]), objPageDetails))
                {
                    chargeTax = false;
                }
                if (TM.CallCheckForQuebecSalesTax(Convert.ToInt32(TR[0]), objPageDetails))
                {
                    chargeTax = false;
                }
                if (TM.CallCheckForRetailSalesTax(Convert.ToInt32(TR[0]), objPageDetails))
                {
                    chargeTax = false;
                }
                SaveTaxIDForNewInventoryItem(inventoryID, Convert.ToInt32(TR[0]), chargeTax, objPageDetails);
            }
        }
        //**Update Item**
        public int UpdateItemInDatabase(object o, object[] objPageDetails)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            int inventoryID = -10;
            if (o is Clubs)
            {
                Clubs club = o as Clubs;
                UpdateClubInDatabase(club, objPageDetails);
                inventoryID = club.intInventoryID;
            }
            else if (o is Accessories)
            {
                Accessories accessory = o as Accessories;
                UpdateAccessoryInDatabase(accessory, objPageDetails);
                inventoryID = accessory.intInventoryID;
            }
            else if (o is Clothing)
            {
                Clothing clothing = o as Clothing;
                UpdateClothingInDatabase(clothing, objPageDetails);
                inventoryID = clothing.intInventoryID;
            }
            //Returns the sku of the new item
            return inventoryID;
        }
        public void SaveInventoryChanges(ItemChangeTracking changeItem, object[] extra, object[] objPageDetails)
        {
            StoreNewAndOldInventoryChanges(changeItem, extra, objPageDetails);
        }
        public void CallSaveTaxIDForNewInventoryItem(int inventoryID, int taxID, bool chargeTax, object[] objPageDetails)
        {
            SaveTaxIDForNewInventoryItem(inventoryID, taxID, chargeTax, objPageDetails);
        }
    }
}
