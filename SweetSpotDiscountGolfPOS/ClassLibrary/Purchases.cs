using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS
{
    public class Purchases
    {
        public int receiptNumber { get; set; }
        public DateTime receiptDate { get; set; }
        public string mopDescription { get; set; }
        public int chequeNumber { get; set; }
        public double amountPaid { get; set; }

        public Purchases() { }
        public Purchases(int N, DateTime D, string M, int C, double A)
        {
            receiptNumber = N;
            receiptDate = D;
            mopDescription = M;
            chequeNumber = C;
            amountPaid = A;
        }
    }
}