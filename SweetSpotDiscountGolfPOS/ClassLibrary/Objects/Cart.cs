using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The cart class is used to define and create easy to access cart information for sales. 
    public class Cart
    {
        public int sku { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public double itemDiscount { get; set; }
        public bool percentage { get; set; }
        public bool isTradeIn { get; set; }
        public int typeID { get; set; }
        public double returnAmount { get; set; }
        public int locationID { get; set; }

        public Cart() { }
    }
}
