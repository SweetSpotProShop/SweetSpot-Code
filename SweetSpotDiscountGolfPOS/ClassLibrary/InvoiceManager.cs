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
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_invoice WHERE invoiceNum = @invoiceNum "
                + "and invoiceSubNum = @invoiceSubNum";

            Object[][] parms =
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
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, "
                + "custID, empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, "
                + "provincialTax, balanceDue, transactionType, comments FROM tbl_invoice WHERE custID = @custID";

            Object[][] parms =
            {
                 new object[] { "@custID", custNum }
            };

            List<Invoice> i = ConvertFromDataTableInvoiceListByCustomer(dbc.returnDataTableData(sqlCmd, parms));
            return i;
        }

        //Returns list of invoices based on search criteria and date range
        public List<Invoice> ReturnInvoicesBasedOnSearchCriteria(DateTime stDate, DateTime endDate, string searchTxt)
        {
            InvoiceItemsManager IIM = new InvoiceItemsManager();
            ArrayList strText = new ArrayList();
            for (int i = 0; i < searchTxt.Split(' ').Length; i++)
            {
                strText.Add(searchTxt.Split(' ')[i]);
            }
            string sqlCmd = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, "
                + "custID, empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, "
                + "balanceDue, transactionType, comments FROM tbl_invoice WHERE invoiceNum IN (SELECT DISTINCT "
                + "invoiceNum FROM tbl_invoiceItem WHERE sku IN (";
                
            sqlCmd += IIM.ReturnStringSearchForAccessories(strText);
            sqlCmd += " UNION ";
            sqlCmd += IIM.ReturnStringSearchForClothing(strText);
            sqlCmd += " UNION ";
            sqlCmd += IIM.ReturnStringSearchForClubs(strText);
            sqlCmd += ")) AND invoiceDate BETWEEN '" + stDate + "' AND '" + endDate + "'";
            return ConvertFromDataTableToInvoice(dbc.returnDataTableData(sqlCmd));
        }
    }
}