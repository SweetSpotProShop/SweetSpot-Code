using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
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
        public Customer customer { get; set; }
        public Employee employee { get; set; }
        public Location location { get; set; }
        public double subTotal { get; set; }
        public double shippingAmount { get; set; }
        public double discountAmount { get; set; }
        public double tradeinAmount { get; set; }
        public double governmentTax { get; set; }
        public double provincialTax { get; set; }
        public double balanceDue { get; set; }
        public List<InvoiceItems> soldItems { get; set; }
        public List<InvoiceMOPs> usedMops { get; set; }
        public int transactionType { get; set; }
        public string comments { get; set; }

        public Invoice() { }
        public Invoice(int I, int S, DateTime D, DateTime T, Employee EID,
            Location LID, double ST, double SA, double DA, double TA, double G, double P,
            double BD, int TT, string C)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            invoiceTime = T;
            employee = EID;
            location = LID;
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
        public Invoice(int I, int S, DateTime D, DateTime T, Customer CID, Employee EID,
            Location LID, double ST, double SA, double DA, double TA, double G, double P, 
            double BD, List<InvoiceItems> SoldItems, List<InvoiceMOPs> UsedMops, int TT, string C)
        {
            invoiceNum = I;
            invoiceSub = S;
            invoiceDate = D;
            invoiceTime = T;
            customer = CID;
            employee = EID;
            location = LID;
            subTotal = ST;
            shippingAmount = SA;
            discountAmount = DA;
            tradeinAmount = TA;
            governmentTax = G;
            provincialTax = P;
            balanceDue = BD;
            soldItems = SoldItems;
            usedMops = UsedMops;
            transactionType = TT;
            comments = C;
        }
    }
}
