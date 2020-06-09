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
    public class InvoiceItemsManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        //Converters
        private List<InvoiceItems> ConvertFromDataTableToInvoiceItemsCurrentSale(DataTable dt, DateTime selectedDate, int provinceID, object[] objPageDetails)
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
                bitIsClubTradeIn = row.Field<bool>("bitIsClubTradeIn"),
                invoiceItemTaxes = ReturnInvoiceItemTaxesCurrent(row.Field<int>("intInvoiceItemID"), selectedDate, provinceID, objPageDetails)
            }).ToList();
            return invoiceItems;
        }
        private List<InvoiceItems> ConvertFromDataTableToInvoiceItemsCurrentSale2(DataTable dt, DateTime selectedDate, int provinceID, object[] objPageDetails)
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
                bitIsClubTradeIn = row.Field<bool>("bitIsClubTradeIn"),
                invoiceItemTaxes = ReturnInvoiceItemTaxesCurrent(row.Field<int>("intInvoiceItemID"), selectedDate, provinceID, objPageDetails)
            }).ToList();
            return invoiceItems;
        }
        private List<InvoiceItems> ConvertFromDataTableToInvoiceItems(DataTable dt, DateTime selectedDate, int provinceID, object[] objPageDetails)
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
                bitIsClubTradeIn = row.Field<bool>("bitIsClubTradeIn"),
                invoiceItemTaxes = ReturnInvoiceItemTaxes(row.Field<int>("intInvoiceItemID"), selectedDate, provinceID, objPageDetails)
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
        private List<InvoiceItems> ConvertFromDataTableToAvaiableToReturnItems(DataTable dt, string invoiceNumber, DateTime selectedDate, int provinceID, object[] objPageDetails)
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
                InvoiceItems temp = GatherRemainingReturnItemInformation(ii, invoiceNumber, objPageDetails);
                ii.varSku = temp.varSku;
                ii.varItemDescription = temp.varItemDescription;
                ii.fltItemPrice = temp.fltItemPrice;
                ii.fltItemDiscount = temp.fltItemDiscount;
                ii.fltItemCost = temp.fltItemCost;
                ii.intItemTypeID = temp.intItemTypeID;
                ii.bitIsDiscountPercent = temp.bitIsDiscountPercent;
                ii.varAdditionalInformation = temp.varAdditionalInformation;
                ii.invoiceItemTaxes = ReturnInvoiceItemTaxes(temp.intInvoiceItemID, selectedDate, provinceID, objPageDetails);
            }
            return invoiceItem;
        }
        private InvoiceItems TurnIntoTempInvoiceItemForReturn(DataTable dt)
        {
            List<InvoiceItems> invoiceItem = dt.AsEnumerable().Select(row =>
            new InvoiceItems
            {
                varSku = row.Field<string>("varSku"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemDiscount = row.Field<double>("fltItemDiscount"),
                fltItemCost = row.Field<double>("fltItemCost"),
                intItemTypeID = row.Field<int>("intItemTypeID"),
                bitIsDiscountPercent = row.Field<bool>("bitIsDiscountPercent"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return invoiceItem[0];
        }



        //DB calls
        //THIS CAN BE UPDATED TO GET ALL INFO FROM INVOICE TABLES (remove all joins)
        private List<InvoiceItems> ReturnInvoiceItems(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItems";
            string sqlCmd = "SELECT II.intInvoiceItemID, II.intInvoiceID, II.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories "
                + "A WHERE A.intInventoryID = II.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = II.intInventoryID) "
                + "WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = II.intInventoryID) THEN (SELECT CL.varSku FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = II.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = II.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = II.intInventoryID) WHEN EXISTS("
                + "SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = II.intInventoryID) THEN (SELECT TI.varSku FROM "
                + "tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = II.intInventoryID) END AS varSku, II.varItemDescription, II.intItemQuantity, "
                + "II.fltItemCost, II.fltItemPrice, II.fltItemDiscount, II.fltItemRefund, II.bitIsDiscountPercent, II.intItemTypeID, II.bitIsClubTradeIn "
                + "FROM tbl_invoiceItem II WHERE II.intInvoiceID = @intInvoiceID UNION SELECT IR.intInvoiceItemID, IR.intInvoiceID, IR.intInventoryID, "
                + "CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = IR.intInventoryID) THEN (SELECT A.varSku FROM "
                + "tbl_accessories A WHERE A.intInventoryID = IR.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE "
                + "CL.intInventoryID = IR.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = IR.intInventoryID) WHEN "
                + "EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = IR.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C "
                + "WHERE C.intInventoryID = IR.intInventoryID) WHEN EXISTS(SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = "
                + "IR.intInventoryID) THEN (SELECT TI.varSku FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = IR.intInventoryID) END AS varSku, "
                + "IR.varItemDescription, IR.intItemQuantity, IR.fltItemCost, IR.fltItemPrice, IR.fltItemDiscount, IR.fltItemRefund, "
                + "IR.bitIsDiscountPercent, IR.intItemTypeID, IR.bitIsClubTradeIn FROM tbl_invoiceItemReturns IR WHERE IR.intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            List<InvoiceItems> invoiceItems = ConvertFromDataTableToInvoiceItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), selectedDate, provinceID, objPageDetails);
            return invoiceItems;
        }
        private List<InvoiceItems> ReturnInvoiceItemsCurrentSale(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsCurrentSale";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, CSI.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A "
                + "WHERE A.intInventoryID = CSI.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = CSI.intInventoryID"
                + ") WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = CSI.intInventoryID) THEN (SELECT CL.varSku "
                + "FROM tbl_clothing CL WHERE CL.intInventoryID = CSI.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = CSI.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = CSI.intInventoryID) WHEN "
                + "EXISTS(SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = CSI.intInventoryID) THEN (SELECT TI.varSku "
                + "FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = CSI.intInventoryID) END AS varSku, varItemDescription, intItemQuantity, "
                + "fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, intItemTypeID, bitIsClubTradeIn FROM "
                + "tbl_currentSalesItems CSI WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };
            
            return ConvertFromDataTableToInvoiceItemsCurrentSale(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), selectedDate, provinceID, objPageDetails);
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private List<InvoiceItems> ReturnInvoiceItemsCurrentSale2(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsCurrentSale2";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, CSI.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A "
                + "WHERE A.intInventoryID = CSI.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = CSI.intInventoryID"
                + ") WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = CSI.intInventoryID) THEN (SELECT CL.varSku "
                + "FROM tbl_clothing CL WHERE CL.intInventoryID = CSI.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = CSI.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = CSI.intInventoryID) WHEN "
                + "EXISTS(SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = CSI.intInventoryID) THEN (SELECT TI.varSku "
                + "FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = CSI.intInventoryID) END AS varSku, varItemDescription, intItemQuantity, "
                + "fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, intItemTypeID, bitIsClubTradeIn FROM "
                + "tbl_currentSalesItems CSI WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoiceItemsCurrentSale2(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), selectedDate, provinceID, objPageDetails);
        }
        private List<InvoiceItems> ReturnInvoiceItemsReceipt(int receiptID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsReceipt";
            string sqlCmd = "SELECT intReceiptItemID, intReceiptID, intInventoryID, intItemQuantity, varItemDescription, fltItemCost "
                + "FROM tbl_receiptItem WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceiptItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ConvertFromDataTableToReceiptItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private string ReturnStringSearchForAccessories(ArrayList array)
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
        private string ReturnStringSearchForClothing(ArrayList array)
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
        private string ReturnStringSearchForClubs(ArrayList array)
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
        private void InsertItemIntoSalesCart(InvoiceItems II, int transactionTypeID, DateTime currentDateTime, int provinceID, object[] objPageDetails)
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
            TaxManager TM = new TaxManager();
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            TM.LoopThroughTaxesForEachItemAddingToCurrentInvoiceItemTaxes(II, transactionTypeID, currentDateTime, provinceID, objPageDetails);
        }
        private void RemoveQTYFromInventoryWithSKU(int inventoryID, int itemTypeID, int remainingQTY, object[] objPageDetails)
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
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ReturnTableNameFromTypeID(int itemTypeID, object[] objPageDetails)
        {
            string strQueryName = "ReturnTableNameFromTypeID";
            string sqlCmd = "SELECT varItemTypeName FROM tbl_itemType WHERE intItemTypeID = @intItemTypeID";
            object[][] parms =
            {
                new object[] { "@intItemTypeID", itemTypeID }
            };

            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnItemsInTheCart(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemsInTheCart";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, intInventoryID, intItemQuantity, varItemDescription, "
                + "fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, bitIsClubTradeIn, "
                + "intItemTypeID FROM tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnItemsInTheReturnCart(string invoice, object[] objPageDetails)
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

            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnInventoryIDFromInvoiceItemID(int invoiceItemID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInventoryIDFromInvoiceItemID";
            string sqlCmd = "SELECT intInventoryID FROM tbl_currentSalesItems WHERE intInvoiceItemID = @intInvoiceItemID";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID }
            };
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateItemFromCurrentSalesTableActualQuery(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "UpdateItemFromCurrentSalesTableActualQuery";
            string sqlCmd = "UPDATE tbl_currentSalesItems SET intItemQuantity = @intItemQuantity, fltItemDiscount = @fltItemDiscount, "
                + "bitIsDiscountPercent = @bitIsDiscountPercent WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intItemQuantity", ii.intItemQuantity },
                new object[] { "@fltItemDiscount", ii.fltItemDiscount },
                new object[] { "@bitIsDiscountPercent", ii.bitIsDiscountPercent },
                new object[] { "@intInvoiceItemID", ii.intInvoiceItemID }
            };

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void NewTradeInChangeChargeTaxToFalse(InvoiceItems invoiceItems, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "NewTradeInChangeChargeTaxToFalse";
            invoiceItems.invoiceItemTaxes = ReturnInvoiceItemTaxesCurrent(ReturnInvoiceItemIDFromInventoryID(invoiceItems, objPageDetails), selectedDate, provinceID, objPageDetails);

            foreach (InvoiceItemTax invoiceItemTax in invoiceItems.invoiceItemTaxes)
            {
                string sqlCmd = "UPDATE tbl_currentSalesItemsTaxes SET bitIsTaxCharged = @bitIsTaxCharged WHERE intInvoiceItemID = "
                + "@intInvoiceItemID AND intTaxTypeID = @intTaxTypeID";
                object[][] parms =
                {
                    new object[] { "@bitIsTaxCharged", false },
                    new object[] { "@intTaxTypeID", invoiceItemTax.intTaxTypeID },
                    new object[] { "@intInvoiceItemID", invoiceItemTax.intInvoiceItemID }
                };

                DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private int ReturnInvoiceItemIDFromInventoryID(InvoiceItems invoiceItem, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemIDFromInventoryID";
            string sqlCmd = "SELECT intInvoiceItemID FROM tbl_currentSalesItems WHERE intInventoryID = @intInventoryID AND intInvoiceID = "
                + "@intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", invoiceItem.intInventoryID },
                new object[] { "@intInvoiceID", invoiceItem.intInvoiceID }
            };
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateItemTaxesFromCurrentSalesTableActualQuery(InvoiceItems invoiceItem, int transactionTypeID, object[] objPageDetails)
        {
            string strQueryName = "UpdateItemTaxesFromCurrentSalesTableActualQuery";
            foreach (InvoiceItemTax iit in invoiceItem.invoiceItemTaxes)
            {
                string sqlCmd = "UPDATE tbl_currentSalesItemsTaxes SET bitIsTaxCharged = @bitIsTaxCharged, fltTaxAmount = @fltTaxAmount WHERE intInvoiceItemID = "
                    + "@intInvoiceItemID AND intTaxTypeID = (SELECT intTaxID FROM tbl_taxType WHERE varTaxName = @varTaxName)";

                double taxAmount = 0;
                if (transactionTypeID == 1)
                {
                    if (invoiceItem.bitIsDiscountPercent)
                    {
                        taxAmount = ((invoiceItem.fltItemPrice - (invoiceItem.fltItemPrice * (invoiceItem.fltItemDiscount / 100))) * iit.fltTaxRate) * invoiceItem.intItemQuantity;

                    }
                    else
                    {
                        taxAmount = ((invoiceItem.fltItemPrice - invoiceItem.fltItemDiscount) * iit.fltTaxRate) * invoiceItem.intItemQuantity;
                    }
                }
                else if (transactionTypeID == 2)
                {
                    taxAmount = (invoiceItem.fltItemRefund * iit.fltTaxRate) * invoiceItem.intItemQuantity;
                }

                object[][] parms =
                {
                    new object[] { "@bitIsTaxCharged", iit.bitIsTaxCharged },
                    new object[] { "@fltTaxAmount", taxAmount },
                    new object[] { "@intInvoiceItemID", iit.intInvoiceItemID },
                    new object[] { "@varTaxName", iit.varTaxName }
                };
                DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
                //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void UpdateSimItemFromCurrentSalesTableActualQuery(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "UpdateSimItemFromCurrentSalesTableActualQuery";
            string sqlCmd = "UPDATE tbl_currentSalesItems SET intItemQuantity = @intItemQuantity, fltItemDiscount = @fltItemDiscount, "
                + "bitIsDiscountPercent = @bitIsDiscountPercent WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intItemQuantity", ii.intItemQuantity },
                new object[] { "@fltItemDiscount", ii.fltItemDiscount },
                new object[] { "@bitIsDiscountPercent", ii.bitIsDiscountPercent },
                new object[] { "@intInvoiceItemID", ii.intInvoiceItemID }
            };

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateItemFromCurrentSalesTableActualQueryForPurchases(InvoiceItems ii, object[] objPageDetails)
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

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveInitialTotalsForTable(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveInitialTotalsForTable";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnQTYofItemFromInventoryID(int inventoryID, string tbl, string column, object[] objPageDetails)
        {
            string strQueryName = "ReturnQTYofItem";
            string sqlCmd = "SELECT " + column + " FROM " + tbl + " WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID }
            };
            //Returns the quantity of the searched item
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<InvoiceItems> ReturnItemDetailsFromCurrentSaleTable(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnItemDetailsFromCurrentSaleTable";
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE "
                + "A.intInventoryID = I.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) WHEN "
                + "EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) THEN (SELECT CL.varSku FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = I.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) WHEN EXISTS("
                + "SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = I.intInventoryID) THEN (SELECT TI.varSku FROM "
                + "tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = I.intInventoryID) END AS varSku, varItemDescription, intItemQuantity, fltItemCost, "
                + "fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, bitIsClubTradeIn, intItemTypeID FROM tbl_currentSalesItems I WHERE "
                + "intInvoiceItemID = @intInvoiceItemID";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID }
            };

            return ConvertFromDataTableToInvoiceItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), selectedDate, provinceID, objPageDetails);
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
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveItemFromCurrentSalesTaxTable(InvoiceItems invoiceItem, object[] objPageDetails)
        {
            string strQueryName = "RemoveItemFromCurrentSalesTable";
            string sqlCmd = "DELETE tbl_currentSalesItemsTaxes WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItem.intInvoiceItemID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveItemFromCurrentSalesTable(InvoiceItems invoiceItem, object[] objPageDetails)
        {
            string strQueryName = "RemoveItemFromCurrentSalesTable";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItem.intInvoiceItemID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<InvoiceItems> ReturnInvoiceItemsFromProcessedSalesForReturn(string invoiceNumber, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemsFromProcessedSalesForReturn";
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

            return ConvertFromDataTableToAvaiableToReturnItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), invoiceNumber, selectedDate, provinceID, objPageDetails);
            //return ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private InvoiceItems GatherRemainingReturnItemInformation(InvoiceItems invoiceItem, string invoiceNumber, object[] objPageDetails)
        {
            string strQueryName = "GatherRemainingReturnItemInformation";
            string sqlCmd = "SELECT CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = II.intInventoryID) THEN (SELECT "
                + "A.varSku FROM tbl_accessories A WHERE A.intInventoryID = II.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL "
                + "WHERE CL.intInventoryID = II.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = II.intInventoryID) "
                + "WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = II.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C "
                + "WHERE C.intInventoryID = II.intInventoryID) WHEN EXISTS(SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID "
                + "= II.intInventoryID) THEN (SELECT TI.varSku FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = II.intInventoryID) END AS varSku, "
                + "varItemDescription, fltItemPrice, fltItemDiscount, fltItemCost, intItemTypeID, bitIsDiscountPercent, varAdditionalInformation FROM "
                + "tbl_invoiceItem II JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.varInvoiceNumber = @varInvoiceNumber AND "
                + "I.intInvoiceSubNumber = 1 AND II.intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@varInvoiceNumber", invoiceNumber },
                new object[] { "@intInventoryID", invoiceItem.intInventoryID }
            };
            return TurnIntoTempInvoiceItemForReturn(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        //Search results for A Return
        private InvoiceItems ReturnInvoiceItemForReturnProcess(int invoiceItemID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemForReturnProcess";
            string sqlCmd = "SELECT I.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) "
                + "THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing "
                + "CL WHERE CL.intInventoryID = I.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) WHEN "
                + "EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE "
                + "C.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT TI.intTradeInID FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = "
                + "I.intInventoryID) THEN (SELECT TI.varSku FROM tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = I.intInventoryID) END AS varSku, "
                + "intItemQuantity, fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, varItemDescription, intItemTypeID, "
                + "bitIsClubTradeIn, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) THEN (SELECT "
                + "A.varAdditionalInformation FROM tbl_accessories A WHERE A.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = I.intInventoryID) THEN (SELECT CL.varAdditionalInformation FROM tbl_clothing CL WHERE "
                + "CL.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) THEN "
                + "(SELECT C.varAdditionalInformation FROM tbl_clubs C WHERE C.intInventoryID = I.intInventoryID) WHEN EXISTS(SELECT TI.intTradeInID FROM "
                + "tbl_tempTradeInCartSkus TI WHERE TI.intTradeInID = I.intInventoryID) THEN (SELECT TI.varSku FROM tbl_tempTradeInCartSkus TI WHERE "
                + "TI.intTradeInID = I.intInventoryID) END AS varAdditionalInformation FROM tbl_invoiceItem I WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID }
            };
            ItemsManager IM = new ItemsManager();
            InvoiceItems invoiceItems = IM.CallConvertFromDataTableToCartItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName))[0];
            //InvoiceItems II = ConvertFromDataTableToInvoiceItems(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName))[0];
            //does not have invoiceItemID
            invoiceItems.intItemQuantity = ReturnQTYofItemFromInventoryID(invoiceItems.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(invoiceItems.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
            return invoiceItems;
        }
        private bool ItemAlreadyInCart(InvoiceItems ii, object[] objPageDetails)
        {
            string strQueryName = "ItemAlreadyInCart";
            bool itemInCart = false;

            string sqlCmd = "SELECT intInventoryID FROM tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID AND intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", ii.intInvoiceID },
                new object[] { "@intInventoryID", ii.intInventoryID }
            };

            DataTable dt = DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //DataTable dt = dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
            if (dt.Rows.Count > 0)
            {
                itemInCart = true;
            }
            return itemInCart;
        }
        private InvoiceItems GatherLoungeSimToAddToInvoice(string buttonName, int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "GatherLoungeSimToAddToInvoice";
            string sqlCmd = "SELECT A.intInventoryID, A.varSku, A.varTypeOfAccessory AS varItemDescription, CAST(1 AS INT) AS intItemQuantity, "
                + "A.fltCost AS fltItemCost, A.fltPrice AS fltItemPrice, A.intItemTypeID, A.varAdditionalInformation FROM tbl_accessories A "
                + "JOIN tbl_LoungeButtonItemCombination LBIC ON LBIC.intInventoryID = A.intInventoryID WHERE LBIC.varLoungeButtonName = @varButtonName";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID },
                new object[] { "@varButtonName", buttonName }
            };
            ItemsManager IM = new ItemsManager();
            return IM.CallConvertFromDataTableToCartItems(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName))[0];
        }
        private List<InvoiceItemTax> ReturnInvoiceItemTaxesCurrent(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemTaxesCurrent";
            //string sqlCmd = "SELECT CSIT.intInvoiceItemID, T.varTaxName, CSIT.intTaxTypeID, fltTaxAmount, bitIsTaxCharged FROM tbl_currentSalesItemsTaxes "
            //    + "CSIT JOIN tbl_taxType T ON T.intTaxID = CSIT.intTaxTypeID WHERE CSIT.intInvoiceItemID = @intInvoiceItemID";
            //string sqlCmd = "SELECT CSIT.intInvoiceItemID, T.varTaxName, CSIT.intTaxTypeID, TAX.fltTaxRate, fltTaxAmount, bitIsTaxCharged FROM "
            //    + "tbl_currentSalesItemsTaxes CSIT JOIN tbl_taxType T ON T.intTaxID = CSIT.intTaxTypeID INNER JOIN(SELECT TD.intTaxID, fltTaxRate, TD.MTD "
            //    + "FROM tbl_taxRate TR INNER JOIN(SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate WHERE dtmTaxEffectiveDate <= "
            //    + "@dtmSelectedDate AND intProvinceID = @intProvinceID GROUP BY intTaxID) TD ON TR.intTaxID = TD.intTaxID AND TR.dtmTaxEffectiveDate = TD.MTD "
            //    + "GROUP BY TD.intTaxID, fltTaxRate, TD.MTD) TAX ON TAX.intTaxID = CSIT.intTaxTypeID WHERE CSIT.intInvoiceItemID = @intInvoiceItemID";
            string sqlCmd = "SELECT CSIT.intInvoiceItemID, T.varTaxName, CSIT.intTaxTypeID, TAX.fltTaxRate, fltTaxAmount, bitIsTaxCharged FROM "
                + "tbl_currentSalesItemsTaxes CSIT JOIN tbl_taxType T ON T.intTaxID = CSIT.intTaxTypeID INNER JOIN(SELECT TD.intTaxID, fltTaxRate, "
                + "TD.MTD FROM tbl_taxRate TR INNER JOIN(SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate WHERE dtmTaxEffectiveDate "
                + "<= @dtmSelectedDate AND intProvinceID = @intProvinceID GROUP BY intTaxID) TD ON TR.intTaxID = TD.intTaxID AND TR.dtmTaxEffectiveDate "
                + "= TD.MTD WHERE intProvinceID = @intProvinceID GROUP BY TD.intTaxID, fltTaxRate, TD.MTD) TAX ON TAX.intTaxID = CSIT.intTaxTypeID "
                + "WHERE CSIT.intInvoiceItemID = @intInvoiceItemID";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID },
                new object[] { "@dtmSelectedDate", selectedDate },
                new object[] { "@intProvinceID", provinceID }
            };
            TaxManager TM = new TaxManager();
            return TM.ConvertFromDataTableToInvoiceItemTax(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private List<InvoiceItemTax> ReturnInvoiceItemTaxesCurrent2(int invoiceItemID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemTaxesCurrent2";
            string sqlCmd = "SELECT CSIT.intInvoiceItemID, T.varTaxName, CSIT.intTaxTypeID, fltTaxAmount, bitIsTaxCharged FROM tbl_currentSalesItemsTaxes "
                + "CSIT JOIN tbl_taxType T ON T.intTaxID = CSIT.intTaxTypeID WHERE CSIT.intInvoiceItemID = @intInvoiceItemID";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID }
            };
            TaxManager TM = new TaxManager();
            return TM.CallConversionFromDataTableToInvoiceItemTax2(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private List<InvoiceItemTax> ReturnInvoiceItemTaxes(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceItemTaxes";
            //string sqlCmd = "SELECT intInvoiceItemID, T.varTaxName, IIT.intTaxTypeID, fltTaxAmount, bitIsTaxCharged FROM tbl_invoiceItemTaxes IIT JOIN "
            //    + "tbl_taxType T ON T.intTaxID = IIT.intTaxTypeID WHERE intInvoiceItemID = @intInvoiceItemID";
            string sqlCmd = "SELECT intInvoiceItemID, T.varTaxName, IIT.intTaxTypeID, TAX.fltTaxRate, fltTaxAmount, bitIsTaxCharged FROM tbl_invoiceItemTaxes "
                + "IIT JOIN tbl_taxType T ON T.intTaxID = IIT.intTaxTypeID INNER JOIN(SELECT TD.intTaxID, fltTaxRate, TD.MTD FROM tbl_taxRate TR INNER "
                + "JOIN(SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate WHERE dtmTaxEffectiveDate <= @dtmSelectedDate AND intProvinceID = "
                + "@intProvinceID GROUP BY intTaxID) TD ON TR.intTaxID = TD.intTaxID AND TR.dtmTaxEffectiveDate = TD.MTD WHERE intProvinceID = @intProvinceID "
                + "GROUP BY TD.intTaxID, fltTaxRate, TD.MTD) TAX ON TAX.intTaxID = IIT.intTaxTypeID WHERE intInvoiceItemID = @intInvoiceItemID";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemID },
                new object[] { "@dtmSelectedDate", selectedDate },
                new object[] { "@intProvinceID", provinceID }
            };
            TaxManager TM = new TaxManager();
            return TM.ConvertFromDataTableToInvoiceItemTax(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }



        //Public calls
        public List<InvoiceItems> CallReturnInvoiceItemsCurrentSale(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            return ReturnInvoiceItemsCurrentSale2(invoiceID, selectedDate, provinceID, objPageDetails);
        }
        public void ReturnQTYToInventory(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            //gather sku info
            List<InvoiceItems> itemToReturn = ReturnItemDetailsFromCurrentSaleTable(invoiceItemID, selectedDate, provinceID, objPageDetails);
            //use info to add quantity back
            AddInventoryBackIntoStock(itemToReturn[0], objPageDetails);
            //remove the sku from currentSales table
            RemoveItemFromCurrentSalesTaxTable(itemToReturn[0], objPageDetails);
            RemoveItemFromCurrentSalesTable(itemToReturn[0], objPageDetails);
        }
        public bool ValidQTY(InvoiceItems ii, object[] objPageDetails)
        {
            bool hasValidQty = true;
            int qtyInCurrentStock = ReturnQTYofItemFromInventoryID(ii.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(ii.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
            int qtyOnCurrentSale = ReturnQTYofItemFromInventoryID(ii.intInventoryID, "tbl_currentSalesItems", "intItemQuantity", objPageDetails);

            int remaingQTYAvailForSale = qtyInCurrentStock - (ii.intItemQuantity - qtyOnCurrentSale);

            if (remaingQTYAvailForSale < 0)
            {
                hasValidQty = false;
            }
            return hasValidQty;
        }
        public void UpdateItemFromCurrentSalesTable(InvoiceItems ii, int transactionTypeID, object[] objPageDetails)
        {
            int AccessoryQTY = ReturnQTYofItemFromInventoryID(ii.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(ii.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
            int OrigQTYonSale = ReturnQTYofItemFromInventoryID(ii.intInventoryID, "tbl_currentSalesItems", "intItemQuantity", objPageDetails);

            int NewQTYonSale = ii.intItemQuantity;

            RemoveQTYFromInventoryWithSKU(ii.intInventoryID, ii.intItemTypeID, AccessoryQTY - (NewQTYonSale - OrigQTYonSale), objPageDetails);
            UpdateItemFromCurrentSalesTableActualQuery(ii, objPageDetails);
            UpdateItemTaxesFromCurrentSalesTableActualQuery(ii, transactionTypeID, objPageDetails); 
        }
        public void LoopThroughTheItemsToReturnToInventory(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            DataTable dt = ReturnItemsInTheCart(invoiceID, objPageDetails);
            //Loop through DataTable
            foreach(DataRow r in dt.Rows)
            {
                ReturnQTYToInventory(Convert.ToInt32(r["intInvoiceItemID"].ToString()), selectedDate, provinceID, objPageDetails);
            }
        }
        public List<InvoiceItems> ReturnItemsToCalculateTotals(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            return ConvertFromDataTableToInvoiceItems(ReturnItemsInTheCart(invoiceID, objPageDetails), selectedDate, provinceID, objPageDetails);
        }
        public void DeleteItemFromCurrentSalesTable(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            List<InvoiceItems> invoiceItems = ReturnItemDetailsFromCurrentSaleTable(invoiceItemID, selectedDate, provinceID, objPageDetails);
            RemoveItemFromCurrentSalesTaxTable(invoiceItems[0], objPageDetails);
            RemoveItemFromCurrentSalesTable(invoiceItems[0], objPageDetails);
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
        private bool CheckItemInInvoice(Invoice invoice)
        {
            bool itemsInTransaction = false;
            if (invoice.invoiceItems.Count > 0)
            {
                itemsInTransaction = true;
            }
            return itemsInTransaction;
        }
        public void DoNotReturnTheItemOnReturn(InvoiceItems invoiceItem, object[] objPageDetails)
        {
            RemoveItemFromCurrentSalesTable(invoiceItem, objPageDetails);
        }
        public InvoiceItems ReturnSkuFromCurrentSalesUsingSKU(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            return ReturnItemDetailsFromCurrentSaleTable(invoiceItemID, selectedDate, provinceID, objPageDetails)[0];
        }
        public int ReturnCurrentQuantityOfItem(InvoiceItems invoiceItems, object[] objPageDetails)
        {
            return ReturnQTYofItemFromInventoryID(invoiceItems.intInventoryID, "tbl_" + ReturnTableNameFromTypeID(invoiceItems.intItemTypeID, objPageDetails), "intQuantity", objPageDetails);
        }
        public void RemoveFromLoungeSimCart(int invoiceItemID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            //gather sku info
            List<InvoiceItems> itemToReturn = ReturnItemDetailsFromCurrentSaleTable(invoiceItemID, selectedDate, provinceID, objPageDetails);
            //remove the sku from currentSales table
            RemoveItemFromCurrentSalesTable(itemToReturn[0], objPageDetails);
        }
        public List<InvoiceItems> CallReturnInvoiceItems(int invoiceID, DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            return ReturnInvoiceItems(invoiceID, selectedDate, provinceID, objPageDetails);
        }
        public List<InvoiceItems> CallReturnInvoiceItemsReceipt(int receiptID, object[] objPageDetails)
        {
            return ReturnInvoiceItemsReceipt(receiptID, objPageDetails);
        }
        public string CallReturnStringSearchForAccessories(ArrayList array)
        {
            return ReturnStringSearchForAccessories( array);
        }
        public string CallReturnStringSearchForClothing(ArrayList array)
        {
            return ReturnStringSearchForClothing(array);
        }
        public string CallReturnStringSearchForClubs(ArrayList array)
        {
            return ReturnStringSearchForClubs(array);
        }
        public string CallReturnTableNameFromTypeID(int itemTypeID, object[] objPageDetails)
        {
            return ReturnTableNameFromTypeID(itemTypeID, objPageDetails);
        }
    }
}