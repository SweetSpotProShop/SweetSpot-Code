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
        public int intInvoiceID { get; set; }
        public string varInvoiceNumber { get; set; }
        public int intInvoiceSubNumber { get; set; }
        public DateTime dtmInvoiceDate { get; set; }
        public DateTime dtmInvoiceTime { get; set; }
        public Customer customer { get; set; }
        public Employee employee { get; set; }
        public Location location { get; set; }
        public double fltSubTotal { get; set; }
        public double fltShippingCharges { get; set; }
        public double fltTotalDiscount { get; set; }
        public double fltTotalTradeIn { get; set; }
        public double fltGovernmentTaxAmount { get; set; }
        public double fltProvincialTaxAmount { get; set; }
        public double fltBalanceDue { get; set; }
        public int intTransactionTypeID { get; set; }
        public string varTransactionName { get; set; }
        public string varAdditionalInformation { get; set; }
        public bool bitChargeGST { get; set; }
        public bool bitChargePST { get; set; }

        public List<InvoiceItems> invoiceItems { get; set; }
        public List<InvoiceMOPs> invoiceMops { get; set; }


        public Invoice() { }
        //public Invoice(int I, int S, DateTime D, DateTime T, Employee EID,
        //    Location LID, double ST, double SA, double DA, double TA, double G, double P,
        //    double BD, int TT, string C, bool GST, bool PST)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    invoiceTime = T;
        //    employee = EID;
        //    location = LID;
        //    subTotal = ST;
        //    shippingAmount = SA;
        //    discountAmount = DA;
        //    tradeinAmount = TA;
        //    governmentTax = G;
        //    provincialTax = P;
        //    balanceDue = BD;
        //    transactionType = TT;
        //    comments = C;
        //    chargeGST = GST;
        //    chargePST = PST;
        //}
        //public Invoice(int I, int S, DateTime D, DateTime T, Customer CID, Employee EID,
        //    Location LID, double ST, double SA, double DA, double TA, double G, double P, 
        //    double BD, List<InvoiceItems> SoldItems, List<InvoiceMOPs> UsedMops, int TT,
        //    string TN, string C, bool GST, bool PST)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    invoiceTime = T;
        //    customer = CID;
        //    employee = EID;
        //    location = LID;
        //    subTotal = ST;
        //    shippingAmount = SA;
        //    discountAmount = DA;
        //    tradeinAmount = TA;
        //    governmentTax = G;
        //    provincialTax = P;
        //    balanceDue = BD;
        //    soldItems = SoldItems;
        //    usedMops = UsedMops;
        //    transactionType = TT;
        //    transactionName = TN;
        //    comments = C;
        //    chargeGST = GST;
        //    chargePST = PST;
        //}

        ////Old collection code
        //public string invoice { get; set; }
        //public double totalCost { get; set; }
        //public bool percentage { get; set; }
        //public string totalProfit { get; set; }
        //public string locationName { get; set; }
        //public int customerID { get; set; }
        //public int employeeID { get; set; }
        //public int locationID { get; set; }
        //public Invoice(string I, double tc, double td, bool p, double tp, string tpf)
        //{
        //    invoice = I;
        //    totalCost = tc;
        //    discountAmount = td;
        //    percentage = p;
        //    balanceDue = tp;
        //    totalProfit = tpf;
        //}
        //public Invoice(int I, int S, DateTime D, string CN, double BD, string LN, string EN)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    customerName = CN;
        //    balanceDue = BD;
        //    locationName = LN;
        //    employeeName = EN;
        //}
        //public Invoice(int I, int S, DateTime D, string CN, double DA, double TA, double ST, double G, double P, double BD, string EN)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    customerName = CN;
        //    discountAmount = DA;
        //    tradeinAmount = TA;
        //    subTotal = ST;
        //    governmentTax = G;
        //    provincialTax = P;
        //    balanceDue = BD;
        //    employeeName = EN;
        //}
        //public Invoice(int I, int S, DateTime D, DateTime T, int CID, int EID, int LID, double ST, double DA, double TA, double G, double P, double BD, int TT, string C)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    invoiceTime = T;
        //    customerID = CID;
        //    employeeID = EID;
        //    locationID = LID;
        //    subTotal = ST;
        //    discountAmount = DA;
        //    tradeinAmount = TA;
        //    governmentTax = G;
        //    provincialTax = P;
        //    balanceDue = BD;
        //    transactionType = TT;
        //    comments = C;
        //}
        //public Invoice(int I, int S, DateTime D, DateTime T, Customer C, Employee E, Location L, double ST, double SA, double DA, double TA, double G, double P, double BD, int TT, string CS)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    invoiceTime = T;
        //    customer = C;
        //    employee = E;
        //    location = L;
        //    subTotal = ST;
        //    shippingAmount = SA;
        //    discountAmount = DA;
        //    tradeinAmount = TA;
        //    governmentTax = G;
        //    provincialTax = P;
        //    balanceDue = BD;
        //    transactionType = TT;
        //    comments = CS;
        //}
        //public Invoice(int num, int subNum, DateTime d, string c, string e, double da, double bd)
        //{
        //    invoiceNum = num;
        //    invoiceSub = subNum;
        //    invoiceDate = d;
        //    customerName = c;
        //    employeeName = e;
        //    discountAmount = da;
        //    balanceDue = bd;
        //}
        //public Invoice(int I, int S, DateTime D, DateTime T, Customer CID, Employee EID,
        //    Location LID, double ST, double SA, double DA, double TA, double G, double P,
        //    double BD, List<InvoiceItems> SoldItems, List<InvoiceMOPs> UsedMops, int TT, string C)
        //{
        //    invoiceNum = I;
        //    invoiceSub = S;
        //    invoiceDate = D;
        //    invoiceTime = T;
        //    customer = CID;
        //    employee = EID;
        //    location = LID;
        //    subTotal = ST;
        //    shippingAmount = SA;
        //    discountAmount = DA;
        //    tradeinAmount = TA;
        //    governmentTax = G;
        //    provincialTax = P;
        //    balanceDue = BD;
        //    soldItems = SoldItems;
        //    usedMops = UsedMops;
        //    transactionType = TT;
        //    comments = C;
        //}
    }
}
