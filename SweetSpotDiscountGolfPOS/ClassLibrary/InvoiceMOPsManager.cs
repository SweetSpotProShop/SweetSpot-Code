using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class InvoiceMOPsManager
    {
        DatabaseCalls DBC = new DatabaseCalls();
        private List<InvoiceMOPs> ConvertFromDataTableToInvoiceMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                intInvoicePaymentID = row.Field<int>("intInvoicePaymentID"),
                intInvoiceID = row.Field<int>("intInvoiceID"),
                intPaymentID = row.Field<int>("intPaymentID"),
                varPaymentName = ReturnMOPString(row.Field<int>("intPaymentID"), objPageDetails),
                fltAmountPaid = row.Field<double>("fltAmountPaid"),
                fltTenderedAmount = row.Field<double>("fltTenderedAmount"),
                fltCustomerChange = row.Field<double>("fltCustomerChange")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToCurrentInvoiceMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                intInvoicePaymentID = row.Field<int>("intInvoicePaymentID"),
                intInvoiceID = row.Field<int>("intInvoiceID"),
                intPaymentID = row.Field<int>("intPaymentID"),
                varPaymentName = ReturnMOPString(row.Field<int>("intPaymentID"), objPageDetails),
                fltAmountPaid = row.Field<double>("fltAmountPaid"),
                fltTenderedAmount = row.Field<double>("fltTenderedAmount"),
                fltCustomerChange = row.Field<double>("fltCustomerChange")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToCurrentPurchaseMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                intInvoicePaymentID = row.Field<int>("intReceiptPaymentID"),
                intInvoiceID = row.Field<int>("intReceiptID"),
                intPaymentID = row.Field<int>("intPaymentID"),
                varPaymentName = ReturnMOPString(row.Field<int>("intPaymentID"), objPageDetails),
                intChequeNumber = row.Field<int>("intChequeNumber"),
                fltAmountPaid = row.Field<double>("fltAmountPaid")
            }).ToList();
            return invoiceMOPs;
        }
        private List<InvoiceMOPs> ConvertFromDataTableToReceiptPurchaseMOPs(DataTable dt, object[] objPageDetails)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                intInvoicePaymentID = row.Field<int>("intReceiptPaymentID"),
                intInvoiceID = row.Field<int>("intReceiptID"),
                intPaymentID = row.Field<int>("intPaymentID"),
                varPaymentName = ReturnMOPString(row.Field<int>("intPaymentID"), objPageDetails),
                intChequeNumber = row.Field<int>("intChequeNumber"),
                fltAmountPaid = row.Field<double>("fltAmountPaid")
            }).ToList();
            return invoiceMOPs;
        }
        //Returns list of MOPs based on an Invoice number
        public List<InvoiceMOPs> ReturnInvoiceMOPs(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceMOPs";
            string sqlCmd = "SELECT intInvoicePaymentID, intInvoiceID, intPaymentID, fltAmountPaid, fltTenderedAmount, fltCustomerChange "
                + "FROM tbl_invoiceMOP WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoiceMOPs(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }

        //Returns list of MOPs based on an Invoice number in currentSales Table
        public List<InvoiceMOPs> ReturnInvoiceMOPsCurrentSale(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceMOPsCurrentSale";
            string sqlCmd = "SELECT intInvoicePaymentID, intInvoiceID, intPaymentID, fltAmountPaid, fltTenderedAmount, fltCustomerChange FROM "
                + "tbl_currentSalesMops WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToCurrentInvoiceMOPs(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToCurrentInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<InvoiceMOPs> ReturnPurchaseMOPsCurrentSale(int receiptID, object[] objPageDetails)
        {
            string strQueryName = "ReturnPurchaseMOPsCurrentSale";
            string sqlCmd = "SELECT intReceiptPaymentID, intReceiptID, intPaymentID, intChequeNumber, fltAmountPaid FROM tbl_currentPurchaseMops "
                + "WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToCurrentPurchaseMOPs(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToCurrentPurchaseMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<InvoiceMOPs> ReturnReceiptMOPsPurchase(int receiptID, object[] objPageDetails)
        {
            string strQueryName = "ReturnReceiptMOPsPurchase";
            string sqlCmd = "SELECT intReceiptPaymentID, intReceiptID, intPaymentID, intChequeNumber, fltAmountPaid FROM tbl_receiptMOP "
                + "WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceiptPurchaseMOPs(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToReceiptPurchaseMOPs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public void AddNewMopToList(InvoiceMOPs invoicePayment, object[] objPageDetails)
        {
            string strQueryName = "AddNewMopToList";
            string sqlCmd = "INSERT INTO tbl_currentSalesMops VALUES(@intInvoiceID, @intPaymentID, @fltAmountPaid, @fltTenderedAmount, "
                + "@fltCustomerChange)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoicePayment.intInvoiceID },
                new object[] { "@intPaymentID", invoicePayment.intPaymentID },
                new object[] { "@fltAmountPaid", invoicePayment.fltAmountPaid },
                new object[] { "@fltTenderedAmount", invoicePayment.fltTenderedAmount },
                new object[] { "@fltCustomerChange", invoicePayment.fltCustomerChange }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void AddNewMopToReceiptList(InvoiceMOPs payment, object[] objPageDetails)
        {
            string strQueryName = "AddNewMopToReceiptList";
            string sqlCmd = "INSERT INTO tbl_currentPurchaseMops VALUES(@intReceiptID, @intPaymentID, @intChequeNumber, @fltAmountPaid)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", payment.intInvoiceID },
                new object[] { "@intPaymentID", payment.intPaymentID },
                new object[] { "@intChequeNumber", payment.intChequeNumber },
                new object[] { "@fltAmountPaid", payment.fltAmountPaid }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void RemoveMopFromList(int invoicePaymentID, object[] objPageDetails)
        {
            string strQueryName = "RemoveMopFromList";
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE intInvoicePaymentID = @intInvoicePaymentID";

            object[][] parms =
            {
                new object[] { "@intInvoicePaymentID", invoicePaymentID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void RemoveMopFromPurchaseList(int receiptPaymentID, int receiptID, object[] objPageDetails)
        {
            string strQueryName = "RemoveMopFromPurchaseList";
            string sqlCmd = "DELETE tbl_currentPurchaseMops WHERE intReceiptID = @intReceiptID AND intReceiptPaymentID = @intReceiptPaymentID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID },
                new object[] { "@intReceiptPaymentID", receiptPaymentID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnMOPInt(string mopName, object[] objPageDetails)
        {
            string strQueryName = "ReturnMOPInt";
            string sqlCmd = "SELECT methodID FROM tbl_methodOfPayment WHERE methodDesc = @mopName";
            object[][] parms =
            {
                new object[] { "@mopName", mopName }
            };
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ReturnMOPString(int paymentID, object[] objPageDetails)
        {
            string strQueryName = "ReturnMOPString";
            string sqlCmd = "SELECT varPaymentName FROM tbl_methodOfPayment WHERE intPaymentID = @intPaymentID";
            object[][] parms =
            {
                new object[] { "@intPaymentID", paymentID }
            };
            return DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public int ReturnMopIntForTable(string mopName, object[] objPageDetails)
        {
            return ReturnMOPInt(mopName, objPageDetails);
        }
    }
}