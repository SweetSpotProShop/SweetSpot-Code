using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class TradeINEntry : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        ItemsManager ItM = new ItemsManager();
        InvoiceManager IM = new InvoiceManager();
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
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
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Finalizing the trade-in item
        protected void btnAddTradeIN_Click(object sender, EventArgs e)
        {
            string method = "btnAddTradeIN_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Creating a new club
                Clubs tradeIN = new Clubs();
                tradeIN.intInventoryID = inventoryID;
                tradeIN.varSku = lblSKUDisplay.Text;
                tradeIN.fltCost = Convert.ToDouble(txtCost.Text);
                tradeIN.fltPrice = Convert.ToDouble(txtPrice.Text);
                tradeIN.fltPremiumCharge = 0;
                tradeIN.intItemTypeID = 1;
                tradeIN.intBrandID = Convert.ToInt32(ddlBrand.SelectedValue);
                tradeIN.intModelID = Convert.ToInt32(ddlModel.SelectedValue);
                tradeIN.intQuantity = Convert.ToInt32(txtQuantity.Text);
                tradeIN.varTypeOfClub = txtClubType.Text;
                tradeIN.varShaftType = txtShaft.Text;
                tradeIN.varClubSpecification = txtClubSpec.Text;
                tradeIN.varShaftFlexability = txtShaftFlex.Text;
                tradeIN.varNumberOfClubs = txtNumberofClubs.Text;
                tradeIN.varShaftSpecification = txtShaftSpec.Text;
                tradeIN.varClubDexterity = txtDexterity.Text;
                tradeIN.varAdditionalInformation = txtComments.Text;
                tradeIN.bitIsUsedProduct = true;
                tradeIN.intLocationID = CU.location.intLocationID;

                //this adds to the temp tradeIncart
                ItM.CallAddTradeInItemToTempTable(tradeIN, objPageDetails);

                //change cost and price for cart
                InvoiceItemsManager IIM = new InvoiceItemsManager();
                InvoiceItems selectedTradeIn = new InvoiceItems();
                selectedTradeIn.intInvoiceID = invoice.intInvoiceID;
                selectedTradeIn.intInventoryID = tradeIN.intInventoryID;
                selectedTradeIn.varSku = tradeIN.varSku;
                selectedTradeIn.intItemQuantity = tradeIN.intQuantity;
                selectedTradeIn.varItemDescription = ItM.CallReturnBrandlNameFromBrandID(tradeIN.intBrandID, objPageDetails) + " " 
                    + ItM.CallReturnModelNameFromModelID(tradeIN.intModelID, objPageDetails) + " " + tradeIN.varClubSpecification + " " 
                    + tradeIN.varTypeOfClub + " " + tradeIN.varShaftSpecification + " " + tradeIN.varShaftFlexability + " " 
                    + tradeIN.varClubDexterity;
                selectedTradeIn.fltItemCost = 0;
                selectedTradeIn.fltItemPrice = tradeIN.fltCost * (-1);
                selectedTradeIn.fltItemDiscount = 0;
                selectedTradeIn.fltItemRefund = 0;
                selectedTradeIn.bitIsDiscountPercent = false;
                selectedTradeIn.bitIsClubTradeIn = true;
                selectedTradeIn.intItemTypeID = 1;

                IIM.CallInsertItemIntoSalesCart(selectedTradeIn, invoice.intTransactionTypeID, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);

                //IIM.NewTradeInChangeChargeTaxToFalse(selectedTradeIn, invoice.dtmInvoiceDate, CU.location.intProvinceID, objPageDetails);

                //Closing the trade in information window
                string redirect = "<script>window.close('TradeINEntry.aspx');</script>";
                Response.Write(redirect);
            }
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}