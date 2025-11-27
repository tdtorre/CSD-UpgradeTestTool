using CSD.UpgradeTestTool.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CSD.UpgradeTestTool.Infrastructure.Data.DbContext;

public class MongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
        Database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
        => Database.GetCollection<T>(name);
}