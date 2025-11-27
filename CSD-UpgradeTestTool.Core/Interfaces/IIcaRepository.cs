using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IIcaRepository
    {
        Task<IEnumerable<LabTestMapping>> GetLabTestMappingsAsync();
    }
}   