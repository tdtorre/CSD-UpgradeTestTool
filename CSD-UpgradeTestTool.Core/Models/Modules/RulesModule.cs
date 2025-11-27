using CSD.UpgradeTestTool.Core.Interfaces;

namespace CSD.UpgradeTestTool.Core.Models.Modules
{
    public class RulesModule: IModule
    {
        public ModuleType ModuleType => ModuleType.Rules;
        public List<TestCase> TestCases { get; set; }
    }
}