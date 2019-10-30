using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesCashOut : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CurrentUser CU;
        Reports R = new Reports();
        //private static Cashout cashout;
        LocationManager LM = new LocationManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCashOut.aspx";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    if (!IsPostBack)
                    {
                        Cashout cashout = new Cashout();
                        //Gathering the start and end dates
                        DateTime selectedDate = DateTime.Parse(Request.QueryString["selectedDate"].ToString());
                        int locationID = Convert.ToInt32(Request.QueryString["location"].ToString());
                        object[] args = { selectedDate, locationID };
                        lblCashoutDate.Text = "Cashout on: " + selectedDate.ToString("dd/MMM/yy") + " for " + LM.ReturnLocationName(locationID, objPageDetails);
                        if (R.CashoutExists(args, objPageDetails))
                        {
                            cashout = R.ReturnSelectedCashout(args, objPageDetails)[0];

                            lblTradeInDisplay.Text = cashout.fltSystemCountedBasedOnSystemTradeIn.ToString("C");
                            lblGiftCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemGiftCard.ToString("C");
                            lblCashDisplay.Text = cashout.fltSystemCountedBasedOnSystemCash.ToString("C");
                            lblDebitDisplay.Text = cashout.fltSystemCountedBasedOnSystemDebit.ToString("C");
                            lblMasterCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemMastercard.ToString("C");
                            lblVisaDisplay.Text = cashout.fltSystemCountedBasedOnSystemVisa.ToString("C");

                            lblPreTaxDisplay.Text = cashout.fltSalesSubTotal.ToString("C");
                            lblGSTDisplay.Text = cashout.fltGovernmentTaxAmount.ToString("C");
                            lblPSTDisplay.Text = cashout.fltProvincialTaxAmount.ToString("C");
                            lblTotalDisplay.Text = (cashout.fltSystemCountedBasedOnSystemTradeIn + cashout.fltSystemCountedBasedOnSystemGiftCard + cashout.fltSystemCountedBasedOnSystemCash 
                                + cashout.fltSystemCountedBasedOnSystemDebit + cashout.fltSystemCountedBasedOnSystemMastercard + cashout.fltSystemCountedBasedOnSystemVisa).ToString("C");

                            txtTradeIn.Text = cashout.fltManuallyCountedBasedOnReceiptsTradeIn.ToString("#0.00");
                            txtGiftCard.Text = cashout.fltManuallyCountedBasedOnReceiptsGiftCard.ToString("#0.00");
                            txtCash.Text = cashout.fltManuallyCountedBasedOnReceiptsCash.ToString("#0.00");
                            txtDebit.Text = cashout.fltManuallyCountedBasedOnReceiptsDebit.ToString("#0.00");
                            txtMasterCard.Text = cashout.fltManuallyCountedBasedOnReceiptsMastercard.ToString("#0.00");
                            txtVisa.Text = cashout.fltManuallyCountedBasedOnReceiptsVisa.ToString("#0.00");

                            lblReceiptsFinal.Text = (cashout.fltManuallyCountedBasedOnReceiptsTradeIn + cashout.fltManuallyCountedBasedOnReceiptsGiftCard + cashout.fltManuallyCountedBasedOnReceiptsCash 
                                + cashout.fltManuallyCountedBasedOnReceiptsDebit + cashout.fltManuallyCountedBasedOnReceiptsMastercard + cashout.fltManuallyCountedBasedOnReceiptsVisa).ToString("C");
                            lblTotalFinal.Text = (cashout.fltSystemCountedBasedOnSystemTradeIn + cashout.fltSystemCountedBasedOnSystemGiftCard + cashout.fltSystemCountedBasedOnSystemCash 
                                + cashout.fltSystemCountedBasedOnSystemDebit + cashout.fltSystemCountedBasedOnSystemMastercard + cashout.fltSystemCountedBasedOnSystemVisa).ToString("C");
                            lblOverShortFinal.Text = cashout.fltCashDrawerOverShort.ToString("C");
                        }
                        else
                        {
                            //Creating a cashout list and calling a method that grabs all mops and amounts paid
                            cashout = R.CreateNewCashout(selectedDate, locationID, objPageDetails);
                            lblVisaDisplay.Text = cashout.fltSystemCountedBasedOnSystemVisa.ToString("C");
                            lblMasterCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemMastercard.ToString("C");
                            lblCashDisplay.Text = cashout.fltSystemCountedBasedOnSystemCash.ToString("C");
                            lblGiftCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemGiftCard.ToString("C");
                            lblDebitDisplay.Text = cashout.fltSystemCountedBasedOnSystemDebit.ToString("C");
                            lblTradeInDisplay.Text = cashout.fltSystemCountedBasedOnSystemTradeIn.ToString("C");
                            lblTotalDisplay.Text = (cashout.fltSystemCountedBasedOnSystemVisa + cashout.fltSystemCountedBasedOnSystemMastercard + cashout.fltSystemCountedBasedOnSystemCash 
                                + cashout.fltSystemCountedBasedOnSystemGiftCard + cashout.fltSystemCountedBasedOnSystemDebit + (cashout.fltSystemCountedBasedOnSystemTradeIn * -1)).ToString("C");
                            lblGSTDisplay.Text = cashout.fltGovernmentTaxAmount.ToString("C");
                            lblPSTDisplay.Text = cashout.fltProvincialTaxAmount.ToString("C");
                            lblPreTaxDisplay.Text = (cashout.fltSalesSubTotal + (cashout.fltSystemCountedBasedOnSystemTradeIn * -1)).ToString("C");

                            cashout.fltManuallyCountedBasedOnReceiptsTradeIn = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsGiftCard = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsCash = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsDebit = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsMastercard = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsVisa = 0;
                            cashout.fltCashDrawerOverShort = 0;
                            R.insertCashout(cashout, objPageDetails);
                        }
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Calculating the cashout
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnCalculate_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                calculteMethod();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Clearing the entered amounts
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnClear_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //Blanking the textboxes
                txtCash.Text = "0.00";
                txtDebit.Text = "0.00";
                txtGiftCard.Text = "0.00";
                txtMasterCard.Text = "0.00";
                txtTradeIn.Text = "0.00";
                txtVisa.Text = "0.00";
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnProcessReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProcessReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {                
                calculteMethod();
                object[] args = { DateTime.Parse(Request.QueryString["selectedDate"].ToString()), Convert.ToInt32(Request.QueryString["location"].ToString()) };
                Cashout cashout = R.ReturnSelectedCashout(args, objPageDetails)[0];

                //Creates new cashout
                cashout.fltManuallyCountedBasedOnReceiptsTradeIn = Convert.ToDouble(txtTradeIn.Text);
                cashout.fltManuallyCountedBasedOnReceiptsGiftCard = Convert.ToDouble(txtGiftCard.Text);
                cashout.fltManuallyCountedBasedOnReceiptsCash = Convert.ToDouble(txtCash.Text);
                cashout.fltManuallyCountedBasedOnReceiptsDebit = Convert.ToDouble(txtDebit.Text);
                cashout.fltManuallyCountedBasedOnReceiptsMastercard = Convert.ToDouble(txtMasterCard.Text);
                cashout.fltManuallyCountedBasedOnReceiptsVisa = Convert.ToDouble(txtVisa.Text);
                cashout.fltCashDrawerOverShort = double.Parse(lblOverShortFinal.Text, NumberStyles.Currency);
                cashout.bitIsCashoutProcessed = true;
                cashout.bitIsCashoutFinalized = false;
                cashout.intEmployeeID = CU.employee.intEmployeeID;

                R.UpdateCashout(cashout, objPageDetails);

                MessageBox.ShowMessage("Cashout has been processed", this);
                btnPrint.Enabled = true;
                btnProcessReport.Enabled = false;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void calculteMethod()
        {
            //Collects current method for error tracking
            string method = "calculateMethod";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {
                //If nothing is entered, setting text to 0.00 and the total to 0
                if (txtCash.Text == "") { txtCash.Text = "0.00"; }
                if (txtDebit.Text == "") { txtDebit.Text = "0.00"; }
                if (txtGiftCard.Text == "") { txtGiftCard.Text = "0.00"; }
                if (txtMasterCard.Text == "") { txtMasterCard.Text = "0.00"; }
                if (txtTradeIn.Text == "") { txtTradeIn.Text = "0.00"; }
                if (txtVisa.Text == "") { txtVisa.Text = "0.00"; }

                //The calculation of the receipt total
                double receiptTotal = Convert.ToDouble(txtCash.Text) + Convert.ToDouble(txtDebit.Text)
                    + Convert.ToDouble(txtGiftCard.Text) + Convert.ToDouble(txtMasterCard.Text)
                    + Convert.ToDouble(txtTradeIn.Text) + Convert.ToDouble(txtVisa.Text);

                //Setting the text for the receipt and sales totals
                lblReceiptsFinal.Text = receiptTotal.ToString("C");
                double cashoutTotal = double.Parse(lblTradeInDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblGiftCardDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblCashDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblDebitDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblMasterCardDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblVisaDisplay.Text, NumberStyles.Currency);
                lblTotalFinal.Text = cashoutTotal.ToString("C");

                //Calculating overShort
                double overShort = receiptTotal - cashoutTotal;

                lblOverShortFinal.Text = overShort.ToString("C");
                //Checking over or under
                if (overShort < 0)
                {
                    lblOverShortFinal.ForeColor = System.Drawing.Color.Red;
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]) + "-V3.2", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}