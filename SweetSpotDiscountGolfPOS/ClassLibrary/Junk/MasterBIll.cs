using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;

namespace SweetSpotDiscountGolfPOS.Junk
{
    public class MasterBIll
    {
        public List<Customer> customers { get; set; }
        public List<Invoice> invoices { get; set; }

    }
}