using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class MopsManager
    {
        DatabaseCalls dbc = new DatabaseCalls();
        private List<Mops> ConvertFromDataTableToMops(DataTable dt)
        {
            List<Mops> mops = dt.AsEnumerable().Select(row =>
            new Mops
            {
                methodOfPayment = row.Field<string>("mopType"),
                amountPaid = row.Field<double>("amountPaid")
            }).ToList();
            return mops;
        }

        //Returns list of InvoiceItems based on an Invoice Number
        public List<Mops> ReturnMopsFromCmdAndParams(string sqlCmd, object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnMopsFromCmdAndParams";
            return ConvertFromDataTableToMops(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
    }
}