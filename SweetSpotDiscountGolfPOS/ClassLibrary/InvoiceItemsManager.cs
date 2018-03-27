using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class InvoiceItemsManager
    {
        DatabaseCalls dbc = new DatabaseCalls();
        private List<InvoiceItems> ConvertFromDataTableToInvoiceItems(DataTable dt)
        {
            List<InvoiceItems> invoiceItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                sku = row.Field<int>("sku"),
                itemQuantity = row.Field<int>("itemQuantity"),
                itemCost = row.Field<double>("itemCost"),
                itemPrice = row.Field<double>("itemPrice"),
                itemDiscount = row.Field<double>("itemDiscount"),
                itemRefund = row.Field<double>("itemRefund"),
                percentage = row.Field<bool>("percentage")
            }).ToList();
            return invoiceItems;
        }
        private List<InvoiceItems> ConvertFromDataTableToInvoiceItemsWithDescription(DataTable dt)
        {
            List<InvoiceItems> invoiceItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                sku = row.Field<int>("sku"),
                itemDescription = row.Field<string>("itemDescription"),
                itemQuantity = row.Field<int>("itemQuantity"),
                itemCost = row.Field<double>("itemCost"),
                itemPrice = row.Field<double>("itemPrice"),
                itemDiscount = row.Field<double>("itemDiscount"),
                itemRefund = row.Field<double>("itemRefund"),
                percentage = row.Field<bool>("percentage")
            }).ToList();
            return invoiceItems;
        }
        private List<InvoiceItems> ConvertFromDataTableToInvoiceItemsCurrentSalesItems(DataTable dt)
        {
            List<InvoiceItems> invoiceItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                sku = row.Field<int>("sku"),
                itemDescription = row.Field<string>("itemDescription"),
                itemQuantity = row.Field<int>("itemQuantity"),
                itemCost = row.Field<double>("itemCost"),
                itemPrice = row.Field<double>("itemPrice"),
                itemDiscount = row.Field<double>("itemDiscount"),
                itemRefund = row.Field<double>("itemRefund"),
                percentage = row.Field<bool>("percentage"),
                typeID = row.Field<int>("typeID"),
                tradeIn = row.Field<bool>("tradeIn")
            }).ToList();
            return invoiceItems;
        }
        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //Returns list of InvoiceItems based on an Invoice Number
        public List<InvoiceItems> ReturnInvoiceItems(string invoice)
        {
            string sqlCmd = "SELECT II.invoiceNum, II.invoiceSubNum, II.sku, (SELECT B.brandName + ' ' + M.modelName "
                + "+ ' ' + C.clubSpec + ' ' + C.clubType + ' ' + C.shaftSpec + ' ' + C.shaftFlex + ' ' + C.dexterity AS "
                + "itemDescription FROM tbl_clubs C JOIN tbl_brand B ON C.brandID = B.brandID JOIN tbl_model M ON "
                + "C.modelID = M.modelID WHERE sku = II.sku) AS itemDescription, II.itemQuantity, II.itemCost, II.itemPrice, "
                + "II.itemDiscount, II.itemRefund, II.percentage FROM tbl_invoiceItem II WHERE II.invoiceNum = @invoiceNum "
                + "AND II.invoiceSubNum = @invoiceSubNum UNION SELECT IR.invoiceNum, IR.invoiceSubNum, IR.sku, (SELECT "
                + "B.brandName + ' ' + M.modelName + ' ' + C.clubSpec + ' ' + C.clubType + ' ' + C.shaftSpec + ' ' + "
                + "C.shaftFlex + ' ' + C.dexterity AS itemDescription FROM tbl_clubs C JOIN tbl_brand B ON C.brandID = "
                + "B.brandID JOIN tbl_model M ON C.modelID = M.modelID WHERE sku = IR.sku) AS itemDescription, IR.itemQuantity, "
                + "IR.itemCost, IR.itemPrice, IR.itemDiscount, IR.itemRefund, IR.percentage FROM tbl_invoiceItemReturns IR "
                + "WHERE IR.invoiceNum = @invoiceNum AND IR.invoiceSubNum = @invoiceSubNum";

            Object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<InvoiceItems> invoiceItems = ConvertFromDataTableToInvoiceItemsWithDescription(dbc.returnDataTableData(sqlCmd, parms));

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.itemDescription;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + C.clubSpec + ' ' + C.clubType "
                        + "+ ' ' + C.shaftSpec + ' ' + C.shaftFlex + ' ' + C.dexterity AS itemDescription "
                        + "FROM tbl_tempTradeInCartSkus C JOIN tbl_brand B ON C.brandID = B.brandID JOIN "
                        + "tbl_model M ON C.modelID = M.modelID WHERE sku = @sku";
                    Object[][] parms2 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.itemDescription = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms2);
                }
            }

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.itemDescription;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + A.size + ' ' + A.colour "
                        + "+ ' ' + A.comments AS itemDescription "
                        + "FROM tbl_accessories A JOIN tbl_brand B ON A.brandID = B.brandID JOIN tbl_model M ON "
                        + "A.modelID = M.modelID WHERE sku = @sku";
                    Object[][] parms3 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.itemDescription = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms3);
                }
            }

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.itemDescription;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + C.size + ' ' + C.colour + ' ' + C.gender "
                        + "+ ' ' + C.style + ' ' + C.comments AS itemDescription "
                        + "FROM tbl_clothing C JOIN tbl_brand B ON C.brandID = B.brandID "
                        + "WHERE sku = @sku";
                    Object[][] parms4 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.itemDescription = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms4);
                }
            }
            return invoiceItems;
        }
        //public DataTable ReturnInvoiceItemsForSalesCart(int sku)
        //{
        //    string sqlCmd = "SELECT ";
        //    object[][] parms =
        //    {
        //        new object[] { "@sku", sku }
        //    };
        //    return dbc.returnDataTableData(sqlCmd, parms);
        //}
        //Returns string for search accessories
        public string ReturnStringSearchForAccessories(ArrayList array)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT sku FROM tbl_accessories WHERE(CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%'"
                    + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%')"
                    + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%')";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT sku FROM tbl_accessories WHERE(CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%'"
                    + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%')"
                    + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(size, colour, accessoryType, comments) LIKE '%" + array[i] + "%'))";
                }
            }
            return sqlCmd;
        }
        //Returns string for search clothing
        public string ReturnStringSearchForClothing(ArrayList array)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT sku FROM tbl_clothing WHERE(CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%'"
                    + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%')";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT sku FROM tbl_clothing WHERE(CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%'"
                    + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(size, colour, gender, style, comments) LIKE '%" + array[i] + "%'))";
                }
            }
            return sqlCmd;
        }
        //Returns string for search clubs
        public string ReturnStringSearchForClubs(ArrayList array)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT sku FROM tbl_clubs WHERE(CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%'"
                    + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%')"
                    + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%')";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT sku FROM tbl_clubs WHERE(CAST(sku AS VARCHAR) LIKE '%" + array[i] + "%'"
                    + "OR brandID IN(SELECT brandID FROM tbl_brand WHERE brandName LIKE '%" + array[i] + "%')"
                    + "OR modelID IN(SELECT modelID FROM tbl_model WHERE modelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(clubSpec, clubType, shaftSpec, shaftFlex, dexterity) LIKE '%" + array[i] + "%'))";
                }
            }
            return sqlCmd;
        }

        public void InsertItemIntoSalesCart(string invoice, int sku, int qty, string desc, double cost, double price, double discount, bool percent, bool tradeIn, int typeID)
        {
            string sqlCmd = "INSERT INTO tbl_currentSalesItems VALUES(@invoiceNum, @invoiceSubNum, @sku, @qty, "
                + "@desc, @cost, @price, @discount, 0, @percent, @tradeIn, @typeID)";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] },
                new object[] { "@sku", sku },
                new object[] { "@qty", qty },
                new object[] { "@desc", desc},
                new object[] { "@cost", cost },
                new object[] { "@price", price },
                new object[] { "@discount", discount },
                new object[] { "@percent", percent },
                new object[] { "@tradeIn", tradeIn },
                new object[] { "@typeID", typeID}
            };
            ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void RemoveQTYFromInventoryWithSKU(int sku, int typeID, int remainingQTY)
        {
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(typeID) + " SET quantity = @quantity WHERE sku = @sku and typeID = @typeID";

            object[][] parms =
            {
                new object[] { "@sku", sku },
                new object[] { "@typeID", typeID },
                new object[] { "@quantity", remainingQTY }
            };
            ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public string ReturnTableNameFromTypeID(int typeID)
        {
            string sqlCmd = "Select typeDescription from tbl_itemType where typeID = @typeID";

            object[][] parms =
            {
                new object[] { "@typeID", typeID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        public DataTable ReturnItemsInTheCart(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, itemQuantity, itemDescription, "
                + "itemCost, itemPrice, itemDiscount, itemRefund, percentage, tradeIn, typeID FROM "
                + "tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public void ReturnQTYToInventory(int sku, string invoice)
        {
            //gather sku info
            List<InvoiceItems> itemToReturn = ReturnItemDetailsFromCurrentSaleTable(sku, invoice);
            //use info to add quantity back
            AddInventoryBackIntoStock(itemToReturn[0]);
            //remove the sku from currentSales table
            RemoveItemFromCurrentSalesTable(itemToReturn[0]);
        }
        public bool ValidQTY(InvoiceItems ii)
        {
            bool hasValidQty = true;
            int qtyInCurrentStock = ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID), "quantity");
            int qtyOnCurrentSale = ReturnQTYofItem(ii.sku, "tbl_currentSalesItems", "itemQuantity");

            int remaingQTYAvailForSale = qtyInCurrentStock - (ii.itemQuantity - qtyOnCurrentSale);

            if (remaingQTYAvailForSale < 0)
            {
                hasValidQty = false;
            }
            return hasValidQty;
        }
        public void UpdateItemFromCurrentSalesTable(InvoiceItems ii)
        {
            int AccessoryQTY = ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID), "quantity");

            int OrigQTYonSale = ReturnQTYofItem(ii.sku, "tbl_currentSalesItems", "itemQuantity");

            int NewQTYonSale = ii.itemQuantity;

            RemoveQTYFromInventoryWithSKU(ii.sku, ii.typeID, AccessoryQTY - (NewQTYonSale - OrigQTYonSale));

            string sqlCmd = "UPDATE tbl_currentSalesItems SET itemQuantity = @itemQuantity, "
                + "itemDiscount = @itemDiscount, percentage = @percentage WHERE invoiceNum = @invoiceNum "
                + "AND invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@itemQuantity", ii.itemQuantity },
                new object[] { "@itemDiscount", ii.itemDiscount },
                new object[] { "@percentage", ii.percentage },
                new object[] { "@invoiceNum", ii.invoiceNum },
                new object[] { "@invoiceSubNum", ii.invoiceSubNum },
                new object[] { "@sku", ii.sku }
            };

            ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void LoopThroughTheItemsToReturnToInventory(string invoice)
        {
            DataTable dt = ReturnItemsInTheCart(invoice);
            //Loop through DataTable
            foreach(DataRow r in dt.Rows)
            {
                ReturnQTYToInventory(Convert.ToInt32(r["sku"].ToString()), invoice);
            }
        }
        public void RemoveInitialTotalsForTable(string invoice)
        {
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public List<InvoiceItems> ReturnItemsToCalculateTotals(string invoice)
        {
            return ConvertFromDataTableToInvoiceItems(ReturnItemsInTheCart(invoice));
        }

        private int ReturnQTYofItem(int sku, string tbl, string column)
        {
            string sqlCmd = "SELECT " + column + " FROM " + tbl + " WHERE sku = @sku";

            object[][] parms =
            {
                new object[] { "@sku", sku }
            };   
            //Returns the quantity of the searched item
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        private List<InvoiceItems> ReturnItemDetailsFromCurrentSaleTable(int sku, string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, itemDescription, itemQuantity, itemCost, "
                + "itemPrice, itemDiscount, itemRefund, percentage, tradeIn, typeID FROM tbl_currentSalesItems "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] },
                new object[] { "@sku", sku }
            };

            return ConvertFromDataTableToInvoiceItemsCurrentSalesItems(dbc.returnDataTableData(sqlCmd, parms));
        }
        private void AddInventoryBackIntoStock(InvoiceItems ii)
        {
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(ii.typeID) + " SET quantity = "
                + "quantity + @quantity WHERE sku = @sku";

            object[][] parms =
            {
                new object[] { "@quantity", ii.itemQuantity },
                new object[] { "@sku", ii.sku }
            };
            ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveItemFromCurrentSalesTable(InvoiceItems ii)
        {
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@invoiceNum", ii.invoiceNum },
                new object[] { "@invoiceSubNum", ii.invoiceSubNum },
                new object[] { "@sku", ii.sku }
            };
            ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}