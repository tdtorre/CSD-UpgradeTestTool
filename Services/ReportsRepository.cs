using Microsoft.Extensions.Options;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Services
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly MongoClient _client;

        private readonly IMongoCollection<UpgradeReport> _reportsCollection;

        public ReportsRepository(IOptions<ReportsDatabaseSettings> databaseSettings) 
        {
            _client = new MongoClient(
            databaseSettings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _reportsCollection = mongoDatabase.GetCollection<UpgradeReport>(
                databaseSettings.Value.ReportsCollectionName);
        }

        public async Task<List<UpgradeReport>> GetReports()
        {
            return await _reportsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<UpgradeReport> GetReport(string id)
        {
            try
            {
                var report = await _reportsCollection.FindAsync(r => r.Id == id);
                return await report.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task InsertReport(UpgradeReport report)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            await _reportsCollection.InsertOneAsync(report);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
