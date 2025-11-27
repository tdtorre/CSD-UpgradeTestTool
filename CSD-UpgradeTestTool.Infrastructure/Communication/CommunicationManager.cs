using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using CSD.UpgradeTestTool.Infrastructure.Communication.Connections;
using CSD.UpgradeTestTool.Infrastructure.Communication.Messages;

namespace CSD.UpgradeTestTool.Infrastructure.Communication;

public class CommunicationManager : ICommunicationManager
{
    public IConnection CreateConnection(CommunicationRole role, string host, int port)
    {
        Console.WriteLine($"Creating connection to '{host}:{port}'.");

        if(role == CommunicationRole.Client)
            return new ClientConnection(host, port);
        
        if(role == CommunicationRole.Server)
            return new ServerConnection(host, port);
            
        throw new InvalidOperationException($"Communication Role '{role}' no recognized as a valid for creating a connection.");
    }

    public IMessage CreateMessage(ProtocolType protocol, MessageType type, string message, Dictionary<string, string>? parameters = null)
    {
        if(protocol == ProtocolType.Astm)
            return new AstmMessage(type, message, parameters);

        if(protocol == ProtocolType.Hl7)
            return new Hl7Message(type, message, parameters);

        throw new InvalidOperationException($"Protocol '{protocol}' no recognized as a valid for creating a message.");
    }
}