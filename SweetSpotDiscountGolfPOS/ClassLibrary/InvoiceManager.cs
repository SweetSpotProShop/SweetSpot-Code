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

        private void ExecuteNonReturnCall(string sqlCmd, object[][] parms)
        {
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private int ReturnInt(string sqlCmd, object[][] parms)
        {
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }
        private List<Invoice> ConvertFromDataTableToInvoice(DataTable dt)
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
                customer = CM.ReturnCustomer(row.Field<int>("custID"))[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"))[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                soldItems = IIM.ReturnInvoiceItems(row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
                usedMops = IMM.ReturnInvoiceMOPs(row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
                transactionType = row.Field<int>("transactionType"),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToCurrentInvoice(DataTable dt)
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
                customer = CM.ReturnCustomer(row.Field<int>("custID"))[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"))[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                soldItems = IIM.ReturnInvoiceItemsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
                usedMops = IMM.ReturnInvoiceMOPsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
                transactionType = row.Field<int>("transactionType"),
                transactionName = ReturnTransactionName(row.Field<int>("transactionType")),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToPurchaseInvoice(DataTable dt)
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
                customer = CM.ReturnCustomer(row.Field<int>("custID"))[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"))[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
                subTotal = row.Field<double>("subTotal"),
                shippingAmount = row.Field<double>("shippingAmount"),
                discountAmount = row.Field<double>("discountAmount"),
                tradeinAmount = row.Field<double>("tradeinAmount"),
                governmentTax = row.Field<double>("governmentTax"),
                provincialTax = row.Field<double>("provincialTax"),
                balanceDue = row.Field<double>("balanceDue"),
                soldItems = IIM.ReturnInvoiceItemsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
                usedMops = IMM.ReturnPurchaseMOPsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
                transactionType = row.Field<int>("transactionType"),
                transactionName = ReturnTransactionName(row.Field<int>("transactionType")),
                comments = row.Field<string>("comments"),
                chargeGST = row.Field<bool>("chargeGST"),
                chargePST = row.Field<bool>("chargePST")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToReceipt(DataTable dt)
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
                customer = CM.ReturnCustomer(row.Field<int>("custID"))[0],
                employee = EM.ReturnEmployee(row.Field<int>("empID"))[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
                subTotal = row.Field<double>("receiptTotal"),
                soldItems = IIM.ReturnInvoiceItemsReceipt(row.Field<int>("receiptNumber").ToString()),
                usedMops = IMM.ReturnReceiptMOPsPurchase(row.Field<int>("receiptNumber").ToString()),
                transactionType = row.Field<int>("transactionType"),
                transactionName = ReturnTransactionName(row.Field<int>("transactionType")),
                comments = row.Field<string>("comments")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToInvoiceForReturns(DataTable dt)
        {
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            List<Invoice> i = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSub = row.Field<int>("invoiceSubNum"),
                invoiceDate = row.Field<DateTime>("invoiceDate"),
                customer = CM.ReturnCustomer(row.Field<int>("custID"))[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
                balanceDue = row.Field<double>("balanceDue")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableInvoiceListByCustomer(DataTable dt)
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
                employee = EM.ReturnEmployee(row.Field<int>("empID"))[0],
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
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
        public List<Invoice> ReturnInvoice(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_invoice WHERE "
                + "invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<Invoice> i = ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        public List<Invoice> ReturnPurchaseInvoice(string receipt)
        {
            string sqlCmd = "SELECT receiptNumber, receiptDate, CAST(receiptTime AS DATETIME) AS receiptTime, "
                + "custID, empID, locationID, receiptTotal, transactionType, comments FROM tbl_receipt WHERE "
                + "receiptNumber = @receiptNumber";

            object[][] parms =
            {
                 new object[] { "@receiptNumber", Convert.ToInt32(receipt.Split('-')[0]) }
            };

            List<Invoice> i = ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        public Invoice ReturnInvoiceForReturns(string invoice)
        {
            invoice = invoice.Split('-')[1].ToString() + "-1";
            return ReturnInvoice(invoice)[0];
        }
        //Returns list of invoices based on an invoice string from the Current table
        public List<Invoice> ReturnCurrentInvoice(string invoice)
        {
            //TODO: Update ReturnCurrentInvoice
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };

            List<Invoice> i = ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        public List<Invoice> ReturnCurrentPurchaseInvoice(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice "
                + "WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };

            List<Invoice> i = ConvertFromDataTableToPurchaseInvoice(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        public List<Invoice> ReturnCurrentOpenInvoices(int locID)
        {
            //TODO: Update ReturnCurrentOpenInvoice
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_currentSalesInvoice "
                + "WHERE locationID = @locID";

            object[][] parms = 
            {
                new object[] { "@locID", locID }
            };
            List<Invoice> i = ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        //Returns list of invoices based on an invoice string
        public List<Invoice> ReturnInvoiceByCustomers(int custNum)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_invoice WHERE custID = @custID";

            object[][] parms =
            {
                 new object[] { "@custID", custNum }
            };

            List<Invoice> i = ConvertFromDataTableInvoiceListByCustomer(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        //Returns list of invoices based on search criteria and date range
        public List<Invoice> ReturnInvoicesBasedOnSearchCriteria(DateTime stDate, DateTime endDate, string searchTxt, int locationID)
        {
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            ArrayList strText = new ArrayList();

            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments, chargeGST, chargePST FROM tbl_invoice WHERE (";

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
            return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms));
        }
        public List<Invoice> ReturnInvoicesBasedOnSearchForReturns(string txtSearch, DateTime selectedDate)
        {
            string sqlCmd = "SELECT I.invoiceNum, I.invoiceSubNum, I.invoiceDate, C.custID, C.firstName, "
                + "C.lastName, I.locationID, I.balanceDue FROM tbl_invoice I JOIN tbl_customers C ON "
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
            return ConvertFromDataTableToInvoiceForReturns(dbc.returnDataTableData(sqlCmd, parms));
        }

        public int CalculateNextInvoiceSubNum(int invoiceNum)
        {
            string sqlCmd = "SELECT MAX(invoiceSubNum) AS invoiceSubNum FROM tbl_invoice WHERE invoiceNum = @invoiceNum";

            object[][] parms = 
            {
                new object[] { "@invoiceNum", invoiceNum }
            };
            //Return the invoice sub num
            return ReturnInt(sqlCmd, parms) + 1;
        }
        public void CalculateNewInvoiceTotalsToUpdate(Invoice I)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotals(I));
        }
        public void CalculateNewInvoiceReturnTotalsToUpdate(Invoice I)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotalsForReturn(I));
        }
        public void CalculateNewReceiptTotalsToUpdate(Invoice I)
        {
            SalesCalculationManager SCM = new SalesCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllReceiptTotals(I));
        }
        public void FinalizeInvoice(Invoice I, string comments, string tbl)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertInvoiceIntoFinalTable(I, comments);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(I);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertInvoiceItemsIntoFinalItemsTable(I, tbl);

            //Step 4: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(I);

            //Step 5: Save New Invoice Mops to the Final Invoice Mops Table
            InsertInvoiceMopsIntoFinalMopsTable(I);

            //Step 6: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentMopsTable(I);
        }
        //Final Invoice move
        private void InsertInvoiceIntoFinalTable(Invoice I, string comments)
        {
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
                new object[] { "@balanceDue", I.balanceDue },
                new object[] { "@transactionType", 1 },
                new object[] { "@comments", comments },
                new object[] { "@chargeGST", I.chargeGST},
                new object[] { "@chargePST", I.chargePST}
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        private void RemoveInvoiceFromTheCurrentInvoiceTable(Invoice I)
        {
            string sqlCmd = "DELETE tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        //Final Items Move
        private void InsertInvoiceItemsIntoFinalItemsTable(Invoice I, string tbl)
        {
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
                ExecuteNonReturnCall(sqlCmd, parms);
            }
        }
        private void RemoveInvoiceItemsFromTheCurrentItemsTable(Invoice I)
        {
            string sqlCmd = "DELETE tbl_currentSalesItems WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        //Final Mops move
        private void InsertInvoiceMopsIntoFinalMopsTable(Invoice I)
        {
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
                ExecuteNonReturnCall(sqlCmd, parms);
            }
        }
        private void RemoveInvoiceMopsFromTheCurrentMopsTable(Invoice I)
        {
            string sqlCmd = "DELETE tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", I.invoiceNum },
                new object[] { "@invoiceSubNum", I.invoiceSub }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        private void RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(Invoice I)
        {
            string sqlCmd = "DELETE tbl_currentPurchaseMops WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                new object[] { "@receiptNum", I.invoiceNum }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        //TODO: Update CreateInitialTotalsForTable
        public void CreateInitialTotalsForTable(Invoice I)
        {
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

            ExecuteNonReturnCall(sqlCmd, parms);
        }
        public bool ReturnBolInvoiceExists(string invoice)
        {
            bool exists = false;
            string sqlCmd = "SELECT COUNT(invoiceNum) AS invoiceCount FROM "
                + "tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1].ToString()) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2].ToString()) }
            };

            if(ReturnInt(sqlCmd, parms) > 0)
            {
                exists = true;
            }
            return exists;
        }
        //TODO:Update UpdateCurentInvoice
        public void UpdateCurrentInvoice(Invoice I) 
        {
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
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        public int ReturnNextInvoiceNumber()
        {
            string sqlCmd = "SELECT invoiceNum FROM tbl_InvoiceNumbers";
            object[][] parms = { };
            int nextInvoiceNum = ReturnInt(sqlCmd, parms) + 1;
            //Creates the invoice with the next invoice num
            CreateInvoiceNum(nextInvoiceNum);
            //Returns the next invoiceNum
            return nextInvoiceNum;
        }
        private void CreateInvoiceNum(int invNum)
        {
            string sqlCmd = "UPDATE tbl_InvoiceNumbers SET invoiceNum = @invNum";
            object[][] parms =
            {
                new object[] { "@invNum", invNum }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        private string ReturnTransactionName(int tranType)
        {
            string sqlCmd = "SELECT transactionTypeDesc FROM tbl_transactionType WHERE transactionTypeID = @tranType";

            object[][] parms =
            {
                new object[] { "@tranType", tranType }
            };
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        public bool VerifyMOPHasBeenAdded(string invoice)
        {
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(currentSalesMID) AS currentSalesMID "
                + "FROM tbl_currentSalesMops WHERE invoiceNum = @invoiceNum AND "
                + "invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                new object[] { "@invoiceNum", invoice.Split('-')[1] },
                new object[] { "@invoiceSubNum", invoice.Split('-')[2] }
            };

            if(ReturnInt(sqlCmd, parms) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }
        public bool VerifyPurchaseMOPHasBeenAdded(string invoice)
        {
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(currentPurchaseMID) AS currentPurchaseMID "
                + "FROM tbl_currentPurchaseMops WHERE receiptNum = @receiptNum";

            object[][] parms =
            {
                new object[] { "@receiptNum", invoice.Split('-')[1] }
            };

            if (ReturnInt(sqlCmd, parms) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }

        public List<Invoice> ReturnCurrentReceipt(string invoice)
        {
            string sqlCmd = "SELECT receipteNumber, receiptDate, CAST(receiptTime AS DATETIME) "
                + "AS receiptTime, custID, empID, locationID, receiptTotal, transactionType, "
                + "comments FROM tbl_receipt WHERE receiptNumber = @receiptNum";

            object[][] parms =
            {
                 new object[] { "@receiptNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<Invoice> i = ConvertFromDataTableToReceipt(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        public int ReturnNextReceiptNumber()
        {
            int nextReceiptNum = 0;
            string sqlCmd = "SELECT receiptNumber FROM tbl_receiptNumbers";

            object[][] parms = { };
            nextReceiptNum = ReturnInt(sqlCmd, parms) + 1;
            //Creates the invoice with the next invoice num
            createReceiptNum(nextReceiptNum);
            //Returns the next invoiceNum
            return nextReceiptNum;
        }
        public void createReceiptNum(int recNum)
        {
            string sqlCmd = "Update tbl_receiptNumbers set receiptNumber = @recNum";

            object[][] parms =
            {
                new object[] { "recNum", recNum }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        public void CancellingReceipt(Invoice R)
        {
            //Step 2: Remove Receipt from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(R);

            //Step 4: Remove Receipt Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(R);

            //Step 6: Remove Receipt Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(R);
        }
        public void FinalizeReceipt(Invoice I, string comments, string tbl)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertReceiptIntoFinalTable(I, comments);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(I);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertReceiptItemsIntoFinalItemsTable(I, tbl);

            //Step 4: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemsFromTheCurrentItemsTable(I);

            //Step 5: Save New Invoice Mops to the Final Invoice Mops Table
            InsertReceiptMopsIntoFinalMopsTable(I);

            //Step 6: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoiceMopsFromTheCurrentReceiptMopsTable(I);
        }
        private void InsertReceiptIntoFinalTable(Invoice I, string comments)
        {
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
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        private void InsertReceiptItemsIntoFinalItemsTable(Invoice I, string tbl)
        {
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
                ExecuteNonReturnCall(sqlCmd, parms);
            }
        }
        private void InsertReceiptMopsIntoFinalMopsTable(Invoice I)
        {
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            foreach (InvoiceMOPs mop in I.usedMops)
            {
                string sqlCmd = "INSERT INTO tbl_receiptMOP VALUES(@receiptNum, "
                    + "@mopType, @chequeNum, @amountPaid)";

                object[][] parms =
                {
                    new object[] { "@receiptNum", mop.invoiceNum },
                    new object[] { "@mopType", IMM.ReturnMopIntForTable(mop.mopType) },
                    new object[] { "@chequeNum", mop.cheque },
                    new object[] { "@amountPaid", mop.amountPaid }
                };
                ExecuteNonReturnCall(sqlCmd, parms);
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