using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Subscriber
{
	class Program
	{
		static void Main(string[] args)
		{
			ConnectionFactory factory = new ConnectionFactory
			{
				Uri = new Uri("amqp://rabbitmq:rabbitmq@localhost:5672")
			};

			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			channel.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
			var message = new { Name = "Producer", Message = "Hello!" };
			var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (sender, e) =>
			{
				var body = e.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				System.Console.WriteLine("Message --------- " + message);
			};

			channel.BasicConsume("demo-queue", true, consumer);
			Console.ReadLine();
		}
	}
}
