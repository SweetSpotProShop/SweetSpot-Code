using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The clubs class is used to define and create easy to access information for clubs.
    public class Clubs
    {
        public int sku { get; set; }
        public int brandID { get; set; }
        public int modelID { get; set; }
        public int typeID { get; set; }
        public string clubType { get; set; }
        public string shaft { get; set; }
        public string numberOfClubs { get; set; }
        public double premium { get; set; }
        public double cost { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string clubSpec { get; set; }
        public string shaftSpec { get; set; }
        public string shaftFlex { get; set; }
        public string dexterity { get; set; }
        public int itemlocation { get; set; }
        public bool used { get; set; }
        public string comments { get; set; }

        public Clubs() { }
        //*******still used in TradeIn page
        public Clubs(int SKU, int brand, int model, int type, string ClubType, string Shaft, string NumberOfClubs,
            double Premium, double Cost, double Price, int Quantity, string ClubSpec, string ShaftSpec, string ShaftFlex,
            string Dexterity, bool Used, string Comments)
        {
            sku = SKU;
            brandID = brand;
            modelID = model;
            typeID = type;
            clubType = ClubType;
            shaft = Shaft;
            numberOfClubs = NumberOfClubs;
            premium = Premium;
            cost = Cost;
            price = Price;
            quantity = Quantity;
            clubSpec = ClubSpec;
            shaftSpec = ShaftSpec;
            shaftFlex = ShaftFlex;
            dexterity = Dexterity;
            used = Used;
            comments = Comments;
        }
        //This one has location
        public Clubs(int SKU, int brand, int model, int type, string ClubType, string Shaft, string NumberOfClubs,
            double Premium, double Cost, double Price, int Quantity, string ClubSpec, string ShaftSpec, string ShaftFlex,
            string Dexterity, int itemLocation, bool Used, string Comments)
        {
            sku = SKU;
            brandID = brand;
            modelID = model;
            typeID = type;
            clubType = ClubType;
            shaft = Shaft;
            numberOfClubs = NumberOfClubs;
            premium = 0;
            cost = Cost;
            price = Price;
            quantity = Quantity;
            clubSpec = ClubSpec;
            shaftSpec = ShaftSpec;
            shaftFlex = ShaftFlex;
            dexterity = Dexterity;
            itemlocation = itemLocation;
            used = Used;
            comments = Comments;
        }
    }
}
