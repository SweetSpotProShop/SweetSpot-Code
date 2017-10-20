using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //Used to define and create an item
    public class Items
    {
        public int sku { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public string size { get; set; }
        public string colour { get; set; }
        public string gender { get; set; }
        public string style { get; set; }
        public string clubType { get; set; }
        public string shaft { get; set; }
        public string numberOfClubs { get; set; }
        public double price { get; set; }
        public double cost { get; set; }
        public string location { get; set; }
        public int typeID { get; set; }

        public Items() { }
        public Items(int s, string d, int q, double p, double c)
        {
            sku = s;
            description = d;
            quantity = q;
            price = p;
            cost = c;
        }
        public Items(int s, string d, int q, double p, double c, string l)
        {
            sku = s;
            description = d;
            quantity = q;
            price = p;
            cost = c;
            location = l;
        }
        public Items(int s, string d, int q, double p, double c, int t, string l)
        {
            sku = s;
            description = d;
            quantity = q;
            price = p;
            cost = c;
            typeID = t;
            location = l;
        }
    }
}
