using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class ReportsCashOut : System.Web.UI.Page
    {
        SweetShopManager ssm = new SweetShopManager();
        ErrorReporting er = new ErrorReporting();
        DateTime startDate;
        DateTime endDate;
        Employee e;
        Reports reports = new Reports();
        ItemDataUtilities idu = new ItemDataUtilities();
        CurrentUser cu = new CurrentUser();
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
            Session["currPage"] = "ReportsCashOut.aspx";
            try
            {
                cu = (CurrentUser)Session["currentUser"];
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("LoginPage.aspx", false);
                }

                //Gathering the start and end dates
                DateTime[] reportDates = (DateTime[])Session["reportDates"];
                startDate = reportDates[0];
                endDate = reportDates[1];
                //Builds string to display in label
                if (startDate == endDate)
                {
                    lblCashoutDate.Text = "Cashout for: " + startDate.ToString("d");
                }
                else
                {
                    lblCashoutDate.Text = "Cashout for: " + startDate.ToString("d") + " to " + endDate.ToString("d");
                }
                //Gathers current employe based on Session id
                //EmployeeManager em = new EmployeeManager();
                //int empNum = idu.returnEmployeeIDfromPassword(Convert.ToInt32(Session["id"]));
                //Employee emp = em.getEmployeeByID(empNum);

                int locationID = cu.locationID;
                //Creating a cashout list and calling a method that grabs all mops and amounts paid
                List<Cashout> lc = reports.cashoutAmounts(startDate, endDate, locationID);
                Cashout rc = reports.getRemainingCashout(startDate, endDate, locationID);
                //int counter = 0;
                //Looping through the list and adding up the totals
                foreach (Cashout ch in lc)
                {
                    if (ch.mop == "Visa")
                    {
                        visaTotal += ch.amount;
                    }
                    else if (ch.mop == "MasterCard")
                    {
                        mcTotal += ch.amount;
                    }
                    else if (ch.mop == "Cash")
                    {
                        cashTotal += ch.amount;
                    }
                    else if (ch.mop == "Gift Card")
                    {
                        giftCertTotal += ch.amount;
                    }
                    else if (ch.mop == "Debit")
                    {
                        debitTotal += ch.amount;
                    }
                    cashoutTotal += ch.amount;
                }
                //Gathers total amount of trade ins done through date range
                //tradeinTotal = -1 * reports.getTradeInsCashout(startDate, endDate, locationID);

                //Calculates a subtotal, gst, and pst
                subtotalTotal = rc.saleSubTotal;
                gstTotal = rc.saleGST;
                pstTotal = rc.salePST;
                tradeinTotal = rc.saleTradeIn * (-1);
                shippingTotal = rc.shippingAmount;
                cashoutTotal += tradeinTotal;
                //tradeinTotal = tradeinTotal * -1;

                Cashout cas = new Cashout(tradeinTotal, giftCertTotal, cashTotal,
                    debitTotal, mcTotal, visaTotal, gstTotal, pstTotal, subtotalTotal);
                //Store the cashout in a Session
                Session["saleCashout"] = cas;
                //Display all totals into labels
                lblVisaDisplay.Text = visaTotal.ToString("C");
                lblMasterCardDisplay.Text = mcTotal.ToString("C");
                lblCashDisplay.Text = cashTotal.ToString("C");
                lblGiftCardDisplay.Text = giftCertTotal.ToString("C");
                lblDebitDisplay.Text = debitTotal.ToString("C");
                lblTradeInDisplay.Text = tradeinTotal.ToString("C");
                lblTotalDisplay.Text = cashoutTotal.ToString("C");
                lblGSTDisplay.Text = gstTotal.ToString("C");
                lblPSTDisplay.Text = pstTotal.ToString("C");
                lblPreTaxDisplay.Text = (subtotalTotal + tradeinTotal + shippingTotal).ToString("C");
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                Cashout cas = new Cashout("lol", receiptTradeinTotal, receiptGiftCertTotal,
                    receiptCashTotal, receiptDebitTotal,
                    receiptMCTotal, receiptVisaTotal, receiptGSTTotal, receiptPSTTotal,
                    receiptSubTotalTotal, overShort);
                Session["receiptCashout"] = cas;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
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
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
        protected void btnProcessReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string method = "btnProcessReport_Click";
            try
            {
                //Sets date and time
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("HH:mm:ss");
                //Grabs cashouts from stored sessions
                Cashout s = (Cashout)Session["saleCashout"];
                Cashout r = (Cashout)Session["receiptCashout"];

                processed = true;
                //Creates new cashout
                Cashout cas = new Cashout(date, time, s.saleTradeIn, s.saleGiftCard,
                    s.saleCash, s.saleDebit, s.saleMasterCard, s.saleVisa, s.saleGST, s.salePST, s.saleSubTotal,
                    r.receiptTradeIn, r.receiptGiftCard, r.receiptCash,
                    r.receiptDebit, r.receiptMasterCard, r.receiptVisa, r.receiptGST, r.receiptPST, r.receiptSubTotal, r.overShort,
                    finalized, processed, Double.Parse(lblPreTaxDisplay.Text, System.Globalization.NumberStyles.Currency), cu.locationID, cu.empID);
                //Processes as done
                reports.insertCashout(cas);
                //Empties current cashout sessions
                Session["saleCashout"] = null;
                Session["receiptCashout"] = null;
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log employee number
                int employeeID = cu.empID;
                //Log current page
                string currPage = Convert.ToString(Session["currPage"]);
                //Log all info into error table
                er.logError(ex, employeeID, currPage, method, this);
                //string prevPage = Convert.ToString(Session["prevPage"]);
                //Display message box
                MessageBox.ShowMessage("An Error has occured and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator", this);
                //Server.Transfer(prevPage, false);
            }
        }
    }
}