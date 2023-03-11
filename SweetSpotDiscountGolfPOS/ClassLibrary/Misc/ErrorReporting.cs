using System;
using SweetSpotDiscountGolfPOS.OB;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;

namespace SweetSpotDiscountGolfPOS.Misc
{
    //This method is used to handle and report an errors that happen during the use of the POS system
    public class ErrorReporting
    {
        DatabaseCalls DBC = new DatabaseCalls();
        public ErrorReporting() {}

        //This methods intended use was to send an automatic email when an error occurred.
        //May revisit at a later point
        //public void sendError(string errorMessage)
        //{
        //    SmtpClient SmtpServer = new SmtpClient();
        //    MailMessage mail = new MailMessage();
        //    SmtpServer.UseDefaultCredentials = true;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential("sweetspotgolfshop@outlook.com", "ARu23B101");
        //    SmtpServer.EnableSsl = true;
        //    SmtpServer.Port = 587;
        //    SmtpServer.Host = "smtp.gmail.com";
        //    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    mail = new MailMessage();
        //    mail.From = new MailAddress("sweetspotgolfshop@outlook.com");
        //    mail.To.Add("sweetspotgolfshop@outlook.com");
        //    mail.Subject = "Error " + DateTime.Now.ToString();
        //    mail.Body = errorMessage;
        //    SmtpServer.Send(mail);
        //}


        //This method is used to log errors in the database
        private void LogError(Exception er, int employeeID, string page, string method, System.Web.UI.Page webPage)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");
            string sqlCmd = "INSERT INTO tbl_error VALUES(@intEmployeeID, @dtmErrorDate, @dtmErrorTime, @varErrorPage, @varErrorMethod, "
                + "@intErrorCode, @varErrorText)";
            object[][] parms =
            {
                new object[] { "@intEmployeeID", employeeID },
                new object[] { "@dtmErrorDate", date },
                new object[] { "@dtmErrorTime", time },
                new object[] { "@varErrorPage", er.Source + " - " + page },
                new object[] { "@varErrorMethod", method },
                new object[] { "@intErrorCode", er.HResult },
                new object[] { "@varErrorText", er.Message }
            };
            DBC.MakeDataBaseCallToNonReturnErrorQuery(sqlCmd, parms);
        }


        public void CallLogError(Exception er, int employeeID, string page, string method, System.Web.UI.Page webPage)
        {
            LogError(er, employeeID, page, method, webPage);
        }




        public void CallLogTaxError(Invoice invoice, object[] objPageDetails)
        {
            LogTaxError(invoice, objPageDetails);
        }
        private void LogTaxError(Invoice invoice, object[] objPageDetails)
        {
            InsertInvoiceIntoTaxErrorTable(invoice, objPageDetails);
            InsertInvoiceItemsIntoTaxErrorTable(invoice, objPageDetails);
            InsertInvoiceItemTaxesIntoTaxErrorTable(invoice, objPageDetails);
            InsertInvoiceMopsIntoTaxErrorTable(invoice, objPageDetails);
        }
        private void InsertInvoiceIntoTaxErrorTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceIntoTaxErrorTable";
            string sqlCmd = "INSERT INTO tblTaxErrorInvoice VALUES(@intInvoiceID, @varInvoiceNumber, @intInvoiceSubNumber, @dtmInvoiceDate, "
                + "@dtmInvoiceTime, @intCustomerID, @intEmployeeID, @intLocationID, @intShippingProvinceID, @fltShippingCharges, @bitIsShipping, @fltTotalDiscount, "
                + "@fltTotalTradeIn, @fltSubTotal, @fltBalanceDue, @fltGovernmentTaxAmount, @fltHarmonizedTaxAmount, @fltLiquorTaxAmount, @fltProvincialTaxAmount, "
                + "@fltQuebecTaxAmount, @fltRetailTaxAmount, @fltShippingTaxAmount, @intTransactionTypeID, @varAdditionalInformation, @bitChargeGST, @bitChargePST, "
                + "@bitChargeLCT)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID },
                new object[] { "@varInvoiceNumber", invoice.varInvoiceNumber },
                new object[] { "@intInvoiceSubNumber", invoice.intInvoiceSubNumber },
                new object[] { "@dtmInvoiceDate", DateTime.Now.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceTime", DateTime.Now.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", invoice.intCustomerID },
                new object[] { "@intEmployeeID", invoice.intEmployeeID },
                new object[] { "@intLocationID", invoice.intLocationID },
                new object[] { "@intShippingProvinceID", invoice.intShippingProvinceID },
                new object[] { "@fltShippingCharges", Math.Round(invoice.fltShippingCharges, 2) },
                new object[] { "@bitIsShipping", invoice.bitIsShipping },
                new object[] { "@fltTotalDiscount", Math.Round(invoice.fltTotalDiscount, 2) },
                new object[] { "@fltTotalTradeIn", Math.Round(invoice.fltTotalTradeIn, 2) },
                new object[] { "@fltSubTotal", Math.Round(invoice.fltSubTotal, 2) },
                new object[] { "@fltBalanceDue", Math.Round(invoice.fltBalanceDue + invoice.fltShippingCharges, 2) },
                new object[] { "@fltGovernmentTaxAmount", Math.Round(invoice.fltGovernmentTaxAmount, 2) },
                new object[] { "@fltHarmonizedTaxAmount", Math.Round(invoice.fltHarmonizedTaxAmount, 2) },
                new object[] { "@fltLiquorTaxAmount", Math.Round(invoice.fltLiquorTaxAmount, 2) },
                new object[] { "@fltProvincialTaxAmount", Math.Round(invoice.fltProvincialTaxAmount, 2) },
                new object[] { "@fltQuebecTaxAmount", Math.Round(invoice.fltQuebecTaxAmount, 2) },
                new object[] { "@fltRetailTaxAmount", Math.Round(invoice.fltRetailTaxAmount, 2) },
                new object[] { "@fltShippingTaxAmount", Math.Round(invoice.fltShippingTaxAmount, 2) },
                new object[] { "@intTransactionTypeID", 1 },
                new object[] { "@varAdditionalInformation", invoice.varAdditionalInformation },
                new object[] { "@bitChargeGST", true },
                new object[] { "@bitChargePST", true },
                new object[] { "@bitChargeLCT", true }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void InsertInvoiceItemsIntoTaxErrorTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceItemsIntoTaxErrorTable";
            foreach (InvoiceItems item in invoice.invoiceItems)
            {
                string sqlCmd = "INSERT INTO tblTaxErrorItems VALUES(@intInvoiceItemID, @intInvoiceID, @intInventoryID, @intItemQuantity, @fltItemCost, "
                    + "@fltItemPrice, @fltItemDiscount, @fltItemRefund, @bitIsDiscountPercent, @varItemDescription, @intItemTypeID, @bitIsClubTradeIn)";

                object[][] parms =
                {
                    new object[] { "@intInvoiceItemID", item.intInvoiceItemID },
                    new object[] { "@intInvoiceID", item.intInvoiceID },
                    new object[] { "@intInventoryID", item.intInventoryID },
                    new object[] { "@intItemQuantity", item.intItemQuantity },
                    new object[] { "@fltItemCost", Math.Round(item.fltItemCost, 2) },
                    new object[] { "@fltItemPrice", Math.Round(item.fltItemPrice, 2) },
                    new object[] { "@fltItemDiscount", Math.Round(item.fltItemDiscount, 2) },
                    new object[] { "@fltItemRefund", Math.Round(item.fltItemRefund, 2) },
                    new object[] { "@bitIsDiscountPercent", item.bitIsDiscountPercent },
                    new object[] { "@varItemDescription", item.varItemDescription },
                    new object[] { "@intItemTypeID", item.intItemTypeID },
                    new object[] { "@bitIsClubTradeIn", item.bitIsClubTradeIn }
                };
                DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }
        private void InsertInvoiceItemTaxesIntoTaxErrorTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceItemTaxesIntoTaxeErrorTable";
            foreach (InvoiceItems ii in invoice.invoiceItems)
            {
                foreach (InvoiceItemTax tax in ii.invoiceItemTaxes)
                {
                    string sqlCmd = "INSERT INTO tblTaxErrorItemsTaxes VALUES(@intInvoiceItemID, @intTaxTypeID, @fltTaxAmount, @bitTaxCharged)";
                    object[][] parms =
                    {
                        new object[] { "@intInvoiceItemID", tax.intInvoiceItemID },
                        new object[] { "@intTaxTypeID", tax.intTaxTypeID },
                        new object[] { "@fltTaxAmount", Math.Round(tax.fltTaxAmount, 2) },
                        new object[] { "@bitTaxCharged", tax.bitIsTaxCharged }
                    };
                    DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
                }
            }
        }
        private void InsertInvoiceMopsIntoTaxErrorTable(Invoice invoice, object[] objPageDetails)
        {
            string strQueryName = "InsertInvoiceMopsIntoTaxErrorTable";
            foreach (InvoiceMOPs payment in invoice.invoiceMops)
            {
                string sqlCmd = "INSERT INTO tblTaxErrorMOPS VALUES(@intInvoicePaymentID, @intInvoiceID, @intPaymentID, "
                    + "@fltAmountPaid, @fltTenderedAmount, @fltCustomerChange)";

                object[][] parms =
                {
                    new object[] { "@intInvoicePaymentID", payment.intInvoicePaymentID },
                    new object[] { "@intInvoiceID", payment.intInvoiceID },
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@fltAmountPaid", Math.Round(payment.fltAmountPaid, 2) },
                    new object[] { "@fltTenderedAmount", Math.Round(payment.fltTenderedAmount, 2) },
                    new object[] { "@fltCustomerChange", Math.Round(payment.fltCustomerChange, 2) }
                };
                DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            }
        }



    }
}