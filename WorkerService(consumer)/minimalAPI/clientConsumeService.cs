using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using streamer.theModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace streamer.minimalAPI
{
    public class clientConsumeService : IClientConsume
    {
        private readonly ILogger<clientConsumeService> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly string RedisCacheKey;

        public clientConsumeService(ILogger<clientConsumeService> logger, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _configuration = configuration;
            RedisCacheKey = configuration["Redis:CacheKey"];
        }

        public async Task<IEnumerable<submitContent>> GetAllClientConsumesAsync()
        {
            _logger.LogInformation("Getting all client consumes from Redis cache.");

            var cachedData = await _distributedCache.GetAsync(RedisCacheKey);

            if (cachedData != null)
            {
                _logger.LogInformation("Data successfully retrieved from Redis cache.");
                var serializedData = Encoding.UTF8.GetString(cachedData);
                return JsonConvert.DeserializeObject<List<submitContent>>(serializedData);
            }
            else
            {
                _logger.LogWarning("No data found in Redis cache.");
                return new List<submitContent>(); // Return an empty list or handle the case as needed
            }
        }
    }
}
