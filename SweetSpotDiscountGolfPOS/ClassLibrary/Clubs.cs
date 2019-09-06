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
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public int intBrandID { get; set; }
        public int intModelID { get; set; }
        public int intItemTypeID { get; set; }
        public string varTypeOfClub { get; set; }
        public string varShaftType { get; set; }
        public string varNumberOfClubs { get; set; }
        public double fltPremiumCharge { get; set; }
        public double fltCost { get; set; }
        public double fltPrice { get; set; }
        public int intQuantity { get; set; }
        public string varClubSpecification { get; set; }
        public string varShaftSpecification { get; set; }
        public string varShaftFlexability { get; set; }
        public string varClubDexterity { get; set; }
        public int intLocationID { get; set; }
        public bool bitIsUsedProduct { get; set; }
        public string varAdditionalInformation { get; set; }
        public List<TaxTypePerInventoryItem> lstTaxTypePerInventoryItem { get; set; }

        public Clubs() { }
        //*******still used in TradeIn page
        //public Clubs(int SKU, int brand, int model, int type, string ClubType, string Shaft, string NumberOfClubs,
        //    double Premium, double Cost, double Price, int Quantity, string ClubSpec, string ShaftSpec, string ShaftFlex,
        //    string Dexterity, bool IsTradeIn, string Comments)
        //{
        //    sku = SKU;
        //    brandID = brand;
        //    modelID = model;
        //    typeID = type;
        //    clubType = ClubType;
        //    shaft = Shaft;
        //    numberOfClubs = NumberOfClubs;
        //    premium = Premium;
        //    cost = Cost;
        //    price = Price;
        //    quantity = Quantity;
        //    clubSpec = ClubSpec;
        //    shaftSpec = ShaftSpec;
        //    shaftFlex = ShaftFlex;
        //    dexterity = Dexterity;
        //    isTradeIn = IsTradeIn;
        //    comments = Comments;
        //}
        ////This one has location
        //public Clubs(int SKU, int brand, int model, int type, string ClubType, string Shaft, string NumberOfClubs,
        //    double Premium, double Cost, double Price, int Quantity, string ClubSpec, string ShaftSpec, string ShaftFlex,
        //    string Dexterity, int itemLocation, bool IsTradeIn, string Comments)
        //{
        //    sku = SKU;
        //    brandID = brand;
        //    modelID = model;
        //    typeID = type;
        //    clubType = ClubType;
        //    shaft = Shaft;
        //    numberOfClubs = NumberOfClubs;
        //    premium = 0;
        //    cost = Cost;
        //    price = Price;
        //    quantity = Quantity;
        //    clubSpec = ClubSpec;
        //    shaftSpec = ShaftSpec;
        //    shaftFlex = ShaftFlex;
        //    dexterity = Dexterity;
        //    itemlocation = itemLocation;
        //    isTradeIn = IsTradeIn;
        //    comments = Comments;
        //}
    }
}
