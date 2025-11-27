namespace CSD.UpgradeTestTool.Core.Models
{
    public class Affiliate
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<UpgradeProject> UpgradeProjects { get; set; }
    }
}