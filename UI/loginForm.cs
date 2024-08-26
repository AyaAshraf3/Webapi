using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace OMS
{
    public partial class loginForm : Form
    {
        public static string entered_username;
        public loginForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            entered_username = txtUserName.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            errorChecking(3,"");
            if (txtUserName.Text.ToString().Trim() != "" && txtPass.Text.ToString().Trim() != "")
            {
                ordersGrid gridViewForm = new ordersGrid();
                gridViewForm.Show();
            }
            else emptyErrorCheck();

        }

        private void emptyErrorCheck()
        {
            if (txtUserName.Text.ToString().Trim() == "")
            {
                errorChecking(1, "Please enter the user Name");
            }
            else if (txtPass.Text.ToString().Trim() == "")
            {
                errorChecking(2, "Please enter the password");
            }
        }

        private void errorChecking(int txtBoxNum, string errorMessage)
        {
            switch (txtBoxNum)
            {
                case 1:
                    errorProvider1.SetError(txtUserName, errorMessage);
                    break;
                case 2:
                    errorProvider1.SetError(txtPass, errorMessage);
                    break;
                case 3:
                    errorProvider1.SetError(txtUserName, errorMessage);
                    errorProvider1.SetError(txtPass, errorMessage);
                    break;
            }
        }

  

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Set the maximum length of text in the control to eight.
            txtPass.MaxLength = 6;
            // Assign the asterisk to be the password character.
            txtPass.PasswordChar = '*';
            // Change all text entered to be lowercase.
            txtPass.CharacterCasing = CharacterCasing.Lower;
        }

       
    }
}
