using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The customer class is used to define what an emplpoyee is
    public class Employee
    {
        public int intEmployeeID { get; set; }
        public string varFirstName { get; set; }
        public string varLastName { get; set; }
        public int intJobID { get; set; }
        public Location location { get; set; }
        public string varEmailAddress { get; set; }
        public string varContactNumber { get; set; }
        public string secondaryContactNumber { get; set; }
        public string varAddress { get; set; }
        public string secondaryAddress { get; set; }
        public string varCityName { get; set; }
        public int intProvinceID { get; set; }
        public int intCountryID { get; set; }
        public string varPostalCode { get; set; }
        
        public Employee() { }
    }
}
