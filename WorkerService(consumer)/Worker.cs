using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using streamer.Hubs;
using streamer.theModel;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;


namespace streamer
{
    public partial class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<StreamingHub> _hubContext;
        private readonly IDistributedCache _distributedCache;
        private const string RedisCacheKey = "ClientConsumesCacheKey";

        public Worker(ILogger<Worker> logger, IHubContext<StreamingHub> hubContext, IDistributedCache distributedCache)
        {
            _logger = logger;
            _hubContext = hubContext;
            _distributedCache = distributedCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "Order_logs", type: ExchangeType.Fanout);

                    var queueName = channel.QueueDeclare(
                        queue: "queue1",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    ).QueueName;

                    channel.QueueBind(queue: queueName,
                        exchange: "Order_logs",
                        routingKey: string.Empty);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var order = System.Text.Json.JsonSerializer.Deserialize<SubmitDTO>(message);

                        // Inside the consumer.Received event handler
                        _logger.LogInformation("Received Order: {0}", message);

                        // Notify the SignalR clients about the new order
                        await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", order);

                        // Add the order to Redis cache
                        _logger.LogInformation("Adding received order to Redis cache.");
                        var cachedData = await _distributedCache.GetAsync(RedisCacheKey);
                        var clientConsumes = cachedData != null
                            ? JsonConvert.DeserializeObject<List<SubmitDTO>>(Encoding.UTF8.GetString(cachedData))
                            : new List<SubmitDTO>();

                        clientConsumes.Add(order);
                        var serializedData = JsonConvert.SerializeObject(clientConsumes);
                        var encodedData = Encoding.UTF8.GetBytes(serializedData);

                        var cacheEntryOptions = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(20));

                        await _distributedCache.SetAsync(RedisCacheKey, encodedData, cacheEntryOptions);
                        _logger.LogInformation("Order successfully added to Redis cache.");

                    };

                    channel.BasicConsume(queue: queueName,
                        autoAck: true,
                        consumer: consumer);

                    _logger.LogInformation("Listening for messages...");

                    // Keep the connection alive
                    while (connection.IsOpen)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
