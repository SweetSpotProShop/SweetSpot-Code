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


        //Returns list of invoice based on an invoice string
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
        //Returns list of invoice based on an invoice string
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
                + "custID, empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, "
                + "balanceDue, transactionType, comments FROM tbl_invoice WHERE (";

            if (searchTxt != "") {
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

            if (txtSearch != "") {
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