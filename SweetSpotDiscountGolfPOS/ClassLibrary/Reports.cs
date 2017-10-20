using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using SweetShop;
using SweetSpotProShop;
using System.Threading;
using System.Diagnostics;
using System.Text;
using OfficeOpenXml;
using System.Windows.Forms;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //This is a mess...
    public class Reports
    {
        string connectionString;
        List<Cashout> cashout = new List<Cashout>();
        List<Cashout> remainingCashout = new List<Cashout>();
        Clubs c = new Clubs();
        Accessories a = new Accessories();
        Clothing cl = new Clothing();
        Customer cu = new Customer();
        SweetShopManager ssm = new SweetShopManager();
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();
        Object o = new Object();
        private System.Data.DataTable exportTable;
        private System.Data.DataTable exportInvoiceTable;
        private System.Data.DataTable exportInvoiceItemTable;
        private System.Data.DataTable exportInvoiceMOPTable;

        //Connection String
        public Reports()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //*******************CASHOUT UTILITIES*******************************************************
        //This method connects to the database and gets the totals for the MOPs based on location and dates
        public List<Cashout> cashoutAmounts(DateTime startDate, DateTime endDate, int locationID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select tbl_invoiceMOP.mopType, tbl_invoiceMOP.amountPaid " +
                "from tbl_invoiceMOP " +
                "INNER JOIN tbl_invoice ON tbl_invoiceMOP.invoiceNum = tbl_invoice.invoiceNum AND tbl_invoiceMOP.invoiceSubNum = tbl_invoice.invoiceSubNum " +
                "where tbl_invoice.invoiceDate between @startDate and @endDate and tbl_invoice.locationID = @locationID;";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Cashout cs = new Cashout(
                    Convert.ToString(reader["mopType"]),
                    Convert.ToDouble(reader["amountPaid"]));
                //Adding the mops to the list of type cashout
                cashout.Add(cs);
            }
            con.Close();
            //Returns the list of type cashout
            return cashout;
        }
        //Used to get the subTotal, government tax, and provincial tax from the invoices based on a location ID and dates
        public Cashout getRemainingCashout(DateTime startDate, DateTime endDate, int locationID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select " +
                "sum(tbl_invoice.tradeinAmount) as tradeinTotal, " +
                "sum(tbl_invoice.subTotal) as subTotal, " +
                "sum(tbl_invoice.governmentTax) as gTax, " +
                "sum(tbl_invoice.provincialtax) as pTax, " +
                "sum(tbl_invoice.shippingAmount) as shippingTotal from tbl_invoice " +
                "where tbl_invoice.invoiceDate " +
                "between @startDate and @endDate " +
                "and tbl_invoice.locationID = @locationID";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            cmd.Connection = con;
            con.Open();
            Cashout cs = new Cashout();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cs = new Cashout(
                    Convert.ToDouble(reader["tradeinTotal"]),
                    Convert.ToDouble(reader["subTotal"]),
                    Convert.ToDouble(reader["gTax"]),
                    Convert.ToDouble(reader["pTax"]),
                    Convert.ToDouble(reader["shippingTotal"]));
                //Adding the totals to a list of type cashout

            }
            con.Close();
            //Return the list of type cashout
            return cs;
        }
        //This method gets the trade in amounts from the invoices based on a location ID and dates
        public double getTradeInsCashout(DateTime startDate, DateTime endDate, int locationID)
        {
            double tradeintotal = 0;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select tradeinAmount from  tbl_invoice" +
                " where invoiceDate between @startDate and @endDate and locationID = @locationID;";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tradeintotal += Convert.ToDouble(reader["tradeinAmount"]);                
            }
            con.Close();
            //Returns the total value of the trade ins
            return tradeintotal;
        }
        //Insert the cashout into the database
        public void insertCashout(Cashout cas)
        {
            int processed = 0;
            int finalized = 0;
            if (cas.processed)
            {
                processed = 1;
            }
            if (cas.finalized)
            {
                finalized = 1;
            }

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //This is a mess...
            cmd.CommandText = "Insert into tbl_cashout values('" +
               cas.date + "', '" + cas.time + "', " + cas.saleTradeIn + ", " + cas.saleGiftCard + ", " +
               cas.saleCash + ", "  + cas.saleDebit + ", " + cas.saleMasterCard + ", " +
               cas.saleVisa + ", " +  cas.receiptTradeIn + ", " + cas.receiptGiftCard + ", " +
               cas.receiptCash + ", "  + cas.receiptDebit + ", " + cas.receiptMasterCard + ", " + cas.receiptVisa + ", " +
               cas.preTax + ", " + cas.saleGST + ", " +  cas.salePST + ", " + 
               cas.overShort + ", " + finalized + ", " + processed + ");";

            //" saleTradeIn = @saleTradeIn, " +
            //" saleGiftCard = @saleGiftCard, saleCash = @saleCash, saleCheque = @saleCheque, " +
            //" saleDebit = @saleDebit, saleMasterCard = @saleMasterCard, saleVisa = @saleVisa, " +
            //" saleAmex = @saleAmex, receiptTradeIn = @receiptTradeIn, receiptGiftCard = @receiptGiftCard, " +
            //" receiptCash = @receiptCash, receiptCheque = @receiptCheque, receiptDebit = @receiptDebit, " +
            //" receiptMasterCard = @receiptMasterCard, receiptVisa = @receiptVisa, receiptAmex = @receiptAmex, " +
            //" overShort = @overShort, finalized = @finalized, processed = @processed);";

            //cmd.Parameters.AddWithValue("@cashoutDate", cas.date);
            //cmd.Parameters.AddWithValue("@cashoutTime", cas.time);
            //cmd.Parameters.AddWithValue("@saleTradeIn", cas.saleTradeIn);

            //cmd.Parameters.AddWithValue("@saleGiftCard", cas.saleGiftCard);
            //cmd.Parameters.AddWithValue("@saleCash", cas.saleCash);
            //cmd.Parameters.AddWithValue("@saleCheque", cas.saleCheque);

            //cmd.Parameters.AddWithValue("@saleDebit", cas.saleDebit);
            //cmd.Parameters.AddWithValue("@saleMasterCard", cas.saleMasterCard);
            //cmd.Parameters.AddWithValue("@saleVisa", cas.saleVisa);

            //cmd.Parameters.AddWithValue("@saleAmex", cas.saleAmex);
            //cmd.Parameters.AddWithValue("@receiptTradeIn", cas.receiptTradeIn);
            //cmd.Parameters.AddWithValue("@receiptGiftCard", cas.receiptGiftCard);

            //cmd.Parameters.AddWithValue("@receiptCash", cas.receiptCash);
            //cmd.Parameters.AddWithValue("@receiptCheque", cas.receiptCheque);
            //cmd.Parameters.AddWithValue("@receiptDebit", cas.receiptDebit);

            //cmd.Parameters.AddWithValue("@receiptMasterCard", cas.receiptMasterCard);
            //cmd.Parameters.AddWithValue("@receiptVisa", cas.receiptVisa);
            //cmd.Parameters.AddWithValue("@receiptAmex", cas.receiptAmex);

            //cmd.Parameters.AddWithValue("@overShort", cas.overShort);
            //cmd.Parameters.AddWithValue("@finalized", finalized);
            //cmd.Parameters.AddWithValue("@processed", processed);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        }

        //******************PURCHASES REPORTING*******************************************************
        public List<Purchases> returnPurchasesDuringDates(DateTime startDate, DateTime endDate, int locationID)
        {
            List<Purchases> purch = new List<Purchases>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT R.receiptNumber, R.receiptDate, D.methodDesc, M.chequeNum, " +
                            "M.amountPaid FROM tbl_receipt R INNER JOIN tbl_receiptMOP M ON " +
                            "R.receiptNumber = M.receiptNum INNER JOIN tbl_methodOfPayment D " +
                            "on M.mopType = D.methodID where R.receiptDate BETWEEN @startDate and " +
                            "@endDate and R.locationID = @locationID";
            cmd.Parameters.AddWithValue("@startDate", startDate);
            cmd.Parameters.AddWithValue("@endDate", endDate);
            cmd.Parameters.AddWithValue("@locationID", locationID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                purch.Add(new Purchases(Convert.ToInt32(reader["receiptNumber"]),
                    Convert.ToDateTime(reader["receiptDate"]), Convert.ToString(reader["methodDesc"]),
                    Convert.ToInt32(reader["chequeNum"]), Convert.ToDouble(reader["amountPaid"])));
            }
            return purch;
        }

        //********************IMPORTING***************************************************************

        //This method is the giant import method
        public void importItems(FileUpload fup)
        {
            //List of clubs
            List<Clubs> listClub = new List<Clubs>();
            //List of clothing
            List<Clothing> listClothing = new List<Clothing>();
            //List of accessories
            List<Accessories> listAccessories = new List<Accessories>();
            //^ I don't think these are actually used anymore
            Form fc = (Form)System.Windows.Forms.Application.OpenForms["SettingsHomePage"];

            //check if there is actually a file being uploaded
            if (fup.HasFile)
            {
                //load the uploaded file into the memorystream
                using (MemoryStream stream = new MemoryStream(fup.FileBytes))
                //Lets the server know to use the excel package
                using (ExcelPackage xlPackage = new ExcelPackage(stream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                    //Gets the row count
                    var rowCnt = worksheet.Dimension.End.Row;
                    //Gets the column count
                    var colCnt = worksheet.Dimension.End.Column;
                    //Beginning the loop for data gathering
                    for (int i = 2; i < rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                    {

 

                        SettingsHomePage page = new SettingsHomePage();
                        //page.counter = i;
                        //page.total = rowCnt;
                        page.progress = i.ToString() + "/" + rowCnt.ToString();
                        page.callJS();
                        //fc..Label1.Text = 
                  //      page.lblP.Text = i.ToString() + "/" + rowCnt.ToString();
                        System.Windows.Forms.Application.DoEvents();




                        string itemType;
                        //Attempts to get the item type
                        try
                        {
                            itemType = (worksheet.Cells[i, 5].Value).ToString(); //Column 5 = itemType
                        }
                        catch (Exception ex)
                        {
                            itemType = "";
                        }

                        //If the row is not null, and there is a value in column 5, proceed
                        if (worksheet.Row(i) != null && worksheet.Cells[i, 5].Value != null)
                        {
                            //Does nothing if itemType is null
                            if (itemType == null) { }
                            //Does nothing if itemType is blank
                            else if (itemType.Equals("")) { }
                            //***************ACCESSORIES*********
                            //If the itemType is accessories or Accessories, the item is an accessory
                            else if (itemType.Equals("Accessories") || itemType.Equals("accessories"))
                            {
                                //***************SKU***************
                                //If there is a sku in column 3, proceed
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null)) //Column 3 = Sku
                                { 
                                    //Sets the accessory sku to the value in column 3
                                    a.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    //Should NEVER happen, but is used as a safety/catch 
                                    a.sku = 0;
                                }
                                //***************BRAND ID***************
                                //Gets the brand name from the itemType. 
                                //If it an accessory, its brand will always be accessory
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    //Will equal the brandID of accessory
                                    a.brandID = bName;
                                }
                                else
                                {
                                    //Should NEVER happen, but is included as a safety/catch
                                    a.brandID = 1;
                                }
                                //***************MODEL ID***************                                
                                try
                                {
                                    string mName;
                                    mName = (worksheet.Cells[i, 6].Value).ToString(); //Column 6 = modelName
                                    //If the model name is null, set the ID to 1
                                    if (mName == null)
                                    {
                                        //Shouldn't happen. Should come out as ""
                                        a.modelID = 1;
                                    }
                                    else
                                    {
                                        //Gets the modelID from the value in column 6
                                        int mID = idu.modelName(mName);
                                        //Check if it is null
                                        if (!mID.Equals(null))
                                            //Hardcoded because the DB refused to allow 360 as a model name
                                            if (mName == "360") { a.modelID = 17; }
                                            //Setting the model ID to what is returned from idu.modelName
                                            else { a.modelID = mID; }
                                        else
                                            //Should NEVER happen, but is included as a safety/catch
                                            a.modelID = 1;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //1427: N/A
                                    a.modelID = 1427;
                                }
                                //***************ACCESSORY TYPE***************
                                try
                                {
                                    //Checks to see if column 7 has a value
                                    if ((string)(worksheet.Cells[i, 7].Value) != null) //Column 7 = accessoryType
                                    {
                                        //Setting the accessory's accessory type to the value in column 7
                                        a.accessoryType = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        //Won't happen very often but can be triggered by a blank cell. In that case, this sets it to be blank
                                        a.accessoryType = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Sets the accessory type to be blank if an error occurs.
                                    a.accessoryType = "";
                                }
                                //***************COST***************
                                try
                                {
                                    //Checks if column 12 has a value
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null)) //Column 12 = cost
                                    {
                                        //Sets the accessory's cost to the value in column 12
                                        a.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        //Sometimes no cost is given so I set it to 0 in the DB
                                        a.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Sets the cost to 0 if an error is thrown
                                    a.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    //Checks if column 15 has a value
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null)) //Column 15 = price
                                    {
                                        //Sets the accessory's price to the value in column 15
                                        a.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        //Sometimes a price is not given so it gets set to 0 in the DB
                                        a.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Sometimes a price is not given and caught so it gets set to 0 in the DB
                                    a.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    //Checks to see if there is a value in column 13
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null)) //Column 13 = quantity
                                    {
                                        //Sets the accessory's quantity to the value in column 13
                                        a.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        //If the quantity is not given or is blank, setting it to 0
                                        a.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, setting the quantity to 0
                                    a.quantity = 0;
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    //Checking if there is a value in column 16
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null)) //Column 16 = comments
                                    {
                                        //Setting the accessory's comments to the value in column 16
                                        a.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        //When comments are not present, they are set to "" in the DB
                                        a.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Whenan error occurs, sets the comments to ""
                                    a.comments = "";
                                }
                                //***************LOCATIONID***************
                                try
                                {
                                    //NEEDS TO BE REWORKED
                                    if (!(worksheet.Cells[i, 22].Value).Equals(null)) //22
                                    {
                                        string destination = (worksheet.Cells[i, 22].Value).ToString();
                                        a.locID = lm.getLocationIDFromDestination(destination);                                       
                                    }
                                    else
                                    {
                                        a.locID = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    a.locID = 1;
                                }


                                a.typeID = 2;  //Accessory type ID = 2
                                a.size = "";   //NOT BEING USED
                                a.colour = ""; //NOT BEING USED

                                //Adds the accessory to the list of type accessory
                                listAccessories.Add(a);
                                o = a as Object;
                            }
                            //***************APPAREL*************
                            //Apparel is used instead of clothing due to the clients previous DB
                            else if (itemType.Equals("Apparel") || itemType.Equals("apparel"))
                            {
                                //***************SKU***************
                                //Checks to see if column 3 has a value
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null)) //Column 3 = sku
                                {
                                    //Sets the clothing sku to the value in column 3
                                    cl.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    //Should NEVER happen, but is a good catch
                                    cl.sku = 0;
                                }
                                //***************BRAND ID***************   
                                //Gets the brand name from the itemType. 
                                //If it clothing, its brand will always be clothing
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    //Will equal the brandID of accessory
                                    cl.brandID = bName;
                                }
                                else
                                {
                                    //Something really broke if this happens
                                    cl.brandID = 1;
                                }
                                //***************COST***************
                                try
                                {
                                    //Checks to see if there is a value in column 12
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null)) //Column 12 = cost
                                    {
                                        //Sets the clothing cost to the value in column 12
                                        cl.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        //Sometime cost is not give so I set it to 0
                                        cl.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, setting the value to 0
                                    cl.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    //Checks to see if there is a value in column 15
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null)) //Column 15 = price
                                    {
                                        //Sets the clothing price to the value in column 15
                                        cl.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        //Sometimes a price is not given so I set it to 0
                                        cl.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, the price is set to 0
                                    cl.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    //Checks to see if there is a value in column 13
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null)) //Column 13 = quantity
                                    {
                                        //Sets the clothing quantity to the value in column 13
                                        cl.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        //Sometimes a quantity is not given so I set it to 0
                                        cl.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, the quantity is set to 0
                                    cl.quantity = 0;
                                }
                                //***************GENDER***************
                                try
                                {
                                    //Checks to see if there isa value in column 6
                                    if (!(worksheet.Cells[i, 6].Value).Equals(null)) //Column 6 = gender
                                    {
                                        //Sets the clothing gender to the value in column 6
                                        cl.gender = (string)(worksheet.Cells[i, 6].Value);
                                    }
                                    else
                                    {
                                        //Sometimes the gender is not given so I set it to ""
                                        cl.gender = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, the gender is set to ""
                                    cl.gender = "";
                                }
                                //***************STYLE***************
                                try
                                {
                                    //Checks to see if there is a value in column 7
                                    if (!(worksheet.Cells[i, 7].Value).Equals(null)) //Column 7 = style
                                    {
                                        //Set the clothing style to the value in column 7
                                        cl.style = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        //Sometimes the style is not given so I set it to ""
                                        cl.style = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, set the style to ""
                                    cl.style = "";
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    //Checks to see if there is a value in column 16
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null)) //Column 16 = comments
                                    {
                                        //Sets the clothing comments to the value in column 16
                                        cl.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        //Sometimes the comments are not given so I set it to ""
                                        cl.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //If an error occurs, set the comments to ""
                                    cl.comments = "";
                                }
                                //***************LOCATIONID***************
                                try
                                {
                                    //NEEDS TO BE REWORKED
                                    if (!(worksheet.Cells[i, 22].Value).Equals(null)) //22
                                    {
                                        string destination = (worksheet.Cells[i, 22].Value).ToString();
                                        cl.locID = lm.getLocationIDFromDestination(destination);
                                    }
                                    else
                                    {
                                        cl.locID = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    cl.locID = 1;
                                }

                                cl.typeID = 3;  //The type ID for clothing is always 3
                                cl.size = "";   //Not used
                                cl.colour = ""; //Not used
                                //Adds the clothing to the list of type clothing
                                listClothing.Add(cl);
                                o = cl as Object;
                            }
                            //***************CLUBS***************
                            else
                            {
                                //***************SKU***************
                                //Checks to see if column 3 has a value
                                if (!Convert.ToInt32(worksheet.Cells[i, 3].Value).Equals(null)) //Column 3 = Sku
                                {
                                    //Sets the club sku to the value in column 3
                                    c.sku = Convert.ToInt32(worksheet.Cells[i, 3].Value);
                                }
                                else
                                {
                                    //Should NEVER happen but is used as a catch
                                    c.sku = 0;
                                }
                                //***************BRAND ID***************
                                int bName = idu.brandName(itemType.ToString());
                                if (!bName.Equals(null))
                                {
                                    c.brandID = bName; //Brand ID will be a type of club
                                }
                                else
                                {
                                    c.brandID = 1;
                                }
                                //***************MODEL ID***************                                
                                try
                                {
                                    string mName;
                                    mName = (worksheet.Cells[i, 6].Value).ToString(); //Column 6 = model name
                                    if (mName == null)
                                    {
                                        c.modelID = 1;
                                    }
                                    else
                                    {
                                        int mID = idu.modelName(mName);
                                        if (!mID.Equals(null))
                                            //Database doesn't like 360 so hardcoded in 
                                            if (mName == "360") { c.modelID = 17; }
                                            else { c.modelID = mID; }

                                        else
                                            c.modelID = 1;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //1427 = N/A
                                    c.modelID = 1427;
                                }
                                //***************COST***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 12].Value).Equals(null)) //Column 12 = cost
                                    {
                                        c.cost = Convert.ToDouble(worksheet.Cells[i, 12].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.cost = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.cost = 0;
                                }
                                //***************PRICE***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 15].Value).Equals(null)) //Column 15 = price
                                    {
                                        c.price = Convert.ToDouble(worksheet.Cells[i, 15].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.price = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.price = 0;
                                }
                                //***************QUANTITY***************
                                try
                                {
                                    if (!Convert.ToInt32(worksheet.Cells[i, 13].Value).Equals(null)) //Column 13 = quantity
                                    {
                                        c.quantity = Convert.ToInt32(worksheet.Cells[i, 13].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.quantity = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.quantity = 0;
                                }
                                //***************COMMENTS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 16].Value).Equals(null)) //Column 16 = comments
                                    {
                                        c.comments = (string)(worksheet.Cells[i, 16].Value);
                                    }
                                    else
                                    {
                                        //Sometime not given
                                        c.comments = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.comments = "";
                                }
                                //***************PREMIUM***************
                                try
                                {
                                    if (!Convert.ToDouble(worksheet.Cells[i, 11].Value).Equals(null)) //Column 11 = premium
                                    {
                                        c.premium = Convert.ToDouble(worksheet.Cells[i, 11].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.premium = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.premium = 0;
                                }
                                //***************CLUB TYPE***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 7].Value).Equals(null)) //Column 7 = clubType
                                    {
                                        c.clubType = (string)(worksheet.Cells[i, 7].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.clubType = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.clubType = "";
                                }
                                //***************SHAFT***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 8].Value).Equals(null)) //Column 8 = shaft
                                    {
                                        c.shaft = (string)(worksheet.Cells[i, 8].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.shaft = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaft = "";
                                }
                                //***************NUMBER OF CLUBS***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 9].Value).Equals(null)) //Column 9 = numberOfClubs
                                    {
                                        c.numberOfClubs = (string)(worksheet.Cells[i, 9].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.numberOfClubs = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.numberOfClubs = "";
                                }
                                //***************CLUB SPEC***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 18].Value).Equals(null)) //Column 18 = clubSpec
                                    {
                                        c.clubSpec = (string)(worksheet.Cells[i, 18].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.clubSpec = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.clubSpec = "";
                                }
                                //***************SHAFT SPEC***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 19].Value).Equals(null)) //Column 19 = shaftSpec
                                    {
                                        c.shaftSpec = (string)(worksheet.Cells[i, 19].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.shaftSpec = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaftSpec = "";
                                }
                                //***************SHAFT FLEX***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 20].Value).Equals(null)) //Column 20 = shaftFlex
                                    {
                                        c.shaftFlex = (string)(worksheet.Cells[i, 20].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.shaftFlex = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.shaftFlex = "";
                                }
                                //***************DEXTERITY***************
                                try
                                {
                                    if (!(worksheet.Cells[i, 21].Value).Equals(null)) //Column 21 = dexterity
                                    {
                                        c.dexterity = (string)(worksheet.Cells[i, 21].Value);
                                    }
                                    else
                                    {
                                        //Sometimes not given
                                        c.dexterity = "";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.dexterity = "";
                                }
                                //***************LOCATIONID***************
                                try
                                {
                                    //NEEDS TO BE REWORKED
                                    if (!(worksheet.Cells[i, 22].Value).Equals(null)) //22
                                    {
                                        string destination = (worksheet.Cells[i, 22].Value).ToString();
                                        c.itemlocation = lm.getLocationIDFromDestination(destination);
                                    }
                                    else
                                    {
                                        c.itemlocation = 1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    c.itemlocation = 1;
                                }



                                c.typeID = 1;    //The type ID of a club is always 1
                                c.used = false;  //Not used
                                //Adds the club to the list of type club
                                listClub.Add(c);
                                o = c as Object;
                            }
                            //Looks for the item in the database
                            ssm.checkForItem(o);
                        }
                    }
                }
            }
        }
        //This method was meant to import the previous customers, but is filled with errors and is not being used
        public void importCustomers(FileUpload fup)
        {
            Excel.Application xlApp = new Excel.Application();
            //string path = fup.PostedFile.FileName;
            //System.Web.HttpContext.Current.Server.MapPath(fup.FileName)
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string path = Path.Combine(pathUser, "Downloads\\");
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path + fup.FileName);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 2; i <= rowCount; i++)
            {
                string itemType = (string)((xlRange.Cells[i, 5] as Range).Value2);

                //Write the value to the console, and start gathering item info for insert
                if (xlRange.Cells[i] != null && xlRange.Cells[i].Value2 != null)
                {
                    //tbl_customers: custID, firstName, lastName, primaryAddress, secondaryAddress, primaryPhoneINT, secondaryPhoneINT
                    //billingAddress, email, city, provStateID, country, postZip                    
                    //First Name
                    if ((xlWorksheet.Cells[i, 2] as Range).Value2 != null)
                        cu.firstName = (xlWorksheet.Cells[i, 2] as Range).Value2;
                    else
                        cu.firstName = "";
                    //Last Name
                    if ((xlWorksheet.Cells[i, 3] as Range).Value2 != null)
                        cu.lastName = (xlWorksheet.Cells[i, 3] as Range).Value2;
                    else
                        cu.lastName = "";
                    //primaryAddress
                    if ((xlWorksheet.Cells[i, 5] as Range).Value2 != null)
                        cu.primaryAddress = (xlWorksheet.Cells[i, 5] as Range).Value2;
                    else
                        cu.primaryAddress = "";
                    //primaryPhoneINT
                    if ((xlWorksheet.Cells[i, 9] as Range).Value2 != null)
                        cu.primaryPhoneNumber = (xlWorksheet.Cells[i, 9] as Range).Value2;
                    else
                        cu.primaryPhoneNumber = "";
                    //secondaryPhoneINT
                    if ((xlWorksheet.Cells[i, 10] as Range).Value2 != null)
                        cu.secondaryPhoneNumber = (xlWorksheet.Cells[i, 10] as Range).Value2;
                    else
                        cu.secondaryPhoneNumber = "";
                    //email
                    if ((xlWorksheet.Cells[i, 11] as Range).Value2 != null)
                        cu.email = (xlWorksheet.Cells[i, 11] as Range).Value2;
                    else
                        cu.email = "";
                    //city
                    if ((xlWorksheet.Cells[i, 6] as Range).Value2 != null)
                        cu.city = (xlWorksheet.Cells[i, 6] as Range).Value2;
                    else
                        cu.city = "";
                    //provStateID
                    if ((xlWorksheet.Cells[i, 7] as Range).Value2 != null)
                    {
                        string provinceName = (xlWorksheet.Cells[i, 7] as Range).Value2;
                        cu.province = lm.pronvinceID(provinceName);
                    }
                    else
                        cu.province = 1;
                    //country                    
                    cu.country = lm.countryIDFromProvince(cu.province);
                    //postZip
                    if ((xlWorksheet.Cells[i, 8] as Range).Value2 != null)
                        cu.postalCode = (xlWorksheet.Cells[i, 8] as Range).Value2;
                    else
                        cu.postalCode = "";

                    cu.secondaryAddress = "";
                    //cu.billingAddress = "";
                }
                ssm.addCustomer(cu);
            }

        }
        //********************EXPORTING***************************************************************

        //Export clubs table to excel file in users Downloads folder        
        public void exportClubs()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_clubs", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\ClubsInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        } //**Incorrect format
        //Export clothing table to excel file in users Downloads folder
        public void exportClothing()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_clothing", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\ClothingInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        } //**Incorrect format
        //Export accessories table to excel file in users Downloads folder
        public void exportAccessories()
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbl_accessories", sqlCon);
            System.Data.DataTable dtMainSQLData = new System.Data.DataTable();
            da.Fill(dtMainSQLData);
            DataColumnCollection dcCollection = dtMainSQLData.Columns;

            // Export Data into EXCEL Sheet
            Microsoft.Office.Interop.Excel.Application ExcelApp = new
            Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);

            for (int i = 1; i < dtMainSQLData.Rows.Count + 2; i++)
            {
                for (int j = 1; j < dtMainSQLData.Columns.Count + 1; j++)
                {
                    if (i == 1)
                    {
                        ExcelApp.Cells[i, j] = dcCollection[j - 1].ToString();
                    }
                    else
                        ExcelApp.Cells[i, j] = dtMainSQLData.Rows[i - 2][j - 1].ToString();
                }
            }
            //Get users profile, downloads folder path, and save to workstation
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\AccessoriesInventory-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            ExcelApp.ActiveWorkbook.Saved = true;
            ExcelApp.Quit();
        } //**Incorrect format
        //Export all items in inventory
        public System.Data.DataTable exportAllItems()
        {
            //This is the table that has all of the information lined up the way Caspio needs it to be
            exportTable = new System.Data.DataTable();

            exportTable.Columns.Add("Vendor", typeof(string));
            exportTable.Columns.Add("Store_ID", typeof(string));
            exportTable.Columns.Add("ItemNumber", typeof(string));
            exportTable.Columns.Add("Shipment_Date", typeof(string));
            exportTable.Columns.Add("Brand", typeof(string));
            exportTable.Columns.Add("Model", typeof(string));
            exportTable.Columns.Add("Club_Type", typeof(string));
            exportTable.Columns.Add("Shaft", typeof(string));
            exportTable.Columns.Add("Number_of_Clubs", typeof(string));
            exportTable.Columns.Add("Tradein_Price", typeof(double));
            exportTable.Columns.Add("Premium", typeof(double));
            exportTable.Columns.Add("WE PAY", typeof(double));
            exportTable.Columns.Add("QUANTITY", typeof(int));
            exportTable.Columns.Add("Ext'd Price", typeof(double));
            exportTable.Columns.Add("RetailPrice", typeof(double));
            exportTable.Columns.Add("Comments", typeof(string));
            exportTable.Columns.Add("Image", typeof(string));
            exportTable.Columns.Add("Club_Spec", typeof(string));
            exportTable.Columns.Add("Shaft_Spec", typeof(string));
            exportTable.Columns.Add("Shaft_Flex", typeof(string));
            exportTable.Columns.Add("Dexterity", typeof(string));
            exportTable.Columns.Add("Destination", typeof(string));
            exportTable.Columns.Add("Received", typeof(string));
            exportTable.Columns.Add("Paid", typeof(string));

            exportAllAdd_Clubs();
            exportAllAdd_Accessories();
            exportAllAdd_Clothing();
            //Returns the table
            return exportTable;
        }
        //Puts the clubs in the export table
        public void exportAllAdd_Clubs()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //This query gathers all required information. No other methods needed
            cmd.CommandText = "select tbl_clubs.sku, " +
                                    "(select tbl_model.modelName from tbl_model where tbl_model.modelID = tbl_clubs.modelID ) as modelName , " +
                                    "(select tbl_brand.brandName from tbl_brand where tbl_brand.brandID = tbl_clubs.brandID ) as brandName , " +
                                    "tbl_clubs.clubType, tbl_clubs.shaft, tbl_clubs.numberOfClubs, tbl_clubs.premium, tbl_clubs.cost, tbl_clubs.price," +
                                    "tbl_clubs.quantity, tbl_clubs.clubSpec, tbl_clubs.shaftSpec, tbl_clubs.shaftFlex, tbl_clubs.dexterity, " +
                                    "(select tbl_itemType.typeDescription from tbl_itemType where tbl_itemType.typeID = tbl_clubs.typeID ) as itemType , " +
                                    "(select tbl_location.locationName from tbl_location where tbl_location.locationID = tbl_clubs.locationID) as locationName, " +
                                    "(select tbl_location.secondaryIdentifier from tbl_location where tbl_location.locationID = tbl_clubs.locationID) as locationSecondary, " +
                                    "tbl_clubs.used, tbl_clubs.comments from tbl_clubs";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exportTable.Rows.Add("",
                    reader["locationName"].ToString(),
                    (Convert.ToInt32(reader["sku"])).ToString(),
                    "",
                    reader["brandName"].ToString(),
                    reader["modelName"].ToString(),
                    reader["clubType"].ToString(),
                    reader["shaft"].ToString(),
                    reader["numberOfClubs"].ToString(),
                    0,
                    Convert.ToDouble(reader["premium"]),
                    Convert.ToDouble(reader["cost"]),
                    Convert.ToInt32(reader["quantity"]),
                    0,
                    Convert.ToDouble(reader["price"]),
                    reader["comments"].ToString(),
                    "",
                    reader["clubSpec"].ToString(),
                    reader["shaftSpec"].ToString(),
                    reader["shaftFlex"].ToString(),
                    reader["dexterity"].ToString(),
                    reader["locationSecondary"].ToString(),
                    "",
                    "");
            }
            conn.Close();
        }
        //Puts the accessories in the export table
        public void exportAllAdd_Accessories()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //This query gathers all required information. No other methods needed
            cmd.CommandText = "select tbl_accessories.sku, tbl_accessories.size, tbl_accessories.colour, tbl_accessories.price, tbl_accessories.cost, " +
                                    "(select tbl_model.modelName from tbl_model where tbl_model.modelID = tbl_accessories.modelID ) as modelName , " +
                                    "(select tbl_brand.brandName from tbl_brand where tbl_brand.brandID = tbl_accessories.brandID ) as brandName ," +
                                    "tbl_accessories.accessoryType, tbl_accessories.quantity, " +
                                    "(select tbl_itemType.typeDescription from tbl_itemType where tbl_itemType.typeID = tbl_accessories.typeID ) as itemType , " +
                                    "(select tbl_location.locationName from tbl_location where tbl_location.locationID = tbl_accessories.locationID) as locationName, " +
                                    "(select tbl_location.secondaryIdentifier from tbl_location where tbl_location.locationID = tbl_accessories.locationID) as locationSecondary, " +
                                    "tbl_accessories.comments " +
                                    "from tbl_accessories; ";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exportTable.Rows.Add("",
                    reader["locationName"].ToString(),
                    (Convert.ToInt32(reader["sku"])).ToString(),
                    "",
                    reader["brandName"].ToString(),
                    reader["modelName"].ToString(),
                    "",
                    "",
                    "",
                    0,
                    0,
                    Convert.ToDouble(reader["cost"]),
                    Convert.ToInt32(reader["quantity"]),
                    0,
                    Convert.ToDouble(reader["price"]),
                    "", "", "", "", "", "",
                    reader["locationSecondary"].ToString(),
                    "", "");
            }
            conn.Close();
        }
        //Puts the clothing in the export table
        public void exportAllAdd_Clothing()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //This query gathers all required information. No other methods needed
            cmd.CommandText = "select tbl_clothing.sku, tbl_clothing.size, tbl_clothing.colour, tbl_clothing.gender, tbl_clothing.style, tbl_clothing.price, tbl_clothing.cost,  " +
                                    "(select tbl_brand.brandName from tbl_brand where tbl_brand.brandID = tbl_clothing.brandID ) as brandName , " +
                                    "tbl_clothing.quantity, " +
                                    "(select tbl_itemType.typeDescription from tbl_itemType where tbl_itemType.typeID = tbl_clothing.typeID ) as itemType ,  " +
                                    "(select tbl_location.locationName from tbl_location where tbl_location.locationID = tbl_clothing.locationID) as locationName,  " +
                                    "(select tbl_location.secondaryIdentifier from tbl_location where tbl_location.locationID = tbl_clothing.locationID) as locationSecondary,  " +
                                    "tbl_clothing.comments from tbl_clothing;";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                exportTable.Rows.Add("",
                    reader["locationName"].ToString(),
                    (Convert.ToInt32(reader["sku"])).ToString(),
                    "",
                    reader["brandName"].ToString(),
                    reader["gender"].ToString(),
                    reader["style"].ToString(),
                    "",
                    "",
                    0,
                    0,
                    Convert.ToDouble(reader["cost"]),
                    Convert.ToInt32(reader["quantity"]),
                    0,
                    Convert.ToDouble(reader["price"]),
                    "", "", "", "", "", "",
                    reader["locationSecondary"].ToString(),
                    "", "");
            }
            conn.Close();
        }
        //Export ALL sales/invoices to excel
        public void exportAllSalesToExcel()
        {
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
            //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
            //provincialTax, balanceDue, transactionType, comments

            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
            //itemPrice, itemDiscount, percentage

            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid

            //Gets the invoice data and puts it in a table
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            SqlDataAdapter im = new SqlDataAdapter("SELECT * FROM tbl_invoice", sqlCon);
            System.Data.DataTable dtim = new System.Data.DataTable();
            im.Fill(dtim);
            DataColumnCollection dcimHeaders = dtim.Columns;
            sqlCon.Close();
            //Gets the invoice item data and puts it in a table
            sqlCon.Open();
            SqlDataAdapter ii = new SqlDataAdapter("SELECT * FROM tbl_invoiceItem", sqlCon);
            System.Data.DataTable dtii = new System.Data.DataTable();
            ii.Fill(dtii);
            DataColumnCollection dciiHeaders = dtii.Columns;
            sqlCon.Close();
            //Gets the invoice mop data and puts it in a table
            sqlCon.Open();
            SqlDataAdapter imo = new SqlDataAdapter("SELECT * FROM tbl_invoiceMOP", sqlCon);
            System.Data.DataTable dtimo = new System.Data.DataTable();
            imo.Fill(dtimo);
            DataColumnCollection dcimoHeaders = dtimo.Columns;
            sqlCon.Close();

            //Initiating Everything
            initiateInvoiceTable();
            exportSales_Invoice();
            initiateInvoiceItemTable();
            exportSales_Items();
            initiateInvoiceMOPTable();
            exportSales_MOP();


            //// Export Data into EXCEL Sheet
            //Application ExcelApp = new Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //Sheets worksheets = ExcelApp.Worksheets;

            //var xlInvoiceMain = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            //xlInvoiceMain.Name = "InvoiceMain";

            //var xlInvoiceItem = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            //xlInvoiceItem.Name = "InvoiceItems";

            //var xlInvoiceMOPS = (Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            //xlInvoiceMOPS.Name = "InvoiceMOPS";


            ////Export mop invoice
            //for (int i = 1; i < exportInvoiceMOPTable.Rows.Count + 2; i++)
            //{
            //    for (int j = 1; j < exportInvoiceMOPTable.Columns.Count + 1; j++)
            //    {
            //        if (i == 1)
            //        {
            //            xlInvoiceMOPS.Cells[i, j] = dcCollection[j - 1].ToString();
            //        }
            //        else
            //            xlInvoiceMOPS.Cells[i, j] = exportInvoiceMOPTable.Rows[i - 2][j - 1].ToString();
            //    }
            //}
            ////Export item invoice
            //for (int i = 1; i < exportInvoiceItemTable.Rows.Count + 2; i++)
            //{
            //    for (int j = 1; j < exportInvoiceItemTable.Columns.Count + 1; j++)
            //    {
            //        if (i == 1)
            //        {
            //            xlInvoiceItem.Cells[i, j] = dcCollection[j - 1].ToString();
            //        }
            //        else
            //            xlInvoiceItem.Cells[i, j] = exportInvoiceItemTable.Rows[i - 2][j - 1].ToString();
            //    }
            //}
            ////Export main invoice
            //for (int i = 1; i < exportInvoiceTable.Rows.Count + 2; i++)
            //{
            //    for (int j = 1; j < exportInvoiceTable.Columns.Count + 1; j++)
            //    {
            //        if (i == 1)
            //        {
            //            xlInvoiceMain.Cells[i, j] = dcCollection[j - 1].ToString();
            //        }
            //        else
            //            xlInvoiceMain.Cells[i, j] = exportInvoiceTable.Rows[i - 2][j - 1].ToString();
            //    }
            //}


            ////Get users profile, downloads folder path, and save to workstation
            //string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            //string pathDownload = Path.Combine(pathUser, "Downloads");
            //ExcelApp.ActiveWorkbook.SaveCopyAs(pathDownload + "\\AllSales-" + DateTime.Now.ToString("d MMM yyyy") + ".xlsx");
            //ExcelApp.ActiveWorkbook.Saved = true;
            //ExcelApp.Quit();
        }
        //Initiates the invoice table
        public System.Data.DataTable initiateInvoiceTable()
        {
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
            //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
            //provincialTax, balanceDue, transactionType, comments
            exportInvoiceTable = new System.Data.DataTable();
            exportInvoiceTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceTable.Columns.Add("invoiceDate", typeof(string));
            exportInvoiceTable.Columns.Add("invoiceTime", typeof(string));
            exportInvoiceTable.Columns.Add("custID", typeof(string));
            exportInvoiceTable.Columns.Add("empID", typeof(string));
            exportInvoiceTable.Columns.Add("locationID", typeof(string));
            exportInvoiceTable.Columns.Add("subTotal", typeof(string));
            exportInvoiceTable.Columns.Add("discountAmount", typeof(string));
            exportInvoiceTable.Columns.Add("tradeinAmount", typeof(string));
            exportInvoiceTable.Columns.Add("governmentTax", typeof(string));
            exportInvoiceTable.Columns.Add("provincialTax", typeof(string));
            exportInvoiceTable.Columns.Add("balanceDue", typeof(string));
            exportInvoiceTable.Columns.Add("transactionType", typeof(string));
            exportInvoiceTable.Columns.Add("comments", typeof(string));
            exportSales_Invoice();

            return exportInvoiceTable;
        }
        //Initiates the invoice item table
        public System.Data.DataTable initiateInvoiceItemTable()
        {
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
            //itemPrice, itemDiscount, percentage
            exportInvoiceItemTable = new System.Data.DataTable();
            exportInvoiceItemTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceItemTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceItemTable.Columns.Add("sku", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemQuantity", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemCost", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemPrice", typeof(string));
            exportInvoiceItemTable.Columns.Add("itemDiscount", typeof(string));
            exportInvoiceItemTable.Columns.Add("percentage", typeof(string));
            exportSales_Items();

            return exportInvoiceItemTable;
        }
        //Initiates the invoice mop table
        public System.Data.DataTable initiateInvoiceMOPTable()
        {
            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
            exportInvoiceMOPTable = new System.Data.DataTable();
            exportInvoiceMOPTable.Columns.Add("ID", typeof(string));
            exportInvoiceMOPTable.Columns.Add("invoiceNum", typeof(string));
            exportInvoiceMOPTable.Columns.Add("invoiceSubNum", typeof(string));
            exportInvoiceMOPTable.Columns.Add("mopType", typeof(string));
            exportInvoiceMOPTable.Columns.Add("amountPaid", typeof(string));
            exportSales_MOP();

            return exportInvoiceMOPTable;
        }
        //Gets the invoice data and puts it in a table
        public void exportSales_Invoice()
        {
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID,
            //locationID, subTotal, discountAmount, tradeinAmount, governmentTax,
            //provincialTax, balanceDue, transactionType, comments
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoice";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string invoiceNum = reader["invoiceNum"].ToString();
                string invoiceSubNum = reader["invoiceSubNum"].ToString();
                string invoiceDate = reader["invoiceDate"].ToString();
                string invioceTime = reader["invoiceTime"].ToString();
                string custID = reader["custID"].ToString();
                string empID = reader["empID"].ToString();
                string locationID = reader["locationID"].ToString();
                string subTotal = reader["subTotal"].ToString();
                string discountAmount = reader["discountAmount"].ToString();
                string tradeinAmount = reader["tradeinAmount"].ToString();
                string governmentTax = reader["governmentTax"].ToString();
                string provincialTax = reader["provincialTax"].ToString();
                string balanceDue = reader["balanceDue"].ToString();
                string transactionType = reader["transactionType"].ToString();
                string comments = reader["comments"].ToString();
                exportInvoiceTable.Rows.Add(invoiceNum, invoiceSubNum, invoiceDate, invioceTime,
                    custID, empID, locationID, subTotal, discountAmount,
                    tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments);
            }
            conn.Close();
        }
        //Gets the invoice item data and puts it in a table
        public void exportSales_Items()
        {
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
            //itemPrice, itemDiscount, percentage
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoiceItem";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string invoiceNum = reader["invoiceNum"].ToString();
                string invoiceSubNum = reader["invoiceSubNum"].ToString();
                string sku = reader["sku"].ToString();
                string itemQuantity = reader["itemQuantity"].ToString();
                string itemCost = reader["itemCost"].ToString();
                string itemPrice = reader["itemPrice"].ToString();
                string itemDisocunt = reader["itemDiscount"].ToString();
                string percentage = reader["percentage"].ToString();
                exportInvoiceItemTable.Rows.Add(invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost,
                    itemPrice, itemDisocunt, percentage);
            }
            conn.Close();
        }
        //Gets the invoice mop data and puts it in a table
        public void exportSales_MOP()
        {
            //ID, invoiceNum, invoiceSubNum, mopType, amountPaid
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * from tbl_invoiceMOP";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string ID = reader["ID"].ToString();
                string invoiceNum = reader["invoiceNum"].ToString();
                string invoiceSubNum = reader["invoiceSubNum"].ToString();
                string mopType = reader["mopType"].ToString();
                string amountPaid = reader["amountPaid"].ToString();
                exportInvoiceMOPTable.Rows.Add(ID, invoiceNum, invoiceSubNum, mopType, amountPaid);
            }
            conn.Close();
        }
    }
}