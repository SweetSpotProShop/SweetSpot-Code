using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SweetShop;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class MasterBIll
    {
        public List<Customer> customers { get; set; }
        public List<Invoice> invoices { get; set; }

    }
}