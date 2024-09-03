using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.Models
{
    //The customer class is used to define what a customer is
    public class SalesCartCustomer
    {
        public int intCustomerID { get; set; }
        public string varFirstName { get; set; }
        public string varLastName { get; set; }
        public string varContactNumber { get; set; }
        public string varEmailAddress { get; set; }
        public bool bitSendMarketing { get; set; }

        public SalesCartCustomer() { }
    }
}
