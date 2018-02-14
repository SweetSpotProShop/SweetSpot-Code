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
            List<Tax> tax = ConvertFromDataTableToTax(ReturnTaxListBasedOnDateAndProvinceForUpdate(provinceID, selectedDate));
            return tax;
        }
        public DataTable ReturnTaxListBasedOnDateAndProvinceForUpdate (int prov, DateTime currentDate)
        {
            string sqlCmd = "SELECT TR.taxID, TR.taxRate, TT.taxName FROM tbl_taxRate AS TR "
                + "INNER JOIN tbl_taxType TT ON TR.taxID = TT.taxID INNER JOIN(SELECT taxID, "
                + "MAX(taxDate) AS MTD FROM tbl_taxRate WHERE (taxDate <= @currentDate) AND "
                + "(provStateID = @prov) GROUP BY taxID) AS TD ON TR.taxID = TD.taxID AND "
                + "TR.taxDate = TD.MTD WHERE (TR.provStateID = @prov)";
            object[][] parms =
            {
                new object[] { "@currentDate", currentDate },
                new object[] { "@prov", prov }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
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