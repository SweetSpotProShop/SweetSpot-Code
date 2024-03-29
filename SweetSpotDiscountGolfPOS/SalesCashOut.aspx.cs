﻿using System;
using System.Globalization;
using System.Threading;
using SweetSpotDiscountGolfPOS.FP;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS
{
    public partial class SalesCashOut : System.Web.UI.Page
    {
        readonly ErrorReporting ER = new ErrorReporting();
        readonly LocationManager LM = new LocationManager();
        readonly CashoutUtilities COU = new CashoutUtilities();
        CurrentUser CU;

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
                        lblCashoutDate.Text = "Cashout on: " + selectedDate.ToString("dd/MMM/yy") + " for " + LM.CallReturnLocationName(locationID, objPageDetails);
                        if (COU.CallCashoutExists(args, objPageDetails))
                        {
                            cashout = COU.CallSelectedCashoutToReturn(args, objPageDetails);

                            lblTradeInDisplay.Text = cashout.fltSystemCountedBasedOnSystemTradeIn.ToString("C");
                            lblGiftCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemGiftCard.ToString("C");
                            lblCashDisplay.Text = cashout.fltSystemCountedBasedOnSystemCash.ToString("C");
                            lblDebitDisplay.Text = cashout.fltSystemCountedBasedOnSystemDebit.ToString("C");
                            lblMasterCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemMastercard.ToString("C");
                            lblVisaDisplay.Text = cashout.fltSystemCountedBasedOnSystemVisa.ToString("C");
                            lblAmExDisplay.Text = cashout.fltSystemCountedBasedOnSystemAmEx.ToString("C");

                            lblPreTaxDisplay.Text = (cashout.fltSalesSubTotal + cashout.fltSystemCountedBasedOnSystemTradeIn).ToString("C");
                            lblGSTDisplay.Text = cashout.fltGovernmentTaxAmount.ToString("C");
                            lblHSTDisplay.Text = cashout.fltHarmonizedTaxAmount.ToString("C");
                            lblLCTDisplay.Text = cashout.fltLiquorTaxAmount.ToString("C");
                            lblPSTDisplay.Text = cashout.fltProvincialTaxAmount.ToString("C");
                            lblQSTDisplay.Text = cashout.fltQuebecTaxAmount.ToString("C");
                            lblRSTDisplay.Text = cashout.fltRetailTaxAmount.ToString("C");


                            lblTotalDisplay.Text = (cashout.fltSystemCountedBasedOnSystemTradeIn + cashout.fltSystemCountedBasedOnSystemGiftCard + cashout.fltSystemCountedBasedOnSystemCash 
                                + cashout.fltSystemCountedBasedOnSystemDebit + cashout.fltSystemCountedBasedOnSystemMastercard + cashout.fltSystemCountedBasedOnSystemVisa
                                + cashout.fltSystemCountedBasedOnSystemAmEx).ToString("C");

                            txtTradeIn.Text = cashout.fltManuallyCountedBasedOnReceiptsTradeIn.ToString("#0.00");
                            txtGiftCard.Text = cashout.fltManuallyCountedBasedOnReceiptsGiftCard.ToString("#0.00");
                            txtCash.Text = cashout.fltManuallyCountedBasedOnReceiptsCash.ToString("#0.00");
                            txtDebit.Text = cashout.fltManuallyCountedBasedOnReceiptsDebit.ToString("#0.00");
                            txtMasterCard.Text = cashout.fltManuallyCountedBasedOnReceiptsMastercard.ToString("#0.00");
                            txtVisa.Text = cashout.fltManuallyCountedBasedOnReceiptsVisa.ToString("#0.00");
                            txtAmEx.Text = cashout.fltManuallyCountedBasedOnReceiptsAmEx.ToString("#0.00");

                            lblReceiptsFinal.Text = (cashout.fltManuallyCountedBasedOnReceiptsTradeIn + cashout.fltManuallyCountedBasedOnReceiptsGiftCard + cashout.fltManuallyCountedBasedOnReceiptsCash 
                                + cashout.fltManuallyCountedBasedOnReceiptsDebit + cashout.fltManuallyCountedBasedOnReceiptsMastercard + cashout.fltManuallyCountedBasedOnReceiptsVisa
                                + cashout.fltManuallyCountedBasedOnReceiptsAmEx).ToString("C");
                            lblTotalFinal.Text = (cashout.fltSystemCountedBasedOnSystemTradeIn + cashout.fltSystemCountedBasedOnSystemGiftCard + cashout.fltSystemCountedBasedOnSystemCash 
                                + cashout.fltSystemCountedBasedOnSystemDebit + cashout.fltSystemCountedBasedOnSystemMastercard + cashout.fltSystemCountedBasedOnSystemVisa
                                + cashout.fltSystemCountedBasedOnSystemAmEx).ToString("C");
                            lblOverShortFinal.Text = cashout.fltCashDrawerOverShort.ToString("C");
                        }
                        else
                        {
                            //Creating a cashout list and calling a method that grabs all mops and amounts paid
                            cashout = COU.CreateNewCashout(selectedDate, locationID, objPageDetails);
                            lblVisaDisplay.Text = cashout.fltSystemCountedBasedOnSystemVisa.ToString("C");
                            lblAmExDisplay.Text = cashout.fltSystemCountedBasedOnSystemAmEx.ToString("C");
                            lblMasterCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemMastercard.ToString("C");
                            lblCashDisplay.Text = cashout.fltSystemCountedBasedOnSystemCash.ToString("C");
                            lblGiftCardDisplay.Text = cashout.fltSystemCountedBasedOnSystemGiftCard.ToString("C");
                            lblDebitDisplay.Text = cashout.fltSystemCountedBasedOnSystemDebit.ToString("C");
                            lblTradeInDisplay.Text = cashout.fltSystemCountedBasedOnSystemTradeIn.ToString("C");
                            lblTotalDisplay.Text = (cashout.fltSystemCountedBasedOnSystemVisa + cashout.fltSystemCountedBasedOnSystemAmEx + cashout.fltSystemCountedBasedOnSystemMastercard + cashout.fltSystemCountedBasedOnSystemCash 
                                + cashout.fltSystemCountedBasedOnSystemGiftCard + cashout.fltSystemCountedBasedOnSystemDebit + (cashout.fltSystemCountedBasedOnSystemTradeIn)).ToString("C");
                            lblGSTDisplay.Text = cashout.fltGovernmentTaxAmount.ToString("C");
                            lblHSTDisplay.Text = cashout.fltHarmonizedTaxAmount.ToString("C");
                            lblLCTDisplay.Text = cashout.fltLiquorTaxAmount.ToString("C");
                            lblPSTDisplay.Text = cashout.fltProvincialTaxAmount.ToString("C");
                            lblQSTDisplay.Text = cashout.fltQuebecTaxAmount.ToString("C");
                            lblRSTDisplay.Text = cashout.fltRetailTaxAmount.ToString("C");
                            lblPreTaxDisplay.Text = (cashout.fltSalesSubTotal + cashout.fltSystemCountedBasedOnSystemTradeIn).ToString("C");

                            cashout.fltManuallyCountedBasedOnReceiptsTradeIn = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsGiftCard = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsCash = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsDebit = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsMastercard = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsVisa = 0;
                            cashout.fltManuallyCountedBasedOnReceiptsAmEx = 0;
                            cashout.fltCashDrawerOverShort = 0;
                            COU.CallInsertCashout(cashout, objPageDetails);
                        }

                        if (cashout.fltGovernmentTaxAmount == 0)
                        {
                            cellGSTS.Visible = false;
                            cellGSTDisplay.Visible = false;
                        }
                        if (cashout.fltHarmonizedTaxAmount == 0)
                        {
                            cellHSTS.Visible = false;
                            cellHSTDisplay.Visible = false;
                        }
                        if (cashout.fltLiquorTaxAmount == 0)
                        {
                            cellLCTS.Visible = false;
                            cellLCTDisplay.Visible = false;
                        }
                        if (cashout.fltProvincialTaxAmount == 0)
                        {
                            cellPSTS.Visible = false;
                            cellPSTDisplay.Visible = false;
                        }
                        if (cashout.fltQuebecTaxAmount == 0)
                        {
                            cellQSTS.Visible = false;
                            cellQSTDisplay.Visible = false;
                        }
                        if (cashout.fltRetailTaxAmount == 0)
                        {
                            cellRSTS.Visible = false;
                            cellRSTDisplay.Visible = false;
                        }
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Calculating the cashout
        protected void BtnCalculate_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnCalculate_Click";
            try
            {
                CalculteMethod();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        //Clearing the entered amounts
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnClear_Click";
            try
            {
                //Blanking the textboxes
                txtCash.Text = "0.00";
                txtDebit.Text = "0.00";
                txtGiftCard.Text = "0.00";
                txtMasterCard.Text = "0.00";
                txtTradeIn.Text = "0.00";
                txtVisa.Text = "0.00";
                txtAmEx.Text = "0.00";
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void BtnProcessReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "BtnProcessReport_Click";
            object[] objPageDetails = { Session["currPage"].ToString(), method };
            try
            {                
                CalculteMethod();
                object[] args = { DateTime.Parse(Request.QueryString["selectedDate"].ToString()), Convert.ToInt32(Request.QueryString["location"].ToString()) };
                Cashout cashout = COU.CallSelectedCashoutToReturn(args, objPageDetails);

                //Creates new cashout
                cashout.fltManuallyCountedBasedOnReceiptsTradeIn = Convert.ToDouble(txtTradeIn.Text);
                cashout.fltManuallyCountedBasedOnReceiptsGiftCard = Convert.ToDouble(txtGiftCard.Text);
                cashout.fltManuallyCountedBasedOnReceiptsCash = Convert.ToDouble(txtCash.Text);
                cashout.fltManuallyCountedBasedOnReceiptsDebit = Convert.ToDouble(txtDebit.Text);
                cashout.fltManuallyCountedBasedOnReceiptsMastercard = Convert.ToDouble(txtMasterCard.Text);
                cashout.fltManuallyCountedBasedOnReceiptsVisa = Convert.ToDouble(txtVisa.Text);
                cashout.fltManuallyCountedBasedOnReceiptsAmEx = Convert.ToDouble(txtAmEx.Text);
                cashout.fltCashDrawerOverShort = double.Parse(lblOverShortFinal.Text, NumberStyles.Currency);
                cashout.bitIsCashoutProcessed = true;
                cashout.bitIsCashoutFinalized = false;
                cashout.intEmployeeID = CU.employee.intEmployeeID;

                COU.CallUpdateCashout(cashout, objPageDetails);

                //this is where we will put the update query
                //update the daily sales data table
                COU.CollectAndStoreDailySalesData(cashout.dtmCashoutDate, cashout.intLocationID, objPageDetails);

                MessageBoxCustom.ShowMessage("Cashout has been processed", this);
                btnPrint.Enabled = true;
                BtnProcessReport.Enabled = false;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        private void CalculteMethod()
        {
            //Collects current method for error tracking
            string method = "CalculateMethod";
            try
            {
                //If nothing is entered, setting text to 0.00 and the total to 0
                if (txtCash.Text == "") { txtCash.Text = "0.00"; }
                if (txtDebit.Text == "") { txtDebit.Text = "0.00"; }
                if (txtGiftCard.Text == "") { txtGiftCard.Text = "0.00"; }
                if (txtMasterCard.Text == "") { txtMasterCard.Text = "0.00"; }
                if (txtTradeIn.Text == "") { txtTradeIn.Text = "0.00"; }
                if (txtVisa.Text == "") { txtVisa.Text = "0.00"; }
                if (txtAmEx.Text == "") { txtAmEx.Text = "0.00"; }

                //The calculation of the receipt total
                double receiptTotal = Convert.ToDouble(txtCash.Text) + Convert.ToDouble(txtDebit.Text)
                    + Convert.ToDouble(txtGiftCard.Text) + Convert.ToDouble(txtMasterCard.Text)
                    + Convert.ToDouble(txtTradeIn.Text) + Convert.ToDouble(txtVisa.Text) + Convert.ToDouble(txtAmEx.Text);

                //Setting the text for the receipt and sales totals
                lblReceiptsFinal.Text = receiptTotal.ToString("C");
                double cashoutTotal = double.Parse(lblTradeInDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblGiftCardDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblCashDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblDebitDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblMasterCardDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblVisaDisplay.Text, NumberStyles.Currency)
                    + double.Parse(lblAmExDisplay.Text, NumberStyles.Currency);
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
                ER.CallLogError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currPage"]), method, this);
                //Display message box
                MessageBoxCustom.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}