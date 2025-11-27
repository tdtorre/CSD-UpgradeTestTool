using CSD.UpgradeTestTool.Core.DependencyInjection;
using CSD.UpgradeTestTool.Infrastructure.Data.Seeds;
using CSD.UpgradeTestTool.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.shared.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CSD Upgrade Test Tool API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();