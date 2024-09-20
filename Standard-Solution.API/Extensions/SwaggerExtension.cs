using Microsoft.OpenApi.Models;

namespace Standard_Solution.API.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Configuring the Swagger documentation for the API
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API - Standard API template for all projects",
                Version = "v1"
            });

            // Adding JWT Bearer Authentication to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using Bearer scheme.\r\n\r\n" +
                    "Enter 'Bearer' [space] followed by your token in the field below.\r\n\r\n" +
                    "Example (without quotes): 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            // Defining the global security requirement for the API
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>() // No specific scopes are required
                }
            });
        });

        return services;
    }

    public static WebApplication UseSwaggerDoc(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}