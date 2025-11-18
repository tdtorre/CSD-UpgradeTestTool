using Models;

namespace Services
{
    public interface IReportsRepository : IDisposable
    {
        Task<List<UpgradeReport>> GetReports();
        Task<UpgradeReport> GetReport(string id);
        Task InsertReport(UpgradeReport report);
    }
}
