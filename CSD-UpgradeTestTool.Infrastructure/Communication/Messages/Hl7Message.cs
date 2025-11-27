using System.Net.Sockets;
using System.Text;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Infrastructure.Communication.Messages
{
    public class Hl7Message : BaseMessage
    {
        private const byte SB = 0x0B;
        private const byte EB = 0x1C;
        private const byte CR = 0x0D;

        public Hl7Message(MessageType messageType, string message, Dictionary<string, string>? parameters = null) : base(messageType, message, parameters)
        {
        }

        public async override Task SendAsync(NetworkStream stream, bool checkAck = false)
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(this.Message);
                using (var ms = new MemoryStream())
                {
                    ms.WriteByte(SB);
                    ms.Write(messageBytes, 0, messageBytes.Length);
                    ms.WriteByte(EB);
                    ms.WriteByte(CR);
                    var toSend = ms.ToArray();
                    stream.Write(toSend, 0, toSend.Length);
                }

                if (checkAck)
                {
                    var buffer = new byte[1024];
                    var ackMessage = new StringBuilder();

                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        string chunk = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        ackMessage.Append(chunk);

                        if (chunk.Contains(((char)EB).ToString() + ((char)CR).ToString()))
                            break;
                    }

                    string rawAck = ackMessage.ToString()
                        .Trim((char)SB, (char)EB, (char)CR);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending HL7 message: {ex.Message}");
            }
        }
    }
}