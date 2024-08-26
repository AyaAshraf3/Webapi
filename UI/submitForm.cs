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
using static OMS.loginForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace OMS
{
    public partial class submitForm : Form
    {
        private bool _isSubmitting = false; // Flag to track submission state

        public submitForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (_isSubmitting) return; // Prevent multiple submissions
            _isSubmitting = true; // Set flag to indicate submission is in progress

            try
            {
                EmptyChecking(4, "");

                if (txtQuantity.Text.ToString().Trim() != "" &&
                    txtPrice.Text.ToString().Trim() != "" &&
                    txtDir.Text.ToString().Trim() != "")
                {
                    using (var client = new HttpClient())
                    {
                        // Create the SubmissionData object directly from the current form
                        SubmissionData sbmt = submitDTO();

                        if (sbmt == null)
                        {
                            // The submitDTO method already handles validation and error messages
                            return;
                        }

                        // Pass the SubmissionData object to the submit method
                        var submissionProcessInstance = new submissionProcess();
                        string responseContent = await submissionProcessInstance.submit(client, sbmt);

                        submissionErrorCheck(responseContent);
                    }
                }
                else emptyErrorCheck();
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

        private void emptyErrorCheck()
        {
            if (txtQuantity.Text.ToString().Trim() == "" &&
                txtPrice.Text.ToString().Trim() == "" &&
                txtDir.Text.ToString().Trim() == "")
            {

                EmptyChecking(4, "Please enter the following information");
            }
            else if (txtQuantity.Text.ToString().Trim() == "")
            {

                EmptyChecking(1, "Please enter the Quantity");
            }
            else if (txtPrice.Text.ToString().Trim() == "")
            {

                EmptyChecking(2, "Please enter the Price");
            }
            else if (txtDir.Text.ToString().Trim() == "")
            {
                EmptyChecking(3, "Please choose if you're buying or selling");
            }
        }

        private void submissionErrorCheck(string responseContent)
        {
            if (responseContent.Trim() == "0")
            {
                MessageBox.Show("Failed Submit.");
            }
            else
            {
                MessageBox.Show("Submit successfully.");

                txtQuantity.Text = "";
                txtPrice.Text = "";
                txtDir.Text = "";

                this.Close();
            }
        }

        internal SubmissionData submitDTO()
        {
            return new SubmissionData
            {
                Username = loginForm.entered_username,
                Qty = Convert.ToInt32(txtQuantity.Text),
                Px = Convert.ToInt32(txtPrice.Text),
                Dir = txtDir.Text.ToString()
            };
        }



        private void EmptyChecking(int txtBoxNum, string setError)
        {
            switch (txtBoxNum)
            {
                case 1:
                    errorProvider1.SetError(txtQuantity, setError);
                    break;
                case 2:
                    errorProvider1.SetError(txtPrice, setError);
                    break;
                case 3:
                    errorProvider1.SetError(txtDir, setError);
                    break;
                case 4:
                    errorProvider1.SetError(txtQuantity, setError);
                    errorProvider1.SetError(txtPrice, setError);
                    errorProvider1.SetError(txtDir, setError);
                    break;
            }



        }

       
    }
}
