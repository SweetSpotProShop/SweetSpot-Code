using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SweetShop;

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
                intInvoiceItemID = row.Field<int>("intInvoiceItemID"),
                intInvoiceID = row.Field<int>("intInvoiceID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                varItemDescription = row.Field<string>("varItemDescription"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemCost = row.Field<double>("fltItemCost"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemDiscount = row.Field<double>("fltItemDiscount"),
                fltItemRefund = row.Field<double>("fltItemRefund"),
                bitIsDiscountPercent = row.Field<bool>("bitIsDiscountPercent"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                bitIsClubTradeIn = row.Field<bool>("bitIsClubTradeIn")
            }).ToList();
            return invoiceItems;
        }
        private List<InvoiceItems> ConvertFromDataTableToReceiptItems(DataTable dt)
        {
            List<InvoiceItems> receiptItems = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                intInvoiceItemID = row.Field<int>("intReceiptItemID"),
                intInvoiceID = row.Field<int>("intReceiptID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                varItemDescription = row.Field<string>("varItemDescription"),
                fltItemCost = row.Field<double>("intItemCost")
            }).ToList();
            return receiptItems;
        }

        private List<InvoiceItems> ConvertFromDataTableToAvaiableToReturnItems(DataTable dt, string invoiceNumber)
        {
            List<InvoiceItems> invoiceItem = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                intInvoiceItemID = row.Field<int>("intInvoiceItemID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                intItemQuantity = row.Field<int>("intItemQuantity")
            }).ToList();
            foreach (InvoiceItems ii in invoiceItem)
            {
                InvoiceItems temp = GatherRemainingReturnItemInformation(ii, invoiceNumber);
                ii.varSku = temp.varSku;
                ii.varItemDescription = temp.varItemDescription;
                ii.fltItemPrice = temp.fltItemPrice;
                ii.fltItemCost = temp.fltItemCost;
                ii.intItemTypeID = temp.intItemTypeID;
                ii.varAdditionalInformation = temp.varAdditionalInformation;
                
            }
            return invoiceItem;
        }

        private void ExecuteNonReturnQuery(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //Returns list of InvoiceItems based on an Invoice Number
        //THIS CAN BE UPDATED TO GET ALL INFO FROM INVOICE TABLES (remove all joins)
        public List<InvoiceItems> ReturnInvoiceItems(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItems";
            string sqlCmd = "SELECT II.intInvoiceItemID, II.intInvoiceID, II.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories "
                + "A WHERE A.intInventoryID = II.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = II.intInventoryID) "
                + "WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = II.intInventoryID) THEN (SELECT CL.varSku FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = II.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = II.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = II.intInventoryID) END AS varSku, "
                + "II.varItemDescription, II.intItemQuantity, II.fltItemCost, II.fltItemPrice, II.fltItemDiscount, II.fltItemRefund, "
                + "II.bitIsDiscountPercent, II.intItemTypeID, II.bitIsClubTradeIn FROM tbl_invoiceItem II WHERE II.intInvoiceID = @intInvoiceID UNION "
                + "SELECT IR.intInvoiceItemID, IR.intInvoiceID, IR.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE "
                + "A.intInventoryID = IR.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = IR.intInventoryID) WHEN "
                + "EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = IR.intInventoryID) THEN (SELECT CL.varSku FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = IR.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = IR.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = IR.intInventoryID) END AS varSku, "
                + "IR.varItemDescription, IR.intItemQuantity, IR.fltItemCost, IR.fltItemPrice, IR.fltItemDiscount, IR.fltItemRefund, "
                + "IR.bitIsDiscountPercent, IR.intItemTypeID, IR.bitIsClubTradeIn FROM tbl_invoiceItemReturns IR WHERE IR.intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            List<InvoiceItems> invoiceItems = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));
            //List<InvoiceItems> invoiceItems = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            //foreach (InvoiceItems ii in invoiceItems)
            //{
            //    string transferDescription = ii.varItemDescription;
            //    if (transferDescription == null)
            //    {
            //        sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + C.clubSpec + ' ' + C.clubType "
            //            + "+ ' ' + C.shaftSpec + ' ' + C.shaftFlex + ' ' + C.dexterity AS description "
            //            + "FROM tbl_tempTradeInCartSkus C JOIN tbl_brand B ON C.brandID = B.brandID JOIN "
            //            + "tbl_model M ON C.modelID = M.modelID WHERE sku = @sku";
            //        object[][] parms2 =
            //        {
            //            new object[] { "@sku", ii.intInventoryID }
            //        };
            //        ii.varItemDescription = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms2);
            //        //ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms2, objPageDetails, strQueryName);
            //    }
            //}

            //foreach (InvoiceItems ii in invoiceItems)
            //{
            //    string transferDescription = ii.varItemDescription;
            //    if (transferDescription == null)
            //    {
            //        sqlCmd = "SELECT B.brandName + ' ' + M.modelName + ' ' + A.size + ' ' + A.colour AS description "
            //            + "FROM tbl_accessories A JOIN tbl_brand B ON A.brandID = B.brandID JOIN tbl_model M ON "
            //            + "A.modelID = M.modelID WHERE sku = @sku";
            //        object[][] parms3 =
            //        {
            //            new object[] { "@sku", ii.intInventoryID }
            //        };
            //        ii.varItemDescription = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms3);
            //        //ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms3, objPageDetails, strQueryName);
            //    }
            //}

            //foreach (InvoiceItems ii in invoiceItems)
            //{
            //    string transferDescription = ii.varItemDescription;
            //    if (transferDescription == null)
            //    {
            //        sqlCmd = "SELECT B.brandName + ' ' + C.size + ' ' + C.colour + ' ' + C.gender "
            //            + "+ ' ' + C.style AS description "
            //            + "FROM tbl_clothing C JOIN tbl_brand B ON C.brandID = B.brandID "
            //            + "WHERE sku = @sku";
            //        object[][] parms4 =
            //        {
            //            new object[] { "@sku", ii.intInventoryID }
            //        };
            //        ii.varItemDescription = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms4);
            //        //ii.description = dbc.MakeDataBaseCallToReturnString(sqlCmd, parms4, objPageDetails, strQueryName);
            //    }
            //}
            return invoiceItems;
        }
        public List<InvoiceItems> ReturnInvoiceItemsCurrentSale(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsCurrentSale";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, CSI.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A "
                + "WHERE A.intInventoryID = CSI.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = CSI.intInventoryID"
                + ") WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = CSI.intInventoryID) THEN (SELECT CL.varSku "
                + "FROM tbl_clothing CL WHERE CL.intInventoryID = CSI.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = CSI.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = CSI.intInventoryID) END AS "
                + "varSku, varItemDescription, intItemQuantity, fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, "
                + "intItemTypeID, bitIsClubTradeIn FROM tbl_currentSalesItems CSI WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };
            
            return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<InvoiceItems> ReturnInvoiceItemsReceipt(int receiptID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsReceipt";
            string sqlCmd = "SELECT intReceiptItemID, intReceiptID, intInventoryID, intItemQuantity, varItemDescription, fltItemCost "
                + "FROM tbl_receiptItem WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
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
                    sqlCmd = "SELECT intInventoryID FROM tbl_accessories WHERE(varSku LIKE '%" + array[i] + "%'"
                    + "OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%')"
                    + "OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(varSize, varColour, varTypeOfAccessory, varAdditionalInformation) LIKE '%" + array[i] + "%')";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT intInventoryID FROM tbl_accessories WHERE(varSku LIKE '%" + array[i] + "%'"
                    + "OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%')"
                    + "OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(varSize, varColour, varTypeOfAccessory, varAdditionalInformation) LIKE '%" + array[i] + "%'))";
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
                    sqlCmd = "SELECT intInventoryID FROM tbl_clothing WHERE(varSku LIKE '%" + array[i] + "%'"
                    + "OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(varSize, varColour, varGender, varStyle, varAdditionalInformation) LIKE '%" + array[i] + "%')";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT intInventoryID FROM tbl_clothing WHERE(varSku LIKE '%" + array[i] + "%'"
                    + "OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(varSize, varColour, varGender, varStyle, varAdditionalInformation) LIKE '%" + array[i] + "%'))";
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
                    sqlCmd = "SELECT intInventoryID FROM tbl_clubs WHERE(varSku LIKE '%" + array[i] + "%'"
                    + "OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%')"
                    + "OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(varClubSpecification, varTypeOfClub, varShaftSpecification, varShaftFlexability, "
                    + "varClubDexterity) LIKE '%" + array[i] + "%')";
                }
                else
                {
                    sqlCmd += "INTERSECT(SELECT intInventoryID FROM tbl_clubs WHERE(varSku LIKE '%" + array[i] + "%'"
                    + "OR intBrandID IN(SELECT intBrandID FROM tbl_brand WHERE varBrandName LIKE '%" + array[i] + "%')"
                    + "OR intModelID IN(SELECT intModelID FROM tbl_model WHERE varModelName LIKE '%" + array[i] + "%')"
                    + "OR CONCAT(varClubSpecification, varTypeOfClub, varShaftSpecification, varShaftFlexability, "
                    + "varClubDexterity) LIKE '%" + array[i] + "%'))";
                }
            }
            return sqlCmd;
        }

        public void InsertItemIntoSalesCart(InvoiceItems II, object[] objPageDetails)
        {
            string strQueryName = "InsertItemIntoSalesCart";
            string sqlCmd = "INSERT INTO tbl_currentSalesItems VALUES(@intInvoiceID, @intInventoryID, @intItemQuantity, "
                + "@varItemDescription, @fltItemCost, @fltItemPrice, @fltItemDiscount, @fltItemRefund, @bitIsDiscountPercent, "
                + "@bitIsClubTradeIn, @intItemTypeID)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", II.intInvoiceID },
                new object[] { "@intInventoryID", II.intInventoryID },
                new object[] { "@intItemQuantity", II.intItemQuantity },
                new object[] { "@VarItemDescription", II.varItemDescription },
                new object[] { "@fltItemCost", II.fltItemCost },
                new object[] { "@fltItemPrice", II.fltItemPrice },
                new object[] { "@fltItemDiscount", II.fltItemDiscount },
                new object[] { "@fltItemRefund", II.fltItemRefund },
                new object[] { "@bitIsDiscountPercent", II.bitIsDiscountPercent },
                new object[] { "@bitIsClubTradeIn", II.bitIsClubTradeIn },
                new object[] { "@intItemTypeID", II.intItemTypeID }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void RemoveQTYFromInventoryWithSKU(int inventoryID, int itemTypeID, int remainingQTY, object[] objPageDetails)
        {
            string strQueryName = "RemoveQTYFromInventoryWithSKU";
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(itemTypeID, objPageDetails) + " SET intQuantity = @intQuantity WHERE intInventoryID "
                + "= @intInventoryID and intItemTypeID = @intItemTypeID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intItemTypeID", itemTypeID },
                new object[] { "@intQuantity", remainingQTY }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public string ReturnTableNameFromTypeID(int itemTypeID, object[] objPageDetails)
        {
            string strQueryName = "ReturnTableNameFromTypeID";
            string sqlCmd = "SELECT varItemTypeName FROM tbl_itemType WHERE intItemTypeID = @intItemTypeID";
            object[][] parms =
            {
                new object[] { "@intItemTypeID", itemTypeID }
            };

            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ReturnItemsInTheCart(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemsInTheCart";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, intInventoryID, intItemQuantity, varItemDescription, "
                + "fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, bitIsClubTradeIn, "
                + "intItemTypeID FROM tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
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
        public void ReturnQTYToInventory(int invoiceItemID, object[] objPageDetails)
        {
            //gather sku info
            List<InvoiceItems> itemToReturn = ReturnItemDetailsFromCurrentSaleTable(invoiceItemID, objPageDetails);
            //use info to add quantity back
            AddInventoryBackIntoStock(itemToReturn[0], objPageDetails);
            //remove the sku from currentSales table
            RemoveItemFromCurrentSalesTable(itemToReturn[0], objPageDetails);
        }
        public bool ValidQTY(InvoiceItems ii, object[] objPageDetails)
        {
            bool hasValidQty = true;
            int qtyInCurrentStock = ReturnQTYofItem(ii.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(ii.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
            int qtyOnCurrentSale = ReturnQTYofItem(ii.intInventoryID, "tbl_currentSalesItems", "intItemQuantity", objPageDetails);

            int remaingQTYAvailForSale = qtyInCurrentStock - (ii.intItemQuantity - qtyOnCurrentSale);

            if (remaingQTYAvailForSale < 0)
            {
                hasValidQty = false;
            }
            return hasValidQty;
        }
        public void UpdateItemFromCurrentSalesTable(InvoiceItems ii, object[] objPageDetails)
        {
            int AccessoryQTY = ReturnQTYofItem(ii.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(ii.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);

            int OrigQTYonSale = ReturnQTYofItem(ii.intInventoryID, "tbl_currentSalesItems", "intItemQuantity", objPageDetails);

            int NewQTYonSale = ii.intItemQuantity;

            RemoveQTYFromInventoryWithSKU(ii.intInventoryID, ii.intItemTypeID, AccessoryQTY - (NewQTYonSale - OrigQTYonSale), objPageDetails);
            UpdateItemFromCurrentSalesTableActualQuery(ii, objPageDetails);
        }
        public void UpdateItemFromCurrentSalesTableActualQuery(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "UpdateItemFromCurrentSalesTableActualQuery";
            string sqlCmd = "UPDATE tbl_currentSalesItems SET intItemQuantity = @intItemQuantity, fltItemDiscount = @fltItemDiscount, "
                + "bitIsDiscountPercent = @bitIsDiscountPercent WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intItemQuantity", ii.intItemQuantity },
                new object[] { "@fltItemDiscount", ii.fltItemDiscount },
                new object[] { "@bitIsDiscountPercent", ii.bitIsDiscountPercent },
                new object[] { "@intInventoryID", ii.intInventoryID }
            };

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public void UpdateItemFromCurrentSalesTableActualQueryForPurchases(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "UpdateItemFromCurrentSalesTableActualQueryForPurchases";
            string sqlCmd = "UPDATE tbl_currentSalesItems SET fltItemCost = @fltItemCost, varItemDescription = @varItemDescription "
                + "WHERE intInvoiceID = @intInvoiceID AND intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@fltItemCost", ii.fltItemCost },
                new object[] { "@varItemDescription", ii.varItemDescription },
                new object[] { "@intInvoiceItemID", ii.intInvoiceItemID },
                new object[] { "@intInvoiceID", ii.intInvoiceID },
            };

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        public void LoopThroughTheItemsToReturnToInventory(int invoiceID, object[] objPageDetails)
        {
            DataTable dt = ReturnItemsInTheCart(invoiceID, objPageDetails);
            //Loop through DataTable
            foreach(DataRow r in dt.Rows)
            {
                ReturnQTYToInventory(Convert.ToInt32(r["intInvoiceItemID"].ToString()), objPageDetails);
            }
        }
        public void RemoveInitialTotalsForTable(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveInitialTotalsForTable";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public List<InvoiceItems> ReturnItemsToCalculateTotals(int invoiceID, object[] objPageDetails)
        {
            return ConvertFromDataTableToInvoiceItems(ReturnItemsInTheCart(invoiceID, objPageDetails));
        }

        private int ReturnQTYofItem(int inventoryID, string tbl, string column, object[] objPageDetails)
        {
            string strQueryName = "ReturnQTYofItem";
            string sqlCmd = "SELECT " + column + " FROM " + tbl + " WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID }
            };   
            //Returns the quantity of the searched item
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<InvoiceItems> ReturnItemDetailsFromCurrentSaleTable(int invoiceItemID, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemDetailsFromCurrentSaleTable";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE "
                + "A.intInventoryID = I.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) WHEN "
                + "EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) THEN (SELECT CL.varSku FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = I.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) END AS varSku, "
                + "varItemDescription, intItemQuantity, fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, "
                + "bitIsClubTradeIn, intItemTypeID FROM tbl_currentSalesItems I WHERE intInvoiceItemID = @intInvoiceItemID";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID }
            };

            return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms));
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private void AddInventoryBackIntoStock(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "AddInventoryBackIntoStock";
            string sqlCmd = "UPDATE tbl_" + ReturnTableNameFromTypeID(ii.intItemTypeID, objPageDetails) + " SET intQuantity = "
                + "intQuantity + @intQuantity WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intQuantity", ii.intItemQuantity },
                new object[] { "@intInventoryID", ii.intInventoryID }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveItemFromCurrentSalesTable(InvoiceItems invoiceItem, object[] objPageDetails)
        {
            string strQueryName = "RemoveItemFromCurrentSalesTable";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItem.intInvoiceItemID }
            };
            ExecuteNonReturnQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public bool CheckForItemsInTransaction(object transaction)
        {
            bool itemsInTransaction = false;

            //if (transaction is PurchaseOrder)
            //{
            //    PurchaseOrder purchaseOrder = transaction as PurchaseOrder;
            //    itemsInTransaction = CheckItemInPurchaseOrder(purchaseOrder);
            //}
            //else if (transaction is Invoice)
            if (transaction is Invoice)
            {
                Invoice invoice = transaction as Invoice;
                itemsInTransaction = CheckItemInInvoice(invoice);
            }
            //else if (transaction is Receipt)
            //{
            //    Receipt receipt = transaction as Receipt;
            //    itemsInTransaction = CheckItemInReceipt(receipt);
            //}

            return itemsInTransaction;
        }

        //private bool CheckItemInPurchaseOrder(PurchaseOrder purchaseOrder)
        //{
        //    bool itemsInTransaction = false;
        //    if (purchaseOrder.intTransactionTypeID == 5)
        //    {
        //        if (purchaseOrder.lstPurchaseOrderItem.Count > 0)
        //        {
        //            itemsInTransaction = true;
        //        }
        //    }
        //    else if (purchaseOrder.intTransactionTypeID == 8)
        //    {
        //        foreach (PurchaseOrderItem poItem in purchaseOrder.lstPurchaseOrderItem)
        //        {
        //            if (poItem.intReceivedQuantity > 0)
        //            {
        //                itemsInTransaction = true;
        //            }
        //        }
        //    }
        //    return itemsInTransaction;
        //}
        private bool CheckItemInInvoice(Invoice invoice)
        {
            bool itemsInTransaction = false;
            if (invoice.invoiceItems.Count > 0)
            {
                itemsInTransaction = true;
            }
            return itemsInTransaction;
        }
        //private bool CheckItemInReceipt(Receipt receipt)
        //{
        //    bool itemsInTransaction = false;
        //    if (receipt.lstReceiptItem.Count > 0)
        //    {
        //        itemsInTransaction = true;
        //    }
        //    return itemsInTransaction;
        //}

        //Search results for A Return
        public List<InvoiceItems> ReturnInvoiceItemsFromProcessedSalesForReturn(string invoiceNumber, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromProcessedSalesForReturn";
            //string sqlCmd = "SELECT I.intInvoiceID, I.varInvoiceNumber, I.intInvoiceSubNumber, I.intInventoryID, (SELECT SUM(DISTINCT II.intItemQuantity) - "
            //    + "((CASE WHEN SUM(IIR.intItemQuantity) IS NULL OR SUM(IIR.intItemQuantity) = '' THEN 0 ELSE SUM(IIR.intItemQuantity) "
            //    + "END) + (CASE WHEN SUM(CSI.intItemQuantity) IS NULL OR SUM(CSI.intItemQuantity) = '' THEN 0 ELSE "
            //    + "SUM(CSI.intItemQuantity) END)) AS intItemQuantity FROM tbl_invoiceItem II LEFT JOIN tbl_invoiceItemReturns "
            //    + "IIR ON II.intInvoiceID = IIR.intInvoiceID AND II.intInventoryID = IIR.intInventoryID LEFT JOIN tbl_currentSalesItems CSI "
            //    + "ON II.intInvoiceID = CSI.intInvoiceID AND II.intInventoryID = CSI.intInventoryID WHERE II.intInvoiceID = @intInvoiceID AND "
            //    + "II.intInventoryID = I.intInventoryID GROUP BY II.intInvoiceID, II.intInventoryID) AS intItemQuantity, I.varItemDescription, (CONCAT((SELECT "
            //    + "L.varLocationName AS varLocationName FROM tbl_accessories A JOIN tbl_location L ON A.intLocationID = "
            //    + "L.intLocationID WHERE A.intInventoryID = I.intInventoryID), (SELECT L.varLocationName AS varLocationName FROM tbl_clothing CL "
            //    + "JOIN tbl_location L ON CL.intLocationID = L.intLocationID WHERE CL.intInventoryID = I.intInventoryID), (SELECT L.varLocationName "
            //    + "AS varLocationName FROM tbl_clubs C JOIN tbl_location L ON C.intLocationID = L.intLocationID WHERE C.intInventoryID = "
            //    + "I.intInventoryID))) AS varLocationName, I.fltItemCost, I.fltItemPrice, I.fltItemDiscount, I.fltItemRefund, I.bitIsDiscountPercent, I.intItemTypeID, "
            //    + "I.bitIsClubTradeIn FROM tbl_invoiceItem I WHERE varInvoiceNumber = @varInvoiceNum and invoiceSubNum = 1";

            string sqlCmd = "SELECT RI2.intInvoiceItemID, RI2.intInventoryID, (SUM(DISTINCT RI2.intFirstSaleQuantity) - ((CASE WHEN SUM(RIR.intReturnQuantity) IS NULL "
                + "OR SUM(RIR.intReturnQuantity) = '' THEN 0 ELSE SUM(RIR.intReturnQuantity) END) + (CASE WHEN SUM(RIC.intCurrentReturnQuantity) IS NULL OR SUM("
                + "RIC.intCurrentReturnQuantity) = '' THEN 0 ELSE SUM(RIC.intCurrentReturnQuantity) END))) AS intItemQuantity FROM(SELECT RIQ.intInvoiceItemID, "
                + "RIQ.intInventoryID, SUM(RIQ.intItemQuantity) AS intFirstSaleQuantity FROM tbl_invoiceItem RIQ JOIN tbl_invoice RIQ2 ON RIQ.intInvoiceID = "
                + "RIQ2.intInvoiceID WHERE RIQ2.varInvoiceNumber = @varInvoiceNumber AND RIQ2.intInvoiceSubNumber = 1 GROUP BY RIQ.intInvoiceItemID, RIQ.intInventoryID"
                + ") RI2 LEFT JOIN(SELECT RIQ3.intInventoryID, SUM(RIQ3.intItemQuantity) AS intReturnQuantity FROM tbl_invoiceItemReturns RIQ3 JOIN tbl_invoice RIQ4 "
                + "ON RIQ3.intInvoiceID = RIQ4.intInvoiceID WHERE RIQ4.varInvoiceNumber = @varInvoiceNumber AND RIQ4.intInvoiceSubNumber > 1 GROUP BY "
                + "RIQ3.intInventoryID) RIR ON RI2.intInventoryID = RIR.intInventoryID LEFT JOIN(SELECT RIQ5.intInventoryID, SUM(RIQ5.intItemQuantity) AS "
                + "intCurrentReturnQuantity FROM tbl_currentSalesItems RIQ5 JOIN tbl_currentSalesInvoice RIQ6 ON RIQ5.intInvoiceID = RIQ6.intInvoiceID WHERE "
                + "RIQ6.varInvoiceNumber = @varInvoiceNumber AND RIQ6.intInvoiceSubNumber > 1 GROUP BY RIQ5.intInventoryID) RIC ON RI2.intInventoryID = "
                + "RIC.intInventoryID GROUP BY RI2.intInvoiceItemID, RI2.intInventoryID";

            object[][] parms =
            {
                new object[] { "@varInvoiceNumber", invoiceNumber }
            };

            return ConvertFromDataTableToAvaiableToReturnItems(dbc.returnDataTableData(sqlCmd, parms), invoiceNumber);
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private InvoiceItems GatherRemainingReturnItemInformation(InvoiceItems invoiceItem, string invoiceNumber)
        {
            string sqlCmd = "SELECT CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = II.intInventoryID) THEN (SELECT A.varSku "
                + "FROM tbl_accessories A WHERE A.intInventoryID = II.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID "
                + "= II.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = II.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID "
                + "FROM tbl_clubs C WHERE C.intInventoryID = II.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = II.intInventoryID) END "
                + "AS varSku, varItemDescription, fltItemPrice, fltItemCost, intItemTypeID, varAdditionalInformation FROM tbl_invoiceItem II JOIN tbl_invoice I ON "
                + "I.intInvoiceID = II.intInvoiceID WHERE I.varInvoiceNumber = @varInvoiceNumber AND I.intInvoiceSubNumber = 1 AND II.intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@varInvoiceNumber", invoiceNumber },
                new object[] { "@intInventoryID", invoiceItem.intInventoryID }
            };
            return TurnIntoTempInvoiceItemForReturn(dbc.returnDataTableData(sqlCmd, parms));
        }
        private InvoiceItems TurnIntoTempInvoiceItemForReturn(DataTable dt)
        {
            List<InvoiceItems> invoiceItem = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                varSku = row.Field<string>("varSku"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemCost = row.Field<double>("fltItemCost"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return invoiceItem[0];
        }
        public InvoiceItems ReturnInvoiceItemForReturnProcess(int invoiceItemID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemForReturnProcess";
            string sqlCmd = "SELECT I.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) "
                + "THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = "
                + "I.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) THEN (SELECT C.varSku FROM "
                + "tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) END AS varSku, intItemQuantity, fltItemCost, fltItemPrice, fltItemDiscount, "
                + "fltItemRefund, bitIsDiscountPercent, varItemDescription, intItemTypeID, bitIsClubTradeIn, CASE WHEN EXISTS(SELECT A.intInventoryID FROM "
                + "tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) THEN (SELECT A.varAdditionalInformation FROM tbl_accessories A WHERE "
                + "A.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) "
                + "THEN (SELECT CL.varAdditionalInformation FROM tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID "
                + "FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) THEN (SELECT C.varAdditionalInformation FROM tbl_clubs C WHERE C.intInventoryID = "
                + "I.intInventoryID) END AS varAdditionalInformation FROM tbl_invoiceItem I WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {                
                new object[] { "@intInvoiceItemID", invoiceItemID }
            };
            ItemsManager IM = new ItemsManager();
            InvoiceItems invoiceItems = IM.ConvertFromDataTableToCartItems(dbc.returnDataTableData(sqlCmd, parms))[0];
            //InvoiceItems II = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName))[0];
            invoiceItems.intItemQuantity = ReturnQTYofItem(invoiceItems.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(invoiceItems.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
            return invoiceItems;
        }
        public void DoNotReturnTheItemOnReturn(InvoiceItems invoiceItem, object[] objPageDetails)
        {
            RemoveItemFromCurrentSalesTable(invoiceItem, objPageDetails);
        }
        public InvoiceItems ReturnSkuFromCurrentSalesUsingSKU(int invoiceItemID, object[] objPageDetails)
        {
            return ReturnItemDetailsFromCurrentSaleTable(invoiceItemID, objPageDetails)[0];
        }
        public int ReturnCurrentQuantityOfItem(InvoiceItems invoiceItems, object[] objPageDetails)
        {
            return ReturnQTYofItem(invoiceItems.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(invoiceItems.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
        }
        public bool ItemAlreadyInCart(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "ItemAlreadyInCart";
            bool itemInCart = false;

            string sqlCmd = "SELECT intInventoryID FROM tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID AND intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", ii.intInvoiceID },
                new object[] { "@intInventoryID", ii.intInventoryID }
            };

            DataTable dt = dbc.returnDataTableData(sqlCmd, parms);
            //DataTable dt = dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
            if (dt.Rows.Count > 0)
            {
                itemInCart = true;
            }
            return itemInCart;
        }


        public InvoiceItems GatherLoungeSimToAddToInvoice(string buttonName, int invoiceID)
        {
            string sqlCmd = "SELECT @intInvoiceID AS intInvoiceID, A.varSku, A.varTypeOfAccessory AS varItemDescriptionescription, "
                + "CAST(1 AS INT) AS intItemQuantity, A.fltItemCost, A.fltItemPrice, CAST(0 AS FLOAT) AS fltItemDiscount, CAST(0 "
                + "AS FLOAT) AS fltItemRefund, CAST(0 AS BIT) AS bitIsDiscountPercent, A.intItemTypeID, CAST(0 AS BIT) AS "
                + "bitIsClubTradeIn FROM tbl_accessories A JOIN tbl_LoungeButtonItemCombination LBIC ON LBIC.intInventoryID = "
                + "A.intInventoryID WHERE LBIC.loungeButton = @buttonName";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID },
                new object[] { "@buttonName", buttonName }
            };
            return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms))[0];
        }
        public void RemoveFromLoungeSimCart(int sku, string invoice, object[] objPageDetails)
        {
            //gather sku info
            List<InvoiceItems> itemToReturn = ReturnItemDetailsFromCurrentSaleTable(sku, objPageDetails);
            //remove the sku from currentSales table
            RemoveItemFromCurrentSalesTable(itemToReturn[0], objPageDetails);
        }
    }
}