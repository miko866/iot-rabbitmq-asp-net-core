using System.Text;
using System;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace RabbitMQ.Publisher
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

			channel.BasicPublish("", "demo-queue", null, body);

		}
	}
}
