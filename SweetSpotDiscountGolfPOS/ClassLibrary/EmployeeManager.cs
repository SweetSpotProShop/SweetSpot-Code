using System;
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

        String connectionString;
        public EmployeeManager()
        {

            connectionString = ConfigurationManager.ConnectionStrings["SweetSpotDevConnectionString"].ConnectionString;
        }

        //Retrieves Customer from search parameters Nathan and Tyler created
        public List<Employee> GetEmployeefromSearch(String searchField)
        {
            try
            {
                //Creating a list for the employees
                List<Employee> employee = new List<Employee>();
                //Creating a table to store the results
                DataTable table = new DataTable();
                SqlConnection con = new SqlConnection(connectionString);
                using (var cmd = new SqlCommand("getEmployeeFromSearch", con)) //Calling the SP
                using (var da = new SqlDataAdapter(cmd))
                {
                    //Adding the parameter
                    cmd.Parameters.AddWithValue("@searchField", searchField);
                    //Executing the SP
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Filling the table with what is found
                    da.Fill(table);
                }
                //Looping through the table and creating employees from the rows
                foreach (DataRow row in table.Rows)
                {
                    Employee emp = new Employee(Convert.ToInt32(row["empID"]),
                        row["firstName"].ToString(),
                        row["lastName"].ToString(),
                        Convert.ToInt32(row["jobID"]),
                        Convert.ToInt32(row["locationID"]),
                        row["email"].ToString(),
                        row["primaryContactINT"].ToString(),
                        row["secondaryContactINT"].ToString(),
                        row["primaryAddress"].ToString(),
                        row["secondaryAddress"].ToString(),
                        row["city"].ToString(),
                        Convert.ToInt32(row["provStateID"]),
                        Convert.ToInt32(row["countryID"]),
                        row["postZip"].ToString());
                    employee.Add(emp);
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
                        Convert.ToInt32(row["locationID"]),
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

        //Returns employeeID after adding employee to table created by Nathan
        public int addEmployee(Employee em)
        {
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("insertEmployee", con)) //Calling the SP      
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@firstName", em.firstName);
                cmd.Parameters.AddWithValue("@lastName", em.lastName);
                cmd.Parameters.AddWithValue("@jobID", em.jobID);
                cmd.Parameters.AddWithValue("@locationID", em.locationID);
                cmd.Parameters.AddWithValue("@email", em.emailAddress);
                cmd.Parameters.AddWithValue("@primaryContactINT", em.primaryContactNumber);
                cmd.Parameters.AddWithValue("@secondaryContactINT", em.secondaryContactNumber);
                cmd.Parameters.AddWithValue("@primaryAddress", em.primaryAddress);
                cmd.Parameters.AddWithValue("@secondaryAddress", em.secondaryAddress);
                cmd.Parameters.AddWithValue("@city", em.city);
                cmd.Parameters.AddWithValue("@province", em.provState);
                cmd.Parameters.AddWithValue("@country", em.country);
                cmd.Parameters.AddWithValue("@postalCode", em.postZip);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //Returns the employee ID of the newly created employee
            return returnEmployeeNumber(em);
        }

        //Returns employeeID created by Nathan
        public int returnEmployeeNumber(Employee em)
        {

            //Variable to store the employee ID
            int empID = 0;
            //Creating a table to store the results
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("getEmployeeID", con)) //Calling the SP   
            using (var da = new SqlDataAdapter(cmd))
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@firstName", em.firstName);
                cmd.Parameters.AddWithValue("@lastName", em.lastName);
                cmd.Parameters.AddWithValue("@jobID", em.jobID);
                cmd.Parameters.AddWithValue("@locationID", em.locationID);
                cmd.Parameters.AddWithValue("@email", em.emailAddress);
                cmd.Parameters.AddWithValue("@primaryContactINT", em.primaryContactNumber);
                cmd.Parameters.AddWithValue("@secondaryContactINT", em.secondaryContactNumber);
                cmd.Parameters.AddWithValue("@primaryAddress", em.primaryAddress);
                cmd.Parameters.AddWithValue("@secondaryAddress", em.secondaryAddress);
                cmd.Parameters.AddWithValue("@city", em.city);
                cmd.Parameters.AddWithValue("@province", em.provState);
                cmd.Parameters.AddWithValue("@country", em.country);
                cmd.Parameters.AddWithValue("@postalCode", em.postZip);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);
            }
            foreach (DataRow row in table.Rows)
            {
                empID = Convert.ToInt32(row["empID"]);
            }
            //Returns the employee ID 
            return empID;
        }

        //Update Employee Nathan and Tyler Created
        public void updateEmployee(Employee em)
        {
            SqlConnection con = new SqlConnection(connectionString);
            using (var cmd = new SqlCommand("updateEmployee", con)) //Calling the SP      
            {
                //Adding the parameter
                cmd.Parameters.AddWithValue("@employeeID", em.employeeID);
                cmd.Parameters.AddWithValue("@firstName", em.firstName);
                cmd.Parameters.AddWithValue("@lastName", em.lastName);
                cmd.Parameters.AddWithValue("@jobID", em.jobID);
                cmd.Parameters.AddWithValue("@locationID", em.locationID);
                cmd.Parameters.AddWithValue("@email", em.emailAddress);
                cmd.Parameters.AddWithValue("@primaryContactINT", em.primaryContactNumber);
                cmd.Parameters.AddWithValue("@secondaryContactINT", em.secondaryContactNumber);
                cmd.Parameters.AddWithValue("@primaryAddress", em.primaryAddress);
                cmd.Parameters.AddWithValue("@secondaryAddress", em.secondaryAddress);
                cmd.Parameters.AddWithValue("@city", em.city);
                cmd.Parameters.AddWithValue("@province", em.provState);
                cmd.Parameters.AddWithValue("@country", em.country);
                cmd.Parameters.AddWithValue("@postalCode", em.postZip);
                //Executing the SP
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        //This method returns the jobID of a given job
        public int jobType(string jobName)
        {

            //Variable to store the employee ID
            int job = 0;
            //Creating a table to store the results
            DataTable table = new DataTable();
            SqlConnection con = new SqlConnection(connectionString);
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
            SqlConnection con = new SqlConnection(connectionString);
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
        //Save new password into user_info
        public bool saveNewPassword(int empID, int pWord)
        {
            bool bolAdded = false;
            //First check if the password is in use by another user.
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select empID from tbl_userInfo where password = @pWord";
            cmd.Parameters.AddWithValue("pWord", pWord);
            conn.Open();
            SqlDataReader pWordUsedReader = cmd.ExecuteReader();
            //Checks to see if the password is already in use
            if (!pWordUsedReader.HasRows)
            {
                conn.Close();
                //When password not in use check if the employee is already in the user info table
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn;
                cmd2.CommandText = "Select empID from tbl_userInfo where empID = @empID";
                cmd2.Parameters.AddWithValue("empID", empID);
                conn.Open();
                SqlDataReader empInTableReader = cmd2.ExecuteReader();

                SqlCommand cmdEmpPass = new SqlCommand();
                cmdEmpPass.Connection = conn;
                if (empInTableReader.HasRows)
                {
                    //Employee is in the userInfo table update password
                    cmdEmpPass.CommandText = "Update tbl_userInfo SET password = @pWord Where empID = @empID";
                }
                else
                {
                    //Employee is not in the table add user and password
                    cmdEmpPass.CommandText = "Insert Into tbl_userInfo values(@empID, @pWord)";
                }
                cmdEmpPass.Parameters.AddWithValue("empID", empID);
                cmdEmpPass.Parameters.AddWithValue("pWord", pWord);
                conn.Close();
                conn.Open();
                cmdEmpPass.ExecuteNonQuery();
                bolAdded = true;
            }
            conn.Close();
            return bolAdded;
        }

        public DataTable returnProvinceDropDown(int countryID)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT provStateID, provName FROM tbl_provState WHERE countryID = @countryID ORDER BY provName";
            cmd.Parameters.AddWithValue("countryID", countryID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public int returnEmployeeIDFromPassword(int empPassword)
        {
            int empID = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT empID FROM tbl_userInfo Where password = @empPassword";
            cmd.Parameters.AddWithValue("empPassword", empPassword);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                empID = Convert.ToInt32(reader["empID"]);
            }
            conn.Close();
            return empID;
        }
        public bool returnCanEmployeeMakeSale(int empID)
        {
            bool bolValid = false;
            int jobID = 0;
            if (empID != 0)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT jobID FROM tbl_employee Where empID = @empID";
                cmd.Parameters.AddWithValue("empID", empID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jobID = Convert.ToInt32(reader["jobID"]);
                }
                conn.Close();
                if(jobID !=-0) { bolValid = true; }
            }
            return bolValid;
        }

    }
}
