using SweetSpotDiscountGolfPOS;
using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SweetShop
{
    //The employee manager class is used to get information about an employee or employees
    class EmployeeManager
    {

        //private string connectionString;
        public EmployeeManager()
        {
            //connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        LocationManager LM = new LocationManager();
        DatabaseCalls dbc = new DatabaseCalls();
        private List<Employee> ConvertFromDataTableToEmployee(DataTable dt, object[] objPageDetails)
        {
            List<Employee> employee = dt.AsEnumerable().Select(row =>
            new Employee
            {
                employeeID = row.Field<int>("empID"),
                firstName = row.Field<string>("firstName"),
                lastName = row.Field<string>("lastName"),
                jobID = row.Field<int>("jobID"),
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                emailAddress = row.Field<string>("email"),
                primaryContactNumber = row.Field<string>("primaryContactINT"),
                secondaryContactNumber = row.Field<string>("secondaryContactINT"),
                primaryAddress = row.Field<string>("primaryAddress"),
                secondaryAddress = row.Field<string>("secondaryAddress"),
                city = row.Field<string>("city"),
                provState = row.Field<int>("provStateID"),
                country = row.Field<int>("countryID"),
                postZip = row.Field<string>("postZip")

            }).ToList();
            return employee;
        }
        private List<CurrentUser> ConvertFromDataTableToCurrentUser(DataTable dt, object[] objPageDetails)
        {
            List<CurrentUser> currentUser = dt.AsEnumerable().Select(row =>
            new CurrentUser
            {
                emp = ReturnEmployee(row.Field<int>("empID"), objPageDetails)[0],
                jobID = row.Field<int>("jobID"),
                location = LM.ReturnLocation(row.Field<int>("locationID"), objPageDetails)[0],
                locationName = row.Field<string>("city"),
                password = row.Field<int>("password")
            }).ToList();
            return currentUser;
        }

        //Returns list of custoemrs based on an customer ID
        public List<Employee> ReturnEmployee(int emp, object[] objPageDetails)
        {
            string strQueryName = "ReturnEmployee";
            string sqlCmd = "SELECT empID, firstName, lastName, jobID, locationID, email, "
                + "primaryContactINT, secondaryContactINT, primaryAddress, secondaryAddress, "
                + "city, provStateID, countryID, postZip "
                + "FROM tbl_employee WHERE empID = @empID";

            object[][] parms =
            {
                 new object[] { "@empID", emp },
            };

            List<Employee> employee = ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
            return employee;
        }
        //Returns list of custoemrs based on an search text
        public List<Employee> ReturnEmployeeBasedOnText(string searchText, object[] objPageDetails)
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
                    sqlCmd = "SELECT empID, firstName, lastName, jobID, locationID, email, primaryContactINT, "
                        + "secondaryContactINT, primaryAddress, secondaryAddress, city, provStateID, countryID, "
                        + "postZip FROM tbl_employee WHERE CAST(empID AS VARCHAR) LIKE @parm1" + i
                        + " OR CONCAT(firstName, lastName) LIKE @parm2" + i
                        + " OR CONCAT(primaryContactINT, secondaryContactINT) LIKE @parm3" + i
                        + " OR email LIKE @parm4" + i + "";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT empID, firstName, lastName, jobID, locationID, email, primaryContactINT, "
                        + "secondaryContactINT, primaryAddress, secondaryAddress, city, provStateID, countryID, postZip "
                        + "FROM tbl_employee WHERE CAST(empID AS VARCHAR) LIKE @parm1" + i
                        + " OR CONCAT(firstName, lastName) LIKE @parm2" + i
                        + " OR CONCAT(primaryContactINT, secondaryContactINT) LIKE @parm3" + i
                        + " OR email LIKE @parm4" + i + ")";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
            }
            sqlCmd += " ORDER BY firstName ASC";
            List<Employee> employee = ConvertFromDataTableToEmployee(dbc.returnDataTableDataFromArrayLists(sqlCmd, parms, strText, objPageDetails, strQueryName), objPageDetails);
            return employee;
        }
        public int AddEmployee(Employee em, object[] objPageDetails)
        {
            string strQueryName = "AddEmployee";
            string sqlCmd = "INSERT INTO tbl_employee (firstName, lastName, jobID, locationID, "
                + "email, primaryContactINT, secondaryContactINT, primaryAddress, secondaryAddress, "
                + "city, provStateID, countryID, postZip) VALUES (@firstName, @lastName, @jobID, "
                + "@locationID, @email, @primaryContactINT, @secondaryContactINT, @primaryAddress, "
                + "@secondaryAddress, @city, @provStateID, @countryID, @postZip)";

            object[][] parms =
            {
                new object[] { "@firstName", em.firstName },
                new object[] { "@lastName", em.lastName },
                new object[] { "@jobID", em.jobID },
                new object[] { "@locationID", em.location.locationID },
                new object[] { "@email", em.emailAddress },
                new object[] { "@primaryContactINT", em.primaryContactNumber },
                new object[] { "@secondaryContactINT", em.secondaryContactNumber },
                new object[] { "@primaryAddress", em.primaryAddress },
                new object[] { "@secondaryAddress", em.secondaryAddress },
                new object[] { "@city", em.city },
                new object[] { "@provStateID", em.provState },
                new object[] { "@countryID", em.country },
                new object[] { "@postZip", em.postZip }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            List<Employee> employee = ReturnEmployeeIDFromEmployeeStats(parms, objPageDetails);
            return employee[0].employeeID;
        }
        //Update Employee Nathan and Tyler Created
        public int UpdateEmployee(Employee em, object[] objPageDetails)
        {
            string strQueryName = "UpdateEmployee";
            string sqlCmd = "UPDATE tbl_employee SET firstName = @firstName, "
                + "lastName = @lastName, jobID = @jobID, locationID = @locationID, "
                + "email = @email, primaryContactINT = @primaryContactINT, "
                + "secondaryContactINT = @secondaryContactINT, primaryAddress = @primaryAddress, "
                + "secondaryAddress = @secondaryAddress, city = @city, "
                + "provStateID = @provStateID, countryID = @countryID, "
                + "postZip = @postZip WHERE empID = @empID";

            object[][] parms =
            {
                new object[] { "@empID", em.employeeID },
                new object[] { "@firstName", em.firstName },
                new object[] { "@lastName", em.lastName },
                new object[] { "@jobID", em.jobID },
                new object[] { "@locationID", em.location.locationID },
                new object[] { "@email", em.emailAddress },
                new object[] { "@primaryContactINT", em.primaryContactNumber },
                new object[] { "@secondaryContactINT", em.secondaryContactNumber },
                new object[] { "@primaryAddress", em.primaryAddress },
                new object[] { "@secondaryAddress", em.secondaryAddress },
                new object[] { "@city", em.city },
                new object[] { "@provStateID", em.provState },
                new object[] { "@countryID", em.country },
                new object[] { "@postZip", em.postZip }
            };
            dbc.executeInsertQuery(sqlCmd, parms, objPageDetails, strQueryName);
            return em.employeeID;
        }
        public List<Employee> ReturnEmployeeIDFromEmployeeStats(object[][] parms, object[] objPageDetails)
        {
            string strQueryName = "ReturnEmployeeIDFromEmployeeStats";
            string sqlCmd = "SELECT empID, firstName, lastName, jobID, locationID, "
                + "email, primaryContactINT, secondaryContactINT, primaryAddress, "
                + "secondaryAddress, city, provStateID, countryID, postZip "
                + "FROM tbl_employee WHERE firstName = @firstName AND lastName = @lastName "
                + "AND jobID = @jobID AND locationID = @locationID AND email = @email AND "
                + "primaryContactINT = @primaryContactINT AND secondaryContactINT "
                + "= @secondaryContactINT AND primaryAddress = @primaryAddress AND "
                + "secondaryAddress = @secondaryAddress AND city = @city AND "
                + "provStateID = @provStateID AND countryID = @countryID AND "
                + "postZip = @postZip";
            return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Save new password into user_info
        public bool saveNewPassword(int empID, int pWord, object[] objPageDetails)
        {
            string strQueryName = "saveNewPassword";
            bool bolAdded = false;
            //First check if the password is in use by another user.
            string sqlCmd = "Select empID from tbl_userInfo where password = @pWord";
            object[][] parms =
            {
                new object[] { "@pWord", pWord }
            };

            //Checks to see if the password is already in use
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName) < 0)
            {

                //When password not in use check if the employee is already in the user info table
                sqlCmd = "Select empID from tbl_userInfo where empID = @empID";
                object[][] parms1 =
                {
                    new object [] { "@empID", empID }
                };

                if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms1, objPageDetails, strQueryName) > -10)
                {
                    //Employee is in the userInfo table update password
                    sqlCmd = "Update tbl_userInfo SET password = @pWord Where empID = @empID";
                }
                else
                {
                    //Employee is not in the table add user and password
                    sqlCmd = "Insert Into tbl_userInfo values(@empID, @pWord)";
                }
                object[][] parms2 =
                {
                    new object[] { "@empID", empID },
                    new object[] { "@pWord", pWord }
                };

                dbc.executeInsertQuery(sqlCmd, parms2, objPageDetails, strQueryName);
                bolAdded = true;
            }

            return bolAdded;
        }
        public List<CurrentUser> ReturnCurrentUserFromPassword(string password, object[] objPageDetails)
        {
            string strQueryName = "ReturnCurrentUserFromPassword";
            string sqlCmd = "SELECT E.empID, E.jobID, E.locationID, L.city, U.password "
                + "FROM tbl_employee E JOIN tbl_location L ON E.locationID = L.locationID "
                + "JOIN tbl_userInfo U ON E.empID = U.empID WHERE U.password = @password";
            object[][] parms =
            {
                new object[] { "@password", password }
            };
            return ConvertFromDataTableToCurrentUser(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
        //Password check to complete a Sale
        public bool returnCanEmployeeMakeSale(string empPassword, object[] objPageDetails)
        {
            bool bolValid = false;

            int jobID = ExecuteJobIDCheck(empPassword, objPageDetails);

            if (jobID > 0) { bolValid = true; }
            return bolValid;
        }
        private int ExecuteJobIDCheck(string empPassword, object[] objPageDetails)
        {
            string strQueryName = "ExecuteJobIDCheck";
            string sqlCmd = "SELECT E.jobID FROM tbl_employee E JOIN tbl_userInfo U "
                + "ON E.empID = U.empID WHERE U.password = @password";

            object[][] parms =
            {
                new object[] { "@password", empPassword }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms, objPageDetails, strQueryName);
        }
        public List<Employee> returnEmployeeFromPassword(int empPassword, object[] objPageDetails)
        {
            string strQueryName = "returnEmployeeFromPassword";
            string sqlCmd = "SELECT e.empID, e.firstName, e.lastName, e.jobID, e.locationID, e.email, "
                + "e.primaryContactINT, e.secondaryContactINT, e.primaryAddress, e.secondaryAddress, "
                + "e.city, e.provStateID, e.countryID, e.postZip FROM tbl_employee e JOIN tbl_userInfo u "
                + "ON u.empID = e.empID Where u.password = @empPassword";
            object[][] parms =
            {
                new object[] { "empPassword", empPassword }
            };

            return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms, objPageDetails, strQueryName), objPageDetails);
        }
    }
}
