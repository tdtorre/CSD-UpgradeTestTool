namespace CSD.UpgradeTestTool.Infrastructure.Data.DTOs
{
    public class HostDto
    {
        public required string HostName { get; set; }
        public required string ConnectionName { get; set; }
        public required string IPaddress { get; set; }
        public required int TCPPort { get; set; }
        public required int IsServer { get; set; }
    }
}