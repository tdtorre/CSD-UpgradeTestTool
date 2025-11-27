using Models;

namespace Services
{
    public interface IModuleExecutionServiceOld
    {
        public UpgradeProject InitializeProject();
        
        public Task<UpgradeReport> IcaModuleRun(UpgradeProject project);
    }
}