using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.SignalR.Client;
using static UI.Form2;


namespace UI
{
    public partial class Form1 : Form
    {
        private bool _isSubmitting = false; // Flag to track submission state

        public class Submit
        {
            public Guid Clordid { get; set; }
            public string Username { get; set; }
            public int Qty { get; set; }
            public decimal Px { get; set; }
            public string Dir { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            
        }
        

        private async void button1_Click(object sender, EventArgs e)
        {
            if (_isSubmitting) return; // Prevent multiple submissions
            _isSubmitting = true; // Set flag to indicate submission is in progress

            try
            {
                
                errorProvider1.SetError(textBox2, "");
                errorProvider1.SetError(textBox3, "");
                errorProvider1.SetError(textBox4, "");

                if ( textBox2.Text.ToString().Trim() != "" && textBox3.Text.ToString().Trim() != "" && textBox4.Text.ToString().Trim() != "")
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:7254");
                        Submit sbmt = new Submit
                        {
                            Username = Form2.entered_username,
                            Qty = Convert.ToInt32(textBox2.Text),
                            Px = Convert.ToDecimal(textBox3.Text),
                            Dir = textBox4.Text.ToString()
                        };

                        // Use asynchronous methods for HTTP requests
                        var response = await client.PostAsJsonAsync("api/Values", sbmt);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (responseContent.Trim() == "0")
                        {
                            MessageBox.Show("Failed Submit.");
                        }
                        else
                        {
                            MessageBox.Show("Submit successfully.");
                            
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";

                            this.Close();
                        }
                    }
                }
                else if ( textBox2.Text.ToString().Trim() == "" && textBox3.Text.ToString().Trim() == "" && textBox4.Text.ToString().Trim() == "")
                {
                    errorProvider1.SetError(textBox2, "Please enter the following information");
                }
                else if (textBox2.Text.ToString().Trim() == "")
                {
                    errorProvider1.SetError(textBox2, "Please enter the Quantity");
                }
                else if (textBox3.Text.ToString().Trim() == "")
                {
                    errorProvider1.SetError(textBox3, "Please enter the Price");
                }
                else if (textBox4.Text.ToString().Trim() == "")
                {
                    errorProvider1.SetError(textBox4, "Please choose if you're buying or selling");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                _isSubmitting = false; // Reset flag once submission is complete or fails
            }


        }
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {

        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
