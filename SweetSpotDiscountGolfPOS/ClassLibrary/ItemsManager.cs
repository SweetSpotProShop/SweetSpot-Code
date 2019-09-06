using SweetShop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class ItemsManager
    {
        DatabaseCalls dbc = new DatabaseCalls();

        //Database connections
        public List<InvoiceItems> ConvertFromDataTableToCartItems(DataTable dt)
        {
            List<InvoiceItems> invoiceItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                varItemDescription = row.Field<string>("varItemDescription"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemCost = row.Field<double>("fltItemCost"),
                //intLocationID = row.Field<int>("intLocationID"),
                //varLocationName = row.Field<string>("varLocationName"),
                intItemTypeID = row.Field<int>("intItemTypeID"),                
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            return invoiceItems;
        }
        public List<InvoiceItems> ConvertFromDataTableToInventoryItem(DataTable dt)
        {
            List<InvoiceItems> invoiceItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                varItemDescription = row.Field<string>("varItemDescription"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemCost = row.Field<double>("fltItemCost"),
                intLocationID = row.Field<int>("intLocationID"),
                varLocationName = row.Field<string>("varLocationName"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            return invoiceItems;
        }
        private int ConvertFromDataTableToInt(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ConvertFromDataTableToString(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //Search results
        private string ReturnItemsFromSearchString(string searchTxt, int quantity)
        {
            ArrayList strText = new ArrayList();
            for (int i = 0; i < searchTxt.Split(' ').Length; i++)
            {
                strText.Add(searchTxt.Split(' ')[i]);
            }
            string sqlCmd = ReturnStringSearchForAccessories(strText, quantity);
            sqlCmd += " UNION ";
            sqlCmd += ReturnStringSearchForClothing(strText, quantity);
            sqlCmd += " UNION ";
            sqlCmd += ReturnStringSearchForClubs(strText, quantity);
            sqlCmd += " ORDER BY varSku DESC";
            return sqlCmd;
        }
        //Returns string for search accessories
        private string ReturnStringSearchForAccessories(ArrayList array, int quantity)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT A.intInventoryID, A.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + AC.varSize + ' ' + AC.varColour "
                        + "AS varItemDescription FROM tbl_accessories AC JOIN tbl_brand B ON AC.intBrandID = B.intBrandID JOIN tbl_model M ON "
                        + "AC.intModelID = M.intModelID WHERE AC.intInventoryID = A.intInventoryID) AS varItemDescription, A.intLocationID, (SELECT L.varCityName FROM "
                        + "tbl_accessories AC JOIN tbl_location L ON AC.intLocationID = L.intLocationID WHERE AC.intInventoryID = A.intInventoryID) AS "
                        + "varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltItemPrice, A.fltCost AS fltItemCost, A.intItemTypeID, CAST(0 AS bit) AS "
                        + "bitIsUsedProduct, A.varAdditionalInformation FROM tbl_accessories A WHERE ((varSku LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID "
                        + "FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE "
                        + "varModelName LIKE '%" + array[i] + "%') OR CONCAT(varSize, varColour, varTypeOfAccessory, varAdditionalInformation) LIKE '%" 
                        + array[i] + "%')) AND A.intQuantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT A.intInventoryID, A.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + AC.varSize + ' ' + "
                        + "AC.varColour AS varItemDescription FROM tbl_accessories AC JOIN tbl_brand B ON AC.intBrandID = B.intBrandID JOIN tbl_model "
                        + "M ON AC.intModelID = M.intModelID WHERE AC.intInventoryID = A.intInventoryID) AS varItemDescription, A.intLocationID, (SELECT L.varCityName "
                        + "FROM tbl_accessories AC JOIN tbl_location L ON AC.intLocationID = L.intLocationID WHERE AC.intInventoryID = A.intInventoryID) "
                        + "AS varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltItemPrice, A.fltCost AS fltItemCost, A.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, "
                        + "A.varAdditionalInformation FROM tbl_accessories A WHERE ((varSku LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID "
                        + "FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE "
                        + "varModelName LIKE '%" + array[i] + "%') OR CONCAT(varSize, varColour, varTypeOfAccessory, varAdditionalInformation) LIKE '%" 
                        + array[i] + "%')) AND A.intQuantity > " + quantity + ") ";
                }
            }
            sqlCmd += ") AS tblResult";
            return sqlCmd;
        }
        //Returns string for search clothing
        private string ReturnStringSearchForClothing(ArrayList array, int quantity)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT CL.intInventoryID, CL.varSku, (SELECT B.varBrandName + ' ' + CLO.varSize + ' ' + CLO.varColour + ' ' + CLO.varGender + "
                        + "' ' + CLO.varStyle AS varItemDescription FROM tbl_clothing CLO JOIN tbl_brand B ON CLO.intBrandID = B.intBrandID WHERE "
                        + "CLO.intInventoryID = CL.intInventoryID) AS varItemDescription, CL.intLocationID, (SELECT L.varCityName FROM tbl_clothing CLO JOIN tbl_location L ON "
                        + "CLO.intLocationID = L.intLocationID WHERE CLO.intInventoryID = CL.intInventoryID) AS varLocationName, CL.intQuantity AS intItemQuantity, CL.fltPrice AS fltItemPrice, "
                        + "CL.fltCost AS fltItemCost, CL.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, CL.varAdditionalInformation FROM tbl_clothing CL WHERE ((varSku "
                        + "LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%') OR "
                        + "CONCAT(varSize, varColour, varGender, varStyle, varAdditionalInformation) LIKE '%" + array[i] + "%')) AND CL.intQuantity > " 
                        + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT CL.intInventoryID, CL.varSku, (SELECT B.varBrandName + ' ' + CLO.varSize + ' ' + CLO.varColour + ' ' + "
                        + "CLO.varGender + ' ' + CLO.varStyle AS varItemDescription FROM tbl_clothing CLO JOIN tbl_brand B ON CLO.intBrandID = B.intBrandID "
                        + "WHERE CLO.intInventoryID = CL.intInventoryID) AS varItemDescription, CL.intLocationID, (SELECT L.varCityName FROM tbl_clothing CLO JOIN tbl_location "
                        + "L ON CLO.intLocationID = L.intLocationID WHERE CLO.intInventoryID = CL.intInventoryID) AS varLocationName, CL.intQuantity AS intItemQuantity, "
                        + "CL.fltPrice AS fltItemPrice, CL.fltCost AS fltItemCost, CL.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, CL.varAdditionalInformation FROM tbl_clothing CL "
                        + "WHERE ((varSku LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%"
                        + array[i] + "%') OR CONCAT(varSize, varColour, varGender, varStyle, varAdditionalInformation) LIKE '%" + array[i] + "%')) AND "
                        + "CL.intQuantity > " + quantity + ") ";
                }
            }
            sqlCmd += ") AS tblResults";
            return sqlCmd;
        }
        //Returns string for search clubs
        private string ReturnStringSearchForClubs(ArrayList array, int quantity)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT C.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + CLU.varClubSpecification + ' ' + "
                        + "CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + CLU.varClubDexterity AS "
                        + "varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID JOIN tbl_model M ON CLU.intModelID = "
                        + "M.intModelID WHERE CLU.intInventoryID = C.intInventoryID) AS varItemDescription, C.intLocationID, (SELECT L.varCityName FROM tbl_clubs CLU "
                        + "JOIN tbl_location L ON CLU.intLocationID = L.intLocationID WHERE CLU.intInventoryID = C.intInventoryID) AS varLocationName, "
                        + "C.intQuantity AS intItemQuantity, C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, C.intItemTypeID, C.bitIsUsedProduct, C.varAdditionalInformation FROM tbl_clubs C WHERE "
                        + "((varSku LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] 
                        + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%') OR "
                        + "CONCAT(varClubSpecification, varTypeOfClub, varShaftSpecification, varShaftFlexability, varClubDexterity) LIKE '%" + array[i] 
                        + "%')) AND C.intQuantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT C.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + CLU.varClubSpecification "
                        + "+ ' ' + CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + CLU.varClubDexterity AS "
                        + "varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID JOIN tbl_model M ON CLU.intModelID = "
                        + "M.intModelID WHERE CLU.intInventoryID = C.intInventoryID) AS varItemDescription, C.intLocationID, (SELECT L.varCityName FROM tbl_clubs CLU JOIN "
                        + "tbl_location L ON CLU.intLocationID = L.intLocationID WHERE CLU.intInventoryID = C.intInventoryID) AS varLocationName, "
                        + "C.intQuantity AS intItemQuantity, C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, C.intItemTypeID, C.bitIsUsedProduct, C.varAdditionalInformation FROM tbl_clubs C WHERE "
                        + "((varSku LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] 
                        + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%') OR CONCAT(varClubSpecification, "
                        + "varTypeOfClub, varShaftSpecification, varShaftFlexability, varClubDexterity) LIKE '%" + array[i] + "%')) AND C.intQuantity > " 
                        + quantity + ") ";
                }
            }
            sqlCmd += ") AS tblResults";
            return sqlCmd;
        }

        //Returning the list of items
        public List<InvoiceItems> ReturnInventoryFromSearchStringAndQuantity(string searchText, bool zeroQuantity, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromSearchStringAndQuantity";
            int quantity = 0;
            if (zeroQuantity)
            {
                quantity = -1;
            }
            string sqlCmd = ReturnItemsFromSearchString(searchText, quantity);
            object[][] parms = { };
            return ConvertFromDataTableToInventoryItem(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<InvoiceItems> ReturnInvoiceItemsFromSearchStringForSale(string searchText, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromSearchStringForSale";
            string sqlCmd = ReturnItemsFromSearchString(searchText, -1);
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<InvoiceItems> ReturnTradeInSku(object[] objPageDetails)
        {
            string strQueryName = "ReturnTradeInSku";
            string sqlCmd = "SELECT C.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + CLU.varClubSpecification "
                + "+ ' ' + CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + CLU.varClubDexterity AS "
                + "varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID JOIN tbl_model M ON CLU.intModelID "
                + "= M.intModelID  WHERE CLU.intInventoryID = C.intInventoryID) AS varItemDescription, (SELECT L.varCityName FROM tbl_clubs "
                + "CLU JOIN tbl_location L ON CLU.intLocationID = L.intLocationID WHERE CLU.intInventoryID = C.intInventoryID) AS varLocationName, "
                + "C.intQuantity, C.fltPrice, C.fltCost, C.intItemTypeID, C.bitIsClubTradeIn, C.varAdditionalInformation FROM tbl_clubs C WHERE "
                + "C.varSku = '100000'";
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }

        //DropDownList insertion
        public DataTable ReturnDropDownForBrand(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForBrand";
            string sqlCmd = "SELECT intBrandID, varBrandName FROM tbl_brand ORDER BY varBrandName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnDropDownForModel(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForModel";
            string sqlCmd = "SELECT intModelID, varModelName FROM tbl_model ORDER BY varModelName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnDropDownForItemType(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForItemType";
            string sqlCmd = "SELECT intItemTypeID, varItemTypeName FROM tbl_itemType ORDER BY varItemTypeName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        
        //TradeIn Criteria
        public string ReserveTradeInSKU(CurrentUser cu, object[] objPageDetails)
        {
            string strQueryName = "ReserveTradeInSKU";
            string sqlCmd = "SELECT CONCAT(varStoreCode, varTradeInCode, CASE WHEN LEN(CAST(intSetTradeInNumber AS INT)) < 6 THEN RIGHT(RTRIM("
                + "'000000' + CAST(intSetTradeInNumber AS VARCHAR(6))),6) ELSE CAST(intSetTradeInNumber AS VARCHAR(MAX)) END) AS varTradeInSKU "
                + "FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };
            TradeInSKU(cu, objPageDetails);
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        private void TradeInSKU(CurrentUser cu, object[] objPageDetails)
        {
            string strQueryName = "TradeInSKU";
            string sqlCmd = "UPDATE tbl_storedStoreNumbers SET intSetTradeInNumber = intSetTradeInNumber + 1 WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        
        public void AddTradeInItemToTempTable(Clubs tradeIn, object[] objPageDetails)
        {
            string strQueryName = "AddTradeInItemToTempTable";
            string sqlCmd = "INSERT INTO tbl_tempTradeInCartSkus VALUES(@varSku, @intBrandID, @intModelID, @varTypeOfClub, @varShaftType, "
                + "@varNumberOfClubs, @fltPremiumCharge, @fltCostost, @fltPrice, @intQuantity, @varClubSpecification, @varShaftSpecification, "
                + "@varShaftFlexability, @varClubDexterity, @intItemTypeID, @intLocationID, @bitIsUsedProduct, @varAdditionalInformtaion)";

            object[][] parms =
            {
                new object[] { "@varSku", tradeIn.varSku },
                new object[] { "@intBrandID", tradeIn.intBrandID },
                new object[] { "@intModelID", tradeIn.intModelID },
                new object[] { "@varTypeOfClub", tradeIn.varTypeOfClub },
                new object[] { "@varShaftType", tradeIn.varShaftType },
                new object[] { "@varNumberOfClubs", tradeIn.varNumberOfClubs },
                new object[] { "@fltPremiumCharge", tradeIn.fltPremiumCharge },
                new object[] { "@fltCost", tradeIn.fltCost },
                new object[] { "@fltPrice", tradeIn.fltPrice },
                new object[] { "@intQuantity", tradeIn.intQuantity },
                new object[] { "@varClubSpecification", tradeIn.varClubSpecification },
                new object[] { "@varShaftSpecification", tradeIn.varShaftSpecification },
                new object[] { "@varShaftFlexability", tradeIn.varShaftFlexability },
                new object[] { "@varClubDexterity", tradeIn.varClubDexterity },
                new object[] { "@intItemTypeID", tradeIn.intItemTypeID },
                new object[] { "@intLocationID", tradeIn.intLocationID },
                new object[] { "@bitIsUsedProduct", tradeIn.bitIsUsedProduct },
                new object[] { "@varAdditionalInformation", tradeIn.varAdditionalInformation }
            };

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public string ReturnModelNameFromModelID(int modelID, object[] objPageDetails)
        {
            string strQueryName = "ReturnModelNameFromModelID";
            string sqlCmd = "SELECT modelName FROM tbl_model WHERE modelID = @modelID";
            object[][] parms =
            {
                new object[] { "@modelID", modelID }
            };
            return ConvertFromDataTableToString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnBrandlNameFromBrandID(int brandID, object[] objPageDetails)
        {
            string strQueryName = "ReturnBrandlNameFromBrandID";
            string sqlCmd = "SELECT brandName FROM tbl_brand WHERE brandID = @brandID";
            object[][] parms =
            {
                new object[] { "@brandID", brandID }
            };
            return ConvertFromDataTableToString(sqlCmd, parms, objPageDetails, strQueryName);
        }


        public void ProgramLoungeSimButton(int sku, string buttonName)
        {
            StoreSkuAndNumber(sku, buttonName);
        }
        private void StoreSkuAndNumber(int sku, string buttonName)
        {
            string sqlCmd = "INSERT INTO tbl_LoungeButtonItemCombination VALUES(@sku, @buttonName)";
            object[][] parms =
            {
                new object[] { "@sku", sku },
                new object[] { "@buttonName", buttonName }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        public List<InvoiceItems> ReturnProgrammedSaleableItems()
        {
            return CallAllProgrammedItems();
        }
        private List<InvoiceItems> CallAllProgrammedItems()
        {
            string sqlCmd = "SELECT LBIC.sku, A.accessoryType AS description, L.locationName, A.quantity, A.price, A.cost, A.typeID, CAST(0 "
                + "AS bit) AS isTradeIn, LBIC.loungeButton AS comments FROM tbl_LoungeButtonItemCombination LBIC LEFT JOIN tbl_accessories "
                + "A ON A.sku = LBIC.sku JOIN tbl_location L ON L.locationID = A.locationID UNION SELECT LBIC.sku, (SELECT B.brandName + "
                + "' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + ' ' + CLO.style AS description FROM tbl_clothing CLO JOIN "
                + "tbl_brand B ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS description, L.locationName, CL.quantity, CL.price, "
                + "CL.cost, CL.typeID, CAST(0 AS bit) AS isTradeIn, LBIC.loungeButton AS comments FROM tbl_LoungeButtonItemCombination LBIC "
                + "LEFT JOIN tbl_clothing CL ON CL.sku = LBIC.sku JOIN tbl_location L ON L.locationID = CL.locationID UNION SELECT "
                + "LBIC.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + CLU.clubType + ' ' + CLU.shaftSpec + ' ' "
                + "+ CLU.shaftFlex + ' ' + CLU.dexterity AS description FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN "
                + "tbl_model M ON CLU.modelID = M.modelID WHERE CLU.sku = C.sku) AS description, L.locationName, C.quantity, C.price, "
                + "C.cost, C.typeID, C.isTradeIn, LBIC.loungeButton AS comments FROM tbl_LoungeButtonItemCombination LBIC LEFT JOIN "
                + "tbl_clubs C ON C.sku = LBIC.sku JOIN tbl_location L ON L.locationID = C.locationID";
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms));
        }




        public List<InvoiceItems> ReturnInvoiceItemsFromForLoungeSim(string searchText, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromForLoungeSim";
            ArrayList strText = new ArrayList();
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add(searchText.Split(' ')[i]);
            }
            string sqlCmd = ReturnStringSearchForAccessoriesLoungeSim(strText);
            sqlCmd += " UNION ";
            sqlCmd += ReturnStringSearchForClothingLoungeSim(strText);
            sqlCmd += " UNION ";
            sqlCmd += ReturnStringSearchForClubsLoungeSim(strText);
            sqlCmd += " ORDER BY sku DESC";

            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms));
        }
        //Returns string for search accessories
        private string ReturnStringSearchForAccessoriesLoungeSim(ArrayList array)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT A.sku, A.accessoryType AS description, "
                            + "(SELECT L.city FROM tbl_accessories AC JOIN tbl_location L ON AC.locationID = L.locationID WHERE AC.sku = A.sku) AS locationName, "
                            + "A.quantity, A.price, A.cost, A.typeID, CAST(0 AS bit) AS isTradeIn, A.comments FROM tbl_accessories A WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                            + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                            + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                            + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%')) ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT A.sku, A.accessoryType AS description, "
                            + "(SELECT L.city FROM tbl_accessories AC JOIN tbl_location L ON AC.locationID = L.locationID WHERE AC.sku = A.sku) AS locationName, "
                            + "A.quantity, A.price, A.cost, A.typeID, CAST(0 AS bit) AS isTradeIn, A.comments FROM tbl_accessories A WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                            + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                            + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                            + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%'))) ";
                }
            }
            sqlCmd += ") AS tblResult";
            return sqlCmd;
        }
        //Returns string for search clothing
        private string ReturnStringSearchForClothingLoungeSim(ArrayList array)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT CL.sku, (SELECT B.brandName + ' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + "
                        + "' ' + CLO.style AS description FROM tbl_clothing CLO JOIN tbl_brand B "
                        + "ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS description, (SELECT L.city "
                        + "FROM tbl_clothing CLO JOIN tbl_location L ON CLO.locationID = L.locationID WHERE CLO.sku = CL.sku) "
                        + "AS locationName, CL.quantity, CL.price, CL.cost, CL.typeID, CAST(0 AS bit) AS isTradeIn, CL.comments FROM tbl_clothing CL WHERE ((CAST(sku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%"
                        + array[i] + "%') OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%')) ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT CL.sku, (SELECT B.brandName + ' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + "
                        + "' ' + CLO.style AS description FROM tbl_clothing CLO JOIN tbl_brand B "
                        + "ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS description, (SELECT L.city "
                        + "FROM tbl_clothing CLO JOIN tbl_location L ON CLO.locationID = L.locationID WHERE CLO.sku = CL.sku) "
                        + "AS locationName, CL.quantity, CL.price, CL.cost, CL.typeID, CAST(0 AS bit) AS isTradeIn, CL.comments FROM tbl_clothing CL WHERE ((CAST(sku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%"
                        + array[i] + "%') OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%'))) ";
                }
            }
            sqlCmd += ") AS tblResults";
            return sqlCmd;
        }
        //Returns string for search clubs
        private string ReturnStringSearchForClubsLoungeSim(ArrayList array)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT C.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + "
                        + "CLU.clubType + ' ' + CLU.shaftSpec + ' ' + CLU.shaftFlex + ' ' + CLU.dexterity AS description "
                        + "FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN tbl_model M ON CLU.modelID = "
                        + "M.modelID WHERE CLU.sku = C.sku) AS description, (SELECT L.city FROM tbl_clubs "
                        + "CLU JOIN tbl_location L ON CLU.locationID = L.locationID WHERE CLU.sku = C.sku) AS locationName, "
                        + "C.quantity, C.price, C.cost, C.typeID, C.isTradeIn, C.comments FROM tbl_clubs C WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                        + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                        + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                        + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%')) ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT C.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + "
                        + "CLU.clubType + ' ' + CLU.shaftSpec + ' ' + CLU.shaftFlex + ' ' + CLU.dexterity AS description "
                        + "FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN tbl_model M ON CLU.modelID = "
                        + "M.modelID WHERE CLU.sku = C.sku) AS description, (SELECT L.city FROM tbl_clubs "
                        + "CLU JOIN tbl_location L ON CLU.locationID = L.locationID WHERE CLU.sku = C.sku) AS locationName, "
                        + "C.quantity, C.price, C.cost, C.typeID, C.isTradeIn, C.comments FROM tbl_clubs C WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                        + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                        + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                        + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%'))) ";
                }
            }
            sqlCmd += ") AS tblResults";
            return sqlCmd;
        }

        



    }
}