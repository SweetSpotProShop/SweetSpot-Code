using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using SweetSpotDiscountGolfPOS;
using System.Data;
using System.Linq;
using System.Configuration;

namespace SweetSpotProShop
{
    //This class is used for way too much...
    public class ItemDataUtilities
    {
        private string connectionString;
        //LocationManager lm = new LocationManager();
        //Connection String
        public ItemDataUtilities()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        DatabaseCalls dbc = new DatabaseCalls();
        private List<Accessories> ConvertFromDataTableToAccessories(DataTable dt)
        {
            List<Accessories> accessories = dt.AsEnumerable().Select(row =>
            new Accessories
            {
                sku = row.Field<int>("sku"),
                size = row.Field<string>("size"),
                colour = row.Field<string>("colour"),
                price = row.Field<double>("price"),
                cost = row.Field<double>("cost"),
                brandID = row.Field<int>("brandID"),
                modelID = row.Field<int>("modelID"),
                accessoryType = row.Field<string>("accessoryType"),
                quantity = row.Field<int>("quantity"),
                typeID = row.Field<int>("typeID"),
                locID = row.Field<int>("locationID"),
                comments = row.Field<string>("comments")
            }).ToList();
            return accessories;
        }
        private List<Clothing> ConvertFromDataTableToClothing(DataTable dt)
        {
            List<Clothing> clothing = dt.AsEnumerable().Select(row =>
            new Clothing
            {
                sku = row.Field<int>("sku"),
                size = row.Field<string>("size"),
                colour = row.Field<string>("colour"),
                gender = row.Field<string>("gender"),
                style = row.Field<string>("style"),
                price = row.Field<double>("price"),
                cost = row.Field<double>("cost"),
                brandID = row.Field<int>("brandID"),
                quantity = row.Field<int>("quantity"),
                typeID = row.Field<int>("typeID"),
                locID = row.Field<int>("locationID"),
                comments = row.Field<string>("comments")
            }).ToList();
            return clothing;
        }
        private List<Clubs> ConvertFromDataTableToClubs(DataTable dt)
        {
            List<Clubs> clubs = dt.AsEnumerable().Select(row =>
            new Clubs
            {
                sku = row.Field<int>("sku"),
                brandID = row.Field<int>("brandID"),
                modelID = row.Field<int>("modelID"),
                clubType = row.Field<string>("clubType"),
                shaft = row.Field<string>("shaft"),
                numberOfClubs = row.Field<string>("numberOfClubs"),
                premium = row.Field<double>("premium"),
                cost = row.Field<double>("cost"),
                price = row.Field<double>("price"),
                quantity = row.Field<int>("quantity"),
                clubSpec = row.Field<string>("clubSpec"),
                shaftSpec = row.Field<string>("shaftSpec"),
                shaftFlex = row.Field<string>("shaftFlex"),
                dexterity = row.Field<string>("dexterity"),
                typeID = row.Field<int>("typeID"),
                itemlocation = row.Field<int>("locationID"),
                used = row.Field<bool>("used"),
                comments = row.Field<string>("comments")
            }).ToList();
            return clubs;
        }


        public List<Object> ReturnListOfObjectsFromThreeTables(int sku)
        {
            string sqlCmd = "SELECT sku, brandID, modelID, clubType, shaft, numberOfClubs, "
                + "premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, "
                + "typeID, locationID, used, comments FROM tbl_clubs WHERE sku = @sku";

            Object[][] parms =
            {
                 new object[] { "@sku", sku }
            };

            List<Clubs> c = ConvertFromDataTableToClubs(dbc.returnDataTableData(sqlCmd, parms));

            sqlCmd = "SELECT sku, size, colour, gender, style, price, cost, brandID, quantity, "
                + "typeID, locationID, comments FROM tbl_clothing WHERE sku = @sku";

            List<Clothing> cl = ConvertFromDataTableToClothing(dbc.returnDataTableData(sqlCmd, parms));

            sqlCmd = "SELECT sku, size, colour, price, cost, brandID, modelID, accessoryType, "
                + "quantity, typeID, locationID, comments FROM tbl_accessories WHERE sku = @sku";

            List<Accessories> a = ConvertFromDataTableToAccessories(dbc.returnDataTableData(sqlCmd, parms));

            List<Object> o = new List<Object>();
            o.AddRange(a);
            o.AddRange(cl);
            o.AddRange(c);
            return o;
        }
        //Return Model string created by Nathan and Tyler **getModelName
        public string ReturnModelNameFromModelID(int modelID)
        {
            string sqlCmd = "Select modelName from tbl_model where modelID = @modelID";

            Object[][] parms =
            {
                 new object[] { "@modelID", modelID }
            };
            //Returns the model name
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        //Return Brand string created by Nathan and Tyler **getBrandName
        public string ReturnBrandNameFromBrandID(int brandID)
        {
            string sqlCmd = "SELECT brandName FROM tbl_brand WHERE brandID = @brandID";

            Object[][] parms =
            {
                 new object[] { "@brandID", brandID }
            };
            //Returns the brand name
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms);
        }
        //Returns max sku from the skuNumber table based on itemType and directs code to store it
        public int ReturnMaxSku(int itemType)
        {
            string sqlCmd = "SELECT sku FROM tbl_skuNumbers WHERE itemType = @itemType";
            Object[][] parms =
            {
                 new object[] { "@itemType", itemType }
            };

            int maxSku = dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) + 1;
            StoreMaxSku(maxSku, itemType);
            //Returns the new max sku
            return maxSku;
        }
        //Stores the max sku in the skuNumber table
        public void StoreMaxSku(int sku, int itemType)
        {
            //This method stores the max sku along with its item type
            string sqlCmd = "UPDATE tbl_skuNumbers SET sku = @sku WHERE itemType = @itemType";
            Object[][] parms =
            {
                 new object[] { "@sku", sku },
                 new object[] { "@itemType", itemType }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //**Add Item**
        //Adds new Item to tables Nathan created
        public int AddNewItemToDatabase(Object o)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            int sku = -10;
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                AddClubToDatabase(c);
                sku = c.sku;
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                AddAccessoryToDatabase(a);
                sku = a.sku;
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                AddClothingToDatabase(cl);
                sku = cl.sku;
            }
            //Returns the sku of the new item
            return sku;
        }
        //These three actully add the item to specific tables Nathan created
        private void AddClubToDatabase(Clubs c)
        {
            
            string sqlCmd = "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs,"
                    + " premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, used, comments)"
                    + " Values (@sku, @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price,"
                    + " @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments)";

            Object[][] parms =
            {
                 new object[] { "@sku", c.sku },
                 new object[] { "@brandID", c.brandID },
                 new object[] { "@modelID", c.modelID },
                 new object[] { "@clubType", c.clubType },
                 new object[] { "@shaft", c.shaft },
                 new object[] { "@numberOfClubs", c.numberOfClubs },
                 new object[] { "@premium", c.premium },
                 new object[] { "@cost", c.cost },
                 new object[] { "@price", c.price },
                 new object[] { "@quantity", c.quantity },
                 new object[] { "@clubSpec", c.clubSpec },
                 new object[] { "@shaftSpec", c.shaftSpec },
                 new object[] { "@shaftFlex", c.shaftFlex },
                 new object[] { "@dexterity", c.dexterity },
                 new object[] { "@typeID", c.typeID },
                 new object[] { "@locationID", c.itemlocation },
                 new object[] { "@used", c.used },
                 new object[] { "@comments", c.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private void AddAccessoryToDatabase(Accessories a)
        {
            string sqlCmd = "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, modelID, accessoryType, quantity, typeID, locationID, comments)"
                + " Values (@sku, @size, @colour, @price, @cost, @brandID, @modelID, @accessoryType, @quantity, @typeID, @locationID, @comments)";

            Object[][] parms =
            {
                 new object[] { "@sku", a.sku },
                 new object[] { "@size", a.size },
                 new object[] { "@colour", a.colour },
                 new object[] { "@price", a.price },
                 new object[] { "@cost", a.cost },
                 new object[] { "@brandID", a.brandID },
                 new object[] { "@modelID", a.modelID },
                 new object[] { "@accessoryType", a.accessoryType },
                 new object[] { "@quantity", a.quantity },
                 new object[] { "@typeID", a.typeID },
                 new object[] { "@locationID", a.locID },
                 new object[] { "@comments", a.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private void AddClothingToDatabase(Clothing cl)
        {
            string sqlCmd = "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID, comments)"
                + " Values (@sku, @size, @colour, @gender, @style, @price, @cost, @brandID, @quantity, @typeID, @locationID, @comments)";

            Object[][] parms =
            {
                 new object[] { "@sku", cl.sku },
                 new object[] { "@size", cl.size },
                 new object[] { "@colour", cl.colour },
                 new object[] { "@gender", cl.gender },
                 new object[] { "@style", cl.style },
                 new object[] { "@price", cl.price },
                 new object[] { "@cost", cl.cost },
                 new object[] { "@brandID", cl.brandID },
                 new object[] { "@quantity", cl.quantity },
                 new object[] { "@typeID", cl.typeID },
                 new object[] { "@locationID", cl.locID },
                 new object[] { "@comments", cl.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }

        //**Update Item**
        public int UpdateItemInDatabase(Object o)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            int sku = -10;
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                UpdateClubInDatabase(c);
                sku = c.sku;
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                UpdateAccessoryInDatabase(a);
                sku = a.sku;
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                UpdateClothingInDatabase(cl);
                sku = cl.sku;
            }
            //Returns the sku of the new item
            return sku;
        }
        //These three actully update the item in their specific tables Nathan created
        private void UpdateClubInDatabase(Clubs c)
        {
            string sqlCmd = "UPDATE tbl_clubs SET brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft,"
                + " numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity,"
                + " clubSpec = @clubSpec, shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity,"
                + " locationID = @locationID, used = @used, comments = @comments WHERE sku = @sku";

            Object[][] parms =
            {
                 new object[] { "@sku", c.sku },
                 new object[] { "@brandID", c.brandID },
                 new object[] { "@modelID", c.modelID },
                 new object[] { "@clubType", c.clubType },
                 new object[] { "@shaft", c.shaft },
                 new object[] { "@numberOfClubs", c.numberOfClubs },
                 new object[] { "@premium", c.premium },
                 new object[] { "@cost", c.cost },
                 new object[] { "@price", c.price },
                 new object[] { "@quantity", c.quantity },
                 new object[] { "@clubSpec", c.clubSpec },
                 new object[] { "@shaftSpec", c.shaftSpec },
                 new object[] { "@shaftFlex", c.shaftFlex },
                 new object[] { "@dexterity", c.dexterity },
                 new object[] { "@locationID", c.itemlocation },
                 new object[] { "@used", c.used },
                 new object[] { "@comments", c.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private void UpdateAccessoryInDatabase(Accessories a)
        {
            string sqlCmd = "UPDATE tbl_accessories SET size = @size, colour = @colour, price = @price, cost = @cost, brandID = @brandID,"
                + "modelID = @modelID, accessoryType = @accessoryType, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku";

            Object[][] parms =
            {
                 new object[] { "@sku", a.sku },
                 new object[] { "@size", a.size },
                 new object[] { "@colour", a.colour },
                 new object[] { "@price", a.price },
                 new object[] { "@cost", a.cost },
                 new object[] { "@brandID", a.brandID },
                 new object[] { "@modelID", a.modelID },
                 new object[] { "@accessoryType", a.accessoryType },
                 new object[] { "@quantity", a.quantity },
                 new object[] { "@locationID", a.locID },
                 new object[] { "@comments", a.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }
        private void UpdateClothingInDatabase(Clothing cl)
        {
            string sqlCmd = "UPDATE tbl_clothing SET size = @size, colour = @colour, gender = @gender, style = @style,"
                + " price = @price, cost = @cost, brandID = @brandID, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku";

            Object[][] parms =
            {
                 new object[] { "@sku", cl.sku },
                 new object[] { "@size", cl.size },
                 new object[] { "@colour", cl.colour },
                 new object[] { "@gender", cl.gender },
                 new object[] { "@style", cl.style },
                 new object[] { "@price", cl.price },
                 new object[] { "@cost", cl.cost },
                 new object[] { "@brandID", cl.brandID },
                 new object[] { "@quantity", cl.quantity },
                 new object[] { "@locationID", cl.locID },
                 new object[] { "@comments", cl.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms);
        }





        //**OLD CODE**

        //Return Model Int created by Nathan and Tyler **getModelID
        public int modelName(string modelN)
        {
            int model = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select modelID from tbl_model where modelName = @modelName";
            cmd.Parameters.AddWithValue("modelName", modelN);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int m = Convert.ToInt32(reader["modelID"]);
                model = m;
            }
            conn.Close();

            if (model == 0)
            {
                model = insertModel(modelN);
            }
            //Returns the modelID 
            return model;
        }
        //Return Brand Int created by Nathan and Tyler **getBrandID
        public int brandName(string brandN)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select brandID from tbl_brand where brandName = '" + brandN + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int brand = 0;

            while (reader.Read())
            {
                int b = Convert.ToInt32(reader["brandID"]);
                brand = b;
            }
            conn.Close();


            if (brand == 0)
            {
                brand = insertBrand(brandN);
            }
            //Returns the brandID
            return brand;
        }
        //Return Item Type string created by Nathan and Tyler **getItemTypeDescritpion
        public string typeName(int typeNum)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select typeDescription from tbl_itemType where typeID = " + typeNum;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string type = null;

            while (reader.Read())
            {
                string t = reader["typeDescription"].ToString();
                type = t;
            }
            conn.Close();
            //Returns the item type description
            return type;
        }
        //Insert new brand name. Returns new brandID
        public int insertBrand(string brandName)
        {
            int brandID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO tbl_brand (brandName) OUTPUT Inserted.brandID VALUES(@brandName); ";
            cmd.Parameters.AddWithValue("brandName", brandName);
            conn.Open();
            brandID = (int)cmd.ExecuteScalar();
            conn.Close();
            //Returns the brandID of the newly added brand
            return brandID;
        }
        //Insert new model name. return new modelID
        public int insertModel(string modelName)
        {
            int modelID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO tbl_model (modelName) OUTPUT Inserted.modelID VALUES(@modelName); ";
            cmd.Parameters.AddWithValue("modelName", modelName);
            conn.Open();
            modelID = (int)cmd.ExecuteScalar();
            conn.Close();
            //Returns the modelID of the newly added model
            return modelID;
        }
        //***NOT USED
        //Return Vendor ID
        public int getVendorID(string vendorName)
        {
            int vendorID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select vendorID from tbl_vendor where vendorName = '" + vendorName + "'";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int vID = Convert.ToInt32(reader["vendorID"]);
                vendorID = vID;
            }
            conn.Close();
            return vendorID;
        }
        //Return Vendor Name
        public string getVendorName(int vendorID)
        {
            string vendorName = null;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select vendorName from tbl_vendor where vendorID = " + vendorID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string vN = reader["vendorName"].ToString();
                vendorName = vN;
            }
            conn.Close();

            return vendorName;
        }
        //NOT USED***
        //Return Club Type ID
        public int getClubTypeID(string typeName)
        {
            int typeID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select typeID from tbl_clubType where typeName = '" + typeName + "'";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int tID = Convert.ToInt32(reader["typeID"]);
                typeID = tID;
            }
            conn.Close();
            //Returns the club type ID
            return typeID;
        }
        //Return Club Type Name
        public string getClubTypeName(int typeID)
        {
            string typeName = null;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select typeName from tbl_clubType where typeID = " + typeID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string tN = reader["typeName"].ToString();
                typeName = tN;
            }
            conn.Close();
            //Returns the name of the club type
            return typeName;
        }
        //Adding items to the Cart class. Totally not stolen by Tickles
        public Cart addingToCart(Object o)
        {
            Cart ca = new Cart();
            //Checks if the item is a club
            if (o is Clubs)
            {
                //Converts the object to type club
                Clubs c = o as Clubs;
                ca.sku = c.sku;
                //Creates the description of the item for the cart
                ca.description = ReturnBrandNameFromBrandID(c.brandID) + " " + ReturnModelNameFromModelID(c.modelID) + " " +
                    c.clubSpec + " " + c.clubType + " " + c.shaftSpec + " " + c.shaftFlex + " " + c.dexterity;
                ca.price = c.price;
                ca.cost = c.cost;
                ca.returnAmount = 0;
                ca.typeID = c.typeID;
            }
            //Checks if the item is an accessory
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                ca.sku = a.sku;
                ca.description = ReturnBrandNameFromBrandID(a.brandID) + " " + ReturnModelNameFromModelID(a.modelID) + " " +
                    a.accessoryType + " " + a.size + " " + a.colour + " " + a.comments;
                ca.price = a.price;
                ca.cost = a.cost;
                ca.returnAmount = 0;
                ca.typeID = a.typeID;
            }
            //Checks if the item is clothing
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                ca.sku = cl.sku;
                ca.description = ReturnBrandNameFromBrandID(cl.brandID) + " " + cl.size + " " + cl.colour + " " +
                    cl.gender + " " + cl.style + " " + cl.comments;
                ca.price = cl.price;
                ca.cost = cl.cost;
                ca.returnAmount = 0;
                ca.typeID = cl.typeID;
            }
            ca.quantity = 1;
            //Returns the item in the form of a cart
            return ca;
        }
        //Populating gridView on Inventory Search button in Sales Cart with location **NOT USED
        public List<Cart> getItemByID(Int32 ItemNumber, string loc)
        {
            //Loops through the database and adds items with the matching sku to a list of type item
            List<Cart> items = new List<Cart>();
            Cart i = new Cart();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //int intLocation = lm.locationIDfromCity(loc);
            cmd.Connection = conn;
            conn.Open();
            //Removed location because client did not want
            cmd.CommandText = "Select sku, quantity, brandID, modelID, accessoryType, size, colour, comments, price, cost, typeID From tbl_accessories Where SKU = @skuAcc";
            cmd.Parameters.AddWithValue("skuAcc", ItemNumber);
            //cmd.Parameters.AddWithValue("locationID", intLocation);

            SqlDataReader readerAcc = cmd.ExecuteReader();
            while (readerAcc.Read())
            {

                //i = new Cart(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"])) + " "
                //    + modelType(Convert.ToInt32(readerAcc["modelID"])) + " " + readerAcc["accessoryType"].ToString() 
                //    + " " + readerAcc["size"].ToString() + " " + readerAcc["colour"].ToString() + " " + readerAcc["comments"].ToString(),
                //    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                //    Convert.ToDouble(readerAcc["cost"]),0,false,0,false,Convert.ToInt32(readerAcc["typeID"]));

            }
            if (!readerAcc.HasRows)
            {
                readerAcc.Close();
                cmd.CommandText = "Select sku, brandID, modelID, clubType, clubSpec, shaftSpec, shaftFlex, "
                    + "dexterity, quantity, price, cost, typeID From tbl_clubs Where SKU = @skuClubs";
                cmd.Parameters.AddWithValue("skuClubs", ItemNumber);
                //cmd.Parameters.AddWithValue("locationIDclubs", intLocation);
                SqlDataReader readerClubs = cmd.ExecuteReader();
                while (readerClubs.Read())
                {
                    //i = new Cart(Convert.ToInt32(readerClubs["sku"]),
                    //    brandType(Convert.ToInt32(readerClubs["brandID"]))
                    //    + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubSpec"].ToString()
                    //    + " " + readerClubs["clubType"].ToString() + " " + readerClubs["shaftSpec"].ToString() + " "
                    //    + readerClubs["shaftFlex"].ToString() + " " + readerClubs["dexterity"].ToString(),

                    //    Convert.ToInt32(readerClubs["quantity"]), Convert.ToDouble(readerClubs["price"]),
                    //    Convert.ToDouble(readerClubs["cost"]),0,false,0,false,Convert.ToInt32(readerClubs["typeID"]));

                }
                if (!readerClubs.HasRows)
                {
                    readerClubs.Close();
                    cmd.CommandText = "Select sku, brandID, size, colour, gender, style, comments, quantity, price, cost, typeID From tbl_clothing Where SKU = @skuClothing";
                    cmd.Parameters.AddWithValue("skuClothing", ItemNumber);
                    //cmd.Parameters.AddWithValue("locationIDclothing", intLocation);
                    SqlDataReader readerClothing = cmd.ExecuteReader();
                    while (readerClothing.Read())
                    {
                        //i = new Cart(Convert.ToInt32(readerClothing["sku"]),
                        //Start of description
                        //    brandType(Convert.ToInt32(readerClothing["brandID"]))
                        //    + " " + readerClothing["size"].ToString() + " " + readerClothing["colour"].ToString()
                        //    + " " + readerClothing["gender"].ToString() + " " + readerClothing["style"].ToString()
                        //    + " " + readerClothing["comments"].ToString(), //End of description
                        //    Convert.ToInt32(readerClothing["quantity"]), Convert.ToDouble(readerClothing["price"]),
                        //    Convert.ToDouble(readerClothing["cost"]), 0, false, 0, false, Convert.ToInt32(readerClothing["typeID"]));
                    }
                }
            }
            if (i.sku > 0)
            {
                items.Add(i);
            }
            conn.Close();
            return items;
        }
        //Populating gridView on Inventory Search button for all locations
        public List<Items> getItemByID(Int32 ItemNumber)
        {
            //Loops through the database searching for items that match sku's with the search
            //Adds the items that are found to a list of type item
            List<Items> items = new List<Items>();
            Items i = new Items();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandText = "Select sku, quantity, brandID, modelID, accessoryType, size, colour, comments, price, cost, typeID, locationID From tbl_accessories Where SKU = @skuAcc";
            cmd.Parameters.AddWithValue("skuAcc", ItemNumber);

            SqlDataReader readerAcc = cmd.ExecuteReader();
            //Starts the search by looking in the accessories
            while (readerAcc.Read())
            {
                //If an item is found, creating a new "item" with the accessory information
                //+ " " + readerAcc["accessoryType"].ToString()
                //i = new Items(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"])) + " "
                //    + modelType(Convert.ToInt32(readerAcc["modelID"])) + " " + readerAcc["size"].ToString() + " "
                //    + readerAcc["colour"].ToString() + " " + readerAcc["comments"].ToString(),
                //    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                //    Convert.ToDouble(readerAcc["cost"]), Convert.ToInt32(readerAcc["typeID"]),
                //    lm.locationName(Convert.ToInt32(readerAcc["locationID"])));

            }
            //If the search provides no results, we move into the next item type category - Clubs
            if (!readerAcc.HasRows)
            {
                readerAcc.Close();
                cmd.CommandText = "Select sku, brandID, modelID, clubType, shaft, numberOfClubs, "
                    + "clubSpec, shaftSpec, shaftFlex, dexterity, quantity, price, cost, typeID, locationID From tbl_clubs Where SKU = @skuClubs";
                cmd.Parameters.AddWithValue("skuClubs", ItemNumber);
                SqlDataReader readerClubs = cmd.ExecuteReader();
                while (readerClubs.Read())
                {
                    //If an item is found, creating a new "item" with the club information
                    //i = new Items(Convert.ToInt32(readerClubs["sku"]), brandType(Convert.ToInt32(readerClubs["brandID"]))
                    //    + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubType"].ToString()
                    //    + " " + readerClubs["clubSpec"].ToString() + " " + readerClubs["clubType"].ToString() + " "
                    //    + readerClubs["shaftSpec"].ToString() + " " + readerClubs["shaftFlex"].ToString() + " "
                    //    + readerClubs["dexterity"].ToString(), Convert.ToInt32(readerClubs["quantity"]),
                    //    Convert.ToDouble(readerClubs["price"]), Convert.ToDouble(readerClubs["cost"]),
                    //    Convert.ToInt32(readerClubs["typeID"]), lm.locationName(Convert.ToInt32(readerClubs["locationID"])));

                }
                //If the search once again provides no results, we search the clothing table for matches
                if (!readerClubs.HasRows)
                {
                    readerClubs.Close();
                    cmd.CommandText = "Select sku, brandID, size, colour, gender, style, comments, quantity, price, cost, typeID, locationID From tbl_clothing Where SKU = @skuClothing";
                    cmd.Parameters.AddWithValue("skuClothing", ItemNumber);
                    SqlDataReader readerClothing = cmd.ExecuteReader();
                    while (readerClothing.Read())
                    {
                        //If an item is found, creating a new "item" with the clothing information
                        //i = new Items(Convert.ToInt32(readerClothing["sku"]), brandType(Convert.ToInt32(readerClothing["brandID"]))
                        //    + " " + readerClothing["size"].ToString() + " " + readerClothing["colour"].ToString()
                        //    + " " + readerClothing["gender"].ToString() + " " + readerClothing["style"].ToString()
                        //    + " " + readerClothing["comments"].ToString(),
                        //    Convert.ToInt32(readerClothing["quantity"]), Convert.ToDouble(readerClothing["price"]),
                        //    Convert.ToDouble(readerClothing["cost"]), Convert.ToInt32(readerClothing["typeID"]), 
                        //    lm.locationName(Convert.ToInt32(readerClothing["locationID"])));
                    }
                }
            }
            //If the sku is greater than 0, add the item to the list
            if (i.sku > 0)
            {
                //Adding the item to the list
                items.Add(i);
            }
            conn.Close();
            //Returns a list of any items that are found
            return items;
        }
        //Being used now to return qty to validate if sku can be added to cart.
        public int getquantity(int sku, int typeID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select quantity from tbl_clubs where SKU = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int itemQTY = 0;

            //Loops through the three item tables looking for the item so it can return its quantity
            //Starting the search with clubs
            while (reader.Read())
            {
                //Setting the itemQTY to the found quantity
                itemQTY = Convert.ToInt32(reader["quantity"]);
            }
            //If the item can't be found in the clubs table, we search the accessory table
            if (!reader.HasRows)
            {
                reader.Close();
                cmd.CommandText = "Select quantity from tbl_accessories where SKU = @skuacces";
                cmd.Parameters.AddWithValue("skuacces", sku);
                SqlDataReader readerAccesories = cmd.ExecuteReader();

                while (readerAccesories.Read())
                {
                    //Setting the itemQTY to the found quantity
                    itemQTY = Convert.ToInt32(readerAccesories["quantity"]);
                }
                //If the item is not in the accessory table, we search the third table - clothing
                if (!readerAccesories.HasRows)
                {
                    readerAccesories.Close();
                    cmd.CommandText = "Select quantity from tbl_clothing where SKU = @skucloth";
                    cmd.Parameters.AddWithValue("skucloth", sku);
                    SqlDataReader readerclothing = cmd.ExecuteReader();

                    while (readerclothing.Read())
                    {
                        //Setting the itemQTY to the found quantity
                        itemQTY = Convert.ToInt32(readerclothing["quantity"]);
                    }
                }
            }
            conn.Close();
            //Returns the quantity of the searched item
            return itemQTY;
        }
        //This method updates an item with its new quantity
        public void removeQTYfromInventoryWithSKU(int sku, int typeID, int remainingQTY)
        {
            //Works by recieving a sku, a typeID, and the new quantity
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //Determines which table to look in by using the typeID and returning the type name(clubs, accessories, clothing)
            string table = typeName(typeID);
            cmd.CommandText = "UPDATE tbl_" + table + " SET quantity = @quantity WHERE sku = @sku and typeID = @typeID";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@typeID", typeID);
            cmd.Parameters.AddWithValue("@quantity", remainingQTY);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //This method updates an item with its new quantity
        public void updateQuantity(int sku, int typeID, int quantity)
        {
            //Works by recieving a sku, a typeID, and the new quantity
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //Determines which table to look in by using the typeID and returning the type name(clubs, accessories, clothing)
            string table = typeName(typeID);
            cmd.CommandText = "UPDATE tbl_" + table + " SET quantity = @quantity WHERE sku = @sku and typeID = @typeID";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@typeID", typeID);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Reserve trade-in sku
        public int reserveTradeInSKu(int loc)
        {
            int tradeInSkuDisplay = 0;
            //Grabs the trade in sku
            tradeInSkuDisplay = tradeInSku(loc) + 1;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "update tbl_tradeInSkusForCart set currentSKU = @sku where locationID = @locationID";
            cmd1.Parameters.AddWithValue("sku", tradeInSkuDisplay);
            cmd1.Parameters.AddWithValue("locationID", loc);
            conn.Open();
            cmd1.ExecuteNonQuery();
            conn.Close();
            //Returns the trade in items display sku
            return tradeInSkuDisplay;
        }
        //Grabbing trade-in sku
        public int tradeInSku(int location)
        {
            int sku = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select currentSKU from tbl_tradeInSkusForCart where locationID = @locationID";
            cmd.Parameters.AddWithValue("locationID", location);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Gets the max sku
                sku = Convert.ToInt32(reader["currentSKU"]);
            }
            conn.Close();

            //Returns the sku that will be used
            return sku;
        }
        //Finds and returns an array containing the upper and lower range for the trade in skus
        public int[] tradeInSkuRange(int location)
        {
            int[] range = new int[2];
            int upper = 0;
            int lower = 0;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select skuStartAt, skuStopAt from tbl_tradeInSkusForCart where locationID = " + location.ToString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                upper = Convert.ToInt32(reader["skuStopAt"].ToString());
                lower = Convert.ToInt32(reader["skuStartAt"].ToString());
            }
            //Setting the values in the array
            range[0] = lower;
            range[1] = upper;


            conn.Close();
            //Returns the range
            return range;
        }
        //Adding tradein item 
        public void addTradeInItem(Clubs tradeInItem, int sku, int loc)
        {
            //This method addes the trade in item to the tempTradeInCartSKus table
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            int used = 0;
            if (tradeInItem.used == true)
            { used = 1; }
            else { used = 0; }
            if (tradeInItem.itemlocation == 0)
            { tradeInItem.itemlocation = 1; }
            cmd.Connection = conn;
            cmd.CommandText = "insert into tbl_tempTradeInCartSkus values(@sku, @brandID, @modelID, "
                + "@clubType, @shaft, @numberOfClubs, @premium, @cost, @price, @quantity, @clubSpec, "
                + "@shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments)";
            //cmd.CommandText = "Update tbl_tempTradeInCartSkus set brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft," +
            //    "numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity, clubSpec = @clubSpec," +
            //    "shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity, typeID = @typeID, locationID = @locationID, used = @used," +
            //    "comments = @comments where sku = @sku;";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Parameters.AddWithValue("brandID", tradeInItem.brandID);
            cmd.Parameters.AddWithValue("modelID", tradeInItem.modelID);
            cmd.Parameters.AddWithValue("clubType", tradeInItem.clubType);
            cmd.Parameters.AddWithValue("shaft", tradeInItem.shaft);
            cmd.Parameters.AddWithValue("numberOfClubs", tradeInItem.numberOfClubs);
            cmd.Parameters.AddWithValue("premium", tradeInItem.premium);
            cmd.Parameters.AddWithValue("cost", tradeInItem.cost);
            cmd.Parameters.AddWithValue("price", tradeInItem.price);
            cmd.Parameters.AddWithValue("quantity", tradeInItem.quantity);
            cmd.Parameters.AddWithValue("clubSpec", tradeInItem.clubSpec);
            cmd.Parameters.AddWithValue("shaftSpec", tradeInItem.shaftSpec);
            cmd.Parameters.AddWithValue("shaftFlex", tradeInItem.shaftFlex);
            cmd.Parameters.AddWithValue("dexterity", tradeInItem.dexterity);
            cmd.Parameters.AddWithValue("typeID", tradeInItem.typeID);
            cmd.Parameters.AddWithValue("locationID", loc);
            cmd.Parameters.AddWithValue("used", tradeInItem.used);
            cmd.Parameters.AddWithValue("comments", tradeInItem.comments);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        //Sending all of the invoice information to the database 
        public void mainInvoice(CheckoutManager ckm, List<Cart> cart, List<Mops> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments, CurrentUser cu)
        {

            string locationInitial = invoiceNumber.Split('-')[0];

            ////Step 1: Find next invoice number 
            //int nextInvoiceNum = getNextInvoiceNum();
            int nextInvoiceNum = Convert.ToInt32(invoiceNumber.Split('-')[1]);

            ////Step 2: Find the invoice sub number
            int nextInvoiceSubNum = 0;
            //If the transaction is a sale
            if (transactionType == 1)
            {
                nextInvoiceSubNum = Convert.ToInt32(invoiceNumber.Split('-')[2]);
            }
            //If the transaction is a return
            else if (transactionType == 2)
            {
                //Gets the next sub num
                nextInvoiceSubNum = getNextInvoiceSubNum(nextInvoiceNum);
            }

            //Step 3: Get date and time
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");

            //Step 4: Insert all relevent info into the mainInvoice table
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //+ ", '" + date + "', '" + time + "', "
            cmd.CommandText = "Insert Into tbl_invoice (invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, "
                + "subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue, "
                + "transactionType, comments) values(@invoiceNum, @invoiceSubNum, @invoiceDate, @invoiceTime, @custID, "
                + "@empID, @locationID, @subtotal, @shippingAmount, @discountAmount, @tradeinAmount, @governmentTax, "
                + "@provincialTax, @balanceDue, @transactionType, @comments);";

            //"update tbl_invoice set " +
            //    "invoiceSubNum = @invoiceSubNum, " +
            //    "custID = @custID, " +
            //    "empID = @empID, " +
            //    "locationID = @locationID, " +
            //    "subTotal = @subtotal, " +
            //    "shippingAmount = @shippingAmount, " +
            //    "discountAmount = @discountAmount, " +
            //    "tradeinAmount = @tradeinAmount, " +
            //    "governmentTax = @governmentTax, " +
            //    "provincialTax = @provincialTax, " +
            //    "balanceDue = @balanceDue, " +
            //    "transactionType = @transactionType, " +
            //    "comments = @comments" +
            //    " where invoiceNum = @invoiceNum;";

            cmd.Parameters.AddWithValue("invoiceNum", nextInvoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", nextInvoiceSubNum);
            cmd.Parameters.AddWithValue("invoiceDate", date);
            cmd.Parameters.AddWithValue("invoiceTime", time);
            cmd.Parameters.AddWithValue("custID", c.customerId);
            cmd.Parameters.AddWithValue("empID", e.employeeID);
            cmd.Parameters.AddWithValue("locationID", cu.locationID);
            cmd.Parameters.AddWithValue("subTotal", ckm.dblSubTotal);
            cmd.Parameters.AddWithValue("shippingAmount", ckm.dblShipping);
            cmd.Parameters.AddWithValue("discountAmount", ckm.dblDiscounts);
            cmd.Parameters.AddWithValue("tradeinAmount", ckm.dblTradeIn);
            double gTax = 0;
            //If the GST is included, set it's value to the checkoutmanager GST value otherwise it stays at 0
            if (ckm.blGst) { gTax = ckm.dblGst; }
            cmd.Parameters.AddWithValue("governmentTax", gTax);
            double pTax = 0;
            //If the GST is included, set it's value to the checkoutmanager GST value otherwise it stays at 0
            if (ckm.blPst) { pTax = ckm.dblPst; }
            cmd.Parameters.AddWithValue("provincialTax", pTax);
            cmd.Parameters.AddWithValue("balanceDue", ckm.dblBalanceDue);
            cmd.Parameters.AddWithValue("transactionType", transactionType);
            cmd.Parameters.AddWithValue("comments", comments);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    nextInvoiceNum = Convert.ToInt32(reader["invoiceNum"]) + 1;
            //}
            conn.Close();
            //Step 5: Insert each item into the invoiceItem table
            //invoiceNum, invoiceSubNum, sku, itemQuantity, itemCost, itemPrice, itemDiscount, Percentage
            string tbl = "";
            //If it is a sale, use tbl_invoiceItem
            if (transactionType == 1)
            {
                tbl = "tbl_invoiceItem";
            }
            //If it is a return, use tbl_invoiceItemReturns
            else if (transactionType == 2)
            {
                tbl = "tbl_invoiceItemReturns";
            }
            //Loops through the cart to look at the items
            foreach (Cart item in cart)
            {
                int percentage = 0;
                if (item.percentage)
                {
                    percentage = 1;
                }
                else
                {
                    percentage = 0;
                }
                string insert = "insert into " + tbl + " values(" + nextInvoiceNum + ", " + nextInvoiceSubNum + ", " + item.sku + ", " + item.quantity + ", " +
                    item.cost + ", " + item.price + ", " + item.discount + ", " + item.returnAmount + ", " + percentage + ");";
                //Inserts the item
                invoiceItem(insert);
            }
            //Step 6: Insert each MOP into the invoiceMOP table
            //ID(autoincrementing), invoiceNum, invoiceSubNum, mopID, amountPaid
            //Loops through the checkout to get the mops
            foreach (Mops mop in mops)
            {
                string insert = "insert into tbl_invoiceMOP values(" + nextInvoiceNum + ", " + nextInvoiceSubNum + ", '" + mop.methodOfPayment + "', " + mop.amountPaid + ");";
                //Inserts the mop
                invoiceMOP(insert);
            }
        }
        public void mainPurchaseInvoice(CheckoutManager ckm, List<Cart> cart, List<Mops> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments, CurrentUser cu)
        {
            ////Step 1: Find next invoice number 
            int receiptNumber = Convert.ToInt32(invoiceNumber.Split('-')[1]);

            //Step 3: Get date and time
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");

            //Step 4: Insert all relevent info into the mainInvoice table
            //SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //cmd.Connection = conn;
            cmd.CommandText = "Insert Into tbl_receipt (receiptNumber, receiptDate, receiptTime, custID, empID, locationID, "
                + "receiptTotal, transactionType, comments) values(@receiptNumber, @receiptDate, @receiptTime, @custID, "
                + "@empID, @locationID, @receiptTotal, @transactionType, @comments);";

            cmd.Parameters.AddWithValue("receiptNumber", receiptNumber);
            cmd.Parameters.AddWithValue("receiptDate", date);
            cmd.Parameters.AddWithValue("receiptTime", time);
            cmd.Parameters.AddWithValue("custID", c.customerId);
            cmd.Parameters.AddWithValue("empID", e.employeeID);
            cmd.Parameters.AddWithValue("locationID", cu.locationID);
            cmd.Parameters.AddWithValue("receiptTotal", ckm.dblTotal);
            cmd.Parameters.AddWithValue("transactionType", transactionType);
            cmd.Parameters.AddWithValue("comments", comments);
            //conn.Open();
            //SqlDataReader reader = cmd.ExecuteReader();
            //conn.Close();

            sqlPassingTest(cmd);

            //Step 5: Insert each item into the invoiceItem table
            //Loops through the cart to look at the items
            foreach (Cart item in cart)
            {
                //string insert = "insert into tbl_receiptItem values(" + receiptNumber + ", " + item.sku + ", " + item.quantity + ", '" +
                //    item.description + "', " + item.cost + ");";
                ////Inserts the item
                //invoiceItem(insert);
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandText = "Insert into tbl_receiptItem values(@receiptNum, @sku, @quantity, "
                    + "@description, @cost)";
                cmd1.Parameters.AddWithValue("receiptNum", receiptNumber);
                cmd1.Parameters.AddWithValue("sku", item.sku);
                cmd1.Parameters.AddWithValue("quantity", item.quantity);
                cmd1.Parameters.AddWithValue("description", item.description);
                cmd1.Parameters.AddWithValue("cost", item.cost);
                sqlPassingTest(cmd1);
            }
            //Step 6: Insert each MOP into the invoiceMOP table
            //Loops through the checkout to get the mops
            foreach (Mops mop in mops)
            {
                //string insert = "insert into tbl_receiptMOP values(" + receiptNumber + ", " + returnMOPNameasInt(mop.methodOfPayment) + ", " + mop.chequeNum + ", " + mop.amountPaid + ");";
                ////Inserts the mop
                //invoiceMOP(insert);
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "Insert into tbl_receiptMOP values(@receiptNumb, @mop, @chequeNum, @amountPaid)";
                cmd2.Parameters.AddWithValue("receiptNumb", receiptNumber);
                cmd2.Parameters.AddWithValue("mop", returnMOPNameasInt(mop.methodOfPayment));
                cmd2.Parameters.AddWithValue("chequeNum", mop.chequeNum);
                cmd2.Parameters.AddWithValue("amountPaid", mop.amountPaid);
                sqlPassingTest(cmd2);
            }
        }
        //Testing something
        public void sqlPassingTest(SqlCommand cmd)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            cmd.Connection = conn;
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int getNextReceiptNum()
        {
            int nextReceiptNum = 0;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select receiptNumber from tbl_receiptNumbers";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If there there is no invoiceNum
                if (reader["receiptNumber"] == DBNull.Value)
                {
                    nextReceiptNum = 0;
                }
                //If an invoiceNum is found
                else
                {
                    //Take the found invoiceNum, and increment by 1 so there won't be a duplicate
                    nextReceiptNum = Convert.ToInt32(reader["receiptNumber"]) + 1;
                }
                //Creates the invoice with the next invoice num
                createReceiptNum(nextReceiptNum);
            }
            conn.Close();
            //Returns the next invoiceNum
            return nextReceiptNum;
        }
        public void createReceiptNum(int recNum)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Update tbl_receiptNumbers set receiptNumber = @recNum";
            cmd.Parameters.AddWithValue("recNum", recNum);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int returnMOPNameasInt(string mopN)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select methodID from tbl_methodOfPayment where methodDesc = '" + mopN + "'";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int mop = 0;

            while (reader.Read())
            {
                int m = Convert.ToInt32(reader["methodID"]);
                mop = m;
            }
            conn.Close();
            //Returns the methodID
            return mop;
        }
        public string returnMOPIntasName(int mopN)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select methodDesc from tbl_methodOfPayment where methodID = @mopN";
            cmd.Parameters.AddWithValue("mopN", mopN);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string mop = "";

            while (reader.Read())
            {
                mop = Convert.ToString(reader["methodDesc"]);
            }
            conn.Close();
            //Returns the methodID
            return mop;
        }
        //Returns the max invoice num + 1
        public int getNextInvoiceNum()
        {
            int nextInvoiceNum = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select invoiceNum from tbl_InvoiceNumbers";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If there there is no invoiceNum
                if (reader["invoiceNum"] == DBNull.Value)
                {
                    nextInvoiceNum = 0;
                }
                //If an invoiceNum is found
                else
                {
                    //Take the found invoiceNum, and increment by 1 so there won't be a duplicate
                    nextInvoiceNum = Convert.ToInt32(reader["invoiceNum"]) + 1;
                }
                //Creates the invoice with the next invoice num
                createInvoiceNum(nextInvoiceNum);
            }
            conn.Close();
            //Returns the next invoiceNum
            return nextInvoiceNum;
        }
        //Create  the newly found invoice number
        public void createInvoiceNum(int invNum)
        {
            //string date = DateTime.Now.ToString("yyyy-MM-dd");
            //string time = DateTime.Now.ToString("HH:mm:ss");

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "update tbl_InvoiceNumbers set invoiceNum = @invNum";
            cmd.Parameters.AddWithValue("invNum", invNum);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        //Returns the max invoice subNum
        public int getNextInvoiceSubNum(int invoiceNumber)
        {
            int invoiceSubNum = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(invoiceSubNum) as invoiceSubNum from tbl_invoice Where invoiceNum = " + invoiceNumber + ";";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If there is no invoice sub num, set it to 0
                if (reader["invoiceSubNum"] == DBNull.Value)
                {
                    invoiceSubNum = 0;
                }
                else
                {
                    //If an invoice sub num is found, increment it
                    invoiceSubNum = Convert.ToInt32(reader["invoiceSubNum"]) + 1;
                }
            }
            conn.Close();
            //Return the invoice sub num
            return invoiceSubNum;
        }
        //Adding items to the invoice 
        public void invoiceItem(string insert)
        {
            //This method works by executing the string that is passed in as a database query
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = insert;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //Adding mops to the invoice 
        public void invoiceMOP(string insert)
        {
            //This method works by executing the string that is passed in as a database query
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = insert;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //Returns max sku
        public int maxSku(int sku, string table)
        {
            int maxSku = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(sku) as largestSku from tbl_" + table + ";";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If no sku is found, set it to 0
                if (reader["largestSku"] == DBNull.Value)
                {
                    maxSku = 0;
                }
                else
                {
                    //If a sku is found, increment by 1 to get the next largest sku
                    maxSku = Convert.ToInt32(reader["largestSku"]) + 1;
                }
            }
            conn.Close();
            //Returns the new largest sku
            return maxSku;
        }
        
    }
}