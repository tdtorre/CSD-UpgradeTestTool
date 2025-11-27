using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models.Modules;
using Microsoft.Extensions.Logging;

namespace CSD.UpgradeTestTool.Core.Models
{
    public class UpgradeExecution
    {
        private readonly ILogger _logger;
        private readonly IModuleExecutionServiceFactory _executionServiceFactory;

        public UpgradeExecution(List<ModuleType> modules, IModuleExecutionServiceFactory executionServiceFactory, ILogger logger)
        {            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _executionServiceFactory = executionServiceFactory ?? throw new ArgumentNullException(nameof(executionServiceFactory));
            
            if(modules == null || modules.Count == 0)
                throw new ArgumentException("At least one module must be selected for execution.", nameof(modules));
            
            this.InitializeExecution(modules);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime ExecutionDate { get; set; }
        public List<IModule> Modules { get; set; }

        public async Task ExecuteModules()
        {
            var module = Modules[0];
            // await Parallel.ForEachAsync(Modules, async (module, _) =>
            // {
                try
                {
                    var moduleExecutionService = _executionServiceFactory.CreateService(module.ModuleType);
                    await moduleExecutionService.ExecuteAsync();   
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Error executing module {module.ModuleType}");
                }
            // });
        }

        private void InitializeExecution(List<ModuleType> modules)
        {
            ExecutionDate = DateTime.Now;
            Modules = this.GetSelectedModules(modules);
        }

        private List<IModule> GetSelectedModules(List<ModuleType> moduleTypes)
        {
            var selectedModules = new List<IModule>();
            foreach (var moduleType in moduleTypes)
            {
                switch (moduleType)
                {
                    case ModuleType.Ica:
                        selectedModules.Add(new IcaModule());
                        break;
                    case ModuleType.Hca:
                        selectedModules.Add(new HcaModule());
                        break;
                    case ModuleType.Rules:
                        selectedModules.Add(new RulesModule());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return selectedModules;
        }
    }
}