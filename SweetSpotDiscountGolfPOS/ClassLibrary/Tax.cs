using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //Used to define and create a tax
    public class Tax
    {
        public double taxRate { get; set; }
        public string taxName { get; set; }

        public Tax() { }
        public Tax(string tName, double tRate)
        {
            taxName = tName;
            taxRate = tRate;
        }
    }
}
