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
    }
}