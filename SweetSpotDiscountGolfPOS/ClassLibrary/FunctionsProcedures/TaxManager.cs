using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;

namespace SweetSpotDiscountGolfPOS.FP
{
    public class TaxManager
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();

        //Converters
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
        //private List<Tax> ReturnListOfTaxes(DataTable dt)
        //{
        //    List<Tax> tax = dt.AsEnumerable().Select(row =>
        //    new Tax
        //    {
        //        fltTaxRate = row.Field<double>("fltTaxRate"),
        //        varTaxName = row.Field<string>("varTaxName")
        //    }).ToList();
        //    return tax;
        //}
        public List<InvoiceItemTax> ConvertFromDataTableToInvoiceItemTax(DataTable dt)
        {
            List<InvoiceItemTax> invoiceItemTax = dt.AsEnumerable().Select(row =>
            new InvoiceItemTax
            {
                intInvoiceItemID = row.Field<int>("intInvoiceItemID"),
                intTaxTypeID = row.Field<int>("intTaxTypeID"),
                varTaxName = row.Field<string>("varTaxName"),
                fltTaxRate = row.Field<double>("fltTaxRate"),
                fltTaxAmount = row.Field<double>("fltTaxAmount"),
                bitIsTaxCharged = row.Field<bool>("bitIsTaxCharged")
            }).ToList();
            return invoiceItemTax;
        }
        //private List<InvoiceItemTax> ConvertFromDataTableToInvoiceItemTax2(DataTable dt)
        //{
        //    List<InvoiceItemTax> invoiceItemTax = dt.AsEnumerable().Select(row =>
        //    new InvoiceItemTax
        //    {
        //        intInvoiceItemID = row.Field<int>("intInvoiceItemID"),
        //        intTaxTypeID = row.Field<int>("intTaxTypeID"),
        //        varTaxName = row.Field<string>("varTaxName"),
        //        fltTaxAmount = row.Field<double>("fltTaxAmount"),
        //        bitIsTaxCharged = row.Field<bool>("bitIsTaxCharged")
        //    }).ToList();
        //    return invoiceItemTax;
        //}




        //DB calls
        private DataTable ReturnTaxListBasedOnDateAndProvinceForUpdate(int provinceID, DateTime selectedDate, object[] objPageDetails)
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
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //private List<Tax> GetTaxes(int invoiceID, object[] objPageDetails)
        //{
        //    string strQueryName = "getTaxes";
        //    //New command
        //    //string sqlCmd = "SELECT TR.fltTaxRate, TT.varTaxName FROM tbl_taxRate TR INNER JOIN tbl_taxType TT ON TR.intTaxID = TT.intTaxID "
        //    //    + "INNER JOIN (SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate WHERE dtmTaxEffectiveDate <= @selectedDate "
        //    //    + "AND intProvinceID = @intProvinceID GROUP BY intTaxID) TD ON TR.intTaxID = TD.intTaxID AND TR.dtmTaxEffectiveDate = "
        //    //    + "TD.MTD WHERE intProvinceID = @intProvinceID";

        //    string sqlCmd = "SELECT CSIT.intInvoiceItemID, intTaxTypeID, fltTaxAmount, bitIsTaxCharged FROM tbl_currentSalesItemsTaxes CSIT JOIN "
        //        + "tbl_currentSalesItems CSI ON CSI.intInvoiceItemID = CSIT.intInvoiceItemID WHERE CSI.intInvoiceID = @intInvoiceID";

        //    object[][] parms =
        //    {
        //        //new object[] { "@intProvinceID", provinceID },
        //        //new object[] { "@selectedDate", selectedDate }
        //        new object[] { "@intInvoiceID", invoiceID }
        //    };
        //    //Returns the list of taxes
        //    return ReturnListOfTaxes(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        //    //return ReturnListOfTaxes(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        //}
        private DataTable GatherFullTaxList(object[] objPageDetails)
        {
            string strQueryName = "GatherFullTaxList";
            string sqlCmd = "SELECT intTaxID FROM tbl_taxType";
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private bool CheckForLiquorTax(int taxID, object[] objPageDetails)
        {
            string strQueryName = "CheckForLiquorTax";
            bool isLiquor = false;
            string sqlCmd = "SELECT varTaxName FROM tbl_taxType WHERE intTaxID = @intTaxID";
            object[][] parms =
            {
                new object[] { "@intTaxID", taxID }
            };

            if (DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName) == "LCT")
            {
                isLiquor = true;
            }
            return isLiquor;
        }
        private bool CheckForQuebecSalesTax(int taxID, object[] objPageDetails)
        {
            string strQueryName = "CheckForQuebecSalesTax";
            bool isQuebecSales = false;
            string sqlCmd = "SELECT varTaxName FROM tbl_taxType WHERE intTaxID = @intTaxID";
            object[][] parms =
            {
                new object[] { "@intTaxID", taxID }
            };

            if (DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName) == "QST")
            {
                isQuebecSales = true;
            }
            return isQuebecSales;
        }
        private bool CheckForRetailSalesTax(int taxID, object[] objPageDetails)
        {
            string strQueryName = "CheckForRetailSalesTax";
            bool isRetailSales = false;
            string sqlCmd = "SELECT varTaxName FROM tbl_taxType WHERE intTaxID = @intTaxID";
            object[][] parms =
            {
                new object[] { "@intTaxID", taxID }
            };

            if (DBC.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName) == "RST")
            {
                isRetailSales = true;
            }
            return isRetailSales;
        }
        private void InsertNewTaxRate(int provinceID, int taxID, DateTime selectedDate, double taxRate, object[] objPageDetails)
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
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void SetTaxChargedForInventory(int inventoryID, int taxID, bool chargeTax, object[] objPageDetails)
        {
            string strQueryName = "SetTaxChargedForInventory";
            string sqlCmd = "UPDATE tbl_taxTypePerInventoryItem SET bitChargeTax = @bitChargeTax WHERE "
                + "intInventoryID = @intInventoryID AND intTaxID = @intTaxID";
            object[][] parms =
            {
                new object[] { "@bitChargeTax", chargeTax },
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intTaxID", taxID }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnTaxIDFromString(string taxName, object[] objPageDetails)
        {
            string strQueryName = "ReturnTaxIDFromString";
            string sqlCmd = "SELECT intTaxID FROM tbl_taxType WHERE varTaxName = @varTaxName";
            object[][] parms =
            {
                new object[] { "@varTaxName", taxName }
            };
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private List<InvoiceItemTax> ReturnTaxesAvailableForItem(InvoiceItems invoiceItem, int transactionTypeID, DateTime currentDateTime, int provinceID, object[] objPageDetails)
        {
            string strQueryName = "ReturnTaxesAvailableForItem";
            InvoiceManager IM = new InvoiceManager();
            string sqlCmd = "SELECT CSI.intInvoiceItemID, TTPII.intTaxID AS intTaxTypeID, T.varTaxName, ITR.fltTaxRate, ";

            if (transactionTypeID == IM.CallReturnTransactionID("Sale", objPageDetails) || transactionTypeID == IM.CallReturnTransactionID("On Hold", objPageDetails))
            {
                sqlCmd += "CASE WHEN CSI.bitIsDiscountPercent = 1 THEN " 
                    //+ "ROUND(" +
                    + "(ROUND((CSI.fltItemPrice - (CSI.fltItemPrice * (CSI.fltItemDiscount / 100))) * ITR.fltTaxRate, 2)) * CSI.intItemQuantity "
                    //", 2) "
                    + "ELSE "
                    //"ROUND(" +
                    + "(ROUND((CSI.fltItemPrice - CSI.fltItemDiscount) * ITR.fltTaxRate, 2)) * CSI.intItemQuantity " 
                    //", 2) "
                    + "END AS fltTaxAmount, ";
            }
            else if (transactionTypeID == IM.CallReturnTransactionID("Return", objPageDetails))
            {
                sqlCmd += //"ROUND(" +
                    "(ROUND(CSI.fltItemRefund * ITR.fltTaxRate, 2)) * CSI.intItemQuantity " 
                    //", 2) "
                    + "AS fltTaxAmount, ";
            }

            sqlCmd += "TTPII.bitChargeTax AS bitIsTaxCharged FROM tbl_currentSalesItems CSI JOIN tbl_taxTypePerInventoryItem TTPII ON "
                + "TTPII.intInventoryID = CSI.intInventoryID JOIN tbl_taxType T ON T.intTaxID = TTPII.intTaxID JOIN(SELECT TTPI.intInventoryID, "
                + "TTPI.intTaxID, fltTaxRate FROM tbl_taxRate TR INNER JOIN(SELECT intTaxID, MAX(dtmTaxEffectiveDate) AS MTD FROM tbl_taxRate "
                + "WHERE dtmTaxEffectiveDate <= @dtmCurrentDate AND intProvinceID = @intProvinceID GROUP BY intTaxID) TD ON TR.intTaxID = "
                + "TD.intTaxID AND TR.dtmTaxEffectiveDate = TD.MTD INNER JOIN(SELECT intInventoryID, intTaxID FROM tbl_taxTypePerInventoryItem "
                + "WHERE bitChargeTax = 1 AND intInventoryID = @intInventoryID) TTPI ON TTPI.intTaxID = TR.intTaxID WHERE intProvinceID = "
                + "@intProvinceID) ITR ON ITR.intInventoryID = CSI.intInventoryID AND ITR.intTaxID = TTPII.intTaxID WHERE CSI.intInvoiceID = "
                + "@intInvoiceID AND CSI.intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceItem.intInvoiceID },
                new object[] { "@dtmCurrentDate",  currentDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@intProvinceID", provinceID },
                new object[] { "@intInventoryID", invoiceItem.intInventoryID }
            };
            return ConvertFromDataTableToInvoiceItemTax(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
        }
        private void InsertItemTaxIntoSalesCart(InvoiceItemTax invoiceItemTaxes, object[] objPageDetails)
        {
            string strQueryName = "InsertItemTaxIntoSalesCart";
            string sqlCmd = "INSERT INTO tbl_currentSalesItemsTaxes VALUES(@intInvoiceItemID, @intTaxID, @fltTaxAmount, @bitIsTaxCharged)";
            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItemTaxes.intInvoiceItemID },
                new object[] { "@intTaxID", invoiceItemTaxes.intTaxTypeID },
                new object[] { "@fltTaxAmount", invoiceItemTaxes.fltTaxAmount },
                new object[] { "@bitIsTaxCharged", invoiceItemTaxes.bitIsTaxCharged }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }



        //Public calls
        public List<Tax> ReturnTaxListBasedOnDate(DateTime selectedDate, int provinceID, object[] objPageDetails)
        {
            return ConvertFromDataTableToTax(ReturnTaxListBasedOnDateAndProvinceForUpdate(provinceID, selectedDate, objPageDetails));
        }
        public DataTable GatherTaxListFromDateAndProvince(int provinceID, DateTime selectedDate, object[] objPageDetails)
        {
            return ReturnTaxListBasedOnDateAndProvinceForUpdate(provinceID, selectedDate, objPageDetails);
        }
        public DataTable ReturnTaxList(object[] objPageDetails)
        {
            return GatherFullTaxList(objPageDetails);
        }
        //public object[] ReturnChargedTaxForSale(Invoice invoice, object[] btnRequirements, object[] objPageDetails)
        //{
        //    //int prov = invoice.location.intProvinceID;
        //    //if (invoice.fltShippingCharges > 0)
        //    //{
        //    //    prov = invoice.customer.intProvinceID;
        //    //}
        //    List<Tax> t = GetTaxes(invoice.intInvoiceID, objPageDetails);
            
            
        //    bool bolGSTDisplay = false;
        //    bool bolPSTDisplay = false;
        //    string gstText = btnRequirements[0].ToString();
        //    string pstText = btnRequirements[1].ToString();
        //    foreach (var T in t)
        //    {
        //        double taxAmount = 0;
        //        switch (T.varTaxName)
        //        {
        //            //If tax is GST calculate and make visible
        //            case "GST":
        //                if (gstText.Split(' ')[0] == "Remove")
        //                {
        //                    //Sets tax amount to a negative version of GST
        //                    taxAmount = -(invoice.fltGovernmentTaxAmount);
        //                    //invoice.bitChargeGST = false;
        //                    //Changes button name
        //                    gstText = "Add GST";
        //                }
        //                else if (gstText.Split(' ')[0] == "Add")
        //                {
        //                    //Sets tax amount to a positive version of GST
        //                    invoice.fltGovernmentTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal + invoice.fltShippingCharges);
        //                    taxAmount = invoice.fltGovernmentTaxAmount;
        //                    //invoice.bitChargeGST = true;
        //                    //Changes button name
        //                    gstText = "Remove GST";
        //                }
        //                bolGSTDisplay = true;
        //                break;
        //            //If tax is PST calculate and make visible
        //            case "PST":
        //                if (pstText.Split(' ')[0] == "Remove")
        //                {
        //                    //Sets tax amount to a negative version of PST
        //                    taxAmount = -(invoice.fltProvincialTaxAmount);
        //                    //invoice.bitChargePST = false;
        //                    //Changes button name
        //                    pstText = "Add PST"; //*** Need to figure out proper name of tax
        //                }
        //                else if (pstText.Split(' ')[0] == "Add")
        //                {
        //                    //Sets tax amount to a positive version of PST
        //                    invoice.fltProvincialTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal);
        //                    taxAmount = invoice.fltProvincialTaxAmount;
        //                    //invoice.bitChargePST = true;
        //                    //Changes button name
        //                    pstText = "Remove PST";
        //                }
        //                bolPSTDisplay = true;
        //                break;
        //            //If tax is HST calculate and make visible
        //            case "HST":
        //                if (gstText.Split(' ')[0] == "Remove")
        //                {
        //                    //Sets tax amount to a negative version of GST
        //                    taxAmount = -(invoice.fltGovernmentTaxAmount);
        //                    //invoice.bitChargeGST = false;
        //                    //Changes button name
        //                    gstText = "Add HST";
        //                }
        //                else if (gstText.Split(' ')[0] == "Add")
        //                {
        //                    //Sets tax amount to a positive version of GST
        //                    invoice.fltGovernmentTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal + invoice.fltShippingCharges);
        //                    taxAmount = invoice.fltGovernmentTaxAmount;
        //                    //invoice.bitChargeGST = true;
        //                    //Changes button name
        //                    gstText = "Remove HST";
        //                }
        //                bolGSTDisplay = true;
        //                break;
        //            //If tax is RST calculate and make visible
        //            case "RST":
        //                if (pstText.Split(' ')[0] == "Remove")
        //                {
        //                    //Sets tax amount to a negative version of PST
        //                    taxAmount = -(invoice.fltProvincialTaxAmount);
        //                    //invoice.bitChargePST = false;
        //                    //Changes button name
        //                    pstText = "Add RST"; //*** Need to figure out proper name of tax
        //                }
        //                else if (pstText.Split(' ')[0] == "Add")
        //                {
        //                    //Sets tax amount to a positive version of PST
        //                    invoice.fltProvincialTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal);
        //                    taxAmount = invoice.fltProvincialTaxAmount;
        //                    //invoice.bitChargePST = true;
        //                    //Changes button name
        //                    pstText = "Remove RST";
        //                }
        //                bolPSTDisplay = true;
        //                break;
        //            //If tax is QST calculate and make visible
        //            case "QST":
        //                if (pstText.Split(' ')[0] == "Remove")
        //                {
        //                    //Sets tax amount to a negative version of PST
        //                    taxAmount = -(invoice.fltProvincialTaxAmount);
        //                    //invoice.bitChargePST = false;
        //                    //Changes button name
        //                    pstText = "Add QST"; //*** Need to figure out proper name of tax
        //                }
        //                else if (pstText.Split(' ')[0] == "Add")
        //                {
        //                    //Sets tax amount to a positive version of PST
        //                    invoice.fltProvincialTaxAmount = CallReturnTaxAmount(T.fltTaxRate, invoice.fltSubTotal);
        //                    taxAmount = invoice.fltProvincialTaxAmount;
        //                    //invoice.bitChargePST = true;
        //                    //Changes button name
        //                    pstText = "Remove QST";
        //                }
        //                bolPSTDisplay = true;
        //                break;
        //        }
        //        //totalTax += taxAmount;
        //    }

        //    object[] btnParms = { bolGSTDisplay, gstText, bolPSTDisplay, pstText };
        //    InvoiceManager IM = new InvoiceManager();
        //    IM.CallUpdateCurrentInvoice(invoice, objPageDetails);
        //    object[] results = { invoice, btnParms };
        //    return results;
        //}
        //private double CallReturnTaxAmount(double rate, double subTotal)
        //{
        //    return ReturnTaxAmount(rate, subTotal);
        //}
        //This method returns the tax amount of the cart based on subtotal **Checked and Verified
        //private double ReturnTaxAmount(double rate, double subtotal)
        //{
        //    //Returns the gst amount 
        //    return Math.Round((rate * subtotal), 2);
        //}
        //public void TaxCorrectionOnReturnInvoice(Invoice invoice, object[] objPageDetails)
        //{
        //    invoice.fltBalanceDue -= (invoice.fltGovernmentTaxAmount + invoice.fltProvincialTaxAmount);
        //    InvoiceManager IM = new InvoiceManager();
        //    IM.CalculateNewInvoiceReturnTotalsToUpdate(invoice, objPageDetails);
        //}
        public void UpdateTaxChargedForInventory(int inventoryID, int taxID, bool chargeTax, object[] objPageDetails)
        {
            SetTaxChargedForInventory(inventoryID, taxID, chargeTax, objPageDetails);
        }
        public double ReturnGovernmentTaxTotal(List<InvoiceItemTax> invoiceItemTaxes, object[] objPageDetails)
        {
            double taxAmount = 0;
            foreach(InvoiceItemTax iit in invoiceItemTaxes)
            {
                if(iit.intTaxTypeID == ReturnTaxIDFromString("GST", objPageDetails))
                {
                    if (iit.bitIsTaxCharged)
                    {
                        taxAmount += iit.fltTaxAmount;
                    }
                }
            }
            return taxAmount;
        }
        public double ReturnHarmonizedTaxTotal(List<InvoiceItemTax> invoiceItemTaxes, object[] objPageDetails)
        {
            double taxAmount = 0;
            foreach (InvoiceItemTax iit in invoiceItemTaxes)
            {
                if (iit.intTaxTypeID == ReturnTaxIDFromString("HST", objPageDetails))
                {
                    if (iit.bitIsTaxCharged)
                    {
                        taxAmount += iit.fltTaxAmount;
                    }
                }
            }
            return taxAmount;
        }
        public double ReturnLiquorTaxTotal(List<InvoiceItemTax> invoiceItemTaxes, object[] objPageDetails)
        {
            double taxAmount = 0;
            foreach (InvoiceItemTax iit in invoiceItemTaxes)
            {
                if (iit.intTaxTypeID == ReturnTaxIDFromString("LCT", objPageDetails))
                {
                    if (iit.bitIsTaxCharged)
                    {
                        taxAmount += iit.fltTaxAmount;
                    }
                }
            }
            return taxAmount;
        }
        public double ReturnProvincialTaxTotal(List<InvoiceItemTax> invoiceItemTaxes, object[] objPageDetails)
        {
            double taxAmount = 0;
            foreach (InvoiceItemTax iit in invoiceItemTaxes)
            {
                if (iit.intTaxTypeID == ReturnTaxIDFromString("PST", objPageDetails))
                {
                    if (iit.bitIsTaxCharged)
                    {
                        taxAmount += iit.fltTaxAmount;
                    }
                }
            }
            return taxAmount;
        }
        public double ReturnQuebecTaxTotal(List<InvoiceItemTax> invoiceItemTaxes, object[] objPageDetails)
        {
            double taxAmount = 0;
            foreach (InvoiceItemTax iit in invoiceItemTaxes)
            {
                if (iit.intTaxTypeID == ReturnTaxIDFromString("QST", objPageDetails))
                {
                    if (iit.bitIsTaxCharged)
                    {
                        taxAmount += iit.fltTaxAmount;
                    }
                }
            }
            return taxAmount;
        }
        public double ReturnRetailTaxTotal(List<InvoiceItemTax> invoiceItemTaxes, object[] objPageDetails)
        {
            double taxAmount = 0;
            foreach (InvoiceItemTax iit in invoiceItemTaxes)
            {
                if (iit.intTaxTypeID == ReturnTaxIDFromString("RST", objPageDetails))
                {
                    if (iit.bitIsTaxCharged)
                    {
                        taxAmount += iit.fltTaxAmount;
                    }
                }
            }
            return taxAmount;
        }
        public double ReturnTaxPercentageForShipping(int intShippingProvinceID, DateTime selectedDate, object[] objPageDetails)
        {
            double fltTaxPercentage = 0;
            DataTable dt = ReturnTaxListBasedOnDateAndProvinceForUpdate(intShippingProvinceID, selectedDate, objPageDetails);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr[0]) == 1)
                {
                    fltTaxPercentage = Convert.ToDouble(dr[1]);
                }
                else if(Convert.ToInt32(dr[0]) == 3)
                {
                    fltTaxPercentage = Convert.ToDouble(dr[1]);
                }
            }
            return fltTaxPercentage;
        }


        public int GatherTaxIDFromString(string taxName, object[] objPageDetails)
        {
            return ReturnTaxIDFromString(taxName, objPageDetails);
        }
        public void LoopThroughTaxesForEachItemAddingToCurrentInvoiceItemTaxes(InvoiceItems invoiceItem, int transactionTypeID, DateTime currentDateTime, int provinceID, object[] objPageDetails)
        {
            List<InvoiceItemTax> invoiceItemTaxes = ReturnTaxesAvailableForItem(invoiceItem, transactionTypeID, currentDateTime, provinceID, objPageDetails);
            foreach (var tax in invoiceItemTaxes)
            {
                InsertItemTaxIntoSalesCart(tax, objPageDetails);
            }
        }
        public void ChangeProvinceTaxesBasedOnShipping(int invoiceID, int shippingProvinceID, object[] objPageDetails)
        {
            //string strQueryName = "ChangeProvinceTaxesBasedOnShipping";
            InvoiceManager IM = new InvoiceManager();
            Invoice invoice = IM.CallReturnCurrentInvoice(invoiceID, shippingProvinceID, objPageDetails)[0];
            IM.CallRemoveInvoiceItemTaxesFromCurrentItemsTaxesTable(invoice.invoiceItems, objPageDetails);
            foreach(InvoiceItems invoiceItem in invoice.invoiceItems)
            {
                LoopThroughTaxesForEachItemAddingToCurrentInvoiceItemTaxes(invoiceItem, invoice.intTransactionTypeID, invoice.dtmInvoiceDate, shippingProvinceID, objPageDetails);
            }
        }
        public bool CallCheckForLiquorTax(int taxID, object[] objPageDetails)
        {
            return CheckForLiquorTax(taxID, objPageDetails);
        }
        public bool CallCheckForQuebecSalesTax(int taxID, object[] objPageDetails)
        {
            return CheckForQuebecSalesTax(taxID, objPageDetails);
        }
        public bool CallCheckForRetailSalesTax(int taxID, object[] objPageDetails)
        {
            return CheckForRetailSalesTax(taxID, objPageDetails);
        }
        public void CallInsertNewTaxRate(int provinceID, int taxID, DateTime selectedDate, double taxRate, object[] objPageDetails)
        {
            InsertNewTaxRate(provinceID, taxID, selectedDate, taxRate, objPageDetails);
        }
    }
}