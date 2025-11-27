using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IModuleExecutionServiceFactory
    {
        IModuleExecutionService CreateService(ModuleType moduleType);
    }
}