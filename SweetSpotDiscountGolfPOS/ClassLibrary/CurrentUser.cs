using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //The current user class is used to keep track of the current user's information
    class CurrentUser
    {
        public int empID { get; set; }
        public int password { get; set; }
        public int locationID { get; set; }
        public string locationName { get; set; }
        public int jobID { get; set; }

        public CurrentUser() { }
        public CurrentUser(int e, int j, int l, string ln, int p) {
            empID = e;
            jobID = j;
            locationID = l;
            locationName = ln;
            password = p;
        }
    }
}
