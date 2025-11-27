using CSD.UpgradeTestTool.Infrastructure.Settings;
using InterSystems.Data.IRISClient;
using Microsoft.Extensions.Options;

namespace CSD.UpgradeTestTool.Infrastructure.Data.DbContext;

public class IrisDbContext : IDisposable
{
    private readonly IRISConnection _connection;

    public IrisDbContext(IOptions<IrisDbSettings> irisDbSettings)
    {
        _connection = new IRISConnection(irisDbSettings.Value.GetConnectionString());
        _connection.Open();
    }

    public IRISConnection Connection => _connection;

    public void Dispose() => _connection?.Dispose();
}