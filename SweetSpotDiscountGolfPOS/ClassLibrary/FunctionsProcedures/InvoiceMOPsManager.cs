using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS.FP
{
    public class InvoiceMOPsManager
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();

        //Converters
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





        //DB calls
        //Returns list of MOPs based on an Invoice number
        private List<InvoiceMOPs> ReturnInvoiceMOPs(int invoiceID, object[] objPageDetails)
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
        private List<InvoiceMOPs> ReturnInvoiceMOPsCurrentSale(int invoiceID, object[] objPageDetails)
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
        private List<InvoiceMOPs> ReturnPurchaseMOPsCurrentSale(int receiptID, object[] objPageDetails)
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
        private List<InvoiceMOPs> ReturnReceiptMOPsPurchase(int receiptID, object[] objPageDetails)
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
        private void AddNewMopToList(InvoiceMOPs invoicePayment, object[] objPageDetails)
        {
            string strQueryName = "AddNewMopToList";
            string sqlCmd = "INSERT INTO tbl_currentSalesMops VALUES(@intInvoiceID, @intPaymentID, @fltAmountPaid, @fltTenderedAmount, "
                + "@fltCustomerChange)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoicePayment.intInvoiceID },
                new object[] { "@intPaymentID", invoicePayment.intPaymentID },
                new object[] { "@fltAmountPaid", Math.Round(invoicePayment.fltAmountPaid, 2) },
                new object[] { "@fltTenderedAmount", Math.Round(invoicePayment.fltTenderedAmount, 2) },
                new object[] { "@fltCustomerChange", Math.Round(invoicePayment.fltCustomerChange, 2) }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void AddNewMopToReceiptList(InvoiceMOPs payment, object[] objPageDetails)
        {
            string strQueryName = "AddNewMopToReceiptList";
            string sqlCmd = "INSERT INTO tbl_currentPurchaseMops VALUES(@intReceiptID, @intPaymentID, @intChequeNumber, @fltAmountPaid)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", payment.intInvoiceID },
                new object[] { "@intPaymentID", payment.intPaymentID },
                new object[] { "@intChequeNumber", payment.intChequeNumber },
                new object[] { "@fltAmountPaid", Math.Round(payment.fltAmountPaid, 2) }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveMopFromList(int invoicePaymentID, object[] objPageDetails)
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
        private void RemoveMopFromPurchaseList(int receiptPaymentID, int receiptID, object[] objPageDetails)
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



        //Public calls
        public int ReturnMopIntForTable(string mopName, object[] objPageDetails)
        {
            return ReturnMOPInt(mopName, objPageDetails);
        }
        public List<InvoiceMOPs> CallReturnInvoiceMOPsCurrentSale(int invoiceID, object[] objPageDetails)
        {
            return ReturnInvoiceMOPsCurrentSale(invoiceID, objPageDetails);
        }
        public List<InvoiceMOPs> CallReturnInvoiceMOPs(int invoiceID, object[] objPageDetails)
        {
            return ReturnInvoiceMOPs(invoiceID, objPageDetails);
        }
        public List<InvoiceMOPs> CallReturnPurchaseMOPsCurrentSale(int receiptID, object[] objPageDetails)
        {
            return ReturnPurchaseMOPsCurrentSale(receiptID, objPageDetails);
        }
        public List<InvoiceMOPs> CallReturnReceiptMOPsPurchase(int receiptID, object[] objPageDetails)
        {
            return ReturnReceiptMOPsPurchase(receiptID, objPageDetails);
        }
        public void CallRemoveMopFromList(int invoicePaymentID, object[] objPageDetails)
        {
            RemoveMopFromList(invoicePaymentID, objPageDetails);
        }
        public void CallAddNewMopToList(InvoiceMOPs invoicePayment, object[] objPageDetails)
        {
            AddNewMopToList(invoicePayment, objPageDetails);
        }
        public void CallAddNewMopToReceiptList(InvoiceMOPs payment, object[] objPageDetails)
        {
            AddNewMopToReceiptList(payment, objPageDetails);
        }
        public void CallRemoveMopFromPurchaseList(int receiptPaymentID, int receiptID, object[] objPageDetails)
        {
            RemoveMopFromPurchaseList(receiptPaymentID, receiptID, objPageDetails);
        }

    }
}