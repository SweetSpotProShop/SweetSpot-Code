using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The customer class is used to define what a customer is
    public class Employee
    {
        public int employeeID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int jobID { get; set; }
        public int locationID { get; set; }
        public string emailAddress { get; set; }
        public string primaryContactNumber { get; set; }
        public string secondaryContactNumber { get; set; }
        public string primaryAddress { get; set; }
        public string secondaryAddress { get; set; }
        public string city { get; set; }
        public int provState { get; set; }
        public int country { get; set; }
        public string postZip { get; set; }
        
        public Employee() { }
        public Employee(int ID, string first, string last, int job, int location, string email,
            string pcNumber, string scNumber, string pAddress, string sAddress, string cty, int pState,
            int cntry, string pZip)
        {
            employeeID = ID;
            firstName = first;
            lastName = last;
            jobID = job;
            locationID = location;
            emailAddress = email;
            primaryContactNumber = pcNumber;
            secondaryContactNumber = scNumber;
            primaryAddress = pAddress;
            secondaryAddress = sAddress;
            city = cty;
            provState = pState;
            country = cntry;
            postZip = pZip;
        }
    }
}
