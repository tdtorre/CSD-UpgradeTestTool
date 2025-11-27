using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IAffiliateService
    {
        Affiliate Affiliate { get; set; }
        
        Task LoadAffiliate();

        Task<UpgradeProject> LoadProject();

        Task PreLoadCommunicationInventory(UpgradeProject project);
    }
}