using Models;

namespace Services
{
    public interface IModuleExecutionService
    {
        public UpgradeProject InitializeProject();
        
        public Task<UpgradeReport> IcaModuleRun(UpgradeProject project);
    }
}