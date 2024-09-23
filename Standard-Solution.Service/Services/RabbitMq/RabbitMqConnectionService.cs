using Serilog;
using RabbitMQ.Client;
using Standard_Solution.Domain.Interfaces.Services;

namespace Standard_Solution.Service.Services.RabbitMq;
public class RabbitMqConnectionService : IRabbitMqConnectionService
{
    private readonly ILogger _logger;
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;

    public RabbitMqConnectionService(ILogger logger, IConnectionFactory connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    public bool TryConnect()
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            if (_connection.IsOpen)
            {
                _logger.Information("Successfully connected to RabbitMQ.");
                return true;
            }
            else
            {
                _logger.Error("Failed to connect to RabbitMQ.");
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while trying to connect to RabbitMQ.");
            return false;
        }
    }

    public void Dispose() => _connection?.Dispose();
}
