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
                description = row.Field<string>("description"),
                quantity = row.Field<int>("quantity"),
                cost = row.Field<double>("Cost"),
                price = row.Field<double>("price"),
                itemDiscount = row.Field<double>("itemDiscount"),
                itemRefund = row.Field<double>("itemRefund"),
                percentage = row.Field<bool>("percentage"),
                typeID = row.Field<int>("typeID"),
                isTradeIn = row.Field<bool>("isTradeIn")
            }).ToList();
            return invoiceItems;
        }
        private List<InvoiceItems> ConvertFromDataTableToReceiptItems(DataTable dt)
        {
            List<InvoiceItems> receiptItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                invoiceNum = row.Field<int>("receiptNum"),
                sku = row.Field<int>("sku"),
                quantity = row.Field<int>("itemQuantity"),
                description = row.Field<string>("description"),
                cost = row.Field<double>("itemCost")
            }).ToList();
            return receiptItems;
        }
        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //Returns list of InvoiceItems based on an Invoice Number
        //THIS CAN BE UPDATED TO GET ALL INFO FROM INVOICE TABLES (remove all joins)
        public List<InvoiceItems> ReturnInvoiceItems(string invoice)
        {
            string sqlCmd = "SELECT II.invoiceNum, II.invoiceSubNum, II.sku, (SELECT B.brandName + ' ' + M.modelName "
                + "+ ' ' + C.clubSpec + ' ' + C.clubType + ' ' + C.shaftSpec + ' ' + C.shaftFlex + ' ' + C.dexterity AS "
                + "description FROM tbl_clubs C JOIN tbl_brand B ON C.brandID = B.brandID JOIN tbl_model M ON "
                + "C.modelID = M.modelID WHERE sku = II.sku) AS description, II.quantity, II.cost, II.price, "
                + "II.itemDiscount, II.itemRefund, II.percentage, II.typeID, II.isTradeIn FROM tbl_invoiceItem II WHERE II.invoiceNum = @invoiceNum "
                + "AND II.invoiceSubNum = @invoiceSubNum UNION SELECT IR.invoiceNum, IR.invoiceSubNum, IR.sku, (SELECT "
                + "B.brandName + ' ' + M.modelName + ' ' + C.clubSpec + ' ' + C.clubType + ' ' + C.shaftSpec + ' ' + "
                + "C.shaftFlex + ' ' + C.dexterity AS description FROM tbl_clubs C JOIN tbl_brand B ON C.brandID = "
                + "B.brandID JOIN tbl_model M ON C.modelID = M.modelID WHERE sku = IR.sku) AS description, IR.quantity, "
                + "IR.cost, IR.price, IR.itemDiscount, IR.itemRefund, IR.percentage, IR.typeID, IR.isTradeIn FROM tbl_invoiceItemReturns IR "
                + "WHERE IR.invoiceNum = @invoiceNum AND IR.invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<InvoiceItems> invoiceItems = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.description;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + C.clubSpec + ' ' + C.clubType "
                        + "+ ' ' + C.shaftSpec + ' ' + C.shaftFlex + ' ' + C.dexterity AS description "
                        + "FROM tbl_tempTradeInCartSkus C JOIN tbl_brand B ON C.brandID = B.brandID JOIN "
                        + "tbl_model M ON C.modelID = M.modelID WHERE sku = @sku";
                    object[][] parms2 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms2);
                }
            }

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.description;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + A.size + ' ' + A.colour "
                        + "+ ' ' + A.comments AS description "
                        + "FROM tbl_accessories A JOIN tbl_brand B ON A.brandID = B.brandID JOIN tbl_model M ON "
                        + "A.modelID = M.modelID WHERE sku = @sku";
                    object[][] parms3 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms3);
                }
            }

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.description;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + C.size + ' ' + C.colour + ' ' + C.gender "
                        + "+ ' ' + C.style + ' ' + C.comments AS description "
                        + "FROM tbl_clothing C JOIN tbl_brand B ON C.brandID = B.brandID "
                        + "WHERE sku = @sku";
                    object[][] parms4 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms4);
                }
            }
            return invoiceItems;
        }
        public List<InvoiceItems> ReturnInvoiceItemsCurrentSale(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, description, quantity, cost, price, "
                + "itemDiscount, itemRefund, percentage, typeID, isTradeIn FROM tbl_currentSalesItems  "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            int num = Convert.ToInt32(invoice.Split('-')[1]);
            int sub = Convert.ToInt32(invoice.Split('-')[2]);

            object[][] parms =
            {
                 new object[] { "@invoiceNum", num },
                 new object[] { "@invoiceSubNum", sub }
            };
            
            return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));
        }
        public List<InvoiceItems> ReturnInvoiceItemsReceipt(string receipt)
        {
            string sqlCmd = "SELECT receiptNum, sku, itemQuantity, description, itemCost "
                + "FROM tbl_receiptItem WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                 new object[] { "@receiptNum", Convert.ToInt32(receipt) }
            };

            return ConvertFromDataTableToReceiptItems(dbc.returnDataTableData(sqlCmd, parms));
        }
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

        public void InsertItemIntoSalesCart(InvoiceItems II)
        {
            string sqlCmd = "INSERT INTO tbl_currentSalesItems VALUES(@invoiceNum, @invoiceSubNum, @sku, @qty, "
                + "@desc, @cost, @price, @itemDiscount, @itemRefund, @percent, @isTradeIn, @typeID)";

            object[][] parms =
            {
                new object[] { "@invoiceNum", II.invoiceNum },
                new object[] { "@invoiceSubNum", II.invoiceSubNum },
                new object[] { "@sku", II.sku },
                new object[] { "@qty", II.quantity },
                new object[] { "@desc", II.description },
                new object[] { "@cost", II.cost },
                new object[] { "@price", II.price },
                new object[] { "@itemDiscount", II.itemDiscount },
                new object[] { "@itemRefund", II.itemRefund },
                new object[] { "@percent", II.percentage },
                new object[] { "@isTradeIn", II.isTradeIn },
                new object[] { "@typeID", II.typeID }
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
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, quantity, description, "
                + "cost, price, itemDiscount, itemRefund, percentage, isTradeIn, typeID FROM "
                + "tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public DataTable ReturnItemsInTheReturnCart(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, quantity, description, "
                + "cost, price, itemDiscount, itemRefund, percentage, isTradeIn, typeID FROM "
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
            int qtyOnCurrentSale = ReturnQTYofItem(ii.sku, "tbl_currentSalesItems", "quantity");

            int remaingQTYAvailForSale = qtyInCurrentStock - (ii.quantity - qtyOnCurrentSale);

            if (remaingQTYAvailForSale < 0)
            {
                hasValidQty = false;
            }
            return hasValidQty;
        }
        public void UpdateItemFromCurrentSalesTable(InvoiceItems ii)
        {
            int AccessoryQTY = ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID), "quantity");

            int OrigQTYonSale = ReturnQTYofItem(ii.sku, "tbl_currentSalesItems", "quantity");

            int NewQTYonSale = ii.quantity;

            RemoveQTYFromInventoryWithSKU(ii.sku, ii.typeID, AccessoryQTY - (NewQTYonSale - OrigQTYonSale));
            UpdateItemFromCurrentSalesTableActualQuery(ii);
        }
        public void UpdateItemFromCurrentSalesTableActualQuery(InvoiceItems ii)
        {
            string sqlCmd = "UPDATE tbl_currentSalesItems SET quantity = @quantity, "
                + "itemDiscount = @itemDiscount, percentage = @percentage WHERE invoiceNum = @invoiceNum "
                + "AND invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@quantity", ii.quantity },
                new object[] { "@itemDiscount", ii.itemDiscount },
                new object[] { "@percentage", ii.percentage },
                new object[] { "@invoiceNum", ii.invoiceNum },
                new object[] { "@invoiceSubNum", ii.invoiceSubNum },
                new object[] { "@sku", ii.sku }
            };

            ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void UpdateItemFromCurrentSalesTableActualQueryForPurchases(InvoiceItems ii)
        {
            string sqlCmd = "UPDATE tbl_currentSalesItems SET cost = @cost, description = @description "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@cost", ii.cost },
                new object[] { "@description", ii.description },
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
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, description, quantity, cost, "
                + "price, itemDiscount, itemRefund, percentage, isTradeIn, typeID FROM tbl_currentSalesItems "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] },
                new object[] { "@sku", sku }
            };

            return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));
        }
        private void AddInventoryBackIntoStock(InvoiceItems ii)
        {
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(ii.typeID) + " SET quantity = "
                + "quantity + @quantity WHERE sku = @sku";

            object[][] parms =
            {
                new object[] { "@quantity", ii.quantity },
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

        //Search results for A Return
        public List<InvoiceItems> ReturnInvoiceItemsFromProcessedSalesForReturn(string invoice)
        {
            string sqlCmd = "SELECT I.invoiceNum, I.invoiceSubNum, I.sku, (SELECT SUM(DISTINCT II.quantity) - "
                + "((CASE WHEN SUM(IIR.quantity) IS NULL OR SUM(IIR.quantity) = '' THEN 0 ELSE SUM(IIR.quantity) "
                + "END) + (CASE WHEN SUM(CSI.quantity) IS NULL OR SUM(CSI.quantity) = '' THEN 0 ELSE "
                + "SUM(CSI.quantity) END)) AS quantity FROM tbl_invoiceItem II LEFT JOIN tbl_invoiceItemReturns "
                + "IIR ON II.invoiceNum = IIR.invoiceNum AND II.sku = IIR.sku LEFT JOIN tbl_currentSalesItems CSI "
                + "ON II.invoiceNum = CSI.invoiceNum AND II.sku = CSI.sku WHERE II.invoiceNum = @invoiceNum AND "
                + "II.sku = I.sku GROUP BY II.invoiceNum, II.sku) AS quantity, I.description, (CONCAT((SELECT "
                + "L.locationName AS locationName FROM tbl_accessories A JOIN tbl_location L ON A.locationID = "
                + "L.locationID WHERE A.sku = I.sku), (SELECT L.locationName AS locationName FROM tbl_clothing CL "
                + "JOIN tbl_location L ON CL.locationID = L.locationID WHERE CL.sku = I.sku), (SELECT L.locationName "
                + "AS locationName FROM tbl_clubs C JOIN tbl_location L ON C.locationID = L.locationID WHERE C.sku = "
                + "I.sku))) AS locationName, I.cost, I.price, I.itemDiscount, I.itemRefund, I.percentage, I.typeID, "
                + "I.isTradeIn FROM tbl_invoiceItem I WHERE invoiceNum = @invoiceNum and invoiceSubNum = 1";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] }
            };

            return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));
        }
        public InvoiceItems ReturnInvoiceItemForReturnProcess(int sku, string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, quantity, cost, price, "
                + "itemDiscount, itemRefund, percentage, description, typeID, isTradeIn FROM "
                + "tbl_invoiceItem WHERE invoiceNum = @invoiceNum AND invoiceSubNum = 1 "
                + "AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@sku", sku }
            };

            InvoiceItems II = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms))[0];
            II.quantity = ReturnQTYofItem(II.sku, "tbl_" + ReturnTableNameFromTypeID(II.typeID), "quantity");
            return II;
        }
        public void DoNotReturnTheItemOnReturn(InvoiceItems ii)
        {
            RemoveItemFromCurrentSalesTable(ii);
        }
        public InvoiceItems ReturnSkuFromCurrentSalesUsingSKU(int sku, string invoice)
        {
            return ReturnItemDetailsFromCurrentSaleTable(sku, invoice)[0];
        }
        public int ReturnCurrentQuantityOfItem(InvoiceItems ii)
        {
            return ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID), "quantity");
        }
        public bool ItemAlreadyInCart(InvoiceItems ii)
        {
            bool itemInCart = false;

            string sqlCmd = "SELECT sku FROM tbl_currentSalesItems WHERE "
                + "invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum "
                + "AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@invoiceNum", ii.invoiceNum },
                new object[] { "@invoiceSubNum", ii.invoiceSubNum },
                new object[] { "@sku", ii.sku }
            };

            DataTable dt = dbc.returnDataTableData(sqlCmd, parms);
            if(dt.Rows.Count > 0)
            {
                itemInCart = true;
            }
            return itemInCart;
        }
    }
}