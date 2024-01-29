using System;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class TradeINEntry : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly ItemsManager ItM = new ItemsManager();
        readonly InvoiceManager IM = new InvoiceManager();
        CurrentUser CU;
        private static Invoice invoice;
        private static int inventoryID;

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = "Page_Load";
            Session["currPage"] = "TradeINEntry.aspx";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                if (!IsPostBack)
                {
                    invoice = IM.CallReturnCurrentInvoice(Convert.ToInt32(Request.QueryString["invoice"]), CU.location.intProvinceID, objPageDetails)[0];

                    string[] inventoryInfo = ItM.CallReserveTradeInSKU(CU, objPageDetails);
                    inventoryID = Convert.ToInt32(inventoryInfo[1]);

                    lblSKUDisplay.Text = inventoryInfo[0].ToString();
                    ddlBrand.DataSource = ItM.CallReturnDropDownForBrand(objPageDetails);
                    ddlBrand.DataBind();

                    ddlModel.DataSource = ItM.CallReturnDropDownForModel(objPageDetails);
                    ddlModel.DataBind();
                    ddlModel.SelectedValue = "2624"; //"2426"; // is the testing value for 'Customer Trade'

                }
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Cancelling the trade-in item
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            string method = "BtnCancel_Click";
            //object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Finalizing the trade-in item
        protected void BtnAddTradeIN_Click(object sender, EventArgs e)
        {
            string method = "BtnAddTradeIN_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Creating a new club
                Clubs tradeIN = new Clubs
                {
                    intInventoryID = inventoryID,
                    varSku = lblSKUDisplay.Text,
                    fltCost = Convert.ToDouble(txtCost.Text),
                    fltPrice = Convert.ToDouble(txtPrice.Text),
                    fltPremiumCharge = 0,
                    intItemTypeID = 1,
                    intBrandID = Convert.ToInt32(ddlBrand.SelectedValue),
                    intModelID = Convert.ToInt32(ddlModel.SelectedValue),
                    intQuantity = Convert.ToInt32(txtQuantity.Text),
                    varTypeOfClub = txtClubType.Text,
                    varShaftType = txtShaft.Text,
                    varClubSpecification = txtClubSpec.Text,
                    varShaftFlexability = txtShaftFlex.Text,
                    varNumberOfClubs = txtNumberofClubs.Text,
                    varShaftSpecification = txtShaftSpec.Text,
                    varClubDexterity = txtDexterity.Text,
                    varAdditionalInformation = txtComments.Text,
                    bitIsUsedProduct = true,
                    intLocationID = CU.location.intLocationID,
                    varProdID = "TRADE-IN"
                };

                //this adds to the temp tradeIncart
                ItM.CallAddTradeInItemToTempTable(tradeIN, objPageDetails);

                //change cost and price for cart
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                InvoiceItems selectedTradeIn = new InvoiceItems
                {
                    intInvoiceID = invoice.intInvoiceID,
                    intInventoryID = tradeIN.intInventoryID,
                    varSku = tradeIN.varSku,
                    intItemQuantity = tradeIN.intQuantity,
                    varItemDescription = ItM.CallReturnBrandlNameFromBrandID(tradeIN.intBrandID, objPageDetails) + " "
                    + ItM.CallReturnModelNameFromModelID(tradeIN.intModelID, objPageDetails) + " " + tradeIN.varClubSpecification + " "
                    + tradeIN.varTypeOfClub + " " + tradeIN.varShaftSpecification + " " + tradeIN.varShaftFlexability + " "
                    + tradeIN.varClubDexterity,
                    fltItemCost = 0,
                    fltItemPrice = tradeIN.fltCost * (-1),
                    fltItemDiscount = 0,
                    fltItemRefund = 0,
                    bitIsDiscountPercent = false,
                    bitIsClubTradeIn = true,
                    intItemTypeID = 1,
                    varProdID = tradeIN.varProdID
                };

                IIM.CallInsertItemIntoSalesCart(selectedTradeIn, invoice.intTransactionTypeID, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);

                //IIM.NewTradeInChangeChargeTaxToFalse(selectedTradeIn, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);

                //Closing the trade in information window
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}