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
        LocationManager L = new LocationManager();

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
                    CU = (CurrentUser)Session["currentUser"];
                    if (!IsPostBack)
                    {
                        //Gathering the start and end dates
                        DateTime startDate = DateTime.Parse(Request.QueryString["dtm"].ToString());
                        int loc = Convert.ToInt32(Request.QueryString["location"]);
                        object[] args = { startDate, loc };
                        lblCashoutDate.Text = "Cashout on: " + startDate.ToString("d") + " for " + L.locationName(loc);
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
                            Cashout C = R.CreateNewCashout(startDate, loc);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
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
                DateTime startDate = DateTime.Parse(Request.QueryString["dtm"].ToString());
                int loc = Convert.ToInt32(Request.QueryString["location"]);
                object[] args = { startDate, loc };

                //DateTime param1 = startDate;
                //DateTime param2 = startDate;
                //double param3 = double.Parse(lblTradeInDisplay.Text, NumberStyles.Currency);
                //double param4 = double.Parse(lblGiftCardDisplay.Text, NumberStyles.Currency);
                //double param5 = double.Parse(lblCashDisplay.Text, NumberStyles.Currency);
                //double param6 = double.Parse(lblDebitDisplay.Text, NumberStyles.Currency);
                //double param7 = double.Parse(lblMasterCardDisplay.Text, NumberStyles.Currency);
                //double param8 = double.Parse(lblVisaDisplay.Text, NumberStyles.Currency);
                //double param9 = Convert.ToDouble(txtTradeIn.Text);
                //double param10 = Convert.ToDouble(txtGiftCard.Text);
                //double param11 = Convert.ToDouble(txtCash.Text);
                //double param12 = Convert.ToDouble(txtDebit.Text);
                //double param13 = Convert.ToDouble(txtMasterCard.Text);
                //double param14 = Convert.ToDouble(txtVisa.Text);
                //double param15 = double.Parse(lblPreTaxDisplay.Text, NumberStyles.Currency);
                //double param16 = double.Parse(lblGSTDisplay.Text, NumberStyles.Currency);
                //double param17 = double.Parse(lblPSTDisplay.Text, NumberStyles.Currency);
                //double param18 = double.Parse(lblOverShortFinal.Text, NumberStyles.Currency);
                //bool param19 = false;
                //bool param20 = true;
                //int param21 = loc;
                //int param22 = CU.emp.employeeID;

                //Cashout cas = new Cashout(param1, param2, param3, param4, param5, param6, param7, param8, param9, param10,
                //    param11, param12, param13, param14, param15, param16, param17, param18, param19, param20, param21, param22);


                //Creates new cashout
                Cashout cas = new Cashout(startDate, startDate, double.Parse(lblTradeInDisplay.Text, NumberStyles.Currency),
                    double.Parse(lblGiftCardDisplay.Text, NumberStyles.Currency), double.Parse(lblCashDisplay.Text, NumberStyles.Currency),
                    double.Parse(lblDebitDisplay.Text, NumberStyles.Currency), double.Parse(lblMasterCardDisplay.Text, NumberStyles.Currency),
                    double.Parse(lblVisaDisplay.Text, NumberStyles.Currency), Convert.ToDouble(txtTradeIn.Text), Convert.ToDouble(txtGiftCard.Text),
                    Convert.ToDouble(txtCash.Text), Convert.ToDouble(txtDebit.Text), Convert.ToDouble(txtMasterCard.Text), Convert.ToDouble(txtVisa.Text),
                    double.Parse(lblPreTaxDisplay.Text, NumberStyles.Currency), double.Parse(lblGSTDisplay.Text, NumberStyles.Currency),
                    double.Parse(lblPSTDisplay.Text, NumberStyles.Currency), double.Parse(lblOverShortFinal.Text, NumberStyles.Currency), false, true, loc, CU.emp.employeeID);

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
                ER.logError(ex, CU.emp.employeeID, Convert.ToString(Session["currPage"]) + "-V3", method, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}