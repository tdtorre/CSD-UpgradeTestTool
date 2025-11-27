using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using CSD.UpgradeTestTool.Core.Settings;
using Microsoft.Extensions.Options;

namespace CSD.UpgradeTestTool.Core.Services
{
    public class AffiliateService: IAffiliateService
    {
        private readonly IOptions<AffiliateSettings> _affiliateSettings;
        private readonly IIrisRepository _irisRepository;
        public Affiliate Affiliate { get; set; }
        
        public AffiliateService(IOptions<AffiliateSettings> affiliatSettings, 
                                IIrisRepository irisRepository)
        {
            _affiliateSettings = affiliatSettings ?? throw new ArgumentNullException(nameof(affiliatSettings));
            _irisRepository = irisRepository ?? throw new ArgumentNullException(nameof(irisRepository));
        }

        public async Task LoadAffiliate()
        {
            // TODO: Create or load Affiliate from DB
            Affiliate = new Affiliate(){
                Name = _affiliateSettings.Value.AfiliateName,
                Code = _affiliateSettings.Value.AfiliateCode,
                UpgradeProjects = new List<UpgradeProject>()
            };
        }

        public async Task<UpgradeProject> LoadProject()
        {
            if(this.Affiliate == null)
                throw new InvalidOperationException("Affiliate is not loaded.");

            var sourceVersion = Version.Parse(_affiliateSettings.Value.SourceVersion);
            var targetVersion = Version.Parse(_affiliateSettings.Value.TargetVersion);

            var project = this.Affiliate.UpgradeProjects
                              .Find(p => p.IsSameVersion(sourceVersion, targetVersion));

            if(project == null)
            {
                project = new UpgradeProject(
                    _affiliateSettings.Value.AfiliateName,
                    _affiliateSettings.Value.AfiliateCode,
                    _affiliateSettings.Value.SourceVersion,
                    _affiliateSettings.Value.TargetVersion
                ); 
                
                Affiliate.UpgradeProjects.Add(project);
            }

            return project;
        }

        public async Task PreLoadCommunicationInventory(UpgradeProject project)
        {
            // TODO. Implement devices retrieval
            var instruments = await _irisRepository.GetInstrumentsAsync();
            if(instruments == null)
                throw new InvalidOperationException("No instruments found for the affiliate.");

            var hosts = await _irisRepository.GetHostsAsync();
            if(hosts == null)
                throw new InvalidOperationException("No hosts found for the affiliate.");
            
            project.Instruments = instruments.ToList();
            project.Hosts = hosts.ToList();  
        }
    }
}