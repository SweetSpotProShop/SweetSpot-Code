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
                amountPaid = row.Field<double>("amountPaid"),
                tender = row.Field<double>("tender"),
                change = row.Field<double>("change")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToCurrentInvoiceMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                id = row.Field<int>("currentSalesMID"),
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                mopType = ReturnMOPString(row.Field<int>("mopType"), objPageDetails),
                amountPaid = row.Field<double>("amountPaid"),
                tender = row.Field<double>("tender"),
                change = row.Field<double>("change")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToCurrentPurchaseMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                id = row.Field<int>("currentPurchaseMID"),
                invoiceNum = row.Field<int>("receiptNum"),
                mopType = ReturnMOPString(row.Field<int>("mopType"), objPageDetails),
                cheque = row.Field<int>("chequeNum"),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToReceiptPurchaseMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                id = row.Field<int>("ID"),
                invoiceNum = row.Field<int>("receiptNum"),
                mopType = ReturnMOPString(row.Field<int>("mopType"), objPageDetails),
                cheque = row.Field<int>("chequeNum"),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return invoiceMOPs;
        }
        //Returns list of MOPs based on an Invoice number
        public List<InvoiceMOPs> ReturnInvoiceMOPs(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceMOPs";
            string sqlCmd = "SELECT ID, invoiceNum, invoiceSubNum, mopType, amountPaid, tender, change "
                + "FROM tbl_invoiceMOP WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<InvoiceMOPs> invoiceMOPs = ConvertFromDataTableToInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
            return invoiceMOPs;
        }

        //Returns list of MOPs based on an Invoice number in currentSales Table
        public List<InvoiceMOPs> ReturnInvoiceMOPsCurrentSale(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceMOPsCurrentSale";
            string sqlCmd = "SELECT currentSalesMID, invoiceNum, invoiceSubNum, mopType, amountPaid, tender, change "
                + "FROM tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };

            return ConvertFromDataTableToCurrentInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<InvoiceMOPs> ReturnPurchaseMOPsCurrentSale(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnPurchaseMOPsCurrentSale";
            string sqlCmd = "SELECT currentPurchaseMID, receiptNum, mopType, chequeNum, amountPaid "
                + "FROM tbl_currentPurchaseMops WHERE receiptNum = @invoiceNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            return ConvertFromDataTableToCurrentPurchaseMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<InvoiceMOPs> ReturnReceiptMOPsPurchase(string receipt, object[] objPageDetails)
        {
            string strQueryName = "ReturnReceiptMOPsPurchase";
            string sqlCmd = "SELECT ID, receiptNum, mopType, chequeNum, amountPaid "
                + "FROM tbl_receiptMOP WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                 new object[] { "@receiptNum", Convert.ToInt32(receipt) }
            };

            return ConvertFromDataTableToReceiptPurchaseMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public void AddNewMopToList(string invoice, double amountPaid, string method, object[] amounts, object[] objPageDetails)
        {
            string strQueryName = "AddNewMopToList";
            string sqlCmd = "INSERT INTO tbl_currentSalesMops VALUES(@invoiceNum, @invoiceSubNum, "
                + "@mopType, @amountPaid, @tender, @change)";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2].ToString()) },
                new object[] { "@mopType", ReturnMOPInt(method, objPageDetails) },
                new object[] { "@amountPaid", amountPaid },
                new object[] { "@tender", Convert.ToDouble(amounts[0]) },
                new object[] { "@change", Convert.ToDouble(amounts[1]) }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void AddNewMopToReceiptList(string invoice, double amountPaid, string method, int chequeNumber, object[] objPageDetails)
        {
            string strQueryName = "AddNewMopToReceiptList";
            string sqlCmd = "INSERT INTO tbl_currentPurchaseMops VALUES(@receiptNum, @mopType, "
                + "@chequeNum, @amountPaid)";

            object[][] parms =
            {
                new object[] { "@receiptNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@mopType", ReturnMOPInt(method, objPageDetails) },
                new object[] { "@chequeNum", chequeNumber },
                new object[] { "@amountPaid", amountPaid }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void RemoveMopFromList(int mopID, string invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveMopFromList";
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum AND currentSalesMID = @mopID";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2].ToString()) },
                new object[] { "@mopID", mopID }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void RemoveMopFromPurchaseList(int mopID, string invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveMopFromPurchaseList";
            string sqlCmd = "DELETE tbl_currentPurchaseMops WHERE receiptNum = @receiptNum AND "
                + "currentPurchaseMID = @mopID";

            object[][] parms =
            {
                new object[] { "@receiptNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@mopID", mopID }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnMOPInt(string mopName, object[] objPageDetails)
        {
            string strQueryName = "ReturnMOPInt";
            string sqlCmd = "SELECT methodID FROM tbl_methodOfPayment WHERE methodDesc = @mopName";
            object[][] parms =
            {
                new object[] { "@mopName", mopName }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ReturnMOPString(int mopID, object[] objPageDetails)
        {
            string strQueryName = "ReturnMOPString";
            string sqlCmd = "SELECT methodDesc FROM tbl_methodOfPayment WHERE methodID = @mopID";
            object[][] parms =
            {
                new object[] { "@mopID", mopID }
            };
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public int ReturnMopIntForTable(string mopName, object[] objPageDetails)
        {
            return ReturnMOPInt(mopName, objPageDetails);
        }
    }
}