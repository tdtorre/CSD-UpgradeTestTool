using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces;

public interface ICommunicationManager
{
    IConnection CreateConnection(CommunicationRole role, string host, int port);

    IMessage CreateMessage(ProtocolType protocol, MessageType type, string message, Dictionary<string, string>? parameters = null);
}