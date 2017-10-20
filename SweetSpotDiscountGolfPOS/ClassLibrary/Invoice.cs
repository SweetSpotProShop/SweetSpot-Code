using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The invoice class is used to keep track or populate the printable invoice webpage with the current invoice's information
    public class Invoice
    {

        public int invoiceNum { get; set; }
        public int invoiceSub { get; set; }
        public DateTime invoiceDate { get; set; }
        public DateTime invoiceTime { get; set; }
        public int customerID { get; set; }
        public int employeeID { get; set; }
        public int locationID { get; set; }
        public double subTotal { get; set; }
        public double discountAmount { get; set; }
        public double tradeinAmount { get; set; }
        public double governmentTax { get; set; }
        public double provincialTax { get; set; }
        public double balanceDue { get; set; }
        public int transactionType { get; set; }
        public string comments { get; set; }
        public string customerName { get; set; }
        public string employeeName { get; set; }
        public string locationName { get; set; }
        public double shippingAmount { get; set; }

        public Invoice() { }
        public Invoice(int I, int S, DateTime D, string CN, double BD, string LN, string EN)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            customerName = CN;
            balanceDue = BD;
            locationName = LN;
            employeeName = EN;
        }
        public Invoice(int I, int S, DateTime D, string CN, double DA, double TA, double ST, double G, double P, double BD, string EN)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            customerName = CN;
            discountAmount = DA;
            tradeinAmount = TA;
            subTotal = ST;
            governmentTax = G;
            provincialTax = P;
            balanceDue = BD;
            employeeName = EN;
        }
        public Invoice(int I, int S, DateTime D, DateTime T, int CID, int EID, int LID, double ST, double DA, double TA, double G, double P, double BD, int TT, string C)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            invoiceTime = T;
            customerID = CID;
            employeeID = EID;
            locationID = LID;
            subTotal = ST;
            discountAmount = DA;
            tradeinAmount = TA;
            governmentTax = G;
            provincialTax = P;
            balanceDue = BD;
            transactionType = TT;
            comments = C;
        }
        public Invoice(int I, int S, DateTime D, DateTime T, int CID, int EID, int LID, double ST, double SA, double DA, double TA, double G, double P, double BD, int TT, string C)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            invoiceTime = T;
            customerID = CID;
            employeeID = EID;
            locationID = LID;
            subTotal = ST;
            shippingAmount = SA;
            discountAmount = DA;
            tradeinAmount = TA;
            governmentTax = G;
            provincialTax = P;
            balanceDue = BD;
            transactionType = TT;
            comments = C;
        }
    }
}
