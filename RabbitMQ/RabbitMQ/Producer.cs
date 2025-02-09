using System;
using System.Text;
using RabbitMQ.Client;

class Producer
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare a queue
            channel.QueueDeclare(queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            string message = "Hello, RabbitMQ!";
            var body = Encoding.UTF8.GetBytes(message);

            // Publish message to the queue
            channel.BasicPublish(exchange: "",
                routingKey: "hello",
                basicProperties: null,
                body: body);

            Console.WriteLine($"[x] Sent: {message}");
        }
    }
}