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
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU = new CurrentUser();
        ItemsManager IM = new ItemsManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "TradeINEntry.aspx";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                if (!IsPostBack)
                {
                    lblSKUDisplay.Text = IM.ReserveTradeInSKU(CU.locationID).ToString();
                    ddlBrand.DataSource = IM.ReturnDropDownForBrand();
                    ddlBrand.DataTextField = "brandName";
                    ddlBrand.DataValueField = "brandID";
                    ddlBrand.DataBind();

                    ddlClubType.DataSource = IM.ReturnDropDownForClubType();
                    ddlClubType.DataTextField = "typeName";
                    ddlClubType.DataValueField = "typeID";
                    ddlClubType.DataBind();

                    ddlModel.DataSource = IM.ReturnDropDownForModel();
                    ddlModel.DataTextField = "modelName";
                    ddlModel.DataValueField = "modelID";
                    ddlModel.DataBind();

                }
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1", method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
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
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1", method, this);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Finalizing the trade-in item
        protected void btnAddTradeIN_Click(object sender, EventArgs e)
        {
            string method = "btnAddTradeIN_Click";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Creating a new club
                Clubs tradeIN = new Clubs();

                tradeIN.sku = Convert.ToInt32(lblSKUDisplay.Text);
                tradeIN.cost = Convert.ToDouble(txtCost.Text);
                tradeIN.price = Convert.ToDouble(txtPrice.Text);
                tradeIN.premium = 0;
                tradeIN.typeID = 1;
                tradeIN.brandID = Convert.ToInt32(ddlBrand.SelectedValue);
                tradeIN.modelID = Convert.ToInt32(ddlModel.SelectedValue);
                tradeIN.quantity = Convert.ToInt32(txtQuantity.Text);
                tradeIN.clubType = IM.GetClubTypeName(Convert.ToInt32(ddlClubType.SelectedValue));
                tradeIN.shaft = txtShaft.Text;
                tradeIN.clubSpec = txtClubSpec.Text;
                tradeIN.shaftFlex = txtShaftFlex.Text;
                tradeIN.numberOfClubs = txtNumberofClubs.Text;
                tradeIN.shaftSpec = txtShaftSpec.Text;
                tradeIN.dexterity = txtDexterity.Text;
                tradeIN.comments = txtComments.Text;
                tradeIN.isTradeIn = true;
                tradeIN.itemlocation = CU.locationID;

                //this adds to the temp tradeIncart
                IM.AddTradeInItemToTempTable(tradeIN);

                //change cost and price for cart
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                InvoiceItems selectedTradeIn = new InvoiceItems();
                selectedTradeIn.invoiceNum = Convert.ToInt32(Request.QueryString["inv"][1]);
                selectedTradeIn.invoiceSubNum = Convert.ToInt32(Request.QueryString["inv"][2]);
                selectedTradeIn.sku = tradeIN.sku;
                selectedTradeIn.quantity = tradeIN.quantity;
                selectedTradeIn.description = IM.ReturnBrandlNameFromBrandID(tradeIN.brandID) + " " + IM.ReturnModelNameFromModelID(tradeIN.modelID) + " " + tradeIN.clubSpec + " " + tradeIN.clubType + " " + tradeIN.shaftSpec + " " + tradeIN.shaftFlex + " " + tradeIN.dexterity;
                selectedTradeIn.cost = 0;
                selectedTradeIn.price = tradeIN.cost * (-1);
                selectedTradeIn.itemDiscount = 0;
                selectedTradeIn.itemRefund = 0;
                selectedTradeIn.percentage = false;
                selectedTradeIn.isTradeIn = true;
                selectedTradeIn.typeID = 1;

                IIM.InsertItemIntoSalesCart(selectedTradeIn);
                //Closing the trade in information window
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3.1", method, this);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}