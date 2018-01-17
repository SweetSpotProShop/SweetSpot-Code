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
        
        //public int typeID { get; set; }
        //public string size { get; set; }
        //public string colour { get; set; }
        //public string gender { get; set; }
        //public string style { get; set; }
        //public string clubType { get; set; }
        //public string shaft { get; set; }
        //public string numberOfClubs { get; set; }
        public string invoice { get; set; }
        public double discount { get; set; }
        public bool percent { get; set; }
        public double difference { get; set; }
        //public int amountSold { get; set; }

        public Items() { }
        public Items(int Sku, string Description, string Location, int Quantity, double Price, double Cost)
        {
            sku = Sku;
            description = Description;
            location = Location;
            quantity = Quantity;
            price = Price;
            cost = Cost;
        }
        //public Items(int s, string d, int q, double p, double c, string l)
        //{
        //    sku = s;
        //    description = d;
        //    quantity = q;
        //    price = p;
        //    cost = c;
        //    location = l;
        //}
        //public Items(int s, string d, int q, double p, double c, int t, string l)
        //{
        //    sku = s;
        //    description = d;
        //    quantity = q;
        //    price = p;
        //    cost = c;
        //    typeID = t;
        //    location = l;
        //}
        public Items(int num, int subNum, int s, double c, double p, double disc, bool perc, double d)
        {
            invoice = num.ToString() + "-" + subNum.ToString();
            sku = s;
            cost = c;
            price = p;
            discount = disc;
            percent = perc;
            difference = d;
        }
        //public Items(int s, int a)
        //{
        //    sku = s;
        //    amountSold = a;
        //}
        //public Items(string d, int a)
        //{
        //    description = d;
        //    amountSold = a;
        //}
    }
}