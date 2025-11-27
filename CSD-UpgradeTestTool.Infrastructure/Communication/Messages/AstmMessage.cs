using System.Net.Sockets;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Infrastructure.Communication.Messages
{
    public class AstmMessage : BaseMessage
    {
        private const byte ENQ = 0x05;
        private const byte EOT = 0x04;
        private const char CR = (char)0x0D;
        private const char STX = (char)0x02;
        private const char ETX = (char)0x03;
        private const char LF = (char)0x0A;
        private const byte ACK = 0x06;

        public AstmMessage(MessageType messageType, string message, Dictionary<string, string>? parameters = null) : base(messageType, message, parameters)
        {
        }

        public async override Task SendAsync(NetworkStream stream, bool checkAck = false)
        {
            try
            {
                var messageShards = this.Message.Split("\r\n");
                await stream.WriteAsync(new[] { ENQ });

                var ack = stream.ReadByte();
                if (ack != ACK)
                {
                    // TODO. If status is not ACK that means the host is busy, what should we do in this case?
                }

                for (var i = 0; i < messageShards.Length; i++)
                {
                    var framedMessage = BuildFramedMessage(messageShards[i], i + 1);
                    var messageBytes = System.Text.Encoding.ASCII.GetBytes(framedMessage);
                    await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    
                    var frameAck = stream.ReadByte();
                    if (frameAck != ACK)
                    {
                        // TODO. Same here
                    }
                }

                await stream.WriteAsync(new[] { EOT });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending ASTM message: {ex.Message}");
            }
        }

        private string BuildFramedMessage(string message, int index)
        {
            var hexSum = CalculateHexSumOfPlainMessage(message, index);
            return STX + index.ToString() + message + CR + ETX + hexSum + CR + LF;
        }

        private string CalculateHexSumOfPlainMessage(string message, int index)
        {
            int sum = 0;
            var messageFrame = index.ToString() + message + CR + ETX;
            foreach (char c in messageFrame)
            {
                sum += c; 
                if (c == ETX)
                    break;
            }

            var hexSum = sum.ToString("X");
            return hexSum.Length >= 2 ? hexSum[^2..] : hexSum.PadLeft(2, '0');
        }
    }
}