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

        private string connectionString;
        public EmployeeManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        LocationManager LM = new LocationManager();
        DatabaseCalls dbc = new DatabaseCalls();
        private List<Employee> ConvertFromDataTableToEmployee(DataTable dt)
        {
            List<Employee> employee = dt.AsEnumerable().Select(row =>
            new Employee
            {
                employeeID = row.Field<int>("empID"),
                firstName = row.Field<string>("firstName"),
                lastName = row.Field<string>("lastName"),
                jobID = row.Field<int>("jobID"),
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
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
        private List<CurrentUser> ConvertFromDataTableToCurrentUser(DataTable dt)
        {
            List<CurrentUser> currentUser = dt.AsEnumerable().Select(row =>
            new CurrentUser
            {
                emp = ReturnEmployee(row.Field<int>("empID"))[0],
                jobID = row.Field<int>("jobID"),
                location = LM.ReturnLocation(row.Field<int>("locationID"))[0],
                locationName = row.Field<string>("city"),
                password = row.Field<int>("password")
            }).ToList();
            return currentUser;
        }

        //Returns list of custoemrs based on an customer ID
        public List<Employee> ReturnEmployee(int emp)
        {
            string sqlCmd = "SELECT empID, firstName, lastName, jobID, locationID, email, "
                + "primaryContactINT, secondaryContactINT, primaryAddress, secondaryAddress, "
                + "city, provStateID, countryID, postZip "
                + "FROM tbl_employee WHERE empID = @empID";

            Object[][] parms =
            {
                 new object[] { "@empID", emp },
            };

            List<Employee> employee = ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms));
            return employee;
        }
        //Returns list of custoemrs based on an search text
        public List<Employee> ReturnEmployeeBasedOnText(string searchText)
        {
            ArrayList strText = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add(searchText.Split(' ')[i]);
                if (i == 0)
                {
                    sqlCmd = "SELECT empID, firstName, lastName, jobID, locationID, email, primaryContactINT, "
                        + "secondaryContactINT, primaryAddress, secondaryAddress, city, provStateID, countryID, "
                        + "postZip FROM tbl_employee WHERE CAST(empID AS VARCHAR) LIKE '%" + strText[i]
                        + "%' OR CONCAT(firstName, lastName) LIKE '%" + strText[i]
                        + "%' OR CONCAT(primaryContactINT, secondaryContactINT) LIKE '%" + strText[i]
                        + "%' OR email LIKE '%" + strText[i] + "%'";
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT empID, firstName, lastName, jobID, locationID, email, primaryContactINT, "
                        + "secondaryContactINT, primaryAddress, secondaryAddress, city, provStateID, countryID, postZip "
                        + "FROM tbl_employee WHERE CAST(empID AS VARCHAR) LIKE '%" + strText[i]
                        + "%' OR CONCAT(firstName, lastName) LIKE '%" + strText[i]
                        + "%' OR CONCAT(primaryContactINT, secondaryContactINT) LIKE '%" + strText[i]
                        + "%' OR email LIKE '%" + strText[i] + "%')";
                }
            }
            sqlCmd += " order by firstName asc";
            List<Employee> employee = ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd));
            return employee;
        }
        public int AddEmployee(Employee em)
        {
            string sqlCmd = "INSERT INTO tbl_employee (firstName, lastName, jobID, locationID, "
                + "email, primaryContactINT, secondaryContactINT, primaryAddress, secondaryAddress, "
                + "city, provStateID, countryID, postZip) VALUES (@firstName, @lastName, @jobID, "
                + "@locationID, @email, @primaryContactINT, @secondaryContactINT, @primaryAddress, "
                + "@secondaryAddress, @city, @provStateID, @countryID, @postZip)";

            Object[][] parms =
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
            dbc.executeInsertQuery(sqlCmd, parms);
            List<Employee> employee = ReturnEmployeeIDFromEmployeeStats(parms);
            return employee[0].employeeID;
        }
        //Update Employee Nathan and Tyler Created
        public int UpdateEmployee(Employee em)
        {
            string sqlCmd = "UPDATE tbl_employee SET firstName = @firstName, "
                + "lastName = @lastName, jobID = @jobID, locationID = @locationID, "
                + "email = @email, primaryContactINT = @primaryContactINT, "
                + "secondaryContactINT = @secondaryContactINT, primaryAddress = @primaryAddress, "
                + "secondaryAddress = @secondaryAddress, city = @city, "
                + "provStateID = @provStateID, countryID = @countryID, "
                + "postZip = @postZip WHERE empID = @empID";

            Object[][] parms =
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
            dbc.executeInsertQuery(sqlCmd, parms);
            return em.employeeID;
        }
        public List<Employee> ReturnEmployeeIDFromEmployeeStats(object[][] parms)
        {
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
            return ConvertFromDataTableToEmployee(dbc.returnDataTableData(sqlCmd, parms));
        }
        //Save new password into user_info
        public bool saveNewPassword(int empID, int pWord)
        {
            bool bolAdded = false;
            //First check if the password is in use by another user.
            string sqlCmd = "Select empID from tbl_userInfo where password = @pWord";
            Object[][] parms =
            {
                new object[] { "@pWord", pWord }
            };

            //Checks to see if the password is already in use
            if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms) < 0)
            {

                //When password not in use check if the employee is already in the user info table
                sqlCmd = "Select empID from tbl_userInfo where empID = @empID";
                Object[][] parms1 =
                {
                    new object [] { "@empID", empID }
                };

                if (dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms1) > -10)
                {
                    //Employee is in the userInfo table update password
                    sqlCmd = "Update tbl_userInfo SET password = @pWord Where empID = @empID";
                }
                else
                {
                    //Employee is not in the table add user and password
                    sqlCmd = "Insert Into tbl_userInfo values(@empID, @pWord)";
                }
                Object[][] parms2 =
                {
                    new object[] { "@empID", empID },
                    new object[] { "@pWord", pWord }
                };

                dbc.executeInsertQuery(sqlCmd, parms2);
                bolAdded = true;
            }

            return bolAdded;
        }
        public List<CurrentUser> ReturnCurrentUserFromPassword(string password)
        {
            string sqlCmd = "SELECT E.empID, E.jobID, E.locationID, L.city, U.password "
                + "FROM tbl_employee E JOIN tbl_location L ON E.locationID = L.locationID "
                + "JOIN tbl_userInfo U ON E.empID = U.empID WHERE U.password = @password";
            object[][] parms =
            {
                new object[] { "@password", password }
            };
            return ConvertFromDataTableToCurrentUser(dbc.returnDataTableData(sqlCmd, parms));
        }
        //Password check to complete a Sale
        public bool returnCanEmployeeMakeSale(string empPassword)
        {
            bool bolValid = false;

            int jobID = ExecuteJobIDCheck(empPassword);

            if (jobID > 0) { bolValid = true; }
            return bolValid;
        }
        private int ExecuteJobIDCheck(string empPassword)
        {
            string sqlCmd = "SELECT E.jobID FROM tbl_employee E JOIN tbl_userInfo U "
                + "ON E.empID = U.empID WHERE U.password = @password";

            object[][] parms =
            {
                new object[] { "@password", empPassword }
            };
            return dbc.MakeDataBaseCallToReturnInt(sqlCmd, parms);
        }


        //Retrieves Employee from search parameters Nathan and Tyler created
        public List<Employee> GetEmployeefromSearch(string searchField)
        {
            try
            {
                //Creating a list for the employees
                List<Employee> employee = new List<Employee>();
                //Creating a table to store the results
                DataTable table = new DataTable();
                //SqlConnection con = new SqlConnection(connectionString);
                //using (var cmd = new SqlCommand("getEmployeeFromSearch", con)) //Calling the SP
                //using (var da = new SqlDataAdapter(cmd))
                {
                    //Adding the parameter
                    //cmd.Parameters.AddWithValue("@searchField", searchField);
                    //Executing the SP
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //Filling the table with what is found
                    //da.Fill(table);
                }
                //Looping through the table and creating employees from the rows
                foreach (DataRow row in table.Rows)
                {
                    //Employee emp = new Employee(Convert.ToInt32(row["empID"]),
                    //    row["firstName"].ToString(),
                    //    row["lastName"].ToString(),
                    //    Convert.ToInt32(row["jobID"]),
                    //    Convert.ToInt32(row["locationID"]),
                    //    row["email"].ToString(),
                    //    row["primaryContactINT"].ToString(),
                    //    row["secondaryContactINT"].ToString(),
                    //    row["primaryAddress"].ToString(),
                    //    row["secondaryAddress"].ToString(),
                    //    row["city"].ToString(),
                    //    Convert.ToInt32(row["provStateID"]),
                    //    Convert.ToInt32(row["countryID"]),
                    //    row["postZip"].ToString());
                    //employee.Add(emp);
                }
                //Returns a full employee
                return employee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        //This method returns an employee based on a given employee ID
        public Employee getEmployeeByID(int empID)
        {
            try
            {
                //New Employee
                Employee employee = new Employee();
                //Creating a table to store the results
                DataTable table = new DataTable();
                SqlConnection con = new SqlConnection(connectionString);
                using (var cmd = new SqlCommand("getEmployeeByID", con)) //Calling the SP
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Adding the parameter
                    cmd.Parameters.AddWithValue("@empID", empID);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Filling the table with what is found
                    da.Fill(table);
                }
                //Looping through the table and creating employees from the rows
                foreach (DataRow row in table.Rows)
                {
                    Employee em = new Employee(Convert.ToInt32(row["empID"]),
                        row["firstName"].ToString(),
                        row["lastName"].ToString(),
                        Convert.ToInt32(row["jobID"]),
                        LM.ReturnLocation(Convert.ToInt32(row["locationID"]))[0],
                        row["email"].ToString(),
                        row["primaryContactINT"].ToString(),
                        row["secondaryContactINT"].ToString(),
                        row["primaryAddress"].ToString(),
                        row["secondaryAddress"].ToString(),
                        row["city"].ToString(),
                        Convert.ToInt32(row["provStateID"]),
                        Convert.ToInt32(row["countryID"]),
                        row["postZip"].ToString());
                    employee = em;
                }
                //Returns a full employee
                return employee;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        //This method returns the jobID of a given job
        public int jobType(string jobName)
        {

            //Variable to store the employee ID
            int job = 0;
            //Creating a table to store the results
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection("");// connectionString);
            using (var cmd = new SqlCommand("getJobID", con)) //Calling the SP   
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@jobName", jobName);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                job = Convert.ToInt32(row["jobID"]);
            }
            //Returns the job ID
            return job;
        }
        //Returns the job name when given a job ID
        public string jobName(int jobNum)
        {
            //Variable to store the employee ID
            string job = "";
            //Creating a table to store the results
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection("");// connectionString);
            using (var cmd = new SqlCommand("getJobName", con)) //Calling the SP   
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@jobID", jobNum);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                job = row["title"].ToString();
            }
            //Returns the job name
            return job;
        }
        //public int returnEmployeeIDFromPassword(int empPassword)
        //{
        //    int empID = 0;
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "SELECT empID FROM tbl_userInfo Where password = @empPassword";
        //    cmd.Parameters.AddWithValue("empPassword", empPassword);
        //    conn.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        empID = Convert.ToInt32(reader["empID"]);
        //    }
        //    conn.Close();
        //    return empID;
        //}
    }
}
