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
                isTradeIn = row.Field<bool>("isTradeIn"),
                comments = row.Field<string>("comments")
            }).ToList();
            return clubs;
        }

        public List<object> ReturnListOfObjectsFromThreeTables(int sku, object[] objPageDetails)
        {
            string strQueryName = "ReturnListOfObjectsFromThreeTables";
            string sqlCmd = "SELECT sku, brandID, modelID, clubType, shaft, numberOfClubs, "
                + "premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, "
                + "typeID, locationID, isTradeIn, comments FROM tbl_clubs WHERE sku = @sku";

            object[][] parms =
            {
                 new object[] { "@sku", sku }
            };

            List<Clubs> c = ConvertFromDataTableToClubs(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            sqlCmd = "SELECT sku, size, colour, gender, style, price, cost, brandID, quantity, "
                + "typeID, locationID, comments FROM tbl_clothing WHERE sku = @sku";

            List<Clothing> cl = ConvertFromDataTableToClothing(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            sqlCmd = "SELECT sku, size, colour, price, cost, brandID, modelID, accessoryType, "
                + "quantity, typeID, locationID, comments FROM tbl_accessories WHERE sku = @sku";

            List<Accessories> a = ConvertFromDataTableToAccessories(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));

            List<object> o = new List<object>();
            o.AddRange(a);
            o.AddRange(cl);
            o.AddRange(c);
            return o;
        }
        //Return Model string created by Nathan and Tyler **getModelName
        public string ReturnModelNameFromModelID(int modelID, object[] objPageDetails)
        {
            string strQueryName = "ReturnModelNameFromModelID";
            string sqlCmd = "Select modelName from tbl_model where modelID = @modelID";

            object[][] parms =
            {
                 new object[] { "@modelID", modelID }
            };
            //Returns the model name
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Return Brand string created by Nathan and Tyler **getBrandName
        public string ReturnBrandNameFromBrandID(int brandID, object[] objPageDetails)
        {
            string strQueryName = "ReturnBrandNameFromBrandID";
            string sqlCmd = "SELECT brandName FROM tbl_brand WHERE brandID = @brandID";
            object[][] parms =
            {
                 new object[] { "@brandID", brandID }
            };
            //Returns the brand name
            return dbc.MakeDataBaseCallToReturnString(sqlCmd, parms, objPageDetails, strQueryName);
        }
        //Returns max sku from the skuNumber table based on itemType and directs code to store it
        public int ReturnMaxSku(int itemType, object[] objPageDetails)
        {
            string strQueryName = "ReturnMaxSku";
            string sqlCmd = "SELECT sku FROM tbl_skuNumbers WHERE itemType = @itemType";
            object[][] parms =
            {
                 new object[] { "@itemType", itemType }
            };

            int maxSku = dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) + 1;
            StoreMaxSku(maxSku, itemType, objPageDetails);
            //Returns the new max sku
            return maxSku;
        }
        //Stores the max sku in the skuNumber table
        public void StoreMaxSku(int sku, int itemType, object[] objPageDetails)
        {
            string strQueryName = "StoreMaxSku";
            //This method stores the max sku along with its item type
            string sqlCmd = "UPDATE tbl_skuNumbers SET sku = @sku WHERE itemType = @itemType";
            object[][] parms =
            {
                 new object[] { "@sku", sku },
                 new object[] { "@itemType", itemType }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //**Add Item**
        //Adds new Item to tables Nathan created
        public int AddNewItemToDatabase(object o, object[] objPageDetails)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            int sku = -10;
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                AddClubToDatabase(c, objPageDetails);
                sku = c.sku;
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                AddAccessoryToDatabase(a, objPageDetails);
                sku = a.sku;
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                AddClothingToDatabase(cl, objPageDetails);
                sku = cl.sku;
            }
            //Returns the sku of the new item
            return sku;
        }
        //These three actully add the item to specific tables Nathan created
        private void AddClubToDatabase(Clubs c, object[] objPageDetails)
        {
            string strQueryName = "AddClubToDatabase";
            string sqlCmd = "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs,"
                    + " premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, isTradeIn, comments)"
                    + " Values (@sku, @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price,"
                    + " @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @isTradeIn, @comments)";

            object[][] parms =
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
                 new object[] { "@isTradeIn", c.isTradeIn },
                 new object[] { "@comments", c.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void AddAccessoryToDatabase(Accessories a, object[] objPageDetails)
        {
            string strQueryName = "AddAccessoryToDatabase";
            string sqlCmd = "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, modelID, accessoryType, quantity, typeID, locationID, comments)"
                + " Values (@sku, @size, @colour, @price, @cost, @brandID, @modelID, @accessoryType, @quantity, @typeID, @locationID, @comments)";

            object[][] parms =
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
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void AddClothingToDatabase(Clothing cl, object[] objPageDetails)
        {
            string strQueryName = "AddClothingToDatabase";
            string sqlCmd = "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID, comments)"
                + " Values (@sku, @size, @colour, @gender, @style, @price, @cost, @brandID, @quantity, @typeID, @locationID, @comments)";

            object[][] parms =
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
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //**Update Item**
        public int UpdateItemInDatabase(object o, object[] objPageDetails)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            int sku = -10;
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                UpdateClubInDatabase(c, objPageDetails);
                sku = c.sku;
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                UpdateAccessoryInDatabase(a, objPageDetails);
                sku = a.sku;
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                UpdateClothingInDatabase(cl, objPageDetails);
                sku = cl.sku;
            }
            //Returns the sku of the new item
            return sku;
        }
        //These three actully update the item in their specific tables Nathan created
        private void UpdateClubInDatabase(Clubs c, object[] objPageDetails)
        {
            string strQueryName = "UpdateClubInDatabase";
            string sqlCmd = "UPDATE tbl_clubs SET brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft,"
                + " numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity,"
                + " clubSpec = @clubSpec, shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity,"
                + " locationID = @locationID, isTradeIn = @isTradeIn, comments = @comments WHERE sku = @sku";

            object[][] parms =
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
                 new object[] { "@isTradeIn", c.isTradeIn },
                 new object[] { "@comments", c.comments }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateAccessoryInDatabase(Accessories a, object[] objPageDetails)
        {
            string strQueryName = "UpdateAccessoryInDatabase";
            string sqlCmd = "UPDATE tbl_accessories SET size = @size, colour = @colour, price = @price, cost = @cost, brandID = @brandID,"
                + "modelID = @modelID, accessoryType = @accessoryType, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku";

            object[][] parms =
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
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private void UpdateClothingInDatabase(Clothing cl, object[] objPageDetails)
        {
            string strQueryName = "UpdateClothingInDatabase";
            string sqlCmd = "UPDATE tbl_clothing SET size = @size, colour = @colour, gender = @gender, style = @style,"
                + " price = @price, cost = @cost, brandID = @brandID, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku";

            object[][] parms =
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
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }

        //**OLD CODE**
        //**Used in Reports.importItems
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
        //**Used in Reports.importItems
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
        //**Used in ItemDataUtilities.brandName
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
        //**Used in ItemDataUtilities.modelName
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
        //**Used in SweetShopManager.transferTradeInStart
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
        //**Used in SweetShopManager.getSingleReceipt
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
    }
}