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
                taxID = row.Field<int>("taxID"),
                taxRate = row.Field<double>("taxRate")
            }).ToList();
            return tax;
        }
        private List<Tax> ReturnListOfTaxes(DataTable dt)
        {
            List<Tax> t = dt.AsEnumerable().Select(row =>
            new Tax
            {
                taxRate = row.Field<double>("taxRate"),
                taxName = row.Field<string>("taxName")
            }).ToList();
            return t;
        }
        public List<Tax> ReturnTaxListBasedOnDate(DateTime selectedDate, int provinceID)
        {
            return ConvertFromDataTableToTax(ReturnTaxListBasedOnDateAndProvinceForUpdate(provinceID, selectedDate));
        }
        public DataTable ReturnTaxListBasedOnDateAndProvinceForUpdate (int prov, DateTime currentDate)
        {
            string sqlCmd = "SELECT TR.taxID, TR.taxRate, TT.taxName FROM tbl_taxRate AS TR "
                + "INNER JOIN tbl_taxType TT ON TR.taxID = TT.taxID INNER JOIN(SELECT taxID, "
                + "MAX(taxDate) AS MTD FROM tbl_taxRate WHERE (taxDate <= @currentDate) AND "
                + "(provStateID = @prov) GROUP BY taxID) AS TD ON TR.taxID = TD.taxID AND "
                + "TR.taxDate = TD.MTD WHERE (TR.provStateID = @prov)";
            object[][] parms =
            {
                new object[] { "@currentDate", currentDate },
                new object[] { "@prov", prov }
            };
            return dbc.returnDataTableData(sqlCmd, parms);
        }

        private List<Tax> getTaxes(int provStateID, DateTime recDate)
        {
            //New command
            string sqlCmd = "SELECT TR.taxRate, TT.taxName from tbl_taxRate TR INNER JOIN tbl_taxType TT ON "
                + "TR.taxID = TT.taxID INNER JOIN (SELECT taxID, MAX(taxDate) AS MTD FROM tbl_taxRate WHERE "
                + "taxDate <= @recDate AND provStateID = @provStateID GROUP BY taxID) TD ON TR.taxID = TD.taxID "
                + "AND TR.taxDate = TD.MTD WHERE provStateID = @provStateID";

            object[][] parms =
            {
                new object[] { "@provStateID", provStateID },
                new object[] { "@recDate", recDate }
            };
            //Returns the list of taxes
            return ReturnListOfTaxes(dbc.returnDataTableData(sqlCmd, parms));
        }

        public void InsertNewTaxRate(int provinceID, int taxID, DateTime selectedDate, double taxRate)
        {
            string sqlCmd = "INSERT INTO tbl_taxRate VALUES(@provID, "
                + "@taxDate, @taxID, @taxRate)";
            Object[][] parms =
            {
                new object[] { "@provID", provinceID },
                new object[] { "@taxDate", selectedDate },
                new object[] { "@taxID", taxID },
                new object[] { "@taxRate", taxRate }
            };
            dbc.executeInsertQuery(sqlCmd,parms);
        }
        public object[] ReturnChargedTaxForSale(Invoice I, object[] btnRequirements)
        {
            int prov = I.location.provID;
            if (I.shippingAmount > 0)
            {
                prov = I.customer.province;
            }
            List<Tax> t = getTaxes(prov, I.invoiceDate);
            
            double totalTax = 0;
            bool bolGSTDisplay = false;
            bool bolPSTDisplay = false;
            string gstText = btnRequirements[0].ToString();
            string pstText = btnRequirements[1].ToString();
            foreach (var T in t)
            {
                double taxAmount = 0;
                switch (T.taxName)
                {
                    //If tax is GST calculate and make visible
                    case "GST":
                        if (gstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of GST
                            taxAmount = -(I.governmentTax);
                            I.governmentTax = 0;
                            //Changes button name
                            gstText = "Add GST";
                        }
                        else if (gstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of GST
                            I.governmentTax = CallReturnTaxAmount(T.taxRate, I.subTotal + I.shippingAmount);
                            taxAmount = I.governmentTax;
                            //Changes button name
                            gstText = "Remove GST";
                        }
                        bolGSTDisplay = true;
                        
                        //lblGovernmentAmount.Text = "$ " + I[0].governmentTax.ToString("#0.00");
                        //lblGovernmentAmount.Visible = true;
                        //btnRemoveGov.Visible = true;
                        break;
                    //If tax is PST calculate and make visible
                    case "PST":
                        if (pstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of PST
                            taxAmount = -(I.provincialTax);
                            I.provincialTax = 0;
                            //Changes button name
                            pstText = "Add PST"; //*** Need to figure out proper name of tax
                        }
                        else if (pstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of PST
                            I.provincialTax = CallReturnTaxAmount(T.taxRate, I.subTotal);
                            taxAmount = I.provincialTax;
                            //Changes button name
                            pstText = "Remove PST";
                        }
                        bolPSTDisplay = true;

                        //lblProvincial.Visible = true;
                        //I.provincialTax = CallReturnTaxAmount(T.taxRate, I.subTotal);
                        //lblProvincialAmount.Text = "$ " + I[0].provincialTax.ToString("#0.00");
                        //lblProvincialAmount.Visible = true;
                        //btnRemoveProv.Visible = true;
                        break;
                    //If tax is HST calculate and make visible
                    case "HST":
                        if (gstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of GST
                            taxAmount = -(I.governmentTax);
                            I.governmentTax = 0;
                            //Changes button name
                            gstText = "Add HST";
                        }
                        else if (gstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of GST
                            I.governmentTax = CallReturnTaxAmount(T.taxRate, I.subTotal + I.shippingAmount);
                            taxAmount = I.governmentTax;
                            //Changes button name
                            gstText = "Remove HST";
                        }
                        bolGSTDisplay = true;

                        //lblProvincial.Visible = false;
                        //lblGovernment.Text = "HST";
                        //I.governmentTax = CallReturnTaxAmount(T.taxRate, I.subTotal + I.shippingAmount);
                        //lblGovernmentAmount.Text = "$ " + I[0].governmentTax.ToString("#0.00");
                        //lblGovernmentAmount.Visible = true;
                        //btnRemoveProv.Visible = false;
                        //btnRemoveGov.Text = "HST";
                        break;
                    //If tax is RST calculate and make visible
                    case "RST":
                        if (pstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of PST
                            taxAmount = -(I.provincialTax);
                            I.provincialTax = 0;
                            //Changes button name
                            pstText = "Add RST"; //*** Need to figure out proper name of tax
                        }
                        else if (pstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of PST
                            I.provincialTax = CallReturnTaxAmount(T.taxRate, I.subTotal);
                            taxAmount = I.provincialTax;
                            //Changes button name
                            pstText = "Remove RST";
                        }
                        bolPSTDisplay = true;

                        //lblProvincial.Visible = true;
                        //lblProvincial.Text = "RST";
                        //I.provincialTax = CallReturnTaxAmount(T.taxRate, I.subTotal);
                        //lblProvincialAmount.Text = "$ " + I[0].provincialTax.ToString("#0.00");
                        //lblProvincialAmount.Visible = true;
                        //btnRemoveProv.Visible = true;
                        //btnRemoveProv.Text = "RST";
                        break;
                    //If tax is QST calculate and make visible
                    case "QST":
                        if (pstText.Split(' ')[0] == "Remove")
                        {
                            //Sets tax amount to a negative version of PST
                            taxAmount = -(I.provincialTax);
                            I.provincialTax = 0;
                            //Changes button name
                            pstText = "Add QST"; //*** Need to figure out proper name of tax
                        }
                        else if (pstText.Split(' ')[0] == "Add")
                        {
                            //Sets tax amount to a positive version of PST
                            I.provincialTax = CallReturnTaxAmount(T.taxRate, I.subTotal);
                            taxAmount = I.provincialTax;
                            //Changes button name
                            pstText = "Remove QST";
                        }
                        bolPSTDisplay = true;

                        //lblProvincial.Visible = true;
                        //lblProvincial.Text = "QST";
                        //I.provincialTax = CallReturnTaxAmount(T.taxRate, I.subTotal);
                        //lblProvincialAmount.Text = "$ " + I[0].provincialTax.ToString("#0.00");
                        //lblProvincialAmount.Visible = true;
                        //btnRemoveProv.Visible = true;
                        //btnRemoveProv.Text = "QST";
                        break;
                }
                totalTax += taxAmount;
            }
            //Adds the tax amount to the balance due
            I.balanceDue += totalTax;
            object[] btnParms = { bolGSTDisplay, gstText, bolPSTDisplay, pstText };
            InvoiceManager IM = new InvoiceManager();
            IM.UpdateCurrentInvoice(I);
            object[] results = { I, btnParms };
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
    }
}