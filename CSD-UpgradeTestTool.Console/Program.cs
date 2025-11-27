using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CSD.UpgradeTestTool.Core.DependencyInjection;
using CSD.UpgradeTestTool.Infrastructure.DependencyInjection;
using CSD.UpgradeTestTool.Infrastructure.Data.Seeds;
using Amazon.Runtime.Internal.Util;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var seeder = host.Services.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();

        var app = host.Services.GetRequiredService<ConsoleApp>();
        await app.RunAsync();
    }

    static IHostBuilder CreateHostBuilder(string[] args){
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var runBasePath = Directory.GetCurrentDirectory();
                var binBasePath = AppContext.BaseDirectory;
                config.SetBasePath(runBasePath);

                config.AddJsonFile("appsettings.shared.json", optional: true);
                config.AddJsonFile(Path.Combine(binBasePath, "appsettings.shared.json"), optional: true);
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructureServices(context.Configuration);
                services.AddCoreServices(context.Configuration);
                services.AddScoped<ConsoleApp>();
            });
    }
}