using SweetSpotDiscountGolfPOS.OB;
using SweetSpotDiscountGolfPOS.Misc;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SweetSpotDiscountGolfPOS.FP
{
    //The employee manager class is used to get information about an employee or employees
    class EmployeeManager
    {
        readonly LocationManager LM = new LocationManager();
        readonly DatabaseCalls DBC = new DatabaseCalls();
        public EmployeeManager() { }

        //Converters
        private List<Employee> ConvertFromDataTableToEmployee(DataTable dt, object[] objPageDetails)
        {
            List<Employee> employee = dt.AsEnumerable().Select(row =>
            new Employee
            {
                intEmployeeID = row.Field<int>("intEmployeeID"),
                varFirstName = row.Field<string>("varFirstName"),
                varLastName = row.Field<string>("varLastName"),
                intJobID = row.Field<int>("intJobID"),
                //location = LM.CallReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],
                intLocationID = row.Field<int>("intLocationID"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varContactNumber = row.Field<string>("varContactNumber"),
                secondaryContactNumber = row.Field<string>("secondaryContactINT"),
                varAddress = row.Field<string>("varAddress"),
                secondaryAddress = row.Field<string>("secondaryAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode")
            }).ToList();
            return employee;
        }
        private List<CurrentUser> ConvertFromDataTableToCurrentUser(DataTable dt, object[] objPageDetails)
        {
            List<CurrentUser> currentUser = dt.AsEnumerable().Select(row =>
            new CurrentUser
            {
                employee = ReturnEmployee(row.Field<int>("intEmployeeID"), objPageDetails)[0],                
                location = LM.CallReturnLocation(row.Field<int>("intLocationID"), objPageDetails)[0],                
                intPassword = row.Field<int>("intUserPassword"),
                isSimEditMode = false
            }).ToList();
            return currentUser;
        }


        //DB calls
        //Returns list of custoemrs based on an customer ID
        private List<Employee> ReturnEmployee(int employeeID, object[] objPageDetails)
        {
            string strQueryName = "ReturnEmployee";
            string sqlCmd = "SELECT intEmployeeID, varFirstName, varLastName, intJobID, intLocationID, varEmailAddress, "
                + "varContactNumber, secondaryContactINT, varAddress, secondaryAddress, varCityName, intProvinceID, "
                + "intCountryID, varPostalCode FROM tbl_employee WHERE intEmployeeID = @intEmployeeID";

            object[][] parms =
            {
                 new object[] { "@intEmployeeID", employeeID },
            };
            return ConvertFromDataTableToEmployee(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Returns list of custoemrs based on an search text
        private List<Employee> ReturnEmployeeBasedOnText(string searchText, object[] objPageDetails)
        {
            string strQueryName = "ReturnEmployeeBasedOnText";
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    sqlCmd = "SELECT intEmployeeID, varFirstName, varLastName, intJobID, intLocationID, varEmailAddress, varContactNumber, "
                        + "secondaryContactINT, varAddress, secondaryAddress, varCityName, intProvinceID, intCountryID, varPostalCode FROM "
                        + "tbl_employee WHERE CAST(intEmployeeID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, varLastName) LIKE "
                        + "@parm2" + i + " OR CONCAT(varContactNumber, secondaryContactINT) LIKE @parm3" + i + " OR varEmailAddress LIKE "
                        + "@parm4" + i + "";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intEmployeeID, varFirstName, varLastName, intJobID, intLocationID, varEmailAddress, "
                        + "varContactNumber, secondaryContactINT, varAddress, secondaryAddress, varCityName, intProvinceID, intCountryID, "
                        + "varPostalCode FROM tbl_employee WHERE CAST(intEmployeeID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, "
                        + "varLastName) LIKE @parm2" + i + " OR CONCAT(varContactNumber, secondaryContactINT) LIKE @parm3" + i + " OR "
                        + "varEmailAddress LIKE @parm4" + i + ")";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
            }
            sqlCmd += " ORDER BY varFirstName ASC";

            return ConvertFromDataTableToEmployee(DBC.MakeDataBaseCallToReturnDataTableFromArrayLists(sqlCmd, parms, strText, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToEmployee(dbc.returnDataTableDataFromArrayLists(sqlCmd, parms, strText, objPageDetails, strQueryName), objPageDetails);
        }
        private int AddEmployee(Employee employee, object[] objPageDetails)
        {
            string strQueryName = "AddEmployee";
            string sqlCmd = "INSERT INTO tbl_employee VALUES(@varFirstName, @varLastName, @intJobID, @intLocationID, @varEmailAddress, "
                + "@varContactNumber, @secondaryContactINT, @varAddress, @secondaryAddress, @varCityName, @intProvinceID, @intCountryID, "
                + "@varPostalCode)";

            object[][] parms =
            {
                new object[] { "@varFirstName", employee.varFirstName },
                new object[] { "@varLastName", employee.varLastName },
                new object[] { "@intJobID", employee.intJobID },
                new object[] { "@intLocationID", employee.intLocationID },
                new object[] { "@varEmailAddress", employee.varEmailAddress },
                new object[] { "@varContactNumber", employee.varContactNumber },
                new object[] { "@secondaryContactINT", employee.secondaryContactNumber },
                new object[] { "@varAddress", employee.varAddress },
                new object[] { "@secondaryAddress", employee.secondaryAddress },
                new object[] { "@varCityName", employee.varCityName },
                new object[] { "@intProvinceID", employee.intProvinceID },
                new object[] { "@intCountryID", employee.intCountryID },
                new object[] { "@varPostalCode", employee.varPostalCode }
            };
            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);

            return ReturnEmployeeIDFromEmployeeStats(parms, objPageDetails)[0].intEmployeeID;
        }
        //Update Employee Nathan and Tyler Created
        private int UpdateEmployee(Employee employee, object[] objPageDetails)
        {
            string strQueryName = "UpdateEmployee";
            string sqlCmd = "UPDATE tbl_employee SET varFirstName = @varFirstName, varLastName = @varLastName, intJobID = @intJobID, "
                + "intLocationID = @intLocationID, varEmailAddress = @varEmailAddress, varContactNumber = @varContactNumber, "
                + "secondaryContactINT = @secondaryContactINT, varAddress = @varAddress, secondaryAddress = @secondaryAddress, "
                + "varCityName = @varCityName, intProvinceID = @intProvinceID, intCountryID = @intCountryID, varPostalCode = "
                + "@varPostalCode WHERE intEmployeeID = @intEmployeeID";

            object[][] parms =
            {
                new object[] { "@intEmployeeID", employee.intEmployeeID },
                new object[] { "@varFirstName", employee.varFirstName },
                new object[] { "@varLastName", employee.varLastName },
                new object[] { "@intJobID", employee.intJobID },
                new object[] { "@intLocationID", employee.intLocationID },
                new object[] { "@varEmailAddress", employee.varEmailAddress },
                new object[] { "@varContactNumber", employee.varContactNumber },
                new object[] { "@secondaryContactINT", employee.secondaryContactNumber },
                new object[] { "@varAddress", employee.varAddress },
                new object[] { "@secondaryAddress", employee.secondaryAddress },
                new object[] { "@varCityName", employee.varCityName },
                new object[] { "@intProvinceID", employee.intProvinceID },
                new object[] { "@intCountryID", employee.intCountryID },
                new object[] { "@varPostalCode", employee.varPostalCode }
            };

            DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms, objPageDetails, strQueryName);
            //dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return employee.intEmployeeID;
        }
        private List<Employee> ReturnEmployeeIDFromEmployeeStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnEmployeeIDFromEmployeeStats";
            string sqlCmd = "SELECT intEmployeeID, varFirstName, varLastName, intJobID, intLocationID, varEmailAddress, varContactNumber, "
                + "secondaryContactINT, varAddress, secondaryAddress, varCityName, intProvinceID, intCountryID, varPostalCode FROM "
                + "tbl_employee WHERE varFirstName = @varFirstName AND varLastName = @varLastName AND intJobID = @intJobID AND intLocationID "
                + "= @intLocationID AND varEmailAddress = @varEmailAddress AND varContactNumber = @varContactNumber AND secondaryContactINT "
                + "= @secondaryContactINT AND varAddress = @varAddress AND secondaryAddress = @secondaryAddress AND varCityName = "
                + "@varCityName AND intProvinceID = @intProvinceID AND intCountryID = @intCountryID AND varPostalCode = @varPostalCode";

            return ConvertFromDataTableToEmployee(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Save new password into user_info
        private bool SaveNewPassword(int employeeID, int pWord, object[] objPageDetails)
        {
            string strQueryName = "saveNewPassword";
            bool bolAdded = false;
            //First check if the password is in use by another user.
            string sqlCmd = "SELECT intEmployeeID FROM tbl_userInfo WHERE intUserPassword = @intUserPassword";
            object[][] parms =
            {
                new object[] { "@intUserPassword", pWord }
            };

            //Checks to see if the password is already in use
            if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) < 0)
            //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) < 0)
            {

                //When password not in use check if the employee is already in the user info table
                sqlCmd = "SELECT intEmployeeID FROM tbl_userInfo WHERE intEmployeeID = @intEmployeeID";
                object[][] parms1 =
                {
                    new object [] { "@intEmployeeID", employeeID }
                };

                if (DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms1, objPageDetails, strQueryName) > -10)
                //if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms1, objPageDetails, strQueryName) > -10)
                {
                    //Employee is in the userInfo table update password
                    sqlCmd = "UPDATE tbl_userInfo SET intUserPassword = @intUserPassword WHERE intEmployeeID = @intEmployeeID";
                }
                else
                {
                    //Employee is not in the table add user and password
                    sqlCmd = "INSERT INTO tbl_userInfo VALUES(@intEmployeeID, @intUserPassword)";
                }
                object[][] parms2 =
                {
                    new object[] { "@intEmployeeID", employeeID },
                    new object[] { "@intUserPassword", pWord }
                };

                DBC.MakeDataBaseCallToNonReturnDataQuery(sqlCmd, parms2, objPageDetails, strQueryName);
                //dbc.executeInsertQuery(sqlCmd, parms2, objPageDetails, strQueryName);
                bolAdded = true;
            }
            return bolAdded;
        }
        private List<CurrentUser> ReturnCurrentUserFromPassword(string password, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentUserFromPassword";
            string sqlCmd = "SELECT E.intEmployeeID, E.intJobID, E.intLocationID, L.varCityName, U.intUserPassword "
                + "FROM tbl_employee E JOIN tbl_location L ON E.intLocationID = L.intLocationID "
                + "JOIN tbl_userInfo U ON E.intEmployeeID = U.intEmployeeID WHERE U.intUserPassword = @intUserPassword";
            object[][] parms =
            {
                new object[] { "@intUserPassword", password }
            };
            return ConvertFromDataTableToCurrentUser(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            //return ConvertFromDataTableToCurrentUser(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        private int ExecuteJobIDCheck(int empPassword, object[] objPageDetails)
        {
            string strQueryName = "ExecuteJobIDCheck";
            string sqlCmd = "SELECT E.intJobID FROM tbl_employee E JOIN tbl_userInfo U ON E.intEmployeeID = U.intEmployeeID "
                + "WHERE U.intUserPassword = @intUserPassword";
            object[][] parms =
            {
                new object[] { "@intUserPassword", empPassword }
            };
            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        private int ReturnEmployeeFromPassword(int empPassword, object[] objPageDetails)
        {
            string strQueryName = "returnEmployeeFromPassword";
            string sqlCmd = "SELECT E.intEmployeeID FROM tbl_employee E JOIN tbl_userInfo U ON U.intEmployeeID = E.intEmployeeID "
                + "WHERE intUserPassword = @intUserPassword";
            object[][] parms =
            {
                new object[] { "intUserPassword", empPassword }
            };

            return DBC.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
            //return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //private List<Employee> ReturnEmployeeFromPassword(int empPassword, object[] objPageDetails)
        //{
        //    string strQueryName = "returnEmployeeFromPassword";
        //    string sqlCmd = "SELECT E.intEmployeeID, varFirstName, varLastName, intJobID, intLocationID, varEmailAddress, "
        //        + "varContactNumber, secondaryContactINT, varAddress, secondaryAddress, varCityName, intProvinceID, "
        //        + "intCountryID, varPostalCode FROM tbl_employee E JOIN tbl_userInfo U ON U.intEmployeeID = E.intEmployeeID "
        //        + "WHERE intUserPassword = @intUserPassword";
        //    object[][] parms =
        //    {
        //        new object[] { "intUserPassword", empPassword }
        //    };

        //    return ConvertFromDataTableToEmployee(DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        //    //return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        //}
        private DataTable ReturnJobPosition(object[] objPageDetails)
        {
            string strQueryName = "ReturnJobPosition";
            string sqlCmd = "SELECT intJobID, varJobTitle FROM tbl_jobPosition ORDER BY varJobTitle";
            object[][] parms = { };

            return DBC.MakeDataBaseCallToReturnDataTable(sqlCmd, parms, objPageDetails, strQueryName);
            //return dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName);
        }





        //Public calls
        //Password check to complete a Sale
        public bool ReturnCanEmployeeMakeSale(int empPassword, object[] objPageDetails)
        {
            bool bolValid = false;

            int jobID = ExecuteJobIDCheck(empPassword, objPageDetails);

            if (jobID > 0) { bolValid = true; }
            return bolValid;
        }
        public List<Employee> CallReturnEmployee(int employeeID, object[] objPageDetails)
        {
            return ReturnEmployee(employeeID, objPageDetails);
        }
        public List<Employee> CallReturnEmployeeBasedOnText(string searchText, object[] objPageDetails)
        {
            return ReturnEmployeeBasedOnText(searchText, objPageDetails);
        }
        public int CallReturnEmployeeFromPassword(int empPassword, object[] objPageDetails)
        {
            return ReturnEmployeeFromPassword(empPassword, objPageDetails);
        }
        //public List<Employee> CallReturnEmployeeFromPassword(int empPassword, object[] objPageDetails)
        //{
        //    return ReturnEmployeeFromPassword(empPassword, objPageDetails);
        //}
        public DataTable CallReturnJobPosition(object[] objPageDetails)
        {
            return ReturnJobPosition(objPageDetails);
        }
        public int CallAddEmployee(Employee employee, object[] objPageDetails)
        {
            return AddEmployee(employee, objPageDetails);
        }
        public int CallUpdateEmployee(Employee employee, object[] objPageDetails)
        {
            return UpdateEmployee(employee, objPageDetails);
        }
        public bool CallSaveNewPassword(int employeeID, int pWord, object[] objPageDetails)
        {
            return SaveNewPassword(employeeID, pWord, objPageDetails);
        }
        public List<CurrentUser> CallReturnCurrentUserFromPassword(string password, object[] objPageDetails)
        {
            return ReturnCurrentUserFromPassword(password, objPageDetails);
        }
    }
}
