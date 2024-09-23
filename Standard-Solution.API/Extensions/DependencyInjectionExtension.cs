using FluentValidation;
using RabbitMQ.Client;
using Standard_Solution.Domain.Interfaces;
using Standard_Solution.Domain.Interfaces.Services;
using Standard_Solution.Domain.Validator;
using Standard_Solution.Infra;
using Standard_Solution.Infra.Contexts.NoSQL;
using Standard_Solution.Service.Services;
using Standard_Solution.Service.Services.RabbitMq;

namespace Standard_Solution.API.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        services.AddSingleton<IConnectionFactory>(serviceProvider => new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        });
        services.AddScoped<IRabbitMqConnectionService, RabbitMqConnectionService>();
        services.AddScoped<EmailConsumerService>();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddTransient<MongoDbContext>();
        services.AddHttpClient();
        services.AddValidatorsFromAssemblyContaining<UserSignUpRequestValidator>();
        return services;
    }
}
