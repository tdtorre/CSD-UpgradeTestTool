using CSD.UpgradeTestTool.Core.Interfaces;

namespace CSD.UpgradeTestTool.Core.Models.Modules
{
    public class IcaModule: IModule
    {        
        public ModuleType ModuleType => ModuleType.Ica;
        public List<TestCase> TestCases { get; set; }
        public List<Instrument> Instruments { get; set; }
        public Host Host { get; set; }
    }
}