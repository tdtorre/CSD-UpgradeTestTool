using System.Net;
using System.Net.Sockets;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Infrastructure.Communication.Connections
{
    public abstract class BaseConnection : IConnection, IDisposable
    {
        public IPAddress Host { get; }
        public int Port { get; }

        public BaseConnection(string host, int port)
        {
            Host = IPAddress.Parse(host);
            Port = port;
        }

        public abstract Task StablishConnectionAsync(CancellationToken cancellationToken = default);
        public abstract Task SendMessageAsync(IMessage message);

        public async Task SendBulkMessageAsync(List<IMessage> messagesList)
        {
            await Parallel.ForEachAsync(messagesList, async (message, _) =>
            {
                // TODO. Shall we control this as a transaction or individually?
                await SendMessageAsync(message);
            });
        }

        public abstract void Dispose();
    }
}