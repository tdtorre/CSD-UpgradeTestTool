
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using CSD.UpgradeTestTool.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;

public class ConsoleApp
{
    private readonly IModuleExecutionServiceFactory _executionServiceFactory;
    private readonly IAffiliateService _affiliateService;
    private readonly ILogger<ConsoleApp> _log;

    public ConsoleApp(
        IModuleExecutionServiceFactory executionServiceFactory,
        IAffiliateService affiliateService,
        ILogger<ConsoleApp> logger)
    {
        _executionServiceFactory = executionServiceFactory;
        _affiliateService = affiliateService;
        _log = logger;
    }

    public async Task RunAsync()
    {
        AnsiConsole.MarkupLine("[green]---------------------[/]");
        AnsiConsole.MarkupLine("[green]CSD Upgrade Test Tool[/]");
        AnsiConsole.MarkupLine("[green]---------------------[/]\r\n");

        AnsiConsole.MarkupLine("[green]Loading affiliate, project and communication devices...[/]\r\n");
        
        var project = await this.InitializeProject();

        AnsiConsole.MarkupLineInterpolated($"\r\nAffiliate: {_affiliateService.Affiliate.Name} ({_affiliateService.Affiliate.Code})");
        AnsiConsole.MarkupLineInterpolated($"Source Version: {_affiliateService.Affiliate.UpgradeProjects.Last().SourceVersion}");
        AnsiConsole.MarkupLineInterpolated($"Target Version: {_affiliateService.Affiliate.UpgradeProjects.Last().TargetVersion}\r\n");

        AnsiConsole.MarkupLine("[green]------------------------------------------[/]\r\n");

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an option:")
                .AddChoices("Execute a new module/s testing",
                            "Menu Option 2",
                            "Menu Option 3",
                            "Menu Option 4",
                            "Exit"));

        switch (option)
        {
            case "Execute a new module/s testing":
                var selectedModules = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<ModuleType>()
                        .Title("Select module/s:")
                        .AddChoices(ModuleType.Ica, ModuleType.Hca, ModuleType.Rules)
                    );
                // AnsiConsole.MarkupLine($"Modules selected: [yellow]{selectedModules}[/]");
                var execution = new UpgradeExecution(selectedModules, _executionServiceFactory, _log);
                await execution.ExecuteModules();
                project.UpgradeExecutions.Add(execution);
                break;

            case "Menu Option 2":
                break;
            case "Menu Option 3":
                break;
            case "Menu Option 4":
                break;


            // case "Run Database Service":
            //     // var project = _moduleExecutionService.InitializeProject();
            //     // var executionReport = await _moduleExecutionService.IcaModuleRun(project);
            //     // AnsiConsole.MarkupLine(executionReport.ToString());
            //     break;

            // case "Show API Key value from config":
            //     var settings = _config.Value;
            //     AnsiConsole.MarkupLine($"Config value: [yellow]{settings.TestMappingQuery}[/]");
            //     break;
        }

        AnsiConsole.MarkupLine("[green]Console App ended![/]");
        await Task.CompletedTask;
    }

    private async Task<UpgradeProject> InitializeProject()
    {
        await _affiliateService.LoadAffiliate();
        var project = await _affiliateService.LoadProject();
        // await _affiliateService.PreLoadCommunicationInventory(project);
        return project;
    }
}