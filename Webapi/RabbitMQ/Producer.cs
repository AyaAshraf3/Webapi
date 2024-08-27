using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi.theModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class OrderSender
{
    private readonly string _hostName;
    private readonly ILogger<OrderSender> _logger;

    public OrderSender(IConfiguration configuration, ILogger<OrderSender> logger)
    {
        _hostName = configuration.GetSection("AppSettings:Hostname").Value;
        _logger = logger;
    }
    public void SendOrder(webapiDTO order)
    {
        var factory = new ConnectionFactory() 
        {
            HostName = _hostName
        };
        try
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                declareExchange(channel);

                string message;
                byte[] body;
                serializeMessage(order, out message, out body);

                publishMessage(channel, body);

                _logger.LogInformation("Order sent: {Message}", message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending order.");
        }
    }

    private static void publishMessage(IModel channel, byte[] body)
    {
        channel.BasicPublish(exchange: "Order_logs", //default exchange "",messages are routed to the queue with the name specified by routingKey
                             routingKey: "",
                             basicProperties: null,
                             body: body);
    }

    private static void serializeMessage(webapiDTO order, out string message, out byte[] body)
    {
        message = System.Text.Json.JsonSerializer.Serialize(order);
        body = Encoding.UTF8.GetBytes(message);
    }

    private static void declareExchange(IModel channel)
    {
        channel.ExchangeDeclare("Order_logs", ExchangeType.Fanout);
    }
}
