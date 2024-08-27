using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using streamer.theModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace streamer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientConsumeController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<ClientConsumeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string RedisCacheKey;

        public ClientConsumeController(IDistributedCache distributedCache, ILogger<ClientConsumeController> logger, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _logger = logger;
            _configuration = configuration;
            RedisCacheKey = configuration["Redis:CacheKey"];
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<submitContent>>> GetClientConsumes()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve data from Redis cache.");

                // Check if the data is in the cache
                var cachedData = await _distributedCache.GetAsync(RedisCacheKey);

                if (cachedData != null)
                {
                    _logger.LogInformation("Data successfully retrieved from Redis cache.");

                    // Data found in cache
                    var serializedData = Encoding.UTF8.GetString(cachedData);
                    var clientConsumes = JsonConvert.DeserializeObject<List<submitContent>>(serializedData);
                    return Ok(clientConsumes);
                }
                else
                {
                    _logger.LogWarning("No data found in Redis cache.");
                    // Data not found in cache
                    return NotFound("No client consumes found in cache.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving data from Redis cache.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }
    }
}
