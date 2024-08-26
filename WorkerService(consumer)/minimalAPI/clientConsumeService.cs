using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using   streamer.theModel;


namespace streamer.minimalAPI
{
    public class clientConsumeService : IClientConsume
    {
        private readonly minimalApiDb context_instance;
        private readonly ILogger<clientConsumeService> _logger;

        public clientConsumeService(minimalApiDb clientRepository, ILogger<clientConsumeService> logger)
        {
            context_instance = clientRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<submitContent>> GetAllClientConsumesAsync()

        {
            _logger.LogInformation("Getting All the orders saved in the DB by the streamer!!");

            //we are using ToListAsync method of dbset to show all entry
            return await context_instance.ClientConsume.ToListAsync();


        }

        
    }
}
