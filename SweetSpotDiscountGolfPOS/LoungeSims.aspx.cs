﻿using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotProShop;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class LoungeSims : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        InvoiceManager IM = new InvoiceManager();
        CurrentUser CU;
        private static DataTable programmed;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "Page_Load";
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("LoginPage.aspx", false);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];

                    programmed = IM.ReturnSeatedTables();
                    ButtonCheck(Page);
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void btnSimulator_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSimulator_Click";
            try
            {
                Button pressedBTN = (Button)sender;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("pressedBTN", pressedBTN.ID.ToString());
                Response.Redirect("LoungeSalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void btnPlayer_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnPlayer_Click";
            try
            {
                Button pressedBTN = (Button)sender;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("pressedBTN", pressedBTN.ID.ToString());
                Response.Redirect("LoungeSalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void btnTable_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnTable_Click";
            try
            {
                Button pressedBTN = (Button)sender;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("pressedBTN", pressedBTN.ID.ToString());
                Response.Redirect("LoungeSalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void btnSeat_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string method = "btnSeat_Click";
            try
            {
                Button pressedBTN = (Button)sender;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("pressedBTN", pressedBTN.ID.ToString());
                Response.Redirect("LoungeSalesCart.aspx?" + nameValues, false);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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

        private void ButtonCheck(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is Button)
                {
                    Button button = (Button)c;
                    foreach (DataRow dr in programmed.Rows)
                    {
                        if (button.ID.ToString() == dr[0].ToString())
                        {
                            button.BackColor = Color.Green;
                            button.Enabled = false;
                        }
                        else if(button.ID.ToString() == (dr[0].ToString() + dr[1].ToString()).ToString())
                        {
                            button.BackColor = Color.Green;
                            button.Enabled = true;
                        }
                    }
                }
                else if (c.HasControls())
                {
                    ButtonCheck(c);
                }
            }
        }
    }
}