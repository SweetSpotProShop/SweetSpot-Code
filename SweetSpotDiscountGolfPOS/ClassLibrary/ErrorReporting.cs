using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using SweetSpotProShop;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
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
        public void logError(Exception er, int employeeID, string page, string method, System.Web.UI.Page webPage)
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
            DBC.executeErrorInsertQuery(sqlCmd, parms);
        }
    }
}