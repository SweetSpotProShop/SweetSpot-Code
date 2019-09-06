using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SweetSpotDiscountGolfPOS.ClassLibrary;

namespace SweetShop
{
    //The current user class is used to keep track of the current user's information
    public class CurrentUser
    {
        public Employee employee { get; set; }
        public int intPassword { get; set; }
        public Location location { get; set; }

        public CurrentUser() { }
        //public CurrentUser(Employee e, int j, Location l, string ln, int p) {
        //    emp = e;
        //    jobID = j;
        //    location = l;
        //    locationName = ln;
        //    password = p;
        //}
    }
}
