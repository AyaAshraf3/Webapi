﻿using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using static UI.Form1;

namespace UI
{
    public partial class GridViewForm : Form
    {
        private HubConnection _hubConnection;

        public GridViewForm()
        {
            InitializeComponent();
            InitializeSignalR();
        }

        private async void InitializeSignalR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/orderHub")
                .Build();
            
            await StartConnectionAsync();

            
            _hubConnection.On<Submit>("ReceiveOrderUpdate", newOrder =>
            {
                dataGridView1.Invoke((Action)(() =>
                {
                    var dataSource = dataGridView1.DataSource as List<Submit>;
                    dataSource.Add(newOrder); // Add the new order
                    dataGridView1.DataSource = null; // Refresh the DataGridView
                    dataGridView1.DataSource = dataSource;
                }));
            });


        }


        private async Task StartConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _hubConnection.StartAsync();
                    MessageBox.Show("Connected to SignalR server!");

                    // Load data as soon as the connection is established
                    await LoadOrdersAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection failed: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Connection is already started.");
            }
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                // Call the SignalR hub method to get the data
                var orders = await _hubConnection.InvokeAsync<List<Submit>>("GetAllData");

                // Use Task.Run to update the UI on the main thread
                await Task.Run(() =>
                {
                    // Populate the DataGridView with the received data
                    dataGridView1.Invoke((Action)(() =>
                    {
                        dataGridView1.DataSource = orders;
                    }));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving data: {ex.Message}");
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hubConnection != null && _hubConnection.State != HubConnectionState.Disconnected)
            {
                _hubConnection.StopAsync();
                _hubConnection.DisposeAsync();
            }
            base.OnFormClosing(e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 orderform = new Form1();
            orderform.Show();
        }
    }
}
