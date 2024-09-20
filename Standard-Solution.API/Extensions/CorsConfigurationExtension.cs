namespace Standard_Solution.API.Extensions;
public static class CorsConfigurationExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                policy =>
                {
                    policy.WithOrigins("")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        return services;
    }
}
