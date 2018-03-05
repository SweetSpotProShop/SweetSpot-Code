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

        private List<Items> ConvertFromDataTableToCartItems(DataTable dt)
        {
            List<Items> items = dt.AsEnumerable().Select(row =>
            new Items
            {
                sku = row.Field<int>("sku"),
                description = row.Field<string>("sku"),
                quantity = row.Field<int>("sku"),
                price = row.Field<double>("sku"),
                cost = row.Field<double>("sku"),
                discount = row.Field<double>("sku"),
                percent = row.Field<bool>("sku"),
                typeID = row.Field<int>("sku"),
                tradeIn = row.Field<bool>("sku")
            }).ToList();
            return items;
        }
        private List<Items> ConvertFromDataTableToItems(DataTable dt)
        {
            List<Items> items = dt.AsEnumerable().Select(row =>
            new Items
            {
                sku = row.Field<int>("sku"),
                description = row.Field<string>("itemDescription"),
                location = row.Field<string>("locationName"),
                quantity = row.Field<int>("quantity"),
                price = row.Field<double>("price"),
                cost = row.Field<double>("cost")
            }).ToList();
            return items;
        }
        //Returns list of InvoiceItems based on a text search
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

            return sqlCmd;
        }
        //Returns string for search accessories
        private string ReturnStringSearchForAccessories(ArrayList array, int quantity)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT A.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + AC.size + ' ' + AC.colour + ' ' + AC.comments AS itemDescription FROM tbl_accessories AC JOIN tbl_brand B ON AC.brandID = B.brandID JOIN tbl_model M ON AC.modelID = M.modelID WHERE AC.sku = A.sku) AS itemDescription, "
                            + "(SELECT L.locationName FROM tbl_accessories AC JOIN tbl_location L ON AC.locationID = L.locationID WHERE AC.sku = A.sku) AS locationName, "
                            + "A.quantity, A.price, A.cost FROM tbl_accessories A WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                            + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                            + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                            + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%')) AND A.quantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT A.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + AC.size + ' ' + AC.colour + ' ' + AC.comments AS itemDescription FROM tbl_accessories AC JOIN tbl_brand B ON AC.brandID = B.brandID JOIN tbl_model M ON AC.modelID = M.modelID WHERE AC.sku = A.sku) AS itemDescription, "
                            + "(SELECT L.locationName FROM tbl_accessories AC JOIN tbl_location L ON AC.locationID = L.locationID WHERE AC.sku = A.sku) AS locationName, "
                            + "A.quantity, A.price, A.cost FROM tbl_accessories A WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                            + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                            + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                            + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%')) AND A.quantity > " + quantity + ") ";
                }
            }
            return sqlCmd;
        }
        //Returns string for search clothing
        private string ReturnStringSearchForClothing(ArrayList array, int quantity)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT CL.sku, (SELECT B.brandName + ' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + "
                        + "' ' + CLO.style + ' ' + CLO.comments AS itemDescription FROM tbl_clothing CLO JOIN tbl_brand B "
                        + "ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS itemDescription, (SELECT L.locationName "
                        + "FROM tbl_clothing CLO JOIN tbl_location L ON CLO.locationID = L.locationID WHERE CLO.sku = CL.sku) "
                        + "AS locationName, CL.quantity, CL.price, CL.cost FROM tbl_clothing CL WHERE ((CAST(sku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%"
                        + array[i] + "%') OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%')) AND CL.quantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT CL.sku, (SELECT B.brandName + ' ' + CLO.size + ' ' + CLO.colour + ' ' + CLO.gender + "
                        + "' ' + CLO.style + ' ' + CLO.comments AS itemDescription FROM tbl_clothing CLO JOIN tbl_brand B "
                        + "ON CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS itemDescription, (SELECT L.locationName "
                        + "FROM tbl_clothing CLO JOIN tbl_location L ON CLO.locationID = L.locationID WHERE CLO.sku = CL.sku) "
                        + "AS locationName, CL.quantity, CL.price, CL.cost FROM tbl_clothing CL WHERE ((CAST(sku AS VARCHAR) "
                        + "LIKE '%" + array[i] + "%' OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%"
                        + array[i] + "%') OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%')) AND CL.quantity > " + quantity + ") ";
                }
            }
            return sqlCmd;
        }
        //Returns string for search clubs
        private string ReturnStringSearchForClubs(ArrayList array, int quantity)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT C.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + "
                        + "CLU.clubType + ' ' + CLU.shaftSpec + ' ' + CLU.shaftFlex + ' ' + CLU.dexterity AS itemDescription "
                        + "FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN tbl_model M ON CLU.modelID = "
                        + "M.modelID  WHERE CLU.sku = C.sku) AS itemDescription, (SELECT L.locationName FROM tbl_clubs "
                        + "CLU JOIN tbl_location L ON CLU.locationID = L.locationID WHERE CLU.sku = C.sku) AS locationName, "
                        + "C.quantity,	C.price, C.cost FROM tbl_clubs C WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                        + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                        + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                        + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%')) AND C.quantity > " + quantity + " ";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT C.sku, (SELECT B.brandName + ' ' + M.modelName + ' ' + CLU.clubSpec + ' ' + "
                        + "CLU.clubType + ' ' + CLU.shaftSpec + ' ' + CLU.shaftFlex + ' ' + CLU.dexterity AS itemDescription "
                        + "FROM tbl_clubs CLU JOIN tbl_brand B ON CLU.brandID = B.brandID JOIN tbl_model M ON CLU.modelID = "
                        + "M.modelID  WHERE CLU.sku = C.sku) AS itemDescription, (SELECT L.locationName FROM tbl_clubs "
                        + "CLU JOIN tbl_location L ON CLU.locationID = L.locationID WHERE CLU.sku = C.sku) AS locationName, "
                        + "C.quantity,	C.price, C.cost FROM tbl_clubs C WHERE ((CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%' "
                        + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%') "
                        + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%') "
                        + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%')) AND C.quantity > " + quantity + ") ";
                }
            }
            return sqlCmd;
        }

        public List<Items> ReturnInvoiceItemsFromSearchStringAndQuantity(string searchText, bool zeroQuantity)
        {
            int quantity = 0;
            if (zeroQuantity)
            {
                quantity = -1;
            }
            string sqlCmd = ReturnItemsFromSearchString(searchText, quantity);
            return ConvertFromDataTableToItems(dbc.returnDataTableData(sqlCmd));
        }
        public List<Items> ReturnInvoiceItemsFromSearchStringForSale(string searchText)
        {
            string sqlCmd = ReturnItemsFromSearchString(searchText, -1);
            return ConvertFromDataTableToItems(dbc.returnDataTableData(sqlCmd));
        }
        //public List<Items> ReturnInvoiceItemsInCartbasedOnInvoiceNumber(string invoice)
        //{
        //    string sqlCmd = "";

        //    object[][] parms = { };
        //}
        public DataTable ReturnDropDownForBrand()
        {
            string sqlCmd = "SELECT brandID, brandName FROM "
                + "tbl_brand ORDER BY brandName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public DataTable ReturnDropDownForModel()
        {
            string sqlCmd = "SELECT modelID, modelName FROM "
                + "tbl_model ORDER BY modelName";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public DataTable ReturnDropDownForItemType()
        {
            string sqlCmd = "SELECT typeID, typeDescription FROM "
                + "tbl_itemType ORDER BY typeDescription";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
    }
}