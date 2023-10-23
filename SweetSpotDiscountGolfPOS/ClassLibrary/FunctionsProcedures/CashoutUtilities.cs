using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.Misc;
using SweetSpotDiscountGolfPOS.OB;

namespace SweetSpotDiscountGolfPOS.FP
{
    public class CashoutUtilities
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();

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
            {
                bolTA = true;
            }
            return bolTA;
        }
        private bool OpenTransactions(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "openTransactions";
            bool bolOT = false;
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_currentSalesInvoice WHERE intTransactionTypeID = (SELECT intTransactionTypeID FROM tbl_transactionType WHERE varTransactionName = 'Sale') "
                + "AND intLocationID = @intLocationID AND dtmInvoiceDate = @dtmInvoiceDate";
            object[][] parms =
            {
                new object[] { "@intLocationID", locationID },
                new object[] { "@dtmInvoiceDate", selectedDate.ToString("yyyy-MM-dd")}
            };
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolOT = true;
            }
            return bolOT;
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
            {
                bolCAD = true;
            }
            return bolCAD;
        }
        public void RemoveUnprocessedReturns(int locationID, DateTime selectedDate, CurrentUser CU, object[] objPageDetails)
        {
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            DataTable dt = ReturnListOfOpenInvoices(locationID, selectedDate, objPageDetails);
            foreach (DataRow dtr in dt.Rows)
            {
                RemoveOpenMopsFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
                RemoveOpenItemTaxesFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
                //need to add into stock Github Issue #20: On Hold Sales 
                //Don't beleive this is needed as the only objects that should come through here are unprocessed returns.
                //Anything that is a sale will already be delt with, anything on hold doesn't count
                //IIM.LoopThroughTheItemsToReturnToInventory(Convert.ToInt32(dtr[0]), selectedDate, CU.location.intProvinceID, objPageDetails);

                RemoveOpenItemsFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
                RemoveOpenInvoiceFromCurrentSales(Convert.ToInt32(dtr[0]), objPageDetails);
            }
        }
        private DataTable ReturnListOfOpenInvoices(int locationID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "ReturnListOfReturnInvoices";
            string sqlCmd = "SELECT intInvoiceID FROM tbl_currentSalesInvoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate "
                + "AND intLocationID = @intLocationID AND intTransactionTypeID <> "
                + "(SELECT intTransactionTypeID FROM tbl_transactionType WHERE varTransactionName = 'On Hold')";
            //add intTransactionTypeID <> 3 GitHub Issue #20: On Hold Sales
            object[][] parms =
            {
                new object[] { "@dtmStartDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmEndDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveOpenMopsFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnMopsFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveOpenItemTaxesFromCurrentSales(int invoiceID, object[] objPageDetails)
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
        private void RemoveOpenItemsFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnItemsFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveOpenInvoiceFromCurrentSales(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "RemoveReturnInvoiceFromCurrentSales";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public bool CallCashoutExists(object[] args, object[] objPageDetails)
        {
            return CashoutExists(args, objPageDetails);
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
            {
                exists = true;
            }
            return exists;
        }
        public Cashout CallSelectedCashoutToReturn(object[] args, object[] objPageDetails)
        {
            Cashout cOut;
            cOut = RecalculateCashoutTotals(args, objPageDetails);
            return cOut;
        }
        private Cashout RecalculateCashoutTotals(object[] args, object[] objPageDetails)
        {
            Cashout cOut = CreateNewCashout(DateTime.Parse(args[0].ToString()), Convert.ToInt32(args[1]), objPageDetails);
            UpdateCalculatedTotalsCashout(cOut, objPageDetails);
            return ReturnSelectedCashout(args, objPageDetails)[0];
        }
        public Cashout CreateNewCashout(DateTime startDate, int locationID, object[] objPageDetails)
        {
            ////Now need to account for anything in Layaway status
            ////These queries will need to be updated
            //Return PivotTable of a list of Mops
            DataTable dt1 = ReturnListOfMOPS(startDate, locationID, objPageDetails);
            //Gather remaining Totals (taxes, subtotal(including shipping), tradein)
            DataTable dt2 = ReturnAdditionTotalsForCashout(startDate, locationID, objPageDetails);
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
                fltSystemCountedBasedOnSystemAmEx = Convert.ToDouble(dt1.Rows[0][6].ToString()),
                fltSalesSubTotal = Convert.ToDouble(dt2.Rows[0][1].ToString()),
                fltGovernmentTaxAmount = Convert.ToDouble(dt2.Rows[0][2].ToString()) + Convert.ToDouble(dt2.Rows[0][4].ToString()),
                fltHarmonizedTaxAmount = Convert.ToDouble(dt2.Rows[0][3].ToString()) + Convert.ToDouble(dt2.Rows[0][5].ToString()),
                fltLiquorTaxAmount = Convert.ToDouble(dt2.Rows[0][6].ToString()),
                fltProvincialTaxAmount = Convert.ToDouble(dt2.Rows[0][7].ToString()),
                fltQuebecTaxAmount = Convert.ToDouble(dt2.Rows[0][8].ToString()),
                fltRetailTaxAmount = Convert.ToDouble(dt2.Rows[0][9].ToString())
            };
            return cashout;
        }
        private void UpdateCalculatedTotalsCashout(Cashout cashout, object[] objPageDetails)
        {
            string strQueryName = "UpdateCashout";
            string sqlCmd = "UPDATE tbl_cashout SET fltSystemCountedBasedOnSystemTradeIn = @fltSystemCountedBasedOnSystemTradeIn, fltSystemCountedBasedOnSystemGiftCard "
                + "= @fltSystemCountedBasedOnSystemGiftCard, fltSystemCountedBasedOnSystemCash = @fltSystemCountedBasedOnSystemCash, fltSystemCountedBasedOnSystemDebit "
                + "= @fltSystemCountedBasedOnSystemDebit, fltSystemCountedBasedOnSystemMastercard = @fltSystemCountedBasedOnSystemMastercard, "
                + "fltSystemCountedBasedOnSystemVisa = @fltSystemCountedBasedOnSystemVisa, fltSystemCountedBasedOnSystemAmEx = @fltSystemCountedBasedOnSystemAmEx, "
                + "fltSalesSubTotal = @fltSalesSubTotal, fltGovernmentTaxAmount = "
                + "@fltGovernmentTaxAmount, fltProvincialTaxAmount = @fltProvincialTaxAmount, fltLiquorTaxAmount = @fltLiquorTaxAmount, fltHarmonizedTaxAmount = "
                + "@fltHarmonizedTaxAmount, fltQuebecTaxAmount = @fltQuebecTaxAmount, fltRetailTaxAmount = @fltRetailTaxAmount "
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
                new object[] { "@fltSystemCountedBasedOnSystemAmEx", cashout.fltSystemCountedBasedOnSystemAmEx },
                new object[] { "@fltSalesSubTotal", cashout.fltSalesSubTotal },
                new object[] { "@fltGovernmentTaxAmount", cashout.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", cashout.fltProvincialTaxAmount },
                new object[] { "@fltLiquorTaxAmount", cashout.fltLiquorTaxAmount },
                new object[] { "@fltHarmonizedTaxAmount", cashout.fltHarmonizedTaxAmount },
                new object[] { "@fltQuebecTaxAmount", cashout.fltQuebecTaxAmount },
                new object[] { "@fltRetailTaxAmount", cashout.fltRetailTaxAmount },
                new object[] { "@intLocationID", cashout.intLocationID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<Cashout> ReturnSelectedCashout(object[] args, object[] objPageDetails)
        {
            string strQueryName = "ReturnSelectedCashout";
            string sqlCmd = "SELECT dtmCashoutDate, intLocationID, intEmployeeID, fltSystemCountedBasedOnSystemTradeIn, fltSystemCountedBasedOnSystemGiftCard, "
                + "fltSystemCountedBasedOnSystemCash, fltSystemCountedBasedOnSystemDebit, fltSystemCountedBasedOnSystemMastercard, fltSystemCountedBasedOnSystemVisa, "
                + "fltSystemCountedBasedOnSystemAmEx, "
                + "fltManuallyCountedBasedOnReceiptsTradeIn, fltManuallyCountedBasedOnReceiptsGiftCard, fltManuallyCountedBasedOnReceiptsCash, "
                + "fltManuallyCountedBasedOnReceiptsDebit, fltManuallyCountedBasedOnReceiptsMastercard, fltManuallyCountedBasedOnReceiptsVisa, "
                + "fltManuallyCountedBasedOnReceiptsAmEx, fltSalesSubTotal, "
                + "fltGovernmentTaxAmount, fltHarmonizedTaxAmount, fltLiquorTaxAmount, fltProvincialTaxAmount, fltQuebecTaxAmount, fltRetailTaxAmount, "
                + "fltCashDrawerOverShort, bitIsCashoutFinalized, bitIsCashoutProcessed " //, intSalesCount 
                + "FROM tbl_cashout WHERE dtmCashoutDate = @dtmCashoutDate AND "
                + "intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmCashoutDate", DateTime.Parse(args[0].ToString()).ToShortDateString() },
                new object[] { "@intLocationID", Convert.ToInt32(args[1]) }
            };
            return ReturnCashoutFromDataTable(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private DataTable ReturnListOfMOPS(DateTime startDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnListOfMOPS";
            string sqlCmd = "SELECT dtmInvoiceDate, ISNULL([5], 0) AS Cash, ISNULL([7], 0) AS Debit, ISNULL([6], 0) AS GiftCard, ISNULL([2], 0) AS Mastercard, "
                + "ISNULL([1],0) AS Visa, ISNULL([3],0) AS AmEx FROM(SELECT i.dtmInvoiceDate, m.intPaymentID, SUM(fltAmountPaid) AS fltAmountPaid FROM tbl_invoiceMOP m JOIN "
                + "tbl_invoice i ON m.intInvoiceID = i.intInvoiceID WHERE i.dtmInvoiceDate = @startDate AND i.intLocationID = @intLocationID GROUP BY "
                + "i.dtmInvoiceDate, m.intPaymentID) ps PIVOT(SUM(fltAmountPaid) FOR intPaymentID IN([5], [7], [6], [2], [1], [3])) AS pvt";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private DataTable ReturnAdditionTotalsForCashout(DateTime startDate, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnAdditionTotalsForCashout";
            string sqlCmd = "SELECT ISNULL(SUM(I.fltTotalTradeIn), 0) AS fltTotalTradeIn, ISNULL(SUM(I.fltSubTotal), 0) + ISNULL(SUM(I.fltShippingCharges), 0) AS "
                + "fltSalesSubTotal, SUM(CASE WHEN I.bitIsShipping = 1 THEN CASE WHEN(I.intShippingProvinceID = 1 OR I.intShippingProvinceID = 3 OR "
                + "I.intShippingProvinceID = 4 OR I.intShippingProvinceID = 5 OR I.intShippingProvinceID = 6 OR I.intShippingProvinceID = 9 OR "
                + "I.intShippingProvinceID = 12 OR I.intShippingProvinceID = 13) THEN I.fltShippingTaxAmount ELSE 0 END ELSE 0 END) AS fltShippingGSTAmount, SUM("
                + "CASE WHEN I.bitIsShipping = 1 THEN CASE WHEN(I.intShippingProvinceID = 2 OR I.intShippingProvinceID = 7 OR I.intShippingProvinceID = 8 OR "
                + "I.intShippingProvinceID = 10 OR I.intShippingProvinceID = 11) THEN I.fltShippingTaxAmount ELSE 0 END ELSE 0 END) AS fltShippingHSTAmount, "
                + "(SELECT SUM(GT.fltTaxAmount) AS fltGovernmentTaxAmount FROM (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'GST') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'GST')) AS GT) AS fltGovernmentTaxAmount, (SELECT SUM(HT.fltTaxAmount) AS "
                + "fltHarmonizedTaxAmount FROM (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem "
                + "II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate "
                + "AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = "
                + "'HST') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON "
                + "IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND "
                + "I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'HST')) "
                + "AS HT) AS fltHarmonizedTaxAmount, (SELECT SUM(LT.fltTaxAmount) AS fltLiquorTaxAmount FROM (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS "
                + "fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON "
                + "I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND "
                + "IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'LCT') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount "
                + "FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'LCT')) AS LT) AS fltLiquorTaxAmount, (SELECT SUM(PT.fltTaxAmount) AS fltProvincialTaxAmount "
                + "FROM (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
                + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = "
                + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'PST') UNION SELECT "
                + "ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = "
                + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = "
                + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'PST')) AS PT) AS "
                + "fltProvincialTaxAmount, (SELECT SUM(QT.fltTaxAmount) AS fltQuebecTaxAmount FROM (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount "
                + "FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'QST') UNION SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = "
                + "IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID "
                + "IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'QST')) AS QT) AS fltQuebecTaxAmount, (SELECT SUM(RT.fltTaxAmount) AS fltRetailTaxAmount "
                + "FROM (SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
                + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = "
                + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'RST') UNION SELECT "
                + "ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = "
                + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = IIR.intInvoiceID WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = "
                + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND IIT.intTaxTypeID IN(SELECT intTaxID FROM tbl_taxType WHERE varTaxName = 'RST')) AS RT) AS "
                + "fltRetailTaxAmount FROM tbl_invoice I WHERE I.dtmInvoiceDate = @dtmStartDate AND I.intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@intLocationID", locationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<Cashout> ReturnCashoutFromDataTable(DataTable dt)
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
                fltSystemCountedBasedOnSystemAmEx = row.Field<double>("fltSystemCountedBasedOnSystemAmEx"),
                fltManuallyCountedBasedOnReceiptsTradeIn = row.Field<double>("fltManuallyCountedBasedOnReceiptsTradeIn"),
                fltManuallyCountedBasedOnReceiptsGiftCard = row.Field<double>("fltManuallyCountedBasedOnReceiptsGiftCard"),
                fltManuallyCountedBasedOnReceiptsCash = row.Field<double>("fltManuallyCountedBasedOnReceiptsCash"),
                fltManuallyCountedBasedOnReceiptsDebit = row.Field<double>("fltManuallyCountedBasedOnReceiptsDebit"),
                fltManuallyCountedBasedOnReceiptsMastercard = row.Field<double>("fltManuallyCountedBasedOnReceiptsMastercard"),
                fltManuallyCountedBasedOnReceiptsVisa = row.Field<double>("fltManuallyCountedBasedOnReceiptsVisa"),
                fltManuallyCountedBasedOnReceiptsAmEx = row.Field<double>("fltManuallyCountedBasedOnReceiptsAmEx"),
                fltSalesSubTotal = row.Field<double>("fltSalesSubTotal"),
                fltGovernmentTaxAmount = row.Field<double>("fltGovernmentTaxAmount"),
                fltHarmonizedTaxAmount = row.Field<double>("fltHarmonizedTaxAmount"),
                fltLiquorTaxAmount = row.Field<double>("fltLiquorTaxAmount"),
                fltProvincialTaxAmount = row.Field<double>("fltProvincialTaxAmount"),
                fltQuebecTaxAmount = row.Field<double>("fltQuebecTaxAmount"),
                fltRetailTaxAmount = row.Field<double>("fltRetailTaxAmount"),
                fltCashDrawerOverShort = row.Field<double>("fltCashDrawerOverShort"),
                bitIsCashoutFinalized = row.Field<bool>("bitIsCashoutFinalized"),
                bitIsCashoutProcessed = row.Field<bool>("bitIsCashoutProcessed"),
                intEmployeeID = row.Field<int>("intEmployeeID"),
                intLocationID = row.Field<int>("intLocationID")
            }).ToList();
            return cashout;
        }
        public void CallInsertCashout(Cashout cashout, object[] objPageDetails)
        {
            InsertCashout(cashout, objPageDetails);
        }
        private void InsertCashout(Cashout cashout, object[] objPageDetails)
        {
            string strQueryName = "insertCashout";
            string sqlCmd = "INSERT INTO tbl_cashout VALUES(@dtmCashoutDate, @dtmCashoutTime, @intLocationID, @intEmployeeID, "
                + "@fltSystemCountedBasedOnSystemTradeIn, @fltSystemCountedBasedOnSystemGiftCard, @fltSystemCountedBasedOnSystemCash, "
                + "@fltSystemCountedBasedOnSystemDebit, @fltSystemCountedBasedOnSystemMastercard, @fltSystemCountedBasedOnSystemVisa, "
                + "@fltSystemCountedBasedOnSystemAmEx, "
                + "@fltManuallyCountedBasedOnReceiptsTradeIn, @fltManuallyCountedBasedOnReceiptsGiftCard, @fltManuallyCountedBasedOnReceiptsCash, "
                + "@fltManuallyCountedBasedOnReceiptsDebit, @fltManuallyCountedBasedOnReceiptsMastercard, @fltManuallyCountedBasedOnReceiptsVisa, "
                + "@fltManuallyCountedBasedOnReceiptsAmEx, "
                + "@fltSalesSubTotal, @fltGovernmentTaxAmount, @fltHarmonizedTaxAmount, @fltLiquorTaxAmount, "
                + "@fltProvincialTaxAmount, @fltQuebecTaxAmount, @fltRetailTaxAmount, "
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
                new object[] { "@fltSystemCountedBasedOnSystemAmEx", cashout.fltSystemCountedBasedOnSystemAmEx },

                new object[] { "@fltManuallyCountedBasedOnReceiptsTradeIn", cashout.fltManuallyCountedBasedOnReceiptsTradeIn },
                new object[] { "@fltManuallyCountedBasedOnReceiptsGiftCard", cashout.fltManuallyCountedBasedOnReceiptsGiftCard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsCash", cashout.fltManuallyCountedBasedOnReceiptsCash },
                new object[] { "@fltManuallyCountedBasedOnReceiptsDebit", cashout.fltManuallyCountedBasedOnReceiptsDebit },
                new object[] { "@fltManuallyCountedBasedOnReceiptsMastercard", cashout.fltManuallyCountedBasedOnReceiptsMastercard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsVisa", cashout.fltManuallyCountedBasedOnReceiptsVisa },
                new object[] { "@fltManuallyCountedBasedOnReceiptsAmEx", cashout.fltManuallyCountedBasedOnReceiptsAmEx },

                new object[] { "@fltSalesSubTotal", cashout.fltSalesSubTotal },
                new object[] { "@fltGovernmentTaxAmount", cashout.fltGovernmentTaxAmount },
                new object[] { "@fltHarmonizedTaxAmount", cashout.fltHarmonizedTaxAmount },
                new object[] { "@fltLiquorTaxAmount", cashout.fltLiquorTaxAmount },
                new object[] { "@fltProvincialTaxAmount", cashout.fltProvincialTaxAmount },
                new object[] { "@fltQuebecTaxAmount", cashout.fltQuebecTaxAmount },
                new object[] { "@fltRetailTaxAmount", cashout.fltRetailTaxAmount },

                new object[] { "@fltCashDrawerOverShort", cashout.fltCashDrawerOverShort },
                new object[] { "@bitIsCashoutFinalized", cashout.bitIsCashoutFinalized },
                new object[] { "@bitIsCashoutProcessed", cashout.bitIsCashoutProcessed },
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CallUpdateCashout(Cashout cashout, object[] objPageDetails)
        {
            UpdateCashout(cashout, objPageDetails);
        }
        private void UpdateCashout(Cashout cashout, object[] objPageDetails)
        {
            string strQueryName = "UpdateCashout";
            string sqlCmd = "UPDATE tbl_cashout SET dtmCashoutTime = @dtmCashoutTime, fltSystemCountedBasedOnSystemTradeIn = @fltSystemCountedBasedOnSystemTradeIn, "
                + "fltSystemCountedBasedOnSystemGiftCard = @fltSystemCountedBasedOnSystemGiftCard, fltSystemCountedBasedOnSystemCash = @fltSystemCountedBasedOnSystemCash, "
                + "fltSystemCountedBasedOnSystemDebit = @fltSystemCountedBasedOnSystemDebit, fltSystemCountedBasedOnSystemMastercard = "
                + "@fltSystemCountedBasedOnSystemMastercard, fltSystemCountedBasedOnSystemVisa = @fltSystemCountedBasedOnSystemVisa, "
                + "fltSystemCountedBasedOnSystemAmEx = @fltSystemCountedBasedOnSystemAmEx, "
                + "fltManuallyCountedBasedOnReceiptsTradeIn = @fltManuallyCountedBasedOnReceiptsTradeIn, fltManuallyCountedBasedOnReceiptsGiftCard = "
                + "@fltManuallyCountedBasedOnReceiptsGiftCard, fltManuallyCountedBasedOnReceiptsCash = @fltManuallyCountedBasedOnReceiptsCash, "
                + "fltManuallyCountedBasedOnReceiptsDebit = @fltManuallyCountedBasedOnReceiptsDebit, fltManuallyCountedBasedOnReceiptsMastercard = "
                + "@fltManuallyCountedBasedOnReceiptsMastercard, fltManuallyCountedBasedOnReceiptsVisa = @fltManuallyCountedBasedOnReceiptsVisa, "
                + "fltManuallyCountedBasedOnReceiptsAmEx = @fltManuallyCountedBasedOnReceiptsAmEx, fltSalesSubTotal = "
                + "@fltSalesSubTotal, fltGovernmentTaxAmount = @fltGovernmentTaxAmount, fltHarmonizedTaxAmount = @fltHarmonizedTaxAmount, fltLiquorTaxAmount = @fltLiquorTaxAmount, "
                + "fltProvincialTaxAmount = @fltProvincialTaxAmount, fltQuebecTaxAmount = @fltQuebecTaxAmount, fltRetailTaxAmount = @fltRetailTaxAmount, "
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
                new object[] { "@fltSystemCountedBasedOnSystemAmEx", cashout.fltSystemCountedBasedOnSystemAmEx },
                new object[] { "@fltManuallyCountedBasedOnReceiptsTradeIn", cashout.fltManuallyCountedBasedOnReceiptsTradeIn },
                new object[] { "@fltManuallyCountedBasedOnReceiptsGiftCard", cashout.fltManuallyCountedBasedOnReceiptsGiftCard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsCash", cashout.fltManuallyCountedBasedOnReceiptsCash },
                new object[] { "@fltManuallyCountedBasedOnReceiptsDebit", cashout.fltManuallyCountedBasedOnReceiptsDebit },
                new object[] { "@fltManuallyCountedBasedOnReceiptsMastercard", cashout.fltManuallyCountedBasedOnReceiptsMastercard },
                new object[] { "@fltManuallyCountedBasedOnReceiptsVisa", cashout.fltManuallyCountedBasedOnReceiptsVisa },
                new object[] { "@fltManuallyCountedBasedOnReceiptsAmEx", cashout.fltManuallyCountedBasedOnReceiptsAmEx },
                new object[] { "@fltSalesSubTotal", cashout.fltSalesSubTotal },
                new object[] { "@fltGovernmentTaxAmount", cashout.fltGovernmentTaxAmount },
                new object[] { "@fltHarmonizedTaxAmount", cashout.fltHarmonizedTaxAmount },
                new object[] { "@fltLiquorTaxAmount", cashout.fltLiquorTaxAmount },
                new object[] { "@fltProvincialTaxAmount", cashout.fltProvincialTaxAmount },
                new object[] { "@fltQuebecTaxAmount", cashout.fltQuebecTaxAmount },
                new object[] { "@fltRetailTaxAmount", cashout.fltRetailTaxAmount },
                new object[] { "@fltCashDrawerOverShort", cashout.fltCashDrawerOverShort },
                new object[] { "@bitIsCashoutFinalized", cashout.bitIsCashoutFinalized },
                new object[] { "@bitIsCashoutProcessed", cashout.bitIsCashoutProcessed },
                new object[] { "@intLocationID", cashout.intLocationID },
                new object[] { "@intEmployeeID", cashout.intEmployeeID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CollectAndStoreDailySalesData(DateTime dtmDate, int intLocationID, object[] objPageDetails)
        {
            //UPDATE ON 3/06/22 GITHUB UPDATE #15
            //gather daily sales datat for date
            DataTable dt = CallReturnDailySalesStatsForLocationAndDate(dtmDate, intLocationID, objPageDetails);
            bool savedData = ReturnIfDailySalesDataAlreadyStored(dtmDate, intLocationID, objPageDetails);
            if (savedData)
            {
                //update the table data
                UpdateDailySalesData(dt, objPageDetails);
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    //store new data
                    StoreNewDailySalesData(dt, objPageDetails);
                }
            }
        }
        private DataTable CallReturnDailySalesStatsForLocationAndDate(DateTime dtmDate, int intLocationID, object[] objPageDetails)
        {
            return ReturnDailySalesStatsForLocationAndDate(dtmDate, intLocationID, objPageDetails);
        }
        private DataTable ReturnDailySalesStatsForLocationAndDate(DateTime dtmDate, int intLocationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnDailySalesStatsForLocationAndDate";
            string sqlCmd = "SELECT dtmInvoiceDate, intLocationID, ROUND(ISNULL(SUM(fltSubTotal), 0), 2) AS fltSubTotal, ROUND(ISNULL(SUM(fltGovernmentTaxCollected), "
                + "0), 2) AS fltGovernmentTaxCollected, ROUND(ISNULL(SUM(fltGovernmentTaxReturned), 0), 2) AS fltGovernmentTaxReturned, ROUND(ISNULL(SUM("
                + "fltProvincialTaxCollected), 0), 2) AS fltProvincialTaxCollected, ROUND(ISNULL(SUM(fltProvincialTaxReturned), 0), 2) AS fltProvincialTaxReturned, "
                + "ROUND(ISNULL(SUM(fltHarmonizedSalesTaxCollected), 0), 2) AS fltHarmonizedSalesTaxCollected, ROUND(ISNULL(SUM(fltHarmonizedSalesTaxReturned), 0), 2) "
                + "AS fltHarmonizedSalesTaxReturned, ROUND(ISNULL(SUM(fltRetailSalesTaxCollected), 0), 2) AS fltRetailSalesTaxCollected, ROUND(ISNULL(SUM("
                + "fltRetailSalesTaxReturned), 0), 2) AS fltRetailSalesTaxReturned, ROUND(ISNULL(SUM(fltQuebecSalesTaxCollected), 0), 2) AS fltQuebecSalesTaxCollected, "
                + "ROUND(ISNULL(SUM(fltQuebecSalesTaxReturned), 0), 2) AS fltQuebecSalesTaxReturned, ROUND(ISNULL(SUM(fltLiquorConsumptionTaxCollected), 0), 2) AS "
                + "fltLiquorConsumptionTaxCollected, ROUND(ISNULL(SUM(fltLiquorConsumptionTaxReturned), 0), 2) AS fltLiquorConsumptionTaxReturned, ROUND(ISNULL(SUM("
                + "fltSalesDollars), 0), 2) AS fltSalesDollars, ROUND(ISNULL(SUM(fltCostofGoods), 0), 2) AS fltCostofGoods FROM (SELECT I.intInvoiceID, I.dtmInvoiceDate, "
                + "I.intLocationID, fltSubTotal AS 'fltSubTotal', CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
                + "THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 1 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN "
                + "tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID WHERE II.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltGovernmentTaxCollected, CASE "
                + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID "
                + "= 1 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID "
                + "= IIT.intInvoiceItemID WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltGovernmentTaxReturned, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM "
                + "tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 2 AND IIT.bitIsTaxCharged = 1) THEN "
                + "IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID WHERE II.intInvoiceID = "
                + "I.intInvoiceID) ELSE 0 END AS fltProvincialTaxCollected, CASE WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID "
                + "= I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 2 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 "
                + "END AS fltProvincialTaxReturned, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT "
                + "SUM(CASE WHEN(IIT.intTaxTypeID = 3 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II "
                + "ON II.intInvoiceItemID = IIT.intInvoiceItemID WHERE II.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltHarmonizedSalesTaxCollected, CASE WHEN EXISTS("
                + "SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 3 AND "
                + "IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = "
                + "IIT.intInvoiceItemID WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltHarmonizedSalesTaxReturned, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM "
                + "tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 4 AND IIT.bitIsTaxCharged = 1) THEN "
                + "IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID WHERE II.intInvoiceID = "
                + "I.intInvoiceID) ELSE 0 END AS fltRetailSalesTaxCollected, CASE WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID "
                + "= I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 4 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes "
                + "IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS "
                + "fltRetailSalesTaxReturned, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE "
                + "WHEN(IIT.intTaxTypeID = 5 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON "
                + "II.intInvoiceItemID = IIT.intInvoiceItemID WHERE II.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltQuebecSalesTaxCollected, CASE WHEN EXISTS(SELECT "
                + "IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 5 AND "
                + "IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = "
                + "IIT.intInvoiceItemID WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltQuebecSalesTaxReturned, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM "
                + "tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 6 AND IIT.bitIsTaxCharged = 1) THEN "
                + "IIT.fltTaxAmount ELSE 0 END) FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID WHERE II.intInvoiceID = "
                + "I.intInvoiceID) ELSE 0 END AS fltLiquorConsumptionTaxCollected, CASE WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE "
                + "IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN(IIT.intTaxTypeID = 6 AND IIT.bitIsTaxCharged = 1) THEN IIT.fltTaxAmount ELSE 0 END) FROM "
                + "tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItemReturns IIR ON IIR.intInvoiceItemID = IIT.intInvoiceItemID WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 "
                + "END AS fltLiquorConsumptionTaxReturned, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN "
                + "(SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE(fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 THEN(fltItemPrice * (fltItemDiscount / 100)) "
                + "ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID "
                + "FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE(fltItemRefund * "
                + "intItemQuantity) END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, CASE WHEN EXISTS(SELECT "
                + "II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItem II "
                + "WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) "
                + "THEN(SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltCostofGoods' "
                + "FROM tbl_invoice I WHERE I.dtmInvoiceDate = @dtmDate AND I.intLocationID = @intLocationID) ABC GROUP BY dtmInvoiceDate, intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmDate", dtmDate },
                new object[] { "@intLocationID", intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private bool ReturnIfDailySalesDataAlreadyStored(DateTime dtmDate, int intLocationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnIfDailySalesDataAlreadyStored";
            bool savedData = false;
            string sqlCmd = "SELECT COUNT(dtmSalesDataDate) FROM tbl_dailySalesData WHERE dtmSalesDataDate = @dtmDate AND intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@dtmDate", dtmDate },
                new object[] { "@intLocationID", intLocationID }
            };
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                savedData = true;
            }
            return savedData;
        }
        private void UpdateDailySalesData(DataTable dt, object[] objPageDetails)
        {
            //UPDATE ON 3/06/22 GITHUB UPDATE #15
            string strQueryName = "UpdateDailySalesData";
            string sqlCmd = "UPDATE tbl_dailySalesData SET fltGSTCollected = @fltGSTCollected, fltGSTReturned = @fltGSTReturned, fltPSTCollected = @fltPSTCollected, "
                + "fltPSTReturned = @fltPSTReturned, fltHSTCollected = @fltHSTCollected, fltHSTReturned = @fltHSTReturned, fltQSTCollected = @fltQSTCollected, "
                + "fltQSTReturned = @fltQSTReturned, fltRSTCollected = @fltRSTCollected, fltRSTReturned = @fltRSTReturned, fltLCTCollected = @fltLCTCollected, "
                + "fltLCTReturned = @fltLCTReturned, fltSalesDollars = @fltSalesDollars, fltSubTotal = @fltSubTotal, fltCostGoodsSold = @fltCostGoodsSold WHERE "
                + "dtmSalesDataDate = @dtmSalesDataDate AND intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@fltGSTCollected", Convert.ToDouble(dt.Rows[0][3].ToString()) },
                new object[] { "@fltGSTReturned", Convert.ToDouble(dt.Rows[0][4].ToString()) },
                new object[] { "@fltPSTCollected", Convert.ToDouble(dt.Rows[0][5].ToString()) },
                new object[] { "@fltPSTReturned", Convert.ToDouble(dt.Rows[0][6].ToString()) },
                new object[] { "@fltHSTCollected", Convert.ToDouble(dt.Rows[0][7].ToString()) },
                new object[] { "@fltHSTReturned", Convert.ToDouble(dt.Rows[0][8].ToString()) },
                new object[] { "@fltQSTCollected", Convert.ToDouble(dt.Rows[0][11].ToString()) },
                new object[] { "@fltQSTReturned", Convert.ToDouble(dt.Rows[0][12].ToString()) },
                new object[] { "@fltRSTCollected", Convert.ToDouble(dt.Rows[0][9].ToString()) },
                new object[] { "@fltRSTReturned", Convert.ToDouble(dt.Rows[0][10].ToString()) },
                new object[] { "@fltLCTCollected", Convert.ToDouble(dt.Rows[0][13].ToString()) },
                new object[] { "@fltLCTReturned", Convert.ToDouble(dt.Rows[0][14].ToString()) },
                new object[] { "@fltSalesDollars", Convert.ToDouble(dt.Rows[0][15].ToString()) },
                new object[] { "@fltSubTotal", Convert.ToDouble(dt.Rows[0][2].ToString()) },
                new object[] { "@fltCostGoodsSold", Convert.ToDouble(dt.Rows[0][16].ToString()) },
                new object[] { "@dtmSalesDataDate", ((DateTime)dt.Rows[0][0]).ToString("yyyy-MM-dd") },
                new object[] { "@intLocationID", Convert.ToInt32(dt.Rows[0][1].ToString()) }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void StoreNewDailySalesData(DataTable dt, object[] objPageDetails)
        {
            //UPDATE ON 3/06/22 GITHUB UPDATE #15
            double costOfInv = ReturnCostOfInventoryForLocation(Convert.ToInt32(dt.Rows[0][1].ToString()), objPageDetails);
            string strQueryName = "StoreNewDailySalesData";
            string sqlCmd = "INSERT INTO tbl_dailySalesData VALUES(@dtmSalesDataDate, @intLocationID, @fltGSTCollected, @fltGSTReturned, @fltPSTCollected, @fltPSTReturned, "
                + "@fltHSTCollected, @fltHSTReturned, @fltQSTCollected, @fltQSTReturned, @fltRSTCollected, @fltRSTReturned, @fltLCTCollected, @fltLCTReturned, "
                + "@fltSalesDollars, @fltSubTotal, @fltCostGoodsSold, @fltCostOfInventoryEOD, @dtmCOIDateTaken, @dtmCOITimeTaken)";

            object[][] parms =
            {
                new object[] { "@dtmSalesDataDate", ((DateTime)dt.Rows[0][0]).ToString("yyyy-MM-dd") },
                new object[] { "@intLocationID", Convert.ToInt32(dt.Rows[0][1].ToString()) },
                new object[] { "@fltGSTCollected", Convert.ToDouble(dt.Rows[0][3].ToString()) },
                new object[] { "@fltGSTReturned", Convert.ToDouble(dt.Rows[0][4].ToString()) },
                new object[] { "@fltPSTCollected", Convert.ToDouble(dt.Rows[0][5].ToString()) },
                new object[] { "@fltPSTReturned", Convert.ToDouble(dt.Rows[0][6].ToString()) },
                new object[] { "@fltHSTCollected", Convert.ToDouble(dt.Rows[0][7].ToString()) },
                new object[] { "@fltHSTReturned", Convert.ToDouble(dt.Rows[0][8].ToString()) },
                new object[] { "@fltQSTCollected", Convert.ToDouble(dt.Rows[0][11].ToString()) },
                new object[] { "@fltQSTReturned", Convert.ToDouble(dt.Rows[0][12].ToString()) },
                new object[] { "@fltRSTCollected", Convert.ToDouble(dt.Rows[0][9].ToString()) },
                new object[] { "@fltRSTReturned", Convert.ToDouble(dt.Rows[0][10].ToString()) },
                new object[] { "@fltLCTCollected", Convert.ToDouble(dt.Rows[0][13].ToString()) },
                new object[] { "@fltLCTReturned", Convert.ToDouble(dt.Rows[0][14].ToString()) },
                new object[] { "@fltSalesDollars", Convert.ToDouble(dt.Rows[0][15].ToString()) },
                new object[] { "@fltSubTotal", Convert.ToDouble(dt.Rows[0][2].ToString()) },
                new object[] { "@fltCostGoodsSold", Convert.ToDouble(dt.Rows[0][16].ToString()) },
                new object[] { "@fltCostOfInventoryEOD", costOfInv },
                new object[] { "@dtmCOIDateTaken", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@dtmCOITimeTaken", DateTime.Now.ToString("HH:mm:ss") }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private double ReturnCostOfInventoryForLocation(int intLocationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCostOfInventoryForLocation";
            string sqlCmd = "SELECT ROUND(ISNULL(fltItemAccessoryCost, 0), 2) + ROUND(ISNULL(fltItemClothingCost, 0), 2) + ROUND(ISNULL(fltItemClubsCost, 0), 2) "
                + "AS fltOverallCost FROM(SELECT intLocationID FROM tbl_location WHERE bitIsRetailStore = 1) L FULL JOIN(SELECT A.intLocationID, ROUND(SUM(fltCost * "
                + "intQuantity), 2) AS fltItemAccessoryCost FROM tbl_accessories A JOIN tbl_location L ON L.intLocationID = A.intLocationID WHERE intQuantity > 0 AND "
                + "L.bitIsRetailStore = 1 GROUP BY A.intLocationID) AC ON AC.intLocationID = L.intLocationID FULL JOIN(SELECT CL.intLocationID, ROUND(SUM(fltCost * "
                + "intQuantity), 2) AS fltItemClothingCost FROM tbl_clothing CL JOIN tbl_location L ON L.intLocationID = CL.intLocationID WHERE intQuantity > 0 AND "
                + "bitIsRetailStore = 1 GROUP BY CL.intLocationID) CLC ON CLC.intLocationID = L.intLocationID FULL JOIN(SELECT C.intLocationID, ROUND(SUM(fltCost * "
                + "intQuantity), 2) AS fltItemClubsCost FROM tbl_clubs C JOIN tbl_location L ON L.intLocationID = C.intLocationID WHERE intQuantity > 0 AND "
                + "bitIsRetailStore = 1 GROUP BY C.intLocationID) CC ON CC.intLocationID = L.intLocationID WHERE L.intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@intLocationID", intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDouble(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CallFinalizeCashout(string args, object[] objPageDetails)
        {
            FinalizeCashout(args, objPageDetails);
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
        }
    }
}