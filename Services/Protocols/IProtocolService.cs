using System.Net.Sockets;

namespace Services.Protocols
{
    public interface IProtocolService : IDisposable
    {
        ProtocolType GetProtocolType();
        Task<string> SendMessageAsync(TcpClient client, string message, bool waitResponse = false, CancellationToken cancellationToken = default);
    }
}