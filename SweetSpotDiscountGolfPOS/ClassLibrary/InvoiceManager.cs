using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS
{
    public class InvoiceManager
    {
        DatabaseCalls dbc = new DatabaseCalls();

        private void ExecuteNonReturnCall(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnInt(string sqlCmd, object[][] parms, object[] objPageDetails, string strQueryName)
        {
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<Invoice> ConvertFromDataTableToInvoice(DataTable dt, object[] objPageDetails)
        {
            CustomerManager CM = new CustomerManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            List<Invoice> invoice = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                intInvoiceSubNumber = row.Field<int>("intInvoiceSubNumber"),
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                dtmInvoiceTime = row.Field<DateTime>("dtmInvoiceTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                fltSubTotal = row.Field<double>("fltSubTotal"),
                fltShippingCharges = row.Field<double>("fltShippingCharges"),
                fltTotalDiscount = row.Field<double>("fltTotalDiscount"),
                fltTotalTradeIn = row.Field<double>("fltTotalTradeIn"),
                fltGovernmentTaxAmount = row.Field<double>("fltGovernmentTaxAmount"),
                fltProvincialTaxAmount = row.Field<double>("fltProvincialTaxAmount"),
                fltBalanceDue = row.Field<double>("fltBalanceDue"),
                invoiceItems = IIM.ReturnInvoiceItems(row.Field<int>("intInvoiceID"), objPageDetails),
                invoiceMops = IMM.ReturnInvoiceMOPs(row.Field<int>("intInvoiceID"), objPageDetails),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation"),
                bitChargeGST = row.Field<bool>("bitChargeGST"),
                bitChargePST = row.Field<bool>("bitChargePST")
            }).ToList();
            return invoice;
        }
        private List<Invoice> ConvertFromDataTableToCurrentInvoice(DataTable dt, object[] objPageDetails)
        {
            //TODO: Update ConvertFromDataTableToCurrentInvoice
            CustomerManager CM = new CustomerManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                intInvoiceSubNumber = row.Field<int>("intInvoiceSubNumber"),
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                dtmInvoiceTime = row.Field<DateTime>("dtmInvoiceTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                fltSubTotal = row.Field<double>("fltSubTotal"),
                fltShippingCharges = row.Field<double>("fltShippingCharges"),
                fltTotalDiscount = row.Field<double>("fltTotalDiscount"),
                fltTotalTradeIn = row.Field<double>("fltTotalTradeIn"),
                fltGovernmentTaxAmount = row.Field<double>("fltGovernmentTaxAmount"),
                fltProvincialTaxAmount = row.Field<double>("fltProvincialTaxAmount"),
                fltBalanceDue = row.Field<double>("fltBalanceDue"),
                invoiceItems = IIM.ReturnInvoiceItemsCurrentSale(row.Field<int>("intInvoiceID"), objPageDetails),
                invoiceMops = IMM.ReturnInvoiceMOPsCurrentSale(row.Field<int>("intInvoiceID"), objPageDetails),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varTransactionName = ReturnTransactionName(row.Field<int>("intTransactionTypeID"), objPageDetails),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation"),
                bitChargeGST = row.Field<bool>("bitChargeGST"),
                bitChargePST = row.Field<bool>("bitChargePST")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToPurchaseInvoice(DataTable dt, object[] objPageDetails)
        {
            CustomerManager CM = new CustomerManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            List<Invoice> invoice = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                intInvoiceSubNumber = row.Field<int>("intInvoiceSubNumber"),
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                dtmInvoiceTime = row.Field<DateTime>("dtmInvoiceTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                fltSubTotal = row.Field<double>("fltSubTotal"),
                fltShippingCharges = row.Field<double>("fltShippingCharges"),
                fltTotalDiscount = row.Field<double>("fltTotalDiscount"),
                fltTotalTradeIn = row.Field<double>("fltTotalTradeIn"),
                fltGovernmentTaxAmount = row.Field<double>("fltGovernmentTaxAmount"),
                fltProvincialTaxAmount = row.Field<double>("fltProvincialTaxAmount"),
                fltBalanceDue = row.Field<double>("fltBalanceDue"),
                invoiceItems = IIM.ReturnInvoiceItemsCurrentSale(Convert.ToInt32(row.Field<int>("intInvoiceID")), objPageDetails),
                invoiceMops = IMM.ReturnPurchaseMOPsCurrentSale(Convert.ToInt32(row.Field<int>("intInvoiceID")), objPageDetails),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varTransactionName = ReturnTransactionName(row.Field<int>("intTransactionTypeID"), objPageDetails),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation"),
                bitChargeGST = row.Field<bool>("bitChargeGST"),
                bitChargePST = row.Field<bool>("bitChargePST")
            }).ToList();
            return invoice;
        }
        private List<Invoice> ConvertFromDataTableToReceipt(DataTable dt, object[] objPageDetails)
        {
            CustomerManager CM = new CustomerManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            List<Invoice> invoice = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intReceiptID"),
                varInvoiceNumber = row.Field<string>("varReceiptNumber"),
                dtmInvoiceDate = row.Field<DateTime>("dtmReceiptDate"),
                dtmInvoiceTime = row.Field<DateTime>("dtmReceiptTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                fltSubTotal = row.Field<double>("fltReceiptTotal"),
                invoiceItems = IIM.ReturnInvoiceItemsReceipt(Convert.ToInt32(row.Field<int>("intReceiptID").ToString()), objPageDetails),
                invoiceMops = IMM.ReturnReceiptMOPsPurchase(Convert.ToInt32(row.Field<int>("intReceiptID").ToString()), objPageDetails),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varTransactionName = ReturnTransactionName(row.Field<int>("intTransactionTypeID"), objPageDetails),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            return invoice;
        }
        private List<Invoice> ConvertFromDataTableToInvoiceForReturns(DataTable dt, object[] objPageDetails)
        {
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                intInvoiceSubNumber = row.Field<int>("intInvoiceSubNumber"),
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                fltBalanceDue = row.Field<double>("fltBalanceDue")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableInvoiceListByCustomer(DataTable dt, object[] objPageDetails)
        {
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                intInvoiceSubNumber = row.Field<int>("intInvoiceSubNumber"),
                dtmInvoiceDate = row.Field<DateTime>("dtmInvoiceDate"),
                dtmInvoiceTime = row.Field<DateTime>("dtmInvoiceTime"),
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                fltSubTotal = row.Field<double>("fltSubTotal"),
                fltShippingCharges = row.Field<double>("fltShippingCharges"),
                fltTotalDiscount = row.Field<double>("fltTotalDiscount"),
                fltTotalTradeIn = row.Field<double>("fltTotalTradeIn"),
                fltGovernmentTaxAmount = row.Field<double>("fltGovernmentTaxAmount"),
                fltProvincialTaxAmount = row.Field<double>("fltProvincialTaxAmount"),
                fltBalanceDue = row.Field<double>("fltBalanceDue"),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation"),
                bitChargeGST = row.Field<bool>("bitChargeGST"),
                bitChargePST = row.Field<bool>("bitChargePST")
            }).ToList();
            return i;
        }


        //Returns list of invoice based on an invoice string from the Final Table
        public List<Invoice> ReturnInvoice(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoice";
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime AS DATETIME) AS dtmInvoiceTime, "
                + "intCustomerID, intEmployeeID, intLocationID, fltSubTotal, fltShippingCharges, fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = "
                + "1 THEN fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END AS "
                + "fltProvincialTaxAmount, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, bitChargePST FROM tbl_invoice WHERE "
                + "intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnPurchaseInvoice(int receiptID, object[] objPageDetails)
        {
            string strQueryName = "ReturnPurchaseInvoice";
            string sqlCmd = "SELECT intReceiptID, varReceiptNumber, dtmReceiptDate, CAST(dtmReceiptTime AS DATETIME) AS dtmReceiptTime, "
                + "intCustomerID, intEmployeeID, intLocationID, fltReceiptTotal, intTransactionTypeID, varAdditionalInformation FROM "
                + "tbl_receipt WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //public Invoice ReturnInvoiceForReturns(string invoice, object[] objPageDetails)
        //{
        //    invoice = invoice.Split('-')[1].ToString() + "-1";
        //    return ReturnInvoice(invoice, objPageDetails)[0];
        //}
        //Returns list of invoices based on an invoice string from the Current table
        public List<Invoice> ReturnCurrentInvoice(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentInvoice";
            //TODO: Update ReturnCurrentInvoice
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime "
                + "AS DATETIME) AS dtmInvoiceTime, intCustomerID, intEmployeeID, intLocationID, fltSubTotal, fltShippingCharges, "
                + "fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount ELSE 0 END AS "
                + "fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END AS "
                + "fltProvincialTaxAmount, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, "
                + "bitChargePST FROM tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnCurrentPurchaseInvoice(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentPurchaseInvoice";
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime AS DATETIME) AS dtmInvoiceTime, "
                + "intCustomerID, intEmployeeID, intLocationID, fltSubTotal, fltShippingCharge, fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = 1 "
                + "THEN fltGovernmentTaxAmount ELSE 0 END AS fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END AS "
                + "fltProvincialTaxAmount, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, bitChargePST FROM "
                + "tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToPurchaseInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToPurchaseInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnCurrentOpenInvoices(int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentOpenInvoices";
            //TODO: Update ReturnCurrentOpenInvoice
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime "
                + "AS DATETIME) AS dtmInvoiceTime, intCustomerID, intEmployeeID, intLocationID, fltSubTotal, fltShippingCharges, "
                + "fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount ELSE 0 END AS "
                + "fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END AS "
                + "fltProvincialTaxAmount, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, "
                + "bitChargePST FROM tbl_currentSalesInvoice WHERE intLocationID = @intLocationID AND intTransactionTypeID = 1";

            object[][] parms = 
            {
                new object[] { "@intLocationID", locationID }
            };

            return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Returns list of invoices based on an invoice string
        public List<Invoice> ReturnInvoiceByCustomers(int customerID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceByCustomers";
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime "
                + "AS DATETIME) AS dtmInvoiceTime, intCustomerID, intEmployeeID, intLocationID, fltSubTotal, fltShippingCharges, "
                + "fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount ELSE 0 END AS "
                + "fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END AS "
                + "fltProvincialTaxAmount, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, "
                + "bitChargePST FROM tbl_invoice WHERE intCustomerID = @intCustomerID";

            object[][] parms =
            {
                 new object[] { "@intCustomerID", customerID }
            };

            return ConvertFromDataTableInvoiceListByCustomer(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableInvoiceListByCustomer(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Returns list of invoices based on search criteria and date range
        public DataTable ReturnInvoicesBasedOnSearchCriteria(DateTime stDate, DateTime endDate, string searchTxt, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoicesBasedOnSearchCriteria";
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            ArrayList strText = new ArrayList();

            string sqlCmd = "SELECT I.intInvoiceID, I.varInvoiceNumber, I.intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime AS DATETIME) AS dtmInvoiceTime, "
                + "CONCAT(C.varLastName, ', ', C.varFirstName) AS customerName, CONCAT(E.varLastName, ', ', E.varFirstName) AS employeeName, I.intLocationID, "
                + "fltSubTotal, fltShippingCharges, fltTotalDiscount, fltTotalTradeIn, CASE WHEN bitChargeGST = 1 THEN fltGovernmentTaxAmount ELSE 0 END AS "
                + "fltGovernmentTaxAmount, CASE WHEN bitChargePST = 1 THEN fltProvincialTaxAmount ELSE 0 END AS fltProvincialTaxAmount, MP.varPaymentName, "
                + "IM.fltAmountPaid, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, bitChargePST FROM tbl_invoice I JOIN "
                + "tbl_invoiceMOP IM ON IM.intInvoiceID = I.intInvoiceID JOIN tbl_customers C ON C.intCustomerID = I.intCustomerID JOIN tbl_employee E ON "
                + "E.intEmployeeID = I.intEmployeeID JOIN tbl_methodOfPayment MP ON MP.intPaymentID = IM.intPaymentID WHERE (";

            if (searchTxt != "")
            {
                for (int i = 0; i < searchTxt.Split(' ').Length; i++)
                {
                    strText.Add(searchTxt.Split(' ')[i]);
                }
                sqlCmd += " I.intInvoiceID IN (SELECT DISTINCT intInvoiceID FROM tbl_invoiceItem WHERE varInvoiceNumber LIKE '%" + searchTxt + "%' OR "
                    + "intInventoryID IN (";
                sqlCmd += IIM.ReturnStringSearchForAccessories(strText);
                sqlCmd += " UNION ";
                sqlCmd += IIM.ReturnStringSearchForClothing(strText);
                sqlCmd += " UNION ";
                sqlCmd += IIM.ReturnStringSearchForClubs(strText) + "))) AND I.intLocationID = @intLocationID";
            }
            else
            {
                sqlCmd += " dtmInvoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "') AND I.intLocationID = @intLocationID";
            }            
            object[][] parms =
            {
                new object[] { "@intLocationID", locationID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnInvoicesBasedOnSearchForReturns(string txtSearch, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoicesBasedOnSearchForReturns";
            string sqlCmd = "SELECT I.intInvoiceID, I.varInvoiceNumber, I.intInvoiceSubNumber, I.dtmInvoiceDate, I.intCustomerID, I.intLocationID, "
                + "(I.fltBalanceDue + CASE WHEN I.bitChargeGST = 1 THEN I.fltGovernmentTaxAmount ELSE 0 END + CASE WHEN I.bitChargePST = 1 THEN "
                + "I.fltProvincialTaxAmount ELSE 0 END) AS fltBalanceDue FROM tbl_invoice I JOIN tbl_customers C ON I.intCustomerID = C.intCustomerID "
                + "WHERE I.intInvoiceSubNumber = 1 AND (I.dtmInvoiceDate = @selectedDate";

            if (txtSearch != "")
            {
                sqlCmd += " OR CAST(I.varInvoiceNumber AS VARCHAR) LIKE '%" + txtSearch + "%' OR "
                + "CONCAT(C.varFirstName, C.varLastName, C.varContactNumber) LIKE '%" + txtSearch + "%'";
            }
            sqlCmd += ") ORDER BY I.varInvoiceNumber DESC";
            object[][] parms =
            {
                 new object[] { "@selectedDate", selectedDate }
            };
            return ConvertFromDataTableToInvoiceForReturns(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToInvoiceForReturns(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }

        public int CalculateNextInvoiceSubNum(string invoiceNumber, object[] objPageDetails)
        {
            string strQueryName = "CalculateNextInvoiceSubNum";
            string sqlCmd = "SELECT MAX(intInvoiceSubNumber) AS intInvoiceSubNumber FROM tbl_invoice WHERE varInvoiceNumber = @varInvoiceNumber";

            object[][] parms = 
            {
                new object[] { "@varInvoiceNumber", invoiceNumber }
            };
            //Return the invoice sub num
            return ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) + 1;
        }
        public void CalculateNewInvoiceTotalsToUpdate(Invoice invoice, object[] objPageDetails)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotals(invoice, objPageDetails), objPageDetails);
        }
        public void CalculateNewInvoiceReturnTotalsToUpdate(Invoice invoice, object[] objPageDetails)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotalsForReturn(invoice), objPageDetails);
        }
        public void CalculateNewReceiptTotalsToUpdate(Invoice invoice, object[] objPageDetails)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllReceiptTotals(invoice), objPageDetails);
        }
        public void FinalizeInvoice(Invoice invoice, string tbl, object[] objPageDetails)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertInvoiceIntoFinalTable(invoice, objPageDetails);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(invoice, objPageDetails);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertInvoiceItemsIntoFinalItemsTable(invoice, tbl, objPageDetails);

            //Step 4: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(invoice, objPageDetails);

            //Step 5: Save New Invoice Mops to the Final Invoice Mops Table
            InsertInvoiceMopsIntoFinalMopsTable(invoice, objPageDetails);

            //Step 6: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentMopsTable(invoice, objPageDetails);
        }
        //Final Invoice move
        private void InsertInvoiceIntoFinalTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceIntoFinalTable";
            string sqlCmd = "INSERT INTO tbl_invoice VALUES(@intInvoiceID, @varInvoiceNumber, @intInvoiceSubNumber, @dtmInvoiceDate, "
                + "@dtmInvoiceTime, @intCustomerID, @intEmployeeID, @intLocationID, @fltSubTotal, @fltShippingCharges, @fltTotalDiscount, "
                + "@fltTotalTradeIn, @fltGovernmentTaxAmount, @fltProvincialTaxAmount, @fltBalanceDue, @intTransactionTypeID, "
                + "@varAdditionalInformation, @bitChargeGST, @bitChargePST)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID },
                new object[] { "@varInvoiceNumber", invoice.varInvoiceNumber },
                new object[] { "@intInvoiceSubNumber", invoice.intInvoiceSubNumber },
                new object[] { "@dtmInvoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intLocationID", invoice.location.intLocationID },
                new object[] { "@fltSubTotal", invoice.fltSubTotal },
                new object[] { "@fltShippingCharges", invoice.fltShippingCharges },
                new object[] { "@fltTotalDiscount", invoice.fltTotalDiscount },
                new object[] { "@fltTotalTradeIn", invoice.fltTotalTradeIn },
                new object[] { "@fltGovernmentTaxAmount", invoice.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", invoice.fltProvincialTaxAmount },
                new object[] { "@fltBalanceDue", invoice.fltBalanceDue + invoice.fltShippingCharges },
                new object[] { "@intTransactionTypeID", 1 },
                new object[] { "@varAdditionalInformation", invoice.varAdditionalInformation },
                new object[] { "@bitChargeGST", invoice.bitChargeGST},
                new object[] { "@bitChargePST", invoice.bitChargePST}
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveInvoiceFromTheCurrentInvoiceTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceFromTheCurrentInvoiceTable";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";
            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Final Items Move
        private void InsertInvoiceItemsIntoFinalItemsTable(Invoice invoice, string tbl, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceItemsIntoFinalItemsTable";
            //InvoiceItemsManager IIM = new InvoiceItemsManager();
            //DataTable dt = IIM.ReturnItemsInTheCart("-" + I.invoiceNum.ToString() + "-" + I.invoiceSub.ToString());
            foreach (InvoiceItems item in invoice.invoiceItems)
            {
                string sqlCmd = "INSERT INTO " + tbl + " VALUES(@intInvoiceItemID, @intInvoiceID, @intInventoryID, @intItemQuantity, @fltItemCost, "
                    + "@fltItemPrice, @fltItemDiscount, @fltItemRefund, @bitIsDiscountPercent, @varItemDescription, @intItemTypeID, @bitIsClubTradeIn)";

                object[][] parms =
                {
                    new object[] { "@intInvoiceItemID", item.intInvoiceItemID },
                    new object[] { "@intInvoiceID", item.intInvoiceID },
                    new object[] { "@intInventoryID", item.intInventoryID },
                    new object[] { "@intItemQuantity", item.intItemQuantity },
                    new object[] { "@fltItemCost", item.fltItemCost },
                    new object[] { "@fltItemPrice", item.fltItemPrice },
                    new object[] { "@fltItemDiscount", item.fltItemDiscount },
                    new object[] { "@fltItemRefund", item.fltItemRefund },
                    new object[] { "@bitIsDiscountPercent", item.bitIsDiscountPercent },
                    new object[] { "@varItemDescription", item.varItemDescription },
                    new object[] { "@intItemTypeID", item.intItemTypeID },
                    new object[] { "@bitIsClubTradeIn", item.bitIsClubTradeIn }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void RemoveInvoiceItemsFromTheCurrentItemsTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceItemsFromTheCurrentItemsTable";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Final Mops move
        private void InsertInvoiceMopsIntoFinalMopsTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceMopsIntoFinalMopsTable";
            foreach (InvoiceMOPs payment in invoice.invoiceMops)
            {
                string sqlCmd = "INSERT INTO tbl_invoiceMOP VALUES(@intInvoicePaymentID, @intInvoiceID, @intPaymentID, "
                    + "@fltAmountPaid, @fltTenderedAmount, @fltCustomerChange)";

                object[][] parms =
                {
                    new object[] { "@intInvoicePaymentID", payment.intInvoicePaymentID },
                    new object[] { "@intInvoiceID", payment.intInvoiceID },
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@fltAmountPaid", payment.fltAmountPaid },
                    new object[] { "@fltTenderedAmount", payment.fltTenderedAmount },
                    new object[] { "@fltCustomerChange", payment.fltCustomerChange }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void RemoveInvoiceMopsFromTheCurrentMopsTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceMopsFromTheCurrentMopsTable";
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceMopsFromTheCurrentReceiptMopsTable";
            string sqlCmd = "DELETE tbl_currentPurchaseMops WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", invoice.intInvoiceID }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //TODO: Update CreateInitialTotalsForTable
        public List<Invoice> CreateInitialTotalsForTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "CreateInitialTotalsForTable";
            string sqlCmd = "INSERT INTO tbl_currentSalesInvoice VALUES(@varInvoiceNumber, @intInvoiceSubNumber, "
                + "@dtmInvoiceDate, @dtmInvoiceTime, @intCustomerID, @intEmployeeID, @intLocationID, @fltSubTotal, "
                + "@fltShippingCharges, @fltTotalDiscount, @fltTotalTradeIn, @fltGovernmentTaxAmount, "
                + "@fltProvincialTaxAmount, @fltBalanceDue, @intTransactionTypeID, @varAdditionalInformation, "
                + "@bitChargeGST, @bitChargePST)";

            object[][] parms =
            {
                new object[] { "@varInvoiceNumber", invoice.varInvoiceNumber },
                new object[] { "@intInvoiceSubNumber", invoice.intInvoiceSubNumber },
                new object[] { "@dtmInvoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intLocationID", invoice.location.intLocationID },
                new object[] { "@fltSubTotal", 0 },
                new object[] { "@fltShippingCharges", 0 },
                new object[] { "@fltTotalDiscount", 0 },
                new object[] { "@fltTotalTradeIn", 0 },
                new object[] { "@fltGovernmentTaxAmount", invoice.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", invoice.fltProvincialTaxAmount },
                new object[] { "@fltBalanceDue", 0 },
                new object[] { "@intTransactionTypeID", invoice.intTransactionTypeID },
                new object[] { "@varAdditionalInformation", "" },
                new object[] { "@bitChargeGST", invoice.bitChargeGST },
                new object[] { "@bitChargePST", invoice.bitChargePST }
            };

            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            return ReturnCurrentInvoiceFromParms(parms, objPageDetails);
        }

        private List<Invoice> ReturnCurrentInvoiceFromParms(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentInvoiceFromParms";
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, intInvoiceSubNumber, dtmInvoiceDate, CAST(dtmInvoiceTime AS DATETIME) AS dtmInvoiceTime, "
                + "intCustomerID, intEmployeeID, intLocationID, fltSubTotal, fltShippingCharges, fltTotalDiscount, fltTotalTradeIn, fltGovernmentTaxAmount, "
                + "fltProvincialTaxAmount, fltBalanceDue, intTransactionTypeID, varAdditionalInformation, bitChargeGST, bitChargePST FROM "
                + "tbl_currentSalesInvoice WHERE varInvoiceNumber = @varInvoiceNumber AND intInvoiceSubNumber = @intInvoiceSubNumber AND dtmInvoiceDate = "
                + "@dtmInvoiceDate AND dtmInvoiceTime = @dtmInvoiceTime AND intCustomerID = @intCustomerID AND intEmployeeID = @intEmployeeID AND "
                + "intLocationID = @intLocationID AND fltSubTotal = @fltSubTotal AND fltShippingCharges = @fltShippingCharges AND fltTotalDiscount = "
                + "@fltTotalDiscount AND fltTotalTradeIn = @fltTotalTradeIn AND fltGovernmentTaxAmount = @fltGovernmentTaxAmount AND fltProvincialTaxAmount "
                + "= @fltProvincialTaxAmount AND fltBalanceDue = @fltBalanceDue AND intTransactionTypeID = @intTransactionTypeID AND "
                + "varAdditionalInformation = @varAdditionalInformation AND bitChargeGST = @bitChargeGST AND bitChargePST = @bitChargePST";

            return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
        }

        public bool ReturnBolInvoiceExists(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnBolInvoiceExists";
            bool exists = false;
            string sqlCmd = "SELECT COUNT(intInvoiceID) AS invoiceCount FROM tbl_currentSalesInvoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            if(ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                exists = true;
            }
            return exists;
        }
        //TODO:Update UpdateCurentInvoice
        public void UpdateCurrentInvoice(Invoice invoice, object[] objPageDetails) 
        {
            string strQueryName = "UpdateCurrentInvoice";
            string sqlCmd = "UPDATE tbl_currentSalesInvoice SET dtmInvoiceDate = @dtmInvoiceDate, dtmInvoiceTime = @dtmInvoiceTime, "
                + "intCustomerID = @intCustomerID, intEmployeeID = @intEmployeeID, intLocationID = @intLocationID, fltSubTotal = "
                + "@fltSubTotal, fltShippingCharges = @fltShippingCharges, fltTotalDiscount = @fltTotalDiscount, fltTotalTradeIn = "
                + "@fltTotalTradeIn, fltGovernmentTaxAmount = @fltGovernmentTaxAmount, fltProvincialTaxAmount = "
                + "@fltProvincialTaxAmount, fltBalanceDue = @fltBalanceDue, intTransactionTypeID = @intTransactionTypeID, "
                + "varAdditionalInformation = @varAdditionalInformation, bitChargeGST = @bitChargeGST, bitChargePST = @bitChargePST "
                + "WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID },
                new object[] { "@dtmInvoiceDate", invoice.dtmInvoiceDate },
                new object[] { "@dtmInvoiceTime", invoice.dtmInvoiceTime },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intLocationID", invoice.location.intLocationID },
                new object[] { "@fltSubTotal", invoice.fltSubTotal },
                new object[] { "@fltShippingCharges", invoice.fltShippingCharges },
                new object[] { "@fltTotalDiscount", invoice.fltTotalDiscount },
                new object[] { "@fltTotalTradeIn", invoice.fltTotalTradeIn },
                new object[] { "@fltGovernmentTaxAmount", invoice.fltGovernmentTaxAmount },
                new object[] { "@fltProvincialTaxAmount", invoice.fltProvincialTaxAmount },
                new object[] { "@fltBalanceDue", invoice.fltBalanceDue },
                new object[] { "@intTransactionTypeID", invoice.intTransactionTypeID },
                new object[] { "@varAdditionalInformation", invoice.varAdditionalInformation },
                new object[] { "@bitChargeGST", invoice.bitChargeGST},
                new object[] { "@bitChargePST", invoice.bitChargePST}
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
    	//used in new POS
        public int ReturnNextInvoiceNumber(object[] objPageDetails)
        {
            string strQueryName = "ReturnNextInvoiceNumber";
            string sqlCmd = "SELECT invoiceNum FROM tbl_InvoiceNumbers";
            object[][] parms = { };
            int nextInvoiceNum = ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) + 1;
            //Creates the invoice with the next invoice num
            CreateInvoiceNum(nextInvoiceNum, objPageDetails);
            //Returns the next invoiceNum
            return nextInvoiceNum;
        }
        public string ReturnNextInvoiceNumberForNewInvoice(CurrentUser cu, object[] objPageDetails)
        {
            string sqlCmd = "SELECT CONCAT(varStoreCode, varInvoiceCode, CASE WHEN LEN(CAST(intSetInvoiceNumber AS INT)) < 6 THEN "
                + "RIGHT(RTRIM('000000' + CAST(intSetInvoiceNumber AS VARCHAR(6))),6) ELSE CAST(intSetInvoiceNumber AS VARCHAR(MAX)) "
                + "END) AS varInvoiceNumber FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };
            CreateInvoiceNum(cu.location.intLocationID, objPageDetails);
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }


        public bool invoiceIsReturn(int invoiceID)
        {
            bool isReturn = false;
            if (InvoiceSubNumberCheck(invoiceID) > 1)
            {
                isReturn = true;
            }
            return isReturn;
        }
        private int InvoiceSubNumberCheck(int invoiceID)
        {
            string strQueryName = "InvoiceSubNumberCheck";
            string sqlCmd = "SELECT intInvoiceSubNumber FROM tbl_invoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }





        private void CreateInvoiceNum(int locationID, object[] objPageDetails)
        {
            string strQueryName = "CreateInvoiceNum";
            string sqlCmd = "UPDATE tbl_storedStoreNumbers SET intSetInvoiceNumber = intSetInvoiceNumber + 1 WHERE intLocationID = @intLocationID";
            object[][] parms =
            {
                new object[] { "@intLocationID", locationID }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ReturnTransactionName(int transactionTypeID, object[] objPageDetails)
        {
            string strQueryName = "ReturnTransactionName";
            string sqlCmd = "SELECT varTransactionName FROM tbl_transactionType WHERE intTransactionTypeID = @intTransactionTypeID";

            object[][] parms =
            {
                new object[] { "@intTransactionTypeID", transactionTypeID }
            };
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public bool VerifyMOPHasBeenAdded(int invoiceID, object[] objPageDetails)
        {
            string strQueryName = "VerifyMOPHasBeenAdded";
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(intInvoicePaymentID) AS intInvoicePaymentID FROM tbl_currentSalesMops "
                + "WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            if(ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }
        public bool VerifyPurchaseMOPHasBeenAdded(int receiptID, object[] objPageDetails)
        {
            string strQueryName = "VerifyPurchaseMOPHasBeenAdded";
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(intReceiptPaymentID) AS intReceiptPaymentIDs FROM tbl_currentPurchaseMops WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };

            if (ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }

        //public List<Invoice> ReturnCurrentReceipt(string invoice, object[] objPageDetails)
        //{
        //    string strQueryName = "ReturnCurrentReceipt";
        //    string sqlCmd = "SELECT receipteNumber, receiptDate, CAST(receiptTime AS DATETIME) "
        //        + "AS receiptTime, custID, empID, locationID, receiptTotal, transactionType, "
        //        + "comments FROM tbl_receipt WHERE receiptNumber = @receiptNum";

        //    object[][] parms =
        //    {
        //         new object[] { "@receiptNum", Convert.ToInt32(invoice.Split('-')[1]) }
        //    };

        //    return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
        //    //return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        //}
        public string ReturnNextReceiptNumber(CurrentUser cu, object[] objPageDetails)
        {
            string strQueryName = "ReturnNextReceiptNumber";
            string sqlCmd = "SELECT CONCAT(varStoreCode, varReceiptCode, LEN(CAST(intSetReceiptNumber AS INT)) < 6 THEN RIGHT(RTRIM('000000' + CAST(intSetReceiptNumber AS "
                + "VARCHAR(6))),6) ELSE CAST(intSetReceiptNumber AS VARCHAR(MAX)) END) AS varReceiptNumber FROM tbl_storedStoreNumbers WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "@intLocationID", cu.location.intLocationID }
            };
            createReceiptNum(cu, objPageDetails);
            //Returns the next invoiceNum
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        public void createReceiptNum(CurrentUser cu, object[] objPageDetails)
        {
            string strQueryName = "createReceiptNum";
            string sqlCmd = "Update tbl_storedStoreNumbers set intSetReceiptNumber = intSetReceiptNumber + 1 WHERE intLocationID = @intLocationID";

            object[][] parms =
            {
                new object[] { "intLocationID", cu.location.intLocationID }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CancellingReceipt(Invoice receipt, object[] objPageDetails)
        {
            //Step 2: Remove Receipt from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(receipt, objPageDetails);

            //Step 4: Remove Receipt Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(receipt, objPageDetails);

            //Step 6: Remove Receipt Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(receipt, objPageDetails);
        }
        public void FinalizeReceipt(Invoice invoice, string table, object[] objPageDetails)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertReceiptIntoFinalTable(invoice, objPageDetails);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(invoice, objPageDetails);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertReceiptItemsIntoFinalItemsTable(invoice, table, objPageDetails);

            //Step 4: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(invoice, objPageDetails);

            //Step 5: Save New Invoice Mops to the Final Invoice Mops Table
            InsertReceiptMopsIntoFinalMopsTable(invoice, objPageDetails);

            //Step 6: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(invoice, objPageDetails);
        }
        private void InsertReceiptIntoFinalTable(Invoice receipt, object[] objPageDetails)
        {
            string strQueryName = "InsertReceiptIntoFinalTable";
            string sqlCmd = "INSERT INTO tbl_receipt VALUES(@intReceiptID, @varReceiptNumber @dtmReceiptDate, @dtmReceiptTime, "
                + "@intCustomerID, @intEmployeeID, @intLocationID, @fltReceiptTotal, @intTransactionTypeID, @varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intInvoiceID },
                new object[] { "@varReceiptNumber", receipt.varInvoiceNumber },
                new object[] { "@dtmReceiptDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@dtmReceiptTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", receipt.customer.intCustomerID },
                new object[] { "@intEmployeeID", receipt.employee.intEmployeeID },
                new object[] { "@intLocationID", receipt.location.intLocationID },
                new object[] { "@fltReceiptTotal", receipt.fltBalanceDue },
                new object[] { "@intTransactionTypeID", 5 },
                new object[] { "@varAdditionalInformation", receipt.varAdditionalInformation }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void InsertReceiptItemsIntoFinalItemsTable(Invoice invoice, string tbl, object[] objPageDetails)
        {
            string strQueryName = "InsertReceiptItemsIntoFinalItemsTable";
            foreach (InvoiceItems item in invoice.invoiceItems)
            {
                string sqlCmd = "INSERT INTO " + tbl + " VALUES(@intReceiptItemID, @intReceiptID, @intInventoryID, @varItemQuantity, "
                        + "@varItemDescription, @fltItemCost)";

                object[][] parms =
                {
                    new object[] { "@intReceiptItemID", item.intInvoiceItemID },
                    new object[] { "@intReceiptID", item.intInvoiceID },
                    new object[] { "@intInventoryID", item.intInventoryID },
                    new object[] { "@intItemQuantity", item.intItemQuantity },
                    new object[] { "@varItemDescription", item.varItemDescription },
                    new object[] { "@fltItemCost", item.fltItemCost }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void InsertReceiptMopsIntoFinalMopsTable(Invoice receipt, object[] objPageDetails)
        {
            string strQueryName = "InsertReceiptMopsIntoFinalMopsTable";
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            foreach (InvoiceMOPs payment in receipt.invoiceMops)
            {
                string sqlCmd = "INSERT INTO tbl_receiptMOP VALUES(@intReceiptPaymentID, @intReceiptID, @intPaymentID, "
                    + "@intChequeNumber, @fltAmountPaid)";

                object[][] parms =
                {
                    new object[] { "@intReceiptPaymentID", payment.intInvoicePaymentID },
                    new object[] { "@intReceiptID", payment.intInvoiceID },
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@intChequeNumber", payment.intChequeNumber },
                    new object[] { "@fltAmountPaid", payment.fltAmountPaid }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }

        public object[] ReturnTotalsForTenderAndChange(Invoice invoice)
        {
            double tender = 0;
            double change = 0;

            foreach(InvoiceMOPs invoicePayment in invoice.invoiceMops)
            {
                tender += invoicePayment.fltTenderedAmount;
                change += invoicePayment.fltCustomerChange;
            }
            object[] amounts = { tender, change };
            return amounts;
        }

        public DataTable ReturnSeatedTables()
        {
            string sqlCmd = "SELECT varTableButton, varSeatNumber FROM tbl_LoungeTableSeatCombination";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public List<Invoice> GatherInvoicesFromTable(string pressedBTN, object[] objPageDetails)
        {
            string[] tableSeat = SplitStringofTableSeat(pressedBTN);
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, custID, empID, "
                + "locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue, "
                + "transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice CSI JOIN tbl_LoungeTableSeatCombination "
                + "LTSC ON LTSC.currentSalesIID = CSI.currentSalesIID WHERE LTSC.tableButton = @pressedTable OR LTSC.seatNumber = "
                + "@pressedSeat";
            object[][] parms =
            {
                new object[] { "@pressedTable", tableSeat[0] },
                new object[] { "@pressedSeat", tableSeat[1] }
            };
            return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
        }
        public void CreateNewInvoiceAtTable(string pressedBTN, CurrentUser cu, object[] objPageDetails)
        {
            int invoiceNum = InsertNewInvoiceAtTable(pressedBTN, cu, objPageDetails);
            int currentSalesIID = GatherCurrentSalesIID(invoiceNum);
            InsertNewInvoiceLoungeTableSeatCombination(pressedBTN, currentSalesIID);
        }
        private int InsertNewInvoiceAtTable(string pressedBTN, CurrentUser cu, object[] objPageDetails)
        {
            int invoiceNum = ReturnNextInvoiceNumber(objPageDetails);
            string sqlCmd = "INSERT INTO tbl_currentSalesInvoice VALUES(@invoiceNum, @invoiceSubNum, @invoiceDate, @invoiceTime, @custID, "
                + "@empID, @locationID, @subTotal, @shippingAmount, @discountAmount, @tradeinAmount, @governmentTax, @provincialTax, "
                + "@balanceDue, @transactionType, @comments, @chargeGST, @chargePST)";
            object[][] parms =
            {
                new object[] { "@invoiceNum", invoiceNum },
                new object[] { "@invoiceSubNum", 20 },
                new object[] { "@invoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@invoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@custID", 1 },
                new object[] { "@empID", cu.employee.intEmployeeID },
                new object[] { "@locationID", cu.location.intLocationID },
                new object[] { "@subTotal", 0 },
                new object[] { "@shippingAmount", 0 },
                new object[] { "@discountAmount", 0 },
                new object[] { "@tradeinAmount", 0 },
                new object[] { "@governmentTax", 0 },
                new object[] { "@provincialTax", 0 },
                new object[] { "@balanceDue", 0 },
                new object[] { "@transactionType", 1 },
                new object[] { "@comments", "" },
                new object[] { "@chargeGST", true },
                new object[] { "@ChargePST", true }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
            return invoiceNum;
        }
        private void InsertNewInvoiceLoungeTableSeatCombination(string pressedBTN, int currentSalesIID)
        {
            string[] tableSeat = SplitStringofTableSeat(pressedBTN);
            string sqlCmd = "INSERT INTO tbl_LoungeTableSeatCombination VALUES(@currentSalesIID, @tableButton, @seatNumber)";
            object[][] parms =
            {
                new object[] { "@currentSalesIID", currentSalesIID },
                new object[] { "@tableButton", tableSeat[0] },
                new object[] { "@seatNumber", tableSeat[1] }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private int GatherCurrentSalesIID(int invoiceNum)
        {
            string sqlCmd = "SELECT currentSalesIID FROM tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum";
            object[][] parms =
            {
                new object[] { "@invoiceNum", invoiceNum }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        private string[] SplitStringofTableSeat(string pressedBTN)
        {
            int splitNumber = 0;
            if (pressedBTN.Contains("Seat"))
            {
                splitNumber = pressedBTN.IndexOf("Seat");
            }
            else if (pressedBTN.Contains("Player"))
            {
                splitNumber = pressedBTN.IndexOf("Player");
            }
            else
            {
                splitNumber = pressedBTN.Length;
                if (pressedBTN.Contains("Table"))
                {
                    pressedBTN += "SeatOne";
                }
                else if (pressedBTN.Contains("Sim"))
                {
                    pressedBTN += "PlayerOne";
                }
            }
            string[] returnStrings = { pressedBTN.Substring(0, splitNumber), pressedBTN.Substring(splitNumber, pressedBTN.Length - pressedBTN.Substring(0, splitNumber).Length) };
            return returnStrings;
        }
    }
}