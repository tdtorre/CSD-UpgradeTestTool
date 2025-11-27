using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IModule
    {
        ModuleType ModuleType { get; }
        List<TestCase> TestCases { get; set; }
    }
}