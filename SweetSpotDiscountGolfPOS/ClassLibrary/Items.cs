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
    public class Items
    {
        public int sku { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public double itemDiscount { get; set; }
        public bool percent { get; set; }
        public int typeID { get; set; }
        public bool isTradeIn { get; set; }
        public string comments { get; set; }

        public Items() { }
        public Items(int Sku, string Description, string Location, int Quantity, double Price, double Cost, double ItemDiscount, bool Percent, int TypeID, bool isTradeIN, string Comments)
        {
            sku = Sku;
            description = Description;
            location = Location;
            quantity = Quantity;
            price = Price;
            cost = Cost;
            itemDiscount = ItemDiscount;
            percent = Percent;
            typeID = TypeID;
            isTradeIn = isTradeIN;
            comments = Comments;
        }
    }
}