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
        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //Returns list of InvoiceItems based on an Invoice Number
        //THIS CAN BE UPDATED TO GET ALL INFO FROM INVOICE TABLES (remove all joins)
        public List<InvoiceItems> ReturnInvoiceItems(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItems";
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
            //List<InvoiceItems> invoiceItems = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

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
                    //ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms2, objPageDetails, strQueryName);
                }
            }

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.description;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + A.size + ' ' + A.colour AS description "
                        + "FROM tbl_accessories A JOIN tbl_brand B ON A.brandID = B.brandID JOIN tbl_model M ON "
                        + "A.modelID = M.modelID WHERE sku = @sku";
                    object[][] parms3 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms3);
                    //ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms3, objPageDetails, strQueryName);
                }
            }

            foreach (InvoiceItems ii in invoiceItems)
            {
                string transferDescription = ii.description;
                if (transferDescription == null)
                {
                    sqlCmd = "SELECT B.brandName + ' ' + C.size + ' ' + C.colour + ' ' + C.gender "
                        + "+ ' ' + C.style AS description "
                        + "FROM tbl_clothing C JOIN tbl_brand B ON C.brandID = B.brandID "
                        + "WHERE sku = @sku";
                    object[][] parms4 =
                    {
                        new object[] { "@sku", ii.sku }
                    };
                    ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms4);
                    //ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms4, objPageDetails, strQueryName);
                }
            }
            return invoiceItems;
        }
        public List<InvoiceItems> ReturnInvoiceItemsCurrentSale(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsCurrentSale";
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
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<InvoiceItems> ReturnInvoiceItemsReceipt(string receipt, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsReceipt";
            string sqlCmd = "SELECT receiptNum, sku, itemQuantity, description, itemCost "
                + "FROM tbl_receiptItem WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                 new object[] { "@receiptNum", Convert.ToInt32(receipt) }
            };

            return ConvertFromDataTableToReceiptItems(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToReceiptItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
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

        public void InsertItemIntoSalesCart(InvoiceItems II, object[] objPageDetails)
        {
            string strQueryName = "InsertItemIntoSalesCart";
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
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void RemoveQTYFromInventoryWithSKU(int sku, int typeID, int remainingQTY, object[] objPageDetails)
        {
            string strQueryName = "RemoveQTYFromInventoryWithSKU";
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(typeID, objPageDetails) + " SET quantity = @quantity WHERE sku = @sku and typeID = @typeID";

            object[][] parms =
            {
                new object[] { "@sku", sku },
                new object[] { "@typeID", typeID },
                new object[] { "@quantity", remainingQTY }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnTableNameFromTypeID(int typeID, object[] objPageDetails)
        {
            string strQueryName = "ReturnTableNameFromTypeID";
            string sqlCmd = "Select typeDescription from tbl_itemType where typeID = @typeID";

            object[][] parms =
            {
                new object[] { "@typeID", typeID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnItemsInTheCart(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemsInTheCart";
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, quantity, description, "
                + "cost, price, itemDiscount, itemRefund, percentage, isTradeIn, typeID FROM "
                + "tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnItemsInTheReturnCart(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemsInTheReturnCart";
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, sku, quantity, description, "
                + "cost, price, itemDiscount, itemRefund, percentage, isTradeIn, typeID FROM "
                + "tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void ReturnQTYToInventory(int sku, string invoice, object[] objPageDetails)
        {
            //gather sku info
            List<InvoiceItems> itemToReturn = ReturnItemDetailsFromCurrentSaleTable(sku, invoice, objPageDetails);
            //use info to add quantity back
            AddInventoryBackIntoStock(itemToReturn[0], objPageDetails);
            //remove the sku from currentSales table
            RemoveItemFromCurrentSalesTable(itemToReturn[0], objPageDetails);
        }
        public bool ValidQTY(InvoiceItems ii, object[] objPageDetails)
        {
            bool hasValidQty = true;
            int qtyInCurrentStock = ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID, objPageDetails), "quantity", objPageDetails);
            int qtyOnCurrentSale = ReturnQTYofItem(ii.sku, "tbl_currentSalesItems", "quantity", objPageDetails);

            int remaingQTYAvailForSale = qtyInCurrentStock - (ii.quantity - qtyOnCurrentSale);

            if (remaingQTYAvailForSale < 0)
            {
                hasValidQty = false;
            }
            return hasValidQty;
        }
        public void UpdateItemFromCurrentSalesTable(InvoiceItems ii, object[] objPageDetails)
        {
            int AccessoryQTY = ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID, objPageDetails), "quantity", objPageDetails);

            int OrigQTYonSale = ReturnQTYofItem(ii.sku, "tbl_currentSalesItems", "quantity", objPageDetails);

            int NewQTYonSale = ii.quantity;

            RemoveQTYFromInventoryWithSKU(ii.sku, ii.typeID, AccessoryQTY - (NewQTYonSale - OrigQTYonSale), objPageDetails);
            UpdateItemFromCurrentSalesTableActualQuery(ii, objPageDetails);
        }
        public void UpdateItemFromCurrentSalesTableActualQuery(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "UpdateItemFromCurrentSalesTableActualQuery";
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

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public void UpdateItemFromCurrentSalesTableActualQueryForPurchases(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "UpdateItemFromCurrentSalesTableActualQueryForPurchases";
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

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public void LoopThroughTheItemsToReturnToInventory(string invoice, object[] objPageDetails)
        {
            DataTable dt = ReturnItemsInTheCart(invoice, objPageDetails);
            //Loop through DataTable
            foreach(DataRow r in dt.Rows)
            {
                ReturnQTYToInventory(Convert.ToInt32(r["sku"].ToString()), invoice, objPageDetails);
            }
        }
        public void RemoveInitialTotalsForTable(string invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveInitialTotalsForTable";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public List<InvoiceItems> ReturnItemsToCalculateTotals(string invoice, object[] objPageDetails)
        {
            return ConvertFromDataTableToInvoiceItems(ReturnItemsInTheCart(invoice, objPageDetails));
        }

        private int ReturnQTYofItem(int sku, string tbl, string column, object[] objPageDetails)
        {
            string strQueryName = "ReturnQTYofItem";
            string sqlCmd = "SELECT " + column + " FROM " + tbl + " WHERE sku = @sku";

            object[][] parms =
            {
                new object[] { "@sku", sku }
            };   
            //Returns the quantity of the searched item
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<InvoiceItems> ReturnItemDetailsFromCurrentSaleTable(int sku, string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemDetailsFromCurrentSaleTable";
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
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private void AddInventoryBackIntoStock(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "AddInventoryBackIntoStock";
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(ii.typeID, objPageDetails) + " SET quantity = "
                + "quantity + @quantity WHERE sku = @sku";

            object[][] parms =
            {
                new object[] { "@quantity", ii.quantity },
                new object[] { "@sku", ii.sku }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveItemFromCurrentSalesTable(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "RemoveItemFromCurrentSalesTable";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum AND sku = @sku";

            object[][] parms =
            {
                new object[] { "@invoiceNum", ii.invoiceNum },
                new object[] { "@invoiceSubNum", ii.invoiceSubNum },
                new object[] { "@sku", ii.sku }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //Search results for A Return
        public List<InvoiceItems> ReturnInvoiceItemsFromProcessedSalesForReturn(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromProcessedSalesForReturn";
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
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public InvoiceItems ReturnInvoiceItemForReturnProcess(int sku, string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemForReturnProcess";
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
            //InvoiceItems II = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName))[0];
            II.quantity = ReturnQTYofItem(II.sku, "tbl_" + ReturnTableNameFromTypeID(II.typeID, objPageDetails), "quantity", objPageDetails);
            return II;
        }
        public void DoNotReturnTheItemOnReturn(InvoiceItems ii, object[] objPageDetails)
        {
            RemoveItemFromCurrentSalesTable(ii, objPageDetails);
        }
        public InvoiceItems ReturnSkuFromCurrentSalesUsingSKU(int sku, string invoice, object[] objPageDetails)
        {
            return ReturnItemDetailsFromCurrentSaleTable(sku, invoice, objPageDetails)[0];
        }
        public int ReturnCurrentQuantityOfItem(InvoiceItems ii, object[] objPageDetails)
        {
            return ReturnQTYofItem(ii.sku, "tbl_" + ReturnTableNameFromTypeID(ii.typeID, objPageDetails), "quantity", objPageDetails);
        }
        public bool ItemAlreadyInCart(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "ItemAlreadyInCart";
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
            //DataTable dt = dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
            if (dt.Rows.Count > 0)
            {
                itemInCart = true;
            }
            return itemInCart;
        }
    }
}