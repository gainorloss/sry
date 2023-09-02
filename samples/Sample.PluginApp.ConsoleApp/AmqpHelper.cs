using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Sample.PluginApp.ConsoleApp
{
    public class AmqpHelper
    {
        private readonly static ConnectionFactory _cf;
        static AmqpHelper()
        {
            _cf = new ConnectionFactory
            {
                HostName = "8.142.158.178",
                UserName = "admin",
                Password = "p@ssw0rd",
                VirtualHost = "dev"
            };
        }
        public static void Consume(Func<string, bool> handler, string queue = "csharp.amqp.test")
        {
            var connection = _cf.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handler.Invoke(message);

                // here channel could also be accessed as ((EventingBasicConsumer)sender).Model
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue,
                                 autoAck: false,
                                 consumer: consumer);

        }

        public static void Publish(string message, string queue = "csharp.amqp.test")
        {
            using var connection = _cf.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queue,
                                 basicProperties: properties,
                                 body: body);
        }
        public static void Publish<T>(T obj, string queue = "csharp.amqp.test")
        {
            using var connection = _cf.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: queue,
                                 basicProperties: properties,
                                 body: body);
        }
    }
}
