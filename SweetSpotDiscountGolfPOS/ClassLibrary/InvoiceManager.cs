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
                comments = row.Field<string>("comments")
            }).ToList();
            return i;
        }
        private List<Invoice> ConvertFromDataTableToCurrentInvoice(DataTable dt)
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
                usedMops = IMM.ReturnInvoiceMOPsCurrentSale("-" + row.Field<int>("invoiceNum").ToString() + "-" + row.Field<int>("invoiceSubNum").ToString()),
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
                comments = row.Field<string>("comments")
            }).ToList();
            return i;
        }


        //Returns list of invoice based on an invoice string from the Final Table
        public List<Invoice> ReturnInvoice(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_invoice WHERE invoiceNum = @invoiceNum "
                + "AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<Invoice> i = ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }

        //Returns list of invoices based on an invoice string from the Current table
        public List<Invoice> ReturnCurrentInvoice(string invoice)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum "
                + "AND invoiceSubNum = @invoiceSubNum";

            object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[1]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[2]) }
            };

            List<Invoice> i = ConvertFromDataTableToCurrentInvoice(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }
        public List<Invoice> ReturnCurrentOpenInvoices(int locID)
        {
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_currentSalesInvoice "
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
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_invoice WHERE custID = @custID";

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
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_invoice WHERE (";

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
                sqlCmd += IIM.ReturnStringSearchForClubs(strText) + ")) OR ";
            }

            sqlCmd += "invoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "') AND locationID = @locationID";
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
                + "I.custID = C.custID WHERE I.invoiceDate = @selectedDate ";

            if (txtSearch != "")
            {
                sqlCmd += "OR CAST(I.invoiceNum AS VARCHAR) LIKE '%" + txtSearch + "%' OR "
                + "CONCAT(C.firstName, C.lastName, C.primaryPhoneINT) LIKE '%" + txtSearch + "%' ";
            }
            sqlCmd += "ORDER BY I.invoiceNum DESC";
            object[][] parms =
            {
                 new object[] { "@selectedDate", selectedDate }
            };
            return ConvertFromDataTableToInvoiceForReturns(dbc.returnDataTableData(sqlCmd, parms));
        }

        public void CalculateNewInvoiceTotalsToUpdate(Invoice I)
        {
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            SalesCalculationManager SCM = new SalesCalculationManager();
            List<InvoiceItems> ii = IIM.ReturnItemsToCalculateTotals("-" + I.invoiceNum.ToString() + "-" + I.invoiceSub.ToString());
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateCurrentInvoice(SCM.SaveAllInvoiceTotals(ii, I));
        }
        public void FinalizeInvoice(Invoice I, string comments)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertInvoiceIntoFinalTable(I, comments);

            //Step 2: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromTheCurrentInvoiceTable(I);

            //Step 3: Save New Invoice Items to the Final Invoice Items Table
            InsertInvoiceItemsIntoFinalItemsTable(I);

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
                + "@governmentTax, @provincialTax, @balanceDue, @transactionType, @comments)";

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
                new object[] { "@comments", comments }
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
        private void InsertInvoiceItemsIntoFinalItemsTable(Invoice I)
        {
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            DataTable dt = IIM.ReturnItemsInTheCart("-" + I.invoiceNum.ToString() + "-" + I.invoiceSub.ToString());
            foreach (DataRow item in dt.Rows)
            {
                string sqlCmd = "INSERT INTO tbl_invoiceItem VALUES(@invoiceNum, @invoiceSubNum, @sku, @itemQuantity, "
                        + "@itemCost, @itemPrice, @itemDiscount, @itemRefund, @percentage)";

                object[][] parms =
                {
                new object[] { "@invoiceNum", Convert.ToInt32(item["invoiceNum"]) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(item["invoiceSubNum"]) },
                new object[] { "@sku", Convert.ToInt32(item["sku"]) },
                new object[] { "@itemQuantity", Convert.ToInt32(item["itemQuantity"]) },
                new object[] { "@itemCost", Convert.ToDouble(item["itemCost"]) },
                new object[] { "@itemPrice", Convert.ToDouble(item["itemPrice"]) },
                new object[] { "@itemDiscount", Convert.ToDouble(item["itemDiscount"]) },
                new object[] { "@itemRefund", Convert.ToDouble(item["itemRefund"]) },
                new object[] { "@percentage", Convert.ToBoolean(item["percentage"]) }
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
            InvoiceMOPsManager IMM = new InvoiceMOPsManager();
            List<InvoiceMOPs> im = IMM.ReturnInvoiceMOPsCurrentSale("-" + I.invoiceNum.ToString() + "-" + I.invoiceSub.ToString());
            foreach (InvoiceMOPs mop in im)
            {
                string sqlCmd = "INSERT INTO tbl_invoiceMOP VALUES(@invoiceNum, @invoiceSubNum, "
                    + "@mopType, @amountPaid)";

                object[][] parms =
                {
                    new object[] { "@invoiceNum", mop.invoiceNum },
                    new object[] { "@invoiceSubNum", mop.invoiceSubNum },
                    new object[] { "@mopType", mop.mopType },
                    new object[] { "@amountPaid", mop.amountPaid }
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

        //public List<Invoice> ReturnInvoiceDuringCartTransactions(string invoice)
        //{
        //    string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
        //        + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
        //        + "provincialTax, balanceDue, transactionType, comments FROM tbl_currentSalesInvoice WHERE invoiceNum = @invoiceNum "
        //        + "AND invoiceSubNum = @invoiceSubNum";

        //    object[][] parms =
        //    {
        //         new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
        //         new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
        //    };

        //    List<Invoice> i = ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms));
        //    return i;
        //}
        public void CreateInitialTotalsForTable(object[] invoiceInfo)
        {
            string sqlCmd = "INSERT INTO tbl_currentSalesInvoice VALUES(@invoiceNum, @invoiceSubNum, "
                + "@invoiceDate, @invoiceTime, @custID, @empID, @locationID, 0, 0, 0, 0, 0, 0, 0, 1, '')";

            object[][] parms =
            {
                new object[] { "@invoiceNum", Convert.ToInt32(Convert.ToString(invoiceInfo[0]).Split('-')[1]) },
                new object[] { "@invoiceSubNum", Convert.ToInt32(Convert.ToString(invoiceInfo[0]).Split('-')[2]) },
                new object[] { "@invoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@invoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@custID", Convert.ToInt32(invoiceInfo[1]) },
                new object[] { "@empID", Convert.ToInt32(invoiceInfo[2]) },
                new object[] { "@locationID", Convert.ToInt32(invoiceInfo[3]) }
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
        public void UpdateCurrentInvoice(Invoice I)
        {
            string sqlCmd = "UPDATE tbl_currentSalesInvoice SET invoiceDate = @invoiceDate, invoiceTime = @invoiceTime, custID = @custID, "
                + "empID = @empID, locationID = @locationID, subTotal = @subTotal, shippingAmount = @shippingAmount, "
                + "discountAmount = @discountAmount, tradeinAmount = @tradeinAmount, governmentTax = @governmentTax, "
                + "provincialTax = @provincialTax, balanceDue = @balanceDue, transactionType = @transactionType, comments = @comments WHERE invoiceNum = @invoiceNum "
                + "AND invoiceSubNum = @invoiceSubNum";

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
                new object[] { "@comments", I.comments }
            };
            ExecuteNonReturnCall(sqlCmd, parms);
        }
        public int ReturnNextInvoiceNumber()
        {
            string sqlCmd = "Select invoiceNum from tbl_InvoiceNumbers";
            object[][] parms = { };
            int nextInvoiceNum = ReturnInt(sqlCmd, parms) + 1;
            //Creates the invoice with the next invoice num
            CreateInvoiceNum(nextInvoiceNum);
            //Returns the next invoiceNum
            return nextInvoiceNum;
        }
        private void CreateInvoiceNum(int invNum)
        {
            string sqlCmd = "update tbl_InvoiceNumbers set invoiceNum = @invNum";
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



        //Returns list of invoices based on search criteria and date range
        //public List<Invoice> ReturnInvoicesBasedOnSearchCriteriaV2(DateTime stDate, DateTime endDate, string searchTxt, int locationID)
        //{
        //    InvoiceItemsManager IIM = new InvoiceItemsManager();
        //    ArrayList strText = new ArrayList();

        //    string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, CAST(invoiceTime AS DATETIME) AS invoiceTime, "
        //        + "custID, empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, "
        //        + "balanceDue, transactionType, comments FROM tbl_invoice WHERE ";

        //    if (searchTxt != "" && stDate == endDate)
        //    {
        //        for (int i = 0; i < searchTxt.Split(' ').Length; i++)
        //        {
        //            strText.Add(searchTxt.Split(' ')[i]);
        //        }
        //        sqlCmd += "( invoiceNum IN (SELECT DISTINCT invoiceNum FROM tbl_invoiceItem WHERE "
        //            + "CAST(invoiceNum AS VARCHAR) LIKE '%" + searchTxt + "%' OR sku IN (";
        //        sqlCmd += IIM.ReturnStringSearchForAccessories(strText);
        //        sqlCmd += " UNION ";
        //        sqlCmd += IIM.ReturnStringSearchForClothing(strText);
        //        sqlCmd += " UNION ";
        //        sqlCmd += IIM.ReturnStringSearchForClubs(strText) + ")))";
        //    }
        //    else if (searchTxt != "" && stDate != endDate)
        //    {
        //        for (int i = 0; i < searchTxt.Split(' ').Length; i++)
        //        {
        //            strText.Add(searchTxt.Split(' ')[i]);
        //        }
        //        sqlCmd += "( invoiceNum IN (SELECT DISTINCT invoiceNum FROM tbl_invoiceItem WHERE "
        //            + "CAST(invoiceNum AS VARCHAR) LIKE '%" + searchTxt + "%' OR sku IN (";
        //        sqlCmd += IIM.ReturnStringSearchForAccessories(strText);
        //        sqlCmd += " UNION ";
        //        sqlCmd += IIM.ReturnStringSearchForClothing(strText);
        //        sqlCmd += " UNION ";
        //        sqlCmd += IIM.ReturnStringSearchForClubs(strText) + "))) ";
        //        sqlCmd += "OR invoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "' AND locationID = @locationID";
        //    }
        //    else if (searchTxt == "" && stDate != endDate)
        //    {
        //        sqlCmd += "invoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "' AND locationID = @locationID";
        //    }
        //    else if (searchTxt == "" && stDate == endDate)
        //    {
        //        sqlCmd += "invoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "' AND locationID = @locationID";
        //    }
        //    object[][] parms =
        //    {
        //        new object[] { "locationID", locationID }
        //    };
        //    return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd, parms));
        //}
    }
}