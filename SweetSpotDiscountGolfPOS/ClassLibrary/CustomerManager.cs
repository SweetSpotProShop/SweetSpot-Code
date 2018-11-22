using SweetShop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    public class CustomerManager
    {
        DatabaseCalls dbc = new DatabaseCalls();

        private List<Customer> ConvertFromDataTableToCustomer(DataTable dt)
        {
            List<Customer> customer = dt.AsEnumerable().Select(row =>
            new Customer
            {
                customerId = row.Field<int>("custID"),
                firstName = row.Field<string>("firstName"),
                lastName = row.Field<string>("lastName"),
                primaryAddress = row.Field<string>("primaryAddress"),
                secondaryAddress = row.Field<string>("secondaryAddress"),
                primaryPhoneNumber = row.Field<string>("primaryPhoneINT"),
                secondaryPhoneNumber = row.Field<string>("secondaryPhoneINT"),
                email = row.Field<string>("email"),
                city = row.Field<string>("city"),
                province = row.Field<int>("provStateID"),
                country = row.Field<int>("country"),
                postalCode = row.Field<string>("postZip"),
                emailList = row.Field<bool>("marketingEmail")
            }).ToList();
            return customer;
        }
        private List<Customer> ConvertFromDataTableToCustomerWithInvoices(DataTable dt, object[] objPageDetails)
        {
            InvoiceManager IM = new InvoiceManager();
            List<Customer> customer = dt.AsEnumerable().Select(row =>
            new Customer
            {
                customerId = row.Field<int>("custID"),
                firstName = row.Field<string>("firstName"),
                lastName = row.Field<string>("lastName"),
                primaryAddress = row.Field<string>("primaryAddress"),
                secondaryAddress = row.Field<string>("secondaryAddress"),
                primaryPhoneNumber = row.Field<string>("primaryPhoneINT"),
                secondaryPhoneNumber = row.Field<string>("secondaryPhoneINT"),
                email = row.Field<string>("email"),
                city = row.Field<string>("city"),
                province = row.Field<int>("provStateID"),
                country = row.Field<int>("country"),
                postalCode = row.Field<string>("postZip"),
                invoices = IM.ReturnInvoiceByCustomers(row.Field<int>("custID"), objPageDetails),
                emailList = row.Field<bool>("marketingEmail")
            }).ToList();
            return customer;
        }

        //Returns list of customers based on an customer ID
        public List<Customer> ReturnCustomer(int cust, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomer";
            string sqlCmd = "SELECT custID, firstName, lastName, primaryAddress, secondaryAddress, "
                + "primaryPhoneINT, secondaryPhoneINT, email, city, provStateID, country, postZip, "
                + "marketingEmail FROM tbl_customers WHERE custID = @custID";

            object[][] parms =
            {
                 new object[] { "@custID", cust },
            };

            List<Customer> customer = ConvertFromDataTableToCustomer(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
            return customer;
        }
        //Returns list of custoemrs based on an customer ID
        public List<Customer> ReturnCustomerWithInvoiceList(int cust, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomerWithInvoiceList";
            string sqlCmd = "SELECT custID, firstName, lastName, primaryAddress, secondaryAddress, "
                + "primaryPhoneINT, secondaryPhoneINT, email, city, provStateID, country, postZip, "
                + "marketingEmail FROM tbl_customers WHERE custID = @custID";

            object[][] parms =
            {
                 new object[] { "@custID", cust },
            };

            List<Customer> customer = ConvertFromDataTableToCustomerWithInvoices(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            return customer;
        }
        //Returns list of custoemrs based on an search text
        public List<Customer> ReturnCustomerBasedOnText(string searchText, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomerBasedOnText";
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    sqlCmd = "SELECT custID, firstName, lastName, primaryAddress, secondaryAddress, "
                        + "primaryPhoneINT, secondaryPhoneINT, email, city, provStateID, country, postZip, "
                        + "marketingEmail FROM tbl_customers WHERE CAST(custID AS VARCHAR) LIKE @parm1" + i
                        + " OR CONCAT(firstName,lastName) LIKE @parm2" + i
                        + " OR CONCAT(primaryPhoneINT,secondaryPhoneINT) LIKE @parm3" + i
                        + " OR email LIKE @parm4" + i + "";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT custID, firstName, lastName, primaryAddress, secondaryAddress, "
                        + "primaryPhoneINT, secondaryPhoneINT, email, city, provStateID, country, postZip, "
                        + "marketingEmail FROM tbl_customers WHERE CAST(custID AS VARCHAR) LIKE @parm1" + i
                        + " OR CONCAT(firstName,lastName) LIKE @parm2" + i
                        + " OR CONCAT(primaryPhoneINT,secondaryPhoneINT) LIKE @parm3" + i
                        + " OR email LIKE @parm4" + i + ")";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
            }
            sqlCmd += " ORDER BY firstName ASC";
            List<Customer> customer = ConvertFromDataTableToCustomer(dbc.returnDataTableDataFromArrayLists(sqlCmd, parms, strText, objPageDetails, strQueryName));
            return customer;
        }
        //Add Customer Nathan and Tyler created. Returns customer ID
        public int addCustomer(Customer c, object[] objPageDetails)
        {
            string strQueryName = "addCustomer";
            //New command
            string sqlCmd = "INSERT INTO tbl_customers (firstName, lastName, primaryAddress, "
                + "secondaryAddress, primaryPhoneINT, secondaryPhoneINT, billingAddress, "
                + "marketingEmail, email, city, provStateID, country, postZip) VALUES "
                + "(@FirstName, @LastName, @primaryAddress, @secondaryAddress, "
                + "@primaryPhoneNumber, @secondaryPhoneNumber, @billingAddress, "
                + "@marketingEmail, @Email, @City, @Province, @Country, @PostalCode)";

            object[][] parms =
            {
                 new object[] { "@FirstName", c.firstName },
                 new object[] { "@LastName", c.lastName },
                 new object[] { "@primaryAddress", c.primaryAddress },
                 new object[] { "@secondaryAddress", c.secondaryAddress },
                 new object[] { "@marketingEmail", c.emailList },
                 new object[] { "@City", c.city },
                 new object[] { "@billingAddress", "" },
                 new object[] { "@PostalCode", c.postalCode },
                 new object[] { "@Province", c.province },
                 new object[] { "@Country", c.country },
                 new object[] { "@primaryPhoneNumber", c.primaryPhoneNumber },
                 new object[] { "@secondaryPhoneNumber", c.secondaryPhoneNumber },
                 new object[] { "@Email", c.email }
            };

            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            List<Customer> customer = ReturnCustomerIDFromCustomerStats(parms, objPageDetails);
            return customer[0].customerId;
        }
        public void updateCustomer(Customer c, object[] objPageDetails)
        {
            string strQueryName = "updateCustomer";
            //New command
            string sqlCmd = "UPDATE tbl_customers SET firstName = @FirstName, lastName = @LastName, primaryAddress = @primaryAddress,"
                + " secondaryAddress = @secondaryAddress, primaryPhoneINT = @primaryPhoneNumber, secondaryPhoneINT = @secondaryPhoneNumber,"
                + " marketingEmail = @marketingEmail, email = @Email, city = @City, provStateID = @Province, country = @Country,"
                + " postZip = @PostalCode WHERE custID = @CustomerID";

            object[][] parms =
            {
                new object[] { "CustomerID", c.customerId },
                new object[] { "FirstName", c.firstName },
                new object[] { "LastName", c.lastName },
                new object[] { "primaryAddress", c.primaryAddress },
                new object[] { "secondaryAddress", c.secondaryAddress },
                new object[] { "primaryPhoneNumber", c.primaryPhoneNumber },
                new object[] { "secondaryPhoneNumber", c.secondaryPhoneNumber },
                new object[] { "marketingEmail", c.emailList },
                new object[] { "Email", c.email },
                new object[] { "City", c.city },
                new object[] { "Province", c.province },
                new object[] { "Country", c.country },
                new object[] { "PostalCode", c.postalCode }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public List<Customer> ReturnCustomerIDFromCustomerStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomerIDFromCustomerStats";
            string sqlCmd = "SELECT custID, firstName, lastName, primaryAddress, secondaryAddress, primaryPhoneINT, "
                + "secondaryPhoneINT, marketingEmail, email, city, provStateID, country, postZip "
                + "FROM tbl_customers WHERE firstName = @FirstName and lastName = @LastName and "
                + "primaryAddress = @primaryAddress and secondaryAddress = @secondaryAddress and primaryPhoneINT = "
                + "@primaryPhoneNumber and secondaryPhoneINT = @secondaryPhoneNumber and "
                + "marketingEmail = @marketingEmail and email = @Email and city = @City and provStateID = @Province "
                + "and country = @Country and postZip = @PostalCode";
            return ConvertFromDataTableToCustomer(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
    }
}