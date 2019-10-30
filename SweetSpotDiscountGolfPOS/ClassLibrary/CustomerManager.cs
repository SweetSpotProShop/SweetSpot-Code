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
        DatabaseCalls DBC = new DatabaseCalls();

        private List<Customer> ConvertFromDataTableToCustomer(DataTable dt)
        {
            List<Customer> customer = dt.AsEnumerable().Select(row =>
            new Customer
            {
                intCustomerID = row.Field<int>("intCustomerID"),
                varFirstName = row.Field<string>("varFirstName"),
                varLastName = row.Field<string>("varLastName"),
                varAddress = row.Field<string>("varAddress"),
                varContactNumber = row.Field<string>("varContactNumber"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode"),
                bitSendMarketing = row.Field<bool>("bitSendMarketing")
            }).ToList();
            return customer;
        }
        private List<Customer> ConvertFromDataTableToCustomerWithInvoices(DataTable dt, object[] objPageDetails)
        {
            InvoiceManager IM = new InvoiceManager();
            List<Customer> customer = dt.AsEnumerable().Select(row =>
            new Customer
            {
                intCustomerID = row.Field<int>("intCustomerID"),
                varFirstName = row.Field<string>("varFirstName"),
                varLastName = row.Field<string>("varLastName"),
                varAddress = row.Field<string>("varAddress"),
                secondaryAddress = row.Field<string>("secondaryAddress"),
                varContactNumber = row.Field<string>("varContactNumber"),
                secondaryPhoneNumber = row.Field<string>("secondaryPhoneINT"),
                billingAddress = row.Field<string>("billingAddress"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode"),
                invoices = IM.ReturnInvoiceByCustomers(row.Field<int>("intCustomerID"), objPageDetails),
                bitSendMarketing = row.Field<bool>("bitSendMarketing")
            }).ToList();
            return customer;
        }

        //Returns list of customers based on an customer ID
        public List<Customer> ReturnCustomer(int customerID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomer";
            string sqlCmd = "SELECT intCustomerID, varFirstName, varLastName, varAddress, varContactNumber, varEmailAddress, "
                + "varCityName, intProvinceID, intCountryID, varPostalCode, bitSendMarketing FROM tbl_customers WHERE "
                + "intCustomerID = @intCustomerID";

            object[][] parms =
            {
                 new object[] { "@intCustomerID", customerID },
            };

            return ConvertFromDataTableToCustomer(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ConvertFromDataTableToCustomer(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
        //Returns list of custoemrs based on an customer ID
        public List<Customer> ReturnCustomerWithInvoiceList(int customerID, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomerWithInvoiceList";
            string sqlCmd = "SELECT intCustomerID, varFirstName, varLastName, varAddress, secondaryAddress, varContactNumber, secondaryPhoneINT, "
                + "billingAddress, varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, bitSendMarketing FROM tbl_customers "
                + "WHERE intCustomerID = @intCustomerID";

            object[][] parms =
            {
                 new object[] { "@intCustomerID", customerID },
            };

            return ConvertFromDataTableToCustomerWithInvoices(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToCustomerWithInvoices(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
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
                    sqlCmd = "SELECT intCustomerID, varFirstName, varLastName, varAddress, varContactNumber, varEmailAddress, "
                        + "varCityName, intProvinceID, intCountryID, varPostalCode, bitSendMarketing FROM tbl_customers WHERE "
                        + "CAST(intCustomerID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, varLastName) LIKE @parm2"
                        + i + " OR CONCAT(varContactNumber, secondaryPhoneINT) LIKE @parm3" + i + " OR varEmailAddress LIKE @parm4" + i + "";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intCustomerID, varFirstName, varLastName, varAddress, varContactNumber, "
                        + "varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, bitSendMarketing FROM "
                        + "tbl_customers WHERE CAST(intCustomerID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, "
                        + "varLastName) LIKE @parm2" + i + " OR CONCAT(varContactNumber, secondaryPhoneINT) LIKE @parm3" + i
                        + " OR varEmailAddress LIKE @parm4" + i + ")";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
            }
            sqlCmd += " ORDER BY varFirstName ASC";

            return ConvertFromDataTableToCustomer(DBC.MakeDataBaseCallToReturnDataTableFromArrayLists(sqlCmd, parms, strText, objPageDetails, strQueryName));
            //return ConvertFromDataTableToCustomer(dbc.returnDataTableDataFromArrayLists(sqlCmd, parms, strText, objPageDetails, strQueryName));
        }
        //Add Customer Nathan and Tyler created. Returns customer ID
        public int addCustomer(Customer customer, object[] objPageDetails)
        {
            string strQueryName = "addCustomer";
            //New command
            string sqlCmd = "INSERT INTO tbl_customers VALUES (@varFirstName, @varLastName, @varAddress, @secondaryAddress, "
                + "@varContactNumber, @secondaryPhoneINT, '', @varEmailAddress, @varCityName, @intProvinceID, "
                + "@intCountryID, @varPostalCode, @bitSendMarketing)";

            object[][] parms =
            {
                new object[] { "@varFirstName", customer.varFirstName },
                new object[] { "@varLastName", customer.varLastName },
                new object[] { "@varAddress", customer.varAddress },
                new object[] { "@secondaryAddress", customer.secondaryAddress },
                new object[] { "@varContactNumber", customer.varContactNumber },
                new object[] { "@secondaryPhoneINT", customer.secondaryPhoneNumber },

                new object[] { "@varEmailAddress", customer.varEmailAddress },
                new object[] { "@varCityName", customer.varCityName },                
                new object[] { "@intProvinceID", customer.intProvinceID },
                new object[] { "@intCountryID", customer.intCountryID },
                new object[] { "@varPostalCode", customer.varPostalCode },
                new object[] { "@bitSendMarketing", customer.bitSendMarketing }
            };

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return ReturnCustomerIDFromCustomerStats(parms, objPageDetails)[0].intCustomerID;
        }
        public void updateCustomer(Customer customer, object[] objPageDetails)
        {
            string strQueryName = "updateCustomer";
            //New command
            string sqlCmd = "UPDATE tbl_customers SET varFirstName = @varFirstName, varLastName = @varLastName, varAddress = @varAddress, "
                + "varContactNumber = @varContactNumber, bitSendMarketing = @bitSendMarketing, varEmailAddress = @varEmailAddress, "
                + "varCityName = @varCityName, intProvinceID = @intProvinceID, intCountryID = @intCountryID, varPostalCode = @varPostalCode "
                + "WHERE intCustomerID = @intCustomerID";

            object[][] parms =
            {
                new object[] { "intCustomerID", customer.intCustomerID },
                new object[] { "varFirstName", customer.varFirstName },
                new object[] { "varLastName", customer.varLastName },
                new object[] { "varAddress", customer.varAddress },
                new object[] { "varContactNumber", customer.varContactNumber },
                new object[] { "bitSendMarketing", customer.bitSendMarketing },
                new object[] { "varEmailAddress", customer.varEmailAddress },
                new object[] { "varCityName", customer.varCityName },
                new object[] { "intProvinceID", customer.intProvinceID },
                new object[] { "intCountryID", customer.intCountryID },
                new object[] { "varPostalCode", customer.varPostalCode }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public List<Customer> ReturnCustomerIDFromCustomerStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnCustomerIDFromCustomerStats";
            string sqlCmd = "SELECT intCustomerID, varFirstName, varLastName, varAddress, secondaryAddress, varContactNumber, secondaryPhoneINT, "
                + "varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, bitSendMarketing FROM tbl_customers WHERE varFirstName "
                + "= @varFirstName AND varLastName = @varLastName AND varAddress = @varAddress AND secondaryAddress = @secondaryAddress AND "
                + "varContactNumber = @varContactNumber AND secondaryPhoneINT = @secondaryPhoneINT AND varEmailAddress = @varEmailAddress AND "
                + "varCityName = @varCityName AND intProvinceID = @intProvinceID AND intCountryID = @intCountryID AND varPostalCode = @varPostalCode "
                + "AND bitSendMarketing = @bitSendMarketing";

            return ConvertFromDataTableToCustomer(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName));
            //return ConvertFromDataTableToCustomer(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName));
        }
    }
}