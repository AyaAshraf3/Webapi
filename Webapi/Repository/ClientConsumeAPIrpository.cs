
using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.DataProtection.Repositories;

using Webapi.ClientConsume;

using Microsoft.EntityFrameworkCore;


namespace Webapi.Repository

{

    public class ClientConsumeAPIrpository : CCInterface  //this interface(ccinterface) specifies the methods that need to be implemented to deal with DB.

    {

        private readonly APIcontext context_instance;

        public ClientConsumeAPIrpository(APIcontext context)

        {

            context_instance = context;

        }

        public async Task<ClientConsumeAPI> Create(ClientConsumeAPI client)

        {

            //we are using Add method of dbset to insert entry

            context_instance.ClientConsume.Add(client);

            await context_instance.SaveChangesAsync();

            return client;

        }

        public async Task Delete(Guid Clordid)

        {

            //we are using findasync method of dbset to find entry

            var ClientToDelete = await context_instance.ClientConsume.FindAsync(Clordid);

            //we are using Remove method of dbset to delete entry

            context_instance.ClientConsume.Remove(ClientToDelete);

            await context_instance.SaveChangesAsync();

        }

        public async Task<IEnumerable<ClientConsumeAPI>> Get()

        {

            //we are using ToListAsync method of dbset to show all entry

            return await context_instance.ClientConsume.ToListAsync();

        }

        public async Task<ClientConsumeAPI> Get(Guid Clordid)

        {

            //we are using findasync method of dbset to find specific entry

            return await context_instance.ClientConsume.FindAsync(Clordid);

        }

        public async Task Update(ClientConsumeAPI Client)

        {

            context_instance.Entry(Client).State = EntityState.Modified;

            await context_instance.SaveChangesAsync();

        }

    }

}