using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetSpotDiscountGolfPOS.OB
{
    //The current user class is used to keep track of the current user's information
    public class CurrentUser
    {
        public Employee employee { get; set; }
        public int intPassword { get; set; }
        public Location location { get; set; }
        public bool isSimEditMode { get; set; }

        public CurrentUser() { }
    }
}
