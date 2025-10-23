using DTOs;

namespace Models
{
    public class UpgradeExecution
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime ExecutionDate { get; set; }
        public IcaModule IcaModule { get; set; }
        public UpgradeReport Report { get; set; }
    }
}