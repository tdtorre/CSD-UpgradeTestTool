
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Services.Modules
{
    public abstract class ModuleExecutionService: IModuleExecutionService
    {
        protected IModule _module;

        protected abstract Task PreLoadDataAsync();
        protected abstract Task TestsCreationAsync();
        protected abstract Task TestsExecutionAsync();
        protected abstract Task TestsValidationAsync();

        public ModuleExecutionService(IModule module)
        {
            if(module == null)
                    throw new InvalidOperationException("Module is null.");

            _module = module;
        }

        // TODO. Upgrade report
        public async Task ExecuteAsync()
        {
            await PreLoadDataAsync();
            await TestsCreationAsync();
            await TestsExecutionAsync();
            await TestsValidationAsync();
        }
    }
}