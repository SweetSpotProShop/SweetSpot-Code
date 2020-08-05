using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace SweetSpotDiscountGolfPOS.Misc
{
    //This class is used to create a popup message box
    public class MessageBoxCustom
    {
        //This method creates a popup box with an alert based message
        public static void ShowMessage(string MessageText, Page MyPage)
        {
            MyPage.ClientScript.RegisterStartupScript(MyPage.GetType(),
                "MessageBox", "alert('" + MessageText.Replace("'", "\'") + "');", true);
        }
    }
}