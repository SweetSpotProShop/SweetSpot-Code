using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.OB
{
    //Used to define and create an item
    [Serializable]
    public class Inventory
    {
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public string varDescription { get; set; }
        public int intLocationID { get; set; }
        public string varLocationName { get; set; }
        public int intQuantity { get; set; }
        public double fltPrice { get; set; }
        public double fltCost { get; set; }
        public int intItemTypeID { get; set; }
        public string varAdditionalInformation { get; set; }

        public Inventory() { }
    }
}