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
        private String connectionString;
        public ErrorReporting() { connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString; }
        //This methods intended use was to send an automatic email when an error occured.
        //May revisit at a later point
        public void sendError(string errorMessage)
        {
            SmtpClient SmtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential("sweetspotgolfshop@outlook.com", "ARu23B101");
            SmtpServer.EnableSsl = true;
            SmtpServer.Port = 587;
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            mail = new MailMessage();
            mail.From = new MailAddress("sweetspotgolfshop@outlook.com");
            mail.To.Add("sweetspotgolfshop@outlook.com");
            mail.Subject = "Error " + DateTime.Now.ToString();
            mail.Body = errorMessage;
            SmtpServer.Send(mail);
        }
        //This method is used to log errors in the database
        public void logError(Exception er, int employeeID, string page, string method, System.Web.UI.Page webPage)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Insert into tbl_error values(@employeeID, @date, @time, "
                + "@errorPage, @errorMethod, @errorCode, @errorText)";
            cmd.Parameters.AddWithValue("employeeID", employeeID);
            cmd.Parameters.AddWithValue("date", date);
            cmd.Parameters.AddWithValue("time", time);
            cmd.Parameters.AddWithValue("errorPage", er.Source + " - " + page);
            cmd.Parameters.AddWithValue("errorMethod", method);
            cmd.Parameters.AddWithValue("errorCode", er.HResult);
            cmd.Parameters.AddWithValue("errorText", er.Message);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            //MessageBox.ShowMessage("An Error has occured and been logged. "
            //    + "If you continue to receive this message please contact "
            //    + "your system administrator", webPage);
        }
    }
}