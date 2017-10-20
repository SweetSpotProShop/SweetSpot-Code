using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetShop
{
    //Used to define and create user info
    class tbl_userinfo
    {
        public int empid { get; set; }
        public int password { get; set; }


        public tbl_userinfo() { }

        public tbl_userinfo(int Empid, int Password)
        {
            empid = Empid;
            password = Password;
        }

    }
}
