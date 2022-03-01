using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
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

        //^^^^^^^^^^^^^^^^^^^^REPORT TRACKING^^^^^^^^^^^^^^^^^^^^
        //All Reports run this so that we can track how many times each report is run.
        public void CallReportLogger(object[] reportLog, object[] objPageDetails)
        {
            //LogReportCall(reportLog, objPageDetails);
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

        //^^^^^^^^^^^^^^^^^^^^CASHOUT REPORT^^^^^^^^^^^^^^^^^^^^ (this report does not do group by)
        public int CashoutsProcessed(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!CashoutInDateRange(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool CashoutInDateRange(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "CashoutInDateRange";
            bool bolCIDR = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT COUNT(dtmCashoutDate) FROM tbl_cashout WHERE dtmCashoutDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolCIDR = true;
            }
            return bolCIDR;
        }
        public DataTable CallReturnCashoutsForSelectedDates(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnCashoutsForSelectedDates(repInfo, objPageDetails);
        }
        private DataTable ReturnCashoutsForSelectedDates(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "ReturnCashoutsForSelectedDates";
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT dtmCashoutDate, intLocationID, fltSystemCountedBasedOnSystemTradeIn, fltManuallyCountedBasedOnReceiptsTradeIn, "
                + "fltSystemCountedBasedOnSystemGiftCard, fltManuallyCountedBasedOnReceiptsGiftCard, fltSystemCountedBasedOnSystemCash, "
                + "fltManuallyCountedBasedOnReceiptsCash, fltSystemCountedBasedOnSystemDebit, fltManuallyCountedBasedOnReceiptsDebit, "
                + "fltSystemCountedBasedOnSystemMastercard, fltManuallyCountedBasedOnReceiptsMastercard, fltSystemCountedBasedOnSystemVisa, "
                + "fltManuallyCountedBasedOnReceiptsVisa, fltCashDrawerOverShort, bitIsCashoutProcessed, bitIsCashoutFinalized FROM "
                + "tbl_cashout WHERE dtmCashoutDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //^^^^^^^^^^^^^^^^^^^^TAX REPORT^^^^^^^^^^^^^^^^^^^^ (Done)
        public int VerifyTaxesCharged(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!TaxesAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool TaxesAvailable(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "taxesAvailable";
            bool bolData = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND I.intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT ROUND(ISNULL(SUM(fltTaxAmount), 0), 2) AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID "
                + "= IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND "
                + "IIT.bitIsTaxCharged = 1" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
            if (DBC.MakeDataBaseCallToReturnDouble(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }
        public List<TaxReport> CallReturnTaxReportDetails(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnTaxReportDetails(repInfo, objPageDetails);
        }
        private List<TaxReport> ReturnTaxReportDetails(ReportInformation repInfo, object[] objPageDetails)
        {
            DateTime cutOverDate = new DateTime(2022, 04, 01);

            if (DateTime.Today.Date > cutOverDate.Date)
            {
                string strQueryName = "returnTaxReportDetails";
                string varLocationName = "";
                string sqlCmd = "";
                string startQuery = "";
                string endQuery = "";
                if (repInfo.intLocationID != 99)
                {
                    varLocationName = " AND DSD.intLocationID = @intLocationID";
                }
                sqlCmd = "SELECT dtmInvoiceDate, varLocationName, ROUND(SUM(fltGovernmentTaxAmountCollected), 2) AS fltGovernmentTaxAmountCollected, ROUND(SUM("
                    + "fltHarmonizedTaxAmountCollected), 2) AS fltHarmonizedTaxAmountCollected, ROUND(SUM(fltLiquorTaxAmountCollected), 2) AS "
                    + "fltLiquorTaxAmountCollected, ROUND(SUM(fltProvincialTaxAmountCollected), 2) AS fltProvincialTaxAmountCollected, ROUND(SUM("
                    + "fltQuebecTaxAmountCollected), 2) AS fltQuebecTaxAmountCollected, ROUND(SUM(fltRetailTaxAmountCollected), 2) AS "
                    + "fltRetailTaxAmountCollected, ROUND(SUM(fltGovernmentTaxAmountReturned), 2) AS fltGovernmentTaxAmountReturned, ROUND(SUM("
                    + "fltHarmonizedTaxAmountReturned), 2) AS fltHarmonizedTaxAmountReturned, ROUND(SUM(fltLiquorTaxAmountReturned), 2) AS "
                    + "fltLiquorTaxAmountReturned, ROUND(SUM(fltProvincialTaxAmountReturned), 2) AS fltProvincialTaxAmountReturned, ROUND(SUM("
                    + "fltQuebecTaxAmountReturned), 2) AS fltQuebecTaxAmountReturned, ROUND(SUM(fltRetailTaxAmountReturned), 2) AS "
                    + "fltRetailTaxAmountReturned FROM (SELECT YEAR(dtmSalesDataDate) AS dtmInvoiceYear, DATENAME(MONTH, dtmSalesDataDate) AS "
                    + "varMonthName, DATEADD(MONTH, DATEDIFF(MONTH, 0, dtmSalesDataDate), 0) AS dtmMonthDate, DATEADD(DAY, 1 - DATEPART(WEEKDAY, "
                    + "dtmSalesDataDate), CAST(dtmSalesDataDate AS DATE)) dtmWeekStartDate, dtmSalesDataDate AS dtmInvoiceDate,  "
                    + "CASE WHEN varLocationName = 'Golf Traders' THEN varLocationName + ' - ' + varCityName ELSE varLocationName END AS varLocationName, "
                    + "ISNULL(fltGSTCollected, 0) AS fltGovernmentTaxAmountCollected, ISNULL(fltHSTCollected, 0) AS fltHarmonizedTaxAmountCollected, "
                    + "ISNULL(fltLCTCollected, 0) AS fltLiquorTaxAmountCollected, ISNULL(fltPSTCollected, 0) AS fltProvincialTaxAmountCollected, "
                    + "ISNULL(fltQSTCollected, 0) AS fltQuebecTaxAmountCollected, ISNULL(fltRSTCollected, 0) AS fltRetailTaxAmountCollected, ISNULL("
                    + "fltGSTReturned, 0) AS fltGovernmentTaxAmountReturned, ISNULL(fltHSTReturned, 0) AS fltHarmonizedTaxAmountReturned, ISNULL("
                    + "fltLCTReturned, 0) AS fltLiquorTaxAmountReturned, ISNULL(fltPSTReturned, 0) AS fltProvincialTaxAmountReturned, ISNULL("
                    + "fltQSTReturned, 0) AS fltQuebecTaxAmountReturned, ISNULL(fltRSTReturned, 0) AS fltRetailTaxAmountReturned FROM tbl_dailySalesData "
                    + "DSD JOIN tbl_location L ON L.intLocationID = DSD.intLocationID WHERE dtmSalesDataDate BETWEEN @dtmStartDate AND @dtmEndDate"
                    + varLocationName + ") AS ABC GROUP BY dtmInvoiceDate, varLocationName ORDER BY dtmInvoiceDate, varLocationName";

                object[][] parms =
                {
                    new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                    new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                    new object[] { "@intLocationID", repInfo.intLocationID }
                };
                return ConvertTaxReportFromDataTable(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            }
            else
            {
                string strQueryName = "returnTaxReportDetails";
                string sqlCmd = "SELECT D.dtmInvoiceDate, "
                    
                    + "ISNULL(C.fltGovernmentTaxAmountCollected, 0) AS fltGovernmentTaxAmountCollected, "
                    + "ISNULL(C.fltProvincialTaxAmountCollected, 0) AS fltProvincialTaxAmountCollected, "
                    + "ISNULL(C.fltLiquorTaxAmountCollected, 0) AS fltLiquorTaxAmountCollected, "
                    + "ISNULL(C.fltHarmonizedTaxAmountCollected, 0) AS fltHarmonizedTaxAmountCollected, "
                    + "ISNULL(C.fltQuebecTaxAmountCollected, 0) AS fltQuebecTaxAmountCollected, "
                    + "ISNULL(C.fltRetailTaxAmountCollected, 0) AS fltRetailTaxAmountCollected, "
                    + "ISNULL(R.fltGovernmentTaxAmountReturned, 0) AS fltGovernmentTaxAmountReturned, "
                    + "ISNULL(R.fltProvincialTaxAmountReturned, 0) AS fltProvincialTaxAmountReturned, "
                    + "ISNULL(R.fltLiquorTaxAmountReturned, 0) AS fltLiquorTaxAmountReturned, "
                    + "ISNULL(R.fltHarmonizedTaxAmountReturned, 0) AS fltHarmonizedTaxAmountReturned, "
                    + "ISNULL(R.fltQuebecTaxAmountReturned, 0) AS fltQuebecTaxAmountReturned, "
                    + "ISNULL(R.fltRetailTaxAmountReturned, 0) AS fltRetailTaxAmountReturned "

                    + "FROM (SELECT I.dtmInvoiceDate FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN "
                    + "tbl_invoiceItem II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN "
                    + "@dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber = 1 GROUP BY I.dtmInvoiceDate UNION "
                    + "SELECT I.dtmInvoiceDate FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItemReturns II ON "
                    + "II.intInvoiceItemID = IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate "
                    + "AND I.intLocationID = @intLocationID AND IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber > 1 GROUP BY I.dtmInvoiceDate) D FULL JOIN (SELECT dtmInvoiceDate, "
                    
                    + "ROUND(ISNULL([GST], 0), 2) AS fltGovernmentTaxAmountCollected, "
                    + "ROUND(ISNULL([HST], 0), 2) AS fltHarmonizedTaxAmountCollected, "
                    + "ROUND(ISNULL([PST], 0), 2) AS fltProvincialTaxAmountCollected, "
                    + "ROUND(ISNULL([QST], 0), 2) AS fltQuebecTaxAmountCollected, "
                    + "ROUND(ISNULL([RST], 0), 2) AS fltRetailTaxAmountCollected, "
                    + "ROUND(ISNULL([LCT], 0), 2) AS fltLiquorTaxAmountCollected "

                    + "FROM (SELECT I.dtmInvoiceDate, TT.varTaxName, SUM(IIT.fltTaxAmount) "
                    + "AS fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
                    + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = "
                    + "@intLocationID AND IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber = 1 GROUP BY I.dtmInvoiceDate, TT.varTaxName) PS1 PIVOT(SUM(fltTaxAmount) FOR varTaxName "
                    + "IN([GST], [HST], [PST], [RST], [QST], [LCT])) AS PVT1) C ON C.dtmInvoiceDate = D.dtmInvoiceDate FULL JOIN (SELECT dtmInvoiceDate, "
                    
                    + "ROUND(ISNULL([GST], 0), 2) AS fltGovernmentTaxAmountReturned, "
                    + "ROUND(ISNULL([HST], 0), 2) AS fltHarmonizedTaxAmountReturned, "
                    + "ROUND(ISNULL([PST], 0), 2) AS fltProvincialTaxAmountReturned, "
                    + "ROUND(ISNULL([QST], 0), 2) AS fltQuebecTaxAmountReturned, "
                    + "ROUND(ISNULL([RST], 0), 2) AS fltRetailTaxAmountReturned, "
                    + "ROUND(ISNULL([LCT], 0), 2) AS fltLiquorTaxAmountReturned "
                    
                    + "FROM (SELECT I.dtmInvoiceDate, TT.varTaxName, SUM(IIT.fltTaxAmount) AS fltTaxAmount FROM "
                    + "tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItemReturns II ON II.intInvoiceItemID = IIT.intInvoiceItemID JOIN "
                    + "tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID AND "
                    + "IIT.bitIsTaxCharged = 1 AND I.intInvoiceSubNumber > 1 GROUP BY I.dtmInvoiceDate, TT.varTaxName) PS2 PIVOT(SUM(fltTaxAmount) FOR varTaxName IN([GST], [HST], "
                    + "[PST], [RST], [QST], [LCT])) AS PVT2) R ON R.dtmInvoiceDate = D.dtmInvoiceDate ORDER BY D.dtmInvoiceDate ASC";
                object[][] parms =
                {
                    new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                    new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                    new object[] { "@intLocationID", repInfo.intLocationID }
                };
                return ConvertTaxReportFromDataTable(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            }
        }

        private List<TaxReport> ConvertTaxReportFromDataTable(DataTable dt)
        {
            List<TaxReport> taxReport = dt.AsEnumerable().Select(row =>
            new TaxReport
            {
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                fltGovernmentTaxAmountCollected = row.Field<double>("fltGovernmentTaxAmountCollected"),
                fltHarmonizedTaxAmountCollected = row.Field<double>("fltHarmonizedTaxAmountCollected"),
                fltLiquorTaxAmountCollected = row.Field<double>("fltLiquorTaxAmountCollected"),
                fltProvincialTaxAmountCollected = row.Field<double>("fltProvincialTaxAmountCollected"),
                fltQuebecTaxAmountCollected = row.Field<double>("fltQuebecTaxAmountCollected"),
                fltRetailTaxAmountCollected = row.Field<double>("fltRetailTaxAmountCollected"),

                fltGovernmentTaxAmountReturned = row.Field<double>("fltGovernmentTaxAmountReturned"),
                fltHarmonizedTaxAmountReturned = row.Field<double>("fltHarmonizedTaxAmountReturned"),
                fltLiquorTaxAmountReturned = row.Field<double>("fltLiquorTaxAmountReturned"),
                fltProvincialTaxAmountReturned = row.Field<double>("fltProvincialTaxAmountReturned"),
                fltQuebecTaxAmountReturned = row.Field<double>("fltQuebecTaxAmountReturned"),
                fltRetailTaxAmountReturned = row.Field<double>("fltRetailTaxAmountReturned")
            }).ToList();
            return taxReport;
        }

        //^^^^^^^^^^^^^^^^^^^^DISCOUNT REPORT^^^^^^^^^^^^^^^^^^^^ (Working On)
        public int VerifyInvoicesCompleted(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!InvoicesCompleted(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool InvoicesCompleted(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "invoicesCompleted";
            bool bolData = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }
        public DataTable CallReturnDiscountsBetweenDates(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnDiscountsBetweenDates(repInfo, objPageDetails);
        }
        private DataTable ReturnDiscountsBetweenDates(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnDiscountsBetweenDates";
            //This method returns all invoices with discounts between two dates
            //string sqlCmd = "SELECT TD.intInvoiceID, I2.varInvoiceNumber, I2.intInvoiceSubNumber, I2.dtmInvoiceDate, (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers "
            //    + "WHERE intCustomerID = I2.intCustomerID) AS customerName, (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee WHERE intEmployeeID = I2.intEmployeeID) AS "
            //    + "employeeName, I2.fltTotalDiscount, I2.fltBalanceDue, TD.fltGovernmentTaxAmount, TD.fltProvincialTaxAmount, TD.fltLiquorTaxAmount FROM tbl_invoice I2 JOIN(SELECT "
            //    + "I.intInvoiceID, IIT.fltGovernmentTaxAmount, IIT.fltProvincialTaxAmount, IIT.fltLiquorTaxAmount FROM tbl_invoice I JOIN tbl_invoiceItem II ON II.intInvoiceID = "
            //    + "I.intInvoiceID JOIN(SELECT intInvoiceID, ROUND(ISNULL([GST], 0) + ISNULL([HST], 0), 2) AS fltGovernmentTaxAmount, ROUND(ISNULL([PST], 0) +ISNULL([RST], 0) +ISNULL("
            //    + "[QST], 0), 2) AS fltProvincialTaxAmount, ROUND(ISNULL([LCT], 0), 2) AS fltLiquorTaxAmount FROM(SELECT II.intInvoiceID, TT.varTaxName, SUM(IIT.fltTaxAmount) AS "
            //    + "fltTaxAmount FROM tbl_invoiceItemTaxes IIT JOIN tbl_taxType TT ON TT.intTaxID = IIT.intTaxTypeID JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
            //    + "IIT.intInvoiceItemID WHERE IIT.bitIsTaxCharged = 1 GROUP BY II.intInvoiceID, TT.varTaxName) PS1 PIVOT(SUM(fltTaxAmount) FOR varTaxName IN([GST], [HST], [PST], "
            //    + "[RST], [QST], [LCT])) AS PVT1) IIT ON IIT.intInvoiceID = II.intInvoiceID WHERE fltTotalDiscount <> 0 AND dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND "
            //    + "intLocationID = @intLocationID GROUP BY I.intInvoiceID, IIT.fltGovernmentTaxAmount, IIT.fltProvincialTaxAmount, IIT.fltLiquorTaxAmount) TD ON TD.intInvoiceID = "
            //    + "I2.intInvoiceID";
            string varLocationName = "";
            if(repInfo.intLocationID != 99)
            {
                varLocationName = " AND I.intLocationID = @intLocationID";
            }

            string sqlCmd = "dtmInvoiceDate, varLocationName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName, ROUND(SUM(fltTotalDiscount), "
                + "2) AS fltTotalDiscount, "
                + "ROUND(SUM(fltBalanceDue), 2) AS fltBalanceDue, "
                + "ROUND(SUM(fltGovernmentTaxAmount), 2) AS fltGovernmentTaxAmount, ROUND(SUM(fltHarmonnizedTaxAmount), 2) AS fltHarmonnizedTaxAmount, ROUND(SUM("
                + "fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND(SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, ROUND(SUM(fltQuebecTaxAmount), 2) "
                + "AS fltQuebecTaxAmount, ROUND(SUM(fltRetailTaxAmount), 2) AS fltRetailTaxAmount, ROUND(SUM(fltShippingTaxAmount), 2) AS fltShippingTaxAmount FROM "
                + "(SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, YEAR(dtmInvoiceDate) AS dtmInvoiceYear, DATENAME(MONTH, dtmInvoiceDate) AS varMonthName, "
                + "DATEADD(MONTH, DATEDIFF(MONTH, 0, dtmInvoiceDate), 0) AS dtmMonthDate, DATEADD(DAY, 1 - DATEPART(WEEKDAY, dtmInvoiceDate), CAST(dtmInvoiceDate AS DATE"
                + ")) dtmWeekStartDate, dtmInvoiceDate AS dtmSelectedDate, CASE WHEN varLocationName = 'Golf Traders' THEN varLocationName + ' - ' + varCityName ELSE "
                + "varLocationName END AS varLocationName, (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers WHERE intCustomerID = I.intCustomerID) AS "
                + "varCustomerName, (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee WHERE intEmployeeID = I.intEmployeeID) AS varEmployeeName, "
                + "fltTotalDiscount, fltBalanceDue, fltGovernmentTaxAmount, 0 AS fltHarmonnizedTaxAmount, fltLiquorTaxAmount, fltProvincialTaxAmount, 0 AS "
                + "fltQuebecTaxAmount, 0 AS fltRetailTaxAmount, 0 AS fltShippingTaxAmount FROM tbl_invoice I JOIN tbl_location L ON L.intLocationID = I.intLocationID "
                + "WHERE fltTotalDiscount<> 0 AND dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName + ") AS ABC ";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            string startQuery;
            string endQuery;

            if (repInfo.intGroupTimeFrame == 1)
            {
                startQuery = "SELECT FORMAT(dtmSelectedDate, 'dd/MM/yyyy') AS selection, ";
                endQuery = "GROUP BY dtmSelectedDate, dtmInvoiceDate, varLocationName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName "
                    + "ORDER BY dtmSelectedDate, dtmInvoiceDate, varLocationName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName";
            }
            else if (repInfo.intGroupTimeFrame == 2)
            {
                startQuery = "SELECT FORMAT(CAST(dtmWeekStartDate AS DATE), 'dd/MM/yyyy') AS selection, ";
                endQuery = "GROUP BY dtmWeekStartDate, dtmInvoiceDate, varLocationName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName "
                    + "ORDER BY dtmWeekStartDate, dtmInvoiceDate, varLocationName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName";
            }
            else
            {
                startQuery = "SELECT dtmMonthDate, CONCAT(varMonthName, ' / ', dtmInvoiceYear) AS selection, ";
                endQuery = "GROUP BY dtmMonthDate, dtmInvoiceDate, varLocationName, dtmInvoiceYear, varMonthName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName "
                    + "ORDER BY dtmMonthDate, dtmInvoiceDate, varLocationName, dtmInvoiceYear, varMonthName, varInvoiceNumber, intInvoiceSubNumber, varCustomerName, varEmployeeName";
            }

            sqlCmd = startQuery + sqlCmd + endQuery;
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //^^^^^^^^^^^^^^^^^^^^SALES BY DATE REPORT^^^^^^^^^^^^^^^^^^^^
        public DataTable CallReturnSalesForSelectedDate(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnSalesForSelectedDate(repInfo, objPageDetails);
        }
        private DataTable ReturnSalesForSelectedDate(ReportInformation repInfo, object[] objPageDetails)
        {
            DateTime cutOverDate = new DateTime(2022, 04, 01);

            if (DateTime.Today.Date > cutOverDate.Date)
            {
                string strQueryName = "returnSalesForSelectedDate";
                string varLocationName = "";
                if (repInfo.intLocationID != 99)
                {
                    varLocationName = " AND intLocationID = @intLocationID";
                }
                string sqlCmd = "SELECT dtmSalesDataDate AS dtmInvoiceDate, SUM(ROUND(fltGSTCollected, 2) + ROUND(fltGSTReturned, 2)) AS fltGovernmentTaxAmount, SUM(ROUND("
                    + "fltHSTCollected, 2) + ROUND(fltHSTReturned, 2)) AS fltHarmonizedTaxAmount, SUM(ROUND(fltLCTCollected, 2) + ROUND(fltLCTReturned, 2)) AS fltLiquorTaxAmount, "
                    + "SUM(ROUND(fltPSTCollected, 2) + ROUND(fltPSTReturned, 2)) AS fltProvincialTaxAmount, SUM(ROUND(fltQSTCollected, 2) + ROUND(fltQSTReturned, 2)) AS "
                    + "fltQuebecTaxAmount, SUM(ROUND(fltRSTCollected, 2) + ROUND(fltRSTReturned, 2)) AS fltRetailTaxAmount, SUM(ROUND(fltSalesDollars, 2)) AS fltSalesDollars, "
                    + "SUM(ROUND(fltSubTotal, 2)) AS fltSubTotal, SUM(ROUND(fltGSTCollected, 2) + ROUND(fltGSTReturned, 2) + ROUND(fltHSTCollected, 2) + ROUND(fltHSTReturned, "
                    + "2) + ROUND(fltLCTCollected, 2) + ROUND(fltLCTReturned, 2) + ROUND(fltPSTCollected, 2) + ROUND(fltPSTReturned, 2) + ROUND(fltQSTCollected, 2) + ROUND("
                    + "fltQSTReturned, 2) + ROUND(fltRSTCollected, 2) + ROUND(fltRSTReturned, 2) + ROUND(fltSubTotal, 2)) AS fltTotalSales FROM tbl_dailySalesData WHERE "
                    + "dtmSalesDataDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName + " GROUP BY dtmSalesDataDate ORDER BY dtmSalesDataDate";
                object[][] parms =
                {
                    new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                    new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                    new object[] { "@intLocationID", repInfo.intLocationID }
                };
                return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            }
            else
            {
                string strQueryName = "returnSalesForSelectedDate";
                //DateTime[] dtm = (DateTime[])repInfo[0];
                //string sqlCmd = "SELECT dtmInvoiceDate, ROUND(SUM(CASE WHEN bitChargeLCT = 1 THEN fltLiquorTaxAmount ELSE 0 END), 2) AS fltLiquorTaxAmount, ROUND(SUM(CASE WHEN "
                //    + "bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END), 2) AS fltProvincialTaxAmount, ROUND(SUM(CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount "
                //    + "ELSE 0 END), 2) AS fltGovernmentTaxAmount, ROUND(SUM(fltBalanceDue), 2) AS fltTotalSales FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND "
                //    + "@dtmEndDate AND intLocationID = @intLocationID GROUP BY dtmInvoiceDate";
                string sqlCmd = "SELECT dtmInvoiceDate, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, ROUND(SUM(fltGovernmentTaxAmount), 2) AS fltGovernmentTaxAmount, ROUND("
                    + "SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, ROUND(SUM(fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND(SUM(fltHarmonizedTaxAmount), "
                    + "2) AS fltHarmonizedTaxAmount, ROUND(SUM(fltQuebecTaxAmount), 2) AS fltQuebecTaxAmount, ROUND(SUM(fltRetailTaxAmount), 2) AS fltRetailTaxAmount, ROUND("
                    + "SUM(fltSalesDollars), 2) "
                    + "AS fltSalesDollars, ROUND((ROUND(SUM(fltSubTotal), 2) + ROUND(SUM(fltGovernmentTaxAmount), 2) + ROUND(SUM(fltProvincialTaxAmount), 2) + ROUND(SUM("
                    + "fltLiquorTaxAmount), 2) + ROUND(SUM(fltHarmonizedTaxAmount), 2) + ROUND(SUM(fltQuebecTaxAmount), 2) + ROUND(SUM(fltRetailTaxAmount), 2)), 2) AS "
                    + "fltTotalSales FROM(SELECT I.intInvoiceID, I.dtmInvoiceDate, ROUND(fltSubTotal, 2) AS 'fltSubTotal', CASE WHEN "
                    + "EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN "
                    + "0 ELSE (fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity "
                    + "END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE "
                    + "IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemRefund * intItemQuantity) END) FROM "
                    + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, CASE WHEN I.bitChargeGST = 1 THEN "
                    + "I.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS "
                    + "fltProvincialTaxAmount, CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, 0 AS fltHarmonizedTaxAmount, 0 AS "
                    + "fltQuebecTaxAmount, 0 AS fltRetailTaxAmount FROM tbl_invoice I WHERE "
                    + "I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.intLocationID = @intLocationID) tblTable GROUP BY dtmInvoiceDate ORDER BY dtmInvoiceDate";

                object[][] parms =
                {
                    new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                    new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                    new object[] { "@intLocationID", repInfo.intLocationID }
                };
                return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
                //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }

        //^^^^^^^^^^^^^^^^^^^^EXTENSIVE INVOICE REPORT^^^^^^^^^^^^^^^^^^^^
        public DataTable CallReturnExtensiveInvoices2(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnExtensiveInvoices2(repInfo, objPageDetails);
        }
        private DataTable ReturnExtensiveInvoices2(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnExtensiveInvoices";
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND I.intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT intInvoiceID, dtmInvoiceDate, varInvoiceNumber, fltShippingCharges, fltTotalTradeIn, fltTotalDiscount, fltSubTotal, fltGovernmentTaxAmount, "
                + "fltProvincialTaxAmount, fltLiquorTaxAmount, fltCostofGoods, fltSalesDollars, (fltSubTotal + fltGovernmentTaxAmount + fltProvincialTaxAmount + "
                + "fltLiquorTaxAmount) AS fltTotalSales, (fltSalesDollars - fltCostofGoods) AS fltRevenueEarned, varCustomerName, varEmployeeName FROM(SELECT I.intInvoiceID, "
                + "I.dtmInvoiceDate, CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', I.fltShippingCharges, (I.fltTotalTradeIn * -1) AS "
                + "'fltTotalTradeIn', CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(CASE WHEN "
                + "bitIsDiscountPercent = 1 THEN((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItem "
                + "II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) "
                + "THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 THEN((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity "
                + "END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltTotalDiscount', ROUND(fltSubTotal, 2) AS 'fltSubTotal', "
                + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 "
                + "THEN 0 ELSE (fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END"
                + ") FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE "
                + "IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemRefund * intItemQuantity) END) FROM "
                + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, CASE WHEN I.bitChargeGST = 1 THEN "
                + "I.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS "
                + "fltProvincialTaxAmount, CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, CASE WHEN EXISTS(SELECT II.intInvoiceID "
                + "FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItem II WHERE "
                + "II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN(SELECT "
                + "SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltCostofGoods', (SELECT CONCAT("
                + "varFirstName, ' ', varLastName) FROM tbl_customers C WHERE C.intCustomerID = I.intCustomerID) AS 'varCustomerName', (SELECT CONCAT(varFirstName, ' ', "
                + "varLastName) FROM tbl_employee E WHERE E.intEmployeeID = I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE I.dtmInvoiceDate BETWEEN "
                + "@dtmStartDate AND @dtmEndDate" + varLocationName + ") tblTable";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //^^^^^^^^^^^^^^^^^^^^COST OF INVENTORY REPORT^^^^^^^^^^^^^^^^^^^^
        public DataTable CallCostOfInventoryReport(ReportInformation repInfo, object[] objPageDetails)
        {
            return CostOfInventoryReport(repInfo, objPageDetails);
        }
        private DataTable CostOfInventoryReport(ReportInformation repInfo, object[] objPageDetails)
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
        }

        //^^^^^^^^^^^^^^^^^^^^STORE STATS REPORT^^^^^^^^^^^^^^^^^^^^ (Done)
        public int VerifyStatsAvailable(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!StatsAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool StatsAvailable(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "statsAvailable";
            bool bolTA = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }
        public DataTable CallReturnStoreStats(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnStoreStats(repInfo, objPageDetails);
        }
        private DataTable ReturnStoreStats(ReportInformation repInfo, object[] objPageDetails)
        {
            DateTime cutOverDate = new DateTime(2022, 04, 01);

            if (DateTime.Today.Date > cutOverDate.Date)
            {
                string strQueryName = "ReturnStoreStats";
                string varLocationName = "";
                if (repInfo.intLocationID != 99)
                {
                    varLocationName = " AND DSD.intLocationID = @intLocationID";
                }

                string sqlCmd = "varLocationName, ROUND(SUM(fltGSTCollected) + SUM(fltGSTReturned), 2) AS fltGovernmentTaxAmount, ROUND(SUM(fltHSTCollected) + "
                    + "SUM(fltHSTReturned), 2) AS fltHarmonizedTaxAmount, ROUND(SUM(fltLCTCollected) + SUM(fltLCTReturned), 2) AS fltLiquorTaxAmount, ROUND(SUM"
                    + "(fltPSTCollected) + SUM(fltPSTReturned), 2) AS fltProvincialTaxAmount, ROUND(SUM(fltQSTCollected) + SUM(fltQSTReturned), 2) AS "
                    + "fltQuebecTaxAmount, ROUND(SUM(fltRSTCollected) + SUM(fltRSTReturned), 2) AS fltRetailTaxAmount, ROUND(SUM(fltCostGoodsSold), 2) AS "
                    + "fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, CASE WHEN SUM(fltSalesDollars) = 0 THEN 0 ELSE ROUND((SUM(fltSalesDollars) - "
                    + "SUM(fltCostGoodsSold)) / SUM(fltSalesDollars), 4) END AS fltProfitMargin, ROUND(ROUND(SUM(fltGSTCollected) + SUM(fltGSTReturned), 2) + "
                    + "ROUND(SUM(fltHSTCollected) + SUM(fltHSTReturned), 2) + ROUND(SUM(fltLCTCollected) + SUM(fltLCTReturned), 2) + ROUND(SUM(fltPSTCollected) "
                    + "+ SUM(fltPSTReturned), 2) + ROUND(SUM(fltQSTCollected) + SUM(fltQSTReturned), 2) + ROUND(SUM(fltRSTCollected) + SUM(fltRSTReturned), 2) +"
                    + "ROUND(SUM(fltSalesDollars), 2), 2) AS fltTotalSales, ROUND(SUM(fltSalesDollars), 2) AS fltSalesDollars FROM (SELECT YEAR(dtmSalesDataDate) AS "
                    + "dtmInvoiceYear, DATENAME(MONTH, dtmSalesDataDate) AS varMonthName, DATEADD(MONTH, DATEDIFF(MONTH, 0, dtmSalesDataDate), 0) AS dtmMonthDate, "
                    + "DATEADD(DAY, 1 - DATEPART(WEEKDAY, dtmSalesDataDate), CAST(dtmSalesDataDate AS DATE)) dtmWeekStartDate, dtmSalesDataDate AS dtmSelectedDate, "
                    //+ "(SELECT CONCAT(L.varLocationName, '-', L.varCityName) FROM tbl_location L WHERE L.intLocationID = DSD.intLocationID) AS varLocationName, "
                    + "CASE WHEN varLocationName = 'Golf Traders' THEN varLocationName +' - ' + varCityName ELSE varLocationName END AS varLocationName, "
                    + "fltGSTCollected, fltGSTReturned, fltPSTCollected, fltPSTReturned, fltHSTCollected, fltHSTReturned, fltQSTCollected, fltQSTReturned, "
                    + "fltRSTCollected, fltRSTReturned, fltLCTCollected, fltLCTReturned, fltSalesDollars, fltSubTotal, fltCostGoodsSold FROM tbl_dailySalesData DSD "
                    + "JOIN tbl_location L ON L.intLocationID = DSD.intLocationID "
                    + "WHERE dtmSalesDataDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName + ") AS ABC ";

                string startQuery;
                string endQuery;

                object[][] parms =
                {
                    new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                    new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                    new object[] { "@intLocationID", repInfo.intLocationID }
                };
                if (repInfo.intGroupTimeFrame == 1)
                {
                    startQuery = "SELECT FORMAT(dtmSelectedDate, 'dd/MM/yyyy') AS selection, ";
                    endQuery = "GROUP BY dtmSelectedDate, varLocationName ORDER BY dtmSelectedDate, varLocationName";
                }
                else if (repInfo.intGroupTimeFrame == 2)
                {
                    startQuery = "SELECT FORMAT(CAST(dtmWeekStartDate AS DATE), 'dd/MM/yyyy') AS selection, ";
                    endQuery = "GROUP BY dtmWeekStartDate, varLocationName ORDER BY dtmWeekStartDate, varLocationName";
                }
                else
                {
                    startQuery = "SELECT dtmMonthDate, CONCAT(varMonthName, ' / ', dtmInvoiceYear) AS selection, ";
                    endQuery = "GROUP BY dtmMonthDate, varLocationName, dtmInvoiceYear, varMonthName ORDER BY dtmMonthDate, dtmInvoiceYear, varMonthName, varLocationName";
                }

                sqlCmd = startQuery + sqlCmd + endQuery;
                return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            }
            else
            {
                string strQueryName = "ReturnStoreStats";
                //string sqlCmd = "(SELECT I.intInvoiceID, YEAR(dtmInvoiceDate) AS dtmInvoiceYear, DATENAME(MONTH, dtmInvoiceDate) AS varMonthName, DATEADD(MONTH, DATEDIFF(MONTH, "
                //    + "0, dtmInvoiceDate), 0) AS dtmMonthDate, DATEADD(DAY, 1 - DATEPART(WEEKDAY, dtmInvoiceDate), CAST(dtmInvoiceDate AS DATE)) dtmWeekStartDate, dtmInvoiceDate "
                //    + "AS dtmSelectedDate, (SELECT CONCAT(L.varLocationName, '-', L.varCityName) FROM tbl_location L WHERE L.intLocationID = I.intLocationID) AS varLocationName, "
                //    + "CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', "

                //    + "I.fltShippingCharges, "
                //    + "(I.fltTotalTradeIn * -1) AS 'fltTotalTradeIn', "

                //    + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 "
                //    + "THEN ((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT "
                //    + "IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 THEN ("
                //    + "(fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS "
                //    + "'fltTotalDiscount', "

                //    + "ROUND(fltSubTotal, 2) AS 'fltSubTotal', "

                //    + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN "
                //    + "bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
                //    + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 "
                //    + "THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, "

                //    + "CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, "
                //    + "CASE WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, "
                //    + "CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, "

                //    + "CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM("
                //    + "fltItemCost * intItemQuantity) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM "
                //    + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE "
                //    + "IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltCostofGoods', "

                //    + "(SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_customers C WHERE "
                //    + "C.intCustomerID = I.intCustomerID) AS 'varCustomerName', (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee E WHERE E.intEmployeeID = "
                //    + "I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate) AS ABC ";

                //string sqlCmd = "(SELECT I.intInvoiceID, YEAR(dtmInvoiceDate) AS dtmInvoiceYear, DATENAME(MONTH, dtmInvoiceDate) AS varMonthName, DATEADD(MONTH, DATEDIFF(MONTH, "
                //    + "0, dtmInvoiceDate), 0) AS dtmMonthDate, DATEADD(DAY, 1 - DATEPART(WEEKDAY, dtmInvoiceDate), CAST(dtmInvoiceDate AS DATE)) dtmWeekStartDate, dtmInvoiceDate "
                //    + "AS dtmSelectedDate, (SELECT CONCAT(L.varLocationName, '-', L.varCityName) FROM tbl_location L WHERE L.intLocationID = I.intLocationID) AS varLocationName, "
                //    + "CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', I.fltShippingCharges, (I.fltTotalTradeIn * -1) AS 'fltTotalTradeIn', CASE "
                //    + "WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 "
                //    + "THEN((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID "
                //    + "= I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(CASE WHEN "
                //    + "bitIsDiscountPercent = 1 THEN((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM "
                //    + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltTotalDiscount', ROUND(fltSubTotal, 2) AS 'fltSubTotal', CASE WHEN "
                //    + "EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE "
                //    + "(fltItemPrice - CASE WHEN bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM "
                //    + "tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = "
                //    + "I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemRefund * intItemQuantity) END) FROM tbl_invoiceItemReturns IIR WHERE "
                //    + "IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END AS "
                //    + "fltGovernmentTaxAmount, CASE WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, CASE WHEN I.bitChargeLCT = 1 THEN "
                //    + "I.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = "
                //    + "I.intInvoiceID) THEN(SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT "
                //    + "IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN(SELECT SUM(fltItemCost * intItemQuantity) FROM "
                //    + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltCostofGoods', (SELECT CONCAT(varFirstName, ' ', varLastName) FROM "
                //    + "tbl_customers C WHERE C.intCustomerID = I.intCustomerID) AS 'varCustomerName', (SELECT CONCAT(varFirstName, ' ', varLastName) FROM tbl_employee E WHERE "
                //    + "E.intEmployeeID = I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate) AS ABC ";
                string location = "";

                if (repInfo.intLocationID != 99)
                {
                    location = " AND intLocationID = @intLocationID";
                }

                string sqlCmd = "(SELECT I.intInvoiceID, YEAR(dtmInvoiceDate) AS dtmInvoiceYear, DATENAME(MONTH, dtmInvoiceDate) AS varMonthName, DATEADD(MONTH, DATEDIFF("
                    + "MONTH, 0, dtmInvoiceDate), 0) AS dtmMonthDate, DATEADD(DAY, 1 - DATEPART(WEEKDAY, dtmInvoiceDate), CAST(dtmInvoiceDate AS DATE)) dtmWeekStartDate, "
                    + "dtmInvoiceDate AS dtmSelectedDate, (SELECT CONCAT(L.varLocationName, '-', L.varCityName) FROM tbl_location L WHERE L.intLocationID = "
                    + "I.intLocationID) AS varLocationName, CONCAT(I.varInvoiceNumber, '-', I.intInvoiceSubNumber) AS 'varInvoiceNumber', I.fltShippingCharges, ("
                    + "I.fltTotalTradeIn * -1) AS 'fltTotalTradeIn', CASE WHEN EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE II.intInvoiceID = "
                    + "I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 THEN ((fltItemPrice * (fltItemDiscount / 100)) * intItemQuantity) ELSE "
                    + "fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM "
                    + "tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsDiscountPercent = 1 THEN ((fltItemPrice * ("
                    + "fltItemDiscount / 100)) * intItemQuantity) ELSE fltItemDiscount * intItemQuantity END) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = "
                    + "I.intInvoiceID) ELSE 0 END AS 'fltTotalDiscount', ROUND(fltSubTotal, 2) AS 'fltSubTotal', CASE WHEN EXISTS(SELECT II.intInvoiceID FROM "
                    + "tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemPrice - CASE WHEN "
                    + "bitIsDiscountPercent = 1 THEN (fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemDiscount END) * intItemQuantity END) FROM tbl_invoiceItem II "
                    + "WHERE II.intInvoiceID = I.intInvoiceID) WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID"
                    + ") THEN (SELECT SUM(CASE WHEN bitIsClubTradeIn = 1 THEN 0 ELSE (fltItemRefund * intItemQuantity) END) FROM tbl_invoiceItemReturns IIR WHERE "
                    + "IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS fltSalesDollars, CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END AS "
                    + "fltGovernmentTaxAmount, 0 AS fltHarmonizedTaxAmount, CASE WHEN I.bitChargePST = 1 THEN I.fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, "
                    + "CASE WHEN I.bitChargeLCT = 1 THEN I.fltLiquorTaxAmount ELSE 0 END AS fltLiquorTaxAmount, 0 AS fltQuebecTaxAmount, 0 AS fltRetailTaxAmount, CASE WHEN "
                    + "EXISTS(SELECT II.intInvoiceID FROM tbl_invoiceItem II WHERE "
                    + "II.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * intItemQuantity) FROM tbl_invoiceItem II WHERE II.intInvoiceID = I.intInvoiceID) "
                    + "WHEN EXISTS(SELECT IIR.intInvoiceID FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) THEN (SELECT SUM(fltItemCost * "
                    + "intItemQuantity) FROM tbl_invoiceItemReturns IIR WHERE IIR.intInvoiceID = I.intInvoiceID) ELSE 0 END AS 'fltCostofGoods', (SELECT CONCAT("
                    + "varFirstName, ' ', varLastName) FROM tbl_customers C WHERE C.intCustomerID = I.intCustomerID) AS 'varCustomerName', (SELECT CONCAT(varFirstName, "
                    + "' ', varLastName) FROM tbl_employee E WHERE E.intEmployeeID = I.intEmployeeID) AS 'varEmployeeName' FROM tbl_invoice I WHERE I.dtmInvoiceDate "
                    + "BETWEEN @dtmStartDate AND @dtmEndDate" + location + ") AS ABC ";

                string startQuery;
                string endQuery;

                object[][] parms =
                {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() }, 
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
                if (repInfo.intGroupTimeFrame == 1)
                {
                    startQuery = "SELECT FORMAT(dtmSelectedDate, 'dd/MM/yyyy') AS selection, varLocationName, ROUND(SUM(fltGovernmentTaxAmount), 2) AS fltGovernmentTaxAmount, "
                        + "ROUND(SUM(fltHarmonizedTaxAmount), 2) AS fltHarmonizedTaxAmount, ROUND(SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, ROUND(SUM("
                        + "fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND(SUM(fltQuebecTaxAmount), 2) AS fltQuebecTaxAmount, ROUND(SUM(fltRetailTaxAmount), 2) AS "
                        + "fltRetailTaxAmount, ROUND(SUM(fltCostofGoods), "
                        + "2) AS fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, CASE WHEN SUM(fltSalesDollars) = 0 THEN 0 ELSE ROUND((SUM(fltSalesDollars) - SUM("
                        + "fltCostofGoods)) / SUM(fltSalesDollars), 4) END AS fltProfitMargin, ROUND(ROUND(SUM(fltGovernmentTaxAmount), 2) + ROUND(SUM(fltProvincialTaxAmount), "
                        + "2) + ROUND(SUM(fltLiquorTaxAmount), 2) + ROUND(SUM(fltSalesDollars), 2), 2) AS fltTotalSales, ROUND(SUM(fltSalesDollars), 2) AS fltSalesDollars FROM ";
                    endQuery = "GROUP BY dtmSelectedDate, varLocationName ORDER BY dtmSelectedDate, varLocationName";
                }
                else if (repInfo.intGroupTimeFrame == 2)
                {
                    startQuery = "SELECT FORMAT(CAST(dtmWeekStartDate AS DATE), 'dd/MM/yyyy') AS selection, varLocationName, ROUND(SUM(fltGovernmentTaxAmount), 2) AS "
                        + "fltGovernmentTaxAmount, ROUND(SUM(fltHarmonizedTaxAmount), 2) AS fltHarmonizedTaxAmount, ROUND(SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, "
                        + "ROUND(SUM(fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND(SUM(fltQuebecTaxAmount), 2) AS fltQuebecTaxAmount, ROUND(SUM(fltRetailTaxAmount), 2) AS "
                        + "fltRetailTaxAmount, "
                        + "ROUND(SUM(fltCostofGoods), 2) AS fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, CASE WHEN SUM(fltSalesDollars) = 0 THEN 0 ELSE ROUND(("
                        + "SUM(fltSalesDollars) - SUM(fltCostofGoods)) / SUM(fltSalesDollars), 4) END AS fltProfitMargin, ROUND(ROUND(SUM(fltGovernmentTaxAmount), 2) + ROUND("
                        + "SUM(fltProvincialTaxAmount), 2) + ROUND(SUM(fltLiquorTaxAmount), 2) + ROUND(SUM(fltSalesDollars), 2), 2) AS fltTotalSales, ROUND(SUM(fltSalesDollars"
                        + "), 2) AS fltSalesDollars FROM ";
                    endQuery = "GROUP BY dtmWeekStartDate, varLocationName ORDER BY dtmWeekStartDate, varLocationName";
                }
                else
                {
                    startQuery = "SELECT dtmMonthDate, CONCAT(varMonthName, ' / ', dtmInvoiceYear) AS selection, varLocationName, ROUND(SUM(fltGovernmentTaxAmount), 2) AS "
                        + "fltGovernmentTaxAmount, ROUND(SUM(fltHarmonizedTaxAmount), 2) AS fltHarmonizedTaxAmount, ROUND(SUM(fltProvincialTaxAmount), 2) AS fltProvincialTaxAmount, "
                        + "ROUND(SUM(fltLiquorTaxAmount), 2) AS fltLiquorTaxAmount, ROUND(SUM(fltQuebecTaxAmount), 2) AS fltQuebecTaxAmount, ROUND(SUM(fltRetailTaxAmount), 2) AS "
                        + "fltRetailTaxAmount, "
                        + "ROUND(SUM(fltCostofGoods), 2) AS fltCostofGoods, ROUND(SUM(fltSubTotal), 2) AS fltSubTotal, CASE WHEN SUM(fltSalesDollars) = 0 THEN 0 ELSE ROUND(("
                        + "SUM(fltSalesDollars) - SUM(fltCostofGoods)) / SUM(fltSalesDollars), 4) END AS fltProfitMargin, ROUND(ROUND(SUM(fltGovernmentTaxAmount), 2) + "
                        + "ROUND(SUM(fltProvincialTaxAmount), 2) + ROUND(SUM(fltLiquorTaxAmount), 2) + ROUND(SUM(fltSalesDollars), 2), 2) AS fltTotalSales, ROUND(SUM("
                        + "fltSalesDollars), 2) AS fltSalesDollars FROM ";
                    endQuery = "GROUP BY dtmMonthDate, varLocationName, dtmInvoiceYear, varMonthName ORDER BY dtmMonthDate, dtmInvoiceYear, varMonthName, varLocationName";
                }

                sqlCmd = startQuery + sqlCmd + endQuery;
                return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);

            }
        }

        //^^^^^^^^^^^^^^^^^^^^INVENTORY CHANGE REPORT^^^^^^^^^^^^^^^^^^^^
        public int VerifyInventoryChange(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!InventoryChangeAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool InventoryChangeAvailable(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "inventoryChangeAvailable";
            bool bolData = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT COUNT(dtmChangeDate) changeCount FROM tbl_itemChangeTracking WHERE dtmChangeDate BETWEEN "
                + "@dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }
        public DataTable CallReturnChangedInventoryForDateRange(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnChangedInventoryForDateRange(repInfo, objPageDetails);
        }
        private DataTable ReturnChangedInventoryForDateRange(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnChangedInventoryForDateRange";
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND ICT.intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT ICT.dtmChangeDate, ICT.dtmChangeTime, CONCAT(E.varFirstName, ', ', E.varLastName) AS employeeName, L.varCityName, "
                + "ICT.intInventoryID, CASE WHEN EXISTS(SELECT A.intInventoryID FROM tbl_accessories A WHERE A.intInventoryID = ICT.intInventoryID) THEN "
                + "(SELECT A.varSku FROM tbl_accessories A WHERE A.intInventoryID = ICT.intInventoryID) WHEN EXISTS(SELECT CL.intInventoryID FROM "
                + "tbl_clothing CL WHERE CL.intInventoryID = ICT.intInventoryID) THEN (SELECT CL.varSku FROM tbl_clothing CL WHERE CL.intInventoryID = "
                + "ICT.intInventoryID) WHEN EXISTS(SELECT C.intInventoryID FROM tbl_clubs C WHERE C.intInventoryID = ICT.intInventoryID) THEN (SELECT "
                + "C.varSku FROM tbl_clubs C WHERE C.intInventoryID = ICT.intInventoryID) END AS varSku, ICT.fltOriginalCost, ICT.fltNewCost, "
                + "ICT.fltOriginalPrice, ICT.fltNewPrice, ICT.intOriginalQuantity, ICT.intNewQuantity, ICT.varOriginalDescription, ICT.varNewDescription "
                + "FROM tbl_itemChangeTracking ICT JOIN tbl_employee E ON E.intEmployeeID = ICT.intEmployeeID JOIN tbl_location L ON L.intLocationID = "
                + "ICT.intLocationID WHERE ICT.dtmChangeDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //^^^^^^^^^^^^^^^^^^^^SPECIFIC APPAREL REPORT^^^^^^^^^^^^^^^^^^^^
        public int VerifySpecificApparel(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!SpecificApparelAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool SpecificApparelAvailable(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "specificApparelAvailable";
            bool bolData = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND I.intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT SUM(II.intItemQuantity) overallQuantity FROM tbl_invoiceItem II JOIN tbl_invoice I ON "
                + "I.intInvoiceID = II.intInvoiceID JOIN tbl_specificApparel SA ON SA.intInventoryID = II.intInventoryID "
                + "WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }
        public DataTable CallReturnSpecificApparelDataTableForReport(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnSpecificApparelDataTableForReport(repInfo, objPageDetails);
        }
        private DataTable ReturnSpecificApparelDataTableForReport(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnSpecificApparelDataTableForReport";
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND O.intLocationID = @intLocationID";
            }
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
                + "= R.intInventoryID WHERE O.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName + " GROUP BY R.intInvoiceID, "
                + "O.intLocationID, S.intInventoryID, R.fltItemCost, R.fltItemPrice) IIR ON IIR.intInvoiceID = II.intInvoiceID JOIN tbl_invoice I "
                + "ON I.intInvoiceID = II.intInvoiceID JOIN tbl_specificApparel SA ON SA.intInventoryID = II.intInventoryID WHERE I.dtmInvoiceDate "
                + "BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY SA.intInventoryID, I.intLocationID, II.varItemDescription, II.fltItemCost, "
                + "II.fltItemPrice, II.bitIsDiscountPercent, II.fltItemDiscount) FQ JOIN tbl_location L ON L.intLocationID = FQ.intLocationID "
                + "GROUP BY FQ.intInventoryID, L.varLocationName, FQ.varItemDescription";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //^^^^^^^^^^^^^^^^^^^^SPECIFIC GRIP REPORT^^^^^^^^^^^^^^^^^^^^
        public int VerifySpecificGrip(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!SpecificGripAvailable(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool SpecificGripAvailable(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "specificGripAvailable";
            bool bolData = false;
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND I.intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT SUM(II.intItemQuantity) overallQuantity FROM tbl_invoiceItem II JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID JOIN "
                + "tbl_specificGrip SG ON SG.intInventoryID = II.intInventoryID WHERE I.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolData = true;
            }
            return bolData;
        }
        public DataTable CallReturnSpecificGripDataTableForReport(ReportInformation repInfo, object[] objPageDetails)
        {
            return ReturnSpecificGripDataTableForReport(repInfo, objPageDetails);
        }
        private DataTable ReturnSpecificGripDataTableForReport(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "returnSpecificGripDataTableForReport";
            string varLocationName = "";
            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND O.intLocationID = @intLocationID";
            }
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
                + "R.intInventoryID WHERE O.dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName + " GROUP BY R.intInvoiceID, "
                + "O.intLocationID, S.intInventoryID, R.fltItemCost, R.fltItemPrice) IIR ON IIR.intInvoiceID = II.intInvoiceID JOIN tbl_invoice I "
                + "ON I.intInvoiceID = II.intInvoiceID JOIN tbl_specificGrip SG ON SG.intInventoryID = II.intInventoryID WHERE I.dtmInvoiceDate "
                + "BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY SG.intInventoryID, I.intLocationID, II.varItemDescription, II.fltItemCost, "
                + "II.fltItemPrice, II.bitIsDiscountPercent, II.fltItemDiscount) FQ JOIN tbl_location L ON L.intLocationID = FQ.intLocationID "
                + "GROUP BY FQ.intInventoryID, L.varLocationName, FQ.varItemDescription";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //^^^^^^^^^^^^^^^^^^^^SALES BY DATE, EXTENSIVE INVOICE^^^^^^^^^^^^^^^^^^^^
        public int VerifySalesHaveBeenMade(ReportInformation repInfo, object[] objPageDetails)
        {
            int indicator = 0;
            if (!TransactionsAvailableOverMultipleDates(repInfo, objPageDetails))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool TransactionsAvailableOverMultipleDates(ReportInformation repInfo, object[] objPageDetails)
        {
            string strQueryName = "transactionsAvailableOverMultipleDates";
            bool bolTA = false;
            string varLocationName = "";

            if (repInfo.intLocationID != 99)
            {
                varLocationName = " AND intLocationID = @intLocationID";
            }
            string sqlCmd = "SELECT COUNT(intInvoiceID) FROM tbl_invoice WHERE dtmInvoiceDate BETWEEN @dtmStartDate AND @dtmEndDate" + varLocationName;
            object[][] parms =
            {
                new object[] { "@dtmStartDate", repInfo.dtmStartDate.ToShortDateString() },
                new object[] { "@dtmEndDate", repInfo.dtmEndDate.ToShortDateString() },
                new object[] { "@intLocationID", repInfo.intLocationID }
            };

            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }

        //^^^^^^^^^^^^^^^^^^^^HOME PAGE SALES^^^^^^^^^^^^^^^^^^^^
        public DataTable CallGetInvoiceBySaleDate(ReportInformation repInfo, object[] objPageDetails)
        {
            return GetInvoiceBySaleDate(repInfo, objPageDetails);
        }
        private DataTable GetInvoiceBySaleDate(ReportInformation repInfo, object[] objPageDetails)
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
                 new object[] { "@startDate", repInfo.dtmStartDate.ToShortDateString() },
                 new object[] { "@endDate", repInfo.dtmEndDate.ToShortDateString() },
                 new object[] { "@intLocationID", repInfo.intLocationID }
            };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
    }
}