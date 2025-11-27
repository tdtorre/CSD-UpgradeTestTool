
namespace CSD.UpgradeTestTool.Core.Models
{
    public class Instrument: Host
    {
        public List<LabTestMapping> LabTestMapping { get; set; }

        public Instrument(string id, string name, string host, string connectionDesc) : base(id, name, host)
        {
            var connectionElements = connectionDesc.Split(" - ");
            if(connectionElements.Length != 3)
                throw new ArgumentException("Invalid Instrument Connection Description format.", nameof(connectionDesc));

            this.SetPort(int.Parse(connectionElements[2]));
            this.SetRole(connectionElements[1] == "S" ? CommunicationRole.Server : CommunicationRole.Client);
            this.LabTestMapping = [];
        }
    }
}