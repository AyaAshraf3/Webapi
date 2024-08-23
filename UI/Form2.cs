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

namespace UI
{
    public partial class Form2 : Form
    {
        public static string entered_username;
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            entered_username = textBox1.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox1, "");
            errorProvider1.SetError(textBox2, "");
            if (textBox1.Text.ToString().Trim() != "" && textBox2.Text.ToString().Trim() != "")
            {
                GridViewForm gridViewForm = new GridViewForm();
                gridViewForm.Show();
            }
            else if (textBox1.Text.ToString().Trim() == "")
            {
                errorProvider1.SetError(textBox1, "Please enter the user Name");
            }
            else if (textBox2.Text.ToString().Trim() == "")
            {
                errorProvider1.SetError(textBox1, "Please enter the password");
            }

        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Set the maximum length of text in the control to eight.
            textBox2.MaxLength = 6;
            // Assign the asterisk to be the password character.
            textBox2.PasswordChar = '*';
            // Change all text entered to be lowercase.
            textBox2.CharacterCasing = CharacterCasing.Lower;
        }

      

       
    }
}
