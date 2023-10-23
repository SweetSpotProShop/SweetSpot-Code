using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.OB
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
        public string varProdID { get; set; }
        public List<TaxTypePerInventoryItem> lstTaxTypePerInventoryItem { get; set; }

        public Clubs() { }
    }
}
