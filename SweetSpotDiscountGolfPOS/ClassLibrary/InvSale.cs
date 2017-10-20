using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SweetShop
{
    //This class is a relic from the previous developers. *****NOT USED
    public class InvSale
    {
        public int invoiceID { get; set; }
        public int SKU { get; set; }
        public int quantity { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string clubType { get; set; }
        public string shaft { get; set; }
        public string numberOfClubs { get; set; }
        public string clubSpec { get; set; }
        public string shaftSpec { get; set; }
        public string shaftFlex { get; set; }
        public string dexterity { get; set; }
        public double wePay { get; set; }
        public double retailPrice { get; set; }


        public InvSale() { }

        public InvSale(int invoiceId, int sku, int Quantity, string Brand, string Model, string ClubType, string Shaft, string NumberOfClubs, string ClubSpec, string ShaftSpec, string ShaftFlex, string Dexterity, double WePay, double RetailPrice)
        {
            invoiceID = invoiceId;
            SKU = sku;
            quantity = Quantity;
            brand = Brand;
            model = Model;
            clubType = ClubType;
            shaft = Shaft;
            numberOfClubs = NumberOfClubs;
            clubSpec = ClubSpec;
            shaftSpec = ShaftSpec;
            shaftFlex = ShaftFlex;
            dexterity = Dexterity;
            wePay = WePay;
            retailPrice = RetailPrice;
            
        }
    }

}
