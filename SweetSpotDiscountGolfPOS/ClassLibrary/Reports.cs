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
using System.Runtime.InteropServices;
using SweetShop;
using SweetSpotProShop;
using System.Threading;
using System.Diagnostics;
using System.Text;
using OfficeOpenXml;
using System.Windows.Forms;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class Reports
    {
        DatabaseCalls dbc = new DatabaseCalls();
        //List<Cashout> cashout = new List<Cashout>();
        //List<Cashout> remainingCashout = new List<Cashout>();
        //LocationManager lm = new LocationManager();
        //private System.Data.DataTable exportTable;
        //private System.Data.DataTable exportInvoiceTable;
        //private System.Data.DataTable exportInvoiceItemTable;
        //private System.Data.DataTable exportInvoiceMOPTable;

        //Connection String
        public Reports()
        {
            //connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        //Report Tracking
        public void CallReportLogger(object[] reportLog)
        {
            logReportCall(reportLog);
        }
        private void logReportCall(object[] reportLog)
        {
            string sqlCmd = "INSERT INTO tbl_reportView VALUES(@reportID, "
                + "@dateClicked, @timeClicked, @employeeID, @locationID)";
            object[][] parms =
            {
                new object[] { "@reportID", Convert.ToInt32(reportLog[0]) },
                new object[] { "@dateClicked", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@timeClicked", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@employeeID", Convert.ToInt32(reportLog[1]) },
                new object[] { "@locationID", Convert.ToInt32(reportLog[2]) }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //*******************HOME PAGE SALES*******************************************************
        //Nathan built for home page sales display
        public System.Data.DataTable getInvoiceBySaleDate(DateTime startDate, DateTime endDate, int locationID)
        {
            //Gets a list of all invoices based on date and location. Stores in a list
            string sqlCmd = "SELECT tbl_invoice.invoiceNum, tbl_invoice.invoiceSubNum, custID, empID, subTotal, discountAmount, "
                + "tradeinAmount, CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "(balanceDue + CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END + CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END) AS balanceDue, "
                + "mopType, amountPaid FROM tbl_invoice INNER JOIN tbl_invoiceMOP ON tbl_invoice.invoiceNum = tbl_invoiceMOP.invoiceNum "
                + "AND tbl_invoiceMOP.invoiceSubNum = tbl_invoice.invoiceSubNum WHERE invoiceDate BETWEEN @startDate AND @endDate AND locationID = @locationID";

            object[][] parms =
            {
                 new object[] { "@startDate", startDate },
                 new object[] { "@endDate", endDate },
                 new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //******************COST OF INVENTORY REPORTING*******************************************************
        public System.Data.DataTable costOfInventoryReport()
        {
            string sqlCmd = "SELECT TOP (1) (SELECT SUM(cost * quantity) FROM tbl_clubs "
                + "WHERE locationID = 1) AS cMJ, (SELECT SUM(cost * quantity) FROM "
                + "tbl_clubs WHERE locationID = 2) AS cCAL, (SELECT SUM(cost * quantity) "
                + "FROM tbl_clubs WHERE locationID = 8) AS cEDM, (SELECT SUM(cost * "
                + "quantity) FROM tbl_accessories WHERE locationID = 1) AS aMJ, (SELECT "
                + "SUM(cost * quantity) FROM tbl_accessories WHERE locationID = 2) AS aCAL, "
                + "(SELECT SUM(cost * quantity) FROM tbl_accessories WHERE locationID = 8) "
                + "AS aEDM, (SELECT SUM(cost * quantity) FROM tbl_clothing WHERE locationID "
                + "= 1) AS clMJ, (SELECT SUM(cost * quantity) FROM tbl_clothing WHERE "
                + "locationID = 2) AS clCAL, (SELECT SUM(cost * quantity) FROM tbl_clothing "
                + "WHERE locationID = 8) AS clEDM FROM tbl_clubs";

            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //*******************CASHOUT UTILITIES*******************************************************
        //Matches new Database Calls
        public int verifyCashoutCanBeProcessed(int location, DateTime dtm)
        {
            int indicator = 0;
            if (transactionsAvailable(location, dtm))
            {
                if (openTransactions(location,dtm))
                {
                    indicator = 2;
                }
                else if(cashoutAlreadyDone(location, dtm))
                {
                    indicator = 3;
                }
            }
            else { indicator = 1; }
            return indicator;
        }
        public bool transactionsAvailable(int location, DateTime dtm)
        {
            bool bolTA = false;
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice "
                        + "WHERE invoiceDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm.ToString("yyyy-MM-dd") },
                new object[] { "@endDate", dtm.ToString("yyyy-MM-dd") },
                new object[] { "@locationID", location }
            };

            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }
        public bool cashoutAlreadyDone(int location, DateTime dtm)
        {
            bool bolCAD = false;
            string sqlCmd = "SELECT COUNT(cashoutDate) from tbl_cashout "
                        + "WHERE cashoutDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm.ToString("yyyy-MM-dd") },
                new object[] { "@endDate", dtm.ToString("yyyy-MM-dd") },
                new object[] { "@locationID", location }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolCAD = true;
            }
            return bolCAD;
        }
        private bool openTransactions(int location, DateTime dtm)
        {
            bool bolOT = false;
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_currentSalesInvoice "
                        + "WHERE transactionType = 1 AND locationID = @locationID "
                        + "AND invoiceDate = @invoiceDate";
            object[][] parms =
            {
                new object[] { "@locationID", location },
                new object[] { "@invoiceDate", dtm.ToString("yyyy-MM-dd")}
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolOT = true;
            }
            return bolOT;
        }
        public void removeUnprocessedReturns(int location, DateTime dtm)
        {
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE invoiceDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm.ToString("yyyy-MM-dd") },
                new object[] { "@endDate", dtm.ToString("yyyy-MM-dd") },
                new object[] { "@locationID", location }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //This method connects to the database and gets the totals for the MOPs based on location and dates
        public List<Mops> SumOfMOPSBasedOnLocationAndDateRange(object[] repInfo)
        {
            MopsManager MM = new MopsManager();
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT IM.mopType, SUM(IM.amountPaid) AS amountPaid "
                            + "FROM tbl_invoiceMOP IM INNER JOIN tbl_invoice I "
                            + "ON IM.invoiceNum = I.invoiceNum AND IM.invoiceSubNum "
                            + "= I.invoiceSubNum WHERE I.invoiceDate BETWEEN @startDate "
                            + "AND @endDate AND I.locationID = @locationID "
                            + "GROUP BY IM.mopType";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            //Returns the list of Mops types and totals
            return MM.ReturnMopsFromCmdAndParams(sqlCmd, parms);
        }
        public double SumOfTradeInsBasedOnLocationAndDateRange(object[] repInfo)
        {
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT SUM(tradeinAmount) AS amountPaid "
                + "FROM tbl_invoice WHERE invoiceDate BETWEEN @startDate "
                + "AND @endDate AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };

            //Returns the list of Mops types and totals
            return dbc.MakeDataBaseCallToReturnDouble(sqlCmd, parms);
        }

        public Cashout CreateNewCashout(DateTime startDate, int locationID)
        {
            ////Now need to account for anything in Layaway status
            ////These queries will need to be updated
            //Return PivotTable of a list of Mops
            System.Data.DataTable dt1 = ReturnListOfMOPS(startDate, locationID);

            //Gather remaining Totals (taxes, subtotal(including shipping), tradein)
            System.Data.DataTable dt2 = ReturnAdditionTotalsForCashout(startDate, locationID);

            //Save all into a cashout and return
            Cashout C = new Cashout();
            C.cashoutDate = DateTime.Parse(dt1.Rows[0][0].ToString());
            C.saleTradeIn = Convert.ToDouble(dt2.Rows[0][0].ToString());
            C.saleGiftCard = Convert.ToDouble(dt1.Rows[0][3].ToString());
            C.saleCash = Convert.ToDouble(dt1.Rows[0][1].ToString());
            C.saleDebit = Convert.ToDouble(dt1.Rows[0][2].ToString());
            C.saleMasterCard = Convert.ToDouble(dt1.Rows[0][4].ToString());
            C.saleVisa = Convert.ToDouble(dt1.Rows[0][5].ToString());
            C.preTax = Convert.ToDouble(dt2.Rows[0][1].ToString());
            C.saleGST = Convert.ToDouble(dt2.Rows[0][2].ToString());
            C.salePST = Convert.ToDouble(dt2.Rows[0][3].ToString());

            return C;
        }
        private System.Data.DataTable ReturnListOfMOPS(DateTime startDate, int locationID)
        {
            string sqlCmd = "SELECT invoiceDate, ISNULL([Cash],0) AS Cash, ISNULL([Debit],0) AS Debit, "
                + "ISNULL([Gift Card],0) AS GiftCard, ISNULL([MasterCard],0) AS Mastercard, ISNULL([Visa],0) AS Visa "
                + "FROM(SELECT i.invoiceDate, m.mopType, SUM(amountPaid) AS totalPaid "
                + "FROM tbl_invoiceMOP m join tbl_invoice i ON m.invoiceNum = i.invoiceNum AND m.invoiceSubNum = i.invoiceSubNum "
                + "WHERE i.invoiceDate = @startDate AND i.locationID = @locationID "
                + "GROUP BY i.invoiceDate, m.mopType) ps "
                + "PIVOT(SUM(totalPaid) FOR mopType IN([Cash], [Debit], [Gift Card], [MasterCard], [Visa])) AS pvt";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        private System.Data.DataTable ReturnAdditionTotalsForCashout(DateTime startDate, int locationID)
        {
            string sqlCmd = "SELECT SUM(tradeinAmount) AS tradeinTotal, "
                + "SUM(subTotal) + SUM(shippingAmount) AS subTotal, "
                + "SUM(CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END) AS gTax, "
                + "SUM(CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END) AS pTax "
                + "FROM tbl_invoice WHERE invoiceDate = @startDate AND locationID = @locationID";

            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        
        //Insert the cashout into the database
        public void insertCashout(Cashout cas)
        {
            string sqlCmd = "INSERT INTO tbl_cashout VALUES(@cashoutDate, @cashoutTime, "
                + "@saleTradeIn, @saleGiftCard, @saleCash, @saleDebit, @saleMasterCard, "
                + "@saleVisa, @receiptTradeIn, @receiptGiftCard, @receiptCash, "
                + "@receiptDebit, @receiptMasterCard, @receiptVisa, @preTax, @gTax, "
                + "@pTax, @overShort, @finalized, @processed, @locID, @empID)";
            object[][] parms =
            {
                new object[] { "@cashoutDate", cas.cashoutDate },
                new object[] { "@cashoutTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@saleTradeIn", cas.saleTradeIn },
                new object[] { "@saleGiftCard", cas.saleGiftCard },
                new object[] { "@saleCash", cas.saleCash },
                new object[] { "@saleDebit", cas.saleDebit },
                new object[] { "@saleMasterCard", cas.saleMasterCard },
                new object[] { "@saleVisa", cas.saleVisa },
                new object[] { "@receiptTradeIn", cas.receiptTradeIn },
                new object[] { "@receiptGiftCard", cas.receiptGiftCard },
                new object[] { "@receiptCash", cas.receiptCash },
                new object[] { "@receiptDebit", cas.receiptDebit },
                new object[] { "@receiptMasterCard", cas.receiptMasterCard },
                new object[] { "@receiptVisa", cas.receiptVisa },
                new object[] { "@preTax", cas.preTax },
                new object[] { "@gTax", cas.saleGST },
                new object[] { "@pTax", cas.salePST },
                new object[] { "@overShort", cas.overShort },
                new object[] { "@finalized", cas.finalized },
                new object[] { "@processed", cas.processed },
                new object[] { "@locID", cas.locationID },
                new object[] { "@empID", cas.empID }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //******************PURCHASES REPORTING*******************************************************
        //Matches new Database Calls
        public int verifyPurchasesMade(object[] repInfo)
        {
            int indicator = 0;
            if (!purchasesAvailable(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        public bool purchasesAvailable(object[] repInfo)
        {
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

            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolTA = true;
            }

            return bolTA;
        }

        public bool tradeinsHaveBeenProcessed(object[] repInfo)
        {
            bool bolTI = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice "
                        + "WHERE invoiceDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID AND tradeInAmount < 0";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolTI = true;
            }
            return bolTI;
        }

        // V2.7.4-TestingAndFixing

        public List<Purchases> returnPurchasesDuringDates(DateTime startDate, DateTime endDate, int locationID)
        {
            string sqlCmd = "SELECT R.receiptNumber, R.receiptDate, D.methodDesc, M.chequeNum, "
                + "M.amountPaid FROM tbl_receipt R INNER JOIN tbl_receiptMOP M ON R.receiptNumber "
                + "= M.receiptNum INNER JOIN tbl_methodOfPayment D ON M.mopType = D.methodID WHERE "
                + "R.receiptDate BETWEEN @startDate AND @endDate AND R.locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            
            return returnPurchasesFromDataTable(dbc.returnDataTableData(sqlCmd, parms));
        }
        private List<Purchases> returnPurchasesFromDataTable(System.Data.DataTable dt)
        {
            List<Purchases> purchase = dt.AsEnumerable().Select(row =>
            new Purchases
            {
                receiptNumber = row.Field<int>("receiptNumber"),
                receiptDate = row.Field<DateTime>("receiptDate"),
                mopDescription = row.Field<string>("methodDesc"),
                chequeNumber = row.Field<int>("chequeNum"),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return purchase;
        }
        //******************MARKETING REPORTING*******************************************************
        public System.Data.DataTable mostSoldItemsReport(DateTime startDate, DateTime endDate, int locationID)
        {
            string sqlCmd = "SELECT TOP (10) II.sku, SUM(II.quantity) AS amountSold FROM tbl_invoiceItem II "
                + "JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum AND I.invoiceSubNum = II.invoiceSubNum "
                + "WHERE II.sku NOT IN(SELECT sku FROM tbl_tempTradeInCartSkus) AND "
                + "I.invoiceDate BETWEEN @startDate AND @endDate AND I.locationID = @locationID "
                + "GROUP BY sku ORDER BY amountSold DESC";

            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public System.Data.DataTable mostSoldBrandsReport(DateTime startDate, DateTime endDate, int locationID)
        {
            string sqlCmd = "SELECT TOP(10) BB.brand, SUM(II.quantity) AS amountSold FROM "
                + "tbl_invoiceItem II JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum "
                + "AND I.invoiceSubNum = II.invoiceSubNum JOIN(SELECT A.sku, (SELECT "
                + "B.brandName AS brand FROM tbl_accessories AC JOIN tbl_brand B ON "
                + "AC.brandID = B.brandID WHERE AC.sku = A.sku) AS brand FROM "
                + "tbl_accessories A UNION SELECT CL.sku, (SELECT B.brandName AS brand "
                + "FROM tbl_clothing CLO JOIN tbl_brand B ON CLO.brandID = B.brandID WHERE "
                + "CLO.sku = CL.sku) AS brand FROM tbl_clothing CL UNION SELECT C.sku, "
                + "(SELECT B.brandName AS brand FROM tbl_clubs CLU JOIN tbl_brand B ON "
                + "CLU.brandID = B.brandID WHERE CLU.sku = C.sku) AS brand FROM tbl_clubs C) "
                + "BB ON BB.sku = II.sku WHERE I.invoiceDate BETWEEN @dtmStartDate AND "
                + "@dtmEndDate AND I.locationID = @locationID AND II.isTradeIn = 0 GROUP BY "
                + "brand ORDER BY amountSold DESC";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public System.Data.DataTable mostSoldModelsReport(DateTime startDate, DateTime endDate, int locationID)
        {
            string sqlCmd = "SELECT TOP (10) MM.model, SUM(II.quantity) AS amountSold FROM "
                + "tbl_invoiceItem II JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum AND "
                + "I.invoiceSubNum = II.invoiceSubNum JOIN(SELECT A.sku, (SELECT M.modelName "
                + "AS model FROM tbl_accessories AC JOIN tbl_model M ON AC.modelID = M.modelID "
                + "WHERE AC.sku = A.sku) AS model FROM tbl_accessories A UNION SELECT C.sku, "
                + "(SELECT M.modelName AS model FROM tbl_clubs CLU JOIN tbl_model M ON "
                + "CLU.modelID = M.modelID WHERE CLU.sku = C.sku) AS model FROM tbl_clubs C) "
                + "MM ON MM.sku = II.sku WHERE I.invoiceDate BETWEEN @dtmStartDate AND "
                + "@dtmEndDate AND I.locationID = @locationID  AND II.isTradeIn = 0 GROUP BY "
                + "model ORDER BY amountSold DESC";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", startDate },
                new object[] { "@dtmEndDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }


        //******************COGS and PM REPORTING*******************************************************
        //Clean SQL command so it's not here and in the correct DBC
        public System.Data.DataTable returnInvoicesForCOGS(DateTime startDate, DateTime endDate, int locationID)
        {
            //This method returns a type of invoice for a report
            string sqlCmd = "SELECT CONCAT(I.invoiceNum, '-', I.invoiceSubNum) AS 'invoice', II.totalPrice, II.totalCost, II.totalDiscount, II.percentage, "
                + "CASE WHEN II.percentage = 1 AND II.totalPrice <> 0 THEN CAST(ROUND((((II.totalPrice - (II.totalPrice * II.totalDiscount) / 100) - II.totalCost) / II.totalPrice) * 100, 2) AS varchar) "
                + "WHEN II.percentage = 0 AND II.totalPrice <> 0 THEN CAST(ROUND(((II.totalPrice - II.totalDiscount - II.totalCost) / II.totalPrice) * 100, 2) AS varchar) "
                + "WHEN II.totalPrice = 0 THEN 'N/A' ELSE 'N/A' END AS 'totalProfit' FROM tbl_invoice I "
                + "JOIN(SELECT invoiceNum, invoiceSubNum, sku, SUM(price * quantity) AS totalPrice, SUM(cost * quantity) AS totalCost, "
                + "CASE WHEN percentage = 1 THEN SUM(itemDiscount) ELSE SUM(itemDiscount * quantity) END AS totalDiscount, percentage FROM tbl_invoiceItem GROUP BY invoiceNum, invoiceSubNum, sku, percentage) AS II "
                + "ON II.invoiceNum = I.invoiceNum AND II.invoiceSubNum = I.invoiceSubNum WHERE II.sku NOT IN(SELECT sku FROM tbl_tempTradeInCartSkus) AND "
                + "II.invoiceNum NOT IN(SELECT invoiceNum FROM tbl_invoiceItemReturns) AND I.locationID = @locationID AND I.invoiceDate BETWEEN @startDate AND @endDate "
                + "ORDER BY I.invoiceNum, I.invoiceSubNum";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public int verifyInvoicesCompleted(object[] repInfo)
        {
            int indicator = 0;
            if (!invoicesCompleted(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool invoicesCompleted(object[] repInfo)
        {
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice WHERE locationID "
                + "= @locationID AND (invoiceDate BETWEEN @dtmStartDate AND @dtmEndDate)";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolData = true;
            }
            return bolData;
        }

        //******************Sales by Date Report*******************************************************
        //Matches new Database Calls
        public int verifySalesHaveBeenMade(object[] repInfo)
        {
            int indicator = 0;
            if (!transactionsAvailableOverMultipleDates(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        public bool transactionsAvailableOverMultipleDates(object[] repInfo)
        {
            bool bolTA = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice "
                        + "WHERE invoiceDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }
        public System.Data.DataTable returnSalesForSelectedDate(object[] repInfo)
        {
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT invoiceDate, SUM(subTotal) AS totalSales FROM "
                + "tbl_invoice WHERE invoiceDate BETWEEN @startDate AND @endDate "
                + "AND locationID = @locationID GROUP BY invoiceDate";
            object[][] parms = 
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //******************Trade Ins by Date Report*******************************************************
        public int verifyTradeInsHaveBeenMade(object[] repInfo)
        {
            int indicator = 0;
            if (!tradeinsHaveBeenProcessed(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        public System.Data.DataTable returnTradeInsForSelectedDate(object[] repInfo)
        {
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT invoiceDate, SUM(tradeInAmount) AS totalTradeIns "
                + "FROM tbl_invoice WHERE invoiceDate BETWEEN @startDate AND @endDate "
                + "AND locationID = @locationID GROUP BY invoiceDate";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //******************Sales by Payment Type By Date Report***************************************
        public System.Data.DataTable returnSalesByPaymentTypeForSelectedDate(object[] repInfo)
        {
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT invoiceDate, ISNULL([Cash],0) AS Cash, "
                + "ISNULL([Debit],0) AS Debit, ISNULL([Gift Card],0) AS GiftCard, "
                + "ISNULL([MasterCard],0) AS Mastercard, ISNULL([Visa],0) AS "
                + "Visa FROM(SELECT i.invoiceDate, m.mopType, SUM(amountPaid) AS "
                + "totalPaid FROM tbl_invoiceMOP m JOIN tbl_invoice i ON "
                + "m.invoiceNum = i.invoiceNum AND m.invoiceSubNum = i.invoiceSubNum "
                + "WHERE i.invoiceDate BETWEEN @startDate AND @endDate AND "
                + "i.locationID = @locationID GROUP BY i.invoiceDate, m.mopType) ps "
                + "PIVOT(SUM(totalPaid) FOR mopType IN([Cash], [Debit], [Gift Card], [MasterCard], [Visa])) AS pvt";
            object[][] parms = 
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //********************IMPORTING***************************************************************
        //This method is the giant import method
        public void importItems(FileUpload fup)
        {
            ItemDataUtilities idu = new ItemDataUtilities();
            Clubs c = new Clubs();
            Accessories a = new Accessories();
            Clothing cl = new Clothing();
            object o = new object();
            //List of clubs
            List<Clubs> listClub = new List<Clubs>();
            //List of clothing
            List<Clothing> listClothing = new List<Clothing>();
            //List of accessories
            List<Accessories> listAccessories = new List<Accessories>();
            //^ I don't think these are actually used anymore
            Form fc = (Form)System.Windows.Forms.Application.OpenForms["SettingsHomePage"];

            //check if there is actually a file being uploaded
            if (fup.HasFile)
            {
                //load the uploaded file into the memorystream
                using (MemoryStream stream = new MemoryStream(fup.FileBytes))
                //Lets the server know to use the excel package
                using (ExcelPackage xlPackage = new ExcelPackage(stream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                    //Gets the row count
                    var rowCnt = worksheet.Dimension.End.Row;
                    //Gets the column count
                    var colCnt = worksheet.Dimension.End.Column;
                    //Beginning the loop for data gathering
                    for (int i = 2; i < rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                    {
                        SettingsHomePage page = new SettingsHomePage();
                        page.counter = i;
                        page.total = rowCnt;
                        page.progress = i.ToString() + "/" + rowCnt.ToString();
                        page.callJS();
                        //fc..Label1.Text = 
                        //      page.lblP.Text = i.ToString() + "/" + rowCnt.ToString();
                        System.Windows.Forms.Application.DoEvents();

                        string itemType;
                        //Attempts to get the item type
                        try
                        {
                            itemType = (worksheet.Cells[i, 5].Value).ToString(); //Column 5 = itemType
                        }
                        catch (Exception ex)
                        {
                            itemType = "";
                        }

                        //If the row is not null, and there is a value in column 5, proceed
                        if (worksheet.Row(i) != null && worksheet.Cells[i, 5].Value != null)
                        {
                            //Does nothing if itemType is null
                            if (itemType == null) { }
                            //Does nothing if itemType is blank
                            else if (itemType.Equals("")) { }
                            //***************ACCESSORIES*********
                            //If the itemType is accessories or Accessories, the item is an accessory
                            else if (itemType.Equals("Accessories") || itemType.Equals("accessories"))
                            {
                                //***************SKU***************
                                //If there is a sku in column 3, proceed
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null)) //Column 3 = Sku
                                {
                                    //Sets the accessory sku to the value in column 3
                                    a.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    //Should NEVER happen, but is used as a safety/catch 
                                    a.sku = 0;
                                }
                                //***************BRAND ID***************
                                //Gets the brand name from the itemType. 
                                //If it an accessory, its brand will always be accessory
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    //Will equal the brandID of accessory
                                    a.brandID = bName;
                                }
                                else
                                {
                                    //Should NEVER happen, but is included as a safety/catch
                                    a.brandID = 1;
                                }
                                //***************MODEL ID***************                                
                                try
                                {
                                    string mName;
                                    mName = (worksheet.Cells[i, 6].Value).ToString(); //Column 6 = modelName
                                                                                      //If the model name is null, set the ID to 1
                                    if (mName == null)
                                    {
                                        //Shouldn't happen. Should come out as ""
                                        a.modelID = 1;
                                    }
                                    else
                                    {
                                        //Gets the modelID from the value in column 6
                                        int mID = idu.modelName(mName);
                                        //Check if it is null
                                        if (!mID.Equals(null))
                                            //Hardcoded because the DB refused to allow 360 as a model name
                                            if (mName == "360") { a.modelID = 17; }
                                            //Setting the model ID to what is returned from idu.modelName
                                            else { a.modelID = mID; }
                                        else
                                            //Should NEVER happen, but is included as a safety/catch
                                            a.modelID = 1;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //1427: N/A
                                    a.modelID = 1427;
                                }
                                //***************ACCESSORY TYPE***************
                                try
                                {
                                    //Checks to see if column 7 has a value
                                    if ((string)(worksheet.Cells[i, 7].Value) != null) //Column 7 = accessoryType
                                    {
                                        //Setting the accessory's accessory type to the value in column 7
                                        a.accessoryType = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        //Won't happen very often but can be triggered by a blank cell. In that case, this sets it to be blank
                                        a.accessoryType = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Sets the accessory type to be blank if an error occurs.
                                    a.accessoryType = "";
                                }
                                //***************COST***************
                                try
                                {
                                    //Checks if column 12 has a value
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null)) //Column 12 = cost
                                    {
                                        //Sets the accessory's cost to the value in column 12
                                        a.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        //Sometimes no cost is given so I set it to 0 in the DB
                                        a.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Sets the cost to 0 if an error is thrown
                                    a.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    //Checks if column 15 has a value
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null)) //Column 15 = price
                                    {
                                        //Sets the accessory's price to the value in column 15
                                        a.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        //Sometimes a price is not given so it gets set to 0 in the DB
                                        a.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Sometimes a price is not given and caught so it gets set to 0 in the DB
                                    a.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    //Checks to see if there is a value in column 13
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null)) //Column 13 = quantity
                                    {
                                        //Sets the accessory's quantity to the value in column 13
                                        a.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        //If the quantity is not given or is blank, setting it to 0
                                        a.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, setting the quantity to 0
                                    a.quantity = 0;
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    //Checking if there is a value in column 16
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null)) //Column 16 = comments
                                    {
                                        //Setting the accessory's comments to the value in column 16
                                        a.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        //When comments are not present, they are set to "" in the DB
                                        a.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Whenan error occurs, sets the comments to ""
                                    a.comments = "";
                                }
                                //***************LOCATIONID***************
                                try
                                {
                                    //NEEDS TO BE REWORKED
                                    if (!(worksheet.Cells[i, 22].Value).Equals(null)) //22
                                    {
                                        string destination = (worksheet.Cells[i, 22].Value).ToString();
                                        //a.locID = lm.getLocationIDFromDestination(destination);
                                    }
                                    else
                                    {
                                        a.locID = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    a.locID = 1;
                                }


                                a.typeID = 2;  //Accessory type ID = 2
                                a.size = "";   //NOT BEING USED
                                a.colour = ""; //NOT BEING USED

                                //Adds the accessory to the list of type accessory
                                listAccessories.Add(a);
                                o = a as object;
                            }
                            //***************APPAREL*************
                            //Apparel is used instead of clothing due to the clients previous DB
                            else if (itemType.Equals("Apparel") || itemType.Equals("apparel"))
                            {
                                //***************SKU***************
                                //Checks to see if column 3 has a value
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null)) //Column 3 = sku
                                {
                                    //Sets the clothing sku to the value in column 3
                                    cl.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    //Should NEVER happen, but is a good catch
                                    cl.sku = 0;
                                }
                                //***************BRAND ID***************   
                                //Gets the brand name from the itemType. 
                                //If it clothing, its brand will always be clothing
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    //Will equal the brandID of accessory
                                    cl.brandID = bName;
                                }
                                else
                                {
                                    //Something really broke if this happens
                                    cl.brandID = 1;
                                }
                                //***************COST***************
                                try
                                {
                                    //Checks to see if there is a value in column 12
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null)) //Column 12 = cost
                                    {
                                        //Sets the clothing cost to the value in column 12
                                        cl.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        //Sometime cost is not give so I set it to 0
                                        cl.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, setting the value to 0
                                    cl.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    //Checks to see if there is a value in column 15
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null)) //Column 15 = price
                                    {
                                        //Sets the clothing price to the value in column 15
                                        cl.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        //Sometimes a price is not given so I set it to 0
                                        cl.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, the price is set to 0
                                    cl.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    //Checks to see if there is a value in column 13
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null)) //Column 13 = quantity
                                    {
                                        //Sets the clothing quantity to the value in column 13
                                        cl.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        //Sometimes a quantity is not given so I set it to 0
                                        cl.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, the quantity is set to 0
                                    cl.quantity = 0;
                                }
                                //***************GENDER***************
                                try
                                {
                                    //Checks to see if there isa value in column 6
                                    if (!(worksheet.Cells[i, 6].Value).Equals(null)) //Column 6 = gender
                                    {
                                        //Sets the clothing gender to the value in column 6
                                        cl.gender = (string)(worksheet.Cells[i, 6].Value);
                                    }
                                    else
                                    {
                                        //Sometimes the gender is not given so I set it to ""
                                        cl.gender = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, the gender is set to ""
                                    cl.gender = "";
                                }
                                //***************STYLE***************
                                try
                                {
                                    //Checks to see if there is a value in column 7
                                    if (!(worksheet.Cells[i, 7].Value).Equals(null)) //Column 7 = style
                                    {
                                        //Set the clothing style to the value in column 7
                                        cl.style = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        //Sometimes the style is not given so I set it to ""
                                        cl.style = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, set the style to ""
                                    cl.style = "";
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    //Checks to see if there is a value in column 16
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null)) //Column 16 = comments
                                    {
                                        //Sets the clothing comments to the value in column 16
                                        cl.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        //Sometimes the comments are not given so I set it to ""
                                        cl.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, set the comments to ""
                                    cl.comments = "";
                                }
                                //***************LOCATIONID***************
                                try
                                {
                                    //NEEDS TO BE REWORKED
                                    if (!(worksheet.Cells[i, 22].Value).Equals(null)) //22
                                    {
                                        string destination = (worksheet.Cells[i, 22].Value).ToString();
                                        //cl.locID = lm.getLocationIDFromDestination(destination);
                                    }
                                    else
                                    {
                                        cl.locID = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.locID = 1;
                                }

                                cl.typeID = 3;  //The type ID for clothing is always 3
                                cl.size = "";   //Not used
                                cl.colour = ""; //Not used
                                                //Adds the clothing to the list of type clothing
                                listClothing.Add(cl);
                                o = cl as object;
                            }
                            //***************CLUBS***************
                            else
                            {
                                //***************SKU***************
                                //Checks to see if column 3 has a value
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null)) //Column 3 = Sku
                                {
                                    //Sets the club sku to the value in column 3
                                    c.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    //Should NEVER happen but is used as a catch
                                    c.sku = 0;
                                }
                                //***************BRAND ID***************
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    c.brandID = bName; //Brand ID will be a type of club
                                }
                                else
                                {
                                    c.brandID = 1;
                                }
                                //***************MODEL ID***************                                
                                try
                                {
                                    string mName;
                                    mName = (worksheet.Cells[i, 6].Value).ToString(); //Column 6 = model name
                                    if (mName == null)
                                    {
                                        c.modelID = 1;
                                    }
                                    else
                                    {
                                        int mID = idu.modelName(mName);
                                        if (!mID.Equals(null))
                                            //Database doesn't like 360 so hardcoded in 
                                            if (mName == "360") { c.modelID = 17; }
                                            else { c.modelID = mID; }

                                        else
                                            c.modelID = 1;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //1427 = N/A
                                    c.modelID = 1427;
                                }
                                //***************COST***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null)) //Column 12 = cost
                                    {
                                        c.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null)) //Column 15 = price
                                    {
                                        c.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null)) //Column 13 = quantity
                                    {
                                        c.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.quantity = 0;
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null)) //Column 16 = comments
                                    {
                                        c.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        //Sometime not given
                                        c.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.comments = "";
                                }
                                //***************PREMIUM***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 11].Value).Equals(null)) //Column 11 = premium
                                    {
                                        c.premium = Convert.ToDouble(worksheet.Cells[i, 11].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.premium = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.premium = 0;
                                }
                                //***************CLUB TYPE***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 7].Value).Equals(null)) //Column 7 = clubType
                                    {
                                        c.clubType = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.clubType = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.clubType = "";
                                }
                                //***************SHAFT***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 8].Value).Equals(null)) //Column 8 = shaft
                                    {
                                        c.shaft = (string)(worksheet.Cells[i, 8].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.shaft = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaft = "";
                                }
                                //***************NUMBER OF CLUBS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 9].Value).Equals(null)) //Column 9 = numberOfClubs
                                    {
                                        c.numberOfClubs = (string)(worksheet.Cells[i, 9].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.numberOfClubs = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.numberOfClubs = "";
                                }
                                //***************CLUB SPEC***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 18].Value).Equals(null)) //Column 18 = clubSpec
                                    {
                                        c.clubSpec = (string)(worksheet.Cells[i, 18].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.clubSpec = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.clubSpec = "";
                                }
                                //***************SHAFT SPEC***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 19].Value).Equals(null)) //Column 19 = shaftSpec
                                    {
                                        c.shaftSpec = (string)(worksheet.Cells[i, 19].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.shaftSpec = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaftSpec = "";
                                }
                                //***************SHAFT FLEX***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 20].Value).Equals(null)) //Column 20 = shaftFlex
                                    {
                                        c.shaftFlex = (string)(worksheet.Cells[i, 20].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.shaftFlex = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaftFlex = "";
                                }
                                //***************DEXTERITY***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 21].Value).Equals(null)) //Column 21 = dexterity
                                    {
                                        c.dexterity = (string)(worksheet.Cells[i, 21].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.dexterity = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.dexterity = "";
                                }
                                //***************LOCATIONID***************
                                try
                                {
                                    //NEEDS TO BE REWORKED
                                    if (!(worksheet.Cells[i, 22].Value).Equals(null)) //22
                                    {
                                        string destination = (worksheet.Cells[i, 22].Value).ToString();
                                        //c.itemlocation = lm.getLocationIDFromDestination(destination);
                                    }
                                    else
                                    {
                                        c.itemlocation = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.itemlocation = 1;
                                }



                                c.typeID = 1;    //The type ID of a club is always 1
                                c.isTradeIn = false;  //Not used
                                                      //Adds the club to the list of type club
                                listClub.Add(c);
                                o = c as object;
                            }
                            //Looks for the item in the database
                            SweetShopManager ssm = new SweetShopManager();
                            ssm.checkForItem(o);
                        }
                    }
                }
            }
        }
        //This method was meant to import the previous customers, but is filled with errors and is not being used
        public void importCustomers(FileUpload fup)
        {
            Excel.Application xlApp = new Excel.Application();
            //string path = fup.PostedFile.FileName;
            //System.Web.HttpContext.Current.Server.MapPath(fup.FileName)
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = Path.Combine(pathUser, "Downloads\\");
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path + fup.FileName);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            Customer cu = new Customer();

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 2; i <= rowCount; i++)
            {
                string itemType = (string)((xlRange.Cells[i, 5] as Range).Value2);

                //Write the value to the console, and start gathering item info for insert
                if (xlRange.Cells[i] != null && xlRange.Cells[i].Value2 != null)
                {
                    //tbl_customers: custID, firstName, lastName, primaryAddress, secondaryAddress, primaryPhoneINT, secondaryPhoneINT
                    //billingAddress, email, city, provStateID, country, postZip                    
                    //First Name
                    if ((xlWorksheet.Cells[i, 2] as Range).Value2 != null)
                        cu.firstName = (xlWorksheet.Cells[i, 2] as Range).Value2;
                    else
                        cu.firstName = "";
                    //Last Name
                    if ((xlWorksheet.Cells[i, 3] as Range).Value2 != null)
                        cu.lastName = (xlWorksheet.Cells[i, 3] as Range).Value2;
                    else
                        cu.lastName = "";
                    //primaryAddress
                    if ((xlWorksheet.Cells[i, 5] as Range).Value2 != null)
                        cu.primaryAddress = (xlWorksheet.Cells[i, 5] as Range).Value2;
                    else
                        cu.primaryAddress = "";
                    //primaryPhoneINT
                    if ((xlWorksheet.Cells[i, 9] as Range).Value2 != null)
                        cu.primaryPhoneNumber = (xlWorksheet.Cells[i, 9] as Range).Value2;
                    else
                        cu.primaryPhoneNumber = "";
                    //secondaryPhoneINT
                    if ((xlWorksheet.Cells[i, 10] as Range).Value2 != null)
                        cu.secondaryPhoneNumber = (xlWorksheet.Cells[i, 10] as Range).Value2;
                    else
                        cu.secondaryPhoneNumber = "";
                    //email
                    if ((xlWorksheet.Cells[i, 11] as Range).Value2 != null)
                        cu.email = (xlWorksheet.Cells[i, 11] as Range).Value2;
                    else
                        cu.email = "";
                    //city
                    if ((xlWorksheet.Cells[i, 6] as Range).Value2 != null)
                        cu.city = (xlWorksheet.Cells[i, 6] as Range).Value2;
                    else
                        cu.city = "";
                    //provStateID
                    if ((xlWorksheet.Cells[i, 7] as Range).Value2 != null)
                    {
                        string provinceName = (xlWorksheet.Cells[i, 7] as Range).Value2;
                        //cu.province = lm.pronvinceID(provinceName);
                    }
                    else
                        cu.province = 1;
                    //country                    
                    //cu.country = lm.countryIDFromProvince(cu.province);
                    //postZip
                    if ((xlWorksheet.Cells[i, 8] as Range).Value2 != null)
                        cu.postalCode = (xlWorksheet.Cells[i, 8] as Range).Value2;
                    else
                        cu.postalCode = "";

                    cu.secondaryAddress = "";
                    //cu.billingAddress = "";
                }
                //ssm.addCustomer(cu);
            }
        }
        //This method is an updated import item method that runs cleaner and should produce less errors
        public System.Data.DataTable uploadItems(FileUpload fup)
        {

            //***************************************************************************************************
            //Step 1: Create datatable to hold the items found in the excel sheet
            //***************************************************************************************************
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            //Datatable to hold any skus that have errors
            System.Data.DataTable skusWithErrors = new System.Data.DataTable();
            skusWithErrors.Columns.Add("sku");
            skusWithErrors.Columns.Add("brandError");
            skusWithErrors.Columns.Add("modelError");
            skusWithErrors.Columns.Add("identifierError");
            //This datatable can hold all items
            System.Data.DataTable listItems = new System.Data.DataTable();
            listItems.Columns.Add("sku");
            listItems.Columns.Add("brandName");
            listItems.Columns.Add("modelName");
            listItems.Columns.Add("cost");
            listItems.Columns.Add("price");
            listItems.Columns.Add("quantity");
            listItems.Columns.Add("comments");
            listItems.Columns.Add("premium");
            listItems.Columns.Add("clubType");
            listItems.Columns.Add("shaft");
            listItems.Columns.Add("numberOfClubs");
            listItems.Columns.Add("clubSpec");
            listItems.Columns.Add("shaftSpec");
            listItems.Columns.Add("shaftFlex");
            listItems.Columns.Add("dexterity");
            listItems.Columns.Add("locationName");
            listItems.Columns.Add("itemType");
            listItems.Columns.Add("size");
            listItems.Columns.Add("colour");

            //Database connections
            SqlConnection con = new SqlConnection(connectionString);
            SqlConnection conTempDB = new SqlConnection(connectionString);
            SqlConnection conInsert = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "IF OBJECT_ID('tempItemStorage', 'U') IS NOT NULL " +
                                  "DROP TABLE tempItemStorage; " +
                              "IF OBJECT_ID('tempErrorSkus', 'U') IS NOT NULL " +
                                  "DROP TABLE tempErrorSkus;";
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
                    con = new SqlConnection(connectionString);
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
                        string itemType = (worksheet.Cells[i, 5].Value).ToString(); //Column 5 = itemType
                        //Adding items to the datatables 
                        //***************************************************************************************************
                        //Step 4: Option A: The item type is Apparel
                        //***************************************************************************************************
                        if (itemType.Equals("Apparel"))
                        {
                            listItems.Rows.Add(
                                //***************SKU***************
                                Convert.ToInt32(worksheet.Cells[i, 3].Value.ToNullSafeString()),
                                //***************BRAND NAME***************
                                itemType.ToString(),
                                //***************MODEL Name***************        
                                (string)(worksheet.Cells[i, 6].Value.ToNullSafeString()), //gender for clothing
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
                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()), //style for clothing
                                                                                          //***************SHAFT***************
                                "",
                                //***************NUMBER OF CLUBS***************
                                "",
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
                                3,
                                //***************SIZE*************** 
                                "",
                                //***************COLOUR*************** 
                                ""
                            );
                        }
                        //***************************************************************************************************
                        //Step 4: Option B: The item type is Accessories
                        //***************************************************************************************************
                        else if (itemType.Equals("Accessories"))
                        {
                            listItems.Rows.Add(
                                //***************SKU***************
                                Convert.ToInt32(worksheet.Cells[i, 3].Value.ToNullSafeString()),
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
                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()), //accessoryType
                                                                                          //***************SHAFT***************
                                "",
                                //***************NUMBER OF CLUBS***************
                                "",
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
                                //***************SIZE*************** 
                                "",
                                //***************COLOUR*************** 
                                ""
                            );
                        }
                        //***************************************************************************************************
                        //Step 4: Option C: The item type is blank
                        //***************************************************************************************************
                        else if (itemType.Equals("")) { }
                        //***************************************************************************************************
                        //Step 4: Option D: The item type is a club
                        //***************************************************************************************************
                        else
                        {
                            listItems.Rows.Add(
                            //***************SKU***************
                            Convert.ToInt32(worksheet.Cells[i, 3].Value.ToNullSafeString()),
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
                                //***************SIZE*************** 
                                "",
                                //***************COLOUR*************** 
                                ""
                            );
                        }
                    }

                    //***************************************************************************************************
                    //Step 5: Create the temp tables for storing the items and skus that cause an error
                    //***************************************************************************************************

                    //Creating the temp tables  
                    conTempDB.Open();
                    cmd.CommandText = "create table tempItemStorage( " +
                                                "sku int primary key, " +
                                                "brandID int, " +
                                                "modelID int, " +
                                                "clubType varchar(150), " +
                                                "shaft varchar(150), " +
                                                "numberOfClubs varchar(150), " +
                                                "premium float, " +
                                                "cost float, " +
                                                "price float, " +
                                                "quantity int, " +
                                                "clubSpec varchar(150), " +
                                                "shaftSpec varchar(150), " +
                                                "shaftFlex varchar(150), " +
                                                "dexterity varchar(150), " +
                                                "typeID int, " +
                                                "locationID int, " +
                                                "comments varchar(500)); " +
                                        "create table tempErrorSkus(" +
                                                "sku int primary key," +
                                                "brandError int," +
                                                "modelError int," +
                                                "identifierError int)";
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
                        cmd.CommandText = "if((select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) >= 0 and " +
                                            "(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) >= 0 and " +
                                            "(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier) >= 0) " +
                                            "Begin " +
                                                "insert into tempItemStorage values( " +
                                                    "@sku, " +
                                                    "(select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName), " +
                                                    "(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName), " +
                                                    "@clubType, @shaft, @numberOfClubs, @premium, @cost, @price, @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, " +
                                                    "(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier), @comments) " +
                                            "end " +
                                        "else if(Not Exists(select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) and " +
                                                "(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) >= 0 and " +
                                                "(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier) >= 0) " +
                                            "Begin " +
                                                "insert into tempErrorSkus values(@sku, 1, 0, 0) " +
                                            "end " +
                                        "else if ((select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) >= 0 and " +
                                                 "Not Exists(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) and " +
                                                 "(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier) >= 0) " +
                                            "Begin " +
                                                    "insert into tempErrorSkus values(@sku, 0, 1, 0) " +
                                            "end " +
                                        "else if ((select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) >= 0 and " +
                                                 "(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) >= 0 and " +
                                                 "Not Exists(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier)) " +
                                            "Begin " +
                                                "insert into tempErrorSkus values(@sku, 0, 0, 1) " +
                                            "end " +
                                        "else if (Not Exists(select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) and " +
                                                 "Not Exists(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) and " +
                                                 "(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier) >= 0) " +
                                            "Begin " +
                                                "insert into tempErrorSkus values(@sku, 1, 1, 0) " +
                                            "end " +
                                        "else if (Not Exists(select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) and " +
                                                 "(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) >= 0 and " +
                                                 "Not Exists(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier)) " +
                                            "Begin " +
                                                "insert into tempErrorSkus values(@sku, 1, 0, 1) " +
                                            "end " +
                                        "else if ((select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName) >= 0 and " +
                                                 "Not Exists(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName) and " +
                                                 "Not Exists(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier)) " +
                                            "Begin " +
                                                "insert into tempErrorSkus values(@sku, 0, 1, 1) " +
                                            "end " +
                                        "else if (Not Exists(select top 1 tbl_brand.brandID from tbl_brand where tbl_brand.brandName = @brandName)and " +
                                                 "Not Exists(select top 1 tbl_model.modelID from tbl_model where tbl_model.modelName = @modelName)and " +
                                                 "Not Exists(select top 1 tbl_location.locationID from tbl_location where tbl_location.secondaryIdentifier = @secondaryIdentifier)) " +
                                            "Begin " +
                                                "insert into tempErrorSkus values(@sku, 1, 1, 1) " +
                                            "end";
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@sku", row[0]);
                        cmd.Parameters.AddWithValue("@brandName", row[1]);
                        cmd.Parameters.AddWithValue("@modelName", row[2]);
                        cmd.Parameters.AddWithValue("@cost", row[3]);
                        cmd.Parameters.AddWithValue("@price", row[4]);
                        cmd.Parameters.AddWithValue("@quantity", row[5]);
                        cmd.Parameters.AddWithValue("@comments", row[6]);
                        cmd.Parameters.AddWithValue("@premium", row[7]);
                        cmd.Parameters.AddWithValue("@clubType", row[8]);
                        cmd.Parameters.AddWithValue("@shaft", row[9]);
                        cmd.Parameters.AddWithValue("@numberOfClubs", row[10]);
                        cmd.Parameters.AddWithValue("@clubSpec", row[11]);
                        cmd.Parameters.AddWithValue("@shaftSpec", row[12]);
                        cmd.Parameters.AddWithValue("@shaftFlex", row[13]);
                        cmd.Parameters.AddWithValue("@dexterity", row[14]);
                        cmd.Parameters.AddWithValue("@secondaryIdentifier", row[15]);
                        cmd.Parameters.AddWithValue("@typeID", row[16]);
                        reader = cmd.ExecuteReader();
                        con.Close();
                        cmd = new SqlCommand();
                    };

                    //***************************************************************************************************
                    //Step 7: Check the error list for any data
                    //***************************************************************************************************

                    //Reading the error list
                    using (cmd = new SqlCommand("select * from tempErrorSkus", con)) //Calling the SP
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
                    cmd.CommandText = "Select * from tempItemStorage";
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


                    foreach (DataRow row in temp.Rows)
                    {
                        //loop through just one, and it will know the itemID because we set it ealier in the process                            

                        //Set club parameters here
                        cmd.Parameters.Clear();//Clearing the parameters. It was giving me an error(ID=1500)
                        cmd.Parameters.AddWithValue("sku", row[0]);
                        cmd.Parameters.AddWithValue("brandID", row[1]);
                        cmd.Parameters.AddWithValue("modelID", row[2]);
                        cmd.Parameters.AddWithValue("clubType", row[3]);
                        cmd.Parameters.AddWithValue("shaft", row[4]);
                        cmd.Parameters.AddWithValue("numberOfClubs", row[5]);
                        cmd.Parameters.AddWithValue("premium", row[6]);
                        cmd.Parameters.AddWithValue("cost", row[7]);
                        cmd.Parameters.AddWithValue("price", row[8]);
                        cmd.Parameters.AddWithValue("quantity", row[9]);
                        cmd.Parameters.AddWithValue("clubSpec", row[10]);
                        cmd.Parameters.AddWithValue("shaftSpec", row[11]);
                        cmd.Parameters.AddWithValue("shaftFlex", row[12]);
                        cmd.Parameters.AddWithValue("dexterity", row[13]);
                        cmd.Parameters.AddWithValue("typeID", row[14]);
                        cmd.Parameters.AddWithValue("locationID", row[15]);
                        cmd.Parameters.AddWithValue("used", 0);
                        cmd.Parameters.AddWithValue("comments", row[16]);
                        cmd.Parameters.AddWithValue("size", "");
                        cmd.Parameters.AddWithValue("colour", "");

                        conInsert.Open();
                        cmd.Connection = conInsert;
                        //This query/insert statement will first look at the typeID of the item being sent in. 
                        //It then looks to see if the items sku is in the table already. If it is, it updates. 
                        //If it is not, it inserts the item into the table
                        cmd.CommandText =
                            "if(@typeID = 1) " +
                                "begin " +
                                    "if exists(select sku from tbl_clubs where sku = @sku) " +
                                        "begin " +
                                            "UPDATE tbl_clubs SET brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft, " +
                                            "numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity, " +
                                            "clubSpec = @clubSpec, shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity, " +
                                            "locationID = @locationID, isTradeIn = @used, comments = @comments WHERE sku = @sku " +
                                        "end " +
                                    "else " +
                                        "begin " +
                                            "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs, " +
                                            "premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, isTradeIn, comments) " +
                                            "Values (@sku, @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price, " +
                                            "@quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments) " +
                                        "end " +
                                "end " +
                            "else if (@typeID = 2) " +
                                "begin " +
                                    "if exists(select sku from tbl_accessories where sku = @sku) " +
                                        "begin " +
                                    "UPDATE tbl_accessories SET size = @size, colour = @colour, price = @price, cost = @cost, brandID = @brandID, " +
                                    "modelID = @modelID, accessoryType = @clubType, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku " +
                                "end " +
                            "else " +
                                "begin " +
                                    "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, modelID, accessoryType, quantity, typeID, locationID, comments) " +
                                    "Values (@sku, @size, @colour, @price, @cost, @brandID, @modelID, @clubType, @quantity, @typeID, @locationID, @comments) " +
                                "end " +
                            "end " +
                            "else if (@typeID = 3) " +
                                "begin " +
                                    "if exists(select sku from tbl_clothing where sku = @sku) " +
                                        "begin " +
                                            "UPDATE tbl_clothing SET size = @size, colour = @colour, gender = @modelID, style = @clubType, " +
                                            "price = @price, cost = @cost, brandID = @brandID, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku " +
                                        "end " +
                                    "else " +
                                        "begin " +
                                            "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID, comments) " +
                                            "Values (@sku, @size, @colour, @modelID, @clubType, @price, @cost, @brandID, @quantity, @typeID, @locationID, @comments) " +
                                        "end " +
                                "end ";
                        reader = cmd.ExecuteReader();
                        conInsert.Close();

                    }
                }
            }

            //***************************************************************************************************
            //Step 9: Delete the temp tables that were used for storage
            //***************************************************************************************************

            cmd.CommandText = "Drop table tempItemStorage; Drop table tempErrorSkus;";
            conTempDB.Open();
            cmd.Connection = conTempDB;
            reader = cmd.ExecuteReader();
            conTempDB.Close();
            return skusWithErrors;
        }

        //******************COGS and PM REPORTING*******************************************************
        public System.Data.DataTable returnExtensiveInvoices(DateTime startDate, DateTime endDate, int locationID)
        {
            //This method returns a collection of relevant data to be used in the forming of an extensive invoice
            string sqlCmd = "SELECT CONCAT(tbl_invoice.invoiceNum, '-', tbl_invoice.invoiceSubNum) AS 'Invoice', "
                + "tbl_invoice.shippingAmount, (tbl_invoice.tradeinAmount * -1) AS 'tradeinAmount', CASE WHEN "
                + "EXISTS(SELECT tbl_invoiceItem.invoiceNum, tbl_invoiceItem.invoiceSubNum FROM tbl_invoiceItem "
                + "WHERE tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItem.invoiceSubNum = "
                + "tbl_invoice.invoiceSubNum) THEN CAST((SELECT SUM(CASE WHEN percentage = 1 AND itemDiscount <> "
                + "0 THEN (price * (itemDiscount / 100)) WHEN percentage = 0 AND itemDiscount <> 0 THEN itemDiscount "
                + "ELSE 0 END) FROM tbl_invoiceItem WHERE tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND "
                + "tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum) AS VARCHAR) WHEN EXISTS(SELECT "
                + "tbl_invoiceItemReturns.invoiceNum, tbl_invoiceItemReturns.invoiceSubNum FROM tbl_invoiceItemReturns "
                + "WHERE tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItemReturns.invoiceSubNum "
                + "= tbl_invoice.invoiceSubNum) THEN CAST((SELECT SUM(CASE WHEN percentage = 1 AND itemDiscount <> 0 "
                + "THEN (price * (itemDiscount / 100)) WHEN percentage = 0 AND itemDiscount <> 0 THEN itemDiscount "
                + "ELSE 0 END) FROM tbl_invoiceItemReturns WHERE tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum "
                + "AND tbl_invoiceItemReturns.invoiceSubNum = tbl_invoice.invoiceSubNum) AS VARCHAR) ELSE 'No items found' "
                + "END AS 'Total Discount', ROUND(subTotal + (tradeInAmount * -1),2) AS 'Pre-Tax', CASE WHEN "
                + "tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END AS governmentTax, CASE WHEN "
                + "tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END AS provincialTax, "
                + "ROUND(tbl_invoice.balanceDue + (CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE "
                + "0 END) + (CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END) + (tradeInAmount "
                + "* -1),2) AS 'Post-Tax', CASE WHEN EXISTS(SELECT tbl_invoiceItem.invoiceNum, tbl_invoiceItem.invoiceSubNum "
                + "FROM tbl_invoiceItem WHERE tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND "
                + "tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum) THEN CAST((SELECT SUM(cost * quantity) "
                + "FROM tbl_invoiceItem WHERE tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND "
                + "tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum) AS VARCHAR) WHEN EXISTS(SELECT "
                + "tbl_invoiceItemReturns.invoiceNum, tbl_invoiceItemReturns.invoiceSubNum FROM tbl_invoiceItemReturns "
                + "WHERE tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItemReturns.invoiceSubNum "
                + "= tbl_invoice.invoiceSubNum) THEN CAST((SELECT SUM(cost * quantity) FROM tbl_invoiceItemReturns WHERE "
                + "tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItemReturns.invoiceSubNum "
                + "= tbl_invoice.invoiceSubNum) AS VARCHAR) ELSE 'No items found' END AS 'COGS', CASE WHEN EXISTS(SELECT "
                + "tbl_invoiceItem.invoiceNum, tbl_invoiceItem.invoiceSubNum FROM tbl_invoiceItem WHERE "
                + "tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItem.invoiceSubNum = "
                + "tbl_invoice.invoiceSubNum) THEN CAST((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) "
                + "- (SELECT SUM(cost * quantity) FROM tbl_invoiceItem WHERE tbl_invoiceItem.invoiceNum = "
                + "tbl_invoice.invoiceNum AND tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum) AS VARCHAR) "
                + "WHEN EXISTS(SELECT tbl_invoiceItemReturns.invoiceNum, tbl_invoiceItemReturns.invoiceSubNum FROM "
                + "tbl_invoiceItemReturns WHERE tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND "
                + "tbl_invoiceItemReturns.invoiceSubNum = tbl_invoice.invoiceSubNum) THEN CAST((tbl_invoice.subTotal + "
                + "(-1 * tbl_invoice.tradeinAmount)) - (SELECT SUM(cost * quantity) FROM tbl_invoiceItemReturns WHERE "
                + "tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItemReturns.invoiceSubNum "
                + "= tbl_invoice.invoiceSubNum) AS VARCHAR) ELSE 'No items found' END AS 'Revenue Earned', CASE WHEN "
                + "EXISTS(SELECT tbl_invoiceItem.invoiceNum, tbl_invoiceItem.invoiceSubNum FROM tbl_invoiceItem WHERE "
                + "tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItem.invoiceSubNum = "
                + "tbl_invoice.invoiceSubNum) THEN CASE WHEN tbl_invoice.subTotal <> 0 THEN CAST(ROUND(((((tbl_invoice.subTotal "
                + "+ (-1 * tbl_invoice.tradeinAmount)) - (SELECT SUM(cost * quantity) FROM tbl_invoiceItem WHERE "
                + "tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItem.invoiceSubNum = "
                + "tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount),0))) "
                + "* 100),2) AS VARCHAR) WHEN tbl_invoice.subTotal = 0 THEN 'N/A' END WHEN EXISTS(SELECT "
                + "tbl_invoiceItemReturns.invoiceNum, tbl_invoiceItemReturns.invoiceSubNum FROM tbl_invoiceItemReturns WHERE "
                + "tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItemReturns.invoiceSubNum = "
                + "tbl_invoice.invoiceSubNum) THEN CASE WHEN tbl_invoice.subTotal <> 0 THEN CAST(ROUND(((((tbl_invoice.subTotal "
                + "+ (-1 * tbl_invoice.tradeinAmount)) - (SELECT SUM(cost * quantity) FROM tbl_invoiceItemReturns WHERE "
                + "tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceItemReturns.invoiceSubNum "
                + "= tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount),0))) "
                + "* 100),2) AS VARCHAR) WHEN tbl_invoice.subTotal = 0 THEN 'N/A' END ELSE 'No items found' END AS "
                + "'Profit Margin', (SELECT SUM(amountPaid) FROM tbl_invoiceMOP WHERE tbl_invoiceMOP.invoiceNum = "
                + "tbl_invoice.invoiceNum AND tbl_invoiceMOP.invoiceSubNum = tbl_invoice.invoiceSubNum) AS 'Payment', "
                + "(SELECT CONCAT(firstname, ' ', lastName) FROM tbl_customers WHERE tbl_customers.custID = "
                + "tbl_invoice.custID) AS 'Customer Name', (SELECT CONCAT(firstname, ' ', lastName) FROM "
                + "tbl_employee WHERE tbl_employee.empID = tbl_invoice.empID) AS 'Employee Name', tbl_invoice.invoiceDate "
                + "FROM tbl_invoice WHERE tbl_invoice.invoiceDate BETWEEN @startDate AND @endDate AND tbl_invoice.locationID = @locationID"; 
            
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //*******GST and PST totals********
        public List<TaxReport> returnTaxReportDetails(DateTime dtmStartDate, DateTime dtmEndDate)
        {
            string sqlCmd = "SELECT invoiceDate, locationID, SUM(subTotal) AS subTotal, SUM(shippingAmount) AS shippingAmount, "
                            + "SUM(discountAmount) AS discountAmount, SUM(tradeInAmount) AS tradeInAmount, "
                            + "SUM(CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END) AS governmentTax, "
                            + "SUM(CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END) AS provincialTax,"
                            + "SUM(balanceDue) AS balanceDue, transactionType FROM tbl_invoice "
                            + "WHERE invoiceDate BETWEEN @dtmStartDate AND @dtmEndDate "
                            + "GROUP BY invoiceDate, locationID, transactionType";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate }
            };
            return convertTaxReportFromDataTable(dbc.returnDataTableData(sqlCmd, parms));
        }
        private List<TaxReport> convertTaxReportFromDataTable(System.Data.DataTable dt)
        {
            List<TaxReport> taxReport = dt.AsEnumerable().Select(row =>
            new TaxReport
            {
                dtmInvoiceDate = row.Field<DateTime>("invoiceDate"),
                locationID = row.Field<int>("locationID"),
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeInAmount = row.Field<double>("tradeInAmount"),
                govTax = row.Field<double>("governmentTax"),
                provTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                transactionType = row.Field<int>("transactionType")
            }).ToList();
            return taxReport;
        }
        public int verifyTaxesCharged(object[] repInfo)
        {
            int indicator = 0;
            if (!taxesAvailable(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool taxesAvailable(object[] repInfo)
        {
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice WHERE (invoiceDate "
                + "BETWEEN @dtmStartDate AND @dtmEndDate) AND ((chargeGST = 1 AND governmentTax "
                + "> 0) OR(chargePST = 1 AND provincialTax > 0))";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0] },
                new object[] { "@dtmEndDate", dtm[1] }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolData = true;
            }
            return bolData;
        }


        //******************ITEMS SOLD REPORTING*******************************************************
        public System.Data.DataTable returnItemsSold(DateTime startDate, DateTime endDate, int locationID)
        {
            string sqlCmd = "SELECT CONCAT(I.invoiceNum, '-', I.invoiceSubNum) AS 'invoice', II.sku, II.totalCost AS cost, "
                + "II.totalPrice AS price, II.totalDiscount AS itemDiscount, II.percentage, CASE WHEN II.percentage = 1 "
                + "THEN II.totalPrice - ((II.totalPrice * (II.totalDiscount / 100)) + II.totalCost) ELSE II.totalPrice "
                + "- (II.totalDiscount + II.totalCost) END AS 'profit' FROM tbl_invoice I JOIN(SELECT invoiceNum, "
                + "invoiceSubNum, sku, SUM(price * quantity) AS totalPrice, SUM(cost * quantity) AS totalCost, CASE WHEN "
                + "percentage = 1 THEN SUM(itemDiscount) ELSE SUM(itemDiscount * quantity) END AS totalDiscount, percentage "
                + "FROM tbl_invoiceItem GROUP BY invoiceNum, invoiceSubNum, sku, percentage) AS II ON II.invoiceNum = "
                + "I.invoiceNum AND II.invoiceSubNum = I.invoiceSubNum WHERE II.sku NOT IN(SELECT sku FROM "
                + "tbl_tempTradeInCartSkus) AND II.invoiceNum NOT IN(SELECT invoiceNum FROM tbl_invoiceItemReturns) "
                + "AND I.locationID = @locationID AND I.invoiceDate BETWEEN @startDate AND @endDate ORDER BY I.invoiceNum, "
                + "I.invoiceSubNum";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //******************DISCOUNT REPORTING*******************************************************
        public System.Data.DataTable returnDiscountsBetweenDates(DateTime startDate, DateTime endDate, int locID)
        {
            //This method returns all invoices with discounts between two dates
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, "
                + "(SELECT CONCAT(firstName, ' ', lastName) FROM tbl_customers "
                + "WHERE custID = tbl_invoice.custID) AS 'customerName', "
                + "(SELECT CONCAT(firstName, ' ', lastName) FROM tbl_employee "
                + "WHERE empID = tbl_invoice.empID) AS 'employeeName', "
                + "discountAmount, balanceDue FROM tbl_invoice WHERE "
                + "discountAmount <> 0 AND invoiceDate BETWEEN @startDate "
                + "AND @endDate AND locationID = @locID";
            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate },
                new object[] { "@locID", locID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public double returnDiscountTotalBetweenDates(DateTime startDate, DateTime endDate)
        {
            //This method returns the total value of discounts between two dates
            string sqlCmd = "SELECT SUM(discountAmount) AS 'sumDiscountTotal' FROM tbl_invoice "
                + "WHERE invoiceDate BETWEEN @startDate AND @endDate";
            object[][] parms =
            {
                new object[] { "startDate", startDate },
                new object[] { "endDate", endDate }
            };
            return dbc.MakeDataBaseCallToReturnDouble(sqlCmd, parms);
        }
        public double returnDiscountTotalsForLocations(DateTime startDate, DateTime endDate, int locID)
        {
            string sqlCmd = "SELECT SUM(discountAmount) AS 'sumDiscountTotal' FROM tbl_invoice WHERE "
                + "invoiceDate BETWEEN @startDate AND @endDate AND discountAmount <> 0 AND locationID = @locID";
            object[][] parms =
            {
                new object[] { "startDate", startDate },
                new object[] { "endDate", endDate },
                new object[] { "locID", locID }
            };
            return dbc.MakeDataBaseCallToReturnDouble(sqlCmd, parms);
        }

        //******************CASHOUT REPORTING*******************************************************
        private List<Cashout> ReturnCashoutFromDataTable(System.Data.DataTable dt)
        {
            List<Cashout> cashout = dt.AsEnumerable().Select(row =>
            new Cashout
            {
                cashoutDate = row.Field<DateTime>("cashoutDate"),
                saleTradeIn = row.Field<double>("saleTradeIn"),
                saleGiftCard = row.Field<double>("saleGiftCard"),
                saleCash = row.Field<double>("saleCash"),
                saleDebit = row.Field<double>("saleDebit"),
                saleMasterCard = row.Field<double>("saleMasterCard"),
                saleVisa = row.Field<double>("saleVisa"),
                receiptTradeIn = row.Field<double>("receiptTradeIn"),
                receiptGiftCard = row.Field<double>("receiptGiftCard"),
                receiptCash = row.Field<double>("receiptCash"),
                receiptDebit = row.Field<double>("receiptDebit"),
                receiptMasterCard = row.Field<double>("receiptMasterCard"),
                receiptVisa = row.Field<double>("receiptVisa"),
                preTax = row.Field<double>("preTax"),
                saleGST = row.Field<double>("governmentTax"),
                salePST = row.Field<double>("provincialTax"),
                overShort = row.Field<double>("overShort")
            }).ToList();
            return cashout;
        }

        //******************STORE STATS REPORT***********************
        public System.Data.DataTable returnStoreStats(DateTime startDate, DateTime endDate, int timeFrame)
        {
            System.Data.DataTable stats = new System.Data.DataTable();

            object[][] parms =
            {
                new object[] { "@startDate", startDate },
                new object[] { "@endDate", endDate }
            };
            if (timeFrame == 1)
            {
                string dailyStats = "SELECT " +
                                        "YEAR(tbl_invoice.invoiceDate) AS invoiceYear, " +
                                        "DATENAME(month, '1900/' + CAST(MONTH(tbl_invoice.invoiceDate) AS VARCHAR(2)) + '/01') AS monthName, " +
                                        "tbl_invoice.invoiceDate as date, " +
                                        "(Select city from tbl_location where locationID = tbl_invoice.locationID) as cityName, " +
                                        "Round(SUM(CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END), 2) AS governmentTax, " +
                                        "Round(SUM(CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END), 2) AS provincialTax, " +
                                        "(SELECT cogs.COGS FROM(SELECT YEAR(G.invoiceDate) AS invoiceYear, G.invoiceDate as invoiceDay, SUM(G.soldCogs) AS COGS, G.locationID FROM( " +
                                            "SELECT i.invoiceDate, i.locationID, ROUND(SUM(II.cost* II.quantity), 2) AS soldCogs, 1 AS sal FROM tbl_invoice i JOIN tbl_invoiceItem II ON II.invoiceNum = i.invoiceNum AND II.invoiceSubNum = i.invoiceSubNum GROUP BY i.invoiceDate, i.locationID " +
                                            "UNION ALL SELECT i.invoiceDate, i.locationID, ROUND(SUM(IR.cost * IR.quantity), 2) AS returnCogs, 0 AS ret FROM tbl_invoice i JOIN tbl_invoiceItemReturns IR ON IR.invoiceNum = i.invoiceNum AND IR.invoiceSubNum = i.invoiceSubNum GROUP BY i.invoiceDate, i.locationID) AS G " +
                                            "GROUP BY locationID, YEAR(G.invoiceDate),G.invoiceDate) AS cogs " +
                                            "WHERE cogs.locationID = tbl_invoice.locationID AND cogs.invoiceYear = YEAR(tbl_invoice.invoiceDate) AND cogs.invoiceDay = tbl_invoice.invoiceDate) AS totalCOGS, " +
                                        "(Select ROUND(AVG(pm.pm), 4) from(SELECT ROUND(((((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) - (select sum(cost * quantity) from tbl_invoiceItem where tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum and tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount), 0)))  ),4)  " +
                                            "AS pm, tbl_invoice.invoiceDate as iDate, YEAR(tbl_invoice.invoiceDate) as iYear, tbl_invoice.locationID, tbl_invoice.invoiceDate FROM tbl_invoice " +
                                            "UNION ALL SELECT ROUND(((((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) - (select sum(cost * quantity) from tbl_invoiceItemReturns where tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum and tbl_invoiceItemReturns.invoiceSubNum = tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount), 0)))  ),4) " +
                                            "AS pm, tbl_invoice.invoiceDate, YEAR(tbl_invoice.invoiceDate), tbl_invoice.locationID, tbl_invoice.invoiceDate FROM tbl_invoice) AS pm WHERE pm.locationID = tbl_invoice.locationID AND pm.iYear = YEAR(tbl_invoice.invoiceDate) AND pm.iDate = tbl_invoice.invoiceDate " +
                                            "Group by pm.invoiceDate, YEAR(pm.invoiceDate), pm.locationID) AS averageProfitMargin, " +
                                    "ROUND(SUM(tbl_invoice.subTotal + (tradeInAmount * -1)), 2) as salespretax, " +
                                    "ROUND(SUM(tbl_invoice.balanceDue + (CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END) + (CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END) +(tradeInAmount * -1)),2) as salesposttax " +
                                    "from tbl_invoice where tbl_invoice.invoiceDate BETWEEN @startDate AND @endDate " +
                                    "group by tbl_invoice.invoiceDate, YEAR(tbl_invoice.invoiceDate), tbl_invoice.locationID " +
                                    "order by invoiceYear asc";
                stats = dbc.returnDataTableData(dailyStats, parms);
            }
            else if( timeFrame == 2)
            {
                string weeklyStats = "SELECT " +
                                        "YEAR(tbl_invoice.invoiceDate) AS invoiceYear, " +
                                        "DATENAME(month, '1900/' + CAST(MONTH(tbl_invoice.invoiceDate) AS VARCHAR(2)) + '/01') AS monthName, " +
                                        "(select dateadd(week, DatePart(wk, tbl_invoice.invoiceDate), dateadd(year, YEAR(tbl_invoice.invoiceDate) - 1900, 0)) - 4 - datepart(dw, dateadd(week, DatePart(wk, tbl_invoice.invoiceDate), dateadd(year, YEAR(tbl_invoice.invoiceDate) - 1900, 0)) - 4) + 1) AS date, " +
                                        "(Select city from tbl_location where locationID = tbl_invoice.locationID) as cityName, " +
                                        "Round(SUM(CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END), 2) AS governmentTax, " +
                                        "Round(SUM(CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END), 2) AS provincialTax, " +
                                            "(SELECT cogs.COGS FROM(SELECT YEAR(G.invoiceDate) AS invoiceYear, MONTH(G.invoiceDate) as invoiceMonth, DATEPART(wk, G.invoiceDate) AS invoiceWeek, SUM(G.soldCogs) AS COGS, G.locationID FROM( " +
                                            "SELECT i.invoiceDate, i.locationID, ROUND(SUM(II.cost* II.quantity), 2) AS soldCogs, 1 AS sal FROM tbl_invoice i JOIN tbl_invoiceItem II ON II.invoiceNum = i.invoiceNum AND II.invoiceSubNum = i.invoiceSubNum GROUP BY i.invoiceDate, i.locationID " +
                                            "UNION ALL SELECT i.invoiceDate, i.locationID, ROUND(SUM(IR.cost * IR.quantity), 2) AS returnCogs, 0 AS ret FROM tbl_invoice i JOIN tbl_invoiceItemReturns IR ON IR.invoiceNum = i.invoiceNum AND IR.invoiceSubNum = i.invoiceSubNum GROUP BY i.invoiceDate, i.locationID) AS G " +
                                            "GROUP BY DATEPART(wk, G.invoiceDate), locationID, YEAR(G.invoiceDate), MONTH(G.invoiceDate)) AS cogs " +
                                            "WHERE cogs.invoiceWeek = DATEPART(wk, tbl_invoice.invoiceDate) AND cogs.locationID = tbl_invoice.locationID AND cogs.invoiceYear = YEAR(tbl_invoice.invoiceDate) AND cogs.invoiceMonth = MONTH(tbl_invoice.invoiceDate)) AS totalCOGS, " +
                                        "(Select ROUND(AVG(pm.pm), 2) from(SELECT ROUND(((((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) - (select sum(cost * quantity) from tbl_invoiceItem where tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum and tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount), 0)))  ),4) " +
                                            "AS pm, DATEPART(wk, tbl_invoice.invoiceDate) as iWeek, MONTH(tbl_invoice.invoiceDate) as iMonth, YEAR(tbl_invoice.invoiceDate) as iYear, tbl_invoice.locationID, tbl_invoice.invoiceDate FROM tbl_invoice " +
                                            "UNION ALL SELECT ROUND(((((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) - (select sum(cost * quantity) from tbl_invoiceItemReturns where tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum and tbl_invoiceItemReturns.invoiceSubNum = tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount), 0)))  ),4) " +
                                            "AS pm, DATEPART(wk, tbl_invoice.invoiceDate), MONTH(tbl_invoice.invoiceDate), YEAR(tbl_invoice.invoiceDate), tbl_invoice.locationID, tbl_invoice.invoiceDate FROM tbl_invoice) AS pm WHERE pm.iWeek = DATEPART(wk, tbl_invoice.invoiceDate) AND pm.locationID = tbl_invoice.locationID AND pm.iYear = YEAR(tbl_invoice.invoiceDate) AND pm.iMonth = MONTH(tbl_invoice.invoiceDate) Group by DATEPART(wk, pm.invoiceDate), MONTH(pm.invoiceDate), YEAR(pm.invoiceDate), pm.locationID) AS averageProfitMargin, " +
                                    "ROUND(SUM(tbl_invoice.subTotal + (tradeInAmount * -1)), 2) as salespretax, " +
                                    "ROUND(SUM(tbl_invoice.balanceDue + (CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END) + (CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END) +(tradeInAmount * -1)),2) as salesposttax " +
                                    "from tbl_invoice where tbl_invoice.invoiceDate BETWEEN @startDate AND @endDate " +
                                    "group by DatePart(wk, tbl_invoice.invoiceDate), MONTH(tbl_invoice.invoiceDate), YEAR(tbl_invoice.invoiceDate), tbl_invoice.locationID " +
                                    "order by date asc, CityName asc";
                stats = dbc.returnDataTableData(weeklyStats, parms);
            }
            else
            {
                string monthlyStats = "SELECT " +
                                    "MONTH(tbl_invoice.invoiceDate) AS invoiceMonth, " +
                                    "YEAR(tbl_invoice.invoiceDate) AS invoiceYear, " +
                                    "DATENAME(month, '1900/' + CAST(MONTH(tbl_invoice.invoiceDate) AS VARCHAR(2)) + '/01') AS monthName, " +
                                    "DATENAME(month, '1900/' + CAST(MONTH(tbl_invoice.invoiceDate) AS VARCHAR(2)) + '/01') AS date, " +
                                    "(Select city from tbl_location where locationID = tbl_invoice.locationID) as cityName, " +
                                    "Round(SUM(CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END), 2) AS governmentTax, " +
                                    "Round(SUM(CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END), 2) AS provincialTax, " +
                                    "(SELECT cogs.COGS FROM(SELECT YEAR(G.invoiceDate) AS invoiceYear, MONTH(G.invoiceDate) as invoiceMonth, SUM(G.soldCogs) AS COGS, G.locationID FROM( " +
                                        "SELECT i.invoiceDate, i.locationID, ROUND(SUM(II.cost* II.quantity), 2) AS soldCogs, 1 AS sal FROM tbl_invoice i JOIN tbl_invoiceItem II ON II.invoiceNum = i.invoiceNum AND II.invoiceSubNum = i.invoiceSubNum GROUP BY i.invoiceDate, i.locationID " +
                                        "UNION ALL SELECT i.invoiceDate, i.locationID, ROUND(SUM(IR.cost * IR.quantity), 2) AS returnCogs, 0 AS ret FROM tbl_invoice i JOIN tbl_invoiceItemReturns IR ON IR.invoiceNum = i.invoiceNum AND IR.invoiceSubNum = i.invoiceSubNum GROUP BY i.invoiceDate, i.locationID) AS G " +
                                        "GROUP BY locationID, YEAR(G.invoiceDate), MONTH(G.invoiceDate)) AS cogs " +
                                        "WHERE cogs.locationID = tbl_invoice.locationID AND cogs.invoiceYear = YEAR(tbl_invoice.invoiceDate) AND cogs.invoiceMonth = MONTH(tbl_invoice.invoiceDate)) AS totalCOGS, " +
                                    "(Select ROUND(AVG(pm.pm), 2) from(SELECT ROUND(((((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) - (select sum(cost * quantity) from tbl_invoiceItem where tbl_invoiceItem.invoiceNum = tbl_invoice.invoiceNum and tbl_invoiceItem.invoiceSubNum = tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount), 0)))  ),4) " +
                                        "AS pm, MONTH(tbl_invoice.invoiceDate) as iMonth, YEAR(tbl_invoice.invoiceDate) as iYear, tbl_invoice.locationID, tbl_invoice.invoiceDate FROM tbl_invoice " +
                                        "UNION ALL SELECT ROUND(((((tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount)) - (select sum(cost * quantity) from tbl_invoiceItemReturns where tbl_invoiceItemReturns.invoiceNum = tbl_invoice.invoiceNum and tbl_invoiceItemReturns.invoiceSubNum = tbl_invoice.invoiceSubNum)) / (NULLIF(tbl_invoice.subTotal + (-1 * tbl_invoice.tradeinAmount), 0)))  ),4) " +
                                        "AS pm, MONTH(tbl_invoice.invoiceDate), YEAR(tbl_invoice.invoiceDate), tbl_invoice.locationID, tbl_invoice.invoiceDate FROM tbl_invoice) AS pm WHERE pm.locationID = tbl_invoice.locationID AND pm.iYear = YEAR(tbl_invoice.invoiceDate) AND pm.iMonth = MONTH(tbl_invoice.invoiceDate) " +
                                        "Group by MONTH(pm.invoiceDate), YEAR(pm.invoiceDate), pm.locationID) AS averageProfitMargin, " +
                                    "ROUND(SUM(tbl_invoice.subTotal + (tradeInAmount * -1)), 2) as salespretax, " +
                                    "ROUND(SUM(tbl_invoice.balanceDue + (CASE WHEN tbl_invoice.chargeGST = 1 THEN tbl_invoice.governmentTax ELSE 0 END) + (CASE WHEN tbl_invoice.chargePST = 1 THEN tbl_invoice.provincialTax ELSE 0 END) +(tradeInAmount * -1)),2) as salesposttax " +
                                    "from tbl_invoice where tbl_invoice.invoiceDate BETWEEN @startDate AND @endDate " +
                                    "group by MONTH(tbl_invoice.invoiceDate), YEAR(tbl_invoice.invoiceDate), tbl_invoice.locationID " +
                                    "order by invoiceYear asc";
                stats = dbc.returnDataTableData(monthlyStats, parms);
            }
            return stats;
        }
        public int verifyStatsAvailable(object[] repInfo)
        {
            int indicator = 0;
            if (!statsAvailable(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        public bool statsAvailable(object[] repInfo)
        {
            bool bolTA = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(invoiceNum) FROM tbl_invoice "
                        + "WHERE invoiceDate BETWEEN @startDate AND @endDate ";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolTA = true;
            }
            return bolTA;
        }

        //*************Specific Apparel Report********* w/CORRECT profit margin calculation
        public System.Data.DataTable returnSpecificApparelDataTableForReport(DateTime dtmStartDate, DateTime dtmEndDate)
        {
            string sqlCmd = "SELECT L.locationName, FQ.sku, FQ.description, SUM(FQ.quantity) AS overallQuantity, "
                + "SUM(FQ.totalCost) AS overallCost, SUM(FQ.totalPrice) AS overallPrice FROM(SELECT I.locationID, "
                + "SA.sku, II.description, (SUM(II.quantity) - ISNULL(SUM(IIR.quantity), 0)) * II.cost AS totalCost, "
                + "(SUM(II.quantity) - ISNULL(SUM(IIR.quantity), 0)) * ROUND(CASE WHEN II.percentage = 1 THEN II.price "
                + "- ((II.itemDiscount / 100) * II.price) ELSE II.price - II.itemDiscount END, 2) AS totalPrice, "
                + "SUM(II.quantity) - ISNULL(SUM(IIR.quantity), 0) AS quantity FROM tbl_invoiceItem II LEFT JOIN(SELECT "
                + "r.invoiceNum, o.locationID, s.sku, r.cost, r.price, SUM(r.quantity) AS quantity FROM tbl_invoiceItemReturns "
                + "r JOIN tbl_invoice o ON o.invoiceNum = r.invoiceNum AND o.invoiceSubNum = r.invoiceSubNum JOIN "
                + "tbl_specificApparel s ON s.sku = r.sku WHERE o.invoiceDate BETWEEN @dtmStartDate AND @dtmEndDate GROUP BY "
                + "r.invoiceNum, o.locationID, s.sku, r.cost, r.price) IIR ON IIR.invoiceNum = II.invoiceNum JOIN "
                + "tbl_invoice I ON I.invoiceNum = II.invoiceNum AND I.invoiceSubNum = II.invoiceSubNum JOIN "
                + "tbl_specificApparel SA ON SA.sku = II.sku WHERE I.invoiceDate BETWEEN @dtmStartDate AND @dtmEndDate "
                + "GROUP BY SA.sku, I.locationID, II.description, II.cost, II.price, II.percentage, II.itemDiscount) "
                + "FQ JOIN tbl_location L ON L.locationID = FQ.locationID GROUP BY FQ.sku, L.locationName, FQ.description";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public int verifySpecificApparel(object[] repInfo)
        {
            int indicator = 0;
            if (!specificApparelAvailable(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool specificApparelAvailable(object[] repInfo)
        {
            bool bolData = false;
            DateTime[] dtm = (DateTime[])repInfo[0];

            string sqlCmd = "SELECT SUM(II.quantity) overallQuantity FROM tbl_invoiceItem II "
                + "JOIN tbl_invoice I ON I.invoiceNum = II.invoiceNum AND I.invoiceSubNum = "
                + "II.invoiceSubNum JOIN tbl_specificApparel SA ON SA.sku = II.sku WHERE "
                + "I.invoiceDate BETWEEN @startDate AND @endDate";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolData = true;
            }
            return bolData;
        }



        public System.Data.DataTable ReturnCashoutsForSelectedDates(object[] passing)
        {
            string sqlCmd = "SELECT cashoutDate, locationID, saleTradeIn, receiptTradeIn, saleGiftCard, "
                + "receiptGiftCard, saleCash, receiptCash, saleDebit, receiptDebit, saleMasterCard, "
                + "receiptMasterCard, saleVisa, receiptVisa, overShort, processed, finalized FROM "
                + "tbl_cashout WHERE cashoutDate BETWEEN @startDate AND @endDate AND locationID = @locationID";

            DateTime[] dtm = (DateTime[])passing[0];
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", passing[1] }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public int CashoutsProcessed(object[] repInfo)
        {
            int indicator = 0;
            if (!CashotInDateRange(repInfo))
            {
                indicator = 1;
            }
            return indicator;
        }
        private bool CashotInDateRange(object[] repInfo)
        {
            bool bolCIDR = false;
            DateTime[] dtm = (DateTime[])repInfo[0];
            string sqlCmd = "SELECT COUNT(cashoutDate) FROM tbl_cashout "
                        + "WHERE cashoutDate BETWEEN @startDate AND @endDate "
                        + "AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] },
                new object[] { "@locationID", Convert.ToInt32(repInfo[1]) }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolCIDR = true;
            }
            return bolCIDR;
        }
        public void FinalizeCashout(string args)
        {
            DateTime dtm = DateTime.Parse(args.Split(' ')[0]);
            int location = Convert.ToInt32(args.Split(' ')[1]);
            string sqlCmd = "UPDATE tbl_cashout SET finalized = 1 "
                + "WHERE cashoutDate = @cashoutDate AND "
                + "locationID = @locationID";

            object[][] parms =
            {
                new object[] { "@cashoutDate", dtm },
                new object[] { "@locationID", location }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        public List<Cashout> ReturnSelectedCashout(object[] args)
        {
            string sqlCmd = "SELECT cashoutDate, saleTradeIn, saleGiftCard, saleCash, "
                + "saleDebit, saleMasterCard, saleVisa, receiptTradeIn, receiptGiftCard, "
                + "receiptCash, receiptDebit, receiptMasterCard, receiptVisa, preTax, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "overShort FROM tbl_cashout WHERE "
                + "cashoutDate = @cashoutDate AND locationID = @locationID";
            object[][] parms =
            {
                new object[] { "@cashoutDate", DateTime.Parse(args[0].ToString()) },
                new object[] { "@locationID", Convert.ToInt32(args[1]) }
            };

            return ReturnCashoutFromDataTable(dbc.returnDataTableData(sqlCmd, parms));
        }
        public bool CashoutExists(object[] args)
        {
            bool exists = false;
            string sqlCmd = "SELECT COUNT(cashoutDate) FROM tbl_cashout "
                + "WHERE cashoutDate = @cashoutDate AND locationID = @locationID";

            object[][] parms =
            {
                new object[] { "@cashoutDate", (DateTime)args[0] },
                new object[] { "@locationID", Convert.ToInt32(args[1]) }
            };
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                exists = true;
            }
            return exists;
        }
        public void UpdateCashout(Cashout cas)
        {
            string sqlCmd = "UPDATE tbl_cashout SET cashoutTime = @cashoutTime, "
                + "saleTradeIn = @saleTradeIn, saleGiftCard = @saleGiftCard, saleCash = @saleCash, "
                + "saleDebit = @saleDebit, saleMasterCard = @saleMasterCard, saleVisa = @saleVisa, "
                + "receiptTradeIn = @receiptTradeIn, receiptGiftCard = @receiptGiftCard, "
                + "receiptCash = @receiptCash, receiptDebit = @receiptDebit, "
                + "receiptMasterCard = @receiptMasterCard, receiptVisa = @receiptVisa, "
                + "preTax = @preTax, governmentTax = @gTax, provincialTax = @pTax, "
                + "overShort = @overShort, finalized = @finalized, processed = @processed, "
                + "empID = @empID WHERE cashoutDate = @cashoutDate AND locationID = @locID";

            object[][] parms =
            {
                new object[] { "@cashoutDate", cas.cashoutDate },
                new object[] { "@cashoutTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@saleTradeIn", cas.saleTradeIn },
                new object[] { "@saleGiftCard", cas.saleGiftCard },
                new object[] { "@saleCash", cas.saleCash },
                new object[] { "@saleDebit", cas.saleDebit },
                new object[] { "@saleMasterCard", cas.saleMasterCard },
                new object[] { "@saleVisa", cas.saleVisa },
                new object[] { "@receiptTradeIn", cas.receiptTradeIn },
                new object[] { "@receiptGiftCard", cas.receiptGiftCard },
                new object[] { "@receiptCash", cas.receiptCash },
                new object[] { "@receiptDebit", cas.receiptDebit },
                new object[] { "@receiptMasterCard", cas.receiptMasterCard },
                new object[] { "@receiptVisa", cas.receiptVisa },
                new object[] { "@preTax", cas.preTax },
                new object[] { "@gTax", cas.saleGST },
                new object[] { "@pTax", cas.salePST },
                new object[] { "@overShort", cas.overShort },
                new object[] { "@finalized", cas.finalized },
                new object[] { "@processed", cas.processed },
                new object[] { "@locID", cas.locationID },
                new object[] { "@empID", cas.empID }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
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
        public void itemExports(string selection, FileInfo newFile, string fileName)
        {
            //This is the table that has all of the information lined up the way Caspio needs it to be
            System.Data.DataTable exportTable = new System.Data.DataTable();
            if (selection.Equals("all"))
            {
                exportTable = exportAllInventory();
            }
            else if (selection.Equals("clubs"))
            {
                exportTable = exportAllAdd_Clubs();
            }
            else if (selection.Equals("accessories"))
            {
                exportTable = exportAllAdd_Accessories();
            }
            else if (selection.Equals("clothing"))
            {
                exportTable = exportAllAdd_Clothing();
            }
            String[] itemExportColumns = { "Vendor", "Store_ID", "ItemNumber", "Shipment_Date", "Brand", "Model", "Club_Type", "Shaft",
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
                    worksheet.Cells[recordIndex, 3].Value = Convert.ToDouble(row[2].ToString());
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
        //Puts the clubs in the export table
        public System.Data.DataTable exportAllAdd_Clubs()
        {
            string sqlCmd = "SELECT '' AS 'vendor', (SELECT tbl_location.locationName FROM "
                + "tbl_location WHERE tbl_location.locationID = tbl_clubs.locationID) AS locationName, "
                + "tbl_clubs.sku, '' AS 'shipmentDate', (SELECT tbl_brand.brandName FROM tbl_brand "
                + "WHERE tbl_brand.brandID = tbl_clubs.brandID ) AS brandName , (SELECT "
                + "tbl_model.modelName FROM tbl_model WHERE tbl_model.modelID = tbl_clubs.modelID) "
                + "AS modelName, tbl_clubs.clubType, tbl_clubs.shaft, tbl_clubs.numberOfClubs, 0 AS "
                + "'tradeinPrice', tbl_clubs.premium, tbl_clubs.cost, tbl_clubs.quantity, 0 AS "
                + "'extendedPrice', tbl_clubs.price, tbl_clubs.comments, '' AS 'image', "
                + "tbl_clubs.clubSpec, tbl_clubs.shaftSpec, tbl_clubs.shaftFlex, tbl_clubs.dexterity, "
                + "(SELECT tbl_location.secondaryIdentifier FROM tbl_location WHERE "
                + "tbl_location.locationID = tbl_clubs.locationID) AS locationSecondary, '' AS "
                + "'received', 0 AS 'paid', (SELECT tbl_itemType.typeDescription FROM tbl_itemType "
                + "WHERE tbl_itemType.typeID = tbl_clubs.typeID) AS itemType, tbl_clubs.isTradeIn "
                + "FROM tbl_clubs";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        //Puts the accessories in the export table
        public System.Data.DataTable exportAllAdd_Accessories()
        {
            string sqlCmd = "SELECT '' AS 'vendor', (SELECT tbl_location.locationName FROM "
                + "tbl_location WHERE tbl_location.locationID = tbl_accessories.locationID) "
                + "AS locationName, tbl_accessories.sku, '' AS 'shipmentDate', (SELECT "
                + "tbl_brand.brandName FROM tbl_brand WHERE tbl_brand.brandID = "
                + "tbl_accessories.brandID) AS brandName, (SELECT tbl_model.modelName FROM "
                + "tbl_model WHERE tbl_model.modelID = tbl_accessories.modelID) AS modelName, "
                + "tbl_accessories.accessoryType AS 'clubType', '' AS 'shaft', '' AS "
                + "'numberOfClubs', 0 AS 'tradeinPrice', 0 AS 'premium', tbl_accessories.cost, "
                + "tbl_accessories.quantity, 0 AS 'extendedPrice', tbl_accessories.price, "
                + "tbl_accessories.comments, '' AS 'image', '' AS 'clubSpec', '' AS 'shaftSpec', "
                + "'' AS 'shaftFlex', '' as 'dexterity', (SELECT tbl_location.secondaryIdentifier "
                + "FROM tbl_location WHERE tbl_location.locationID = tbl_accessories.locationID) "
                + "AS locationSecondary, '' AS 'received', 0 AS 'paid', (SELECT "
                + "tbl_itemType.typeDescription FROM tbl_itemType WHERE tbl_itemType.typeID = "
                + "tbl_accessories.typeID) AS itemType, 0 AS 'isTradeIn' FROM tbl_accessories";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        //Puts the clothing in the export table
        public System.Data.DataTable exportAllAdd_Clothing()
        {
            string sqlCmd = "SELECT '' AS 'vendor', (SELECT tbl_location.locationName "
                + "FROM tbl_location WHERE tbl_location.locationID = "
                + "tbl_clothing.locationID) AS locationName, tbl_clothing.sku, '' AS "
                + "'shipmentDate', (SELECT tbl_brand.brandName FROM tbl_brand WHERE "
                + "tbl_brand.brandID = tbl_clothing.brandID) AS brandName, "
                + "tbl_clothing.gender AS 'modelName', tbl_clothing.style AS 'clubType', "
                + "'' AS 'shaft', '' AS 'numberOfClubs', 0 AS 'tradeinPrice', 0 AS "
                + "'premium', tbl_clothing.cost, tbl_clothing.quantity, 0 AS 'extendedPrice', "
                + "tbl_clothing.price, tbl_clothing.comments, '' AS 'image', '' AS 'clubSpec', "
                + "'' AS 'shaftSpec', '' AS 'shaftFlex', '' AS 'dexterity', (SELECT "
                + "tbl_location.secondaryIdentifier FROM tbl_location WHERE "
                + "tbl_location.locationID = tbl_clothing.locationID) AS locationSecondary, "
                + "'' AS 'received', 0 AS 'paid', (SELECT tbl_itemType.typeDescription "
                + "FROM tbl_itemType WHERE tbl_itemType.typeID = tbl_clothing.typeID) "
                + "AS itemType, 0 AS 'isTradeIn' FROM tbl_clothing";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public System.Data.DataTable exportAllInventory()
        {
            string sqlCmd = "SELECT '' AS 'vendor', (SELECT tbl_location.locationName FROM "
                + "tbl_location WHERE tbl_location.locationID = tbl_clubs.locationID) AS locationName, "
                + "tbl_clubs.sku, '' AS 'shipmentDate', (SELECT tbl_brand.brandName FROM tbl_brand "
                + "WHERE tbl_brand.brandID = tbl_clubs.brandID ) AS brandName , (SELECT "
                + "tbl_model.modelName FROM tbl_model WHERE tbl_model.modelID = tbl_clubs.modelID) "
                + "AS modelName, tbl_clubs.clubType, tbl_clubs.shaft, tbl_clubs.numberOfClubs, 0 AS "
                + "'tradeinPrice', tbl_clubs.premium, tbl_clubs.cost, tbl_clubs.quantity, 0 AS "
                + "'extendedPrice', tbl_clubs.price, tbl_clubs.comments, '' AS 'image', "
                + "tbl_clubs.clubSpec, tbl_clubs.shaftSpec, tbl_clubs.shaftFlex, tbl_clubs.dexterity, "
                + "(SELECT tbl_location.secondaryIdentifier FROM tbl_location WHERE "
                + "tbl_location.locationID = tbl_clubs.locationID) AS locationSecondary, '' AS "
                + "'received', 0 AS 'paid', (SELECT tbl_itemType.typeDescription FROM tbl_itemType "
                + "WHERE tbl_itemType.typeID = tbl_clubs.typeID) AS itemType, tbl_clubs.isTradeIn "
                + "FROM tbl_clubs UNION "
                + "SELECT '' AS 'vendor', (SELECT tbl_location.locationName FROM "
                + "tbl_location WHERE tbl_location.locationID = tbl_accessories.locationID) "
                + "AS locationName, tbl_accessories.sku, '' AS 'shipmentDate', (SELECT "
                + "tbl_brand.brandName FROM tbl_brand WHERE tbl_brand.brandID = "
                + "tbl_accessories.brandID) AS brandName, (SELECT tbl_model.modelName FROM "
                + "tbl_model WHERE tbl_model.modelID = tbl_accessories.modelID) AS modelName, "
                + "tbl_accessories.accessoryType AS 'clubType', '' AS 'shaft', '' AS "
                + "'numberOfClubs', 0 AS 'tradeinPrice', 0 AS 'premium', tbl_accessories.cost, "
                + "tbl_accessories.quantity, 0 AS 'extendedPrice', tbl_accessories.price, "
                + "tbl_accessories.comments, '' AS 'image', '' AS 'clubSpec', '' AS 'shaftSpec', "
                + "'' AS 'shaftFlex', '' as 'dexterity', (SELECT tbl_location.secondaryIdentifier "
                + "FROM tbl_location WHERE tbl_location.locationID = tbl_accessories.locationID) "
                + "AS locationSecondary, '' AS 'received', 0 AS 'paid', (SELECT "
                + "tbl_itemType.typeDescription FROM tbl_itemType WHERE tbl_itemType.typeID = "
                + "tbl_accessories.typeID) AS itemType, 0 AS 'isTradeIn' FROM tbl_accessories "
                + "UNION "
                + "SELECT '' AS 'vendor', (SELECT tbl_location.locationName "
                + "FROM tbl_location WHERE tbl_location.locationID = "
                + "tbl_clothing.locationID) AS locationName, tbl_clothing.sku, '' AS "
                + "'shipmentDate', (SELECT tbl_brand.brandName FROM tbl_brand WHERE "
                + "tbl_brand.brandID = tbl_clothing.brandID) AS brandName, "
                + "tbl_clothing.gender AS 'modelName', tbl_clothing.style AS 'clubType', "
                + "'' AS 'shaft', '' AS 'numberOfClubs', 0 AS 'tradeinPrice', 0 AS "
                + "'premium', tbl_clothing.cost, tbl_clothing.quantity, 0 AS 'extendedPrice', "
                + "tbl_clothing.price, tbl_clothing.comments, '' AS 'image', '' AS 'clubSpec', "
                + "'' AS 'shaftSpec', '' AS 'shaftFlex', '' AS 'dexterity', (SELECT "
                + "tbl_location.secondaryIdentifier FROM tbl_location WHERE "
                + "tbl_location.locationID = tbl_clothing.locationID) AS locationSecondary, "
                + "'' AS 'received', 0 AS 'paid', (SELECT tbl_itemType.typeDescription "
                + "FROM tbl_itemType WHERE tbl_itemType.typeID = tbl_clothing.typeID) "
                + "AS itemType, 0 AS 'isTradeIn' FROM tbl_clothing";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        //New return queries for reports


        private string QueryBuilder(object[] selections)
        {
            string strSelect = "SELECT ";
            string strFrom = "FROM ";
            string strJoin = "JOIN ";
            string strWhere = "WHERE ";
            string strGroupBy = "GROUP BY ";
            string strOrderBy = "ORDER BY ";

            return strSelect + strFrom + strJoin + strWhere + strGroupBy + strOrderBy;

        }
        //Returns Trade Ins
        private string[] returnTradeInDuringDateRnageAndLocatio()
        {
            string strSelect = "I.invoiceDate, TI.invoiceNum, TI.invoiceSubNum, I.locationID, TI.sku, "
                + "TI.cost ";

            string strFrom = "tbl_invoice I ";

            string strJoin = "(SELECT II.invoiceNum, II.invoiceSubNum, T.locationID, "
                + "T.sku, T.cost FROM tbl_tempTradeInCartSkus T JOIN tbl_invoiceItem II ON II.sku = T.sku) "
                + "TI ON TI.invoiceNum = I.invoiceNum AND TI.invoiceSubNum = I.invoiceSubNum ";

            string strWhere = "I.invoiceDate BETWEEN @dtmStartDate AND @dtmEndDate AND I.locationID = @locationID ";

            string[] query = { strSelect, strFrom, strJoin, strWhere };
            return query;
        }
    }
}