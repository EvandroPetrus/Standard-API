namespace Standard_Solution.Domain.Interfaces.Services;

public interface IRabbitMqService
{
    void PublishMessage<T>(T message);
}
