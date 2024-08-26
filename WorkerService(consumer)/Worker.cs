using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using streamer.Hubs;
using Microsoft.AspNetCore.SignalR;



namespace streamer
{
    public partial class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<streamer.Hubs.StreamingHub> _hubContext;

        public Worker(ILogger<Worker> logger,IHubContext<streamer.Hubs.StreamingHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
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
                                                         queue: "queue1",           // Leave the queue name empty to let RabbitMQ generate a unique name, or specify your own name.
                                                         durable: true,      // Set to true if you want the queue to survive a broker restart.
                                                         exclusive: false,    // Set to true if the queue is only used by one connection and will be deleted when that connection closes.
                                                         autoDelete: false,   // Set to false to prevent the queue from being automatically deleted when the last consumer unsubscribes.
                                                         arguments: null      // Additional optional arguments, can usually be null.
                                                     ).QueueName;

                    channel.QueueBind(queue: queueName,
                                      exchange: "Order_logs",
                                      routingKey: string.Empty);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var order = JsonSerializer.Deserialize<SubmitDTO>(message);

                        _logger.LogInformation("Received Order: {0}", message);

                        // Notify the SignalR clients about the new order
                        await _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", order);
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


