using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Collections;
using System.Configuration;
using SweetSpotProShop;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetShop
{
    public class SweetShopManager
    {
        private string connectionString;
        ItemDataUtilities idu = new ItemDataUtilities();
        LocationManager lm = new LocationManager();

        public SweetShopManager()
        {

            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        /*******Customer Utilities************************************************************************************/
        //Retrieves Customer from search parameters Nathan and Tyler created
        public List<Customer> GetCustomerfromSearch(string searchField)
        {
            try
            {
                ArrayList strText = new ArrayList();
                int numFields = searchField.Split(' ').Length;

                if (numFields > 1)
                {
                    for (int i = 0; i < numFields; i++)
                    {
                        strText.Add(searchField.Split(' ')[i]);
                    }
                }
                else
                {
                    strText.Add(searchField);
                }
                //Declares space for connection string and new command
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();

                for (int i = 0; i < numFields; i++)
                {
                    if (i == 0)
                    {
                        //cmd.CommandText = "Select * From tbl_customers Where (firstName + ' ' + lastName) Like '%@searchField1%' or (primaryPhoneINT + ' ' + secondaryPhoneINT) like '%@searchField2%' order by firstName asc";
                        cmd.CommandText = "Select * From tbl_customers Where Concat(firstName,lastName) Like '%" + strText[i] + "%' or Concat(primaryPhoneINT,secondaryPhoneINT) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + " Intersect (Select * From tbl_customers Where Concat(firstName,lastName) Like '%" + strText[i] + "%' or Concat(primaryPhoneINT,secondaryPhoneINT) like '%" + strText[i] + "%')";
                    }
                }
                cmd.CommandText = cmd.CommandText + "  order by firstName asc;";
                SqlDataReader reader = cmd.ExecuteReader();
                //New List for Customer
                List<Customer> customer = new List<Customer>();
                //Begin reading
                while (reader.Read())
                {
                    Customer c = new Customer(Convert.ToInt32(reader["custID"]),
                        reader["firstName"].ToString(),
                        reader["lastName"].ToString(),
                        reader["primaryAddress"].ToString(),
                        reader["secondaryAddress"].ToString(),
                        reader["primaryPhoneINT"].ToString(),
                        reader["secondaryPhoneINT"].ToString(),
                        Convert.ToBoolean(reader["marketingEmail"]),
                        reader["email"].ToString(),
                        reader["city"].ToString(),
                        Convert.ToInt32(reader["provStateID"]),
                        Convert.ToInt32(reader["country"]),
                        reader["postZip"].ToString());
                    //Adds the customer
                    customer.Add(c);
                }
                con.Close();
                //Returns the customer(s)
                return customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        //Retreive Customer from custID Nathan and Tyler created
        public Customer GetCustomerbyCustomerNumber(int custNum)
        {
            try
            {
                //Declares space for connection string and new command
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select * From tbl_customers Where custID = @custNum";
                cmd.Parameters.AddWithValue("custNum", custNum);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //New Customer
                Customer customer = new Customer();
                //Begin reading
                while (reader.Read())
                {
                    Customer c = new Customer(Convert.ToInt32(reader["custID"]),
                        reader["firstName"].ToString(),
                        reader["lastName"].ToString(),
                        reader["primaryAddress"].ToString(),
                        reader["secondaryAddress"].ToString(),
                        reader["primaryPhoneINT"].ToString(),
                        reader["secondaryPhoneINT"].ToString(),
                        Convert.ToBoolean(reader["marketingEmail"]),
                        reader["email"].ToString(),
                        reader["city"].ToString(),
                        Convert.ToInt32(reader["provStateID"]),
                        Convert.ToInt32(reader["country"]),
                        reader["postZip"].ToString());
                    //Sets the customer
                    customer = c;
                }
                con.Close();
                //Returns customer
                return customer;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        //Add Customer Nathan and Tyler created. Returns customer ID
        public int addCustomer(Customer c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Insert Into tbl_customers (firstName, lastName, primaryAddress,"
                + " secondaryAddress, primaryPhoneINT, secondaryPhoneINT, billingAddress, marketingEmail, email,"
                + " city, provStateID, country, postZip) Values (@FirstName, @LastName, @primaryAddress,"
                + " @secondaryAddress, @primaryPhoneNumber, @secondaryPhoneNumber, @billingAddress, @marketingEmail,"
                + " @Email, @City, @Province, @Country, @PostalCode)";
            cmd.Parameters.AddWithValue("FirstName", c.firstName);
            cmd.Parameters.AddWithValue("LastName", c.lastName);
            cmd.Parameters.AddWithValue("primaryAddress", c.primaryAddress);
            cmd.Parameters.AddWithValue("secondaryAddress", c.secondaryAddress);
            cmd.Parameters.AddWithValue("marketingEmail", c.emailList);
            cmd.Parameters.AddWithValue("City", c.city);
            cmd.Parameters.AddWithValue("billingAddress", "");
            cmd.Parameters.AddWithValue("PostalCode", c.postalCode);
            cmd.Parameters.AddWithValue("Province", c.province);
            cmd.Parameters.AddWithValue("Country", c.country);
            cmd.Parameters.AddWithValue("primaryPhoneNumber", c.primaryPhoneNumber);
            cmd.Parameters.AddWithValue("secondaryPhoneNumber", c.secondaryPhoneNumber);
            cmd.Parameters.AddWithValue("Email", c.email);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
            //Returns customer ID
            return returnCustomerNumber(c);
        }
        //Nathan and Tyler created. Returns customer ID
        public int returnCustomerNumber(Customer c)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select custID From tbl_customers Where firstName = @FirstName and lastName = @LastName and primaryAddress = @primaryAddress " +
                "and secondaryAddress = @secondaryAddress and primaryPhoneINT = @primaryPhoneNumber " +
                "and secondaryPhoneINT = @secondaryPhoneNumber and marketingEmail = @marketingEmail and email = @Email " +
                "and city = @City and provStateID = @Province and country = @Country and postZip = @PostalCode;";
            cmd.Parameters.AddWithValue("FirstName", c.firstName);
            cmd.Parameters.AddWithValue("LastName", c.lastName);
            cmd.Parameters.AddWithValue("primaryAddress", c.primaryAddress);
            cmd.Parameters.AddWithValue("secondaryAddress", c.secondaryAddress);
            cmd.Parameters.AddWithValue("primaryPhoneNumber", c.primaryPhoneNumber);
            cmd.Parameters.AddWithValue("secondaryPhoneNumber", c.secondaryPhoneNumber);
            cmd.Parameters.AddWithValue("marketingEmail", c.emailList);
            cmd.Parameters.AddWithValue("Email", c.email);
            cmd.Parameters.AddWithValue("City", c.city);
            cmd.Parameters.AddWithValue("Province", c.province);
            cmd.Parameters.AddWithValue("Country", c.country);
            cmd.Parameters.AddWithValue("PostalCode", c.postalCode);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //New Customer number
            int custNum = 0;
            //Begin reading
            while (reader.Read())
            {
                custNum = Convert.ToInt32(reader["custID"]);
            }
            con.Close();
            //Returns customer ID
            return custNum;
        }
        //Update Customer Nathan and Tyler Created
        public void updateCustomer(Customer c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update tbl_customers Set firstName = @FirstName, lastName = @LastName, primaryAddress = @primaryAddress,"
                + " secondaryAddress = @secondaryAddress, primaryPhoneINT = @primaryPhoneNumber, secondaryPhoneINT = @secondaryPhoneNumber,"
                + " marketingEmail = @marketingEmail, email = @Email, city = @City, provStateID = @Province, country = @Country,"
                + " postZip = @PostalCode Where custID = @CustomerID";
            cmd.Parameters.AddWithValue("CustomerID", c.customerId);
            cmd.Parameters.AddWithValue("FirstName", c.firstName);
            cmd.Parameters.AddWithValue("LastName", c.lastName);
            cmd.Parameters.AddWithValue("primaryAddress", c.primaryAddress);
            cmd.Parameters.AddWithValue("secondaryAddress", c.secondaryAddress);
            cmd.Parameters.AddWithValue("primaryPhoneNumber", c.primaryPhoneNumber);
            cmd.Parameters.AddWithValue("secondaryPhoneNumber", c.secondaryPhoneNumber);
            cmd.Parameters.AddWithValue("marketingEmail", c.emailList);
            cmd.Parameters.AddWithValue("Email", c.email);
            cmd.Parameters.AddWithValue("City", c.city);
            cmd.Parameters.AddWithValue("Province", c.province);
            cmd.Parameters.AddWithValue("Country", c.country);
            cmd.Parameters.AddWithValue("PostalCode", c.postalCode);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Update
            cmd.ExecuteNonQuery();
            con.Close();
        }

        /*******Item Utilities************************************************************************************/
        //Returns a list of items from a search
        public List<Items> returnSearchFromAllThreeItemSets(string searchedText, string loc)
        {
            //Different lists for the items
            List<Items> searchClubs = new List<Items>();
            List<Items> searchClothing = new List<Items>();
            List<Items> searchAccessories = new List<Items>();
            List<Items> searchedItems = new List<Items>();
            //Gets all items that match the searched text
            searchClubs = GetItemfromSearch(searchedText, "Clubs", loc);
            searchClothing = GetItemfromSearch(searchedText, "Accessories", loc);
            searchAccessories = GetItemfromSearch(searchedText, "Clothing", loc);
            //Looping through the clubs
            foreach (var item in searchClubs)
            {
                //Adds to the list
                searchedItems.Add(item);
            }
            //Looping through the clothing
            foreach (var item in searchClothing)
            {
                //Adds to the list
                searchedItems.Add(item);
            }
            //Looping through the accessories
            foreach (var item in searchAccessories)
            {
                //Adds to the list
                searchedItems.Add(item);
            }
            //Returns the searched items in a list
            return searchedItems;
        }
        //Robust search through inventory Nathan and Tyler created for specific location
        public List<Items> GetItemfromSearch(string itemSearched, string itemType, string loc)
        {
            //Array used to store the search elements
            ArrayList strText = new ArrayList();
            int intLocation = lm.locationIDfromCity(loc);
            int numFields = itemSearched.Split(' ').Length;

            //If there is more than one search element
            if (numFields > 1)
            {
                //Looping through the search elements, and adding them to the array
                for (int i = 0; i < numFields; i++)
                {
                    strText.Add(itemSearched.Split(' ')[i]);
                }
            }
            //Otherwise search the single search element
            else
            {
                strText.Add(itemSearched);
            }

            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //If type is clubs perform this search

            //Item List

            List<Items> item = new List<Items>();
            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            for (int i = 0; i < numFields; i++)
            {
                //Removed itemLocation because client no longer wants it. All locations can now view all items
                if (itemType == "Clubs")
                {
                    if (i == 0)
                    {
                        //Do the first search here, and then intersect everything else
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, shaft, numberOfClubs, clubSpec, shaftSpec, shaftFlex, dexterity) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        //The INTERSECT operator is used to return the records that are in common between two SELECT statements or data sets. 
                        //If a record exists in one query and not in the other, it will be omitted from the INTERSECT results. 
                        //It is the intersection of the two SELECT statements.
                        cmd.CommandText = cmd.CommandText + " Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, shaft, numberOfClubs, clubSpec, shaftSpec, shaftFlex, dexterity) like '%" + strText[i] + "%')";
                    }
                }
                // if type is accessories perform this search
                else if (itemType == "Accessories")
                {
                    if (i == 0)
                    {
                        //Do the first search here, and then intersect everything else
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, accessoryType, comments) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        //The INTERSECT operator is used to return the records that are in common between two SELECT statements or data sets. 
                        //If a record exists in one query and not in the other, it will be omitted from the INTERSECT results. 
                        //It is the intersection of the two SELECT statements.
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, accessoryType, comments) like '%" + strText[i] + "%')";
                    }
                }
                //if type is clothing perform this search
                else if (itemType == "Clothing")
                {
                    if (i == 0)
                    {
                        //Do the first search here, and then intersect everything else
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style, comments) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        //The INTERSECT operator is used to return the records that are in common between two SELECT statements or data sets. 
                        //If a record exists in one query and not in the other, it will be omitted from the INTERSECT results. 
                        //It is the intersection of the two SELECT statements.
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style, comments) like '%" + strText[i] + "%')";
                    }
                }
            }
            cmd.CommandText = cmd.CommandText + ";";
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string brand = null;
                string model = null;
                string description = null;

                ItemDataUtilities idu = new ItemDataUtilities();
                //if brand is not null return brand description
                if (reader["brandID"] != null)
                {
                    brand = idu.brandType(Convert.ToInt32(reader["brandID"]));
                }
                //if search type is clubs enter here for model and description builder
                if (itemType == "Clubs")
                {
                    //if model is not null return model description
                    if (reader["modelID"] != null)
                    {
                        model = idu.modelType(Convert.ToInt32(reader["modelID"]));
                    }
                    //create string for a club description
                    description = brand + " " + model + " " + reader["clubSpec"].ToString() + " " + reader["clubType"].ToString() + " "
                    + reader["shaftSpec"].ToString() + " " + reader["shaftFlex"].ToString() + " " + reader["dexterity"].ToString();
                }
                //if search type is accessories create accessories description
                else if (itemType == "Accessories")
                {
                    //if model is not null return model description
                    if (reader["modelID"] != null)
                    {
                        model = idu.modelType(Convert.ToInt32(reader["modelID"]));
                    }
                    description = brand + " " + model + " " + reader["accessoryType"] + " "
                        + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                        + reader["comments"].ToString();
                }
                //if search type is clothing create clothing description
                else if (itemType == "Clothing")
                {
                    description = brand + " " + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                    + reader["gender"].ToString() + " " + reader["style"].ToString() + " " + reader["comments"].ToString();
                }
                //enter all returned items into item list for display
                Items j = new Items(Convert.ToInt32(reader["sku"]),
                    description,
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToDouble(reader["price"]),
                    Convert.ToDouble(reader["cost"]));
                item.Add(j);
            }
            con.Close();
            //Returns the list of items
            return item;
        }
        //Robust search through inventory Nathan and Tyler created
        //Same as above method with the exception of not having string loc
        public List<Items> GetItemfromSearch(string itemSearched, string itemType)
        {
            ArrayList strText = new ArrayList();
            int numFields = itemSearched.Split(' ').Length;

            if (numFields > 1)
            {
                for (int i = 0; i < numFields; i++)
                {
                    strText.Add(itemSearched.Split(' ')[i]);
                }
            }
            else
            {
                strText.Add(itemSearched);
            }

            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //If type is clubs perform this search

            //Item List
            List<Items> item = new List<Items>();
            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            for (int i = 0; i < numFields; i++)
            {
                if (itemType == "Clubs")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, clubSpec, shaftSpec, shaftFlex, dexterity) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + " Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                        + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                        + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                        + " concat(clubType, clubSpec, shaftSpec, shaftFlex, dexterity) like '%" + strText[i] + "%')";
                    }
                }
                // if type is accessories perform this search
                else if (itemType == "Accessories")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, accessoryType, comments) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " modelID in (Select modelID from tbl_model where modelName like '%" + strText[i] + "%') or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, accessoryType, comments) like '%" + strText[i] + "%')";
                    }
                }
                //if type is clothing perform this search
                else if (itemType == "Clothing")
                {
                    if (i == 0)
                    {
                        cmd.CommandText = "Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style, comments) like '%" + strText[i] + "%'";
                    }
                    else
                    {
                        cmd.CommandText = cmd.CommandText + "Intersect (Select * from tbl_" + itemType + " where sku like '%" + strText[i] + "%' or "
                                    + " brandID in (Select brandID from tbl_brand where brandName like '%" + strText[i] + "%') or "
                                    + " concat(size, colour, gender, style, comments) like '%" + strText[i] + "%')";
                    }
                }
            }
            cmd.CommandText = cmd.CommandText + ";";
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string brand = null;
                string model = null;
                string description = null;

                ItemDataUtilities idu = new ItemDataUtilities();
                //if brand is not null return brand description
                if (reader["brandID"] != null)
                {
                    brand = idu.brandType(Convert.ToInt32(reader["brandID"]));
                }
                //if search type is clubs enter here for model and description builder
                if (itemType == "Clubs")
                {
                    //if model is not null return model description
                    if (reader["modelID"] != null)
                    {
                        model = idu.modelType(Convert.ToInt32(reader["modelID"]));
                    }
                    //create string for a club description
                    description = brand + " " + model + " " + reader["clubSpec"].ToString() + " " + reader["clubType"].ToString() +
                            " " + reader["shaftSpec"].ToString() + " " + reader["shaftFlex"].ToString() + " " + reader["dexterity"].ToString();
                }
                //if search type is accessories create accessories description
                else if (itemType == "Accessories")
                {
                    //if model is not null return model description
                    if (reader["modelID"] != null)
                    {
                        model = idu.modelType(Convert.ToInt32(reader["modelID"]));
                    }
                    description = brand + " " + model + " " + reader["accessoryType"].ToString()
                        + " " + reader["size"].ToString() + " " + reader["colour"].ToString()
                        + " " + reader["comments"].ToString();
                }
                //if search type is clothing create clothing description
                else if (itemType == "Clothing")
                {
                    description = brand + " " + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                    + reader["gender"].ToString() + " " + reader["style"].ToString() + " " + reader["comments"].ToString();
                }
                //enter all returned items into item list for display
                Items j = new Items(Convert.ToInt32(reader["sku"]),
                    description,
                    Convert.ToInt32(reader["quantity"]),
                    Convert.ToDouble(reader["price"]),
                    Convert.ToDouble(reader["cost"]),
                    lm.locationName(Convert.ToInt32(reader["locationID"])));
                item.Add(j);
            }
            con.Close();
            return item;
        }
        //Trade-in specific club from inventory lookup
        public Clubs tradeInItemLookUp(int sku)
        {
            //Looks for a trade in item by sku, and returns all info based on it
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, modelID, clubType, shaft, numberOfClubs, premium, cost, price, quantity, clubSpec,"
                + " shaftSpec, shaftFlex, dexterity, used, comments From tbl_tempTradeInCartSkus Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //Item List
            Clubs clubs = new Clubs();
            while (reader.Read())
            {
                clubs.sku = Convert.ToInt32(reader["sku"]);
                clubs.brandID = Convert.ToInt32(reader["brandID"]);
                clubs.modelID = Convert.ToInt32(reader["modelID"]);
                clubs.clubType = reader["clubType"].ToString();
                clubs.shaft = reader["shaft"].ToString();
                clubs.numberOfClubs = reader["numberOfClubs"].ToString();
                clubs.premium = Convert.ToDouble(reader["premium"]);
                clubs.cost = Convert.ToDouble(reader["cost"]);
                clubs.price = Convert.ToDouble(reader["price"]);
                clubs.quantity = Convert.ToInt32(reader["quantity"]);
                clubs.clubSpec = reader["clubSpec"].ToString();
                clubs.shaftSpec = reader["shaftSpec"].ToString();
                clubs.shaftFlex = reader["shaftFlex"].ToString();
                clubs.dexterity = reader["dexterity"].ToString();
                clubs.used = Convert.ToBoolean(reader["used"]);
                clubs.comments = reader["comments"].ToString();
            }
            con.Close();
            //The trade in item is being returned
            return clubs;
        }
        //Select specific club from inventory Nathan and Tyler created
        public Clubs singleItemLookUp(int sku)
        {
            //Searches for a club based on sku
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, modelID, typeID, clubType, shaft, numberOfClubs, premium, cost, price, quantity, clubSpec,"
                + " shaftSpec, shaftFlex, dexterity, locationID, used, comments From tbl_clubs Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //Item List
            Clubs clubs = new Clubs();
            while (reader.Read())
            {
                clubs.sku = Convert.ToInt32(reader["sku"]);
                clubs.brandID = Convert.ToInt32(reader["brandID"]);
                clubs.modelID = Convert.ToInt32(reader["modelID"]);
                clubs.typeID = Convert.ToInt32(reader["typeID"]);
                clubs.clubType = reader["clubType"].ToString();
                clubs.shaft = reader["shaft"].ToString();
                clubs.numberOfClubs = reader["numberOfClubs"].ToString();
                clubs.premium = Convert.ToDouble(reader["premium"]);
                clubs.cost = Convert.ToDouble(reader["cost"]);
                clubs.price = Convert.ToDouble(reader["price"]);
                clubs.quantity = Convert.ToInt32(reader["quantity"]);
                clubs.clubSpec = reader["clubSpec"].ToString();
                clubs.shaftSpec = reader["shaftSpec"].ToString();
                clubs.shaftFlex = reader["shaftFlex"].ToString();
                clubs.dexterity = reader["dexterity"].ToString();
                clubs.itemlocation = Convert.ToInt32(reader["locationID"]);
                clubs.used = Convert.ToBoolean(reader["used"]);
                clubs.comments = reader["comments"].ToString();
            }
            con.Close();
            //Returns the club
            return clubs;
        }
        //Adds new Item to tables Nathan created
        public int addItem(Object o)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for insertion
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                addClub(c);
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                addAccessory(a);
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                addClothing(cl);
            }
            //Returns the sku of the new item
            return returnItemNumber(o);
        }
        //Looks to see if the item already exists
        public void checkForItem(Object o)
        {
            //This method checks to see what type the object o is, and sends it to the proper method for checking
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                checkClub(c);
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                checkAccessory(a);
            }
            else if (o is Clothing)
            {
                Clothing cl = o as Clothing;
                checkClothing(cl);
            }
        }
        //returns sku number after adding to reload page with new item Nathan created
        public int returnItemNumber(Object o)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            if (o is Clubs)
            {
                Clubs c = o as Clubs;
                cmd.CommandText = "Select sku From tbl_clubs Where brandID = @brandID and modelID = @modelID and clubType = @clubType and "
                    + " shaft = @shaft and numberOfClubs = @numberOfClubs and premium = @premium and cost = @cost and price = @price and "
                    + " quantity = @quantity and clubSpec = @clubSpec and shaftSpec = @shaftSpec and shaftFlex = @shaftFlex and "
                    + " dexterity = @dexterity and  used = @used and typeID = @typeID and comments = @comments";
                cmd.Parameters.AddWithValue("brandID", c.brandID);
                cmd.Parameters.AddWithValue("modelID", c.modelID);
                cmd.Parameters.AddWithValue("clubType", c.clubType);
                cmd.Parameters.AddWithValue("shaft", c.shaft);
                cmd.Parameters.AddWithValue("numberOfClubs", c.numberOfClubs);
                cmd.Parameters.AddWithValue("premium", c.premium);
                cmd.Parameters.AddWithValue("cost", c.cost);
                cmd.Parameters.AddWithValue("price", c.price);
                cmd.Parameters.AddWithValue("quantity", c.quantity);
                cmd.Parameters.AddWithValue("clubSpec", c.clubSpec);
                cmd.Parameters.AddWithValue("shaftSpec", c.shaftSpec);
                cmd.Parameters.AddWithValue("shaftFlex", c.shaftFlex);
                cmd.Parameters.AddWithValue("dexterity", c.dexterity);
                cmd.Parameters.AddWithValue("used", c.used);
                cmd.Parameters.AddWithValue("typeID", c.typeID);
                cmd.Parameters.AddWithValue("comments", c.comments);
            }
            else if (o is Accessories)
            {
                Accessories a = o as Accessories;
                cmd.CommandText = "Select sku From tbl_accessories Where brandID = @brandID and size = @size and colour = @colour and"
                    + " price = @price and cost = @cost and quantity = @quantity";
                cmd.Parameters.AddWithValue("brandID", a.brandID);
                cmd.Parameters.AddWithValue("size", a.size);
                cmd.Parameters.AddWithValue("colour", a.colour);
                cmd.Parameters.AddWithValue("price", a.price);
                cmd.Parameters.AddWithValue("cost", a.cost);
                cmd.Parameters.AddWithValue("quantity", a.quantity);
            }
            else if (o is Clothing)
            {
                Clothing c = o as Clothing;
                cmd.CommandText = "Select sku From tbl_clothing Where brandID = @brandID and size = @size and colour = @colour and"
                    + " gender = @gender and style = @style and price = @price and cost = @cost and quantity = @quantity";
                cmd.Parameters.AddWithValue("brandID", c.brandID);
                cmd.Parameters.AddWithValue("size", c.size);
                cmd.Parameters.AddWithValue("colour", c.colour);
                cmd.Parameters.AddWithValue("gender", c.gender);
                cmd.Parameters.AddWithValue("style", c.style);
                cmd.Parameters.AddWithValue("price", c.price);
                cmd.Parameters.AddWithValue("cost", c.cost);
                cmd.Parameters.AddWithValue("quantity", c.quantity);
            }

            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            SqlDataReader reader = cmd.ExecuteReader();
            //New sku number
            int sku = 0;
            //Begin reading
            while (reader.Read())
            {
                sku = Convert.ToInt32(reader["sku"]);
            }
            con.Close();
            //Returns the sku of the item
            return sku;
        }
        //These three actully add the item to specific tables Nathan created
        public void addClub(Clubs c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            //Might not need this if statement anymore due to changes in how the skus are generated **LOOK INTO
            //Used to distinguish if the item was added via imports or the website
            if (c.sku < 10000000)
            {
                cmd.CommandText = "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs,"
                    + " premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, used, comments)"
                    + " Values (@sku, @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price,"
                    + " @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments)";
                //cmd.Parameters.AddWithValue("sku", c.sku);
            }
            else
            {
                //int nextSku = idu.maxSku(c.sku, "clubs");
                cmd.CommandText = "Insert Into tbl_clubs (sku, brandID, modelID, clubType, shaft, numberOfClubs,"
                    + " premium, cost, price, quantity, clubSpec, shaftSpec, shaftFlex, dexterity, typeID, locationID, used, comments)"
                    + " Values (@sku, @brandID, @modelID, @clubType, @shaft, @numberOfClubs, @premium, @cost, @price,"
                    + " @quantity, @clubSpec, @shaftSpec, @shaftFlex, @dexterity, @typeID, @locationID, @used, @comments)";
            }
            cmd.Parameters.AddWithValue("sku", c.sku);
            cmd.Parameters.AddWithValue("brandID", c.brandID);
            cmd.Parameters.AddWithValue("modelID", c.modelID);
            cmd.Parameters.AddWithValue("clubType", c.clubType);
            cmd.Parameters.AddWithValue("shaft", c.shaft);
            cmd.Parameters.AddWithValue("numberOfClubs", c.numberOfClubs);
            cmd.Parameters.AddWithValue("premium", c.premium);
            cmd.Parameters.AddWithValue("cost", c.cost);
            cmd.Parameters.AddWithValue("price", c.price);
            cmd.Parameters.AddWithValue("quantity", c.quantity);
            cmd.Parameters.AddWithValue("clubSpec", c.clubSpec);
            cmd.Parameters.AddWithValue("shaftSpec", c.shaftSpec);
            cmd.Parameters.AddWithValue("shaftFlex", c.shaftFlex);
            cmd.Parameters.AddWithValue("dexterity", c.dexterity);
            cmd.Parameters.AddWithValue("typeID", c.typeID);
            cmd.Parameters.AddWithValue("locationID", c.itemlocation);
            cmd.Parameters.AddWithValue("used", c.used);
            cmd.Parameters.AddWithValue("comments", c.comments);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void addAccessory(Accessories a)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            //Might not need this if statement anymore due to changes in how the skus are generated **LOOK INTO
            //Used to distinguish if the item was added via imports or the website
            if (a.sku < 30000000)
            {
                cmd.CommandText = "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, modelID, accessoryType, quantity, typeID, locationID, comments)"
            + " Values (@sku, @size, @colour, @price, @cost, @brandID, @modelID, @accessoryType, @quantity, @typeID, @locationID, @comments)";
                //cmd.Parameters.AddWithValue("sku", a.sku);
            }
            else
            {
                // int nextSku = idu.maxSku(a.sku, "accessories");
                cmd.CommandText = "Insert Into tbl_accessories (sku, size, colour, price, cost, brandID, modelID, accessoryType, quantity, typeID, locationID, comments)"
            + " Values (@sku, @size, @colour, @price, @cost, @brandID, @modelID, @accessoryType, @quantity, @typeID, @locationID, @comments)";
            }
            cmd.Parameters.AddWithValue("sku", a.sku);
            cmd.Parameters.AddWithValue("size", a.size);
            cmd.Parameters.AddWithValue("colour", a.colour);
            cmd.Parameters.AddWithValue("price", a.price);
            cmd.Parameters.AddWithValue("cost", a.cost);
            cmd.Parameters.AddWithValue("brandID", a.brandID);
            cmd.Parameters.AddWithValue("modelID", a.modelID);
            cmd.Parameters.AddWithValue("accessoryType", a.accessoryType);
            cmd.Parameters.AddWithValue("quantity", a.quantity);
            cmd.Parameters.AddWithValue("typeID", a.typeID);
            cmd.Parameters.AddWithValue("locationID", a.locID);
            cmd.Parameters.AddWithValue("comments", a.comments);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public void addClothing(Clothing c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            //Might not need this if statement anymore due to changes in how the skus are generated **LOOK INTO
            //Used to distinguish if the item was added via imports or the website
            if (c.sku < 50000000)
            {
                cmd.CommandText = "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID, comments)"
                + " Values (@sku, @size, @colour, @gender, @style, @price, @cost, @brandID, @quantity, @typeID, @locationID, @comments)";
                //cmd.Parameters.AddWithValue("sku", c.sku);
            }
            else
            {
                //int nextSku = idu.maxSku(c.sku, "clothing");
                cmd.CommandText = "Insert Into tbl_clothing (sku, size, colour, gender, style, price, cost, brandID, quantity, typeID, locationID, comments)"
                + " Values (@sku, @size, @colour, @gender, @style, @price, @cost, @brandID, @quantity, @typeID, @locationID, @comments)";
            }
            cmd.Parameters.AddWithValue("sku", c.sku);
            cmd.Parameters.AddWithValue("size", c.size);
            cmd.Parameters.AddWithValue("colour", c.colour);
            cmd.Parameters.AddWithValue("gender", c.gender);
            cmd.Parameters.AddWithValue("style", c.style);
            cmd.Parameters.AddWithValue("price", c.price);
            cmd.Parameters.AddWithValue("cost", c.cost);
            cmd.Parameters.AddWithValue("brandID", c.brandID);
            cmd.Parameters.AddWithValue("quantity", c.quantity);
            cmd.Parameters.AddWithValue("typeID", c.typeID);
            cmd.Parameters.AddWithValue("locationID", c.locID);
            cmd.Parameters.AddWithValue("@comments", c.comments);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Get item type
        public int getItemType(int sku)
        {
            //Bool's used to signify the item type of an item
            //Based on sku
            bool isClub = checkClubForItem(sku);
            bool isAccessory = checkAccessoryForItem(sku);
            bool isClothing = checkClothingForItem(sku);

            if (isClub == true)
            {
                return 1;
            }
            else if (isAccessory == true)
            {
                return 2;
            }
            else if (isClothing == true)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
        //Checks tbl_clubs for the sku. Used to see if the item is a club
        public bool checkClubForItem(int sku)
        {
            bool isClub = false;
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clubs where sku = " + sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                isClub = true;
            }
            //Closing
            con.Close();
            //Returns true or false
            return isClub;
        }
        //Checks tbl_accessories for the sku. Used to see if the item is an accessory
        public bool checkAccessoryForItem(int sku)
        {
            bool isAccessory = false;
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_accessories where sku = " + sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                isAccessory = true;
            }
            //If item doesn't exist
            else
            {
                isAccessory = false;
            }
            //Closing
            con.Close();
            //Returns true or false
            return isAccessory;
        }
        //Checks tbl_clothing for the sku. Used to see if the item is clothing
        public bool checkClothingForItem(int sku)
        {
            bool isClothing = false;
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clothing where sku = " + sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                isClothing = true;
            }
            //If item doesn't exist
            else
            {
                isClothing = false;
            }
            //Closing
            con.Close();
            //Returns true or false
            return isClothing;
        }
        //The check methods check if an item already exists or does not exist. It will update if it exists, and add if not.
        public void checkClub(Clubs c)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clubs where sku = " + c.sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            //This returns a 1 or higher if something is found
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                //update
                updateClub(c);
            }
            //If item doesn't exist
            else
            {
                //add
                addClub(c);
            }
            //Closing
            con.Close();
        }
        public void checkAccessory(Accessories a)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_accessories where sku = " + a.sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                //update
                updateAccessories(a);
            }
            //If item doesn't exist
            else
            {
                //add
                addAccessory(a);
            }
            //Closing
            con.Close();
        }
        public void checkClothing(Clothing cl)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select count(*) from tbl_clothing where sku = " + cl.sku;
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute search
            cmd.ExecuteNonQuery();
            int itemExists = (int)cmd.ExecuteScalar();

            //If item exists
            if (itemExists > 0)
            {
                //update
                updateClothing(cl);
            }
            //If item doesn't exist
            else
            {
                //add
                addClothing(cl);
            }
            //Closing
            con.Close();
        }
        //Returns single accessory Nathan created
        public Accessories getAccessory(int sku)
        {
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, modelID, accessoryType, size, colour, price, cost, quantity, typeID, locationID, comments From tbl_accessories Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);

            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            //Item List
            Accessories a = new Accessories();

            while (reader.Read())
            {
                a.sku = Convert.ToInt32(reader["sku"]);
                a.brandID = Convert.ToInt32(reader["brandID"]);
                a.modelID = Convert.ToInt32(reader["modelID"]);
                a.accessoryType = reader["accessoryType"].ToString();
                a.size = reader["size"].ToString();
                a.colour = reader["colour"].ToString();
                a.cost = Convert.ToDouble(reader["cost"]);
                a.price = Convert.ToDouble(reader["price"]);
                a.quantity = Convert.ToInt32(reader["quantity"]);
                a.typeID = Convert.ToInt32(reader["typeID"]);
                a.locID = Convert.ToInt32(reader["locationID"]);
                a.comments = reader["comments"].ToString();
            }
            con.Close();
            //Returns the accessory
            return a;
        }
        //Returns single clothing Nathan created
        public Clothing getClothing(int sku)
        {
            //New Command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select sku, brandID, size, colour, gender, style, price, cost, quantity, typeID, locationID, comments From tbl_clothing Where sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);

            //Open Database Connection
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            //Item List
            Clothing c = new Clothing();

            while (reader.Read())
            {
                c.sku = Convert.ToInt32(reader["sku"]);
                c.brandID = Convert.ToInt32(reader["brandID"]);
                c.size = reader["size"].ToString();
                c.colour = reader["colour"].ToString();
                c.gender = reader["gender"].ToString();
                c.style = reader["style"].ToString();
                c.cost = Convert.ToDouble(reader["cost"]);
                c.price = Convert.ToDouble(reader["price"]);
                c.quantity = Convert.ToInt32(reader["quantity"]);
                c.typeID = Convert.ToInt32(reader["typeID"]);
                c.locID = Convert.ToInt32(reader["locationID"]);
                c.comments = reader["comments"].ToString();
            }
            con.Close();
            //Returns the clothing
            return c;
        }
        //Update club in inventory updated Nathan 
        public void updateClub(Clubs c)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE tbl_clubs SET brandID = @brandID, modelID = @modelID, clubType = @clubType, shaft = @shaft,"
                + " numberOfClubs = @numberOfClubs, premium = @premium, cost = @cost, price = @price, quantity = @quantity,"
                + " clubSpec = @clubSpec, shaftSpec = @shaftSpec, shaftFlex = @shaftFlex, dexterity = @dexterity,"
                + " locationID = @locationID, comments = @comments WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", c.sku);
            cmd.Parameters.AddWithValue("@brandID", c.brandID);
            cmd.Parameters.AddWithValue("@modelID", c.modelID);
            cmd.Parameters.AddWithValue("@clubType", c.clubType);
            cmd.Parameters.AddWithValue("@shaft", c.shaft);
            cmd.Parameters.AddWithValue("@numberOfClubs", c.numberOfClubs);
            cmd.Parameters.AddWithValue("@premium", c.premium);
            cmd.Parameters.AddWithValue("@cost", c.cost);
            cmd.Parameters.AddWithValue("@quantity", c.quantity);
            cmd.Parameters.AddWithValue("@price", c.price);
            cmd.Parameters.AddWithValue("@comments", c.comments);
            cmd.Parameters.AddWithValue("@locationID", c.itemlocation);
            cmd.Parameters.AddWithValue("@clubSpec", c.clubSpec);
            cmd.Parameters.AddWithValue("@shaftSpec", c.shaftSpec);
            cmd.Parameters.AddWithValue("@shaftFlex", c.shaftFlex);
            cmd.Parameters.AddWithValue("@dexterity", c.dexterity);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Update accessory in inventory created Nathan
        public void updateAccessories(Accessories a)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE tbl_accessories SET size = @size, colour = @colour, price = @price, cost = @cost, brandID = @brandID,"
                + "modelID = @modelID, accessoryType = @accessoryType, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", a.sku);
            cmd.Parameters.AddWithValue("@size", a.size);
            cmd.Parameters.AddWithValue("@colour", a.colour);
            cmd.Parameters.AddWithValue("@price", a.price);
            cmd.Parameters.AddWithValue("@cost", a.cost);
            cmd.Parameters.AddWithValue("@brandID", a.brandID);
            cmd.Parameters.AddWithValue("@modelID", a.modelID);
            cmd.Parameters.AddWithValue("@accessoryType", a.accessoryType);
            cmd.Parameters.AddWithValue("@locationID", a.locID);
            cmd.Parameters.AddWithValue("@quantity", a.quantity);
            cmd.Parameters.AddWithValue("@comments", a.comments);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Update clothing in inventory created Nathan
        public void updateClothing(Clothing cl)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE tbl_clothing SET size = @size, colour = @colour, gender = @gender, style = @style,"
                + " price = @price, cost = @cost, brandID = @brandID, quantity = @quantity, locationID = @locationID, comments = @comments WHERE sku = @sku";
            cmd.Parameters.AddWithValue("@sku", cl.sku);
            cmd.Parameters.AddWithValue("@size", cl.size);
            cmd.Parameters.AddWithValue("@colour", cl.colour);
            cmd.Parameters.AddWithValue("@gender", cl.gender);
            cmd.Parameters.AddWithValue("@style", cl.style);
            cmd.Parameters.AddWithValue("@price", cl.price);
            cmd.Parameters.AddWithValue("@cost", cl.cost);
            cmd.Parameters.AddWithValue("@brandID", cl.brandID);
            cmd.Parameters.AddWithValue("@quantity", cl.quantity);
            cmd.Parameters.AddWithValue("@locationID", cl.locID);
            cmd.Parameters.AddWithValue("@comments", cl.comments);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //Checks to see if an item is used
        public bool isTradein(int sku)
        {
            bool trade = false;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT used FROM tbl_clubs WHERE sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    trade = Convert.ToBoolean(reader["used"]);
                }
            }
            con.Close();
            //Returns true or false
            return trade;
        }
        //Determines what type an item is
        public int whatTypeIsItem(int sku)
        {

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            //Check clubs
            cmd.CommandText = "SELECT typeID FROM tbl_clubs WHERE sku = @sku";
            cmd.Parameters.AddWithValue("sku", sku);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            int intType = 0;
            while (reader.Read())
            {
                intType = Convert.ToInt32(reader["typeID"]);
            }
            if (!reader.HasRows) //Couldn't find anything 
            {
                reader.Close();
                //Check accessories
                cmd.CommandText = "Select typeID from tbl_accessories where SKU = @skuacces";
                cmd.Parameters.AddWithValue("skuacces", sku);
                SqlDataReader readerAccesories = cmd.ExecuteReader();

                while (readerAccesories.Read())
                {
                    intType = Convert.ToInt32(readerAccesories["typeID"]);
                }
                if (!readerAccesories.HasRows) //Couldn't find anything
                {
                    readerAccesories.Close();
                    //Check clothing
                    cmd.CommandText = "Select typeID from tbl_clothing where SKU = @skucloth";
                    cmd.Parameters.AddWithValue("skucloth", sku);
                    SqlDataReader readerclothing = cmd.ExecuteReader();

                    while (readerclothing.Read())
                    {
                        intType = Convert.ToInt32(readerclothing["typeID"]);
                    }
                }
            }
            //Returns the item type ID
            return intType;
        }
        //Returns the description of the searched item
        public string getDescription(int sku, int type)
        {
            //Current: Brand Name, Model, Club Type, Shaft, Number of Clubs, Dexterity
            //Needs to change to: Brand Name, Model, Club Spec, Club Type, Shaft Spec, Shaftflex, Dexterity
            string desc = "";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            //***********CONVERT ALL TO HAVE SUBQUERIES TO GET NAMES
            if (type == 1) //Club
            {
                // cmd.CommandText = "Select brandID, modelID, clubType, shaft, numberOfClubs, dexterity from tbl_clubs where sku = @skuCl";
                cmd.CommandText = "Select brandID, modelID, clubSpec, clubType, shaftSpec, shaftFlex, dexterity from tbl_clubs where sku = @skuCl";
                cmd.Parameters.AddWithValue("@skuCl", sku);
            }
            else if (type == 2) //Accessory
            {
                cmd.CommandText = "Select brandID, modelID, accessoryType, size, colour from tbl_accessories where sku = @skuAc";
                cmd.Parameters.AddWithValue("@skuAc", sku);
            }
            else if (type == 3) //Clothing
            {
                cmd.CommandText = "Select brandID, size, colour, gender, style from tbl_clothing where sku = @skuClo";
                cmd.Parameters.AddWithValue("@skuClo", sku);
            }
            cmd.CommandText = cmd.CommandText + ";";
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //Club
                if (type == 1)
                {
                    desc = idu.brandType(Convert.ToInt32(reader["brandID"])).ToString() + " " + idu.modelType(Convert.ToInt32(reader["modelID"])).ToString() + " "
                        + reader["clubSpec"].ToString() + " " + reader["clubType"].ToString() + " "
                        + reader["shaftSpec"].ToString() + " " + reader["shaftFlex"].ToString() + " " + reader["dexterity"].ToString();
                }
                //Accessory
                else if (type == 2)
                {
                    desc = idu.brandType(Convert.ToInt32(reader["brandID"])).ToString() + idu.modelType(Convert.ToInt32(reader["modelID"])).ToString()
                        + " " + reader["accessoryType"] + " " + reader["size"].ToString() + " " + reader["colour"].ToString();
                }
                //Clothing
                else if (type == 3)
                {
                    desc = idu.brandType(Convert.ToInt32(reader["brandID"])).ToString() + " " + reader["size"].ToString() + " " + reader["colour"].ToString() + " "
                        + reader["gender"].ToString() + " " + reader["style"].ToString();
                }
            }
            //Returns the description
            return desc;
        }
        /*******Invoice Utilities************************************************************************************/
        //Get Invoice by invoiceID and return the invoice object
        public List<Invoice> getInvoice(int invoiceID)
        {
            //12
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments FROM tbl_invoice "
                + "WHERE invoiceNum = @invoiceNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                i.Add(new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    Convert.ToDateTime(reader["invoiceTime"]), Convert.ToInt32(reader["custID"]), Convert.ToInt32(reader["empID"]), Convert.ToInt32(reader["locationID"]),
                    Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), Convert.ToInt32(reader["transactionType"]), reader["comments"].ToString()));
            }
            con.Close();
            //Returns the invoice
            return i;
        }
        //Nathan built for home page sales display
        public List<Invoice> getInvoiceBySaleDate(DateTime givenDate, int locationID)
        {
            //Gets a list of all invoices based on date and location. Stores in a list
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, custID, empID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue FROM tbl_invoice "
                + "WHERE invoiceDate = @givenDate AND locationID = @locationID  ;";
            cmd.Parameters.AddWithValue("givenDate", givenDate);
            cmd.Parameters.AddWithValue("locationID", locationID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                Invoice inv = new Invoice();
                inv.invoiceNum = Convert.ToInt32(reader["invoiceNum"]);
                inv.invoiceSub = Convert.ToInt32(reader["invoiceSubNum"]);
                inv.customerID = Convert.ToInt32(reader["custID"]);
                inv.employeeID = Convert.ToInt32(reader["empID"]);
                inv.subTotal = Convert.ToDouble(reader["subTotal"]);
                inv.discountAmount = Convert.ToDouble(reader["discountAmount"]);
                inv.tradeinAmount = Convert.ToDouble(reader["tradeinAmount"]);
                inv.governmentTax = Convert.ToDouble(reader["governmentTax"]);
                inv.provincialTax = Convert.ToDouble(reader["provincialTax"]);
                inv.balanceDue = Convert.ToDouble(reader["balanceDue"]);

                i.Add(inv);
            }
            con.Close();
            //Returns a list of invoices
            return i;
        }
        public Invoice getSingleInvoice(int invoiceID, int invoiceSub)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments FROM tbl_invoice "
                + "WHERE invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceID);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSub);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Invoice i = new Invoice();
            while (reader.Read())
            {
                i = new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    Convert.ToDateTime(reader["invoiceTime"]), Convert.ToInt32(reader["custID"]), Convert.ToInt32(reader["empID"]), Convert.ToInt32(reader["locationID"]),
                    Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), Convert.ToInt32(reader["transactionType"]), reader["comments"].ToString());
            }
            con.Close();
            con.Open();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments FROM tbl_deletedInvoice "
                + "WHERE invoiceNum = @invoiceNumb and invoiceSubNum = @invoiceSubNumb";
            cmd.Parameters.AddWithValue("invoiceNumb", invoiceID);
            cmd.Parameters.AddWithValue("invoiceSubNumb", invoiceSub);
            SqlDataReader readerDel = cmd.ExecuteReader();
            while (readerDel.Read())
            {
                i = new Invoice(Convert.ToInt32(readerDel["invoiceNum"]), Convert.ToInt32(readerDel["invoiceSubNum"]), Convert.ToDateTime(readerDel["invoiceDate"]),
                    Convert.ToDateTime(readerDel["invoiceTime"]), Convert.ToInt32(readerDel["custID"]), Convert.ToInt32(readerDel["empID"]), Convert.ToInt32(readerDel["locationID"]),
                    Convert.ToDouble(readerDel["subTotal"]), Convert.ToDouble(readerDel["discountAmount"]), Convert.ToDouble(readerDel["tradeinAmount"]), Convert.ToDouble(readerDel["governmentTax"]),
                    Convert.ToDouble(readerDel["provincialTax"]), Convert.ToDouble(readerDel["balanceDue"]), Convert.ToInt32(readerDel["transactionType"]), readerDel["comments"].ToString());
            }
            con.Close();
            //Returns the invoice
            return i;
        }
        //Returns a list of invoices that fit the search criteria
        public List<Invoice> multiTypeSearchInvoices(string searchCriteria, Nullable<DateTime> searchDate)
        {
            //Gets a list of customers that match the search criteria
            List<Customer> c = GetCustomerfromSearch(searchCriteria);
            List<int> customerNum = new List<int>();
            //Loops through the customers and gets their customer ID
            foreach (var cust in c)
            {
                customerNum.Add(cust.customerId);
            }

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            string selectStatement = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue, transactionType, comments FROM tbl_invoice "
                + "WHERE (";
            int firstCust = Enumerable.First(customerNum);
            //Loops through the customer IDs
            foreach (var num in customerNum)
            {
                //Adding more "where" statements 
                if (num.Equals(firstCust))
                {
                    selectStatement += "custID = " + num;
                }
                else
                {
                    selectStatement += "or custID = " + num;
                }
            }
            //Doesn't always have a value, but when it does...
            if (searchDate.HasValue)
            {
                //Comparing the invoice date to the searched date
                selectStatement += ") and invoiceDate = @searchDate";
                cmd.Parameters.AddWithValue("@searchDate", searchDate);
            }
            //Closing the select statement
            else
            {
                selectStatement += ")";
            }

            cmd.CommandText = selectStatement;
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                Invoice inv = new Invoice();
                inv.invoiceNum = Convert.ToInt32(reader["invoiceNum"]);
                inv.invoiceSub = Convert.ToInt32(reader["invoiceSubNum"]);
                inv.invoiceDate = Convert.ToDateTime(reader["invoiceDate"]);
                inv.invoiceTime = Convert.ToDateTime(reader["invoiceTime"]);
                inv.customerID = Convert.ToInt32(reader["custID"]);
                inv.employeeID = Convert.ToInt32(reader["empID"]);
                inv.locationID = Convert.ToInt32(reader["locationID"]);
                inv.subTotal = Convert.ToDouble(reader["subTotal"]);
                inv.discountAmount = Convert.ToDouble(reader["discountAmount"]);
                inv.tradeinAmount = Convert.ToDouble(reader["tradeinAmount"]);
                inv.governmentTax = Convert.ToDouble(reader["governmentTax"]);
                inv.provincialTax = Convert.ToDouble(reader["provincialTax"]);
                inv.balanceDue = Convert.ToDouble(reader["balanceDue"]);
                inv.transactionType = Convert.ToInt32(reader["transactionType"]);
                inv.comments = reader["comments"].ToString();

                i.Add(inv);
            }
            con.Close();
            //Returns a list of invoices that match the search criteria
            return i;
        }
        public List<Cart> returningItems(int invoiceNumber, int invoiceSub)
        {
            //The list where the invoice items will go
            List<Cart> retItems = new List<Cart>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT sku, itemQuantity, itemCost, itemPrice, itemDiscount, percentage"
                + " FROM tbl_invoiceItem WHERE invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNumber);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSub);

            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Cart cartItem;
            while (reader.Read())
            {
                //Checking item type
                int intType = whatTypeIsItem(Convert.ToInt32(reader["sku"]));
                bool bolTrade = false;
                //If it is a club, check if it is a trade in
                if (intType == 1)
                    bolTrade = isTradein(Convert.ToInt32(reader["sku"]));

                cartItem = new Cart(Convert.ToInt32(reader["sku"]),
                getDescription(Convert.ToInt32(reader["sku"]), intType),
                Convert.ToInt32(reader["itemQuantity"]),
                Convert.ToDouble(reader["itemPrice"]),
                Convert.ToDouble(reader["itemCost"]),
                Convert.ToDouble(reader["itemDiscount"]),
                Convert.ToBoolean(reader["percentage"]),
                bolTrade,
                intType);
                //Adding the item to the returnItem list
                retItems.Add(cartItem);
            }
            reader.Close();
            List<Cart> remaingItemsAvailForRet = new List<Cart>();

            //Compares the returned item quantities with those of the original invoice. It takes the difference to find out how many are left and able to be returned
            cmd.CommandText = "select tbl_invoiceItem.invoiceNum, tbl_invoiceItem.sku, sum(distinct tbl_invoiceItem.itemQuantity) - "
                            + "case when sum(tbl_invoiceItemReturns.itemQuantity) is null or sum(tbl_invoiceItemReturns.itemQuantity) = '' "
                            + "then 0 else sum(tbl_invoiceItemReturns.itemQuantity) end as itemQuantity from tbl_invoiceItem "
                            + "left Join tbl_invoiceItemReturns ON tbl_invoiceItem.invoiceNum = tbl_invoiceItemReturns.invoiceNum "
                            + "and tbl_invoiceItem.sku = tbl_invoiceItemReturns.sku where tbl_invoiceItem.invoiceNum = @iNum "
                            + "group by tbl_invoiceItem.invoiceNum, tbl_invoiceItem.sku";

            cmd.Parameters.AddWithValue("iNum", invoiceNumber);


            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cartItem = new Cart(Convert.ToInt32(reader["sku"]), "", Convert.ToInt32(reader["itemQuantity"]),
                0, 0, 0, false, false, 0);
                //These are the items that can still be returned
                remaingItemsAvailForRet.Add(cartItem);
            }
            //Looping through the original items in the invoice
            List<Cart> finalItems = new List<Cart>();
            foreach (var rCart in retItems)
            {
                bool bolAlreadyRet = false;
                //Looping through the items that can be returned for each item in the invoice
                foreach (var arCart in remaingItemsAvailForRet)
                {
                    //Comparing sku's to see if the returned items and invoice items sku matches
                    if (rCart.sku == arCart.sku)
                    {
                        bolAlreadyRet = true;

                        if (arCart.quantity > 0)
                        {
                            cartItem = new Cart(rCart.sku, rCart.description, arCart.quantity,
                                    rCart.price, rCart.cost, rCart.discount, rCart.percentage, rCart.tradeIn, rCart.typeID);
                            finalItems.Add(cartItem);
                        }
                    }
                }
                if (!bolAlreadyRet)
                {
                    finalItems.Add(rCart);
                }
            }
            con.Close();
            //The items that can be returned
            return finalItems;
        }
        //Gets invoices between dates
        public List<Invoice> getInvoiceBetweenDates(DateTime startDate, DateTime endDate)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, custID, empID, locationID, subTotal, discountAmount, "
                + "tradeinAmount, governmentTax, provincialTax, balanceDue FROM tbl_invoice "
                + "WHERE invoiceDate between @startDate AND @endDate";
            cmd.Parameters.AddWithValue("startDate", startDate);
            cmd.Parameters.AddWithValue("endDate", endDate);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                Invoice inv = new Invoice();
                inv.invoiceNum = Convert.ToInt32(reader["invoiceNum"]);
                inv.invoiceSub = Convert.ToInt32(reader["invoiceSubNum"]);
                inv.invoiceDate = Convert.ToDateTime(reader["invoiceDate"]);
                inv.customerID = Convert.ToInt32(reader["custID"]);
                inv.employeeID = Convert.ToInt32(reader["empID"]);
                inv.locationID = Convert.ToInt32(reader["locationID"]);
                inv.subTotal = Convert.ToDouble(reader["subTotal"]);
                inv.discountAmount = Convert.ToDouble(reader["discountAmount"]);
                inv.tradeinAmount = Convert.ToDouble(reader["tradeinAmount"]);
                inv.governmentTax = Convert.ToDouble(reader["governmentTax"]);
                inv.provincialTax = Convert.ToDouble(reader["provincialTax"]);
                inv.balanceDue = Convert.ToDouble(reader["balanceDue"]);

                i.Add(inv);
            }
            con.Close();
            //Returns the list of invoices
            return i;
        }
        //Get Items
        public List<Cart> invoice_getItems(int invoiceNum, int invoiceSubNum, string table)
        {
            List<Cart> items = new List<Cart>();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select * FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int sku = Convert.ToInt32(reader["sku"]);
                items.Add(new Cart(sku, getDescription(sku, getItemType(sku)),
                    Convert.ToInt32(reader["itemQuantity"]), Convert.ToDouble(reader["itemPrice"]),
                    Convert.ToDouble(reader["itemCost"]), Convert.ToDouble(reader["itemDiscount"]),
                    Convert.ToBoolean(reader["percentage"]), Convert.ToDouble(reader["itemRefund"])));
            }
            conn.Close();
            //Returns list of the items
            return items;
        }
        //Get Checkout Totals
        public CheckoutManager invoice_getCheckoutTotals(int invoiceNum, int invoiceSubNum, string table)
        {
            CheckoutManager ckm = new CheckoutManager();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select subTotal, shippingAmount, discountAmount, tradeinAmount, governmentTax, provincialTax " +
                "FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                double gst = Convert.ToDouble(reader["governmentTax"]);
                double pst = Convert.ToDouble(reader["provincialTax"]);
                bool isGST = false;
                bool isPST = false;
                if (gst != 0)
                {
                    isGST = true;
                }
                if (pst != 0)
                {
                    isPST = true;
                }
                double shipping;
                try
                {
                    shipping = Convert.ToDouble(reader["shippingAmount"]);
                }
                catch (Exception ex)
                {
                    shipping = 0;
                }

                //ckm = new CheckoutManager(Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["discountAmount"]),
                //    Convert.ToDouble(reader["tradeinAmount"]), 0, isGST, isPST, gst, pst, 0);
                ckm = new CheckoutManager(Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]),
                    shipping, isGST, isPST, gst, pst, 0, Convert.ToDouble(reader["subTotal"]));


            }
            conn.Close();
            //Returns checkout totals
            return ckm;
        }
        //Get Methods of Payment
        public List<Checkout> invoice_getMOP(int invoiceNum, int invoiceSubNum, string table)
        {
            List<Checkout> mops = new List<Checkout>();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select mopType, amountPaid FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                mops.Add(new Checkout(reader["mopType"].ToString(), Convert.ToDouble(reader["amountPaid"])));
            }
            conn.Close();
            //Returns the methods of payment
            return mops;
        }
        //Gets Location id from invoice
        public string invoice_getLocation(int invoiceNum, int invoiceSubNum, string table)
        {
            int locID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select locationID FROM " + table + " Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                locID = Convert.ToInt32(reader["locationID"]);
            }
            conn.Close();
            string loc = lm.locationCity(locID);
            //Returns the location 
            return loc;
        }
        //Gets the reason why the invoice was deleted
        public string deletedInvoice_getReason(int invoiceNum, int invoiceSubNum)
        {
            string reason = "";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select deletionReason FROM tbl_deletedInvoice Where invoiceNum = @invoiceNum and invoiceSubNum = @invoiceSubNum";
            cmd.Parameters.AddWithValue("invoiceNum", invoiceNum);
            cmd.Parameters.AddWithValue("invoiceSubNum", invoiceSubNum);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                reason = reader["deletionReason"].ToString();
            }
            conn.Close();
            //Returns the reason the invoice was deleted
            return reason;
        }
        //Gets the ivoices based on a sku
        public List<Invoice> getInvoiceFromSku(int sku, int locID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT tbl_invoice.invoiceNum, tbl_invoice.invoiceSubNum, tbl_invoice.invoiceDate," +
                " Cast(tbl_invoice.invoiceTime as DATETIME) as invoiceTime, tbl_invoice.custID, tbl_invoice.empID," +
                " tbl_invoice.locationID, tbl_invoice.subTotal, tbl_invoice.discountAmount, " +
                " tbl_invoice.tradeinAmount, tbl_invoice.governmentTax, tbl_invoice.provincialTax, " +
                " tbl_invoice.balanceDue, tbl_invoice.transactionType, tbl_invoice.comments FROM tbl_invoice " +
                " inner join tbl_invoiceItem on " +
                " tbl_invoice.invoiceNum = tbl_invoiceItem.invoiceNum " +
                " where tbl_invoiceItem.sku = @sku and tbl_invoice.locationID = @locID";
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@locID", locID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                i.Add(new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    Convert.ToDateTime(reader["invoiceTime"]), Convert.ToInt32(reader["custID"]), Convert.ToInt32(reader["empID"]), Convert.ToInt32(reader["locationID"]),
                    Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), Convert.ToInt32(reader["transactionType"]), reader["comments"].ToString()));
            }
            con.Close();
            //Returns the invoice
            return i;
        }
        //Gets ivoice based on custID
        public List<Invoice> getInvoiceFromCustID(int custID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT invoiceNum, invoiceSubNum, invoiceDate, Cast(invoiceTime as DATETIME) as invoiceTime, custID, "
                            + "empID, locationID, subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue, "
                            + "transactionType, comments from tbl_invoice where custID = @custID";
            cmd.Parameters.AddWithValue("@custID", custID);
            cmd.Connection = con;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Invoice> i = new List<Invoice>();
            while (reader.Read())
            {
                i.Add(new Invoice(Convert.ToInt32(reader["invoiceNum"]), Convert.ToInt32(reader["invoiceSubNum"]), Convert.ToDateTime(reader["invoiceDate"]),
                    Convert.ToDateTime(reader["invoiceTime"]), Convert.ToInt32(reader["custID"]), Convert.ToInt32(reader["empID"]), Convert.ToInt32(reader["locationID"]),
                    Convert.ToDouble(reader["subTotal"]), Convert.ToDouble(reader["discountAmount"]), Convert.ToDouble(reader["tradeinAmount"]), Convert.ToDouble(reader["governmentTax"]),
                    Convert.ToDouble(reader["provincialTax"]), Convert.ToDouble(reader["balanceDue"]), Convert.ToInt32(reader["transactionType"]), reader["comments"].ToString()));
            }
            con.Close();
            //Returns the invoice
            return i;
        }
        /*******Tax Utilities************************************************************************************/
        public List<Tax> getTaxes(int provStateID, DateTime recDate)
        {
            //New command
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select tr.taxRate, tbl_taxType.taxName from tbl_taxRate tr"
                            + " inner Join tbl_taxType on tr.taxID = tbl_taxType.taxID"
                            + " inner join (select taxID, max(taxDate) as MTD from tbl_taxRate where taxDate <= @recDate and provStateID = @provStateID Group By taxID) td"
                            + " on tr.taxID = td.taxID and tr.taxDate = td.MTD where provStateID = @provStateID;";
            cmd.Parameters.AddWithValue("provStateID", provStateID);
            cmd.Parameters.AddWithValue("recDate", recDate);
            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            List<Tax> tax = new List<Tax>();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Tax t = new Tax(reader["taxName"].ToString(),
                            Convert.ToDouble(reader["taxRate"]));
                tax.Add(t);
            }
            con.Close();
            //Returns the list of taxes
            return tax;
        }
        public void updateNewTaxRate(int provID, DateTime taxDate, int taxID, double taxRate)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Insert Into tbl_taxRate values(@provID, @taxDate, @taxID, @taxRate)";
            cmd.Parameters.AddWithValue("@provID", provID);
            cmd.Parameters.AddWithValue("@taxDate", taxDate);
            cmd.Parameters.AddWithValue("@taxID", taxID);
            cmd.Parameters.AddWithValue("@taxRate", taxRate);

            //Declare and open connection
            cmd.Connection = con;
            con.Open();
            //Execute Insert
            cmd.ExecuteNonQuery();
            con.Close();
        }
        //************************************Inventory Sales Utilities********************************************/
        //Returns the total discount of the cart
        public double returnDiscount(List<Cart> itemsSold)
        {
            double singleDiscoount = 0;
            double totalDiscount = 0;
            foreach (var cart in itemsSold)
            {
                //If it is a percent discount compared to a dollar amount
                if (cart.percentage)
                {
                    singleDiscoount = cart.quantity * (cart.price * (cart.discount / 100));
                }
                else
                {
                    singleDiscoount = cart.quantity * cart.discount;
                }
                totalDiscount += singleDiscoount;
            }
            //Returns the total discount rounded to 2 decimal places
            return Math.Round(totalDiscount, 2);
        }
        //Returns the total trade in amount of the cart
        public double returnTradeInAmount(List<Cart> itemsSold)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;
            ItemDataUtilities idu = new ItemDataUtilities();
            int[] range = idu.tradeInSkuRange(0);
            foreach (var cart in itemsSold)
            {
                if (cart.sku <= range[1] && cart.sku >= range[0])
                {
                    singleTradeInAmount = cart.quantity * cart.price;
                    totalTradeinAmount += singleTradeInAmount;
                }
            }
            //Returns the total trade in amount of the cart
            return totalTradeinAmount;
        }
        //Returns the total subtotal of the cart
        public double returnSubtotalAmount(List<Cart> itemsSold)
        {
            double totalSubtotalAmount = 0;
            double totalDiscountAmount = returnDiscount(itemsSold);
            double totalTradeInAmount = returnTradeInAmount(itemsSold);
            double totalTotalAmount = returnTotalAmount(itemsSold);

            totalSubtotalAmount = totalSubtotalAmount + totalTotalAmount;
            totalSubtotalAmount = totalSubtotalAmount - totalDiscountAmount;
            totalSubtotalAmount = totalSubtotalAmount - (totalTradeInAmount * (-1));
            //Returns the total subtotal amount of the cart
            return totalSubtotalAmount;
        }
        //Returns the total refund subtotal of the cart
        public double returnRefundSubtotalAmount(List<Cart> itemsSold)
        {
            double singleRefundSubtotalAmount = 0;
            double totalRefundSubtotalAmount = 0;
            foreach (var cart in itemsSold)
            {
                singleRefundSubtotalAmount = cart.quantity * cart.returnAmount;
                totalRefundSubtotalAmount += singleRefundSubtotalAmount;
            }
            //Returns the total refund subtotal of the cart
            return totalRefundSubtotalAmount;
        }
        //Returns the total amount of the cart
        public double returnTotalAmount(List<Cart> itemsSold)
        {
            ItemDataUtilities idu = new ItemDataUtilities();
            int[] range = idu.tradeInSkuRange(0);
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            foreach (var cart in itemsSold)
            {
                if (cart.sku >= range[1] || cart.sku <= range[0])
                {
                    singleTotalAmount = cart.quantity * cart.price;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            //Returns the total amount of the cart
            return totalTotalAmount;
        }
        public double returnPurchaseAmount(List<Cart> itemsSold)
        {
            double singlePurchaseAmount = 0;
            double totalPurchaseAmount = 0;
            foreach (var cart in itemsSold)
            {
                singlePurchaseAmount = cart.quantity * cart.cost;
                totalPurchaseAmount += singlePurchaseAmount;
            }
            //Returns the total amount of the cart
            return totalPurchaseAmount * -1;
        }
        //Transfering the trade in item to the clubs table
        //Not being used 12.9.17
        public void transferTradeInStart(List<Cart> Clubs)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            int[] range = idu.tradeInSkuRange(0);
            Object o;

            foreach (Cart ti in Clubs)
            {
                if (ti.sku <= range[1] && ti.sku >= range[0])
                {
                    cmd.CommandText = "Select * from tbl_tempTradeInCartSkus Where sku = " + ti.sku;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        Clubs transferClub = new Clubs
                        (Convert.ToInt32(reader["sku"]),
                        Convert.ToInt32(reader["brandID"].ToString()),
                        Convert.ToInt32(reader["modelID"].ToString()),
                        Convert.ToInt32(reader["typeID"].ToString()),
                        reader["clubType"].ToString(),
                        reader["shaft"].ToString(),
                        reader["numberOfClubs"].ToString(),
                        Convert.ToDouble(reader["premium"]),
                        Convert.ToDouble(reader["cost"]),
                        Convert.ToDouble(reader["price"]),
                        Convert.ToInt32(reader["quantity"]),
                        reader["clubSpec"].ToString(),
                        reader["shaftSpec"].ToString(),
                        reader["shaftFlex"].ToString(),
                        reader["dexterity"].ToString(),
                        Convert.ToInt32(reader["locationID"].ToString()),
                        Convert.ToBoolean(reader["used"].ToString()),
                        reader["comments"].ToString());

                        o = transferClub as Object;
                        addItem(o);
                        transferTradeInDeleteOld(transferClub.sku);
                    }
                    conn.Close();
                }
            }
        }
        public void transferTradeInDeleteOld(int sku)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Delete from tbl_tempTradeInCartSkus Where sku = " + sku;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            conn.Close();
        }
    }
}