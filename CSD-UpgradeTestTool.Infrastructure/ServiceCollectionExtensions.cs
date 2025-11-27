using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Settings;
using CSD.UpgradeTestTool.Infrastructure.Communication;
using CSD.UpgradeTestTool.Infrastructure.Data.DbContext;
using CSD.UpgradeTestTool.Infrastructure.Data.Repositories;
using CSD.UpgradeTestTool.Infrastructure.Data.Seeds;
using CSD.UpgradeTestTool.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSD.UpgradeTestTool.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CommonDataSettings>(config.GetSection("CommonDataSettings"));
        services.Configure<IcaSettings>(config.GetSection("Modules:IcaSettings"));
        services.Configure<IrisDbSettings>(config.GetSection("DbConnections:IrisDbSettings"));
        services.Configure<MongoDbSettings>(config.GetSection("DbConnections:MongoDbSettings"));
        
        services.AddTransient<IrisDbContext>();
        services.AddTransient<MongoDbContext>();
        services.AddSingleton<DataSeeder>();

        services.AddScoped<IIcaRepository, IcaRepository>();
        services.AddScoped<IIrisRepository, IrisRepository>();
        services.AddScoped<IMongoRepository, MongoRepository>();
        services.AddScoped<ICommunicationManager, CommunicationManager>();
        
        return services;
    }
}
