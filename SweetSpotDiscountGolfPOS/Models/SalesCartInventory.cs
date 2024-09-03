using SweetSpotDiscountGolfPOS.OB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.Models
{
    //Used to define and create an item
    [Serializable]
    public class SalesCartInventory
    {
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public int intQuantity { get; set; }
        public string varDescription { get; set; }
        public double fltPrice { get; set; }
        public double fltDiscount { get; set; }
        public bool bitIsDiscountPercent { get; set; }
        public int intItemTypeID { get; set; }
        public bool bitIsClubTradeIn { get; set; }
        public string varProdID { get; set; }

        public SalesCartInventory() { }
    }
}