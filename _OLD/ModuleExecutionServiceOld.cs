using DTOs;
using Models;
using Services.Tests;
using Microsoft.Extensions.Configuration;

namespace Services
{
    public class ModuleExecutionServiceOld : IModuleExecutionServiceOld
    {
        private readonly IConfiguration _configuration;
        private readonly ITestsCreationService _testsCreationService;
        private readonly ITestsExecutionService _testsExecutionService;

        public ModuleExecutionServiceOld(IConfiguration configuration, ITestsCreationService testsCreationService, ITestsExecutionService testsExecutionService)
        {
            _configuration = configuration;
            _testsCreationService = testsCreationService;
            _testsExecutionService = testsExecutionService;
        }

        public UpgradeProject InitializeProject()
        {
            return InitializeDummyProject();
        }

        public async Task<UpgradeReport> IcaModuleRun(UpgradeProject project)
        {
            var icaModule = await LoadIcaModule();
            icaModule.TestCases = await _testsCreationService.IcaTestsCreation(icaModule.Instruments);
            icaModule.TestCases = await _testsExecutionService.IcaTestsExecution(icaModule.TestCases);

            var lastExecution = project.UpgradeExecutions.Last();
            lastExecution.Modules = new List<IModuleOld<TestMappingDto>> { icaModule };
            lastExecution.IcaModule = icaModule;
            lastExecution.Report = UpgradeReport.Generate(project);
            return lastExecution.Report;
        }

        private async Task<IcaModuleOld> LoadIcaModule()
        {
            var icaModule = new IcaModuleOld(_configuration);
            var data = await icaModule.ExtractData();
            if (data != null && data.Count > 0)
            {
                var instruments = data.Select(d => new { d.Id, d.Name }).Distinct();
                // To review Instrument host and port assignment
                icaModule.Instruments.AddRange(instruments.Select(i => new Instrument(i.Id, i.Name, "", 0)));
                icaModule.Instruments.ForEach(i =>
                {
                    var testMapping = data.Where(d => d.Id == i.Id).Select(d => new TestMapping() { InstrumentTest = d.AnalyserTest, InstrumentSample = d.AnalyserSample, ExternalCode = d.ExternalCode });
                    i.TestMapping.AddRange(testMapping);
                });
            }

            return icaModule;
        }

        private UpgradeProject InitializeDummyProject()
        {
            var project = new UpgradeProject
            {
                Afiliate = new Affiliate { Name = "Sample Afiliate", Code = "SAF" },
                Name = "Sample Upgrade Project",
                Description = "This is a sample upgrade project for demonstration purposes.",
                SourceVersion = new Version(1, 0),
                TargetVersion = new Version(2, 0),
                UpgradeExecutions = new List<UpgradeExecution>()
            };

            project.UpgradeExecutions.Add(InitalizeUpgradeExecution());
            return project;
        }

        private UpgradeExecution InitalizeUpgradeExecution()
        {
            var upgradeExecution = new UpgradeExecution()
            {
                ExecutionDate = DateTime.Now
            };

            upgradeExecution.Modules = new List<IModule>
            {
                new HcaModule(),
                new IcaModule(_configuration)
            };

            upgradeExecution.Modules.ForEach(async m =>
            {
               var executionService = ModuleExecutionServiceFactory.CreateService(m.ModuleType);
               await executionService.ExecuteAsync();
               await executionService.GenerateReportAsync();
            });

            return upgradeExecution;
        }
    }
}