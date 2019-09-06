using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotDiscountGolfPOS;
using System.Data;
using System.Linq;
using System.Configuration;

namespace SweetSpotProShop
{
    //This class is used for way too much...
    public class ItemDataUtilities
    {
        private string connectionString;
        //LocationManager lm = new LocationManager();
        //Connection String
        public ItemDataUtilities()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        DatabaseCalls dbc = new DatabaseCalls();
        //private List<Accessories> ConvertFromDataTableToAccessories(DataTable dt)
        //{
        //    List<Accessories> accessories = dt.AsEnumerable().Select(row =>
        //    new Accessories
        //    {
        //        sku = row.Field<int>("sku"),
        //        size = row.Field<string>("size"),
        //        colour = row.Field<string>("colour"),
        //        price = row.Field<double>("price"),
        //        cost = row.Field<double>("cost"),
        //        brandID = row.Field<int>("brandID"),
        //        modelID = row.Field<int>("modelID"),
        //        accessoryType = row.Field<string>("accessoryType"),
        //        quantity = row.Field<int>("quantity"),
        //        typeID = row.Field<int>("typeID"),
        //        locID = row.Field<int>("locationID"),
        //        comments = row.Field<string>("comments")
        //    }).ToList();
        //    return accessories;
        //}
        private List<Accessories> ConvertFromDataTableToAccessoriesForInventoryAddNew(DataTable dt, DateTime currentDate, int provinceID)
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
                a.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(currentDate, provinceID, a.intInventoryID);
            }
            return accessories;
        }
        //private List<Clothing> ConvertFromDataTableToClothing(DataTable dt)
        //{
        //    List<Clothing> clothing = dt.AsEnumerable().Select(row =>
        //    new Clothing
        //    {
        //        sku = row.Field<int>("sku"),
        //        size = row.Field<string>("size"),
        //        colour = row.Field<string>("colour"),
        //        gender = row.Field<string>("gender"),
        //        style = row.Field<string>("style"),
        //        price = row.Field<double>("price"),
        //        cost = row.Field<double>("cost"),
        //        brandID = row.Field<int>("brandID"),
        //        quantity = row.Field<int>("quantity"),
        //        typeID = row.Field<int>("typeID"),
        //        locID = row.Field<int>("locationID"),
        //        comments = row.Field<string>("comments")
        //    }).ToList();
        //    return clothing;
        //}
        private List<Clothing> ConvertFromDataTableToClothingForInventoryAddNew(DataTable dt, DateTime currentDate, int provinceID)
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
                c.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(currentDate, provinceID, c.intInventoryID);
            }
            return clothing;
        }
        //private List<Clubs> ConvertFromDataTableToClubs(DataTable dt)
        //{
        //    List<Clubs> clubs = dt.AsEnumerable().Select(row =>
        //    new Clubs
        //    {
        //        sku = row.Field<int>("sku"),
        //        brandID = row.Field<int>("brandID"),
        //        modelID = row.Field<int>("modelID"),
        //        clubType = row.Field<string>("clubType"),
        //        shaft = row.Field<string>("shaft"),
        //        numberOfClubs = row.Field<string>("numberOfClubs"),
        //        premium = row.Field<double>("premium"),
        //        cost = row.Field<double>("cost"),
        //        price = row.Field<double>("price"),
        //        quantity = row.Field<int>("quantity"),
        //        clubSpec = row.Field<string>("clubSpec"),
        //        shaftSpec = row.Field<string>("shaftSpec"),
        //        shaftFlex = row.Field<string>("shaftFlex"),
        //        dexterity = row.Field<string>("dexterity"),
        //        typeID = row.Field<int>("typeID"),
        //        itemlocation = row.Field<int>("locationID"),
        //        isTradeIn = row.Field<bool>("isTradeIn"),
        //        comments = row.Field<string>("comments")
        //    }).ToList();
        //    return clubs;
        //}
        private List<Clubs> ConvertFromDataTableToClubsForInventoryAddNew(DataTable dt, DateTime currentDate, int provinceID)
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
                c.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(currentDate, provinceID, c.intInventoryID);
            }
            return clubs;
        }
        //public List<object> ReturnListOfObjectsFromThreeTables(int sku, object[] objPageDetails)
        //{
        //    string strQueryName = "ReturnListOfObjectsFromThreeTables";
        //    string sqlCmd = "SELECT sku, brandID, modelID, clubType, shaft, numberOfClubs, "
        //        + "premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, "
        //        + "typeID, locationID, isTradeIn, comments FROM tbl_clubs WHERE sku = @sku";

        //    object[][] parms =
        //    {
        //         new object[] { "@sku", sku }
        //    };

        //    List<Clubs> c = ConvertFromDataTableToClubs(dbc.returnDataTableData(sqlCmd, parms));
        //    //List<Clubs> c = ConvertFromDataTableToClubs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

        //    sqlCmd = "SELECT sku, size, colour, gender, style, price, cost, brandID, quantity, "
        //        + "typeID, locationID, comments FROM tbl_clothing WHERE sku = @sku";

        //    List<Clothing> cl = ConvertFromDataTableToClothing(dbc.returnDataTableData(sqlCmd, parms));
        //    //List<Clothing> cl = ConvertFromDataTableToClothing(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

        //    sqlCmd = "SELECT sku, size, colour, price, cost, brandID, modelID, accessoryType, "
        //        + "quantity, typeID, locationID, comments FROM tbl_accessories WHERE sku = @sku";

        //    List<Accessories> a = ConvertFromDataTableToAccessories(dbc.returnDataTableData(sqlCmd, parms));
        //    //List<Accessories> a = ConvertFromDataTableToAccessories(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

        //    List<object> o = new List<object>();
        //    o.AddRange(a);
        //    o.AddRange(cl);
        //    o.AddRange(c);
        //    return o;
        //}

        public List<object> ReturnListOfObjectsFromThreeTablesForInventoryAddNew(int inventoryID, object[] objPageDetails, DateTime currentDate, int provinceID)
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

            List<Clubs> c = ConvertFromDataTableToClubsForInventoryAddNew(dbc.returnDataTableData(sqlCmd, parms), currentDate, provinceID);
            //List<Clubs> c = ConvertFromDataTableToClubs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            sqlCmd = "SELECT intInventoryID, varSku, varSize, varColour, varGender, varStyle, fltPrice, fltCost, intBrandID, intQuantity, "
                + "intItemTypeID, intLocationID, varAdditionalInformation FROM tbl_clothing WHERE intInventoryID = @intInventoryID";

            List<Clothing> cl = ConvertFromDataTableToClothingForInventoryAddNew(dbc.returnDataTableData(sqlCmd, parms), currentDate, provinceID);
            //List<Clothing> cl = ConvertFromDataTableToClothing(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            sqlCmd = "SELECT intInventoryID, varSku, varSize, varColour, fltPrice, fltCost, intBrandID, intModelID, varTypeOfAccessory, "
                + "intQuantity, intItemTypeID, intLocationID, varAdditionalInformation FROM tbl_accessories WHERE intInventoryID = @intInventoryID";

            List<Accessories> a = ConvertFromDataTableToAccessoriesForInventoryAddNew(dbc.returnDataTableData(sqlCmd, parms), currentDate, provinceID);
            //List<Accessories> a = ConvertFromDataTableToAccessories(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            List<object> o = new List<object>();
            o.AddRange(a);
            o.AddRange(cl);
            o.AddRange(c);
            return o;
        }
        private List<TaxTypePerInventoryItem> ReturnTaxTypePerInventoryItem(DateTime currentDate, int provinceID, int inventoryID)
        {
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

            return ConvertFromDataTableToTaxTypePerInventoryItem(dbc.returnDataTableData(sqlCmd, parms));
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


        //Return Model string created by Nathan and Tyler **getModelName
        public string ReturnModelNameFromModelID(int modelID, object[] objPageDetails)
        {
            string strQueryName = "ReturnModelNameFromModelID";
            string sqlCmd = "Select modelName from tbl_model where modelID = @modelID";

            object[][] parms =
            {
                 new object[] { "@modelID", modelID }
            };
            //Returns the model name
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Return Brand string created by Nathan and Tyler **getBrandName
        public string ReturnBrandNameFromBrandID(int brandID, object[] objPageDetails)
        {
            string strQueryName = "ReturnBrandNameFromBrandID";
            string sqlCmd = "SELECT brandName FROM tbl_brand WHERE brandID = @brandID";
            object[][] parms =
            {
                 new object[] { "@brandID", brandID }
            };
            //Returns the brand name
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Returns max sku from the skuNumber table based on itemType and directs code to store it
        public string[] ReturnMaxSku(int itemTypeID, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnMaxSku";
            string sqlCmd = "SELECT CONCAT(varStoreCode, CASE WHEN @intItemTypeID = 1 THEN varClubCode WHEN @intItemTypeID = 2 THEN varAccessoryCode "
                + "WHEN @intItemTypeID = 3 THEN varClothingCode END, CASE WHEN LEN(CAST(intSetInventoryNumber AS INT)) < 6 THEN RIGHT(RTRIM("
                + "'000000' + CAST(intSetInventroyNumber AS VARCHAR(6))),6) ELSE CAST(intSetInventoryNumber AS VARCHAR(MAX)) END) AS "
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

            string inventorySku = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            string inventoryID = dbc.MakeDataBaseCallToReturnString(sqlCmd2, parms2);
            string[] inventory = { inventorySku, inventoryID };
            StoreMaxSku(itemTypeID, locationID, objPageDetails);
            //Returns the new max sku
            return inventory;
        }
        //Stores the max sku in the skuNumber table
        public void StoreMaxSku(int itemTypeID, int locationID, object[] objPageDetails)
        {
            string strQueryName = "StoreMaxSku";
            //This method stores the max sku along with its item type
            string sqlCmd = "UPDATE tbl_storedStoreNumbers SET intSetInventoryNumber = intSetInventoryNumber + 1 WHERE intLocationID = @intLocationID";

            //if(itemTypeID == 1)
            //{
            //    sqlCmd += "UPDATE tbl_storedStoreNumbers SET intSetClubNumber = intSetClubNumber + 1 WHERE intLocationID = @intLocationID";

            //}
            //else if (itemTypeID == 2)
            //{
            //    sqlCmd += "UPDATE tbl_storedStoreNumbers SET intSetAccessoryNumber = intSetAccessoryNumber + 1 WHERE intLocationID = @intLocationID";

            //}
            //else if(itemTypeID == 3)
            //{
            //    sqlCmd += "UPDATE tbl_storedStoreNumbers SET intSetClothingNumber = intSetClothingNumber + 1 WHERE intLocationID = @intLocationID";

            //}

            object[][] parms =
            {
                 new object[] { "@intLocationID", locationID }
            };

            dbc.executeInsertQuery(sqlCmd, parms);


            string sqlCmd2 = "UPDATE tbl_storedStoreNumbers SET intInventoryIDTracking = intInventoryIDTracking + 1";
            object[][] parms2 = { };
            dbc.executeInsertQuery(sqlCmd2, parms2);
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
                inventoryID = AddClubToDatabase(club, objPageDetails);
            }
            else if (o is Accessories)
            {
                Accessories accessory = o as Accessories;
                inventoryID = AddAccessoryToDatabase(accessory, objPageDetails);
            }
            else if (o is Clothing)
            {
                Clothing clothing = o as Clothing;
                inventoryID = AddClothingToDatabase(clothing, objPageDetails);
            }
            //Returns the sku of the new item
            SetTaxesForNewInventory(inventoryID, true);
            return inventoryID;
        }
        public void SetTaxesForNewInventory(int inventoryID, bool chargeTax)
        {
            TaxManager TM = new TaxManager();
            DataTable lTax = TM.ReturnTaxList();
            foreach (DataRow TR in lTax.Rows)
            {
                SaveTaxIDForNewInventoryItem(inventoryID, Convert.ToInt32(TR[0]), chargeTax);
            }
        }
        private void SaveTaxIDForNewInventoryItem(int inventoryID, int taxID, bool chargeTax)
        {
            string sqlCmd = "INSERT INTO tbl_taxTypePerInventoryItem VALUES("
                + "@intInventoryID, @intTaxID, @bitChargeTax)";
            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intTaxID", taxID },
                new object[] { "@bitChargeTax", chargeTax }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }


        //These three actully add the item to specific tables Nathan created
        private int AddClubToDatabase(Clubs club, object[] objPageDetails)
        {
            string strQueryName = "AddClubToDatabase";
            string sqlCmd = "INSERT INTO tbl_clubs VALUES(@varSku, @intBrandID, @intModelID, @varTypeOfClub, @varShaftType, @varNumberOfClubs, "
                + "@fltPremiumCharge, @fltCost, @fltPrice, @intQuantity, @varClubSpecification, @varShaftSpecification, @varShaftFlexability, "
                + "@varClubDexterity, @intItemTypeID, @intLocationID, @bitIsUsedProduct, @varAdditionalInformation)";

            object[][] parms =
            {
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
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return ReturnClubIDFromClubStats(parms, objPageDetails);
        }
        private int AddAccessoryToDatabase(Accessories accessory, object[] objPageDetails)
        {
            string strQueryName = "AddAccessoryToDatabase";
            string sqlCmd = "INSERT INTO tbl_accessories VALUES(@varSku, @varSize, @varColour, @fltPrice, @fltCost, @intBrandID, @intModelID, "
                + "@varTypeOfAccessory, @intQuantity, @intItemTypeID, @intLocationID, @varAdditionalInformation)";

            object[][] parms =
            {
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
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return ReturnAccessoryIDFromAccessoryStats(parms, objPageDetails);
        }
        private int AddClothingToDatabase(Clothing clothing, object[] objPageDetails)
        {
            string strQueryName = "AddClothingToDatabase";
            string sqlCmd = "INSERT INTO tbl_clothing VALUES(@varSku, @varSize, @varColour, @varGender, @varStyle, @fltPrice, @fltCost, "
                + "@intBrandID, @intQuantity, @intItemTypeID, @intLocationID, @varAdditionalInformation)";

            object[][] parms =
            {
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
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return ReturnClothingIDFromClothingStats(parms, objPageDetails);
        }
        private int ReturnClubIDFromClubStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnClubIDFromClubStats";
            string sqlCmd = "SELECT intInventoryID FROM tbl_clubs WHERE varSku = @varSku AND intBrandID = @intBrandID AND intModelID = @intModelID "
                + "AND varTypeOfClub = @varTypeOfClub AND varShaftType = @varShaftType AND varNumberOfClubs = @varNumberOfClubs AND fltPremiumCharge "
                + "= @fltPremiumCharge AND fltCost = @fltCost AND fltPrice = @fltPrice AND intQuantity = @intQuantity AND varCkubSpecification = "
                + "@varClubSpecification AND varShaftSpecification = @varShaftSpecification AND varShaftFlexability = @ varShaftFlexability AND "
                + "varClubDexterity = @varClubDexterity AND intItemTypeID = @intItemTypeID AND intLocationID = @intLocationID AND bitIsUsedProduct = "
                + "@bitIsUsedProduct AND varAdditionalInformation = @varAdditionalInformation";

            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        private int ReturnAccessoryIDFromAccessoryStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnAccessoryIDFromAccessoryStats";
            string sqlCmd = "SELECT intInventoryID FROM tbl_accessories WHERE varSku = @varSku AND varSize = @varSize AND varColour = @varColour AND "
                + "fltCost = @fltCost AND fltPrice = @fltPrice AND intBrandID = @intBrandID AND intModelID = @intModelID AND varTypeOfAccessory = "
                + "@varTypeOfAccessory AND intQuantity = @intQuantity AND intItemTypeID = @intItemTypeID AND intLocationID = @intLocationID AND "
                + "varAdditionalInformation = @varAdditionalInformation";

            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        private int ReturnClothingIDFromClothingStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnClothingIDFromClothingStats";
            string sqlCmd = "SELECT intInventoryID FROM tbl_clothing WHERE varSku = @varSku AND varSize = @varSize AND varColour = @varColour AND "
                + "varGender = @varGender AND varStyle = @varStyle AND fltCost = @fltCost AND fltPrice = @fltPrice AND intBrandID = @intBrandID "
                + "AND intQuantity = @intQuantity AND intItemTypeID = @intItemTypeID AND intLocationID = @intLocationID AND "
                + "varAdditionalInformation = @varAdditionalInformation";

            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
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
            dbc.executeInsertQuery(sqlCmd, parms);
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
            dbc.executeInsertQuery(sqlCmd, parms);
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
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public void SaveInventoryChanges(ItemChangeTracking changeItem, object[] extra)
        {
            StoreNewAndOldInventoryChanges(changeItem, extra);
        }

        private void StoreNewAndOldInventoryChanges(ItemChangeTracking changeItem, object[] extra)
        {
            string sqlCmd = "INSERT INTO tbl_itemChangeTracking VALUES(@dtmChangeDate, @dtmChangeTime, @intEmployeeID, @intLocationID, "
                + "@intInventoryID, @originalCost, @newCost, @originalPrice, @newPrice, @originalQuantity, "
                + "@newQuantity, @originalDescription, @newDescription)";
            object[][] parms =
            {
                new object[] { "@dtmChangeDate", DateTime.Now },
                new object[] { "@dtmChangeTime", DateTime.Now },
                new object[] { "@intEmployeeID", extra[0] },
                new object[] { "@intLocationID", extra[1] },
                new object[] { "@intSku", changeItem.intInventoryID },
                new object[] { "@originalCost", changeItem.fltOriginalCost },
                new object[] { "@newCost", changeItem.fltNewCost },
                new object[] { "@originalPrice", changeItem.fltOriginalPrice },
                new object[] { "@newPrice", changeItem.fltNewPrice },
                new object[] { "@originalQuantity", changeItem.intOriginalQuantity },
                new object[] { "@newQuantity", changeItem.intNewQuantity },
                new object[] { "@originalDescription", changeItem.varOriginalDescription },
                new object[] { "@newDescription", changeItem.varNewDescription }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        //**OLD CODE**
        //**Used in Reports.importItems
        public int modelName(string modelN)
        {
            int model = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select modelID from tbl_model where modelName = @modelName";
            cmd.Parameters.AddWithValue("modelName", modelN);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int m = Convert.ToInt32(reader["modelID"]);
                model = m;
            }
            conn.Close();

            if (model == 0)
            {
                model = insertModel(modelN);
            }
            //Returns the modelID 
            return model;
        }
        //**Used in Reports.importItems
        public int brandName(string brandN)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select brandID from tbl_brand where brandName = '" + brandN + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int brand = 0;

            while (reader.Read())
            {
                int b = Convert.ToInt32(reader["brandID"]);
                brand = b;
            }
            conn.Close();
            if (brand == 0)
            {
                brand = insertBrand(brandN);
            }
            //Returns the brandID
            return brand;
        }
        //**Used in ItemDataUtilities.brandName
        public int insertBrand(string brandName)
        {
            int brandID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO tbl_brand (brandName) OUTPUT Inserted.brandID VALUES(@brandName); ";
            cmd.Parameters.AddWithValue("brandName", brandName);
            conn.Open();
            brandID = (int)cmd.ExecuteScalar();
            conn.Close();
            //Returns the brandID of the newly added brand
            return brandID;
        }
        //**Used in ItemDataUtilities.modelName
        public int insertModel(string modelName)
        {
            int modelID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO tbl_model (modelName) OUTPUT Inserted.modelID VALUES(@modelName); ";
            cmd.Parameters.AddWithValue("modelName", modelName);
            conn.Open();
            modelID = (int)cmd.ExecuteScalar();
            conn.Close();
            //Returns the modelID of the newly added model
            return modelID;
        }
        //**Used in SweetShopManager.transferTradeInStart
        public int[] tradeInSkuRange(int location)
        {
            int[] range = new int[2];
            int upper = 0;
            int lower = 0;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select skuStartAt, skuStopAt from tbl_tradeInSkusForCart where locationID = " + location.ToString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                upper = Convert.ToInt32(reader["skuStopAt"].ToString());
                lower = Convert.ToInt32(reader["skuStartAt"].ToString());
            }
            //Setting the values in the array
            range[0] = lower;
            range[1] = upper;


            conn.Close();
            //Returns the range
            return range;
        }
        //**Used in SweetShopManager.getSingleReceipt
        public string returnMOPIntasName(int mopN)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select methodDesc from tbl_methodOfPayment where methodID = @mopN";
            cmd.Parameters.AddWithValue("mopN", mopN);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string mop = "";

            while (reader.Read())
            {
                mop = Convert.ToString(reader["methodDesc"]);
            }
            conn.Close();
            //Returns the methodID
            return mop;
        }
    }
}
