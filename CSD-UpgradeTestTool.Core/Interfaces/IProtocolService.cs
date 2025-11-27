using System.Net.Sockets;
using ProtocolType = CSD.UpgradeTestTool.Core.Models.ProtocolType;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IProtocolService
    {
        ProtocolType GetProtocolType();
        Task SendQueryMessageAsync(TcpClient client, string message, bool checkAck = false, CancellationToken cancellationToken = default);
    }
}