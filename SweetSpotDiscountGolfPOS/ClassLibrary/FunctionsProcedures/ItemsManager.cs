using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;


namespace SweetSpotDiscountGolfPOS.FP
{
    public class ItemsManager
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();

        //Converters
        private List<InvoiceItems> ConvertFromDataTableToCartItems(DataTable dt)
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
        private List<InvoiceItems> ConvertFromDataTableToInventoryItem(DataTable dt)
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


        //DB calls
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
                    sqlCmd += "SELECT A.intInventoryID, A.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + AC.varSize + ' ' + AC.varColour + "
                        + "' ' + AC.varTypeOfAccessory AS varItemDescription FROM tbl_accessories AC JOIN tbl_brand B ON AC.intBrandID = B.intBrandID JOIN "
                        + "tbl_model M ON AC.intModelID = M.intModelID WHERE AC.intInventoryID = A.intInventoryID) AS varItemDescription, A.intLocationID, "
                        + "(SELECT L.varCityName FROM tbl_accessories AC JOIN tbl_location L ON AC.intLocationID = L.intLocationID WHERE AC.intInventoryID "
                        + "= A.intInventoryID) AS varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltItemPrice, A.fltCost AS fltItemCost, "
                        + "A.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, A.varAdditionalInformation FROM tbl_accessories A WHERE ((varSku LIKE '%" 
                        + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%') OR intModelID "
                        + "IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%') OR CONCAT(varSize, varColour, "
                        + "varTypeOfAccessory, varAdditionalInformation) LIKE '%" + array[i] + "%')) AND A.intQuantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT A.intInventoryID, A.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + AC.varSize + ' ' + "
                        + "AC.varColour + ' ' + AC.varTypeOfAccessory AS varItemDescription FROM tbl_accessories AC JOIN tbl_brand B ON AC.intBrandID = "
                        + "B.intBrandID JOIN tbl_model M ON AC.intModelID = M.intModelID WHERE AC.intInventoryID = A.intInventoryID) AS varItemDescription"
                        + ", A.intLocationID, (SELECT L.varCityName FROM tbl_accessories AC JOIN tbl_location L ON AC.intLocationID = L.intLocationID "
                        + "WHERE AC.intInventoryID = A.intInventoryID) AS varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltItemPrice, "
                        + "A.fltCost AS fltItemCost, A.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, A.varAdditionalInformation FROM tbl_accessories "
                        + "A WHERE ((varSku LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" 
                        + array[i] + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%') OR CONCAT(varSize"
                        + ", varColour, varTypeOfAccessory, varAdditionalInformation) LIKE '%" + array[i] + "%')) AND A.intQuantity > " + quantity + ") ";
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
        private List<InvoiceItems> ReturnInventoryFromSearchStringAndQuantity(string searchText, bool zeroQuantity, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromSearchStringAndQuantity";
            int quantity = 0;
            if (zeroQuantity)
            {
                quantity = -1;
            }
            string sqlCmd = ReturnItemsFromSearchString(searchText, quantity);
            object[][] parms = { };
            return ConvertFromDataTableToInventoryItem(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private List<InvoiceItems> ReturnInvoiceItemsFromSearchStringForSale(string searchText, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromSearchStringForSale";
            string sqlCmd = ReturnItemsFromSearchString(searchText, -1);
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        //private List<InvoiceItems> ReturnTradeInSku(object[] objPageDetails)
        //{
        //    string strQueryName = "ReturnTradeInSku";
        //    string sqlCmd = "SELECT C.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + CLU.varClubSpecification "
        //        + "+ ' ' + CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + CLU.varClubDexterity AS "
        //        + "varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID JOIN tbl_model M ON CLU.intModelID "
        //        + "= M.intModelID  WHERE CLU.intInventoryID = C.intInventoryID) AS varItemDescription, (SELECT L.varCityName FROM tbl_clubs "
        //        + "CLU JOIN tbl_location L ON CLU.intLocationID = L.intLocationID WHERE CLU.intInventoryID = C.intInventoryID) AS varLocationName, "
        //        + "C.intQuantity AS intItemQuantity, C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, C.intItemTypeID, C.bitIsUsedProduct, "
        //        + "C.varAdditionalInformation FROM tbl_clubs C WHERE C.varSku = '100000'";
        //    object[][] parms = { };
        //    return ConvertFromDataTableToCartItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        //    //return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        //}
        private DataTable ReturnDropDownForBrand(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForBrand";
            string sqlCmd = "SELECT intBrandID, varBrandName FROM tbl_brand ORDER BY varBrandName";
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnDropDownForModel(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForModel";
            string sqlCmd = "SELECT intModelID, varModelName FROM tbl_model ORDER BY varModelName";
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnDropDownForItemType(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForItemType";
            string sqlCmd = "SELECT intItemTypeID, varItemTypeName FROM tbl_itemType ORDER BY varItemTypeName";
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //TradeIn Criteria
        private string[] ReserveTradeInSKU(CurrentUser cu, object[] objPageDetails)
        {
            string strQueryName = "ReserveTradeInSKU";
            string sqlCmd = "SELECT CONCAT(varStoreCode, varTradeInCode, CASE WHEN LEN(CAST(intSetTradeInNumber AS INT)) < 6 THEN RIGHT(RTRIM("
                + "'000000' + CAST(intSetTradeInNumber AS VARCHAR(6))),6) ELSE CAST(intSetTradeInNumber AS VARCHAR(MAX)) END) AS varTradeInSKU "
                + "FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };

            string sqlCmd2 = "SELECT intInventoryIDTracking FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";

            object[][] parms2 =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };

            string inventorySku = DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            string inventoryID = DBC.MakeDataBaseCallToReturnString(sqlCmd2, parms2, objPageDetails, strQueryName);
            string[] inventory = { inventorySku, inventoryID };

            TradeInSKU(cu, objPageDetails);
            return inventory;
        }
        private void TradeInSKU(CurrentUser cu, object[] objPageDetails)
        {
            string strQueryName = "TradeInSKU";
            string sqlCmd = "UPDATE tbl_storedStoreNumbers SET intSetTradeInNumber = intSetTradeInNumber + 1 WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);

            string sqlCmd2 = "UPDATE tbl_storedStoreNumbers SET intInventoryIDTracking = intInventoryIDTracking + 1";
            object[][] parms2 = { };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd2, parms2, objPageDetails, strQueryName);
        }     
        private void AddTradeInItemToTempTable(Clubs tradeIn, object[] objPageDetails)
        {
            string strQueryName = "AddTradeInItemToTempTable";
            string sqlCmd = "INSERT INTO tbl_tempTradeInCartSkus VALUES(@intInventoryID, @varSku, @intBrandID, @intModelID, @varTypeOfClub, "
                + "@varShaftType, @varNumberOfClubs, @fltPremiumCharge, @fltCost, @fltPrice, @intQuantity, @varClubSpecification, "
                + "@varShaftSpecification, @varShaftFlexability, @varClubDexterity, @intItemTypeID, @intLocationID, @bitIsUsedProduct, "
                + "@varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@intInventoryID", tradeIn.intInventoryID },
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

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            SetTaxesForNewTradeInInventory(tradeIn.intInventoryID, objPageDetails);
        }
        private void SetTaxesForNewTradeInInventory(int inventoryID, object[] objPageDetails)
        {
            TaxManager TM = new TaxManager();
            ItemDataUtilities IDU = new ItemDataUtilities();
            DataTable lTax = TM.ReturnTaxList(objPageDetails);
            foreach (DataRow TR in lTax.Rows)
            {
                bool chargeTax = true;
                if (TM.CallCheckForLiquorTax(Convert.ToInt32(TR[0]), objPageDetails))
                {
                    chargeTax = false;
                }
                IDU.CallSaveTaxIDForNewInventoryItem(inventoryID, Convert.ToInt32(TR[0]), chargeTax, objPageDetails);
            }
        }
        private string ReturnModelNameFromModelID(int modelID, object[] objPageDetails)
        {
            string strQueryName = "ReturnModelNameFromModelID";
            string sqlCmd = "SELECT varModelName FROM tbl_model WHERE intModelID = @intModelID";
            object[][] parms =
            {
                new object[] { "@intModelID", modelID }
            };
            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ReturnBrandlNameFromBrandID(int brandID, object[] objPageDetails)
        {
            string strQueryName = "ReturnBrandlNameFromBrandID";
            string sqlCmd = "SELECT varBrandName FROM tbl_brand WHERE intBrandID = @intBrandID";
            object[][] parms =
            {
                new object[] { "@intBrandID", brandID }
            };
            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void ProgramLoungeSimButton(int inventoryID, string buttonText, string buttonName, object[] objPageDetails)
        {
            DeleteProgrammedButton(buttonName, objPageDetails);
            StoreSkuAndNumber(inventoryID, buttonText, buttonName, objPageDetails);
        }
        private void DeleteProgrammedButton(string buttonName, object[] objPageDetails)
        {
            string strQueryName = "DeleteProgrammedButton";
            string sqlCmd = "DELETE FROM tbl_LoungeButtonItemCombination WHERE varLoungeButtonName = @varLoungeButtonName";
            object[][] parms =
            {
                new object[] { "@varLoungeButtonName", buttonName }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void StoreSkuAndNumber(int inventoryID, string buttonText, string buttonName, object[] objPageDetails)
        {
            string strQueryName = "StoreSkuAndNumber";
            string sqlCmd = "INSERT INTO tbl_LoungeButtonItemCombination VALUES(@intInventoryID, @varButtonText, @varLoungeButtonName)";
            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@varButtonText", buttonText },
                new object[] { "@varLoungeButtonName", buttonName }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public List<InvoiceItems> ReturnProgrammedSaleableItems(object[] objPageDetails)
        {
            return CallAllProgrammedItems(objPageDetails);
        }
        private List<InvoiceItems> CallAllProgrammedItems(object[] objPageDetails)
        {
            string strQueryName = "CallAllProgrammedItems";
            //string sqlCmd = "SELECT LBIC.intInventoryID, A.varTypeOfAccessory AS varItemDescription, L.varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltOtemPrice, "
            //    + "A.fltCost AS fltItemCost, A.intItemTypeID, CAST(0 AS bit) AS bitIsClubTradeIn, LBIC.varLoungeButton AS varAdditionalInformation FROM tbl_LoungeButtonItemCombination "
            //    + "LBIC LEFT JOIN tbl_accessories A ON A.intInventoryID = LBIC.intInventoryID JOIN tbl_location L ON L.intLocationID = A.intLocationID UNION SELECT LBIC.intInventoryID, "
            //    + "(SELECT B.varBrandName + ' ' + CLO.varSize + ' ' + CLO.varColour + ' ' + CLO.varGender + ' ' + CLO.varStyle AS varItemDescription FROM tbl_clothing CLO JOIN tbl_brand "
            //    + "B ON CLO.intBrandID = B.intBrandID WHERE CLO.intInventoryID = CL.intInventoryID) AS varItemDescription, L.varLocationName, CL.intQuantity AS intItemQuantity, "
            //    + "CL.fltPrice AS fltItemPrice, CL.fltCost AS fltItemCost, CL.intItemTypeID, CAST(0 AS bit) AS bitIsClubTradeIn, LBIC.varLoungeButton AS varAdditionalInformation FROM "
            //    + "tbl_LoungeButtonItemCombination LBIC LEFT JOIN tbl_clothing CL ON CL.intInventoryID = LBIC.intInventoryID JOIN tbl_location L ON L.intLocationID = CL.intLocationID "
            //    + "UNION SELECT LBIC.intInventoryID, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + CLU.varClubSpecification + ' ' + CLU.varTypeOfClub + ' ' + "
            //    + "CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + CLU.varClubDexterity AS varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = "
            //    + "B.intBrandID JOIN tbl_model M ON CLU.intModelID = M.intModelID WHERE CLU.intInventoryID = C.intInventoryID) AS varItemDescription, L.varLocationName, C.intQuantity AS "
            //    + "intItemQuantity, C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, C.intItemTypeID, C.bitIsUsedProduct AS bitIsTradeIn, LBIC.varLoungeButton AS "
            //    + "varAdditionalInformation FROM tbl_LoungeButtonItemCombination LBIC LEFT JOIN tbl_clubs C ON C.intInventoryID = LBIC.intInventoryID JOIN tbl_location L ON "
            //    + "L.intLocationID = C.intLocationID";

            string sqlCmd = "SELECT LBIC.intInventoryID, A.varSku, A.varTypeOfAccessory AS varItemDescription, A.intQuantity AS intItemQuantity, A.fltPrice "
                + "AS fltItemPrice, A.fltCost AS fltItemCost, A.intItemTypeID, LBIC.varLoungeButtonName AS varAdditionalInformation FROM "
                + "tbl_LoungeButtonItemCombination LBIC LEFT JOIN tbl_accessories A ON A.intInventoryID = LBIC.intInventoryID JOIN tbl_location L ON "
                + "L.intLocationID = A.intLocationID UNION SELECT LBIC.intInventoryID, CL.varSku, (SELECT B.varBrandName + ' ' + CLO.varSize + ' ' + "
                + "CLO.varColour + ' ' + CLO.varGender + ' ' + CLO.varStyle AS varItemDescription FROM tbl_clothing CLO JOIN tbl_brand B ON CLO.intBrandID "
                + "= B.intBrandID WHERE CLO.intInventoryID = CL.intInventoryID) AS varItemDescription, CL.intQuantity AS intItemQuantity, CL.fltPrice AS "
                + "fltItemPrice, CL.fltCost AS fltItemCost, CL.intItemTypeID, LBIC.varLoungeButtonName AS varAdditionalInformation FROM "
                + "tbl_LoungeButtonItemCombination LBIC LEFT JOIN tbl_clothing CL ON CL.intInventoryID = LBIC.intInventoryID JOIN tbl_location L ON "
                + "L.intLocationID = CL.intLocationID UNION SELECT LBIC.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + "
                + "CLU.varClubSpecification + ' ' + CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + "
                + "CLU.varClubDexterity AS varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID JOIN tbl_model M ON "
                + "CLU.intModelID = M.intModelID WHERE CLU.intInventoryID = C.intInventoryID) AS varItemDescription, C.intQuantity AS intItemQuantity, "
                + "C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, C.intItemTypeID, LBIC.varLoungeButtonName AS varAdditionalInformation FROM "
                + "tbl_LoungeButtonItemCombination LBIC LEFT JOIN tbl_clubs C ON C.intInventoryID = LBIC.intInventoryID JOIN tbl_location L ON "
                + "L.intLocationID = C.intLocationID UNION SELECT LBIC.intInventoryID, '' AS varSku, LBIC.varButtonText AS varItemDescription, CAST(0 AS "
                + "INT) AS intItemQuantity, CAST(0 AS FLOAT) AS fltItemPrice, CAST(0 AS FLOAT) AS fltItemCost, CAST(0 AS INT) AS intItemTypeID, "
                + "LBIC.varLoungeButtonName AS varAdditionalInformation FROM tbl_LoungeButtonItemCombination LBIC WHERE LBIC.intInventoryID = 0";

            object[][] parms = { };
            return ConvertFromDataTableToCartItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private List<InvoiceItems> ReturnInvoiceItemsFromForLoungeSim(string searchText, object[] objPageDetails)
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
            sqlCmd += " ORDER BY varSku DESC";

            object[][] parms = { };
            return ConvertFromDataTableToCartItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        //Returns string for search accessories
        private string ReturnStringSearchForAccessoriesLoungeSim(ArrayList array)
        {
            string sqlCmd = "SELECT TOP (2000) * FROM (";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd += "SELECT A.intInventoryID, A.varSku, A.varTypeOfAccessory AS varItemDescription, (SELECT L.varCityName "
                        + "FROM tbl_accessories AC JOIN tbl_location L ON AC.intLocationID = L.intLocationID WHERE AC.varSku = A.varSku) AS "
                        + "varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltItemPrice, A.fltCost AS fltItemCost, "
                        + "A.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, A.varAdditionalInformation FROM tbl_accessories A WHERE (("
                        + "CAST(varSku AS VARCHAR) LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE "
                        + "varBrandName LIKE '%" + array[i] + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" 
                        + array[i] + "%') OR CONCAT(varSize, varColour, varTypeOfAccessory, varAdditionalInformation) LIKE '%" + array[i] + "%')) ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT A.intInventoryID, A.varSku, A.varTypeOfAccessory AS varItemDescription, (SELECT L.varCityName "
                        + "FROM tbl_accessories AC JOIN tbl_location L ON AC.intLocationID = L.intLocationID WHERE AC.varSku = A.varSku) AS "
                        + "varLocationName, A.intQuantity AS intItemQuantity, A.fltPrice AS fltItemPrice, A.fltCost AS fltItemCost, "
                        + "A.intItemTypeID, CAST(0 AS bit) AS bitIsUsedProduct, A.varAdditionalInformation FROM tbl_accessories A WHERE ((CAST("
                        + "varSku AS VARCHAR) LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName "
                        + "LIKE '%" + array[i] + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] 
                        + "%') OR CONCAT(varSize, varColour, varTypeOfAccessory, varAdditionalInforamtion) LIKE '%" + array[i] + "%'))) ";
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
                    sqlCmd += "SELECT CL.intInventoryID, CL.varSku, (SELECT B.varBrandName + ' ' + CLO.varSize + ' ' + CLO.varColour + ' ' + "
                        + "CLO.varGender + ' ' + CLO.varStyle AS varItemDescription FROM tbl_clothing CLO JOIN tbl_brand B ON CLO.intBrandID = "
                        + "B.intBrandID WHERE CLO.varSku = CL.varSku) AS varItemDescription, (SELECT L.varCityName FROM tbl_clothing CLO JOIN "
                        + "tbl_location L ON CLO.intLocationID = L.intLocationID WHERE CLO.varSku = CL.varSku) AS varLocationName, CL.intQuantity "
                        + "AS intItemQuantity, CL.fltPrice AS fltItemPrice, CL.fltCost AS fltItemCost, CL.intItemTypeID, CAST(0 AS bit) AS "
                        + "bitIsUsedProduct, CL.varAdditionalInformation FROM tbl_clothing CL WHERE ((CAST(varSku AS VARCHAR) LIKE '%" 
                        + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%') OR "
                        + "CONCAT(varSize, varColour, varGender, varStyle, varAdditionalInformation) LIKE '%" + array[i] + "%')) ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT CL.intInventoryID, CL.varSku, (SELECT B.varBrandName + ' ' + CLO.varSize + ' ' + CLO.varColour + "
                        + "' ' + CLO.varGender + ' ' + CLO.varStyle AS varItemDescription FROM tbl_clothing CLO JOIN tbl_brand B ON CLO.intBrandID "
                        + "= B.intBrandID WHERE CLO.varSku = CL.varSku) AS varItemDescription, (SELECT L.varCityName FROM tbl_clothing CLO JOIN "
                        + "tbl_location L ON CLO.intLocationID = L.intLocationID WHERE CLO.varSku = CL.varSku) AS varLocationName, "
                        + "CL.intQuantity AS intItemQuantity, CL.fltPrice AS fltItemPrice, CL.fltCost AS fltItemCost, CL.intItemTypeID, CAST(0 "
                        + "AS bit) AS bitIsUsedProduct, CL.varAdditionalInformation FROM tbl_clothing CL WHERE ((CAST(varSku AS VARCHAR) LIKE '"
                        + "%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%') "
                        + "OR CONCAT(varSize, varColour, varGender, varStyle, varAdditionalInformation) LIKE '%" + array[i] + "%'))) ";
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
                    sqlCmd += "SELECT C.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + CLU.varClubSpecification "
                        + "+ ' ' + CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + ' ' + CLU.varClubDexterity "
                        + "AS varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID JOIN tbl_model M ON "
                        + "CLU.intModelID = M.intModelID WHERE CLU.varSku = C.varSku) AS varItemDescription, (SELECT L.varCityName FROM "
                        + "tbl_clubs CLU JOIN tbl_location L ON CLU.intLocationID = L.intLocationID WHERE CLU.varSku = C.varSku) AS "
                        + "varLocationName, C.intQuantity AS intItemQuantity, C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, "
                        + "C.intItemTypeID, C.bitIsUsedProduct, C.varAdditionalInformation FROM tbl_clubs C WHERE ((CAST(varSku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] 
                        + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%') OR CONCAT("
                        + "varClubSpecification, varTypeOfClub, varShaftSpecification, varShaftFlexability, varClubDexterity) LIKE '%" 
                        + array[i] + "%')) ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT C.intInventoryID, C.varSku, (SELECT B.varBrandName + ' ' + M.varModelName + ' ' + "
                        + "CLU.varClubSpecification + ' ' + CLU.varTypeOfClub + ' ' + CLU.varShaftSpecification + ' ' + CLU.varShaftFlexability + "
                        + "' ' + CLU.varClubDexterity AS varItemDescription FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.intBrandID = B.intBrandID "
                        + "JOIN tbl_model M ON CLU.intModelID = M.intModelID WHERE CLU.varSku = C.varSku) AS varItemDescription, (SELECT "
                        + "L.varCityName FROM tbl_clubs CLU JOIN tbl_location L ON CLU.intLocationID = L.intLocationID WHERE CLU.varSku = C.varSku) "
                        + "AS varLocationName, C.intQuantity AS intItemQuantity, C.fltPrice AS fltItemPrice, C.fltCost AS fltItemCost, "
                        + "C.intItemTypeID, C.bitIsUsedProduct, C.varAdditionalInformation FROM tbl_clubs C WHERE ((CAST(varSku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] 
                        + "%') OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%') OR CONCAT("
                        + "varClubSpecification, varTypeOfClub, varShaftSpecification, varShaftFlexability, varClubDexterity) LIKE '%" 
                        + array[i] + "%'))) ";
                }
            }
            sqlCmd += ") AS tblResults";
            return sqlCmd;
        }


        //Public Calls
        public List<InvoiceItems> CallConvertFromDataTableToCartItems(DataTable dt)
        {
            return CallConvertFromDataTableToCartItems(dt);
        }
        public string[] CallReserveTradeInSKU(CurrentUser cu, object[] objPageDetails)
        {
            return ReserveTradeInSKU(cu, objPageDetails);
        }
        public DataTable CallReturnDropDownForBrand(object[] objPageDetails)
        {
            return ReturnDropDownForBrand(objPageDetails);
        }
        public DataTable CallReturnDropDownForModel(object[] objPageDetails)
        {
            return ReturnDropDownForModel(objPageDetails);
        }
        public void CallAddTradeInItemToTempTable(Clubs tradeIn, object[] objPageDetails)
        {
            AddTradeInItemToTempTable(tradeIn, objPageDetails);
        }
        public string CallReturnModelNameFromModelID(int modelID, object[] objPageDetails)
        {
            return ReturnModelNameFromModelID(modelID, objPageDetails);
        }
        public string CallReturnBrandlNameFromBrandID(int brandID, object[] objPageDetails)
        {
            return ReturnBrandlNameFromBrandID(brandID, objPageDetails);
        }
        public List<InvoiceItems> CallReturnInvoiceItemsFromSearchStringForSale(string searchText, object[] objPageDetails)
        {
            return ReturnInvoiceItemsFromSearchStringForSale(searchText, objPageDetails);
        }
        public List<InvoiceItems> CallReturnInvoiceItemsFromForLoungeSim(string searchText, object[] objPageDetails)
        {
            return ReturnInvoiceItemsFromForLoungeSim(searchText, objPageDetails);
        }
        public List<InvoiceItems> CallReturnInventoryFromSearchStringAndQuantity(string searchText, bool zeroQuantity, object[] objPageDetails)
        {
            return ReturnInventoryFromSearchStringAndQuantity(searchText, zeroQuantity, objPageDetails);
        }
        public DataTable CallReturnDropDownForItemType(object[] objPageDetails)
        {
            return ReturnDropDownForItemType(objPageDetails);
        }

    }
}