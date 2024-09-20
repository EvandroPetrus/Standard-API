using FluentValidation;
using Standard_Solution.Domain.Interfaces;
using Standard_Solution.Domain.Interfaces.Services;
using Standard_Solution.Domain.Validator;
using Standard_Solution.Infra;
using Standard_Solution.Service.Services;
namespace Standard_Solution.API.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpClient();
        services.AddValidatorsFromAssemblyContaining<UserSignUpRequestValidator>();
        return services;
    }
}
