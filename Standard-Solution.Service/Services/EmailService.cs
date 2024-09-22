using Microsoft.Extensions.Configuration;
using Standard_Solution.Domain.Interfaces.Services;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Standard_Solution.Service.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
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
            // TODO: Log the error or handle it as needed
            Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
        }
    }

    private static void SendMessage(MailMessage message, string host, int port, string userNameEmailServer, string passwordEmailServer)
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
            // TODO: Log the error or handle it as needed
            Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
        }
    }
}
