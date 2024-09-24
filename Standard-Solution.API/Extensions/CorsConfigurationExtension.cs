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
                    policy.WithOrigins("http://localhost:5178")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        return services;
    }
}
