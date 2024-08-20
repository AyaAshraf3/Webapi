
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Webapi.ClientConsume;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Webapi.Repository;


namespace Webapi.Hubs
{
    public class SendOrders : Hub
    {
        private readonly CCInterface _clientRepository;

        public SendOrders(CCInterface clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Method to get all data (both new and old)
        public async Task<List<ClientConsumeAPI>> GetAllData()
        {
            // Fetch the data from the database via the repository
            var data = await _clientRepository.Get();

            return data.ToList();
        }

        // Method to notify clients of a new order submission
        public async Task NotifyNewOrder(ClientConsumeAPI newOrder)
        {
            await Clients.All.SendAsync("ReceiveOrderUpdate", newOrder);
        }

       
    }
}
