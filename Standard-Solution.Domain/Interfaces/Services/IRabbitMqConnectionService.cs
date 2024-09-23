namespace Standard_Solution.Domain.Interfaces.Services;

public interface IRabbitMqConnectionService
{
    bool TryConnect();
    void Dispose();
}
