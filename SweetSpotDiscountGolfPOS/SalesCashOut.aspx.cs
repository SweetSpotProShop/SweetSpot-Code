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
        CurrentUser CU = new CurrentUser();
        Reports R = new Reports();

        SweetShopManager ssm = new SweetShopManager();
        Employee e;
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager l = new LocationManager();
        double cashoutTotal;
        double mcTotal = 0;
        double visaTotal = 0;
        double giftCertTotal = 0;
        double cashTotal = 0;
        double debitTotal = 0;
        double tradeinTotal = 0;
        double subtotalTotal;
        double pstTotal = 0;
        double gstTotal = 0;
        double receiptTotal = 0;
        double receiptMCTotal;// = 0;
        double receiptVisaTotal;// = 0;        
        double receiptGiftCertTotal;// = 0;
        double receiptCashTotal;// = 0;        
        double receiptDebitTotal;// = 0;
        double receiptSubTotalTotal;
        double receiptGSTTotal;
        double receiptPSTTotal;
        double receiptTradeinTotal;// = 0;
        double overShort = 0;
        bool finalized = false;
        bool processed = false;
        double shippingTotal;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            Session["currPage"] = "SalesCashOut.aspx";
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
                    if (!IsPostBack)
                    {
                        CU = (CurrentUser)Session["currentUser"];
                        //Gathering the start and end dates
                        DateTime startDate = DateTime.Parse(Request.QueryString["dtm"].ToString());
                        int loc = Convert.ToInt32(Request.QueryString["location"]);
                        object[] args = { startDate, loc };
                        lblCashoutDate.Text = "Cashout on: " + startDate.ToString("d") + " for " + l.locationName(loc);
                        if (R.CashoutExists(args))
                        {
                            Cashout C = R.ReturnSelectedCashout(args)[0];

                            lblTradeInDisplay.Text = C.saleTradeIn.ToString("C");
                            lblGiftCardDisplay.Text = C.saleGiftCard.ToString("C");
                            lblCashDisplay.Text = C.saleCash.ToString("C");
                            lblDebitDisplay.Text = C.saleDebit.ToString("C");
                            lblMasterCardDisplay.Text = C.saleMasterCard.ToString("C");
                            lblVisaDisplay.Text = C.saleVisa.ToString("C");

                            lblPreTaxDisplay.Text = C.preTax.ToString("C");
                            lblGSTDisplay.Text = C.saleGST.ToString("C");
                            lblPSTDisplay.Text = C.salePST.ToString("C");
                            lblTotalDisplay.Text = (C.saleTradeIn + C.saleGiftCard + C.saleCash + C.saleDebit + C.saleMasterCard + C.saleVisa).ToString("C");

                            txtTradeIn.Text = C.receiptTradeIn.ToString();
                            txtGiftCard.Text = C.receiptGiftCard.ToString();
                            txtCash.Text = C.receiptCash.ToString();
                            txtDebit.Text = C.receiptDebit.ToString();
                            txtMasterCard.Text = C.receiptMasterCard.ToString();
                            txtVisa.Text = C.receiptVisa.ToString();

                            lblReceiptsFinal.Text = (C.receiptTradeIn + C.receiptGiftCard + C.receiptCash + C.receiptDebit + C.receiptMasterCard + C.receiptVisa).ToString("C");
                            lblTotalFinal.Text = (C.saleTradeIn + C.saleGiftCard + C.saleCash + C.saleDebit + C.saleMasterCard + C.saleVisa).ToString("C");
                            lblOverShortFinal.Text = C.overShort.ToString("C");
                        }
                        else
                        {
                            //Creating a cashout list and calling a method that grabs all mops and amounts paid
                            //List<Cashout> lc = R.cashoutAmounts(startDate, startDate, loc);
                            Cashout C = R.CreateNewCashout(startDate, loc);
                            //Looping through the list and adding up the totals
                            //foreach (Cashout ch in lc)
                            //{
                            //    if (ch.mop == "Visa")
                            //    {
                            //        visaTotal += ch.amount;
                            //    }
                            //    else if (ch.mop == "MasterCard")
                            //    {
                            //        mcTotal += ch.amount;
                            //    }
                            //    else if (ch.mop == "Cash")
                            //    {
                            //        cashTotal += ch.amount;
                            //    }
                            //    else if (ch.mop == "Gift Card")
                            //    {
                            //        giftCertTotal += ch.amount;
                            //    }
                            //    else if (ch.mop == "Debit")
                            //    {
                            //        debitTotal += ch.amount;
                            //    }
                            //    cashoutTotal += ch.amount;
                            //}
                            ////Gathers total amount of trade ins done through date range

                            ////Calculates a subtotal, gst, and pst
                            //subtotalTotal = rc.saleSubTotal;
                            //gstTotal = rc.saleGST;
                            //pstTotal = rc.salePST;
                            //tradeinTotal = rc.saleTradeIn * (-1);
                            //shippingTotal = rc.shippingAmount;
                            //cashoutTotal += tradeinTotal;

                            //Cashout cas = new Cashout(tradeinTotal, giftCertTotal, cashTotal,
                            //    debitTotal, mcTotal, visaTotal, gstTotal, pstTotal, subtotalTotal);
                            ////Store the cashout in a Session
                            //Session["saleCashout"] = cas;
                            //Display all totals into labels
                            lblVisaDisplay.Text = C.saleVisa.ToString("C");
                            lblMasterCardDisplay.Text = C.saleMasterCard.ToString("C");
                            lblCashDisplay.Text = C.saleCash.ToString("C");
                            lblGiftCardDisplay.Text = C.saleGiftCard.ToString("C");
                            lblDebitDisplay.Text = C.saleDebit.ToString("C");
                            lblTradeInDisplay.Text = (C.saleTradeIn * -1).ToString("C");
                            lblTotalDisplay.Text = (C.saleVisa + C.saleMasterCard + C.saleCash + C.saleGiftCard + C.saleDebit + (C.saleTradeIn * -1)).ToString("C");
                            lblGSTDisplay.Text = C.saleGST.ToString("C");
                            lblPSTDisplay.Text = C.salePST.ToString("C");
                            lblPreTaxDisplay.Text = (C.preTax + (C.saleTradeIn * -1)).ToString("C");
                        }
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
            try
            {
                //If nothing is entered, setting text to 0.00 and the total to 0
                if (txtCash.Text == "") { txtCash.Text = "0.00"; receiptCashTotal = 0; }
                if (txtDebit.Text == "") { txtDebit.Text = "0.00"; receiptDebitTotal = 0; }
                if (txtGiftCard.Text == "") { txtGiftCard.Text = "0.00"; receiptGiftCertTotal = 0; }
                if (txtMasterCard.Text == "") { txtMasterCard.Text = "0.00"; receiptMCTotal = 0; }
                if (txtTradeIn.Text == "") { txtTradeIn.Text = "0.00"; receiptTradeinTotal = 0; }
                if (txtVisa.Text == "") { txtVisa.Text = "0.00"; receiptVisaTotal = 0; }

                //Giving values to the entered totals
                receiptCashTotal = Convert.ToDouble(txtCash.Text);
                receiptDebitTotal = Convert.ToDouble(txtDebit.Text);
                receiptGiftCertTotal = Convert.ToDouble(txtGiftCard.Text);
                receiptMCTotal = Convert.ToDouble(txtMasterCard.Text);
                receiptTradeinTotal = Convert.ToDouble(txtTradeIn.Text);
                receiptVisaTotal = Convert.ToDouble(txtVisa.Text);

                //The calculation of the receipt total
                receiptTotal = receiptCashTotal +
                    receiptDebitTotal + receiptGiftCertTotal + receiptMCTotal +
                    receiptTradeinTotal + receiptVisaTotal;

                //Setting the text for the receipt and sales totals
                lblReceiptsFinal.Text = receiptTotal.ToString("C");
                cashoutTotal = double.Parse(lblTradeInDisplay.Text, NumberStyles.Currency) 
                    + double.Parse(lblGiftCardDisplay.Text, NumberStyles.Currency) 
                    + double.Parse(lblCashDisplay.Text, NumberStyles.Currency) 
                    + double.Parse(lblDebitDisplay.Text, NumberStyles.Currency) 
                    + double.Parse(lblMasterCardDisplay.Text, NumberStyles.Currency) 
                    + double.Parse(lblVisaDisplay.Text, NumberStyles.Currency);
                lblTotalFinal.Text = cashoutTotal.ToString("C");

                //Calculating overShort
                overShort = receiptTotal - cashoutTotal;

                //Checking over or under
                if (overShort >= 0)
                {
                    lblOverShortFinal.Text = overShort.ToString("C");
                }
                else if (overShort < 0)
                {
                    lblOverShortFinal.Text = "(" + overShort.ToString("C") + ")";
                }

                //Storing in session
                //Cashout cas = new Cashout("lol", receiptTradeinTotal, receiptGiftCertTotal,
                //    receiptCashTotal, receiptDebitTotal,
                //    receiptMCTotal, receiptVisaTotal, receiptGSTTotal, receiptPSTTotal,
                //    receiptSubTotalTotal, overShort);
                //Session["receiptCashout"] = cas;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
            try
            {
                //Blanking the textboxes
                txtCash.Text = "";
                txtDebit.Text = "";
                txtGiftCard.Text = "";
                txtMasterCard.Text = "";
                txtTradeIn.Text = "";
                txtVisa.Text = "";
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void printReport(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "printReport";
            //Current method does nothing
            try
            { }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
            try
            {
                //Sets date and time
                //Object[] repInfo = (Object[])Session["reportInfo"];
                //DateTime[] reportDates = (DateTime[])repInfo[0];
                //string date = reportDates[0].ToString();
                //Grabs cashouts from stored sessions
                //Cashout s = (Cashout)Session["saleCashout"];
                //Cashout r = (Cashout)Session["receiptCashout"];

                DateTime startDate = DateTime.Parse(Request.QueryString["dtm"].ToString());
                int loc = Convert.ToInt32(Request.QueryString["location"]);
                object[] args = { startDate, loc };

                processed = true;
                //Creates new cashout
                Cashout cas = new Cashout(startDate, startDate, double.Parse(lblTradeInDisplay.Text, NumberStyles.Currency), 
                    double.Parse(lblGiftCardDisplay.Text, NumberStyles.Currency), double.Parse(lblCashDisplay.Text, NumberStyles.Currency), 
                    double.Parse(lblDebitDisplay.Text, NumberStyles.Currency), double.Parse(lblMasterCardDisplay.Text, NumberStyles.Currency),
                    double.Parse(lblVisaDisplay.Text, NumberStyles.Currency), Convert.ToDouble(txtTradeIn.Text), Convert.ToDouble(txtGiftCard.Text), 
                    Convert.ToDouble(txtCash.Text), Convert.ToDouble(txtDebit.Text), Convert.ToDouble(txtMasterCard.Text), Convert.ToDouble(txtVisa.Text),
                    double.Parse(lblPreTaxDisplay.Text, NumberStyles.Currency), double.Parse(lblGSTDisplay.Text, NumberStyles.Currency),
                    double.Parse(lblPSTDisplay.Text, NumberStyles.Currency), double.Parse(lblOverShortFinal.Text, NumberStyles.Currency), finalized, processed, loc, CU.empID);

                //Processes as done
                if (R.CashoutExists(args))
                {
                    R.UpdateCashout(cas);
                }
                else
                {
                    R.insertCashout(cas);
                }
                //Empties current cashout sessions
                Session["saleCashout"] = null;
                Session["receiptCashout"] = null;
                MessageBox.ShowMessage("Cashout has been processed", this);
                btnPrint.Enabled = true;
                btnProcessReport.Enabled = false;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU.empID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}