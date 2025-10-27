using System.Net.Sockets;

namespace Services.Protocols
{
    public interface IProtocolService
    {
        ProtocolType GetProtocolType();
        Task SendMessageAsync(TcpClient client, string message, bool checkAck = false, CancellationToken cancellationToken = default);
    }
}