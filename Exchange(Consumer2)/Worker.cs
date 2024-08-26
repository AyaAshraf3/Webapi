using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Exchange
{
    public partial class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _hostName;

        public Worker(ILogger<Worker> logger, string hostName)
        {

            _logger = logger;
            _hostName = hostName;
            InitializeRabbitMQListener(); // Initialize RabbitMQ on startup
            
        }

        //This method is called in the constructor to ensure the RabbitMQ listener is set up as soon as the service starts.
        private void InitializeRabbitMQListener()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            exchangedeclare();
            queuedeclare();
            queueBind();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var order = JsonSerializer.Deserialize<SubmitDTO>(message);

                _logger.LogInformation("Received Order: {0}", message);

                // Here you can do further processing, like saving to a database
            };

            _channel.BasicConsume(queue: "queue2",
                                 autoAck: true,
                                 consumer: consumer);

            _logger.LogInformation("RabbitMQ listener initialized. Waiting for messages...");
        }

        private void exchangedeclare()
        {
            _channel.ExchangeDeclare(exchange: "Order_logs", type: ExchangeType.Fanout);
        }

        private void queueBind()
        {
            _channel.QueueBind(queue: "queue2",
                               exchange: "Order_logs",
                               routingKey: string.Empty);
        }

        private void queuedeclare()
        {
            _channel.QueueDeclare(
                                 queue: "queue2",           // Leave the queue name empty to let RabbitMQ generate a unique name, or specify your own name.
                                 durable: true,      // Set to true if you want the queue to survive a broker restart.
                                 exclusive: false,    // Set to true if the queue is only used by one connection and will be deleted when that connection closes.
                                 autoDelete: false,   // Set to false to prevent the queue from being automatically deleted when the last consumer unsubscribes.
                                 arguments: null      // Additional optional arguments, can usually be null.
                             );
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Keep the connection alive
                await Task.Delay(1000, stoppingToken);
            }
        }

        //Properly closes the RabbitMQ connection and channel when the service stops.
        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

