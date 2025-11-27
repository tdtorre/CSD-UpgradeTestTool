using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CSD.UpgradeTestTool.Core.Settings;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Services.Modules;
using CSD.UpgradeTestTool.Core.Services;

namespace CSD.UpgradeTestTool.Core.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AffiliateSettings>(config.GetSection("AffiliateSettings"));
        
        services.AddSingleton<IModuleExecutionServiceFactory, ModuleExecutionServiceFactory>();

        services.AddScoped<IAffiliateService, AffiliateService>();
        services.AddScoped<IcaExecutionService>();        

        return services;
    }
}