namespace Standard_Solution.Domain.Interfaces.Services;

public interface IEmailService
{
    void SendEmail(string userName, string email, string emailTemplatePath, string token, string subject);
    void HandleRabbitMqMessage(string message);
}
