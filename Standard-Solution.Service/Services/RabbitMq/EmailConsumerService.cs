using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Standard_Solution.Domain.Interfaces.Services;
using System.Text;

namespace Standard_Solution.Service.Services.RabbitMq
{
    public class EmailConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly string _hostname;
        private readonly string _queueName;

        public EmailConsumerService(IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            _hostname = _configuration["RabbitMq:HostName"];
            _queueName = _configuration["RabbitMq:QueueName"];
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _emailService.HandleRabbitMqMessage(message);
            };

            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
