using CSD.UpgradeTestTool.Core.Interfaces;

namespace CSD.UpgradeTestTool.Core.Models.Modules
{
    public class HcaModule: IModule
    {
        public ModuleType ModuleType => ModuleType.Hca;
        public List<TestCase> TestCases { get; set; }
    }
}