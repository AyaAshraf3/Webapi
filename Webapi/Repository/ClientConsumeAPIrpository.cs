
using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.DataProtection.Repositories;

using Webapi.ClientConsume;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;


namespace Webapi.Repository

{

    public class ClientConsumeAPIrpository : CCInterface  //this interface(ccinterface) specifies the methods that need to be implemented to deal with DB.

    {

        private readonly APIcontext context_instance;
        private readonly ILogger<ClientConsumeAPIrpository> _logger;

        public ClientConsumeAPIrpository(APIcontext context, ILogger<ClientConsumeAPIrpository> logger)

        {

            context_instance = context;
            _logger = logger;
        }

        public async Task<ClientConsumeAPI> Create(ClientConsumeAPI client)

        {

            //we are using Add method of dbset to insert entry

            context_instance.ClientConsume.Add(client);

            await context_instance.SaveChangesAsync();

            _logger.LogInformation("The new order is created!!");

            return client;

        }

        public async Task Delete(Guid Clordid)

        {

            //we are using findasync method of dbset to find entry

            var ClientToDelete = await context_instance.ClientConsume.FindAsync(Clordid);
            _logger.LogInformation("Found the matched order Id!!");

            //we are using Remove method of dbset to delete entry

            context_instance.ClientConsume.Remove(ClientToDelete);

            await context_instance.SaveChangesAsync();

            _logger.LogInformation("the matched order is now deleted!!");

        }

        public async Task<IEnumerable<ClientConsumeAPI>> Get()

        {
            _logger.LogInformation("Getting All the orders saved in the DB!!");

            //we are using ToListAsync method of dbset to show all entry
            return await context_instance.ClientConsume.ToListAsync();


        }

        public async Task<ClientConsumeAPI> Get(Guid Clordid)

        {
            _logger.LogInformation("Trying to find the requested order!!");
            //we are using findasync method of dbset to find specific entry

            return await context_instance.ClientConsume.FindAsync(Clordid);

        }

        public async Task Update(ClientConsumeAPI Client)

        {

            context_instance.Entry(Client).State = EntityState.Modified;

            _logger.LogInformation("The DB is now updates with the new modification!!");

            await context_instance.SaveChangesAsync();

        }

    }

}