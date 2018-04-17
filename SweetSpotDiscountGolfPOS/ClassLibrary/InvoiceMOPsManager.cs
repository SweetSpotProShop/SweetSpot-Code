using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class InvoiceMOPsManager
    {
        DatabaseCalls dbc = new DatabaseCalls();
        private List<InvoiceMOPs> ConvertFromDataTableToInvoiceMOPs(DataTable dt)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                id = row.Field<int>("ID"),
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                mopType = row.Field<string>("mopType"),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToCurrentInvoiceMOPs(DataTable dt)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                id = row.Field<int>("currentSalesMID"),
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                mopType = ReturnMOPString(row.Field<int>("mopType")),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return invoiceMOPs;
        }

        //Returns list of MOPs based on an Invoice number
        public List<InvoiceMOPs> ReturnInvoiceMOPs(string invoice)
        {
            string sqlCmd = "SELECT ID, invoiceNum, invoiceSubNum, mopType, amountPaid "
                + "FROM tbl_invoiceMOP WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            Object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<InvoiceMOPs> invoiceMOPs = ConvertFromDataTableToInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms));
            return invoiceMOPs;
        }

        //Returns list of MOPs based on an Invoice number in currentSales Table
        public List<InvoiceMOPs> ReturnInvoiceMOPsCurrentSale(string invoice)
        {
            string sqlCmd = "SELECT currentSalesMID, invoiceNum, invoiceSubNum, mopType, amountPaid "
                + "FROM tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            int num = Convert.ToInt32(invoice.Split('-')[1]);
            int sub = Convert.ToInt32(invoice.Split('-')[2]);

            Object[][] parms =
            {
                 new object[] { "@invoiceNum", num },
                 new object[] { "@invoiceSubNum", sub }
            };

            return ConvertFromDataTableToCurrentInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms));
        }
        public void AddNewMopToList(string invoice, double amountPaid, string method)
        {
            string sqlCmd = "INSERT INTO tbl_currentSalesMops VALUES(@invoiceNum, @invoiceSubNum, "
                + "@mopType, @amountPaid)";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2].ToString()) },
                new object[] { "@mopType", ReturnMOPInt(method) },
                new object[] { "@amountPaid", amountPaid }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        public void AddNewMopToReceiptList(string invoice, double amountPaid, string method, int chequeNumber)
        {
            string sqlCmd = "INSERT INTO tbl_currentPurchaseMops VALUES(@receiptNum, @mopType, "
                + "@chequeNum, @amountPaid)";

            object[][] parms =
            {
                new object[] { "@receiptNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@mopType", ReturnMOPInt(method) },
                new object[] { "@chequeNum", chequeNumber },
                new object[] { "@amountPaid", amountPaid }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        public void RemoveMopFromList(int mopID, string invoice)
        {
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum AND currentSalesMID = @mopID";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2].ToString()) },
                new object[] { "@mopID", mopID }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private int ReturnMOPInt(string mopName)
        {
            string sqlCmd = "SELECT methodID FROM tbl_methodOfPayment WHERE methodDesc = @mopName";
            object[][] parms =
            {
                new object[] { "@mopName", mopName }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        private string ReturnMOPString(int mopID)
        {
            string sqlCmd = "SELECT methodDesc FROM tbl_methodOfPayment WHERE methodID = @mopID";
            object[][] parms =
            {
                new object[] { "@mopID", mopID }
            };
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
    }
}