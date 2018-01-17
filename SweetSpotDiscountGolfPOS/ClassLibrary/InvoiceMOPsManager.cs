using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class InvoiceMOPsManager
    {
        DatabaseCalls dbc = new DatabaseCalls();
        private List<InvoiceMOPs> ConvertFromDataTableToInvoiceMOPs(DataTable dt)
        {
            List<InvoiceMOPs> invoiceMOPs = dt.AsEnumerable().Select(row =>
            new InvoiceMOPs
            {
                id = row.Field<int>("ID"),
                invoiceNum = row.Field<int>("invoiceNum"),
                invoiceSubNum = row.Field<int>("invoiceSubNum"),
                mopType = row.Field<string>("mopType"),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return invoiceMOPs;
        }

        //Returns list of custoemrs based on an customer ID
        public List<InvoiceMOPs> ReturnInvoiceMOPs(string invoice)
        {
            string sqlCmd = "SELECT ID, invoiceNum, invoiceSubNum, mopType, amountPaid "
                + "FROM tbl_invoiceMOP WHERE invoiceNum = @invoiceNum AND invoiceSubNum = @invoiceSubNum";

            Object[][] parms =
            {
                 new object[] { "@invoiceNum", Convert.ToInt32(invoice.Split('-')[0]) },
                 new object[] { "@invoiceSubNum", Convert.ToInt32(invoice.Split('-')[1]) }
            };

            List<InvoiceMOPs> invoiceMOPs = ConvertFromDataTableToInvoiceMOPs(dbc.returnDataTableData(sqlCmd, parms));
            return invoiceMOPs;
        }
    }
}