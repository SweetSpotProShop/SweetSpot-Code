using OfficeOpenXml;
using SweetSpotDiscountGolfPOS.Misc;
using SweetSpotDiscountGolfPOS.OB;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS.FP
{
    public class ImportExport
    {
        readonly DatabaseCalls DBC = new DatabaseCalls();

        public DataTable CallUploadItems(FileUpload fup, CurrentUser cu, object[] objPageDetails)
        {
            return UploadItems(fup, cu, objPageDetails);
        }
        public DataTable CallSpecialUpdateTool(FileUpload fupSU, string strRColumn, string strUColumn, CurrentUser cu, object[] objPageDetails)
        {
            return SpecialUpdateTool(fupSU, strRColumn, strUColumn, cu, objPageDetails);
        }
        private DataTable UploadItems(FileUpload fup, CurrentUser cu, object[] objPageDetails)
        {

            //***************************************************************************************************
            //Step 1: Create datatable to hold the items found in the excel sheet
            //***************************************************************************************************
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            //Datatable to hold any skus that have errors
            DataTable skusWithErrors = new DataTable();
            DataTable listItems = new DataTable();

            listItems.Columns.Add("varSku"); //("sku");
            listItems.Columns.Add("varBrandName"); //("brandName");
            listItems.Columns.Add("varModelName"); //("modelName");
            listItems.Columns.Add("fltCost"); //("cost");
            listItems.Columns.Add("fltPrice"); //("price");
            listItems.Columns.Add("intQuantity"); //("quantity");
            listItems.Columns.Add("varAdditionalInformation"); //("comments");
            listItems.Columns.Add("fltPremiumCharge"); //("premium");
            listItems.Columns.Add("varTypeOfClub"); //("clubType");
            listItems.Columns.Add("varShaftType"); //("shaft");
            listItems.Columns.Add("varNumberOfClubs"); //("numberOfClubs");
            listItems.Columns.Add("varClubSpecification"); //("clubSpec");
            listItems.Columns.Add("varShaftSpecification"); //("shaftSpec");
            listItems.Columns.Add("varShaftFlexability"); //("shaftFlex");
            listItems.Columns.Add("varClubDexterity"); //("dexterity");
            listItems.Columns.Add("varLocationName"); //("locationName");
            listItems.Columns.Add("varItemType"); //("itemType");
            listItems.Columns.Add("bitIsUsedProduct"); //
            listItems.Columns.Add("varProdID"); //
            //listItems.Columns.Add//("size");
            //listItems.Columns.Add//("colour");

            //Database connections
            SqlConnection con = new SqlConnection(connectionString);
            SqlConnection conTempDB = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "IF OBJECT_ID('tempItemStorage', 'U') IS NOT NULL DROP TABLE tempItemStorage; IF OBJECT_ID('tempErrorSkus', 'U') IS NOT NULL DROP TABLE tempErrorSkus;";
            conTempDB.Open();
            cmd.Connection = conTempDB;
            reader = cmd.ExecuteReader();
            conTempDB.Close();

            //***************************************************************************************************
            //Step 2: Check to see if there is any data in the uploaded file
            //***************************************************************************************************

            //If there are files, proceed
            if (fup.HasFiles)
            {
                //***************************************************************************************************
                //Step 3: Create an excel sheet and set its content to the uploaded file
                //***************************************************************************************************
                //Load the uploaded file into the memorystream
                using (MemoryStream stream = new MemoryStream(fup.FileBytes))
                //Lets the server know to use the excel package
                using (ExcelPackage xlPackage = new ExcelPackage(stream))
                {
                    con = new SqlConnection(connectionString);
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Trade-in_Detail_Report_"];
                    var rowCnt = worksheet.Dimension.End.Row; //Gets the row count                   
                    var colCnt = worksheet.Dimension.End.Column; //Gets the column count
                    //***************************************************************************************************
                    //Step 4: Looping through the data found in the excel sheet and storing it in the datatable
                    //***************************************************************************************************
                    //Beginning the loop for data gathering
                    for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                    {
                        string itemType = (worksheet.Cells[i, 6].Value).ToString(); //Column 25 = itemType
                        //Adding items to the datatables 
                        //***************************************************************************************************
                        //Step 4: Option A: The item type is Apparel
                        //***************************************************************************************************
                        if (itemType.Equals("Apparel"))
                        {
                            listItems.Rows.Add(
                                //***************SKU**********************
                                worksheet.Cells[i, 4].Value.ToNullSafeString(),
                                //***************BRAND NAME***************
                                itemType.ToString(),
                                //***************MODEL NAME***************
                                //'N/A'
                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()), 
                                //***************COST*********************
                                Convert.ToDouble(worksheet.Cells[i, 13].Value),
                                //***************PRICE********************
                                Convert.ToDouble(worksheet.Cells[i, 16].Value),
                                //***************QUANTITY*****************
                                Convert.ToInt32(worksheet.Cells[i, 14].Value),
                                //***************COMMENTS*****************
                                (string)(worksheet.Cells[i, 17].Value.ToNullSafeString()),
                                //***************PREMIUM******************
                                Convert.ToDouble(0),
                                //***************CLUB TYPE****************
                                //Style for clothing
                                (string)(worksheet.Cells[i, 8].Value.ToNullSafeString()),
                                //***************SHAFT********************
                                //Colour for clothing
                                (string)(worksheet.Cells[i, 9].Value.ToNullSafeString()),
                                //***************NUMBER OF CLUBS**********
                                //Size for clothing
                                (string)(worksheet.Cells[i, 10].Value.ToNullSafeString()),
                                //***************CLUB SPEC****************
                                //Gender for clothing
                                (string)(worksheet.Cells[i, 19].Value.ToNullSafeString()), 
                                //***************SHAFT SPEC***************
                                "",
                                //***************SHAFT FLEX***************
                                "",
                                //***************DEXTERITY****************
                                "",
                                //***************LOCATION NAME************
                                //Second Location
                                (string)(worksheet.Cells[i, 23].Value.ToNullSafeString()), 
                                //***************ITEM TYPE****************
                                3,
                                //***************USED PRODUCT*************
                                Convert.ToBoolean(false),
                                //***************PROD ID******************
                                (string)(worksheet.Cells[i,1].Value.ToNullSafeString())
                            );
                        }
                        //***************************************************************************************************
                        //Step 4: Option B: The item type is Accessories
                        //***************************************************************************************************
                        else if (itemType.Equals("Accessories"))
                        {
                            listItems.Rows.Add(
                                //***************SKU***************
                                worksheet.Cells[i, 4].Value.ToNullSafeString(),
                                //***************BRAND NAME***************
                                itemType.ToString(),
                                //***************MODEL Name***************        
                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()),
                                //***************COST***************
                                Convert.ToDouble(worksheet.Cells[i, 13].Value),
                                //***************PRICE***************
                                Convert.ToDouble(worksheet.Cells[i, 16].Value),
                                //***************QUANTITY***************
                                Convert.ToInt32(worksheet.Cells[i, 14].Value),
                                //***************COMMENTS***************
                                (string)(worksheet.Cells[i, 17].Value.ToNullSafeString()),
                                //***************PREMIUM***************
                                Convert.ToDouble(0),
                                //***************CLUB TYPE***************
                                //accessoryType
                                (string)(worksheet.Cells[i, 8].Value.ToNullSafeString()),
                                //***************SHAFT***************
                                //Colour for accessory
                                (string)(worksheet.Cells[i, 9].Value.ToNullSafeString()),
                                //***************NUMBER OF CLUBS***************
                                //Size for accessory
                                (string)(worksheet.Cells[i, 10].Value.ToNullSafeString()),

                                //***************CLUB SPEC***************
                                "",
                                //***************SHAFT SPEC***************
                                "",
                                //***************SHAFT FLEX***************
                                "",
                                //***************DEXTERITY***************
                                "",
                                //***************LOCATION NAME***************
                                (string)(worksheet.Cells[i, 23].Value.ToNullSafeString()),
                                //***************ITEM TYPE***************
                                2,
                                //***************USED PRODUCT*************
                                Convert.ToBoolean(false),
                                //***************PROD ID******************
                                (string)(worksheet.Cells[i, 1].Value.ToNullSafeString())
                            );
                        }
                        //***************************************************************************************************
                        //Step 4: Option C: The item type is a club
                        //***************************************************************************************************
                        else
                        {
                            listItems.Rows.Add(
                                //***************SKU***************
                                worksheet.Cells[i, 4].Value.ToNullSafeString(),
                                //***************BRAND NAME***************
                                itemType.ToString(),
                                //***************MODEL Name***************        
                                (string)(worksheet.Cells[i, 7].Value.ToNullSafeString()),
                                //***************COST***************
                                Convert.ToDouble(worksheet.Cells[i, 13].Value),
                                //***************PRICE***************
                                Convert.ToDouble(worksheet.Cells[i, 16].Value),
                                //***************QUANTITY***************
                                Convert.ToInt32(worksheet.Cells[i, 14].Value),
                                //***************COMMENTS***************
                                (string)(worksheet.Cells[i, 17].Value.ToNullSafeString()),
                                //***************PREMIUM***************
                                Convert.ToDouble(worksheet.Cells[i, 12].Value),
                                //***************CLUB TYPE***************
                                (string)(worksheet.Cells[i, 8].Value.ToNullSafeString()),
                                //***************SHAFT***************
                                (string)(worksheet.Cells[i, 9].Value.ToNullSafeString()),
                                //***************NUMBER OF CLUBS***************
                                (string)(worksheet.Cells[i, 10].Value.ToNullSafeString()),
                                //***************CLUB SPEC***************
                                (string)(worksheet.Cells[i, 19].Value.ToNullSafeString()),
                                //***************SHAFT SPEC***************
                                (string)(worksheet.Cells[i, 20].Value.ToNullSafeString()),
                                //***************SHAFT FLEX***************
                                (string)(worksheet.Cells[i, 21].Value.ToNullSafeString()),
                                //***************DEXTERITY***************
                                (string)(worksheet.Cells[i, 22].Value.ToNullSafeString()),
                                //***************LOCATION NAME***************
                                (string)(worksheet.Cells[i, 23].Value.ToNullSafeString()),
                                //***************ITEM TYPE***************
                                1,
                                //***************USED PRODUCT*************
                                Convert.ToBoolean(false),
                                //***************PROD ID******************
                                (string)(worksheet.Cells[i, 1].Value.ToNullSafeString())
                            );
                        }
                    }

                    //***************************************************************************************************
                    //Step 5: Create the temp tables for storing the items and skus that cause an error
                    //***************************************************************************************************

                    //Creating the temp tables  
                    conTempDB.Open();
                    cmd.CommandText = "CREATE TABLE tempItemStorage(varSku VARCHAR(25), intBrandID INT, intModelID INT, varTypeOfClub VARCHAR(150), varShaftType VARCHAR(150), "
                        + "varNumberOfClubs VARCHAR(150), fltPremiumCharge FLOAT, fltCost FLOAT, fltPrice FLOAT, intQuantity INT, varClubSpecification VARCHAR(150), "
                        + "varShaftSpecification VARCHAR(150), varShaftFlexability VARCHAR(150), varClubDexterity VARCHAR(150), intItemTypeID INT, intLocationID INT, "
                        + "varAdditionalInformation VARCHAR(500), bitIsUsedProduct BIT, varProdID VARCHAR(10)); CREATE TABLE tempErrorSkus(varSku VARCHAR(25), intBrandError INT, intModelError INT, "
                        + "intIdentifierError INT)";
                    cmd.Connection = conTempDB;
                    reader = cmd.ExecuteReader();
                    conTempDB.Close();

                    //***************************************************************************************************
                    //Step 6: Check each item in the datatable to see if it will cause an error. If not, insert into the temp item table
                    //***************************************************************************************************

                    foreach (DataRow row in listItems.Rows)
                    {
                        con.Open();
                        //This query will look up the brand, model, and locationID of the item being passed in. 
                        //If all three are found, it will insert the item into the tempItemStorage table.
                        //If not, it is added to the tempErrorSkus table
                        cmd.CommandText = "IF((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                            "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                            "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                            "BEGIN " +
                                                "INSERT INTO tempItemStorage VALUES( " +
                                                    "@varSku, " +
                                                    "(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName), " +
                                                    "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName), " +
                                                    "@varTypeOfClub, @varShaftType, @varNumberOfClubs, @fltPremiumCharge, @fltCost, @fltPrice, @intQuantity, "
                                                    + "@varClubSpecification, @varShaftSpecification, @varShaftFlexability, @varClubDexterity, @intItemTypeID, " +
                                                    "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID), "
                                                    + "@varAdditionalInformation, @bitIsUsedProduct, @varProdID) " +
                                            "END " +
                                        "ELSE IF(NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 0, 0) " +
                                            "END " +
                                        "ELSE IF ((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                 "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                            "BEGIN " +
                                                    "INSERT INTO tempErrorSkus VALUES(@varSku, 0, 1, 0) " +
                                            "END " +
                                        "ELSE IF ((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                 "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 0, 0, 1) " +
                                            "END " +
                                        "ELSE IF (NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                 "(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID) >= 0) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 1, 0) " +
                                            "END " +
                                        "ELSE IF (NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                 "(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) >= 0 AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 0, 1) " +
                                            "END " +
                                        "ELSE IF ((SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) >= 0 AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 0, 1, 1) " +
                                            "END " +
                                        "ELSE IF (NOT EXISTS(SELECT TOP 1 tbl_brand.intBrandID FROM tbl_brand WHERE tbl_brand.varBrandName = @varBrandName) AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_model.intModelID FROM tbl_model WHERE tbl_model.varModelName = @varModelName) AND " +
                                                 "NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varSecondLocationID)) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varSku, 1, 1, 1) " +
                                            "END";
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@varSku", row[0]);
                        cmd.Parameters.AddWithValue("@varBrandName", row[1]);
                        cmd.Parameters.AddWithValue("@varModelName", row[2]);
                        cmd.Parameters.AddWithValue("@fltCost", row[3]);
                        cmd.Parameters.AddWithValue("@fltPrice", row[4]);
                        cmd.Parameters.AddWithValue("@intQuantity", row[5]);
                        cmd.Parameters.AddWithValue("@varAdditionalInformation", row[6]);
                        cmd.Parameters.AddWithValue("@fltPremiumCharge", row[7]);
                        cmd.Parameters.AddWithValue("@varTypeOfClub", row[8]);
                        cmd.Parameters.AddWithValue("@varShaftType", row[9]);
                        cmd.Parameters.AddWithValue("@varNumberOfClubs", row[10]);
                        cmd.Parameters.AddWithValue("@varClubSpecification", row[11]);
                        cmd.Parameters.AddWithValue("@varShaftSpecification", row[12]);
                        cmd.Parameters.AddWithValue("@varShaftFlexability", row[13]);
                        cmd.Parameters.AddWithValue("@varClubDexterity", row[14]);
                        cmd.Parameters.AddWithValue("@varSecondLocationID", row[15]);
                        cmd.Parameters.AddWithValue("@intItemTypeID", row[16]);
                        cmd.Parameters.AddWithValue("@bitIsUsedProduct", row[17]);
                        cmd.Parameters.AddWithValue("@varProdID", row[18]);
                        reader = cmd.ExecuteReader();
                        con.Close();
                        cmd = new SqlCommand();
                    };

                    //***************************************************************************************************
                    //Step 7: Check the error list for any data
                    //***************************************************************************************************

                    //Reading the error list
                    using (cmd = new SqlCommand("SELECT * FROM tempErrorSkus", con)) //Calling the SP
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        //Filling the table with what is found
                        da.Fill(skusWithErrors);
                    }
                    //***************************************************************************************************
                    //Step 8: If no data is found in the error table
                    //***************************************************************************************************
                    //Start inserting into actual tables
                    con.Open();
                    cmd.CommandText = "SELECT * FROM tempItemStorage";
                    DataTable temp = new DataTable();
                    using (var dataTable = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        dataTable.Fill(temp);
                    }
                    con.Close();

                    //***************************************************************************************************
                    //Step 9: Loop through the temp datatable and insert the rows into the database
                    //***************************************************************************************************
                    foreach (DataRow row in temp.Rows)
                    {
                        //loop through just one, and it will know the itemID because we set it ealier in the process                            
                        ImportNewItem(row, cu, objPageDetails);
                    }
                }
            }
            //***************************************************************************************************
            //Step 10: Delete the temp tables that were used for storage
            //***************************************************************************************************

            cmd.CommandText = "DROP TABLE tempItemStorage; DROP TABLE tempErrorSkus;";
            conTempDB.Open();
            cmd.Connection = conTempDB;
            reader = cmd.ExecuteReader();
            conTempDB.Close();
            return skusWithErrors;
        }
        private DataTable SpecialUpdateTool(FileUpload fupSU, string strRColumn, string strUColumn, CurrentUser cu, object[] objPageDetails)
        {


            //***************************************************************************************************
            //Step 1: Create datatable to hold the items found in the excel sheet
            //***************************************************************************************************
            string connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
            //Datatable to hold any skus that have errors
            DataTable skusWithErrors = new DataTable();
            DataTable listItems = new DataTable();

            listItems.Columns.Add("varReferenceColumn");
            listItems.Columns.Add("varUdateColumn");

            //Database connections
            SqlConnection con = new SqlConnection(connectionString);
            SqlConnection conTempDB = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "IF OBJECT_ID('tempItemStorage', 'U') IS NOT NULL DROP TABLE tempItemStorage; IF OBJECT_ID('tempErrorSkus', 'U') IS NOT NULL DROP TABLE tempErrorSkus;";
            conTempDB.Open();
            cmd.Connection = conTempDB;
            reader = cmd.ExecuteReader();
            conTempDB.Close();

            //***************************************************************************************************
            //Step 2: Check to see if there is any data in the uploaded file
            //***************************************************************************************************
            ItemDataUtilities IDU = new ItemDataUtilities();
            //If there are files, proceed
            if (fupSU.HasFiles)
            {
                //***************************************************************************************************
                //Step 3: Create an excel sheet and set its content to the uploaded file
                //***************************************************************************************************
                //Load the uploaded file into the memorystream
                using (MemoryStream stream = new MemoryStream(fupSU.FileBytes))
                //Lets the server know to use the excel package
                using (ExcelPackage xlPackage = new ExcelPackage(stream))
                {
                    con = new SqlConnection(connectionString);
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Special_Update"];
                    var rowCnt = worksheet.Dimension.End.Row; //Gets the row count                   
                    var colCnt = worksheet.Dimension.End.Column; //Gets the column count
                    //***************************************************************************************************
                    //Step 4: Looping through the data found in the excel sheet and storing it in the datatable
                    //***************************************************************************************************
                    //Beginning the loop for data gathering
                    string strUpdateColumnQueryString = "";
                    for (int i = 2; i <= rowCnt; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                    {

                        //If Statement to determin what type of data to store as.
                        if (strUColumn.ToString() == "ProdID" || strUColumn.ToString() == "Dexterity")
                        {
                            listItems.Rows.Add(
                                //***************Reference Column**********************
                                worksheet.Cells[i, 1].Value.ToNullSafeString(),
                                //***************Update Column***************
                                (string)(worksheet.Cells[i, 2].Value.ToNullSafeString())
                                );
                            strUpdateColumnQueryString = "varVarParameter VARCHAR(50),";
                        }
                        else if (strUColumn.ToString() == "Cost" || strUColumn.ToString() == "Price")
                        {
                            listItems.Rows.Add(
                                //***************Reference Column**********************
                                worksheet.Cells[i, 1].Value.ToNullSafeString(),
                                //***************Update Column***************
                                Convert.ToDouble(worksheet.Cells[i, 2].Value)
                                );
                            strUpdateColumnQueryString = "fltFloatParameter FLOAT, ";
                        }
                        else if (strUColumn.ToString() == "Quantity")
                        {
                            listItems.Rows.Add(
                                //***************Reference Column**********************
                                worksheet.Cells[i, 1].Value.ToNullSafeString(),
                                //***************Update Column***************
                                Convert.ToInt32(worksheet.Cells[i, 2].Value)
                                );
                            strUpdateColumnQueryString = "intIntegerParameter INT, ";
                        }
                        else if (strUColumn.ToString() == "Location")
                        {
                            listItems.Rows.Add(
                                //***************Reference Column**********************
                                worksheet.Cells[i, 1].Value.ToNullSafeString(),
                                //***************Update Column***************
                                worksheet.Cells[i, 2].Value.ToNullSafeString()
                                );
                            strUpdateColumnQueryString = "intIntegerParameter INT, ";
                        }
                    }

                    //***************************************************************************************************
                    //Step 5: Create the temp tables for storing the items and skus that cause an error
                    //***************************************************************************************************

                    


                    //Creating the temp tables  
                    conTempDB.Open();
                    cmd.CommandText = "CREATE TABLE tempItemStorage(varSku VARCHAR(25), " + strUpdateColumnQueryString + "); CREATE TABLE tempErrorSkus(varSku VARCHAR(25), " + strUpdateColumnQueryString + ")";
                    cmd.Connection = conTempDB;
                    reader = cmd.ExecuteReader();
                    conTempDB.Close();

                    //***************************************************************************************************
                    //Step 6: Check each item in the datatable to see if it will cause an error. If not, insert into the temp item table
                    //***************************************************************************************************

                    foreach (DataRow row in listItems.Rows)
                    {
                        con.Open();
                        //This query will look up the brand, model, and locationID of the item being passed in. 
                        //If all three are found, it will insert the item into the tempItemStorage table.
                        //If not, it is added to the tempErrorSkus table

                        if (strUColumn.ToString() == "Location") 
                        {
                            cmd.CommandText = "IF((SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varUpdateColumn) >= 0) " +
                                            "BEGIN " +
                                                "INSERT INTO tempItemStorage VALUES(@varReferenceColumn, (SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varUpdateColumn)) " +
                                            "END " +
                                        "ELSE IF(NOT EXISTS(SELECT TOP 1 tbl_location.intLocationID FROM tbl_location WHERE tbl_location.varSecondLocationID = @varUpdateColumn)) " +
                                            "BEGIN " +
                                                "INSERT INTO tempErrorSkus VALUES(@varReferenceColumn, 1) " +
                                            "END ";
                        } 
                        else 
                        {
                            cmd.CommandText = "BEGIN " +
                                                "INSERT INTO tempItemStorage VALUES(@varReferenceColumn, @varUpdateColumn) " +
                                            "END ";
                        }

                        

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@varReferenceColumn", row[0].ToString());


                        if (strUColumn.ToString() == "Location" || strUColumn.ToString() == "ProdID" || strUColumn.ToString() == "Dexterity")
                        {
                            cmd.Parameters.AddWithValue("@varUpdateColumn", row[1].ToString());
                        }
                        else if (strUColumn.ToString() == "Cost" || strUColumn.ToString() == "Price")
                        {
                            cmd.Parameters.AddWithValue("@varUpdateColumn", Convert.ToDouble(row[1]));
                        }
                        else if (strUColumn.ToString() == "Quantity")
                        {
                            cmd.Parameters.AddWithValue("@varUpdateColumn", Convert.ToInt32(row[1]));
                        }
                        reader = cmd.ExecuteReader();
                        con.Close();
                        cmd = new SqlCommand();
                    };

                    //***************************************************************************************************
                    //Step 7: Check the error list for any data
                    //***************************************************************************************************

                    //Reading the error list
                    using (cmd = new SqlCommand("SELECT * FROM tempErrorSkus", con)) //Calling the SP
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        //Filling the table with what is found
                        da.Fill(skusWithErrors);
                    }
                    //***************************************************************************************************
                    //Step 8: If no data is found in the error table
                    //***************************************************************************************************
                    //Start inserting into actual tables
                    con.Open();
                    cmd.CommandText = "SELECT * FROM tempItemStorage";
                    DataTable temp = new DataTable();
                    using (var dataTable = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.Text;
                        dataTable.Fill(temp);
                    }
                    con.Close();

                    //***************************************************************************************************
                    //Step 9: Loop through the temp datatable and insert the rows into the database
                    //***************************************************************************************************
                    foreach (DataRow row in temp.Rows)
                    {
                        //loop through just one, and it will know the itemID because we set it ealier in the process                            
                        IDU.SpecialUpdate(row, strRColumn, strUColumn, objPageDetails);
                    }
                }
            }
            //***************************************************************************************************
            //Step 10: Delete the temp tables that were used for storage
            //***************************************************************************************************

            cmd.CommandText = "DROP TABLE tempItemStorage; DROP TABLE tempErrorSkus;";
            conTempDB.Open();
            cmd.Connection = conTempDB;
            reader = cmd.ExecuteReader();
            conTempDB.Close();
            return skusWithErrors;
        }
        public void ImportNewItem(DataRow row, CurrentUser cu, object[] objPageDetails)
        {
            ItemDataUtilities IDU = new ItemDataUtilities();
            int inventoryID = 0;
            int itemTypeID = Convert.ToInt32(row[14]);
            string sku = row[0].ToString();
            Object o = new Object();

            inventoryID = IDU.CheckIfSkuAlreadyInDatabase(sku, itemTypeID, objPageDetails);

            if (inventoryID > 0)
            {
                //Check for item in table
                if (itemTypeID == 1)
                {
                    Clubs club = new Clubs();
                    //if item type is club then save as club class
                    club.intInventoryID = inventoryID;
                    club.fltCost = Convert.ToDouble(row[7]);
                    club.intBrandID = Convert.ToInt32(row[1]);
                    club.fltPrice = Convert.ToDouble(row[8]);
                    club.intQuantity = Convert.ToInt32(row[9]);
                    club.intLocationID = Convert.ToInt32(row[15]);
                    club.varTypeOfClub = row[3].ToString();
                    club.intModelID = Convert.ToInt32(row[2]);
                    club.varShaftType = row[4].ToString();
                    club.varNumberOfClubs = row[5].ToString();
                    club.varClubSpecification = row[10].ToString();
                    club.varShaftSpecification = row[11].ToString();
                    club.varShaftFlexability = row[12].ToString();
                    club.varClubDexterity = row[13].ToString();
                    club.varAdditionalInformation = row[16].ToString();
                    club.bitIsUsedProduct = Convert.ToBoolean(row[17]);
                    club.varProdID = row[18].ToString();
                    o = club as Object;
                }
                else if (itemTypeID == 2)
                {
                    Accessories accessory = new Accessories();
                    //if item type is accesory then save as accessory class
                    accessory.intInventoryID = inventoryID;
                    accessory.intBrandID = Convert.ToInt32(row[1]);
                    accessory.fltCost = Convert.ToDouble(row[7]);
                    accessory.fltPrice = Convert.ToDouble(row[8]);
                    accessory.intQuantity = Convert.ToInt32(row[9]);
                    accessory.intLocationID = Convert.ToInt32(row[15]);
                    accessory.varSize = row[5].ToString();
                    accessory.varColour = row[4].ToString();
                    accessory.varTypeOfAccessory = row[3].ToString();
                    accessory.intModelID = Convert.ToInt32(row[2]);
                    accessory.varAdditionalInformation = row[16].ToString();
                    accessory.varProdID = row[18].ToString();
                    o = accessory as Object;
                }
                else if(itemTypeID == 3)
                {
                    Clothing clothing = new Clothing();
                    //if item type is clothing then save as clothing class
                    clothing.intInventoryID = inventoryID;
                    clothing.intBrandID = Convert.ToInt32(row[1]);
                    clothing.fltCost = Convert.ToDouble(row[7]);
                    clothing.fltPrice = Convert.ToDouble(row[8]);
                    clothing.intQuantity = Convert.ToInt32(row[9]);
                    clothing.intLocationID = Convert.ToInt32(row[15]);
                    clothing.varSize = row[5].ToString();
                    clothing.varColour = row[4].ToString();
                    clothing.varGender = row[10].ToString();
                    clothing.varStyle = row[3].ToString();
                    clothing.varAdditionalInformation = row[16].ToString();
                    clothing.varProdID = row[18].ToString();
                    o = clothing as Object;
                }
                IDU.UpdateItemInDatabase(o, objPageDetails);
            }
            else
            {
                if (itemTypeID == 1)
                {
                    Clubs club = new Clubs();
                    //Transfers all info into Club class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(itemTypeID, cu.location.intLocationID, objPageDetails);
                    club.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    club.varSku = sku;
                    club.fltCost = Convert.ToDouble(row[7]);
                    club.intBrandID = Convert.ToInt32(row[1]);                    
                    club.fltPrice = Convert.ToDouble(row[8]);
                    club.intQuantity = Convert.ToInt32(row[9]);
                    club.intLocationID = Convert.ToInt32(row[15]);
                    club.varTypeOfClub = row[3].ToString();
                    club.intModelID = Convert.ToInt32(row[2]);
                    club.varShaftType = row[4].ToString();
                    club.varNumberOfClubs = row[5].ToString();
                    club.varClubSpecification = row[10].ToString();
                    club.varShaftSpecification = row[11].ToString();
                    club.varShaftFlexability = row[12].ToString();
                    club.varClubDexterity = row[13].ToString();
                    club.bitIsUsedProduct = Convert.ToBoolean(row[17]);
                    club.varAdditionalInformation = row[16].ToString();
                    club.intItemTypeID = itemTypeID;
                    club.varProdID = row[18].ToString();
                    //stores club as an object
                    o = club as Object;
                }
                else if (itemTypeID == 2)
                {
                    Accessories accessory = new Accessories();
                    //Transfers all info into Accessory class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(itemTypeID, cu.location.intLocationID, objPageDetails);
                    accessory.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    accessory.varSku = sku;
                    accessory.intBrandID = Convert.ToInt32(row[1]);
                    accessory.intModelID = Convert.ToInt32(row[2]);
                    accessory.fltCost = Convert.ToDouble(row[7]);
                    accessory.fltPrice = Convert.ToDouble(row[8]);
                    accessory.intQuantity = Convert.ToInt32(row[9]);
                    accessory.intLocationID = Convert.ToInt32(row[15]);
                    accessory.intItemTypeID = itemTypeID;
                    accessory.varSize = row[5].ToString();
                    accessory.varColour = row[4].ToString();
                    accessory.varTypeOfAccessory = row[3].ToString();
                    accessory.varAdditionalInformation = row[16].ToString();
                    accessory.varProdID = row[18].ToString();
                    //stores accessory as an object
                    o = accessory as Object;
                }
                else if (itemTypeID == 3)
                {
                    Clothing clothing = new Clothing();
                    //Transfers all info into Clothing class
                    string[] inventoryInfo = IDU.CallReturnMaxSku(itemTypeID, cu.location.intLocationID, objPageDetails);
                    clothing.intInventoryID = Convert.ToInt32(inventoryInfo[1]);
                    clothing.varSku = sku;
                    clothing.intBrandID = Convert.ToInt32(row[1]);
                    clothing.fltCost = Convert.ToDouble(row[7]);
                    clothing.fltPrice = Convert.ToDouble(row[8]);
                    clothing.intQuantity = Convert.ToInt32(row[9]);
                    clothing.intLocationID = Convert.ToInt32(row[15]);
                    clothing.intItemTypeID = itemTypeID;
                    clothing.varSize = row[5].ToString();
                    clothing.varColour = row[4].ToString();
                    clothing.varGender = row[10].ToString();
                    clothing.varStyle = row[3].ToString();
                    clothing.varAdditionalInformation = row[16].ToString();
                    clothing.varProdID = row[18].ToString();
                    //stores clothing as an object
                    o = clothing as Object;
                }
                IDU.AddNewItemToDatabase(o, objPageDetails);
            }
        }
        public void CallItemExports(string selection, FileInfo newFile, string fileName, object[] objPageDetails)
        {
            ItemExports(selection, newFile, fileName, objPageDetails);
        }
        private void ItemExports(string selection, FileInfo newFile, string fileName, object[] objPageDetails)
        {
            //This is the table that has all of the information lined up the way Caspio needs it to be
            System.Data.DataTable exportTable = new System.Data.DataTable();
            if (selection.Equals("all"))
            {
                exportTable = ExportAllInventory(objPageDetails);
            }
            else if (selection.Equals("clubs"))
            {
                exportTable = ExportAllAdd_Clubs(objPageDetails);
            }
            else if (selection.Equals("accessories"))
            {
                exportTable = ExportAllAdd_Accessories(objPageDetails);
            }
            else if (selection.Equals("clothing"))
            {
                exportTable = ExportAllAdd_Clothing(objPageDetails);
            }
            string[] itemExportColumns = { "ProdID", "Vendor", "Store_ID", "ItemNumber", "Shipment_Date", "Brand", "Model", "Club_Type", "Shaft",
                    "Number_of_Clubs", "Tradein_Price", "Premium", "WE PAY", "QUANTITY", "Ext'd Price", "RetailPrice", "Comments",
                    "Image", "Club_Spec", "Shaft_Spec", "Shaft_Flex", "Dexterity", "Destination", "Received", "Paid", "ItemType", "isTradeIn"};
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Add page to the work book called inventory
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Inventory");
                worksheet.Cells[1, 1].Value = "Date Created: " + DateTime.Now.ToString("dd.M.yyyy");
                //Sets the column headers
                for (int i = 0; i < itemExportColumns.Count(); i++)
                {
                    worksheet.Cells[2, i + 1].Value = itemExportColumns[i].ToString();
                }
                DataColumnCollection dcCollection = exportTable.Columns;
                int recordIndex = 3;
                foreach (DataRow row in exportTable.Rows)
                {
                    worksheet.Cells[recordIndex, 1].Value = row[0].ToString();
                    worksheet.Cells[recordIndex, 2].Value = row[1].ToString();
                    worksheet.Cells[recordIndex, 3].Value = row[2].ToString();
                    worksheet.Cells[recordIndex, 4].Value = row[3].ToString();
                    worksheet.Cells[recordIndex, 5].Value = row[4].ToString();
                    worksheet.Cells[recordIndex, 6].Value = row[5].ToString();
                    worksheet.Cells[recordIndex, 7].Value = row[6].ToString();
                    worksheet.Cells[recordIndex, 8].Value = row[7].ToString();
                    worksheet.Cells[recordIndex, 9].Value = row[8].ToString();
                    worksheet.Cells[recordIndex, 10].Value = row[9].ToString();
                    worksheet.Cells[recordIndex, 11].Value = Convert.ToDouble(row[10].ToString());
                    worksheet.Cells[recordIndex, 12].Value = Convert.ToDouble(row[11].ToString());
                    worksheet.Cells[recordIndex, 13].Value = Convert.ToDouble(row[12].ToString());
                    worksheet.Cells[recordIndex, 14].Value = Convert.ToDouble(row[13].ToString());
                    worksheet.Cells[recordIndex, 15].Value = Convert.ToDouble(row[14].ToString());
                    worksheet.Cells[recordIndex, 16].Value = Convert.ToDouble(row[15].ToString());
                    worksheet.Cells[recordIndex, 17].Value = row[16].ToString();
                    worksheet.Cells[recordIndex, 18].Value = row[17].ToString();
                    worksheet.Cells[recordIndex, 19].Value = row[18].ToString();
                    worksheet.Cells[recordIndex, 20].Value = row[19].ToString();
                    worksheet.Cells[recordIndex, 21].Value = row[20].ToString();
                    worksheet.Cells[recordIndex, 22].Value = row[21].ToString();
                    worksheet.Cells[recordIndex, 23].Value = row[22].ToString();
                    worksheet.Cells[recordIndex, 24].Value = row[23].ToString();
                    worksheet.Cells[recordIndex, 25].Value = Convert.ToDouble(row[24].ToString());
                    worksheet.Cells[recordIndex, 26].Value = row[25].ToString();
                    worksheet.Cells[recordIndex, 27].Value = row[26].ToString();
                    recordIndex++;
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(xlPackage.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }
        private DataTable ExportAllInventory(object[] objPageDetails)
        {
            string strQueryName = "exportAllInventory";
            string sqlCmd = "";
            sqlCmd += ExportClubString();
            sqlCmd += " UNION ";
            sqlCmd += ExportAccessoryString();
            sqlCmd += " UNION ";
            sqlCmd += ExportClothingString();

            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ExportAllAdd_Clubs(object[] objPageDetails)
        {
            string strQueryName = "exportAllAdd_Clubs";
            string sqlCmd = ExportClubString();
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ExportAllAdd_Accessories(object[] objPageDetails)
        {
            string strQueryName = "exportAllAdd_Accessories";
            string sqlCmd = ExportAccessoryString();
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public DataTable ExportAllAdd_Clothing(object[] objPageDetails)
        {
            string strQueryName = "exportAllAdd_Clothing";
            string sqlCmd = ExportClothingString();
            object[][] parms = { };
            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private string ExportClubString()
        {
            string sqlCmd = "SELECT varProdID, '' AS Vendor, (SELECT varLocationName FROM tbl_location L WHERE L.intLocationID = C.intLocationID) AS varLocationName, "
                + "varSku, '' AS ShipmentDate, (SELECT varBrandName FROM tbl_brand B WHERE B.intBrandID = C.intBrandID ) AS varBrandName , (SELECT varModelName "
                + "FROM tbl_model M WHERE M.intModelID = C.intModelID) AS varModelName, varTypeOfClub, varShaftType, varNumberOfClubs, 0 AS TradeinPrice, "
                + "fltPremiumCharge, fltCost, intQuantity, 0 AS ExtendedPrice, fltPrice, varAdditionalInformation, '' AS Image, varClubSpecification, "
                + "varShaftSpecification, varShaftFlexability, varClubDexterity, (SELECT varSecondLocationID FROM tbl_location L WHERE L.intLocationID = "
                + "C.intLocationID) AS locationSecondary, '' AS Received, 0 AS Paid, (SELECT varItemTypeName FROM tbl_itemType IT WHERE IT.intItemTypeID = "
                + "C.intItemTypeID) AS itemType, bitIsUsedProduct, varProdID FROM tbl_clubs C";
            return sqlCmd;
        }
        private string ExportAccessoryString()
        {
            string sqlCmd = "SELECT varProdID, '' AS Vendor, (SELECT varLocationName FROM tbl_location L WHERE L.intLocationID = A.intLocationID) AS varLocationName, "
                + "varSku, '' AS ShipmentDate, (SELECT varBrandName FROM tbl_brand B WHERE B.intBrandID = A.intBrandID) AS varBrandName, (SELECT varModelName "
                + "FROM tbl_model M WHERE M.intModelID = A.intModelID) AS varModelName, varTypeOfAccessory AS varTypeOfClub, varColour AS varShaftType, "
                + "varSize AS varNumberOfClubs, 0 AS TradeinPrice, 0 AS fltPremiumCharge, fltCost, intQuantity, 0 AS ExtendedPrice, fltPrice, "
                + "varAdditionalInformation, '' AS Image, '' AS varClubSpecification, '' AS varShaftSpecification, '' AS varShaftFlexability, "
                + "'' as varClubDexterity, (SELECT varSecondLocationID FROM tbl_location L WHERE L.intLocationID = A.intLocationID) AS locationSecondary, "
                + "'' AS Received, 0 AS Paid, (SELECT varItemTypeName FROM tbl_itemType IT WHERE IT.intItemTypeID = A.intItemTypeID) AS itemType, "
                + "0 AS bitIsUsedProduct, varProdID FROM tbl_accessories A";
            return sqlCmd;
        }
        private string ExportClothingString()
        {
            string sqlCmd = "SELECT varProdID, '' AS Vendor, (SELECT varLocationName FROM tbl_location L WHERE L.intLocationID = CL.intLocationID) AS varLocationName, "
                + "varSku, '' AS ShipmentDate, (SELECT varBrandName FROM tbl_brand B WHERE B.intBrandID = CL.intBrandID) AS varBrandName, '' AS varModelName, "
                + "varStyle AS varTypeOfClub, varColour AS varShaftType, varSize AS varNumberOfClubs, 0 AS TradeinPrice, 0 AS fltPremiumCharge, fltCost, "
                + "intQuantity, 0 AS ExtendedPrice, fltPrice, varAdditionalInformation, '' AS Image, varGender AS varClubSpecification, '' AS "
                + "varShaftSpecification, '' AS varShaftFlexability, '' AS varClubDexterity, (SELECT varSecondLocationID FROM tbl_location L WHERE "
                + "L.intLocationID = CL.intLocationID) AS locationSecondary, '' AS Received, 0 AS Paid, (SELECT varItemTypeName FROM tbl_itemType IT WHERE "
                + "IT.intItemTypeID = CL.intItemTypeID) AS itemType, 0 AS bitIsUsedProduct, varProdID FROM tbl_clothing CL";
            return sqlCmd;
        }
        public void CallExportInvoiceDateRange(DateTime[] dtm, FileInfo newFile, string fileName)
        {
            ExportInvoiceDateRange(dtm, newFile, fileName);
        }
        private void ExportInvoiceDateRange(DateTime[] dtm, FileInfo newFile, string fileName)
        {
            object[][] parms =
            {
                new object[] { "@startDate", dtm[0] },
                new object[] { "@endDate", dtm[1] }
            };

            //Selects everything form the invoice table
            DataTable dtim = DBC.MakeDataBaseCallToReturnDataTableFromStoredProcedure("getInvoiceAll", parms);
            DataColumnCollection dcimHeaders = dtim.Columns;

            //Selects everything form the invoice item table
            DataTable dtii = DBC.MakeDataBaseCallToReturnDataTableFromStoredProcedure("getInvoiceItemAll", parms);
            DataColumnCollection dciiHeaders = dtii.Columns;

            //Selects everything form the invoice item tax table
            DataTable dtiit = DBC.MakeDataBaseCallToReturnDataTableFromStoredProcedure("getInvoiceItemTaxAll", parms);
            DataColumnCollection dciitHeaders = dtiit.Columns;

            //Selects everything form the invoice mop table
            DataTable dtimo = DBC.MakeDataBaseCallToReturnDataTableFromStoredProcedure("getInvoiceMOPAll", parms);
            DataColumnCollection dcimoHeaders = dtimo.Columns;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Creates a seperate sheet for each data table
                ExcelWorksheet invoiceMain = xlPackage.Workbook.Worksheets.Add("Invoice Main");
                ExcelWorksheet invoiceItems = xlPackage.Workbook.Worksheets.Add("Invoice Items");
                ExcelWorksheet invoiceItemsTax = xlPackage.Workbook.Worksheets.Add("Invoice Item Tax");
                ExcelWorksheet invoiceMOPS = xlPackage.Workbook.Worksheets.Add("Invoice MOPS");
                // write to sheet                  

                //Export main invoice
                for (int i = 1; i <= dtim.Rows.Count + 1; i++)
                {
                    for (int j = 1; j < dtim.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceMain.Cells[i, j].Value = dcimHeaders[j - 1].ToString();
                        }
                        else
                        {
                            invoiceMain.Cells[i, j].Value = dtim.Rows[i - 2][j - 1];
                        }
                    }
                }
                //Export item invoice
                for (int i = 1; i <= dtii.Rows.Count + 1; i++)
                {
                    for (int j = 1; j < dtii.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceItems.Cells[i, j].Value = dciiHeaders[j - 1].ToString();
                        }
                        else
                        {
                            invoiceItems.Cells[i, j].Value = dtii.Rows[i - 2][j - 1];
                        }
                    }
                }
                //Export item tax invoice
                for (int i = 1; i <= dtiit.Rows.Count + 1; i++)
                {
                    for (int j = 1; j < dtiit.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceItemsTax.Cells[i, j].Value = dciitHeaders[j - 1].ToString();
                        }
                        else
                        {
                            invoiceItemsTax.Cells[i, j].Value = dtiit.Rows[i - 2][j - 1];
                        }
                    }
                }
                //Export mop invoice
                for (int i = 1; i <= dtimo.Rows.Count + 1; i++)
                {
                    for (int j = 1; j < dtimo.Columns.Count + 1; j++)
                    {
                        if (i == 1)
                        {
                            invoiceMOPS.Cells[i, j].Value = dcimoHeaders[j - 1].ToString();
                        }
                        else
                        {
                            invoiceMOPS.Cells[i, j].Value = dtimo.Rows[i - 2][j - 1];
                        }
                    }
                }

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.BinaryWrite(xlPackage.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }
    }
}