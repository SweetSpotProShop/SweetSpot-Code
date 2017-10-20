using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class TradeINEntry : System.Web.UI.Page
    {
        ErrorReporting er = new ErrorReporting();
        Object o = new Object();
        SweetShopManager ssm = new SweetShopManager();
        Clubs c = new Clubs();
        LocationManager lm = new LocationManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        CurrentUser cu;
        public int tradeInSku;
        public int storeLocation;

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "TradeINEntry.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                if (!IsPostBack)
                {
                    storeLocation = cu.locationID;
                    lblSKUDisplay.Text = (idu.reserveTradeInSKu(storeLocation)).ToString();
                    tradeInSku = Convert.ToInt32(lblSKUDisplay.Text);
                    //lblSKUDisplay.Text = idu.tradeInSku(storeLocation).ToString();
                }
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                int employeeID = cu.empID;
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Cancelling the trade-in item
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string method = "btnCancel_Click";
            try
            {
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                int employeeID = cu.empID;
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
        //Finalizing the trade-in item
        protected void btnAddTradeIN_Click(object sender, EventArgs e)
        {
            string method = "btnAddTradeIN_Click";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //Grabbing the values of the trade-in item
                //int sku = idu.tradeInSku(storeLocation);
                int sku = Convert.ToInt32(lblSKUDisplay.Text);
                double cost = Convert.ToDouble(txtCost.Text);
                int brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                int modelID = Convert.ToInt32(ddlModel.SelectedValue);
                int quant = Convert.ToInt32(txtQuantity.Text);
                int clubTypeID = Convert.ToInt32(ddlClubType.SelectedValue);
                string clubType = idu.getClubTypeName(clubTypeID);
                string shaft = txtShaft.Text;
                string clubSpec = txtClubSpec.Text;
                string shaftFlex = txtShaftFlex.Text;
                string numOfClubs = txtNumberofClubs.Text;
                string shaftSpec = txtShaftSpec.Text;
                string dext = txtDexterity.Text;
                string comments = txtComments.Text;
                bool used = true; //Set to true because a trade-in item is used

                //Creating a new club
                Clubs tradeIN = new Clubs(sku, brandID, modelID, 1, clubType,
                    shaft, numOfClubs, 0, cost, 0, quant, clubSpec, shaftSpec,
                    shaftFlex, dext, used, comments);
                //Trade in club to be displayed
                Clubs tradeINDisplay = new Clubs(sku, brandID, modelID, 1, clubType,
                    shaft, numOfClubs, 0, 0, (cost * (-1)), quant, clubSpec, shaftSpec,
                    shaftFlex, dext, used, comments);
                int location = cu.locationID;
                //Adding the trade-in item to the trade-in storage
                idu.addTradeInItem(tradeIN, Convert.ToInt32(lblSKUDisplay.Text), location);
                //Adding trade-in item to cart
                List<Cart> itemsInCart;
                itemsInCart = (List<Cart>)Session["ItemsInCart"];
                o = tradeINDisplay as Object;
                itemsInCart.Add(idu.addingToCart(o));
                Session["UpdateTheCart"] = true;
                //Closing the trade in information window
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                int employeeID = cu.empID;
                string currPage = Convert.ToString(Session["currPage"]);
                er.logError(ex, employeeID, currPage, method, this);
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
            }
        }
    }
}