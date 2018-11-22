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
        private List<Items> ConvertFromDataTableToCartItems(DataTable dt)
        {
            List<Items> items = dt.AsEnumerable().Select(row =>
            new Items
            {
                sku = row.Field<int>("sku"),
                description = row.Field<string>("description"),
                location = row.Field<string>("locationName"),
                quantity = row.Field<int>("quantity"),
                price = row.Field<double>("price"),
                cost = row.Field<double>("cost"),
                typeID = row.Field<int>("typeID"),
                isTradeIn = row.Field<bool>("isTradeIn"),
                comments = row.Field<string>("comments")
            }).ToList();
            return items;
        }
        private int ConvertFromDataTableToInt(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ConvertFromDataTableToString(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
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
            sqlCmd += " ORDER BY sku DESC";
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
                    sqlCmd += "SELECT A.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + AC.size + ' ' + AC.colour AS description FROM tbl_accessories AC JOIN tbl_brand B ON AC.brandID = B.brandID JOIN tbl_model M ON AC.modelID = M.modelID WHERE AC.sku = A.sku) AS description, "
                            + "(SELECT L.city FROM tbl_accessories AC JOIN tbl_location L ON AC.locationID = L.locationID WHERE AC.sku = A.sku) AS locationName, "
                            + "A.quantity, A.price, A.cost, A.typeID, CAST(0 AS bit) AS isTradeIn, A.comments FROM tbl_accessories A WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                            + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                            + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                            + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%')) AND A.quantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT A.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + AC.size + ' ' + AC.colour AS description FROM tbl_accessories AC JOIN tbl_brand B ON AC.brandID = B.brandID JOIN tbl_model M ON AC.modelID = M.modelID WHERE AC.sku = A.sku) AS description, "
                            + "(SELECT L.city FROM tbl_accessories AC JOIN tbl_location L ON AC.locationID = L.locationID WHERE AC.sku = A.sku) AS locationName, "
                            + "A.quantity, A.price, A.cost, A.typeID, CAST(0 AS bit) AS isTradeIn, A.comments FROM tbl_accessories A WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                            + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                            + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                            + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%')) AND A.quantity > " + quantity + ") ";
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
                    sqlCmd += "SELECT CL.sku, (SELECT B.brandName + ' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + "
                        + "' ' + CLO.style AS description FROM tbl_clothing CLO JOIN tbl_brand B "
                        + "ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS description, (SELECT L.city "
                        + "FROM tbl_clothing CLO JOIN tbl_location L ON CLO.locationID = L.locationID WHERE CLO.sku = CL.sku) "
                        + "AS locationName, CL.quantity, CL.price, CL.cost, CL.typeID, CAST(0 AS bit) AS isTradeIn, CL.comments FROM tbl_clothing CL WHERE ((CAST(sku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%"
                        + array[i] + "%') OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%')) AND CL.quantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT CL.sku, (SELECT B.brandName + ' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + "
                        + "' ' + CLO.style AS description FROM tbl_clothing CLO JOIN tbl_brand B "
                        + "ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS description, (SELECT L.city "
                        + "FROM tbl_clothing CLO JOIN tbl_location L ON CLO.locationID = L.locationID WHERE CLO.sku = CL.sku) "
                        + "AS locationName, CL.quantity, CL.price, CL.cost, CL.typeID, CAST(0 AS bit) AS isTradeIn, CL.comments FROM tbl_clothing CL WHERE ((CAST(sku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%"
                        + array[i] + "%') OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%')) AND CL.quantity > " + quantity + ") ";
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
                    sqlCmd += "SELECT C.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + "
                        + "CLU.clubType + ' ' + CLU.shaftSpec + ' ' + CLU.shaftFlex + ' ' + CLU.dexterity AS description "
                        + "FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN tbl_model M ON CLU.modelID = "
                        + "M.modelID WHERE CLU.sku = C.sku) AS description, (SELECT L.city FROM tbl_clubs "
                        + "CLU JOIN tbl_location L ON CLU.locationID = L.locationID WHERE CLU.sku = C.sku) AS locationName, "
                        + "C.quantity, C.price, C.cost, C.typeID, C.isTradeIn, C.comments FROM tbl_clubs C WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                        + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                        + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                        + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%')) AND C.quantity > " + quantity + " ";
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
                        + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%')) AND C.quantity > " + quantity + ") ";
                }
            }
            sqlCmd += ") AS tblResults";
            return sqlCmd;
        }

        //Returning the list of items
        public List<Items> ReturnInvoiceItemsFromSearchStringAndQuantity(string searchText, bool zeroQuantity, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromSearchStringAndQuantity";
            int quantity = 0;
            if (zeroQuantity)
            {
                quantity = -1;
            }
            string sqlCmd = ReturnItemsFromSearchString(searchText, quantity);
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<Items> ReturnInvoiceItemsFromSearchStringForSale(string searchText, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromSearchStringForSale";
            string sqlCmd = ReturnItemsFromSearchString(searchText, -1);
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<Items> ReturnTradeInSku(object[] objPageDetails)
        {
            string strQueryName = "ReturnTradeInSku";
            string sqlCmd = "SELECT C.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + "
                       + "CLU.clubType + ' ' + CLU.shaftSpec + ' ' + CLU.shaftFlex + ' ' + CLU.dexterity AS description "
                       + "FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN tbl_model M ON CLU.modelID = "
                       + "M.modelID  WHERE CLU.sku = C.sku) AS description, (SELECT L.city FROM tbl_clubs "
                       + "CLU JOIN tbl_location L ON CLU.locationID = L.locationID WHERE CLU.sku = C.sku) AS locationName, "
                       + "C.quantity,	C.price, C.cost, C.typeID, C.isTradeIn, C.comments FROM tbl_clubs C WHERE C.sku = 100000";
            object[][] parms = { };
            return ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }

        //DropDownList insertion
        public DataTable ReturnDropDownForBrand(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForBrand";
            string sqlCmd = "SELECT brandID, brandName FROM "
                + "tbl_brand ORDER BY brandName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnDropDownForModel(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForModel";
            string sqlCmd = "SELECT modelID, modelName FROM "
                + "tbl_model ORDER BY modelName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnDropDownForItemType(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForItemType";
            string sqlCmd = "SELECT typeID, typeDescription FROM "
                + "tbl_itemType ORDER BY typeDescription";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnDropDownForClubType(object[] objPageDetails)
        {
            string strQueryName = "ReturnDropDownForClubType";
            string sqlCmd = "SELECT typeID, typeName FROM tbl_clubType ORDER BY typeName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //TradeIn Criteria
        public int ReserveTradeInSKU(int location, object[] objPageDetails)
        {
            string strQueryName = "ReserveTradeInSKU";
            string sqlCmd = "UPDATE tbl_tradeInSkusForCart SET currentSKU = @sku "
                + "WHERE locationID = @locationID";
            int newSKU = TradeInSKU(location, objPageDetails) + 1;
            object[][] parms =
            {
                new object[] { "@sku", newSKU },
                new object[] { "@locationID", location }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return newSKU;
        }
        private int TradeInSKU(int location, object[] objPageDetails)
        {
            string strQueryName = "TradeInSKU";
            string sqlCmd = "SELECT currentSKU FROM tbl_tradeInSkusForCart WHERE locationID = @locationID";

            object[][] parms =
            {
                new object[] { "@locationID", location }
            };
            return ConvertFromDataTableToInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string GetClubTypeName(int typeID, object[] objPageDetails)
        {
            string strQueryName = "GetClubTypeName";
            string sqlCmd = "SELECT typeName FROM tbl_clubType WHERE typeID = @typeID";

            object[][] parms =
            {
                new object[] { "@typeID", typeID }
        };
            //Returns the name of the club type
            return ConvertFromDataTableToString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void AddTradeInItemToTempTable(Clubs T, object[] objPageDetails)
        {
            string strQueryName = "AddTradeInItemToTempTable";
            string sqlCmd = "INSERT INTO tbl_tempTradeInCartSkus VALUES(@sku, @brandID, @modelID, "
                + "@clubType, @shaft, @numberOfClubs, @premium, @cost, @price, @quantity, @clubSpec, "
                + "@shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @isTradeIn, @comments)";

            object[][] parms =
            {
                new object[] { "@sku", T.sku },
                new object[] { "brandID", T.brandID },
                new object[] { "modelID", T.modelID },
                new object[] { "clubType", T.clubType },
                new object[] { "shaft", T.shaft },
                new object[] { "numberOfClubs", T.numberOfClubs },
                new object[] { "premium", T.premium },
                new object[] { "cost", T.cost },
                new object[] { "price", T.price },
                new object[] { "quantity", T.quantity },
                new object[] { "clubSpec", T.clubSpec },
                new object[] { "shaftSpec", T.shaftSpec },
                new object[] { "shaftFlex", T.shaftFlex },
                new object[] { "dexterity", T.dexterity },
                new object[] { "typeID", T.typeID },
                new object[] { "locationID", T.itemlocation },
                new object[] { "isTradeIn", T.isTradeIn },
                new object[] { "comments", T.comments }
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
    }
}