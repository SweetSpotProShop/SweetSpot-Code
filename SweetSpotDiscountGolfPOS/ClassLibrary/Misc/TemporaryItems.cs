using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.Misc
{
    public class TemporaryItems
    {
        //need procedure that will correctly update the taxes charged between 1/01/19 - 9/30/19

        public void UpdateTaxesInTaxTable(DateTime dtmStartDate, DateTime dtmEndDate, object[] objPageDetails)
        {
            string strQueryName = "UpdateTaxesInTaxTable";
            string sqlCmd = "SELECT intInvoiceID, intInvoiceItemID, intTaxTypeID, fltTaxAmount, CASE WHEN intTaxTypeID = 1 "
                + "THEN ROUND((CalcFinalPrice * intItemQuantity) * 0.05, 2) WHEN intTaxTypeID = 2 THEN ROUND((CalcFinalPrice "
                + "* intItemQuantity) * 0.06, 2) WHEN intTaxTypeID = 6 THEN ROUND((CalcFinalPrice * intItemQuantity) * 0.10, "
                + "2) ELSE 0 END AS CalcTaxAmount FROM (SELECT I.intInvoiceID, IIT.intInvoiceItemID, intItemQuantity, "
                + "fltItemCost, fltItemPrice, fltItemDiscount, fltItemRefund, bitIsDiscountPercent, intTaxTypeID, fltTaxAmount, "
                + "bitIsTaxCharged, (CASE WHEN fltItemRefund = 0 THEN CASE WHEN bitIsDiscountPercent = 1 THEN fltItemPrice - "
                + "(fltItemPrice * (fltItemDiscount / 100)) ELSE fltItemPrice - fltItemDiscount END ELSE fltItemRefund END) "
                + "AS CalcFinalPrice FROM tbl_invoiceItemTaxes IIT JOIN tbl_invoiceItem II ON II.intInvoiceItemID = "
                + "IIT.intInvoiceItemID JOIN tbl_invoice I ON I.intInvoiceID = II.intInvoiceID WHERE dtmInvoiceDate BETWEEN "
                + "@dtmStartDate AND @dtmEndDate) AS APD";

            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtmStartDate },
                new object[] { "@dtmEndDate", dtmEndDate }
            };

            DatabaseCalls DBC = new DatabaseCalls();
            DataTable dt = DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);

            foreach (DataRow r in dt.Rows)
            {
                if (Convert.ToDouble(r[4]) != Convert.ToDouble(r[3]))
                {
                    string sqlCmd2 = "UPDATE tbl_invoiceItemTaxes SET fltTaxAmount = @fltTaxAmount WHERE intInvoiceItemID = "
                        + "@intInvoiceItemID AND intTaxTypeID = @intTaxTypeID";

                    object[][] parms2 =
                    {
                        new object[] { "@fltTaxAmount", Convert.ToDouble(r[4]) },
                        new object[] { "@intInvoiceItemID", Convert.ToInt32(r[1]) },
                        new object[] { "@intTaxTypeID", Convert.ToInt32(r[2]) }
                    };
                    DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd2, parms2, objPageDetails, strQueryName);
                }
            }
        }
    }
}