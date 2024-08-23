using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq; // Import the LINQ namespace
using System.Threading.Tasks;
using WorkerService_consumer_.minimalAPI;
using WorkerService_consumer_.theModel;

namespace WorkerService_consumer_.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientConsumeController : ControllerBase
    {
        private readonly IClientConsume _clientConsumeService;

        public ClientConsumeController(IClientConsume clientConsumeService)
        {
            _clientConsumeService = clientConsumeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<submitContent>>> GetClientConsumes()
        {
            try
            {
                var clientConsumes = await _clientConsumeService.GetAllClientConsumesAsync();

                if (clientConsumes == null || !clientConsumes.Any()) // Use Any() to check for empty collection
                {
                    // If no data is found, return a 404 Not Found status
                    return NotFound("No client consumes found in the database.");
                }

                // If data is found, return it with a 200 OK status
                return Ok(clientConsumes);
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a logging framework like Serilog or NLog here)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
    }
}
