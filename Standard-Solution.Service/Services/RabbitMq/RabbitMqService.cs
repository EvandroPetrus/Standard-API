using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Standard_Solution.Domain.Interfaces.Services;
using System.Text;
using System.Text.Json;

namespace Standard_Solution.Service.Services.RabbitMq;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConfiguration _configuration;
    private readonly string _hostName;
    private readonly string _queueName;

    public RabbitMqService(IConfiguration configuration)
    {
        _configuration = configuration;
        _hostName = _configuration["RabbitMq:HostName"];
        _queueName = _configuration["RabbitMq:QueueName"];
    }

    public void PublishMessage<T>(T message)
    {
        var factory = new ConnectionFactory() { HostName = _hostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var messageBody = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageBody);

        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
    }
}
