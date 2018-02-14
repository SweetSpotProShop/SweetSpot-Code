using SweetSpotDiscountGolfPOS.ClassLibrary;
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
        public Location location { get; set; }
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
        public Employee(int ID, string First, string Last, int Job, Location Location, string Email,
            string PCNumber, string SCNumber, string PAddress, string SAddress, string City, int PState,
            int Country, string PZip)
        {
            employeeID = ID;
            firstName = First;
            lastName = Last;
            jobID = Job;
            location = Location;
            emailAddress = Email;
            primaryContactNumber = PCNumber;
            secondaryContactNumber = SCNumber;
            primaryAddress = PAddress;
            secondaryAddress = SAddress;
            city = City;
            provState = PState;
            country = Country;
            postZip = PZip;
        }
    }
}
