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
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSub = row.Field<int>("invoiceSubNum"),
                invoiceDate = row.Field<DateTime>("invoiceDate"),
                invoiceTime = row.Field<DateTime>("invoiceTime"),
                customer = CM.ReturnCustomer(row.Field<int>("custID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                soldItems = IIM.ReturnInvoiceItems(row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString(), objPageDetails),
                usedMops = IMM.ReturnInvoiceMOPs(row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString(), objPageDetails),
                transactionType = row.Field<int>("transactionType"),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
            }).ToList();
            return i;
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
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSub = row.Field<int>("invoiceSubNum"),
                invoiceDate = row.Field<DateTime>("invoiceDate"),
                invoiceTime = row.Field<DateTime>("invoiceTime"),
                customer = CM.ReturnCustomer(row.Field<int>("custID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                soldItems = IIM.ReturnInvoiceItemsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString(), objPageDetails),
                usedMops = IMM.ReturnInvoiceMOPsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString(), objPageDetails),
                transactionType = row.Field<int>("transactionType"),
                transactionName = ReturnTransactionName(row.Field<int>("transactionType"), objPageDetails),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
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
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSub = row.Field<int>("invoiceSubNum"),
                invoiceDate = row.Field<DateTime>("invoiceDate"),
                invoiceTime = row.Field<DateTime>("invoiceTime"),
                customer = CM.ReturnCustomer(row.Field<int>("custID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                soldItems = IIM.ReturnInvoiceItemsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString(), objPageDetails),
                usedMops = IMM.ReturnPurchaseMOPsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString(), objPageDetails),
                transactionType = row.Field<int>("transactionType"),
                transactionName = ReturnTransactionName(row.Field<int>("transactionType"), objPageDetails),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToReceipt(DataTable dt, object[] objPageDetails)
        {
            CustomerManager CM = new CustomerManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                invoiceNum = row.Field<int>("receiptNumber"),
                invoiceDate = row.Field<DateTime>("receiptDate"),
                invoiceTime = row.Field<DateTime>("receiptTime"),
                customer = CM.ReturnCustomer(row.Field<int>("custID"), objPageDetails)[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                subTotal = row.Field<double>("receiptTotal"),
                soldItems = IIM.ReturnInvoiceItemsReceipt(row.Field<int>("receiptNumber").ToString(), objPageDetails),
                usedMops = IMM.ReturnReceiptMOPsPurchase(row.Field<int>("receiptNumber").ToString(), objPageDetails),
                transactionType = row.Field<int>("transactionType"),
                transactionName = ReturnTransactionName(row.Field<int>("transactionType"), objPageDetails),
                comments = row.Field<string>("comments")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToInvoiceForReturns(DataTable dt, object[] objPageDetails)
        {
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSub = row.Field<int>("invoiceSubNum"),
                invoiceDate = row.Field<DateTime>("invoiceDate"),
                customer = CM.ReturnCustomer(row.Field<int>("custID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                balanceDue = row.Field<double>("balanceDue")
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
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSub = row.Field<int>("invoiceSubNum"),
                invoiceDate = row.Field<DateTime>("invoiceDate"),
                invoiceTime = row.Field<DateTime>("invoiceTime"),
                employee = EM.ReturnEmployee(row.Field<int>("empID"), objPageDetails)[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                transactionType = row.Field<int>("transactionType"),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
            }).ToList();
            return i;
        }


        //Returns list of invoice based on an invoice string from the Final Table
        public List<Invoice> ReturnInvoice(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoice";
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_invoice WHERE "
                + "invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnPurchaseInvoice(string receipt, object[] objPageDetails)
        {
            string strQueryName = "ReturnPurchaseInvoice";
            string sqlCmd = "SELECT receiptNumber, receiptDate, CAST(receiptTime AS DATETIME) AS receiptTime, "
                + "custID, empID, locationID, receiptTotal, transactionType, comments FROM tbl_receipt WHERE "
                + "receiptNumber = @receiptNumber";

            object[][] parms =
            {
                 new object[] { "@receiptNumber", Convert.ToInt32(receipt.Split('-')[0]) }
            };

            return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public Invoice ReturnInvoiceForReturns(string invoice, object[] objPageDetails)
        {
            invoice = invoice.Split('-')[1].ToString() + "-1";
            return ReturnInvoice(invoice, objPageDetails)[0];
        }
        //Returns list of invoices based on an invoice string from the Current table
        public List<Invoice> ReturnCurrentInvoice(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentInvoice";
            //TODO: Update ReturnCurrentInvoice
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };

            return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnCurrentPurchaseInvoice(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentPurchaseInvoice";
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };

            return ConvertFromDataTableToPurchaseInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToPurchaseInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnCurrentOpenInvoices(int locID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentOpenInvoices";
            //TODO: Update ReturnCurrentOpenInvoice
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice "
                + "WHERE locationID = @locID AND transactionType = 1";

            object[][] parms = 
            {
                new object[] { "@locID", locID }
            };

            return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Returns list of invoices based on an invoice string
        public List<Invoice> ReturnInvoiceByCustomers(int custNum, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoiceByCustomers";
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_invoice WHERE custID = @custID";

            object[][] parms =
            {
                 new object[] { "@custID", custNum }
            };

            return ConvertFromDataTableInvoiceListByCustomer(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableInvoiceListByCustomer(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Returns list of invoices based on search criteria and date range
        public List<Invoice> ReturnInvoicesBasedOnSearchCriteria(DateTime stDate, DateTime endDate, string searchTxt, int locationID, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoicesBasedOnSearchCriteria";
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            ArrayList strText = new ArrayList();

            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, "
                + "CASE WHEN chargeGST = 1 THEN governmentTax ELSE 0 END AS governmentTax, "
                + "CASE WHEN chargePST = 1 THEN provincialTax ELSE 0 END AS provincialTax, "
                + "balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_invoice WHERE (";

            if (searchTxt != "")
            {
                for (int i = 0; i < searchTxt.Split(' ').Length; i++)
                {
                    strText.Add(searchTxt.Split(' ')[i]);
                }
                sqlCmd += " invoiceNum IN (SELECT DISTINCT invoiceNum FROM tbl_invoiceItem WHERE "
                    + "CAST(invoiceNum AS VARCHAR) LIKE '%" + searchTxt + "%' OR sku IN (";
                sqlCmd += IIM.ReturnStringSearchForAccessories(strText);
                sqlCmd += " UNION ";
                sqlCmd += IIM.ReturnStringSearchForClothing(strText);
                sqlCmd += " UNION ";
                sqlCmd += IIM.ReturnStringSearchForClubs(strText) + "))) AND locationID = @locationID";
            }
            else
            {
                sqlCmd += " invoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "') AND locationID = @locationID";
            }            
            object[][] parms =
            {
                new object[] { "locationID", locationID }
            };
            return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public List<Invoice> ReturnInvoicesBasedOnSearchForReturns(string txtSearch, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "ReturnInvoicesBasedOnSearchForReturns";
            string sqlCmd = "SELECT I.invoiceNum, I.invoiceSubNum, I.invoiceDate, C.custID, C.firstName, "
                + "C.lastName, I.locationID, (I.balanceDue + CASE WHEN I.chargeGST = 1 THEN I.governmentTax ELSE 0 END + "
                + "CASE WHEN I.chargePST = 1 THEN I.provincialTax ELSE 0 END) AS balanceDue FROM tbl_invoice I JOIN tbl_customers C ON "
                + "I.custID = C.custID WHERE I.invoiceSubNum = 1 AND (I.invoiceDate = @selectedDate";

            if (txtSearch != "")
            {
                sqlCmd += " OR CAST(I.invoiceNum AS VARCHAR) LIKE '%" + txtSearch + "%' OR "
                + "CONCAT(C.firstName, C.lastName, C.primaryPhoneINT) LIKE '%" + txtSearch + "%'";
            }
            sqlCmd += ") ORDER BY I.invoiceNum DESC";
            object[][] parms =
            {
                 new object[] { "@selectedDate", selectedDate }
            };
            return ConvertFromDataTableToInvoiceForReturns(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToInvoiceForReturns(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }

        public int CalculateNextInvoiceSubNum(int invoiceNum, object[] objPageDetails)
        {
            string strQueryName = "CalculateNextInvoiceSubNum";
            string sqlCmd = "SELECT MAX(invoiceSubNum) AS invoiceSubNum FROM tbl_invoice WHERE invoiceNum = @invoiceNum";

            object[][] parms = 
            {
                new object[] { "@invoiceNum", invoiceNum }
            };
            //Return the invoice sub num
            return ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) + 1;
        }
        public void CalculateNewInvoiceTotalsToUpdate(Invoice I, object[] objPageDetails)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotals(I, objPageDetails), objPageDetails);
        }
        public void CalculateNewInvoiceReturnTotalsToUpdate(Invoice I, object[] objPageDetails)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotalsForReturn(I), objPageDetails);
        }
        public void CalculateNewReceiptTotalsToUpdate(Invoice I, object[] objPageDetails)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllReceiptTotals(I), objPageDetails);
        }
        public void FinalizeInvoice(Invoice I, string comments, string tbl, object[] objPageDetails)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertInvoiceIntoFinalTable(I, comments, objPageDetails);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(I, objPageDetails);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertInvoiceItemsIntoFinalItemsTable(I, tbl, objPageDetails);

            //Step 4: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(I, objPageDetails);

            //Step 5: Save New Invoice Mops to the Final Invoice Mops Table
            InsertInvoiceMopsIntoFinalMopsTable(I, objPageDetails);

            //Step 6: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentMopsTable(I, objPageDetails);
        }
        //Final Invoice move
        private void InsertInvoiceIntoFinalTable(Invoice I, string comments, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceIntoFinalTable";
            string sqlCmd = "INSERT INTO tbl_invoice VALUES(@invoiceNum, @invoiceSubNum, @invoiceDate, @invoiceTime, "
                + "@custID, @empID, @locationID, @subtotal, @shippingAmount, @discountAmount, @tradeinAmount, "
                + "@governmentTax, @provincialTax, @balanceDue, @transactionType, @comments, @chargeGST, @chargePST)";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub },
                new object[] { "@invoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@invoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@custID", I.customer.customerId },
                new object[] { "@empID", I.employee.employeeID },
                new object[] { "@locationID", I.location.locationID },
                new object[] { "@subtotal", I.subTotal },
                new object[] { "@shippingAmount", I.shippingAmount },
                new object[] { "@discountAmount", I.discountAmount },
                new object[] { "@tradeinAmount", I.tradeinAmount },
                new object[] { "@governmentTax", I.governmentTax },
                new object[] { "@provincialTax", I.provincialTax },
                new object[] { "@balanceDue", I.balanceDue + I.shippingAmount },
                new object[] { "@transactionType", 1 },
                new object[] { "@comments", comments },
                new object[] { "@chargeGST", I.chargeGST},
                new object[] { "@chargePST", I.chargePST}
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveInvoiceFromTheCurrentInvoiceTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceFromTheCurrentInvoiceTable";
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Final Items Move
        private void InsertInvoiceItemsIntoFinalItemsTable(Invoice I, string tbl, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceItemsIntoFinalItemsTable";
            //InvoiceItemsManager IIM = new InvoiceItemsManager();
            //DataTable dt = IIM.ReturnItemsInTheCart("-" + I.invoiceNum.ToString() + "-" + I.invoiceSub.ToString());
            foreach (InvoiceItems item in I.soldItems)
            {
                string sqlCmd = "INSERT INTO " + tbl + " VALUES(@invoiceNum, @invoiceSubNum, @sku, @quantity, "
                        + "@cost, @price, @itemDiscount, @itemRefund, @percentage, @description, @typeID, @isTradeIn)";

                object[][] parms =
                {
                new object[] { "@invoiceNum", item.invoiceNum },
                new object[] { "@invoiceSubNum", item.invoiceSubNum },
                new object[] { "@sku", item.sku },
                new object[] { "@quantity", item.quantity },
                new object[] { "@cost", item.cost },
                new object[] { "@price", item.price },
                new object[] { "@itemDiscount", item.itemDiscount },
                new object[] { "@itemRefund", item.itemRefund },
                new object[] { "@percentage", item.percentage },
                new object[] { "@description", item.description },
                new object[] { "@typeID", item.typeID },
                new object[] { "@isTradeIn", item.isTradeIn }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void RemoveInvoiceItemsFromTheCurrentItemsTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceItemsFromTheCurrentItemsTable";
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Final Mops move
        private void InsertInvoiceMopsIntoFinalMopsTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceMopsIntoFinalMopsTable";
            foreach (InvoiceMOPs mop in I.usedMops)
            {
                string sqlCmd = "INSERT INTO tbl_invoiceMOP VALUES(@invoiceNum, @invoiceSubNum, "
                    + "@mopType, @amountPaid, @tender, @change)";

                object[][] parms =
                {
                    new object[] { "@invoiceNum", mop.invoiceNum },
                    new object[] { "@invoiceSubNum", mop.invoiceSubNum },
                    new object[] { "@mopType", mop.mopType },
                    new object[] { "@amountPaid", mop.amountPaid },
                    new object[] { "@tender", mop.tender},
                    new object[] { "@change", mop.change }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void RemoveInvoiceMopsFromTheCurrentMopsTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceMopsFromTheCurrentMopsTable";
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "RemoveInvoiceMopsFromTheCurrentReceiptMopsTable";
            string sqlCmd = "DELETE tbl_currentPurchaseMops WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                new object[] { "@receiptNum", I.invoiceNum }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //TODO: Update CreateInitialTotalsForTable
        public void CreateInitialTotalsForTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "CreateInitialTotalsForTable";
            string sqlCmd = "INSERT INTO tbl_currentSalesInvoice VALUES(@invoiceNum, @invoiceSubNum, "
                + "@invoiceDate, @invoiceTime, @custID, @empID, @locationID, @subTotal, @shippingAmount, "
                + "@discountAmount, @trdaeinAmount, @governmentTax, @provincialTax, @balanceDue, "
                + "@transactionType, @comments, @chargeGST, @chargePST)";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub },
                new object[] { "@invoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@invoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@custID", I.customer.customerId },
                new object[] { "@empID", I.employee.employeeID },
                new object[] { "@locationID", I.location.locationID },
                new object[] { "@subTotal", I.subTotal },
                new object[] { "@shippingAmount", I.shippingAmount },
                new object[] { "@discountAmount", I.discountAmount },
                new object[] { "@trdaeinAmount", I.tradeinAmount },
                new object[] { "@governmentTax", I.governmentTax },
                new object[] { "@provincialTax", I.provincialTax },
                new object[] { "@balanceDue", I.balanceDue },
                new object[] { "@transactionType", I.transactionType },
                new object[] { "@comments", I.comments },
                new object[] { "@chargeGST", I.chargeGST },
                new object[] { "@chargePST", I.chargePST}
            };

            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public bool ReturnBolInvoiceExists(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnBolInvoiceExists";
            bool exists = false;
            string sqlCmd = "SELECT COUNT(invoiceNum) AS invoiceCount FROM "
                + "tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2].ToString()) }
            };

            if(ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                exists = true;
            }
            return exists;
        }
        //TODO:Update UpdateCurentInvoice
        public void UpdateCurrentInvoice(Invoice I, object[] objPageDetails) 
        {
            string strQueryName = "UpdateCurrentInvoice";
            string sqlCmd = "UPDATE tbl_currentSalesInvoice SET invoiceDate = @invoiceDate, invoiceTime = @invoiceTime, custID = @custID, "
                + "empID = @empID, locationID = @locationID, subTotal = @subTotal, shippingAmount = @shippingAmount, "
                + "discountAmount = @discountAmount, tradeinAmount = @tradeinAmount, governmentTax = @governmentTax, "
                + "provincialTax = @provincialTax, balanceDue = @balanceDue, transactionType = @transactionType, comments = @comments, "
                + "chargeGST = @chargeGST, chargePST = @chargePST WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub },
                new object[] { "@invoiceDate", I.invoiceDate },
                new object[] { "@invoiceTime", I.invoiceTime },
                new object[] { "@custID", I.customer.customerId },
                new object[] { "@empID", I.employee.employeeID },
                new object[] { "@locationID", I.location.locationID },
                new object[] { "@subTotal", I.subTotal },
                new object[] { "@shippingAmount", I.shippingAmount },
                new object[] { "@discountAmount", I.discountAmount },
                new object[] { "@tradeinAmount", I.tradeinAmount },
                new object[] { "@governmentTax", I.governmentTax },
                new object[] { "@provincialTax", I.provincialTax },
                new object[] { "@balanceDue", I.balanceDue },
                new object[] { "@transactionType", I.transactionType },
                new object[] { "@comments", I.comments },
                new object[] { "@chargeGST", I.chargeGST},
                new object[] { "@chargePST", I.chargePST}
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
	//used in new POS
        private void CreateInvoiceNum(int invNum, object[] objPageDetails)
        {
            string strQueryName = "CreateInvoiceNum";
            string sqlCmd = "UPDATE tbl_InvoiceNumbers SET invoiceNum = @invNum";
            object[][] parms =
            {
                new object[] { "@invNum", invNum }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ReturnTransactionName(int tranType, object[] objPageDetails)
        {
            string strQueryName = "ReturnTransactionName";
            string sqlCmd = "SELECT transactionTypeDesc FROM tbl_transactionType WHERE transactionTypeID = @tranType";

            object[][] parms =
            {
                new object[] { "@tranType", tranType }
            };
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
            //return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public bool VerifyMOPHasBeenAdded(string invoice, object[] objPageDetails)
        {
            string strQueryName = "VerifyMOPHasBeenAdded";
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(currentSalesMID) AS currentSalesMID "
                + "FROM tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            if(ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }
        public bool VerifyPurchaseMOPHasBeenAdded(string invoice, object[] objPageDetails)
        {
            string strQueryName = "VerifyPurchaseMOPHasBeenAdded";
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(currentPurchaseMID) AS currentPurchaseMID "
                + "FROM tbl_currentPurchaseMops WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                new object[] { "@receiptNum", invoice.Split('-')[1] }
            };

            if (ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }

        public List<Invoice> ReturnCurrentReceipt(string invoice, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentReceipt";
            string sqlCmd = "SELECT receipteNumber, receiptDate, CAST(receiptTime AS DATETIME) "
                + "AS receiptTime, custID, empID, locationID, receiptTotal, transactionType, "
                + "comments FROM tbl_receipt WHERE receiptNumber = @receiptNum";

            object[][] parms =
            {
                 new object[] { "@receiptNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms), objPageDetails);
            //return ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        public int ReturnNextReceiptNumber(object[] objPageDetails)
        {
            string strQueryName = "ReturnNextReceiptNumber";
            int nextReceiptNum = 0;
            string sqlCmd = "SELECT receiptNumber FROM tbl_receiptNumbers";

            object[][] parms = { };
            nextReceiptNum = ReturnInt(sqlCmd, parms, objPageDetails, strQueryName) + 1;
            //Creates the invoice with the next invoice num
            createReceiptNum(nextReceiptNum, objPageDetails);
            //Returns the next invoiceNum
            return nextReceiptNum;
        }
        public void createReceiptNum(int recNum, object[] objPageDetails)
        {
            string strQueryName = "createReceiptNum";
            string sqlCmd = "Update tbl_receiptNumbers set receiptNumber = @recNum";

            object[][] parms =
            {
                new object[] { "recNum", recNum }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public void CancellingReceipt(Invoice R, object[] objPageDetails)
        {
            //Step 2: Remove Receipt from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(R, objPageDetails);

            //Step 4: Remove Receipt Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(R, objPageDetails);

            //Step 6: Remove Receipt Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(R, objPageDetails);
        }
        public void FinalizeReceipt(Invoice I, string comments, string tbl, object[] objPageDetails)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertReceiptIntoFinalTable(I, comments, objPageDetails);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(I, objPageDetails);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertReceiptItemsIntoFinalItemsTable(I, tbl, objPageDetails);

            //Step 4: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(I, objPageDetails);

            //Step 5: Save New Invoice Mops to the Final Invoice Mops Table
            InsertReceiptMopsIntoFinalMopsTable(I, objPageDetails);

            //Step 6: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(I, objPageDetails);
        }
        private void InsertReceiptIntoFinalTable(Invoice I, string comments, object[] objPageDetails)
        {
            string strQueryName = "InsertReceiptIntoFinalTable";
            string sqlCmd = "INSERT INTO tbl_receipt VALUES(@receiptNumber, @receiptDate, @receiptTime, "
                + "@custID, @empID, @locationID, @receiptTotal, @transactionType, @comments)";

            object[][] parms =
            {
                new object[] { "@receiptNumber", I.invoiceNum },
                new object[] { "@receiptDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@receiptTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@custID", I.customer.customerId },
                new object[] { "@empID", I.employee.employeeID },
                new object[] { "@locationID", I.location.locationID },
                new object[] { "@receiptTotal", I.balanceDue },
                new object[] { "@transactionType", 5 },
                new object[] { "@comments", comments }
            };
            ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void InsertReceiptItemsIntoFinalItemsTable(Invoice I, string tbl, object[] objPageDetails)
        {
            string strQueryName = "InsertReceiptItemsIntoFinalItemsTable";
            foreach (InvoiceItems item in I.soldItems)
            {
                string sqlCmd = "INSERT INTO " + tbl + " VALUES(@receiptNum, @sku, @quantity, "
                        + "@description, @cost)";

                object[][] parms =
                {
                new object[] { "@receiptNum", item.invoiceNum },
                new object[] { "@sku", item.sku },
                new object[] { "@quantity", item.quantity },
                new object[] { "@description", item.description },
                new object[] { "@cost", item.cost }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void InsertReceiptMopsIntoFinalMopsTable(Invoice I, object[] objPageDetails)
        {
            string strQueryName = "InsertReceiptMopsIntoFinalMopsTable";
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            foreach (InvoiceMOPs mop in I.usedMops)
            {
                string sqlCmd = "INSERT INTO tbl_receiptMOP VALUES(@receiptNum, "
                    + "@mopType, @chequeNum, @amountPaid)";

                object[][] parms =
                {
                    new object[] { "@receiptNum", mop.invoiceNum },
                    new object[] { "@mopType", IMM.ReturnMopIntForTable(mop.mopType, objPageDetails) },
                    new object[] { "@chequeNum", mop.cheque },
                    new object[] { "@amountPaid", mop.amountPaid }
                };
                ExecuteNonReturnCall(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }

        public object[] ReturnTotalsForTenderAndChange(Invoice I)
        {
            double tender = 0;
            double change = 0;

            foreach(InvoiceMOPs m in I.usedMops)
            {
                tender += m.tender;
                change += m.change;
            }
            object[] amounts = { tender, change };
            return amounts;
        }
    }
}