using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Standard_Solution.API.Extensions;

public static class LoggerExtension
{
    public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
            .Enrich.WithProperty("Environment", environment)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        return services.AddSingleton(Log.Logger);
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment)
    {
        var elasticUri = configuration["ElasticConfiguration:Uri"];

        if (string.IsNullOrEmpty(elasticUri))        
            throw new ArgumentNullException(nameof(elasticUri), "ElasticConfiguration:Uri is not configured.");
        
        return new ElasticsearchSinkOptions(new Uri(elasticUri))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{configuration["ApplicationName"]}-logs-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            NumberOfShards = 2,
            NumberOfReplicas = 1,
            ModifyConnectionSettings = connectionConfiguration =>
                connectionConfiguration.ServerCertificateValidationCallback((o, certificate, arg3, arg4) => true)
        };
    }
}