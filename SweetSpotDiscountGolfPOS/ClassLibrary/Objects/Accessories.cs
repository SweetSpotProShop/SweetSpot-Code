using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The accessories class is used to define and create an easy to access information for accessories. 
    public class Accessories
    {
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public string varSize { get; set; }
        public string varColour { get; set; }
        public double fltPrice { get; set; }
        public double fltCost { get; set; }
        public int intBrandID { get; set; }
        public int intModelID { get; set; }
        public string varTypeOfAccessory { get; set; }
        public int intQuantity { get; set; }
        public int intItemTypeID { get; set; }
        public int intLocationID { get; set; }
        public string varAdditionalInformation { get; set; }
        public string varProdID { get; set; }
        public List<TaxTypePerInventoryItem> lstTaxTypePerInventoryItem { get; set; }

        public Accessories() { }
    }
}
