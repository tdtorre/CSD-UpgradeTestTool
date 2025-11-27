namespace CSD.UpgradeTestTool.Core.Models
{
    public class UpgradeProject
    {
        public UpgradeProject(string affiliateName, string affiliateCode, string sourceVersion, string targetVersion)
        {
            Name = $"{affiliateName} - [{affiliateCode}] - Upgrade from {sourceVersion} version to {targetVersion}.";
            SourceVersion = Version.Parse(sourceVersion);
            TargetVersion = Version.Parse(targetVersion);
            UpgradeExecutions = [];
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; internal set; }
        public string? Description { get; internal set; }
        public Version SourceVersion { get; set; }
        public Version TargetVersion { get; set; }
        public List<Instrument> Instruments { get; set; }
        public List<Host> Hosts { get; set; }
        public List<UpgradeExecution> UpgradeExecutions { get; set; }

        public bool IsSameVersion(Version source, Version target)
        {
            return this.SourceVersion == source && this.TargetVersion == target;
        }
    }
}