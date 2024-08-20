using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace ConsoleAppConsumer
{

    public class OrderReceiver 

    {
        public class Submit
        {
            public Guid Clordid { get; set; }
            public string Name { get; set; }
            public int Qty { get; set; }
            public decimal Px { get; set; }
            public string Dir { get; set; }
        }
        public void ReceiveOrders()
        {
            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            using (var connection = factory.CreateConnection())
            //Here we create channel with session and model
            using (var channel = connection.CreateModel())
            {
                //declare the queue after mentioning name and a few property related to that
                channel.QueueDeclare(queue: "order_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                //Set Event object which listen message from chanel which is sent by producer
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var order = JsonSerializer.Deserialize<Submit>(message);

                    Console.WriteLine(" [x] Received {0}", message);


                };

                //read the message
                channel.BasicConsume(queue: "order_queue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var orderReceiver = new OrderReceiver();
            orderReceiver.ReceiveOrders();
        }
    }

}

