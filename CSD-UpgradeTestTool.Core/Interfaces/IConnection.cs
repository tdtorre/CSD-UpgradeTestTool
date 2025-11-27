using System.Net;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IConnection
    {
        IPAddress Host { get;}
        int Port { get; }

        Task StablishConnectionAsync(CancellationToken cancellationToken = default);
        Task SendMessageAsync(IMessage message);
        Task SendBulkMessageAsync(List<IMessage> messagesList);
    }
}