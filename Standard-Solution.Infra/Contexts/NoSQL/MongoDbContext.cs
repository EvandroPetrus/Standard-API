using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serilog;

namespace Standard_Solution.Infra.Contexts.NoSQL;

public class MongoDbContext
{
    private readonly MongoDbSettings _mongoDbSettings;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly ILogger _logger;

    public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings, ILogger logger)
    {
        _mongoDbSettings = mongoDbSettings?.Value ?? throw new ArgumentNullException(nameof(mongoDbSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        try
        {
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(_mongoDbSettings.Host));

            if (_mongoDbSettings.IsSSL)
            {
                clientSettings.SslSettings = new SslSettings
                {
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };
            }

            var client = new MongoClient(clientSettings);
            _mongoDatabase = client.GetDatabase(_mongoDbSettings.Name);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while initializing the MongoDB client.");
            throw;
        }
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }

        return _mongoDatabase.GetCollection<T>(collectionName);
    }
}
