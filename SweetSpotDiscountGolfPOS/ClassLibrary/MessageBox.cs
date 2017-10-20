using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace SweetSpotProShop
{
    //This class is used to create a popup message box
    public class MessageBox
    {
        //This method creates a popup box with an alert based message
        public static void ShowMessage(string MessageText, Page MyPage)
        {
            MyPage.ClientScript.RegisterStartupScript(MyPage.GetType(),
                "MessageBox", "alert('" + MessageText.Replace("'", "\'") + "');", true);
        }
        //This method was to be used to get user input. Javascript is being used instead
        public string inputBoxV2(string title, string promptText)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = title;
            System.Windows.Forms.Label textLabel = new System.Windows.Forms.Label() { Left = 50, Top = 20, Text = promptText };
            System.Windows.Forms.TextBox inputBox = new System.Windows.Forms.TextBox() { Left = 50, Top = 50, Width = 400 };
            System.Windows.Forms.Button confirmation = new System.Windows.Forms.Button() { Text = "Ok", Left = 350, Width = 100, Top = 70 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.ShowDialog();
            return inputBox.Text;
        }

    }
}