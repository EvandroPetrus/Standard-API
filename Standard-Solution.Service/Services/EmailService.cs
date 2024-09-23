using Microsoft.Extensions.Configuration;
using Serilog;
using Standard_Solution.Domain.Interfaces.Services;
using Standard_Solution.Domain.Models;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Web;

namespace Standard_Solution.Service.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public EmailService(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void SendEmail(string userName, string email, string emailTemplatePath, string token, string subject)
    {
        try
        {
            string encodedToken = HttpUtility.UrlEncode(token);
            string template = File.ReadAllText(emailTemplatePath);

            using MailMessage message = new()
            {
                From = new MailAddress(email),
                To = { new MailAddress(email) },
                Subject = subject,
                Body = template
            };

            var smtpSettings = _configuration.GetSection("SmtpSettings");
            string host = smtpSettings["Host"];
            int port = int.Parse(smtpSettings["Port"]);
            string userNameEmailServer = smtpSettings["Username"];
            string passwordEmailServer = smtpSettings["Password"];

            SendMessage(message, host, port, userNameEmailServer, passwordEmailServer);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while sending the email.");
        }
    }

    public void HandleRabbitMqMessage(string message)
    {
        try
        {
            var emailConfig = JsonSerializer.Deserialize<EmailConfigurations>(message);
            if (emailConfig != null)
            {
                SendEmail(emailConfig.UserName, emailConfig.Email, emailConfig.Template, "", emailConfig.Subject);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while processing the RabbitMQ message.");
        }
    }

    private void SendMessage(MailMessage message, string host, int port, string userNameEmailServer, string passwordEmailServer)
    {
        try
        {
            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userNameEmailServer, passwordEmailServer),
                EnableSsl = true
            };

            client.Send(message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while sending the email.");
        }
    }
}
