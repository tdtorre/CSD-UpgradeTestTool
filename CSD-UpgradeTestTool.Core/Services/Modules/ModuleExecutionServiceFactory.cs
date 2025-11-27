using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CSD.UpgradeTestTool.Core.Services.Modules
{
    public class ModuleExecutionServiceFactory: IModuleExecutionServiceFactory
    {
        private readonly IServiceProvider _provider;

        public ModuleExecutionServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IModuleExecutionService CreateService(ModuleType moduleType)
        {
            IModuleExecutionService module;

            switch (moduleType)
            {
                case ModuleType.Ica:
                     module = _provider.GetRequiredService<IcaExecutionService>();
                     break;
                case ModuleType.Hca:
                case ModuleType.Rules:
                    throw new NotImplementedException($"Module type '{moduleType}' is not implemented yet.");
                default:
                    throw new NotSupportedException($"Module type '{moduleType}' doesn't exist in the Execution Service Factory.");
            }

            return module;
        }
    }
}