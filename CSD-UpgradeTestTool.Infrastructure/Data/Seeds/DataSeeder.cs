using System.Text.Json;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Infrastructure.Data.DbContext;
using CSD.UpgradeTestTool.Infrastructure.Data.DTOs;
using CSD.UpgradeTestTool.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CSD.UpgradeTestTool.Infrastructure.Data.Seeds
{
    public class DataSeeder: IDataSeeder
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly IOptions<MongoDbSettings> _mongoDbSettings;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(MongoDbContext mongoDbContext, IOptions<MongoDbSettings> mongoDbSettings, ILogger<DataSeeder> logger)
        {
            _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
            _mongoDbSettings = mongoDbSettings ?? throw new ArgumentNullException(nameof(mongoDbSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SeedAsync()
        {
            await SeedCollectionAsync<IcaMessageDto>("IcaMessages", "icaMessages.json");
        }

        private async Task SeedCollectionAsync<T>(string collectionName, string fileName)
        {
            var filePath = Path.Combine(_mongoDbSettings.Value.SeedFolderPath, fileName);
            var collection = _mongoDbContext.GetCollection<T>(collectionName);
            var count = await collection.EstimatedDocumentCountAsync();
            if (count > 0)
            {
                _logger.LogInformation($"Skipping {collectionName} seeding — already contains data.");
                return;
            }

            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"Seed file missing: {filePath}");
                return;
            }

            var jsonSeeder = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(jsonSeeder);

            if (data is not null && data.Any())
            {
                await collection.InsertManyAsync(data);
                _logger.LogInformation($"Seeded {collectionName} with {data.Count} items.");
            }
        }
    }
}