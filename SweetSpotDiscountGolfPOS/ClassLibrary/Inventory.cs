using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
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

        //public Items() { }
        //public Items(int Sku, string Description, string Location, int Quantity, double Price, double Cost, double ItemDiscount, bool Percent, int TypeID, bool isTradeIN, string Comments)
        //{
        //    sku = Sku;
        //    description = Description;
        //    location = Location;
        //    quantity = Quantity;
        //    price = Price;
        //    cost = Cost;
        //    itemDiscount = ItemDiscount;
        //    percent = Percent;
        //    typeID = TypeID;
        //    isTradeIn = isTradeIN;
        //    comments = Comments;
        //}
    }
}