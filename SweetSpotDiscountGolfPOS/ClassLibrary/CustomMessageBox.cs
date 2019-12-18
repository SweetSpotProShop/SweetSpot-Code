using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Threading;

namespace SweetSpotDiscountGolfPOS.ClassLibrary
{
    //The custom message box class was ment to be used as a way to get user input but javascript is currently being used.

    //Might revisit it at a later point
    public class CustomMessageBox
    {
        public string InputBox(string title, string promptText)
        {
            Form form = new Form();
            System.Windows.Forms.Label label = new System.Windows.Forms.Label();
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
            System.Windows.Forms.Button buttonOk = new System.Windows.Forms.Button();
            form.Text = title;
            label.Text = promptText;
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;
            label.SetBounds(9, 10, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            label.AutoSize = true;
            label.Anchor = AnchorStyles.Left;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk });//Out of memory error caused on this line
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            DialogResult dialogResult = form.ShowDialog();
            return textBox.Text;
        }
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