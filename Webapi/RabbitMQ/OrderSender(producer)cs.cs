using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi.ClientConsume;

public class OrderSender
{
    public void SendOrder(ClientConsumeAPI order)
    {
        var factory = new ConnectionFactory() 
        {
            HostName = "localhost" 
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("Order_logs", ExchangeType.Fanout);
            

            string message = System.Text.Json.JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "Order_logs", //default exchange "",messages are routed to the queue with the name specified by routingKey
                                 routingKey: "",
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
