using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using SweetShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetSpotProShop
{
    //This class is used for way too much...
    public class ItemDataUtilities
    {
        private String connectionString;
        LocationManager lm = new LocationManager();
        //Connection String
        public ItemDataUtilities()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }
        //Return Model string created by Nathan and Tyler **getModelName
        public string modelType(int modelID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select modelName from tbl_model where modelID = " + modelID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string model = null;

            while (reader.Read())
            {
                string m = reader["modelName"].ToString();
                model = m;
            }
            conn.Close();
            //Returns the model name
            return model;
        }
        //Return Brand string created by Nathan and Tyler **getBrandName
        public string brandType(int brandID)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = conn;
            cmd.CommandText = "Select brandName from tbl_brand where brandID = " + brandID;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            string brand = null;

            while (reader.Read())
            {
                string b = reader["brandName"].ToString();
                brand = b;
            }
            conn.Close();
            //Returns the brand name
            return brand;
        }
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
                ca.description = brandType(c.brandID) + " " + modelType(c.modelID) + " " + 
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
                ca.description = brandType(a.brandID) + " " + modelType(a.modelID) + " " + 
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
                ca.description = brandType(cl.brandID) + " " + cl.size + " " + cl.colour + " " + 
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
        public List<Items> getItemByID(Int32 ItemNumber, string loc)
        {
            //Loops through the database and adds items with the matching sku to a list of type item
            List<Items> items = new List<Items>();
            Items i = new Items();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            int intLocation = lm.locationIDfromCity(loc);
            cmd.Connection = conn;
            conn.Open();
            //Removed location because client did not want
            cmd.CommandText = "Select sku, quantity, brandID, modelID, accessoryType, size, colour, comments, price, cost From tbl_accessories Where SKU = @skuAcc";
            cmd.Parameters.AddWithValue("skuAcc", ItemNumber);
            //cmd.Parameters.AddWithValue("locationID", intLocation);

            SqlDataReader readerAcc = cmd.ExecuteReader();
            while (readerAcc.Read())
            {

                i = new Items(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"])) + " "
                    + modelType(Convert.ToInt32(readerAcc["modelID"])) + " " + readerAcc["accessoryType"].ToString() 
                    + " " + readerAcc["size"].ToString() + " " + readerAcc["colour"].ToString() + " " + readerAcc["comments"].ToString(),
                    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                    Convert.ToDouble(readerAcc["cost"]));

            }
            if (!readerAcc.HasRows)
            {
                readerAcc.Close();
                cmd.CommandText = "Select sku, brandID, modelID, clubType, clubSpec, shaftSpec, shaftFlex, "
                    + "dexterity, quantity, price, cost From tbl_clubs Where SKU = @skuClubs";
                cmd.Parameters.AddWithValue("skuClubs", ItemNumber);
                //cmd.Parameters.AddWithValue("locationIDclubs", intLocation);
                SqlDataReader readerClubs = cmd.ExecuteReader();
                while (readerClubs.Read())
                {
                    i = new Items(Convert.ToInt32(readerClubs["sku"]),
                        
                        brandType(Convert.ToInt32(readerClubs["brandID"]))
                        + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubSpec"].ToString()
                        + " " + readerClubs["clubType"].ToString() + " " + readerClubs["shaftSpec"].ToString() + " "
                        + readerClubs["shaftFlex"].ToString() + " " + readerClubs["dexterity"].ToString(),

                        Convert.ToInt32(readerClubs["quantity"]), Convert.ToDouble(readerClubs["price"]),
                        Convert.ToDouble(readerClubs["cost"]));

                }
                if (!readerClubs.HasRows)
                {
                    readerClubs.Close();
                    cmd.CommandText = "Select sku, brandID, size, colour, gender, style, comments, quantity, price, cost From tbl_clothing Where SKU = @skuClothing";
                    cmd.Parameters.AddWithValue("skuClothing", ItemNumber);
                    //cmd.Parameters.AddWithValue("locationIDclothing", intLocation);
                    SqlDataReader readerClothing = cmd.ExecuteReader();
                    while (readerClothing.Read())
                    {
                        i = new Items(Convert.ToInt32(readerClothing["sku"]), brandType(Convert.ToInt32(readerClothing["brandID"]))
                            + " " + readerClothing["size"].ToString() + " " + readerClothing["colour"].ToString()
                            + " " + readerClothing["gender"].ToString() + " " + readerClothing["style"].ToString()
                            + " " + readerClothing["comments"].ToString(),
                            Convert.ToInt32(readerClothing["quantity"]), Convert.ToDouble(readerClothing["price"]),
                            Convert.ToDouble(readerClothing["cost"]));
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
                i = new Items(Convert.ToInt32(readerAcc["sku"]), brandType(Convert.ToInt32(readerAcc["brandID"])) + " "
                    + modelType(Convert.ToInt32(readerAcc["modelID"])) + " " + readerAcc["size"].ToString() + " "
                    + readerAcc["colour"].ToString() + " " + readerAcc["comments"].ToString(),
                    Convert.ToInt32(readerAcc["quantity"]), Convert.ToDouble(readerAcc["price"]),
                    Convert.ToDouble(readerAcc["cost"]), Convert.ToInt32(readerAcc["typeID"]),
                    lm.locationName(Convert.ToInt32(readerAcc["locationID"])));

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
                    i = new Items(Convert.ToInt32(readerClubs["sku"]), brandType(Convert.ToInt32(readerClubs["brandID"]))
                        + " " + modelType(Convert.ToInt32(readerClubs["modelID"])) + " " + readerClubs["clubType"].ToString()
                        + " " + readerClubs["clubSpec"].ToString() + " " + readerClubs["clubType"].ToString() + " "
                        + readerClubs["shaftSpec"].ToString() + " " + readerClubs["shaftFlex"].ToString() + " "
                        + readerClubs["dexterity"].ToString(), Convert.ToInt32(readerClubs["quantity"]),
                        Convert.ToDouble(readerClubs["price"]), Convert.ToDouble(readerClubs["cost"]),
                        Convert.ToInt32(readerClubs["typeID"]), lm.locationName(Convert.ToInt32(readerClubs["locationID"])));

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
                        i = new Items(Convert.ToInt32(readerClothing["sku"]), brandType(Convert.ToInt32(readerClothing["brandID"]))
                            + " " + readerClothing["size"].ToString() + " " + readerClothing["colour"].ToString()
                            + " " + readerClothing["gender"].ToString() + " " + readerClothing["style"].ToString()
                            + " " + readerClothing["comments"].ToString(),
                            Convert.ToInt32(readerClothing["quantity"]), Convert.ToDouble(readerClothing["price"]),
                            Convert.ToDouble(readerClothing["cost"]), Convert.ToInt32(readerClothing["typeID"]), 
                            lm.locationName(Convert.ToInt32(readerClothing["locationID"])));
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
            tradeInSkuDisplay = tradeInSku(loc);
            int[] range = new int[2];
            //Returns the range for the trade in sku
            range = tradeInSkuRange(loc);
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Insert into tbl_tempTradeInCartSkus (sku, brandID, modelID, clubType, "
                + "shaft, numberOfClubs, premium, cost, price, quantity, clubSpec, shaftSpec, "
                + "shaftFlex, dexterity, typeID, locationID, used, comments) values (@sku, 10, 2148, "
                + "'', '', '', 0, 0, 0, 0, '', '', '', '', 1, @locationID, 1, '');";
            cmd.Parameters.AddWithValue("sku", tradeInSkuDisplay);
            cmd.Parameters.AddWithValue("locationID", loc);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            //Returns the trade in items display sku
            return tradeInSkuDisplay;
        }
        //Grabbing trade-in sku
        public int tradeInSku(int location)
        {
            int sku = 0;
            int[] range = new int[2];
            //Returns the range for the trade in sku
            range = tradeInSkuRange(location);
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select max(sku) as maxsku from tbl_tempTradeInCartSkus where locationID = @locationID";
            cmd.Parameters.AddWithValue("locationID", location.ToString());
            //cmd.Parameters.AddWithValue("lowerRange", range[0]);
            //cmd.Parameters.AddWithValue("upperRange", range[1]);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            Clubs club = new Clubs();
            int maxSku = 0;
            while (reader.Read())
            {
                //Gets the max sku
                maxSku = (reader["maxsku"] as int?) ?? range[0]; //Setting it to 0
            }
            conn.Close();

            //If the maxSku returns as null, sets the sku as the min range
            if (maxSku.Equals(null))
            {
                sku = range[0];
            }
            //If the maxSku is less than the upper range, sku is the max sku + 1
            else if (maxSku < range[1])
            {
                sku = maxSku + 1;
            }
            //If the maxSku equals the upper range, sku equals the min range
            else if (maxSku == range[1])
            {
                sku = range[0];
            }

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
            if(tradeInItem.itemlocation == 0)
            { tradeInItem.itemlocation = 1; }
            cmd.Connection = conn;
            //cmd.CommandText = "insert into tbl_tempTradeInCartSkus values(" + tradeInItem.sku + ", " + tradeInItem.brandID + ", " +
            //    tradeInItem.modelID + ", '" + tradeInItem.clubType + "', '" + tradeInItem.shaft + "', '" + tradeInItem.numberOfClubs + "', " +
            //    tradeInItem.premium + ", " + tradeInItem.cost + ", " + tradeInItem.price + ", " + tradeInItem.quantity + ", '" +
            //    tradeInItem.clubSpec + "', '" + tradeInItem.shaftSpec + "', '" + tradeInItem.shaftFlex + "', '" +
            //    tradeInItem.dexterity + "', " + tradeInItem.typeID + ", " + tradeInItem.itemlocation + ", " +
            //    used + ", '" + tradeInItem.comments + "');";
            cmd.CommandText = "Update tbl_tempTradeInCartSkus set brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft," +
                "numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity, clubSpec = @clubSpec," +
                "shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity, typeID = @typeID, locationID = @locationID, used = @used," +
                "comments = @comments where sku = @sku;";
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
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
        //Sending all of the invoice information to the database 
        public void mainInvoice(CheckoutManager ckm, List<Cart> cart, List<Checkout> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments)
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
            else if(transactionType == 2)
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
            cmd.Parameters.AddWithValue("locationID", e.locationID);
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
            if(transactionType == 1)
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
            foreach (Checkout mop in mops)
            {
                string insert = "insert into tbl_invoiceMOP values(" + nextInvoiceNum + ", " + nextInvoiceSubNum + ", '" + mop.methodOfPayment + "', " + mop.amountPaid + ");";
                //Inserts the mop
                invoiceMOP(insert);
            }
        }
        public void mainPurchaseInvoice(CheckoutManager ckm, List<Cart> cart, List<Checkout> mops, Customer c, Employee e, int transactionType, string invoiceNumber, string comments)
        {
            ////Step 1: Find next invoice number 
            int receiptNumber = Convert.ToInt32(invoiceNumber.Split('-')[1]);

            //Step 3: Get date and time
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");

            //Step 4: Insert all relevent info into the mainInvoice table
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Insert Into tbl_receipt (receiptNumber, receiptDate, receiptTime, custID, empID, locationID, "
                + "receiptTotal, transactionType, comments) values(@receiptNumber, @receiptDate, @receiptTime, @custID, "
                + "@empID, @locationID, @receiptTotal, @transactionType, @comments);";

            cmd.Parameters.AddWithValue("receiptNumber", receiptNumber);
            cmd.Parameters.AddWithValue("receiptDate", date);
            cmd.Parameters.AddWithValue("receiptTime", time);
            cmd.Parameters.AddWithValue("custID", c.customerId);
            cmd.Parameters.AddWithValue("empID", e.employeeID);
            cmd.Parameters.AddWithValue("locationID", e.locationID);
            cmd.Parameters.AddWithValue("receiptTotal", ckm.dblTotal);
            cmd.Parameters.AddWithValue("transactionType", transactionType);
            cmd.Parameters.AddWithValue("comments", comments);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
            //Step 5: Insert each item into the invoiceItem table
            //Loops through the cart to look at the items
            foreach (Cart item in cart)
            {
                string insert = "insert into tbl_receiptItem values(" + receiptNumber + ", " + item.sku + ", " + item.quantity + ", '" +
                    item.description + "', " + item.cost + ");";
                //Inserts the item
                invoiceItem(insert);
            }
            //Step 6: Insert each MOP into the invoiceMOP table
            //Loops through the checkout to get the mops
            foreach (Checkout mop in mops)
            {
                string insert = "insert into tbl_receiptMOP values(" + receiptNumber + ", " + returnMOPNameasInt(mop.methodOfPayment) + ", " + mop.chequeNum + ", " + mop.amountPaid + ");";
                //Inserts the mop
                invoiceMOP(insert);
            }
        }
        public int getNextReceiptNum()
        {
            int nextReceiptNum = 0;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(receiptNumber) as receiptNumber from tbl_receiptNumbers";
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
            cmd.CommandText = "Insert into tbl_receiptNumbers(receiptNumber) values(@recNum);";
            cmd.Parameters.AddWithValue("recNum", recNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
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
        //Returns the max invoice num + 1
        public int getNextInvoiceNum()
        {
            int nextInvoiceNum = 0;
            
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(invoiceNum) as invoiceNum from tbl_InvoiceNumbers";
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
            //invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, empID, locationID, subTotal, discountAmount, tradeinAmount
            //governmentTax, provincialTax, balanceDue, transactionType, comments
            cmd.CommandText = "Insert into tbl_InvoiceNumbers(invoiceNum) values(@invNum);";

            //"Insert into tbl_invoice(invoiceNum, invoiceSubNum, invoiceDate, invoiceTime, custID, " +
            //"empID, locationID, subTotal, shippingAmount, discountAmount, tradeinAmount, " +
            //"governmentTax, provincialTax, balanceDue, transactionType, comments) values(@invNum, @invSubNum, @date, @time" +
            //", 1, @empID, 1, 0, 0, 0, 0, 0, 0, 0, 1, '');";
            cmd.Parameters.AddWithValue("invNum", invNum);
            //cmd.Parameters.AddWithValue("invSubNum", subNum);
            //cmd.Parameters.AddWithValue("date", date);
            //cmd.Parameters.AddWithValue("time", time);            
            //cmd.Parameters.AddWithValue("@empID", empID);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
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
        //Returns max sku from the skuNumber table based on itemType and directs code to store it
        public int maxSku(int itemType)
        {
            int maxSku = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select Max(sku) as largestSku from tbl_skuNumbers where itemType = @itemType;";
            cmd.Parameters.AddWithValue("@itemType", itemType);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //If there is no sku found, set it to 0
                if (reader["largestSku"] == DBNull.Value)
                {
                    maxSku = 0;
                    //Stores the max sku along with its itemType
                    storeMaxSku(maxSku, itemType);
                }
                else
                {
                    //If a sku is found, increment it by 1
                    maxSku = Convert.ToInt32(reader["largestSku"]) + 1;
                    //Stores the new max sku along with its itemType
                    storeMaxSku(maxSku, itemType);
                }
            }
            conn.Close();
            //Returns the new max sku
            return maxSku;
        }
        //Stores the max sku in the skuNumber table
        public void storeMaxSku(int sku, int itemType)
        {
            //This method stores the max sku along with its item type
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "insert into tbl_skuNumbers values(@sku, @itemType);";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@itemType", itemType);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
    }
}