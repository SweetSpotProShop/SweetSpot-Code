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
        public int intTaxID { get; set; }
        public double fltTaxRate { get; set; }
        public string varTaxName { get; set; }        
        public Tax() { }
    }
    public class TaxTypePerInventoryItem
    {
        public int intInventoryID { get; set; }
        public int intTaxID { get; set; }
        public string varTaxName { get; set; }
        public double fltTaxRate { get; set; }
        public bool bitChargeTax { get; set; }

        public TaxTypePerInventoryItem() { }
    }
}
