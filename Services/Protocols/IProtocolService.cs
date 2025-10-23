using System.Net.Sockets;

namespace Services.Protocols
{
    public interface IProtocolService
    {
        ProtocolType GetProtocolType();
        Task SendMessageAsync(TcpClient client, string astmMessage, CancellationToken cancellationToken = default);
    }
}