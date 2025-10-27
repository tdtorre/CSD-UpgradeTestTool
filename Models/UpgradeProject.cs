namespace Models
{
    public class UpgradeProject
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public Affiliate Afiliate { get; set; }
        public Version SourceVersion { get; set; }
        public Version TargetVersion { get; set; }
        public List<UpgradeExecution> UpgradeExecutions { get; set; }
        public UpgradeReport Report { get; set; }
    }
}