using SweetShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class TaxManager
    {
        DatabaseCalls dbc = new DatabaseCalls();

        private List<Tax> ConvertFromDataTableToTax(DataTable dt)
        {
            List<Tax> tax = dt.AsEnumerable().Select(row =>
            new Tax
            {
                taxID = row.Field<int>("taxID"),
                taxRate = row.Field<double>("taxRate")
            }).ToList();
            return tax;
        }

        public List<Tax> ReturnTaxListBasedOnDate(DateTime selectedDate, int provinceID)
        {
            string sqlCmd = "SELECT tr.taxRate, tr.taxID FROM tbl_taxRate tr "
                + "INNER JOIN (SELECT taxID, MAX(taxDate) AS MTD FROM tbl_taxRate "
                + "WHERE taxDate <= @selectedDate AND provStateID = @provinceID "
                + "GROUP BY taxID) td ON tr.taxID = td.taxID and tr.taxDate = "
                + "td.MTD where provStateID = @provinceID";
            Object[][] parms =
            {
                new object[] { "@selectedDate", selectedDate },
                new object[] { "@provinceID", provinceID }
            };

            List<Tax> tax = ConvertFromDataTableToTax(dbc.returnDataTableData(sqlCmd, parms));
            return tax;
        }
        public void InsertNewTaxRate(int provinceID, int taxID, DateTime selectedDate, double taxRate)
        {
            string sqlCmd = "INSERT INTO tbl_taxRate VALUES(@provID, "
                + "@taxDate, @taxID, @taxRate)";
            Object[][] parms =
            {
                new object[] { "@provID", provinceID },
                new object[] { "@taxDate", selectedDate },
                new object[] { "@taxID", taxID },
                new object[] { "@taxRate", taxRate }
            };
            dbc.executeInsertQuery(sqlCmd,parms);
        }
    }
}