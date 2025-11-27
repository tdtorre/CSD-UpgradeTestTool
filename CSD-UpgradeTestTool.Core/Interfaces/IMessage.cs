using System.Net.Sockets;

namespace CSD.UpgradeTestTool.Core.Models
{
    public enum MessageType
    {
        QueryMessage,
        QueryReplyMessage,
        PatientResultMessage,
        CreateOrderMessage,
    }

    public interface IMessage
    {
        string Message { get; }
        Dictionary<string, string> Parameters { get; }
        MessageType MessageType { get; }
        bool IsSent { get; }
        string Error { get; }
        bool HasError { get; }
        Task SendAsync(NetworkStream stream, bool checkAck = false);
        void MarkAsSent(string? error = null);
    }
}