namespace Standard_Solution.Domain.Models;

public class EmailConfigurations
{
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public string Template { get; private set; }
    public string Subject { get; private set; }

    public EmailConfigurations(string email, string userName, string template, string subject)
    {
        Email = email;
        UserName = userName;
        Template = template;
        Subject = subject;
    }
}
