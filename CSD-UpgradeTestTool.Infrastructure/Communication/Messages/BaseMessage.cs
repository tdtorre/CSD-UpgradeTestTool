using System.Net.Sockets;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Infrastructure.Communication.Messages
{
    public abstract class BaseMessage : IMessage
    {
        public string Message { get; private set; }

        public Dictionary<string, string> Parameters { get; private set; } = [];

        public MessageType MessageType { get; private set; }
        public bool IsSent { get; private set; } = false;
        public string Error { get; private set; }
        public bool HasError { get { return !string.IsNullOrEmpty(Error); } }

        public BaseMessage(MessageType messageType, string message, Dictionary<string, string>? parameters = null)
        {
            Message = message;
            MessageType = messageType;

            if(parameters != null && parameters.Count > 0)
                Parameters = parameters;
        }

        public abstract Task SendAsync(NetworkStream stream, bool checkAck = false);

        public void MarkAsSent(string? error = null)
        {
            if(error != null) Error = error;
            IsSent = true;
        }
    }
}