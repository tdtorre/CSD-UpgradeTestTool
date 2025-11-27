
namespace CSD.UpgradeTestTool.Core.Models
{
    public class Host
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string HostAddress { get; private set; }
        public int Port { get; private set; }
        public CommunicationRole Role { get; private set; }
        public ProtocolType Protocol { get; private set; }

        public Host(string id, string name, string hostAddress)
        {
            this.Id = id;
            this.Name = name;
            this.HostAddress = hostAddress;
        }

        public Host(string id, string name, string hostAddress, int port, ProtocolType protocol, CommunicationRole role)
        {
            this.Id = id;
            this.Name = name;
            this.HostAddress = hostAddress;
            this.Port = port;
            this.Protocol = protocol;
            this.Role = role;
        }

        protected void SetPort(int port)
        {
            this.Port = port;
        }

        protected void SetRole(CommunicationRole role)
        {
            this.Role = role;
        }
    }
}