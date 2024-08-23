﻿using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WorkerService_consumer_.minimalAPI;
using WorkerService_consumer_.theModel;

namespace WorkerService.Hubs
{
    public class SendOrders : Hub
    {
        private readonly IClientConsume _clientRepository;

        public SendOrders(IClientConsume clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Method to get all data (both new and old)
        public async Task<List<submitContent>> GetAllData()
        {
            // Fetch the data from the database via the repository
            var data = await _clientRepository.GetAllClientConsumesAsync();
            return data.ToList();
        }

        // Method to notify clients of a new order submission
        public async Task NotifyNewOrder(submitContent newOrder)
        {
            await Clients.All.SendAsync("ReceiveOrderUpdate", newOrder);
        }
    }
}
