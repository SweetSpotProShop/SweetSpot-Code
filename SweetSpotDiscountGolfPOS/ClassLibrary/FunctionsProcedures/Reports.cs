using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS.FP
{
    public class Reports
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();

        public Reports() { }

        //Report Tracking
        public void CallReportLogger(object[] reportLog, object[] objPageDetails)
        {
            LogReportCall(reportLog, objPageDetails);
        }
        private void LogReportCall(object[] reportLog, object[] objPageDetails)
        {
            string strQueryName = "logReportCall";
            string sqlCmd = "INSERT INTO tbl_reportView VALUES(@intReportID, @dtmReportClickedDate, @dtmReportClickedTime, "
                + "@intEmployeeID, @intLocationID)";
            object[][] parms =
            {
                new object[] { "@intReportID", Convert.ToInt32(reportLog[0]) },
                new object[] { "@dtmReportClickedDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@dtmReportClickedTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@intEmployeeID", Convert.ToInt32(reportLog[1]) },
                new object[] { "@intLocationID", Convert.ToInt32(reportLog[2]) }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //*******************HOME PAGE SALES*******************************************************
        //Nathan built for home page sales display
        private System.Data.DataTable GetInvoiceBySaleDate(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "getInvoiceBySaleDate";
            //Gets a list of all invoices based on date and location. Stores in a list
            string sqlCmd = "SELECT dtmInvoiceDate, dtmInvoiceTime, I.intInvoiceID, I.varInvoiceNumber, I.intInvoiceSubNumber, intCustomerID, CONCAT("
                + "E.varLastName, ', ', E.varFirstName) AS employeeName, fltSubTotal, fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = 1 "
                + "THEN fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 "
                + "END AS fltProvincialTaxAmount, CASE WHEN bitChargeLCT = 1 THEN fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, (fltBalanceDue "
                + "+ CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount ELSE 0 END + CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE "
                + "0 END + CASE WHEN bitChargeLCT = 1 THEN fltLiquorTaxAmount ELSE 0 END) AS fltBalanceDue, varPaymentName, fltAmountPaid FROM "
                + "tbl_invoice I JOIN tbl_employee E ON E.intEmployeeID = I.intEmployeeID INNER JOIN tbl_invoiceMOP IM ON IM.intInvoiceID = "
                + "I.intInvoiceID INNER JOIN tbl_methodOfPayment MOP ON MOP.intPaymentID = IM.intPaymentID WHERE dtmInvoiceDate BETWEEN @startDate "
                + "AND @endDate AND I.intLocationID = @intLocationID ORDER BY dtmInvoiceDate DESC, dtmInvoiceTime DESC";

            object[][] parms =
            {
                 new object[] { "@startDate", startDate },
                 new object[] { "@endDate", endDate },
                 new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallGetInvoiceBySaleDate(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return GetInvoiceBySaleDate(startDate, endDate, locationID, objPageDetails);
        }

        //******************COST OF INVENTORY REPORTING*******************************************************
        private System.Data.DataTable CostOfInventoryReport(object[] objPageDetails)
        {
            string strQueryName = "costOfInventoryReport";
            string sqlCmd = "SELECT L.varLocationName, ROUND(ISNULL(fltItemAccessoryCost, 0), 2) AS fltAccessoriesCost, ROUND(ISNULL(fltItemClothingCost, 0), 2) AS "
                + "fltClothingCost, ROUND(ISNULL(fltItemClubsCost, 0), 2) AS fltClubsCost FROM (SELECT CONCAT(varLocationName, '-', varCityName) AS varLocationName "
                + "FROM tbl_location WHERE bitIsRetailStore = 1) L FULL JOIN(SELECT CONCAT(L.varLocationName, '-', L.varCityName) AS varLocationName, ROUND(SUM("
                + "fltCost * intQuantity), 2) AS fltItemAccessoryCost FROM tbl_accessories A JOIN tbl_location L ON L.intLocationID = A.intLocationID WHERE "
                + "intQuantity > 0 AND L.bitIsRetailStore = 1 GROUP BY L.varLocationName, L.varCityName) AC ON AC.varLocationName = L.varLocationName FULL JOIN("
                + "SELECT CONCAT(L.varLocationName, '-', L.varCityName) AS varLocationName, ROUND(SUM(fltCost * intQuantity), 2) AS fltItemClothingCost FROM "
                + "tbl_clothing CL JOIN tbl_location L ON L.intLocationID = CL.intLocationID WHERE intQuantity > 0 AND bitIsRetailStore = 1 GROUP BY "
                + "L.varLocationName, L.varCityName) CLC ON CLC.varLocationName = L.varLocationName FULL JOIN(SELECT CONCAT(L.varLocationName, '-', L.varCityName) "
                + "AS varLocationName, ROUND(SUM(fltCost * intQuantity), 2) AS fltItemClubsCost FROM tbl_clubs C JOIN tbl_location L ON L.intLocationID = "
                + "C.intLocationID WHERE intQuantity > 0 AND bitIsRetailStore = 1 GROUP BY L.varLocationName, L.varCityName) CC ON CC.varLocationName = "
                + "L.varLocationName";

            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallCostOfInventoryReport(object[] objPageDetails)
        {
            return CostOfInventoryReport(objPageDetails);
        }

        //*******************CASHOUT UTILITIES*******************************************************
        //Matches new Database Calls
        public int VerifyCashoutCanBeProcessed(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            int indicator = 0;
            if (TransactionsAvailable(locationID, selectedDate, objPageDetails))
            {
                if (OpenTransactions(locationID, selectedDate, objPageDetails))
                {
                    indicator = 2;
                }
                else if (CashoutAlreadyDone(locationID, selectedDate, objPageDetails))
                {
                    indicator = 3;
                }
            }
            else { indicator = 1; }
            return indicator;
        }
        private bool TransactionsAvailable(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "transactionsAvailable";
            bool bolTA = false;
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND "
                + "@dtmEndDate AND intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmEndDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intLocationID", locationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }
        private bool CashoutAlreadyDone(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "cashoutAlreadyDone";
            bool bolCAD = false;
            string sqlCmd = "SELECT COUNT(dtmCashoutDate) FROM tbl_cashout WHERE dtmCashoutDate BETWEEN @dtmStartDate AND "
                + "@dtmEndDate AND intLocationID = @intLocationID AND bitIsCashoutProcessed = 1";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmEndDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intLocationID", locationID }
            };
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolCAD = true;
            }
            return bolCAD;
        }
        private bool OpenTransactions(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "openTransactions";
            bool bolOT = false;
            string sqlCmd = "SELECT COUNT(intCurrentInvoiceID) FROM tbl_currentSalesInvoice WHERE intTransactionTypeID = 1 "
                + "AND intLocationID = @locationID AND dtmInvoiceDate = @dtmInvoiceDate";
            object[][] parms =
            {
                new object[] { "@locationID", locationID },
                new object[] { "@invoiceDate", selectedDate.ToString("yyyy-MM-dd")}
            };
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolOT = true;
            }
            return bolOT;
        }
        public void RemoveUnprocessedReturns(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            System.Data.DataTable dt = ReturnListOfReturnInvoices(locationID, selectedDate, objPageDetails);

            foreach (DataRow dtr in dt.Rows)
            {
                RemoveReturnMopsFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
                RemoveReturnItemTaxesFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
                RemoveReturnItemsFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
                RemoveReturnInvoiceFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
            }
        }

        private System.Data.DataTable ReturnListOfReturnInvoices(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "ReturnListOfReturnInvoices";
            string sqlCmd = "SELECT intInvoiceID FROM tbl_currentSalesInvoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate "
                + "AND intLocationID = @intLocationID AND intTransactionTypeID = 2";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmEndDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveReturnMopsFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnMopsFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveReturnItemTaxesFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnItemTaxesFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesItemsTaxes FROM tbl_currentSalesItemsTaxes CSIT JOIN tbl_currentSalesItems CSI ON "
                + "CSI.intInvoiceItemID = CSIT.intInvoiceItemID WHERE CSI.intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveReturnItemsFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnItemsFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveReturnInvoiceFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnInvoiceFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public Cashout CreateNewCashout(DateTime startDate, int locationID, object[] objPageDetails)
        {
            ////Now need to account for anything in Layaway status
            ////These queries will need to be updated
            //Return PivotTable of a list of Mops
            System.Data.DataTable dt1 = ReturnListOfMOPS(startDate, locationID, objPageDetails);

            //Gather remaining Totals (taxes, subtotal(including shipping), tradein)
            System.Data.DataTable dt2 = ReturnAdditionTotalsForCashout(startDate, locationID, objPageDetails);

            //Save all into a cashout and return
            Cashout cashout = new Cashout
            {
                dtmCashoutDate = startDate,
                intLocationID = locationID,
                fltSystemCountedBasedOnSystemTradeIn = Convert.ToDouble(dt2.Rows[0][0].ToString()) * -1,
                fltSystemCountedBasedOnSystemGiftCard = Convert.ToDouble(dt1.Rows[0][3].ToString()),
                fltSystemCountedBasedOnSystemCash = Convert.ToDouble(dt1.Rows[0][1].ToString()),
                fltSystemCountedBasedOnSystemDebit = Convert.ToDouble(dt1.Rows[0][2].ToString()),
                fltSystemCountedBasedOnSystemMastercard = Convert.ToDouble(dt1.Rows[0][4].ToString()),
                fltSystemCountedBasedOnSystemVisa = Convert.ToDouble(dt1.Rows[0][5].ToString()),
                fltSalesSubTotal = Convert.ToDouble(dt2.Rows[0][1].ToString()),
                fltGovernmentTaxAmount = Convert.ToDouble(dt2.Rows[0][2].ToString()),
                fltLiquorTaxAmount = Convert.ToDouble(dt2.Rows[0][4].ToString()),
                fltProvincialTaxAmount = Convert.ToDouble(dt2.Rows[0][3].ToString())

                //cashout.fltHarmonizedTaxAmount = Convert.ToDouble(dt2.Rows[0][3].ToString());
                //cashout.fltLiquorTaxAmount = Convert.ToDouble(dt2.Rows[0][4].ToString());
                //cashout.fltProvincialTaxAmount = Convert.ToDouble(dt2.Rows[0][5].ToString());
                //cashout.fltQuebecTaxAmount = Convert.ToDouble(dt2.Rows[0][6].ToString());
                //cashout.fltRetailTaxAmount = Convert.ToDouble(dt2.Rows[0][7].ToString());
                //cashout.intSalesCount = Convert.ToDouble(dt2.Rows[0][8].ToString());
            };

            return cashout;
        }
        private System.Data.DataTable ReturnListOfMOPS(DateTime startDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnListOfMOPS";
            string sqlCmd = "SELECT dtmInvoiceDate, ISNULL([5], 0) AS Cash, ISNULL([7], 0) AS Debit, ISNULL([6], 0) AS GiftCard, ISNULL([2], 0) AS Mastercard, "
                + "ISNULL([1],0) AS Visa FROM(SELECT i.dtmInvoiceDate, m.intPaymentID, SUM(fltAmountPaid) AS fltAmountPaid FROM tbl_invoiceMOP m JOIN "
                + "tbl_invoice i ON m.intInvoiceID = i.intInvoiceID WHERE i.dtmInvoiceDate = @startDate AND i.intLocationID = @intLocationID GROUP BY "
                + "i.dtmInvoiceDate, m.intPaymentID) ps PIVOT(SUM(fltAmountPaid) FOR intPaymentID IN([5], [7], [6], [2], [1])) AS pvt";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private System.Data.DataTable ReturnAdditionTotalsForCashout(DateTime startDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnAdditionTotalsForCashout";
            //string sqlCmd = "SELECT ISNULL(SUM(I.fltTotalTradeIn), 0) AS fltTotalTradeIn, ISNULL(SUM(I.fltSubTotal), 0) + ISNULL(SUM(I.fltShippingCharges), 0) AS "
            //    + "fltSalesSubTotal, (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltGovernmentTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem "
            //    + "II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate "
            //    + "AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = "
            //    + "'GST' OR varTaxName = 'HST')) AS fltGovernmentTaxAmount, (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltProvincialTaxAmount FROM "
            //    + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
            //    + "II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
            //    + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'PST' OR varTaxName = 'RST' OR varTaxName = 'QST')) AS fltProvincialTaxAmount, (SELECT "
            //    + "ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltLiquorTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
            //    + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = "
            //    + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'LCT')) AS "
            //    + "fltLiquorTaxAmount FROM tbl_invoice I WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID";

            string sqlCmd = "SELECT ISNULL(SUM(I.fltTotalTradeIn), 0) AS fltTotalTradeIn, ISNULL(SUM(I.fltSubTotal), 0) + ISNULL(SUM(I.fltShippingCharges), 0) AS "
                + "fltSalesSubTotal, (SELECT SUM(GT.fltTaxAmount) AS fltGovernmentTaxAmount FROM(SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'GST' OR varTaxName = 'HST') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS "
                + "fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON "
                + "I.intInvoiceID = IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND "
                + "IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'GST' OR varTaxName = 'HST')) AS GT) AS fltGovernmentTaxAmount, (SELECT "
                + "SUM(PT.fltTaxAmount) AS fltProvincialTaxAmount FROM(SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT "
                + "JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE "
                + "I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM "
                + "tbl_taxType WHERE varTaxName = 'PST' OR varTaxName = 'RST' OR varTaxName = 'QST') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS "
                + "fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON "
                + "I.intInvoiceID = IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND "
                + "IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'PST' OR varTaxName = 'RST' OR varTaxName = 'QST')) AS PT) AS "
                + "fltProvincialTaxAmount, (SELECT SUM(LT.fltTaxAmount) AS fltLiquorTaxAmount FROM(SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount "
                + "FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'LCT') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'LCT')) AS LT) AS fltLiquorTaxAmount FROM tbl_invoice I WHERE I.dtmInvoiceDate = "
                + "@dtmStartDate AND I.intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //Insert the cashout into the database
        private void InsertCashout(Cashout cashout, object[] objPageDetails)
        {
            string strQueryName = "insertCashout";
            string sqlCmd = "INSERT INTO tbl_cashout VALUES(@dtmCashoutDate, @dtmCashoutTime, @intLocationID, @intEmployeeID, "

                + "@fltSystemCountedBasedOnSystemTradeIn, @fltSystemCountedBasedOnSystemGiftCard, @fltSystemCountedBasedOnSystemCash, "
                + "@fltSystemCountedBasedOnSystemDebit, @fltSystemCountedBasedOnSystemMastercard, @fltSystemCountedBasedOnSystemVisa, "

                + "@fltManuallyCountedBasedOnReceiptsTradeIn, @fltManuallyCountedBasedOnReceiptsGiftCard, @fltManuallyCountedBasedOnReceiptsCash, "
                + "@fltManuallyCountedBasedOnReceiptsDebit, @fltManuallyCountedBasedOnReceiptsMastercard, @fltManuallyCountedBasedOnReceiptsVisa, "

                + "@fltSalesSubTotal, @fltGovernmentTaxAmount, @fltProvincialTaxAmount, @fltLiquorTaxAmount, "
                + "@fltCashDrawerOverShort, @bitIsCashoutFinalized, @bitIsCashoutProcessed)";

            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", cashout.dtmCashoutDate.ToShortDateString() },
                new object[] { "@dtmCashoutTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@intLocationID", cashout.intLocationID },
                new object[] { "@intEmployeeID", cashout.intEmployeeID },

                new object[] { "@fltSystemCountedBasedOnSystemTradeIn", cashout.fltSystemCountedBasedOnSystemTradeIn },
                new object[] { "@fltSystemCountedBasedOnSystemGiftCard", cashout.fltSystemCountedBasedOnSystemGiftCard },
                new object[] { "@fltSystemCountedBasedOnSystemCash", cashout.fltSystemCountedBasedOnSystemCash },
                new object[] { "@fltSystemCountedBasedOnSystemDebit", cashout.fltSystemCountedBasedOnSystemDebit },
                new object[] { "@fltSystemCountedBasedOnSystemMastercard", cashout.fltSystemCountedBasedOnSystemMastercard },
                new object[] { "@fltSystemCountedBasedOnSystemVisa", cashout.fltSystemCountedBasedOnSystemVisa },

                new object[] { "@fltManuallyCountedBasedOnReceiptsTradeIn", cashout.fltManuallyCountedBasedOnReceiptsTradeIn },
                new object[] { "@fltManuallyCountedBasedOnReceiptsGiftCard", cashout.fltManuallyCountedBasedOnReceiptsGiftCard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsCash", cashout.fltManuallyCountedBasedOnReceiptsCash },
                new object[] { "@fltManuallyCountedBasedOnReceiptsDebit", cashout.fltManuallyCountedBasedOnReceiptsDebit },
                new object[] { "@fltManuallyCountedBasedOnReceiptsMastercard", cashout.fltManuallyCountedBasedOnReceiptsMastercard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsVisa", cashout.fltManuallyCountedBasedOnReceiptsVisa },

                new object[] { "@fltSalesSubTotal", cashout.fltSalesSubTotal },
                new object[] { "@fltGovernmentTaxAmount", cashout.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", cashout.fltProvincialTaxAmount },
                new object[] { "@fltLiquorTaxAmount", cashout.fltLiquorTaxAmount },
                new object[] { "@fltCashDrawerOverShort", cashout.fltCashDrawerOverShort },
                new object[] { "@bitIsCashoutFinalized", cashout.bitIsCashoutFinalized },
                new object[] { "@bitIsCashoutProcessed", cashout.bitIsCashoutProcessed }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CallInsertCashout(Cashout cashout, object[] objPageDetails)
        {
            InsertCashout(cashout, objPageDetails);
        }

        //******************PURCHASES REPORTING*******************************************************
        //Matches new Database Calls
        public int VerifyPurchasesMade(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!PurchasesAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool PurchasesAvailable(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "purchasesAvailable";
            bool bolTA = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT COUNT(receiptNumber) FROM tbl_receipt "
                        + "WHERE receiptDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTA = true;
            }

            return bolTA;
        }
        private bool TradeInsHaveBeenProcessed(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "tradeinsHaveBeenProcessed";
            bool bolTI = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice WHERE invoiceDate BETWEEN @startDate AND @endDate AND locationID = @locationID AND tradeInAmount < 0";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTI = true;
            }
            return bolTI;
        }

        //public List<Purchases> returnPurchasesDuringDates(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        //{
        //    string strQueryName = "returnPurchasesDuringDates";
        //    string sqlCmd = "SELECT R.receiptNumber, R.receiptDate, D.methodDesc, M.chequeNum, "
        //        + "M.amountPaid FROM tbl_receipt R INNER JOIN tbl_receiptMOP M ON R.receiptNumber "
        //        + "= M.receiptNum INNER JOIN tbl_methodOfPayment D ON M.mopType = D.methodID WHERE "
        //        + "R.receiptDate BETWEEN @startDate AND @endDate AND R.locationID = @locationID";
        //    object[][] parms =
        //    {
        //        new object[] { "@startDate", startDate },
        //        new object[] { "@endDate", endDate },
        //        new object[] { "@locationID", locationID }
        //    };

        //    return returnPurchasesFromDataTable(dbc.returnDataTableData(sqlCmd, parms));
        //    //return returnPurchasesFromDataTable(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        //}
        //private List<Purchases> returnPurchasesFromDataTable(System.Data.DataTable dt)
        //{
        //    List<Purchases> purchase = dt.AsEnumerable().Select(row =>
        //    new Purchases
        //    {
        //        receiptNumber = row.Field<int>("receiptNumber"),
        //        receiptDate = row.Field<DateTime>("receiptDate"),
        //        mopDescription = row.Field<string>("methodDesc"),
        //        chequeNumber = row.Field<int>("chequeNum"),
        //        amountPaid = row.Field<double>("amountPaid")
        //    }).ToList();
        //    return purchase;
        //}
        //******************MARKETING REPORTING*******************************************************
        private System.Data.DataTable MostSoldItemsReport(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "mostSoldItemsReport";
            string sqlCmd = "SELECT TOP (10) II.sku, SUM(II.quantity) AS amountSold FROM tbl_invoiceItem II JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum AND "
                + "I.invoiceSubNum = II.invoiceSubNum WHERE II.sku NOT IN(SELECT sku FROM tbl_tempTradeInCartSkus) AND I.invoiceDate BETWEEN @startDate AND @endDate "
                + "AND I.locationID = @locationID GROUP BY sku ORDER BY amountSold DESC";

            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallMostSoldItemsReport(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return MostSoldItemsReport(startDate, endDate, locationID, objPageDetails);
        }
        private System.Data.DataTable MostSoldBrandsReport(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "mostSoldBrandsReport";
            string sqlCmd = "SELECT TOP(10) BB.brand, SUM(II.quantity) AS amountSold FROM tbl_invoiceItem II JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum AND "
                + "I.invoiceSubNum = II.invoiceSubNum JOIN(SELECT A.sku, (SELECT B.brandName AS brand FROM tbl_accessories AC JOIN tbl_brand B ON AC.brandID = B.brandID "
                + "WHERE AC.sku = A.sku) AS brand FROM tbl_accessories A UNION SELECT CL.sku, (SELECT B.brandName AS brand FROM tbl_clothing CLO JOIN tbl_brand B ON "
                + "CLO.brandID = B.brandID WHERE CLO.sku = CL.sku) AS brand FROM tbl_clothing CL UNION SELECT C.sku, (SELECT B.brandName AS brand FROM tbl_clubs CLU "
                + "JOIN tbl_brand B ON CLU.brandID = B.brandID WHERE CLU.sku = C.sku) AS brand FROM tbl_clubs C) BB ON BB.sku = II.sku WHERE I.invoiceDate BETWEEN "
                + "@dtmStartDate AND @dtmEndDate AND I.locationID = @locationID AND II.isTradeIn = 0 GROUP BY brand ORDER BY amountSold DESC";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallMostSoldBrandsReport(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return MostSoldBrandsReport(startDate, endDate, locationID, objPageDetails);
        }
        private System.Data.DataTable MostSoldModelsReport(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "mostSoldModelsReport";
            string sqlCmd = "SELECT TOP (10) MM.model, SUM(II.quantity) AS amountSold FROM tbl_invoiceItem II JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum AND "
                + "I.invoiceSubNum = II.invoiceSubNum JOIN(SELECT A.sku, (SELECT M.modelName AS model FROM tbl_accessories AC JOIN tbl_model M ON AC.modelID = M.modelID "
                + "WHERE AC.sku = A.sku) AS model FROM tbl_accessories A UNION SELECT C.sku, (SELECT M.modelName AS model FROM tbl_clubs CLU JOIN tbl_model M ON "
                + "CLU.modelID = M.modelID WHERE CLU.sku = C.sku) AS model FROM tbl_clubs C) MM ON MM.sku = II.sku WHERE I.invoiceDate BETWEEN @dtmStartDate AND "
                + "@dtmEndDate AND I.locationID = @locationID  AND II.isTradeIn = 0 GROUP BY model ORDER BY amountSold DESC";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallMostSoldModelsReport(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return MostSoldModelsReport(startDate, endDate, locationID, objPageDetails);
        }


        //******************COGS and PM REPORTING*******************************************************
        private System.Data.DataTable ReturnInvoicesForCOGS(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "returnInvoicesForCOGS";
            //This method returns a type of invoice for a report
            string sqlCmd = "SELECT CONCAT(I.invoiceNum, '-', I.invoiceSubNum) AS 'invoice', II.totalPrice, II.totalCost, II.totalDiscount, II.percentage, CASE WHEN "
                + "II.percentage = 1 AND II.totalPrice <> 0 THEN CAST(ROUND((((II.totalPrice - (II.totalPrice * II.totalDiscount) / 100) - II.totalCost) / II.totalPrice) "
                + "* 100, 2) AS varchar) WHEN II.percentage = 0 AND II.totalPrice <> 0 THEN CAST(ROUND(((II.totalPrice - II.totalDiscount - II.totalCost) / II.totalPrice) "
                + "* 100, 2) AS varchar) WHEN II.totalPrice = 0 THEN 'N/A' ELSE 'N/A' END AS 'totalProfit' FROM tbl_invoice I JOIN(SELECT invoiceNum, invoiceSubNum, sku, "
                + "SUM(price * quantity) AS totalPrice, SUM(cost * quantity) AS totalCost, CASE WHEN percentage = 1 THEN SUM(itemDiscount) ELSE SUM(itemDiscount * quantity) "
                + "END AS totalDiscount, percentage FROM tbl_invoiceItem GROUP BY invoiceNum, invoiceSubNum, sku, percentage) AS II ON II.invoiceNum = I.invoiceNum AND "
                + "II.invoiceSubNum = I.invoiceSubNum WHERE II.sku NOT IN(SELECT sku FROM tbl_tempTradeInCartSkus) AND II.invoiceNum NOT IN(SELECT invoiceNum FROM "
                + "tbl_invoiceItemReturns) AND I.locationID = @locationID AND I.invoiceDate BETWEEN @startDate AND @endDate ORDER BY I.invoiceNum, I.invoiceSubNum";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnInvoicesForCOGS(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return ReturnInvoicesForCOGS(startDate, endDate, locationID, objPageDetails);
        }
        public int VerifyInvoicesCompleted(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!InvoicesCompleted(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool InvoicesCompleted(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "invoicesCompleted";
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE intLocationID = @intLocationID AND (dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate)";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] },
                new object[] { "@intLocationID", Convert.ToInt32(repInfo[1]) }
            };
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }

        //******************Sales by Date Report*******************************************************
        //Matches new Database Calls
        public int VerifySalesHaveBeenMade(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!TransactionsAvailableOverMultipleDates(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool TransactionsAvailableOverMultipleDates(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "transactionsAvailableOverMultipleDates";
            bool bolTA = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] },
                new object[] { "@intLocationID", Convert.ToInt32(repInfo[1]) }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }
        private System.Data.DataTable ReturnSalesForSelectedDate(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnSalesForSelectedDate";
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT dtmInvoiceDate, ROUND(SUM(CASE WHEN bitChargeLCT = 1 THEN fltLiquorTaxAmount ELSE 0 END), 2) AS fltLiquorTaxAmount, ROUND(SUM(CASE WHEN "
                + "bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END), 2) AS fltProvincialTaxAmount, ROUND(SUM(CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount "
                + "ELSE 0 END), 2) AS fltGovernmentTaxAmount, ROUND(SUM(fltBalanceDue), 2) AS fltTotalSales FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND "
                + "@dtmEndDate AND intLocationID = @intLocationID GROUP BY dtmInvoiceDate";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] },
                new object[] { "@intLocationID", Convert.ToInt32(repInfo[1]) }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnSalesForSelectedDate(object[] repInfo, object[] objPageDetails)
        {
            return ReturnSalesForSelectedDate(repInfo, objPageDetails);
        }

        //******************Trade Ins by Date Report*******************************************************
        public int VerifyTradeInsHaveBeenMade(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!TradeInsHaveBeenProcessed(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private System.Data.DataTable ReturnTradeInsForSelectedDate(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnTradeInsForSelectedDate";
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT invoiceDate, SUM(tradeInAmount) AS totalTradeIns FROM tbl_invoice WHERE invoiceDate BETWEEN @startDate AND @endDate "
                + "AND locationID = @locationID GROUP BY invoiceDate";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnTradeInsForSelectedDate(object[] repInfo, object[] objPageDetails)
        {
            return ReturnTradeInsForSelectedDate(repInfo, objPageDetails);
        }

        //******************Sales by Payment Type By Date Report***************************************
        private System.Data.DataTable ReturnSalesByPaymentTypeForSelectedDate(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnSalesByPaymentTypeForSelectedDate";
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT invoiceDate, ISNULL([Cash],0) AS Cash, ISNULL([Debit],0) AS Debit, ISNULL([Gift Card],0) AS GiftCard, ISNULL([MasterCard],0) "
                + "AS Mastercard, ISNULL([Visa],0) AS Visa FROM(SELECT i.invoiceDate, m.mopType, SUM(amountPaid) AS totalPaid FROM tbl_invoiceMOP m JOIN tbl_invoice "
                + "i ON m.invoiceNum = i.invoiceNum AND m.invoiceSubNum = i.invoiceSubNum WHERE i.invoiceDate BETWEEN @startDate AND @endDate AND i.locationID = "
                + "@locationID GROUP BY i.invoiceDate, m.mopType) ps PIVOT(SUM(totalPaid) FOR mopType IN([Cash], [Debit], [Gift Card], [MasterCard], [Visa])) AS pvt";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnSalesByPaymentTypeForSelectedDate(object[] repInfo, object[] objPageDetails)
        {
            return ReturnSalesByPaymentTypeForSelectedDate(repInfo, objPageDetails);
        }
        //This method is an updated import item method that runs cleaner and should produce less errors
        private System.Data.DataTable UploadItems(FileUpload fup, CurrentUser cu, object[] objPageDetails)
        {

            //***************************************************************************************************
            //Step 1: Create datatable to hold the items found in the excel sheet
            //***************************************************************************************************
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            //Datatable to hold any skus that have errors
            using (System.Data.DataTable skusWithErrors = new System.Data.DataTable())
            {
                //skusWithErrors.Columns.Add("sku");
                //skusWithErrors.Columns.Add("brandError");
                //skusWithErrors.Columns.Add("modelError");
                //skusWithErrors.Columns.Add("identifierError");
                //This datatable can hold all items
                using (System.Data.DataTable listItems = new System.Data.DataTable())
                {
                    listItems.Columns.Add("varSku"); //("sku");
                    listItems.Columns.Add("varBrandName"); //("brandName");
                    listItems.Columns.Add("varModelName"); //("modelName");
                    listItems.Columns.Add("fltCost"); //("cost");
                    listItems.Columns.Add("fltPrice"); //("price");
                    listItems.Columns.Add("intQuantity"); //("quantity");
                    listItems.Columns.Add("varAdditionalInformation"); //("comments");
                    listItems.Columns.Add("fltPremiumCharge"); //("premium");
                    listItems.Columns.Add("varTypeOfClub"); //("clubType");
                    listItems.Columns.Add("varShaftType"); //("shaft");
                    listItems.Columns.Add("varNumberOfClubs"); //("numberOfClubs");
                    listItems.Columns.Add("varClubSpecification"); //("clubSpec");
                    listItems.Columns.Add("varShaftSpecification"); //("shaftSpec");
                    listItems.Columns.Add("varShaftFlexability"); //("shaftFlex");
                    listItems.Columns.Add("varClubDexterity"); //("dexterity");
                    listItems.Columns.Add("varLocationName"); //("locationName");
                    listItems.Columns.Add("varItemType"); //("itemType");
                    listItems.Columns.Add("bitIsUsedProduct"); //
                                                               //listItems.Columns.Add//("size");
                                                               //listItems.Columns.Add//("colour");

                    //Database connections
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlConnection conTempDB = new SqlConnection(connectionString);
                        using (SqlConnection conInsert = new SqlConnection(connectionString))
                        {
                        }
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            SqlDataReader reader;

                            cmd.CommandText = "IF OBJECT_ID('tempItemStorage', 'U') IS NOT NULL DROP TABLE tempItemStorage; IF OBJECT_ID('tempErrorSkus', 'U') IS NOT NULL DROP TABLE tempErrorSkus;";
                            conTempDB.Open();
                            cmd.Connection = conTempDB;
                            reader = cmd.ExecuteReader();
                            conTempDB.Close();

                            //***************************************************************************************************
                            //Step 2: Check to see if there is any data in the uploaded file
                            //***************************************************************************************************

                            //If there are files, proceed
                            if (fup.HasFiles)
                            {

                                //***************************************************************************************************
                                //Step 3: Create an excel sheet and set its content to the uploaded file
                                //***************************************************************************************************

                                //Load the uploaded file into the memorystream
                                using (MemoryStream stream = new MemoryStream(fup.FileBytes))
                                //Lets the server know to use the excel package
                                using (ExcelPackage xlPackage = new ExcelPackage(stream))
                                {
                                    //con = new SqlConnection(connectionString);
                                    // get the first worksheet in the workbook
                                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                                    var rowCnt = worksheet.Dimension.End.Row; //Gets the row count                   
                                    var colCnt = worksheet.Dimension.End.Column; //Gets the column count

                                    //***************************************************************************************************
                                    //Step 4: Looping through the data found in the excel sheet and storing it in the datatable
                                    //***************************************************************************************************

                                    //Beginning the loop for data gathering
                                    for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                                    {
                                        string itemType = (worksheet.Cells[i, 5].Value).ToString(); //Column 25 = itemType
                                                                                                    //Adding items to the datatables 
                                                                                                    //***************************************************************************************************
                                                                                                    //Step 4: Option A: The item type is Apparel
                                                                                                    //***************************************************************************************************
                                        if (itemType.Equals("Apparel"))
                                        {
                                            listItems.Rows.Add(
                                                //***************SKU**********************
                                                worksheet.Cells[i, 3].Value.ToNullSafeString(),
                                                //***************BRAND NAME***************
                                                itemType.ToString(),
                                                //***************MODEL NAME***************        
                                                (string)(worksheet.Cells[i, 6].Value.ToNullSafeString()), //'N/A'
                                                                                                          //***************COST*********************
                                                Convert.ToDouble(worksheet.Cells[i, 12].Value),
                                                //***************PRICE********************
                                                Convert.ToDouble(worksheet.Cells[i, 15].Value),
                                                //***************QUANTITY*****************
                                                Convert.ToInt32(worksheet.Cells[i, 13].Value),
                                                //***************COMMENTS*****************
                                                (string)(worksheet.Cells[i, 16].Value.ToNullSafeString()),
                                                //***************PREMIUM******************
                                                Convert.ToDouble(0),
                                                //***************CLUB TYPE****************
                                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()), //Style for clothing
                                                                                                          //***************SHAFT********************
                                                (string)(worksheet.Cells[i, 8].Value.ToNullSafeString()), //Colour for clothing
                                                                                                          //***************NUMBER OF CLUBS**********
                                                (string)(worksheet.Cells[i, 9].Value.ToNullSafeString()), //Size for clothing
                                                                                                          //***************CLUB SPEC****************
                                                (string)(worksheet.Cells[i, 18].Value.ToNullSafeString()), //Gender for clothing
                                                                                                           //***************SHAFT SPEC***************
                                                "",
                                                //***************SHAFT FLEX***************
                                                "",
                                                //***************DEXTERITY****************
                                                "",
                                                //***************LOCATION NAME************
                                                (string)(worksheet.Cells[i, 22].Value.ToNullSafeString()), //Second Location
                                                                                                           //***************ITEM TYPE****************
                                                3,
                                                //***************USED PRODUCT*************
                                                Convert.ToBoolean(worksheet.Cells[i, 26].Value)
                                            );
                                        }
                                        //***************************************************************************************************
                                        //Step 4: Option B: The item type is Accessories
                                        //***************************************************************************************************
                                        else if (itemType.Equals("Accessories"))
                                        {
                                            listItems.Rows.Add(
                                                //***************SKU***************
                                                worksheet.Cells[i, 3].Value.ToNullSafeString(),
                                                //***************BRAND NAME***************
                                                itemType.ToString(),
                                                //***************MODEL Name***************        
                                                (string)(worksheet.Cells[i, 6].Value.ToNullSafeString()),
                                                //***************COST***************
                                                Convert.ToDouble(worksheet.Cells[i, 12].Value),
                                                //***************PRICE***************
                                                Convert.ToDouble(worksheet.Cells[i, 15].Value),
                                                //***************QUANTITY***************
                                                Convert.ToInt32(worksheet.Cells[i, 13].Value),
                                                //***************COMMENTS***************
                                                (string)(worksheet.Cells[i, 16].Value.ToNullSafeString()),
                                                //***************PREMIUM***************
                                                Convert.ToDouble(0),
                                                //***************CLUB TYPE***************
                                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()), //accessoryType
                                                                                                          //***************SHAFT***************
                                                (string)(worksheet.Cells[i, 8].Value.ToNullSafeString()), //Colour for accessory
                                                                                                          //***************NUMBER OF CLUBS***************
                                                (string)(worksheet.Cells[i, 9].Value.ToNullSafeString()), //Size for accessory
                                                                                                          //***************CLUB SPEC***************
                                                "",
                                                //***************SHAFT SPEC***************
                                                "",
                                                //***************SHAFT FLEX***************
                                                "",
                                                //***************DEXTERITY***************
                                                "",
                                                //***************LOCATION NAME***************
                                                (string)(worksheet.Cells[i, 22].Value.ToNullSafeString()),
                                                //***************ITEM TYPE***************
                                                2,
                                                //***************USED PRODUCT*************
                                                Convert.ToBoolean(worksheet.Cells[i, 26].Value)
                                            );
                                        }
                                        //***************************************************************************************************
                                        //Step 3: Option D: The item type is a club
                                        //***************************************************************************************************
                                        else
                                        {
                                            listItems.Rows.Add(
                                                //***************SKU***************
                                                worksheet.Cells[i, 3].Value.ToNullSafeString(),
                                                //***************BRAND NAME***************
                                                itemType.ToString(),
                                                //***************MODEL Name***************        
                                                (string)(worksheet.Cells[i, 6].Value.ToNullSafeString()),
                                                //***************COST***************
                                                Convert.ToDouble(worksheet.Cells[i, 12].Value),
                                                //***************PRICE***************
                                                Convert.ToDouble(worksheet.Cells[i, 15].Value),
                                                //***************QUANTITY***************
                                                Convert.ToInt32(worksheet.Cells[i, 13].Value),
                                                //***************COMMENTS***************
                                                (string)(worksheet.Cells[i, 16].Value.ToNullSafeString()),
                                                //***************PREMIUM***************
                                                Convert.ToDouble(worksheet.Cells[i, 11].Value),
                                                //***************CLUB TYPE***************
                                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()),
                                                //***************SHAFT***************
                                                (string)(worksheet.Cells[i, 8].Value.ToNullSafeString()),
                                                //***************NUMBER OF CLUBS***************
                                                (string)(worksheet.Cells[i, 9].Value.ToNullSafeString()),
                                                //***************CLUB SPEC***************
                                                (string)(worksheet.Cells[i, 18].Value.ToNullSafeString()),
                                                //***************SHAFT SPEC***************
                                                (string)(worksheet.Cells[i, 19].Value.ToNullSafeString()),
                                                //***************SHAFT FLEX***************
                                                (string)(worksheet.Cells[i, 20].Value.ToNullSafeString()),
                                                //***************DEXTERITY***************
                                                (string)(worksheet.Cells[i, 21].Value.ToNullSafeString()),
                                                //***************LOCATION NAME***************
                                                (string)(worksheet.Cells[i, 22].Value.ToNullSafeString()),
                                                //***************ITEM TYPE***************
                                                1,
                                                //***************USED PRODUCT*************
                                                Convert.ToBoolean(worksheet.Cells[i, 26].Value)
                                            );
                                        }
                                    }

                                    //***************************************************************************************************
                                    //Step 5: Create the temp tables for storing the items and skus that cause an error
                                    //***************************************************************************************************

                                    //Creating the temp tables  
                                    conTempDB.Open();
                                    cmd.CommandText = "CREATE TABLE tempItemStorage(varSku VARCHAR(25), intBrandID INT, intModelID INT, varTypeOfClub VARCHAR(150), varShaftType VARCHAR(150), "
                                        + "varNumberOfClubs VARCHAR(150), fltPremiumCharge FLOAT, fltCost FLOAT, fltPrice FLOAT, intQuantity INT, varClubSpecification VARCHAR(150), "
                                        + "varShaftSpecification VARCHAR(150), varShaftFlexability VARCHAR(150), varClubDexterity VARCHAR(150), intItemTypeID INT, intLocationID INT, "
                                        + "varAdditionalInformation VARCHAR(500), bitIsUsedProduct BIT); CREATE TABLE tempErrorSkus(varSku VARCHAR(25), intBrandError INT, intModelError INT, "
                                        + "intIdentifierError INT)";
                                    cmd.Connection = conTempDB;
                                    reader = cmd.ExecuteReader();
                                    conTempDB.Close();

                                    //***************************************************************************************************
                                    //Step 6: Check each item in the datatable to see if it will cause an error. If not, insert into the temp item table
                                    //***************************************************************************************************

                                    foreach (DataRow row in listItems.Rows)
                                    {
                                        con.Open();
                                        //This query will look up the brand, model, and locationID of the item being passed in. 
                                        //If all three are found, it will insert the item into the tempItemStorage table.
                                        //If not, it is added to the tempErrorSkus table
                                        cmd.CommandText = "IF((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                            "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                            "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempItemStorage VALUES( " +
                                                                    "@varSku, " +
                                                                    "(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName), " +
                                                                    "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName), " +
                                                                    "@varTypeOfClub, @varShaftType, @varNumberOfClubs, @fltPremiumCharge, @fltCost, @fltPrice, @intQuantity, "
                                                                    + "@varClubSpecification, @varShaftSpecification, @varShaftFlexability, @varClubDexterity, @intItemTypeID, " +
                                                                    "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID), "
                                                                    + "@varAdditionalInformation, @bitIsUsedProduct) " +
                                                            "END " +
                                                        "ELSE IF(NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                                "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                                "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 0, 0) " +
                                                            "END " +
                                                        "ELSE IF ((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                                 "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                                            "BEGIN " +
                                                                    "INSERT INTO tempErrorSkus VALUES(@varSku, 0, 1, 0) " +
                                                            "END " +
                                                        "ELSE IF ((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                                 "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 0, 0, 1) " +
                                                            "END " +
                                                        "ELSE IF (NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                                 "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 1, 0) " +
                                                            "END " +
                                                        "ELSE IF (NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                                 "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 0, 1) " +
                                                            "END " +
                                                        "ELSE IF ((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 0, 1, 1) " +
                                                            "END " +
                                                        "ELSE IF (NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                                            "BEGIN " +
                                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 1, 1) " +
                                                            "END";
                                        cmd.Connection = con;
                                        cmd.Parameters.AddWithValue("@varSku", row[0]);
                                        cmd.Parameters.AddWithValue("@varBrandName", row[1]);
                                        cmd.Parameters.AddWithValue("@varModelName", row[2]);
                                        cmd.Parameters.AddWithValue("@fltCost", row[3]);
                                        cmd.Parameters.AddWithValue("@fltPrice", row[4]);
                                        cmd.Parameters.AddWithValue("@intQuantity", row[5]);
                                        cmd.Parameters.AddWithValue("@varAdditionalInformation", row[6]);
                                        cmd.Parameters.AddWithValue("@fltPremiumCharge", row[7]);
                                        cmd.Parameters.AddWithValue("@varTypeOfClub", row[8]);
                                        cmd.Parameters.AddWithValue("@varShaftType", row[9]);
                                        cmd.Parameters.AddWithValue("@varNumberOfClubs", row[10]);
                                        cmd.Parameters.AddWithValue("@varClubSpecification", row[11]);
                                        cmd.Parameters.AddWithValue("@varShaftSpecification", row[12]);
                                        cmd.Parameters.AddWithValue("@varShaftFlexability", row[13]);
                                        cmd.Parameters.AddWithValue("@varClubDexterity", row[14]);
                                        cmd.Parameters.AddWithValue("@varSecondLocationID", row[15]);
                                        cmd.Parameters.AddWithValue("@intItemTypeID", row[16]);
                                        cmd.Parameters.AddWithValue("@bitIsUsedProduct", row[17]);
                                        reader = cmd.ExecuteReader();
                                        con.Close();
                                    };

                                    //***************************************************************************************************
                                    //Step 7: Check the error list for any data
                                    //***************************************************************************************************

                                    //Reading the error list
                                    using (SqlCommand cmd2 = new SqlCommand("SELECT * FROM tempErrorSkus", con)) //Calling the SP
                                    using (var da = new SqlDataAdapter(cmd))
                                    {
                                        //Filling the table with what is found
                                        da.Fill(skusWithErrors);
                                    }
                                    //***************************************************************************************************
                                    //Step 7: If no data is found in the error table
                                    //***************************************************************************************************
                                    //Start inserting into actual tables
                                    con.Open();
                                    cmd.CommandText = "SELECT * FROM tempItemStorage";
                                    System.Data.DataTable temp = new System.Data.DataTable();
                                    using (var dataTable = new SqlDataAdapter(cmd))
                                    {
                                        cmd.CommandType = CommandType.Text;
                                        dataTable.Fill(temp);
                                    }
                                    con.Close();

                                    //***************************************************************************************************
                                    //Step 8: Loop through the temp datatable and insert the rows into the database
                                    //***************************************************************************************************

                                    ImportExport IE = new ImportExport();
                                    foreach (DataRow row in temp.Rows)
                                    {
                                        //loop through just one, and it will know the itemID because we set it ealier in the process                            
                                        IE.ImportNewItem(row, cu, objPageDetails);
                                        //Set club parameters here
                                        //cmd.Parameters.Clear();//Clearing the parameters. It was giving me an error(ID=1500)

                                        //cmd.Parameters.AddWithValue("varSku", row[0]);
                                        //cmd.Parameters.AddWithValue("intBrandID", row[1]);
                                        //cmd.Parameters.AddWithValue("intModelID", row[2]);
                                        //cmd.Parameters.AddWithValue("varTypeOfClub", row[3]);
                                        //cmd.Parameters.AddWithValue("varShaftType", row[4]);
                                        //cmd.Parameters.AddWithValue("varNumberOfClubs", row[5]);
                                        //cmd.Parameters.AddWithValue("fltPremiumCharge", row[6]);
                                        //cmd.Parameters.AddWithValue("fltCost", row[7]);
                                        //cmd.Parameters.AddWithValue("fltPrice", row[8]);
                                        //cmd.Parameters.AddWithValue("intQuantity", row[9]);
                                        //cmd.Parameters.AddWithValue("varClubSpecification", row[10]);
                                        //cmd.Parameters.AddWithValue("varShaftSpecification", row[11]);
                                        //cmd.Parameters.AddWithValue("varShaftFlexability", row[12]);
                                        //cmd.Parameters.AddWithValue("varClubDexterity", row[13]);
                                        //cmd.Parameters.AddWithValue("intItemTypeID", row[14]);
                                        //cmd.Parameters.AddWithValue("intLocationID", row[15]);
                                        //cmd.Parameters.AddWithValue("bitIsUsedProduct", 0);
                                        //cmd.Parameters.AddWithValue("varAdditionalInformation", row[16]);

                                        //conInsert.Open();
                                        //cmd.Connection = conInsert;
                                        ////This query/insert statement will first look at the typeID of the item being sent in. 
                                        ////It then looks to see if the items sku is in the table already. If it is, it updates. 
                                        ////If it is not, it inserts the item into the table
                                        //cmd.CommandText =
                                        //    "IF(@intItemTypeID = 1) " +
                                        //        "BEGIN " +
                                        //            "IF EXISTS(SELECT varSku FROM tbl_clubs WHERE varSku = @varSku) " +
                                        //                "BEGIN " +
                                        //                    "UPDATE tbl_clubs SET intBrandID = @intBrandID, intModelID = @intModelID, varTypeOfClub = @varTypeOfClub, "
                                        //                    + "varShaftType = @varShaftType, varNumberOfClubs = @varNumberOfClubs, fltPremiumCharge = @fltPremiumCharge, "
                                        //                    + "fltCost = @fltCost, fltPrice = @fltPrice, intQuantity = @intQuantity, varClubSpecification = "
                                        //                    + "@varClubSpecification, varShaftSpecification = @varShaftSpecification, varShaftFlexability = "
                                        //                    + "@varShaftFlexability, varClubDexterity = @varClubDexterity, intLocationID = @intLocationID, "
                                        //                    + "bitIsUsedProduct = @bitIsUsedProduct, varAdditionalInformation = @varAdditionalInformation WHERE varSku = "
                                        //                    + "@varSku " +
                                        //                "END " +
                                        //            "ELSE " +
                                        //                "BEGIN " +
                                        //                    "INSERT INTO tbl_clubs VALUES(@varSku, @intBrandID, @intModelID, @varTypeOfClub, @varShaftType, "
                                        //                    + "@varNumberOfClubs, @fltPremiumCharge, @fltCost, @fltPrice, @intQuantity, @varClubSpecification, "
                                        //                    + "@varShaftSpecification, @varShaftFlexability, @varClubDexterity, @intItemTypeID, @intLocationID, "
                                        //                    + "@bitIsUsedProduct, @varAdditionalInformation) " +
                                        //                "END " +
                                        //        "END " +
                                        //    "ELSE IF (@intItemTypeID = 2) " +
                                        //        "BEGIN " +
                                        //            "IF EXISTS(SELECT VarSku FROM tbl_accessories WHERE varSku = @varSku) " +
                                        //                "BEGIN " +
                                        //                    "UPDATE tbl_accessories SET varSize = @varNumberOfClubs, varColour = @varShaftType, fltPrice = @fltPrice, "
                                        //                    + "fltCost = @fltCost, intBrandID = @intBrandID, intModelID = @intModelID, varTypeOfAcessory = "
                                        //                    + "@varTypeOfClub, intQuantity = @intQuantity, intLocationID = @intLocationID, varAdditionalInformation = "
                                        //                    + "@varAdditionalInformation WHERE varSku = @varSku " +
                                        //                "END " +
                                        //            "ELSE " +
                                        //                "BEGIN " +
                                        //                    "INSERT INTO tbl_accessories VALUES(@varSku, @varNumberOfClubs, @varShaftType, @fltPrice, @fltCost, @intBrandID, "
                                        //                    + "@intModelID, @varTypeOfClub, @intQuantity, @intItemTypeID, @intLocationID, @varAdditionalInformation) "
                                        //                + "END " +
                                        //        "END " +
                                        //    "ELSE IF (@intItemTypeID = 3) " +
                                        //        "BEGIN " +
                                        //            "IF EXISTS(SELECT varSku FROM tbl_clothing WHERE varSku = @varSku) " +
                                        //                "BEGIN " +
                                        //                    "UPDATE tbl_clothing SET varSize = @varNumberOfClubs, varColour = @varShaftType, varGender = @varClubSpecification, "
                                        //                    + "varStyle = @varTypeOfClub, fltPrice = @fltPrice, fltCost = @fltCost, intBrandID = @intBrandID, intQuantity = "
                                        //                    + "@intQuantity, intLocationID = @intLocationID, varAdditionalInformation = @varAdditionalInformation WHERE varSku = "
                                        //                    + "@varSku " +
                                        //                "END " +
                                        //            "ELSE " +
                                        //                "BEGIN " +
                                        //                    "INSERT INTO tbl_clothing VALUES(@varSku, @varNumberOfClubs, @varShaftType, @varClubSpecification, @varTypeOfClub, "
                                        //                    + "@fltPrice, @fltCost, @intBrandID, @intQuantity, @intItemTypeID, @intLocationID, @varAdditionalInformation) " +
                                        //                "END " +
                                        //        "END";
                                        //reader = cmd.ExecuteReader();
                                        //conInsert.Close();
                                    }
                                }
                            }
                            //***************************************************************************************************
                            //Step 9: Delete the temp tables that were used for storage
                            //***************************************************************************************************

                            cmd.CommandText = "DROP TABLE tempItemStorage; DROP TABLE tempErrorSkus;";
                            conTempDB.Open();
                            cmd.Connection = conTempDB;
                            reader = cmd.ExecuteReader();
                        }
                    }
                }
                return skusWithErrors;
            }
        }
        public System.Data.DataTable CallUploadItems(FileUpload fup, CurrentUser cu, object[] objPageDetails)
        {
            return UploadItems(fup, cu, objPageDetails);
        }

        //******************COGS and PM REPORTING*******************************************************
        //private System.Data.DataTable ReturnExtensiveInvoices(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        //{
        //    string strQueryName = "returnExtensiveInvoices";
        //    //This method returns a collection of relevant data to be used in the forming of an extensive invoice
        //    //string sqlCmd = "SELECT tbl_invoice.intInvoiceID, CONCAT(tbl_invoice.varInvoiceNumber, '-', tbl_invoice.intInvoiceSubNumber) AS 'varInvoiceNumber', tbl_invoice.fltShippingCharges, "
        //    //    + "(tbl_invoice.fltTotalTradeIn * -1) AS 'fltTotalTradeIn', CASE WHEN EXISTS(SELECT tbl_invoiceItem.intInvoiceID FROM tbl_invoiceItem WHERE tbl_invoiceItem.intInvoiceID = "
        //    //    + "tbl_invoice.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 AND fltItemDiscount <> 0 THEN (fltItemPrice * (fltItemDiscount / 100)) WHEN bitIsDiscountPercent "
        //    //    + "= 0 AND fltItemDiscount <> 0 THEN fltItemDiscount ELSE 0 END) FROM tbl_invoiceItem WHERE tbl_invoiceItem.intInvoiceID = tbl_invoice.intInvoiceID) WHEN EXISTS(SELECT "
        //    //    + "tbl_invoiceItemReturns.intInvoiceID FROM tbl_invoiceItemReturns WHERE tbl_invoiceItemReturns.intInvoiceID = tbl_invoice.intInvoiceID) THEN (SELECT SUM(CASE WHEN "
        //    //    + "bitIsDiscountPercent = 1 AND fltItemDiscount <> 0 THEN (fltItemPrice * (fltItemDiscount / 100)) WHEN bitIsDiscountPercent = 0 AND fltItemDiscount <> 0 THEN fltItemDiscount "
        //    //    + "ELSE 0 END) FROM tbl_invoiceItemReturns WHERE tbl_invoiceItemReturns.intInvoiceID = tbl_invoice.intInvoiceID) ELSE 0 END AS 'fltTotalDiscount', ROUND(fltSubTotal + "
        //    //    + "(fltTotalTradeIn * -1), 2) AS 'fltPreTax', CASE WHEN tbl_invoice.bitChargeGST = 1 THEN tbl_invoice.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE WHEN "
        //    //    + "tbl_invoice.bitChargePST = 1 THEN tbl_invoice.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, CASE WHEN tbl_invoice.bitChargeLCT = 1 THEN "
        //    //    + "tbl_invoice.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, ROUND(tbl_invoice.fltBalanceDue + (CASE WHEN tbl_invoice.bitChargeGST = 1 THEN "
        //    //    + "tbl_invoice.fltGovernmentTaxAmount ELSE 0 END) + (CASE WHEN tbl_invoice.bitChargePST = 1 THEN tbl_invoice.fltProvincialTaxAmount ELSE 0 END) + (CASE WHEN "
        //    //    + "tbl_invoice.bitChargeLCT = 1 THEN tbl_invoice.fltLiquorTaxAmount ELSE 0 END) + (fltTotalTradeIn * -1), 2) AS 'fltTotalSales', CASE WHEN EXISTS(SELECT "
        //    //    + "tbl_invoiceItem.intInvoiceID FROM tbl_invoiceItem WHERE tbl_invoiceItem.intInvoiceID = tbl_invoice.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM "
        //    //    + "tbl_invoiceItem WHERE tbl_invoiceItem.intInvoiceID = tbl_invoice.intInvoiceID) WHEN EXISTS(SELECT tbl_invoiceItemReturns.intInvoiceID FROM tbl_invoiceItemReturns WHERE "
        //    //    + "tbl_invoiceItemReturns.intInvoiceID = tbl_invoice.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns WHERE "
        //    //    + "tbl_invoiceItemReturns.intInvoiceID = tbl_invoice.intInvoiceID) ELSE 0 END AS 'fltCostofGoods', CASE WHEN EXISTS(SELECT tbl_invoiceItem.intInvoiceID FROM tbl_invoiceItem "
        //    //    + "WHERE tbl_invoiceItem.intInvoiceID = tbl_invoice.intInvoiceID) THEN (tbl_invoice.fltSubTotal + (-1 * tbl_invoice.fltTotalTradeIn)) - (SELECT SUM(fltItemCost * "
        //    //    + "intItemQuantity) FROM tbl_invoiceItem WHERE tbl_invoiceItem.intInvoiceID = tbl_invoice.intInvoiceID) WHEN EXISTS(SELECT tbl_invoiceItemReturns.intInvoiceID FROM "
        //    //    + "tbl_invoiceItemReturns WHERE tbl_invoiceItemReturns.intInvoiceID = tbl_invoice.intInvoiceID) THEN (tbl_invoice.fltSubTotal + (-1 * tbl_invoice.fltTotalTradeIn)) - (SELECT "
        //    //    + "SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns WHERE tbl_invoiceItemReturns.intInvoiceID = tbl_invoice.intInvoiceID) ELSE 0 END AS 'fltRevenueEarned', (SELECT "
        //    //    + "CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers WHERE tbl_customers.intCustomerID = tbl_invoice.intCustomerID) AS 'varCustomerName', (SELECT CONCAT(varFirstName, "
        //    //    + "' ', varLastName) FROM tbl_employee WHERE tbl_employee.intEmployeeID = tbl_invoice.intEmployeeID) AS 'varEmployeeName', tbl_invoice.dtmInvoiceDate FROM tbl_invoice WHERE "
        //    //    + "tbl_invoice.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND tbl_invoice.intLocationID = @intLocationID";

        //    //New Trial Query **, CASE WHEN fltPreTax = 0 THEN 0 ELSE ROUND(((fltPreTax - fltCostofGoods) / fltPreTax), 2) END AS fltProfitMargin 
        //    string sqlCmd = "SELECT intInvoiceID, dtmInvoiceDate, varInvoiceNumber, fltShippingCharges, fltTotalTradeIn, fltTotalDiscount, fltPreTax, fltGovernmentTaxAmount, "
        //        + "fltProvincialTaxAmount, fltLiquorTaxAmount, fltCostofGoods, (fltPreTax + fltGovernmentTaxAmount + fltProvincialTaxAmount + fltLiquorTaxAmount) AS fltTotalSales, "
        //        + "(fltPreTax - fltCostofGoods) AS fltRevenueEarned, varCustomerName, varEmployeeName FROM "
        //        + "(SELECT I.intInvoiceID, I.dtmInvoiceDate, CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', I.fltShippingCharges, (I.fltTotalTradeIn * "
        //        + "-1) AS 'fltTotalTradeIn', CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN "
        //        + "bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
        //        + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 "
        //        + "THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS "
        //        + "'fltTotalDiscount', ROUND(fltSubTotal, 2) AS 'fltPreTax', CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE "
        //        + "WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS "
        //        + "fltLiquorTaxAmount, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * "
        //        + "intItemQuantity) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE "
        //        + "IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE "
        //        + "0 END AS 'fltCostofGoods', (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers C WHERE C.intCustomerID = I.intCustomerID) AS 'varCustomerName', ("
        //        + "SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee E WHERE E.intEmployeeID = I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE "
        //        + "I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID) tblTable";

        //    object[][] parms =
        //    {
        //        new object[] { "@dtmStartDate", startDate },
        //        new object[] { "@dtmEndDate", endDate },
        //        new object[] { "@intLocationID", locationID }
        //    };
        //    return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        //    //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        //}
        private System.Data.DataTable ReturnExtensiveInvoices2(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "returnExtensiveInvoices";
            //New Trial Query **, CASE WHEN fltPreTax = 0 THEN 0 ELSE ROUND(((fltPreTax - fltCostofGoods) / fltPreTax), 2) END AS fltProfitMargin 
            string sqlCmd = "SELECT intInvoiceID, dtmInvoiceDate, varInvoiceNumber, fltShippingCharges, fltTotalTradeIn, fltTotalDiscount, fltSubTotal, fltGovernmentTaxAmount, "
                + "fltProvincialTaxAmount, fltLiquorTaxAmount, fltCostofGoods, fltSalesDollars, (fltSubTotal + fltGovernmentTaxAmount + fltProvincialTaxAmount + fltLiquorTaxAmount) AS fltTotalSales, "
                + "(fltSalesDollars - fltCostofGoods) AS fltRevenueEarned, varCustomerName, varEmployeeName FROM (SELECT I.intInvoiceID, I.dtmInvoiceDate, "
                + "CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', "

                + "I.fltShippingCharges, "
                + "(I.fltTotalTradeIn * -1) AS 'fltTotalTradeIn', "

                + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN "
                + "bitIsDiscountPercent = 1 THEN ((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
                + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 "
                + "THEN ((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS "
                + "'fltTotalDiscount', "

                + "ROUND(fltSubTotal, 2) AS 'fltSubTotal', "

                + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN "
                + "bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
                + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 "
                + "THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, "

                + "CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE "
                + "WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS "
                + "fltLiquorTaxAmount, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * "
                + "intItemQuantity) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE "
                + "IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE "
                + "0 END AS 'fltCostofGoods', (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers C WHERE C.intCustomerID = I.intCustomerID) AS 'varCustomerName', ("
                + "SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee E WHERE E.intEmployeeID = I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE "
                + "I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID) tblTable";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnExtensiveInvoices2(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return ReturnExtensiveInvoices2(startDate, endDate, locationID, objPageDetails);
        }
        //*******GST, PST, and LCT totals********
        private List<TaxReport> ReturnTaxReportDetails(DateTime dtmStartDate, DateTime dtmEndDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "returnTaxReportDetails";
            string sqlCmd = "SELECT D.dtmInvoiceDate, ISNULL(C.fltGovernmentTaxAmountCollected, 0) AS fltGovernmentTaxAmountCollected, ISNULL(C.fltProvincialTaxAmountCollected, 0) "
                + "AS fltProvincialTaxAmountCollected, ISNULL(C.fltLiquorTaxAmountCollected, 0) AS fltLiquorTaxAmountCollected, ISNULL(R.fltGovernmentTaxAmountReturned, 0) AS "
                + "fltGovernmentTaxAmountReturned, ISNULL(R.fltProvincialTaxAmountReturned, 0) AS fltProvincialTaxAmountReturned, ISNULL(R.fltLiquorTaxAmountReturned, 0) AS "
                + "fltLiquorTaxAmountReturned FROM (SELECT I.dtmInvoiceDate FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN "
                + "tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN "
                + "@dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber = 1 GROUP BY I.dtmInvoiceDate UNION "
                + "SELECT I.dtmInvoiceDate FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItemReturns II ON "
                + "II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate "
                + "AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber > 1 GROUP BY I.dtmInvoiceDate) D FULL JOIN (SELECT dtmInvoiceDate, "
                + "ROUND(ISNULL([GST], 0) + ISNULL([HST], 0), 2) AS fltGovernmentTaxAmountCollected, ROUND(ISNULL([PST], 0) +ISNULL([RST], 0) +ISNULL([QST], 0), 2) AS "
                + "fltProvincialTaxAmountCollected, ROUND(ISNULL([LCT], 0), 2) AS fltLiquorTaxAmountCollected FROM (SELECT I.dtmInvoiceDate, TT.varTaxName, SUM(IIT.fltTaxAmount) "
                + "AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
                + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = "
                + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber = 1 GROUP BY I.dtmInvoiceDate, TT.varTaxName) PS1 PIVOT(SUM(fltTaxAmount) FOR varTaxName "
                + "IN([GST], [HST], [PST], [RST], [QST], [LCT])) AS PVT1) C ON C.dtmInvoiceDate = D.dtmInvoiceDate FULL JOIN (SELECT dtmInvoiceDate, ROUND(ISNULL([GST], 0) + "
                + "ISNULL([HST], 0), 2) AS fltGovernmentTaxAmountReturned, ROUND(ISNULL([PST], 0) +ISNULL([RST], 0) +ISNULL([QST], 0), 2) AS fltProvincialTaxAmountReturned, "
                + "ROUND(ISNULL([LCT], 0), 2) AS fltLiquorTaxAmountReturned FROM (SELECT I.dtmInvoiceDate, TT.varTaxName, SUM(IIT.fltTaxAmount) AS fltTaxAmount FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItemReturns II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN "
                + "tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID AND "
                + "IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber > 1 GROUP BY I.dtmInvoiceDate, TT.varTaxName) PS2 PIVOT(SUM(fltTaxAmount) FOR varTaxName IN([GST], [HST], "
                + "[PST], [RST], [QST], [LCT])) AS PVT2) R ON R.dtmInvoiceDate = D.dtmInvoiceDate ORDER BY D.dtmInvoiceDate ASC";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate },
                new object[] { "@intLocationID", locationID }
            };
            return ConvertTaxReportFromDataTable(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return convertTaxReportFromDataTable(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public List<TaxReport> CallReturnTaxReportDetails(DateTime dtmStartDate, DateTime dtmEndDate, int locationID, object[] objPageDetails)
        {
            return ReturnTaxReportDetails(dtmStartDate, dtmEndDate, locationID, objPageDetails);
        }
        private List<TaxReport> ConvertTaxReportFromDataTable(System.Data.DataTable dt)
        {
            List<TaxReport> taxReport = dt.AsEnumerable().Select(row =>
            new TaxReport
            {
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                fltGovernmentTaxAmountCollected = row.Field<double>("fltGovernmentTaxAmountCollected"),
                fltProvincialTaxAmountCollected = row.Field<double>("fltProvincialTaxAmountCollected"),
                fltLiquorTaxAmountCollected = row.Field<double>("fltLiquorTaxAmountCollected"),
                fltGovernmentTaxAmountReturned = row.Field<double>("fltGovernmentTaxAmountReturned"),
                fltProvincialTaxAmountReturned = row.Field<double>("fltProvincialTaxAmountReturned"),
                fltLiquorTaxAmountReturned = row.Field<double>("fltLiquorTaxAmountReturned")
            }).ToList();
            return taxReport;
        }
        public int VerifyTaxesCharged(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!TaxesAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool TaxesAvailable(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "taxesAvailable";
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID "
                + "= IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND "
                + "I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0].ToShortDateString() },
                new object[] { "@dtmEndDate", dtm[1].ToShortDateString() },
                new object[] { "@intLocationID", Convert.ToInt32(repInfo[1]) }
            };

            if (DBC.MakeDataBaseCallToReturnDouble(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnDouble(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }

        //******************ITEMS SOLD REPORTING*******************************************************
        private System.Data.DataTable ReturnItemsSold(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "returnItemsSold";
            string sqlCmd = "SELECT CONCAT(I.invoiceNum, '-', I.invoiceSubNum) AS 'invoice', II.sku, II.totalCost AS cost, II.totalPrice AS price, II.totalDiscount AS "
                + "itemDiscount, II.percentage, CASE WHEN II.percentage = 1 THEN II.totalPrice - ((II.totalPrice * (II.totalDiscount / 100)) + II.totalCost) ELSE "
                + "II.totalPrice - (II.totalDiscount + II.totalCost) END AS 'profit' FROM tbl_invoice I JOIN(SELECT invoiceNum, invoiceSubNum, sku, SUM(price * "
                + "quantity) AS totalPrice, SUM(cost * quantity) AS totalCost, CASE WHEN percentage = 1 THEN SUM(itemDiscount) ELSE SUM(itemDiscount * quantity) END "
                + "AS totalDiscount, percentage FROM tbl_invoiceItem GROUP BY invoiceNum, invoiceSubNum, sku, percentage) AS II ON II.invoiceNum = I.invoiceNum AND "
                + "II.invoiceSubNum = I.invoiceSubNum WHERE II.sku NOT IN(SELECT sku FROM tbl_tempTradeInCartSkus) AND II.invoiceNum NOT IN(SELECT invoiceNum FROM "
                + "tbl_invoiceItemReturns) AND I.locationID = @locationID AND I.invoiceDate BETWEEN @startDate AND @endDate ORDER BY I.invoiceNum, I.invoiceSubNum";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnItemsSold(DateTime startDate, DateTime endDate, int locationID, object[] objPageDetails)
        {
            return ReturnItemsSold(startDate, endDate, locationID, objPageDetails);
        }

        //******************DISCOUNT REPORTING*******************************************************
        private System.Data.DataTable ReturnDiscountsBetweenDates(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnDiscountsBetweenDates";
            DateTime[] reportDates = (DateTime[])repInfo[0];
            DateTime startDate = reportDates[0];
            DateTime endDate = reportDates[1];
            int locationID = Convert.ToInt32(repInfo[1]);
            //This method returns all invoices with discounts between two dates
            string sqlCmd = "SELECT TD.intInvoiceID, I2.varInvoiceNumber, I2.intInvoiceSubNumber, I2.dtmInvoiceDate, (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers "
                + "WHERE intCustomerID = I2.intCustomerID) AS customerName, (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee WHERE intEmployeeID = I2.intEmployeeID) AS "
                + "employeeName, I2.fltTotalDiscount, I2.fltBalanceDue, TD.fltGovernmentTaxAmount, TD.fltProvincialTaxAmount, TD.fltLiquorTaxAmount FROM tbl_invoice I2 JOIN(SELECT "
                + "I.intInvoiceID, IIT.fltGovernmentTaxAmount, IIT.fltProvincialTaxAmount, IIT.fltLiquorTaxAmount FROM tbl_invoice I JOIN tbl_invoiceItem II ON II.intInvoiceID = "
                + "I.intInvoiceID JOIN(SELECT intInvoiceID, ROUND(ISNULL([GST], 0) + ISNULL([HST], 0), 2) AS fltGovernmentTaxAmount, ROUND(ISNULL([PST], 0) +ISNULL([RST], 0) +ISNULL("
                + "[QST], 0), 2) AS fltProvincialTaxAmount, ROUND(ISNULL([LCT], 0), 2) AS fltLiquorTaxAmount FROM(SELECT II.intInvoiceID, TT.varTaxName, SUM(IIT.fltTaxAmount) AS "
                + "fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
                + "IIT.intInvoiceItemID WHERE IIT.bitIsTaxCharged = 1 GROUP BY II.intInvoiceID, TT.varTaxName) PS1 PIVOT(SUM(fltTaxAmount) FOR varTaxName IN([GST], [HST], [PST], "
                + "[RST], [QST], [LCT])) AS PVT1) IIT ON IIT.intInvoiceID = II.intInvoiceID WHERE fltTotalDiscount <> 0 AND dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND "
                + "intLocationID = @intLocationID GROUP BY I.intInvoiceID, IIT.fltGovernmentTaxAmount, IIT.fltProvincialTaxAmount, IIT.fltLiquorTaxAmount) TD ON TD.intInvoiceID = "
                + "I2.intInvoiceID";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnDiscountsBetweenDates(object[] repInfo, object[] objPageDetails)
        {
            return ReturnDiscountsBetweenDates(repInfo, objPageDetails);
        }

        //******************CASHOUT REPORTING*******************************************************
        private List<Cashout> ReturnCashoutFromDataTable(System.Data.DataTable dt)
        {
            List<Cashout> cashout = dt.AsEnumerable().Select(row =>
            new Cashout
            {
                dtmCashoutDate = row.Field<DateTime>("dtmCashoutDate"),
                fltSystemCountedBasedOnSystemTradeIn = row.Field<double>("fltSystemCountedBasedOnSystemTradeIn"),
                fltSystemCountedBasedOnSystemGiftCard = row.Field<double>("fltSystemCountedBasedOnSystemGiftCard"),
                fltSystemCountedBasedOnSystemCash = row.Field<double>("fltSystemCountedBasedOnSystemCash"),
                fltSystemCountedBasedOnSystemDebit = row.Field<double>("fltSystemCountedBasedOnSystemDebit"),
                fltSystemCountedBasedOnSystemMastercard = row.Field<double>("fltSystemCountedBasedOnSystemMastercard"),
                fltSystemCountedBasedOnSystemVisa = row.Field<double>("fltSystemCountedBasedOnSystemVisa"),
                fltManuallyCountedBasedOnReceiptsTradeIn = row.Field<double>("fltManuallyCountedBasedOnReceiptsTradeIn"),
                fltManuallyCountedBasedOnReceiptsGiftCard = row.Field<double>("fltManuallyCountedBasedOnReceiptsGiftCard"),
                fltManuallyCountedBasedOnReceiptsCash = row.Field<double>("fltManuallyCountedBasedOnReceiptsCash"),
                fltManuallyCountedBasedOnReceiptsDebit = row.Field<double>("fltManuallyCountedBasedOnReceiptsDebit"),
                fltManuallyCountedBasedOnReceiptsMastercard = row.Field<double>("fltManuallyCountedBasedOnReceiptsMastercard"),
                fltManuallyCountedBasedOnReceiptsVisa = row.Field<double>("fltManuallyCountedBasedOnReceiptsVisa"),
                fltSalesSubTotal = row.Field<double>("fltSalesSubTotal"),
                fltGovernmentTaxAmount = row.Field<double>("fltGovernmentTaxAmount"),
                fltProvincialTaxAmount = row.Field<double>("fltProvincialTaxAmount"),
                fltLiquorTaxAmount = row.Field<double>("fltLiquorTaxAmount"),
                fltCashDrawerOverShort = row.Field<double>("fltCashDrawerOverShort"),
                bitIsCashoutFinalized = row.Field<bool>("bitIsCashoutFinalized"),
                bitIsCashoutProcessed = row.Field<bool>("bitIsCashoutProcessed"),
                intEmployeeID = row.Field<int>("intEmployeeID"),
                intLocationID = row.Field<int>("intLocationID")
            }).ToList();
            return cashout;
        }

        //******************STORE STATS REPORT***********************
        private System.Data.DataTable ReturnStoreStats(DateTime startDate, DateTime endDate, int timeFrame, object[] objPageDetails)
        {
            string strQueryName = "returnStoreStats";
            string sqlCmd = "(SELECT I.intInvoiceID, YEAR(dtmInvoiceDate) AS dtmInvoiceYear, DATENAME(MONTH, dtmInvoiceDate) AS varMonthName, DATEADD(MONTH, DATEDIFF(MONTH, "
                + "0, dtmInvoiceDate), 0) AS dtmMonthDate, DATEADD(DAY, 1 - DATEPART(WEEKDAY, dtmInvoiceDate), CAST(dtmInvoiceDate AS DATE)) dtmWeekStartDate, dtmInvoiceDate "
                + "AS dtmSelectedDate, (SELECT CONCAT(L.varLocationName, '-', L.varCityName) FROM tbl_location L WHERE L.intLocationID = I.intLocationID) AS varLocationName, "
                + "CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', "

                + "I.fltShippingCharges, "
                + "(I.fltTotalTradeIn * -1) AS 'fltTotalTradeIn', "

                + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 "
                + "THEN ((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT "
                + "IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 THEN ("
                + "(fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS "
                + "'fltTotalDiscount', "

                + "ROUND(fltSubTotal, 2) AS 'fltSubTotal', "

                + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN "
                + "bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
                + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 "
                + "THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, "

                + "CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, "
                + "CASE WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, "
                + "CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, "

                + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM("
                + "fltItemCost * intItemQuantity) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM "
                + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE "
                + "IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltCostofGoods', "

                + "(SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers C WHERE "
                + "C.intCustomerID = I.intCustomerID) AS 'varCustomerName', (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee E WHERE E.intEmployeeID = "
                + "I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate) AS ABC ";
            string startQuery;
            string endQuery;

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate }
                //new object[] { "@intLocationID", locationID }
            };
            if (timeFrame == 1)
            {
                startQuery = "SELECT FORMAT(dtmSelectedDate, 'dd/MM/yyyy') AS selection, varLocationName, ROUND(SUM(fltGovernmentTaxAmount), 2) AS fltGovernmentTaxAmount, ROUND("
                    + "SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, ROUND(SUM(fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND(SUM(fltCostofGoods), 2) AS "
                    + "fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, ROUND((SUM(fltSalesDollars) - SUM(fltCostofGoods)) / SUM(fltSalesDollars), 4) AS fltProfitMargin, ROUND("
                    + "ROUND(SUM(fltGovernmentTaxAmount), 2) + ROUND(SUM(fltProvincialTaxAmount), 2) + ROUND(SUM(fltLiquorTaxAmount), 2) + ROUND(SUM(fltSalesDollars), 2), 2) AS "
                    + "fltTotalSales, ROUND(SUM(fltSalesDollars), 2) AS fltSalesDollars FROM ";
                endQuery = "GROUP BY dtmSelectedDate, varLocationName ORDER BY dtmSelectedDate, varLocationName";
            }
            else if (timeFrame == 2)
            {
                startQuery = "SELECT FORMAT(CAST(dtmWeekStartDate AS DATE), 'dd/MM/yyyy') AS selection, varLocationName, ROUND(SUM(fltGovernmentTaxAmount), 2) AS "
                    + "fltGovernmentTaxAmount, ROUND(SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, ROUND(SUM(fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND("
                    + "SUM(fltCostofGoods), 2) AS fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, ROUND((SUM(fltSalesDollars) - SUM(fltCostofGoods)) / SUM(fltSalesDollars), 4) "
                    + "AS fltProfitMargin, ROUND(ROUND(SUM(fltGovernmentTaxAmount), 2) + ROUND(SUM(fltProvincialTaxAmount), 2) + ROUND(SUM(fltLiquorTaxAmount), 2) + ROUND(SUM("
                    + "fltSalesDollars), 2), 2) AS fltTotalSales, ROUND(SUM(fltSalesDollars), 2) AS fltSalesDollars FROM ";
                endQuery = "GROUP BY dtmWeekStartDate, varLocationName ORDER BY dtmWeekStartDate, varLocationName";
            }
            else
            {
                startQuery = "SELECT dtmMonthDate, CONCAT(varMonthName, ' / ', dtmInvoiceYear) AS selection, varLocationName, ROUND(SUM(fltGovernmentTaxAmount), 2) AS "
                    + "fltGovernmentTaxAmount, ROUND(SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, ROUND(SUM(fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND("
                    + "SUM(fltCostofGoods), 2) AS fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, ROUND((SUM(fltSalesDollars) - SUM(fltCostofGoods)) / SUM(fltSalesDollars), 4) "
                    + "AS fltProfitMargin, ROUND(ROUND(SUM(fltGovernmentTaxAmount), 2) + ROUND(SUM(fltProvincialTaxAmount), 2) + ROUND(SUM(fltLiquorTaxAmount), 2) + ROUND(SUM("
                    + "fltSalesDollars), 2), 2) AS fltTotalSales, ROUND(SUM(fltSalesDollars), 2) AS fltSalesDollars FROM ";
                endQuery = "GROUP BY dtmMonthDate, varLocationName, dtmInvoiceYear, varMonthName ORDER BY dtmMonthDate, dtmInvoiceYear, varMonthName, varLocationName";
            }

            sqlCmd = startQuery + sqlCmd + endQuery;
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnStoreStats(DateTime startDate, DateTime endDate, int timeFrame, object[] objPageDetails)
        {
            return ReturnStoreStats(startDate, endDate, timeFrame, objPageDetails);
        }
        public int VerifyStatsAvailable(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!StatsAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool StatsAvailable(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "statsAvailable";
            bool bolTA = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate ";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }

        //*************Specific Apparel Report********* w/CORRECT profit margin calculation
        private System.Data.DataTable ReturnSpecificApparelDataTableForReport(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            string strQueryName = "returnSpecificApparelDataTableForReport";
            string sqlCmd = "SELECT L.varLocationName, FQ.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE "
                + "A.intInventoryID = FQ.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = FQ.intInventoryID) "
                + "WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = FQ.intInventoryID) THEN (SELECT CL.varSku "
                + "FROM tbl_clothing CL WHERE CL.intInventoryID = FQ.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = FQ.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = FQ.intInventoryID) END "
                + "AS varSku, FQ.varItemDescription, SUM(FQ.intItemQuantity) AS intOverallQuantity, SUM(FQ.fltTotalCost) AS fltOverallCost, "
                + "SUM(FQ.fltTotalPrice) AS fltOverallPrice FROM(SELECT I.intLocationID, SA.intInventoryID, II.varItemDescription, "
                + "(SUM(II.intItemQuantity) - ISNULL(SUM(IIR.intItemQuantity), 0)) * II.fltItemCost AS fltTotalCost, (SUM(II.intItemQuantity) - "
                + "ISNULL(SUM(IIR.intItemQuantity), 0)) * ROUND(CASE WHEN II.bitIsDiscountPercent = 1 THEN II.fltItemPrice - ((II.fltItemDiscount "
                + "/ 100) * II.fltItemPrice) ELSE II.fltItemPrice - II.fltItemDiscount END, 2) AS fltTotalPrice, SUM(II.intItemQuantity) - "
                + "ISNULL(SUM(IIR.intItemQuantity), 0) AS intItemQuantity FROM tbl_invoiceItem II LEFT JOIN(SELECT R.intInvoiceID, "
                + "O.intLocationID, S.intInventoryID, R.fltItemCost, R.fltItemPrice, SUM(R.intItemQuantity) AS intItemQuantity FROM "
                + "tbl_invoiceItemReturns R JOIN tbl_invoice O ON O.intInvoiceID = R.intInvoiceID JOIN tbl_specificApparel S ON S.intInventoryID "
                + "= R.intInventoryID WHERE O.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY R.intInvoiceID, O.intLocationID, "
                + "S.intInventoryID, R.fltItemCost, R.fltItemPrice) IIR ON IIR.intInvoiceID = II.intInvoiceID JOIN tbl_invoice I ON "
                + "I.intInvoiceID = II.intInvoiceID JOIN tbl_specificApparel SA ON SA.intInventoryID = II.intInventoryID WHERE I.dtmInvoiceDate "
                + "BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY SA.intInventoryID, I.intLocationID, II.varItemDescription, II.fltItemCost, "
                + "II.fltItemPrice, II.bitIsDiscountPercent, II.fltItemDiscount) FQ JOIN tbl_location L ON L.intLocationID = FQ.intLocationID "
                + "GROUP BY FQ.intInventoryID, L.varLocationName, FQ.varItemDescription";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnSpecificApparelDataTableForReport(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            return ReturnSpecificApparelDataTableForReport(dtmStartDate, dtmEndDate, objPageDetails);
        }
        public int VerifySpecificApparel(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!SpecificApparelAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool SpecificApparelAvailable(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "specificApparelAvailable";
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT SUM(II.intItemQuantity) overallQuantity FROM tbl_invoiceItem II JOIN tbl_invoice I ON "
                + "I.intInvoiceID = II.intInvoiceID JOIN tbl_specificApparel SA ON SA.intInventoryID = II.intInventoryID "
                + "WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }

        //*************Specific Grip Report********* w/CORRECT profit margin calculation
        private System.Data.DataTable ReturnSpecificGripDataTableForReport(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            string strQueryName = "returnSpecificGripDataTableForReport";
            string sqlCmd = "SELECT L.varLocationName, FQ.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE "
                + "A.intInventoryID = FQ.intInventoryID) THEN (SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = FQ.intInventoryID) "
                + "WHEN EXISTS(SELECT CL.intInventoryID FROM tbl_clothing CL WHERE CL.intInventoryID = FQ.intInventoryID) THEN (SELECT CL.varSku "
                + "FROM tbl_clothing CL WHERE CL.intInventoryID = FQ.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE "
                + "C.intInventoryID = FQ.intInventoryID) THEN (SELECT C.varSku FROM tbl_clubs C WHERE C.intInventoryID = FQ.intInventoryID) END "
                + "AS varSku, FQ.varItemDescription, SUM(FQ.intItemQuantity) AS intOverallQuantity, SUM(FQ.fltTotalCost) AS fltOverallCost, "
                + "SUM(FQ.fltTotalPrice) AS fltOverallPrice FROM(SELECT I.intLocationID, SG.intInventoryID, II.varItemDescription, "
                + "(SUM(II.intItemQuantity) - ISNULL(SUM(IIR.intItemQuantity), 0)) * II.fltItemCost AS fltTotalCost, (SUM(II.intItemQuantity) - "
                + "ISNULL(SUM(IIR.intItemQuantity), 0)) * ROUND(CASE WHEN II.bitIsDiscountPercent = 1 THEN II.fltItemPrice - ((II.fltItemDiscount "
                + "/ 100) * II.fltItemPrice) ELSE II.fltItemPrice - II.fltItemDiscount END, 2) AS fltTotalPrice, SUM(II.intItemQuantity) - "
                + "ISNULL(SUM(IIR.intItemQuantity), 0) AS intItemQuantity FROM tbl_invoiceItem II LEFT JOIN(SELECT R.intInvoiceID, "
                + "O.intLocationID, S.intInventoryID, R.fltItemCost, R.fltItemPrice, SUM(R.intItemQuantity) AS intItemQuantity FROM "
                + "tbl_invoiceItemReturns R JOIN tbl_invoice O ON O.intInvoiceID = R.intInvoiceID JOIN tbl_specificGrip S ON S.intInventoryID = "
                + "R.intInventoryID WHERE O.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY R.intInvoiceID, O.intLocationID, "
                + "S.intInventoryID, R.fltItemCost, R.fltItemPrice) IIR ON IIR.intInvoiceID = II.intInvoiceID JOIN tbl_invoice I ON "
                + "I.intInvoiceID = II.intInvoiceID JOIN tbl_specificGrip SG ON SG.intInventoryID = II.intInventoryID WHERE I.dtmInvoiceDate "
                + "BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY SG.intInventoryID, I.intLocationID, II.varItemDescription, II.fltItemCost, "
                + "II.fltItemPrice, II.bitIsDiscountPercent, II.fltItemDiscount) FQ JOIN tbl_location L ON L.intLocationID = FQ.intLocationID "
                + "GROUP BY FQ.intInventoryID, L.varLocationName, FQ.varItemDescription";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnSpecificGripDataTableForReport(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            return ReturnSpecificGripDataTableForReport(dtmStartDate, dtmEndDate, objPageDetails);
        }
        public int VerifySpecificGrip(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!SpecificGripAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool SpecificGripAvailable(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "specificGripAvailable";
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT SUM(II.intItemQuantity) overallQuantity FROM tbl_invoiceItem II JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID JOIN "
                + "tbl_specificGrip SG ON SG.intInventoryID = II.intInventoryID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }

        private System.Data.DataTable ReturnCashoutsForSelectedDates(object[] passing, object[] objPageDetails)
        {
            string strQueryName = "ReturnCashoutsForSelectedDates";

            string sqlCmd = "SELECT dtmCashoutDate, intLocationID, fltSystemCountedBasedOnSystemTradeIn, fltManuallyCountedBasedOnReceiptsTradeIn, "
                + "fltSystemCountedBasedOnSystemGiftCard, fltManuallyCountedBasedOnReceiptsGiftCard, fltSystemCountedBasedOnSystemCash, "
                + "fltManuallyCountedBasedOnReceiptsCash, fltSystemCountedBasedOnSystemDebit, fltManuallyCountedBasedOnReceiptsDebit, "
                + "fltSystemCountedBasedOnSystemMastercard, fltManuallyCountedBasedOnReceiptsMastercard, fltSystemCountedBasedOnSystemVisa, "
                + "fltManuallyCountedBasedOnReceiptsVisa, fltCashDrawerOverShort, bitIsCashoutProcessed, bitIsCashoutFinalized FROM "
                + "tbl_cashout WHERE dtmCashoutDate BETWEEN @dtmStartDate AND @dtmEndDate AND intLocationID = @intLocationID";

            DateTime[] dtm = (DateTime[])passing[0];
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0].ToShortDateString() },
                new object[] { "@dtmEndDate", dtm[1].ToShortDateString() },
                new object[] { "@intLocationID", passing[1] }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnCashoutsForSelectedDates(object[] passing, object[] objPageDetails)
        {
            return ReturnCashoutsForSelectedDates(passing, objPageDetails);
        }
        public int CashoutsProcessed(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!CashoutInDateRange(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool CashoutInDateRange(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "CashoutInDateRange";
            bool bolCIDR = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(dtmCashoutDate) FROM tbl_cashout WHERE dtmCashoutDate BETWEEN @dtmStartDate AND @dtmEndDate AND intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0].ToShortDateString() },
                new object[] { "@dtmEndDate", dtm[1].ToShortDateString() },
                new object[] { "@intLocationID", Convert.ToInt32(repInfo[1]) }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolCIDR = true;
            }
            return bolCIDR;
        }
        private void FinalizeCashout(string args, object[] objPageDetails)
        {
            string strQueryName = "FinalizeCashout";
            DateTime selectedDate = DateTime.Parse(args.Split(' ')[0]);
            int location = Convert.ToInt32(args.Split(' ')[1]);
            string sqlCmd = "UPDATE tbl_cashout SET bitIsCashoutFinalized = 1 WHERE dtmCashoutDate = @dtmCashoutDate AND intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", selectedDate.ToShortDateString() },
                new object[] { "@intLocationID", location }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CallFinalizeCashout(string args, object[] objPageDetails)
        {
            FinalizeCashout(args, objPageDetails);
        }

        public Cashout CallSelectedCashoutToReturn(object[] args, object[] objPageDetails)
        {
            Cashout cOut;
            //if (CheckForNewSales(args, objPageDetails))
            //{
            cOut = RecalculateCashoutTotals(args, objPageDetails);
            //}
            //else
            //{
            //    cOut = ReturnSelectedCashout(args, objPageDetails)[0];
            //}
            return cOut;
        }
        //private bool CheckForNewSales(object[] args, object[] objPageDetails)
        //{
        //    string strQueryName = "CheckForNewSales";
        //    bool newSales = false;

        //    string sqlCmd = "SELECT (SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate = @dtmSelectedDate AND intLocationID = @intLocationID) "
        //        + "- C.intSalesCount FROM tbl_cashout C WHERE C.dtmCashoutDate = @dtmSelectedDate AND C.intLocationID = @intLocationID";
        //    object[][] parms =
        //    {
        //        new object[] { "@dtmSelectedDate", DateTime.Parse(args[0].ToString()).ToShortDateString() },
        //        new object[] { "@intLocationID", Convert.ToInt32(args[1]) }
        //    };

        //    if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
        //    {
        //        newSales = true;
        //    }
        //    return newSales;
        //}
        private Cashout RecalculateCashoutTotals(object[] args, object[] objPageDetails)
        {
            Cashout cOut = CreateNewCashout(DateTime.Parse(args[0].ToString()), Convert.ToInt32(args[1]), objPageDetails);
            UpdateCalculatedTotalsCashout(cOut, objPageDetails);
            return ReturnSelectedCashout(args, objPageDetails)[0];
        }

        private void UpdateCalculatedTotalsCashout(Cashout cashout, object[] objPageDetails)
        {
            string strQueryName = "UpdateCashout";
            string sqlCmd = "UPDATE tbl_cashout SET fltSystemCountedBasedOnSystemTradeIn = @fltSystemCountedBasedOnSystemTradeIn, fltSystemCountedBasedOnSystemGiftCard "
                + "= @fltSystemCountedBasedOnSystemGiftCard, fltSystemCountedBasedOnSystemCash = @fltSystemCountedBasedOnSystemCash, fltSystemCountedBasedOnSystemDebit "
                + "= @fltSystemCountedBasedOnSystemDebit, fltSystemCountedBasedOnSystemMastercard = @fltSystemCountedBasedOnSystemMastercard, "
                + "fltSystemCountedBasedOnSystemVisa = @fltSystemCountedBasedOnSystemVisa, fltSalesSubTotal = @fltSalesSubTotal, fltGovernmentTaxAmount = "
                + "@fltGovernmentTaxAmount, fltProvincialTaxAmount = @fltProvincialTaxAmount, fltLiquorTaxAmount = @fltLiquorTaxAmount " //, fltHarmonizedTaxAmount = "
                                                                                                                                         //+ "@fltHarmonizedTaxAmount, fltQuebecTaxAmount = @fltQuebecTaxAmount, fltRetailTaxAmount = @fltRetailTaxAmount, intSalesCount = @intSalesCount "
                + "WHERE dtmCashoutDate = @dtmCashoutDate AND intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", cashout.dtmCashoutDate.ToShortDateString() },
                new object[] { "@fltSystemCountedBasedOnSystemTradeIn", cashout.fltSystemCountedBasedOnSystemTradeIn },
                new object[] { "@fltSystemCountedBasedOnSystemGiftCard", cashout.fltSystemCountedBasedOnSystemGiftCard },
                new object[] { "@fltSystemCountedBasedOnSystemCash", cashout.fltSystemCountedBasedOnSystemCash },
                new object[] { "@fltSystemCountedBasedOnSystemDebit", cashout.fltSystemCountedBasedOnSystemDebit },
                new object[] { "@fltSystemCountedBasedOnSystemMastercard", cashout.fltSystemCountedBasedOnSystemMastercard },
                new object[] { "@fltSystemCountedBasedOnSystemVisa", cashout.fltSystemCountedBasedOnSystemVisa },
                new object[] { "@fltSalesSubTotal", cashout.fltSalesSubTotal },
                new object[] { "@fltGovernmentTaxAmount", cashout.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", cashout.fltProvincialTaxAmount },
                new object[] { "@fltLiquorTaxAmount", cashout.fltLiquorTaxAmount },
                //new object[] { "@fltHarmonizedTaxAmount", cashout.fltHarmonizedTaxAmount },
                //new object[] { "@fltQuebecTaxAmount", cashout.fltQuebecTaxAmount },
                //new object[] { "@fltRetailTaxAmount", cashout.fltRetailTaxAmount },
                //new object[] { "@intSalesCount", cashout.intSalesCount },
                new object[] { "@intLocationID", cashout.intLocationID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<Cashout> ReturnSelectedCashout(object[] args, object[] objPageDetails)
        {
            string strQueryName = "ReturnSelectedCashout";
            string sqlCmd = "SELECT dtmCashoutDate, intLocationID, intEmployeeID, fltSystemCountedBasedOnSystemTradeIn, fltSystemCountedBasedOnSystemGiftCard, "
                + "fltSystemCountedBasedOnSystemCash, fltSystemCountedBasedOnSystemDebit, fltSystemCountedBasedOnSystemMastercard, fltSystemCountedBasedOnSystemVisa, "
                + "fltManuallyCountedBasedOnReceiptsTradeIn, fltManuallyCountedBasedOnReceiptsGiftCard, fltManuallyCountedBasedOnReceiptsCash, "
                + "fltManuallyCountedBasedOnReceiptsDebit, fltManuallyCountedBasedOnReceiptsMastercard, fltManuallyCountedBasedOnReceiptsVisa, fltSalesSubTotal, "
                + "fltGovernmentTaxAmount, " //fltHarmonizedTaxAmount, 
                + "fltLiquorTaxAmount, fltProvincialTaxAmount, " //fltQuebecTaxAmount, fltRetailTaxAmount, "
                + "fltCashDrawerOverShort, bitIsCashoutFinalized, bitIsCashoutProcessed " //, intSalesCount 
                + "FROM tbl_cashout WHERE dtmCashoutDate = @dtmCashoutDate AND "
                + "intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", DateTime.Parse(args[0].ToString()).ToShortDateString() },
                new object[] { "@intLocationID", Convert.ToInt32(args[1]) }
            };

            return ReturnCashoutFromDataTable(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ReturnCashoutFromDataTable(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private bool CashoutExists(object[] args, object[] objPageDetails)
        {
            string strQueryName = "CashoutExists";
            bool exists = false;
            string sqlCmd = "SELECT COUNT(dtmCashoutDate) FROM tbl_cashout WHERE dtmCashoutDate = @dtmCashoutDate AND intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", DateTime.Parse(args[0].ToString()).ToShortDateString() },
                new object[] { "@intLocationID", Convert.ToInt32(args[1]) }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                exists = true;
            }
            return exists;
        }
        public bool CallCashoutExists(object[] args, object[] objPageDetails)
        {
            return CashoutExists(args, objPageDetails);
        }
        private void UpdateCashout(Cashout cashout, object[] objPageDetails)
        {
            string strQueryName = "UpdateCashout";
            string sqlCmd = "UPDATE tbl_cashout SET dtmCashoutTime = @dtmCashoutTime, fltSystemCountedBasedOnSystemTradeIn = @fltSystemCountedBasedOnSystemTradeIn, "
                + "fltSystemCountedBasedOnSystemGiftCard = @fltSystemCountedBasedOnSystemGiftCard, fltSystemCountedBasedOnSystemCash = @fltSystemCountedBasedOnSystemCash, "
                + "fltSystemCountedBasedOnSystemDebit = @fltSystemCountedBasedOnSystemDebit, fltSystemCountedBasedOnSystemMastercard = "
                + "@fltSystemCountedBasedOnSystemMastercard, fltSystemCountedBasedOnSystemVisa = @fltSystemCountedBasedOnSystemVisa, "
                + "fltManuallyCountedBasedOnReceiptsTradeIn = @fltManuallyCountedBasedOnReceiptsTradeIn, fltManuallyCountedBasedOnReceiptsGiftCard = "
                + "@fltManuallyCountedBasedOnReceiptsGiftCard, fltManuallyCountedBasedOnReceiptsCash = @fltManuallyCountedBasedOnReceiptsCash, "
                + "fltManuallyCountedBasedOnReceiptsDebit = @fltManuallyCountedBasedOnReceiptsDebit, fltManuallyCountedBasedOnReceiptsMastercard = "
                + "@fltManuallyCountedBasedOnReceiptsMastercard, fltManuallyCountedBasedOnReceiptsVisa = @fltManuallyCountedBasedOnReceiptsVisa, fltSalesSubTotal = "
                + "@fltSalesSubTotal, fltGovernmentTaxAmount = @fltGovernmentTaxAmount, fltProvincialTaxAmount = @fltProvincialTaxAmount, fltLiquorTaxAmount = @fltLiquorTaxAmount, "
                + "fltCashDrawerOverShort = @fltCashDrawerOverShort, bitIsCashoutFinalized = @bitIsCashoutFinalized, bitIsCashoutProcessed = @bitIsCashoutProcessed, intEmployeeID "
                + "= @intEmployeeID WHERE dtmCashoutDate = @dtmCashoutDate AND intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", cashout.dtmCashoutDate.ToShortDateString() },
                new object[] { "@dtmCashoutTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@fltSystemCountedBasedOnSystemTradeIn", cashout.fltSystemCountedBasedOnSystemTradeIn },
                new object[] { "@fltSystemCountedBasedOnSystemGiftCard", cashout.fltSystemCountedBasedOnSystemGiftCard },
                new object[] { "@fltSystemCountedBasedOnSystemCash", cashout.fltSystemCountedBasedOnSystemCash },
                new object[] { "@fltSystemCountedBasedOnSystemDebit", cashout.fltSystemCountedBasedOnSystemDebit },
                new object[] { "@fltSystemCountedBasedOnSystemMastercard", cashout.fltSystemCountedBasedOnSystemMastercard },
                new object[] { "@fltSystemCountedBasedOnSystemVisa", cashout.fltSystemCountedBasedOnSystemVisa },
                new object[] { "@fltManuallyCountedBasedOnReceiptsTradeIn", cashout.fltManuallyCountedBasedOnReceiptsTradeIn },
                new object[] { "@fltManuallyCountedBasedOnReceiptsGiftCard", cashout.fltManuallyCountedBasedOnReceiptsGiftCard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsCash", cashout.fltManuallyCountedBasedOnReceiptsCash },
                new object[] { "@fltManuallyCountedBasedOnReceiptsDebit", cashout.fltManuallyCountedBasedOnReceiptsDebit },
                new object[] { "@fltManuallyCountedBasedOnReceiptsMastercard", cashout.fltManuallyCountedBasedOnReceiptsMastercard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsVisa", cashout.fltManuallyCountedBasedOnReceiptsVisa },
                new object[] { "@fltSalesSubTotal", cashout.fltSalesSubTotal },
                new object[] { "@fltGovernmentTaxAmount", cashout.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", cashout.fltProvincialTaxAmount },
                new object[] { "@fltLiquorTaxAmount", cashout.fltLiquorTaxAmount },
                new object[] { "@fltCashDrawerOverShort", cashout.fltCashDrawerOverShort },
                new object[] { "@bitIsCashoutFinalized", cashout.bitIsCashoutFinalized },
                new object[] { "@bitIsCashoutProcessed", cashout.bitIsCashoutProcessed },
                new object[] { "@intLocationID", cashout.intLocationID },
                new object[] { "@intEmployeeID", cashout.intEmployeeID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CallUpdateCashout(Cashout cashout, object[] objPageDetails)
        {
            UpdateCashout(cashout, objPageDetails);
        }


        //************Change Inventory Reort***********************
        private System.Data.DataTable ReturnChangedInventoryForDateRange(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            string strQueryName = "returnChangedInventoryForDateRange";
            string sqlCmd = "SELECT ICT.dtmChangeDate, ICT.dtmChangeTime, CONCAT(E.varFirstName, ', ', E.varLastName) AS employeeName, L.varCityName, "
                + "ICT.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = ICT.intInventoryID) THEN "
                + "(SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = ICT.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = ICT.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = "
                + "ICT.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = ICT.intInventoryID) THEN (SELECT "
                + "C.varSku FROM tbl_clubs C WHERE C.intInventoryID = ICT.intInventoryID) END AS varSku, ICT.fltOriginalCost, ICT.fltNewCost, "
                + "ICT.fltOriginalPrice, ICT.fltNewPrice, ICT.intOriginalQuantity, ICT.intNewQuantity, ICT.varOriginalDescription, ICT.varNewDescription "
                + "FROM tbl_itemChangeTracking ICT JOIN tbl_employee E ON E.intEmployeeID = ICT.intEmployeeID JOIN tbl_location L ON L.intLocationID = "
                + "ICT.intLocationID WHERE ICT.dtmChangeDate BETWEEN @dtmStartDate AND @dtmEndDate";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public System.Data.DataTable CallReturnChangedInventoryForDateRange(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            return ReturnChangedInventoryForDateRange(dtmStartDate, dtmEndDate, objPageDetails);
        }
        public int VerifyInventoryChange(object[] repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!InventoryChangeAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool InventoryChangeAvailable(object[] repInfo, object[] objPageDetails)
        {
            string strQueryName = "inventoryChangeAvailable";
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT COUNT(dtmChangeDate) changeCount FROM tbl_itemChangeTracking WHERE dtmChangeDate BETWEEN "
                + "@dtmStartDate AND @dtmEndDate";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }

        //********************EXPORTING***************************************************************
        //Export ALL sales/invoices to excel
        //public void exportAllSalesToExcel()
        //{
        //    //Gets the invoice data and puts it in a table
        //    SqlConnection sqlCon = new SqlConnection(connectionString);
        //    sqlCon.Open();
        //    SqlDataAdapter im = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
        //    System.Data.DataTable dtim = new System.Data.DataTable();
        //    im.Fill(dtim);
        //    DataColumnCollection dcimHeaders = dtim.Columns;
        //    sqlCon.Close();
        //    //Gets the invoice item data and puts it in a table
        //    sqlCon.Open();
        //    SqlDataAdapter ii = new SqlDataAdapter("SELECT * FROM tbl_invoiceItem", sqlCon);
        //    System.Data.DataTable dtii = new System.Data.DataTable();
        //    ii.Fill(dtii);
        //    DataColumnCollection dciiHeaders = dtii.Columns;
        //    sqlCon.Close();
        //    //Gets the invoice mop data and puts it in a table
        //    sqlCon.Open();
        //    SqlDataAdapter imo = new SqlDataAdapter("SELECT * FROM tbl_invoiceMOP", sqlCon);
        //    System.Data.DataTable dtimo = new System.Data.DataTable();
        //    imo.Fill(dtimo);
        //    DataColumnCollection dcimoHeaders = dtimo.Columns;
        //    sqlCon.Close();

        //    //Initiating Everything
        //    initiateInvoiceTable();
        //    exportSales_Invoice();
        //    initiateInvoiceItemTable();
        //    exportSales_Items();
        //    initiateInvoiceMOPTable();
        //    exportSales_MOP();
        //}
        ////Initiates the invoice table
        //public System.Data.DataTable initiateInvoiceTable()
        //{
        //    System.Data.DataTable exportInvoiceTable = new System.Data.DataTable();
        //    exportInvoiceTable.Columns.Add("invoiceNum", typeof(string));
        //    exportInvoiceTable.Columns.Add("invoiceSubNum", typeof(string));
        //    exportInvoiceTable.Columns.Add("invoiceDate", typeof(string));
        //    exportInvoiceTable.Columns.Add("invoiceTime", typeof(string));
        //    exportInvoiceTable.Columns.Add("custID", typeof(string));
        //    exportInvoiceTable.Columns.Add("empID", typeof(string));
        //    exportInvoiceTable.Columns.Add("locationID", typeof(string));
        //    exportInvoiceTable.Columns.Add("subTotal", typeof(string));
        //    exportInvoiceTable.Columns.Add("discountAmount", typeof(string));
        //    exportInvoiceTable.Columns.Add("tradeinAmount", typeof(string));
        //    exportInvoiceTable.Columns.Add("governmentTax", typeof(string));
        //    exportInvoiceTable.Columns.Add("provincialTax", typeof(string));
        //    exportInvoiceTable.Columns.Add("chargeGST", typeof(string));
        //    exportInvoiceTable.Columns.Add("chargePST", typeof(string));
        //    exportInvoiceTable.Columns.Add("balanceDue", typeof(string));
        //    exportInvoiceTable.Columns.Add("transactionType", typeof(string));
        //    exportInvoiceTable.Columns.Add("comments", typeof(string));
        //    exportSales_Invoice();

        //    return exportInvoiceTable;
        //}
        ////Initiates the invoice item table
        //public System.Data.DataTable initiateInvoiceItemTable()
        //{
        //    System.Data.DataTable exportInvoiceItemTable = new System.Data.DataTable();
        //    exportInvoiceItemTable.Columns.Add("invoiceNum", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("invoiceSubNum", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("sku", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("quantity", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("cost", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("price", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("itemDiscount", typeof(string));
        //    exportInvoiceItemTable.Columns.Add("percentage", typeof(string));
        //    exportSales_Items();

        //    return exportInvoiceItemTable;
        //}
        ////Initiates the invoice mop table
        //public System.Data.DataTable initiateInvoiceMOPTable()
        //{
        //    //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
        //    System.Data.DataTable exportInvoiceMOPTable = new System.Data.DataTable();
        //    exportInvoiceMOPTable.Columns.Add("ID", typeof(string));
        //    exportInvoiceMOPTable.Columns.Add("invoiceNum", typeof(string));
        //    exportInvoiceMOPTable.Columns.Add("invoiceSubNum", typeof(string));
        //    exportInvoiceMOPTable.Columns.Add("mopType", typeof(string));
        //    exportInvoiceMOPTable.Columns.Add("amountPaid", typeof(string));
        //    exportSales_MOP();

        //    return exportInvoiceMOPTable;
        //}
        ////Gets the invoice data and puts it in a table
        //public System.Data.DataTable exportSales_Invoice(System.Data.DataTable dt)
        //{
        //    //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
        //    //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
        //    //provincialTax, balanceDue, transactionType, comments
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select * from tbl_invoice";
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        string invoiceNum = reader["invoiceNum"].ToString();
        //        string invoiceSubNum = reader["invoiceSubNum"].ToString();
        //        string invoiceDate = reader["invoiceDate"].ToString();
        //        string invioceTime = reader["invoiceTime"].ToString();
        //        string custID = reader["custID"].ToString();
        //        string empID = reader["empID"].ToString();
        //        string locationID = reader["locationID"].ToString();
        //        string subTotal = reader["subTotal"].ToString();
        //        string discountAmount = reader["discountAmount"].ToString();
        //        string tradeinAmount = reader["tradeinAmount"].ToString();
        //        string governmentTax = reader["governmentTax"].ToString();
        //        string provincialTax = reader["provincialTax"].ToString();
        //        string chargeGST = reader["chargeGST"].ToString();
        //        string chargePST = reader["chargePST"].ToString();
        //        string balanceDue = reader["balanceDue"].ToString();
        //        string transactionType = reader["transactionType"].ToString();
        //        string comments = reader["comments"].ToString();
        //        exportInvoiceTable.Rows.Add(invoiceNum, invoiceSubNum, invoiceDate, invioceTime,
        //            custID, empID, locationID, subTotal, discountAmount,
        //            tradeinAmount, governmentTax, provincialTax, chargeGST, chargePST, balanceDue, transactionType, comments);
        //    }
        //    conn.Close();
        //}
        ////Gets the invoice item data and puts it in a table
        //public void exportSales_Items()
        //{
        //    //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
        //    //itemPrice, itemDiscount, percentage
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select * from tbl_invoiceItem";
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        string invoiceNum = reader["invoiceNum"].ToString();
        //        string invoiceSubNum = reader["invoiceSubNum"].ToString();
        //        string sku = reader["sku"].ToString();
        //        string quantity = reader["quantity"].ToString();
        //        string cost = reader["cost"].ToString();
        //        string price = reader["price"].ToString();
        //        string itemDisocunt = reader["itemDiscount"].ToString();
        //        string percentage = reader["percentage"].ToString();
        //        exportInvoiceItemTable.Rows.Add(invoiceNum, invoiceSubNum, sku, quantity, cost,
        //            price, itemDisocunt, percentage);
        //    }
        //    conn.Close();
        //}
        ////Gets the invoice mop data and puts it in a table
        //public void exportSales_MOP()
        //{
        //    //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "Select * from tbl_invoiceMOP";
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        string ID = reader["ID"].ToString();
        //        string invoiceNum = reader["invoiceNum"].ToString();
        //        string invoiceSubNum = reader["invoiceSubNum"].ToString();
        //        string mopType = reader["mopType"].ToString();
        //        string amountPaid = reader["amountPaid"].ToString();
        //        exportInvoiceMOPTable.Rows.Add(ID, invoiceNum, invoiceSubNum, mopType, amountPaid);
        //    }
        //    conn.Close();
        //}
        private void ItemExports(string selection, FileInfo newFile, string fileName, object[] objPageDetails)
        {
            //This is the table that has all of the information lined up the way Caspio needs it to be
            System.Data.DataTable exportTable = new System.Data.DataTable();
            if (selection.Equals("all"))
            {
                exportTable = ExportAllInventory(objPageDetails);
            }
            else if (selection.Equals("clubs"))
            {
                exportTable = ExportAllAdd_Clubs(objPageDetails);
            }
            else if (selection.Equals("accessories"))
            {
                exportTable = ExportAllAdd_Accessories(objPageDetails);
            }
            else if (selection.Equals("clothing"))
            {
                exportTable = ExportAllAdd_Clothing(objPageDetails);
            }
            string[] itemExportColumns = { "Vendor", "Store_ID", "ItemNumber", "Shipment_Date", "Brand", "Model", "Club_Type", "Shaft",
                    "Number_of_Clubs", "Tradein_Price", "Premium", "WE PAY", "QUANTITY", "Ext'd Price", "RetailPrice", "Comments",
                    "Image", "Club_Spec", "Shaft_Spec", "Shaft_Flex", "Dexterity", "Destination", "Received", "Paid", "ItemType", "isTradeIn"};
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Add page to the work book called inventory
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Inventory");
                worksheet.Cells[1, 1].Value = "Date Created: " + DateTime.Now.ToString("dd.M.yyyy");
                //Sets the column headers
                for (int i = 0; i < itemExportColumns.Count(); i++)
                {
                    worksheet.Cells[2, i + 1].Value = itemExportColumns[i].ToString();
                }
                DataColumnCollection dcCollection = exportTable.Columns;
                int recordIndex = 3;
                foreach (DataRow row in exportTable.Rows)
                {
                    worksheet.Cells[recordIndex, 1].Value = row[0].ToString();
                    worksheet.Cells[recordIndex, 2].Value = row[1].ToString();
                    worksheet.Cells[recordIndex, 3].Value = row[2].ToString();
                    worksheet.Cells[recordIndex, 4].Value = row[3].ToString();
                    worksheet.Cells[recordIndex, 5].Value = row[4].ToString();
                    worksheet.Cells[recordIndex, 6].Value = row[5].ToString();
                    worksheet.Cells[recordIndex, 7].Value = row[6].ToString();
                    worksheet.Cells[recordIndex, 8].Value = row[7].ToString();
                    worksheet.Cells[recordIndex, 9].Value = row[8].ToString();
                    worksheet.Cells[recordIndex, 10].Value = Convert.ToDouble(row[9].ToString());
                    worksheet.Cells[recordIndex, 11].Value = Convert.ToDouble(row[10].ToString());
                    worksheet.Cells[recordIndex, 12].Value = Convert.ToDouble(row[11].ToString());
                    worksheet.Cells[recordIndex, 13].Value = Convert.ToDouble(row[12].ToString());
                    worksheet.Cells[recordIndex, 14].Value = Convert.ToDouble(row[13].ToString());
                    worksheet.Cells[recordIndex, 15].Value = Convert.ToDouble(row[14].ToString());
                    worksheet.Cells[recordIndex, 16].Value = row[15].ToString();
                    worksheet.Cells[recordIndex, 17].Value = row[16].ToString();
                    worksheet.Cells[recordIndex, 18].Value = row[17].ToString();
                    worksheet.Cells[recordIndex, 19].Value = row[18].ToString();
                    worksheet.Cells[recordIndex, 20].Value = row[19].ToString();
                    worksheet.Cells[recordIndex, 21].Value = row[20].ToString();
                    worksheet.Cells[recordIndex, 22].Value = row[21].ToString();
                    worksheet.Cells[recordIndex, 23].Value = row[22].ToString();
                    worksheet.Cells[recordIndex, 24].Value = Convert.ToDouble(row[23].ToString());
                    worksheet.Cells[recordIndex, 25].Value = row[24].ToString();
                    worksheet.Cells[recordIndex, 26].Value = row[25].ToString();
                    recordIndex++;
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(xlPackage.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }
        public void CallItemExports(string selection, FileInfo newFile, string fileName, object[] objPageDetails)
        {
            ItemExports(selection, newFile, fileName, objPageDetails);
        }
        //Puts the clubs in the export table
        public System.Data.DataTable ExportAllAdd_Clubs(object[] objPageDetails)
        {
            string strQueryName = "exportAllAdd_Clubs";
            string sqlCmd = ExportClubString();
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ExportClubString()
        {
            string sqlCmd = "SELECT '' AS Vendor, (SELECT tbl_location.varLocationName FROM tbl_location WHERE tbl_location.intLocationID = tbl_clubs.intLocationID) AS varLocationName, "
                + "tbl_clubs.varSku, '' AS ShipmentDate, (SELECT tbl_brand.varBrandName FROM tbl_brand WHERE tbl_brand.intBrandID = tbl_clubs.intBrandID ) AS varBrandName , (SELECT "
                + "tbl_model.varModelName FROM tbl_model WHERE tbl_model.intModelID = tbl_clubs.intModelID) AS varModelName, tbl_clubs.varTypeOfClub, tbl_clubs.varShaftType, "
                + "tbl_clubs.varNumberOfClubs, 0 AS TradeinPrice, tbl_clubs.fltPremiumCharge, tbl_clubs.fltCost, tbl_clubs.intQuantity, 0 AS ExtendedPrice, tbl_clubs.fltPrice, "
                + "tbl_clubs.varAdditionalInformation, '' AS Image, tbl_clubs.varClubSpecification, tbl_clubs.varShaftSpecification, tbl_clubs.varShaftFlexability, "
                + "tbl_clubs.varClubDexterity, (SELECT tbl_location.varSecondLocationID FROM tbl_location WHERE tbl_location.intLocationID = tbl_clubs.intLocationID) AS "
                + "locationSecondary, '' AS Received, 0 AS Paid, (SELECT tbl_itemType.varItemTypeName FROM tbl_itemType WHERE tbl_itemType.intItemTypeID = tbl_clubs.intItemTypeID) AS "
                + "itemType, tbl_clubs.bitIsUsedProduct FROM tbl_clubs";
            return sqlCmd;
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Puts the accessories in the export table
        public System.Data.DataTable ExportAllAdd_Accessories(object[] objPageDetails)
        {
            string strQueryName = "exportAllAdd_Accessories";
            string sqlCmd = ExportAccessoryString();
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ExportAccessoryString()
        {
            string sqlCmd = "SELECT '' AS Vendor, (SELECT tbl_location.varLocationName FROM tbl_location WHERE tbl_location.intLocationID = tbl_accessories.intLocationID) AS varLocationName, "
                + "tbl_accessories.varSku, '' AS ShipmentDate, (SELECT tbl_brand.varBrandName FROM tbl_brand WHERE tbl_brand.intBrandID = tbl_accessories.intBrandID) AS varBrandName, (SELECT "
                + "tbl_model.varModelName FROM tbl_model WHERE tbl_model.intModelID = tbl_accessories.intModelID) AS varModelName, tbl_accessories.varTypeOfAccessory AS varTypeOfClub, "
                + "tbl_accessories.varColour AS varShaftType, tbl_accessories.varSize AS varNumberOfClubs, 0 AS TradeinPrice, 0 AS fltPremiumCharge, tbl_accessories.fltCost, "
                + "tbl_accessories.intQuantity, 0 AS ExtendedPrice, tbl_accessories.fltPrice, tbl_accessories.varAdditionalInformation, '' AS Image, '' AS varClubSpecification, '' AS "
                + "varShaftSpecification, '' AS varShaftFlexability, '' as varClubDexterity, (SELECT tbl_location.varSecondLocationID FROM tbl_location WHERE tbl_location.intLocationID = "
                + "tbl_accessories.intLocationID) AS locationSecondary, '' AS Received, 0 AS Paid, (SELECT tbl_itemType.varItemTypeName FROM tbl_itemType WHERE tbl_itemType.intItemTypeID = "
                + "tbl_accessories.intItemTypeID) AS itemType, 0 AS bitIsUsedProduct FROM tbl_accessories";
            return sqlCmd;
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Puts the clothing in the export table
        public System.Data.DataTable ExportAllAdd_Clothing(object[] objPageDetails)
        {
            string strQueryName = "exportAllAdd_Clothing";
            string sqlCmd = ExportClothingString();
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ExportClothingString()
        {
            string sqlCmd = "SELECT '' AS Vendor, (SELECT tbl_location.varLocationName FROM tbl_location WHERE tbl_location.intLocationID = tbl_clothing.intLocationID) AS varLocationName, "
                + "tbl_clothing.varSku, '' AS ShipmentDate, (SELECT tbl_brand.varBrandName FROM tbl_brand WHERE tbl_brand.intBrandID = tbl_clothing.intBrandID) AS varBrandName, '' AS "
                + "varModelName, tbl_clothing.varStyle AS varTypeOfClub, tbl_clothing.varColour AS varShaftType, tbl_clothing.varSize AS varNumberOfClubs, 0 AS TradeinPrice, 0 AS "
                + "fltPremiumCharge, tbl_clothing.fltCost, tbl_clothing.intQuantity, 0 AS ExtendedPrice, tbl_clothing.fltPrice, tbl_clothing.varAdditionalInformation, '' AS Image, "
                + "tbl_clothing.varGender AS varClubSpecification, '' AS varShaftSpecification, '' AS varShaftFlexability, '' AS varClubDexterity, (SELECT tbl_location.varSecondLocationID "
                + "FROM tbl_location WHERE tbl_location.intLocationID = tbl_clothing.intLocationID) AS locationSecondary, '' AS Received, 0 AS Paid, (SELECT tbl_itemType.varItemTypeName "
                + "FROM tbl_itemType WHERE tbl_itemType.intItemTypeID = tbl_clothing.intItemTypeID) AS itemType, 0 AS bitIsUsedProduct FROM tbl_clothing";
            return sqlCmd;
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private System.Data.DataTable ExportAllInventory(object[] objPageDetails)
        {
            string strQueryName = "exportAllInventory";
            string sqlCmd = "";
            sqlCmd += ExportClubString();
            sqlCmd += " UNION ";
            sqlCmd += ExportAccessoryString();
            sqlCmd += " UNION ";
            sqlCmd += ExportClothingString();

            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
    }
}