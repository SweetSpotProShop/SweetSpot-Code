using SweetShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class TaxManager
    {
        DatabaseCalls dbc = new DatabaseCalls();

        private List<Tax> ConvertFromDataTableToTax(DataTable dt)
        {
            List<Tax> tax = dt.AsEnumerable().Select(row =>
            new Tax
            {
                intTaxID = row.Field<int>("intTaxID"),
                fltTaxRate = row.Field<double>("fltTaxRate")
            }).ToList();
            return tax;
        }
        private List<Tax> ReturnListOfTaxes(DataTable dt)
        {
            List<Tax> tax = dt.AsEnumerable().Select(row =>
            new Tax
            {
                fltTaxRate = row.Field<double>("fltTaxRate"),
                varTaxName = row.Field<string>("varTaxName")
            }).ToList();
            return tax;
        }
        public List<Tax> ReturnTaxListBasedOnDate(DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            return ConvertFromDataTableToTax(ReturnTaxListBasedOnDateAndProvinceForUpdate(provinceID, selectedDate, objPageDetails));
        }
        public DataTable ReturnTaxListBasedOnDateAndProvinceForUpdate (int provinceID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "ReturnTaxListBasedOnDateAndProvinceForUpdate";
            string sqlCmd = "SELECT TR.intTaxID, TR.fltTaxRate, TT.varTaxName FROM tbl_taxRate AS TR INNER JOIN tbl_taxType TT ON "
                + "TR.intTaxID = TT.intTaxID INNER JOIN(SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate WHERE "
                + "(dtmTaxEffectiveDate <= @dtmSelectedDate) AND (intProvinceID = @intProvinceID) GROUP BY intTaxID) AS TD ON TR.intTaxID "
                + "= TD.intTaxID AND TR.dtmTaxEffectiveDate = TD.MTD WHERE (TR.intProvinceID = @intProvinceID)";
            object[][] parms =
            {
                new object[] { "@dtmSelectedDate", selectedDate },
                new object[] { "@intProvinceID", provinceID }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }

        private List<Tax> getTaxes(int provinceID, DateTime selectedDate, object[] objPageDetails)
        {
            string strQueryName = "getTaxes";
            //New command
            string sqlCmd = "SELECT TR.fltTaxRate, TT.varTaxName FROM tbl_taxRate TR INNER JOIN tbl_taxType TT ON TR.intTaxID = TT.intTaxID "
                + "INNER JOIN (SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate WHERE dtmTaxEffectiveDate <= @selectedDate "
                + "AND intProvinceID = @intProvinceID GROUP BY intTaxID) TD ON TR.intTaxID = TD.intTaxID AND TR.dtmTaxEffectiveDate = "
                + "TD.MTD WHERE intProvinceID = @intProvinceID";

            object[][] parms =
            {
                new object[] { "@intProvinceID", provinceID },
                new object[] { "@selectedDate", selectedDate }
            };
            //Returns the list of taxes
            return ReturnListOfTaxes(dbc.returnDataTableData(sqlCmd, parms));
            //return ReturnListOfTaxes(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        public DataTable ReturnTaxList()
        {
            return GatherFullTaxList();
        }
        private DataTable GatherFullTaxList()
        {
            string sqlCmd = "SELECT intTaxID FROM tbl_taxType";
            object[][] parms = { };
            return dbc.returnDataTableData(sqlCmd, parms);
        }
        public void InsertNewTaxRate(int provinceID, int taxID, DateTime selectedDate, double taxRate, object[] objPageDetails)
        {
            string strQueryName = "InsertNewTaxRate";
            string sqlCmd = "INSERT INTO tbl_taxRate VALUES(@intProvinceID, @dtmTaxDate, @intTaxID, @fltTaxRate)";
            object[][] parms =
            {
                new object[] { "@intProvinceID", provinceID },
                new object[] { "@dtmTaxDate", selectedDate },
                new object[] { "@intTaxID", taxID },
                new object[] { "@fltTaxRate", taxRate }
            };
            dbc.executeInsertQuery(sqlCmd,parms);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public object[] ReturnChargedTaxForSale(Invoice invoice, object[] btnRequirements, object[] objPageDetails)
        {
            int prov = invoice.location.intProvinceID;
            if (invoice.fltShippingCharges > 0)
            {
                prov = invoice.customer.intProvinceID;
            }
            List<Tax> t = getTaxes(prov, invoice.dtmInvoiceDate, objPageDetails);
            
            double totalTax = 0;
            bool bolGSTDisplay = false;
            bool bolPSTDisplay = false;
            string gstText = btnRequirements[0].ToString();
            string pstText = btnRequirements[1].ToString();
            foreach (var T in t)
            {
                double taxAmount = 0;
                switch (T.varTaxName)
                {
                    //If tax is GST calculate and make visible
                    case "GST":
                        if (gstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of GST
                            taxAmount = -(invoice.fltGovernmentTaxAmount);
                            invoice.bitChargeGST = false;
                            //Changes button name
                            gstText = "Add GST";
                        }
                        else if (gstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of GST
                            invoice.fltGovernmentTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal + invoice.fltShippingCharges);
                            taxAmount = invoice.fltGovernmentTaxAmount;
                            invoice.bitChargeGST = true;
                            //Changes button name
                            gstText = "Remove GST";
                        }
                        bolGSTDisplay = true;
                        break;
                    //If tax is PST calculate and make visible
                    case "PST":
                        if (pstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of PST
                            taxAmount = -(invoice.fltProvincialTaxAmount);
                            invoice.bitChargePST = false;
                            //Changes button name
                            pstText = "Add PST"; //*** Need to figure out proper name of tax
                        }
                        else if (pstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of PST
                            invoice.fltProvincialTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal);
                            taxAmount = invoice.fltProvincialTaxAmount;
                            invoice.bitChargePST = true;
                            //Changes button name
                            pstText = "Remove PST";
                        }
                        bolPSTDisplay = true;
                        break;
                    //If tax is HST calculate and make visible
                    case "HST":
                        if (gstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of GST
                            taxAmount = -(invoice.fltGovernmentTaxAmount);
                            invoice.bitChargeGST = false;
                            //Changes button name
                            gstText = "Add HST";
                        }
                        else if (gstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of GST
                            invoice.fltGovernmentTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal + invoice.fltShippingCharges);
                            taxAmount = invoice.fltGovernmentTaxAmount;
                            invoice.bitChargeGST = true;
                            //Changes button name
                            gstText = "Remove HST";
                        }
                        bolGSTDisplay = true;
                        break;
                    //If tax is RST calculate and make visible
                    case "RST":
                        if (pstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of PST
                            taxAmount = -(invoice.fltProvincialTaxAmount);
                            invoice.bitChargePST = false;
                            //Changes button name
                            pstText = "Add RST"; //*** Need to figure out proper name of tax
                        }
                        else if (pstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of PST
                            invoice.fltProvincialTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal);
                            taxAmount = invoice.fltProvincialTaxAmount;
                            invoice.bitChargePST = true;
                            //Changes button name
                            pstText = "Remove RST";
                        }
                        bolPSTDisplay = true;
                        break;
                    //If tax is QST calculate and make visible
                    case "QST":
                        if (pstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of PST
                            taxAmount = -(invoice.fltProvincialTaxAmount);
                            invoice.bitChargePST = false;
                            //Changes button name
                            pstText = "Add QST"; //*** Need to figure out proper name of tax
                        }
                        else if (pstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of PST
                            invoice.fltProvincialTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal);
                            taxAmount = invoice.fltProvincialTaxAmount;
                            invoice.bitChargePST = true;
                            //Changes button name
                            pstText = "Remove QST";
                        }
                        bolPSTDisplay = true;
                        break;
                }
                //totalTax += taxAmount;
            }

            object[] btnParms = { bolGSTDisplay, gstText, bolPSTDisplay, pstText };
            InvoiceManager IM = new InvoiceManager();
            IM.UpdateCurrentInvoice(invoice, objPageDetails);
            object[] results = { invoice, btnParms };
            return results;
        }
        private double CallReturnTaxAmount(double rate, double subTotal)
        {
            return returnTaxAmount(rate, subTotal);
        }
        //This method returns the tax amount of the cart based on subtotal **Checked and Verified
        private double returnTaxAmount(double rate, double subtotal)
        {
            double TaxAmount = 0;
            TaxAmount = Math.Round((rate * subtotal), 2);
            //Returns the gst amount 
            return TaxAmount;
        }
        public void TaxCorrectionOnReturnInvoice(Invoice invoice, object[] objPageDetails)
        {
            invoice.fltBalanceDue = invoice.fltBalanceDue - (invoice.fltGovernmentTaxAmount + invoice.fltProvincialTaxAmount);
            InvoiceManager IM = new InvoiceManager();
            IM.CalculateNewInvoiceReturnTotalsToUpdate(invoice, objPageDetails);
        }



        public void UpdateTaxChargedForInventory(int inventoryID, int taxID, bool chargeTax)
        {
            SetTaxChargedForInventory(inventoryID, taxID, chargeTax);
        }

        private void SetTaxChargedForInventory(int inventoryID, int taxID, bool chargeTax)
        {
            string sqlCmd = "UPDATE tbl_taxTypePerInventoryItem SET bitChargeTax = @bitChargeTax WHERE "
                + "intIventoryID = @intInventoryID AND intTaxID = @intTaxID";
            object[][] parms =
            {
                new object[] { "@bitChargeTax", chargeTax },
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intTaxID", taxID }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
    }
}