using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The clothing class is used to define and create easy to access information for clothing.
    public class Clothing
    {
        public int sku { get; set; }
        public int brandID { get; set; }
        public string size { get; set; }
        public string colour { get; set; }
        public string gender { get; set; }
        public string style { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public int quantity { get; set; }
        public int typeID { get; set; }
        public int locID { get; set; }
        public string comments { get; set; }

        public Clothing() { }
    }
}
