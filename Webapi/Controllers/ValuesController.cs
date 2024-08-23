using Webapi.ClientConsume;

using Webapi.Repository;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.DataProtection.Repositories;

using Microsoft.AspNetCore.SignalR;




//An API controller is a class that is responsible for handling requests at endpoints.

namespace Webapi.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class ValuesController : ControllerBase
    {
        private readonly CCInterface ClientRepository;
        

        //dependency injection
        public ValuesController(CCInterface ClientRepo)

        {
            ClientRepository = ClientRepo;
           

        }

        //HttpGet signifies that this method will handle all Get 

        //Http Request

        [HttpGet]

        public async Task<IEnumerable<ClientConsumeAPI>> GetClients()

        {

            return await ClientRepository.Get();

        }

        [HttpGet("{Clordid}")]

        public async Task<ActionResult<ClientConsumeAPI>> GetClient(Guid Clordid)

        {
            

            return await ClientRepository.Get(Clordid);

        }

        //HttpPost signifies that this method will handle all Post 

        //Http Request

        [HttpPost]

        public async Task<ActionResult<ClientConsumeAPI>> PostClients([FromBody] ClientConsumeAPI Client)

        {
            Client.Clordid = Guid.NewGuid();
            var NewClient = await ClientRepository.Create(Client);

            // Send the order to RabbitMQ
            var orderSender = new OrderSender();
            orderSender.SendOrder(NewClient);

            


            return CreatedAtAction(nameof(GetClients), new { Clordid = NewClient.Clordid }, NewClient);

        }

        //HttpPut signifies that this method will handle all Put 

        //Http Request

        [HttpPut]

        public async Task<ActionResult> PutClient(Guid Clordid, [FromBody] ClientConsumeAPI Client)

        {
            

            //Check if the given id is present database or not

            // if not then we will return bad request

            if (!Clordid.Equals(Client.Clordid))

            {

                return BadRequest();

            }

            await ClientRepository.Update(Client);

            return NoContent();

        }

        //HttpDelete signifies that this method will handle all 

        //Http Delete Request

        [HttpDelete("{Clordid}")]

        public async Task<ActionResult> Delete(Guid Clordid)

        {

            var ClientToDelete = await ClientRepository.Get(Clordid);

            // first we will check i the given id is 

            //present in database or not

            if (ClientToDelete == null)

                return NotFound();

            await ClientRepository.Delete(ClientToDelete.Clordid);

            return NoContent();

        }

    }

}