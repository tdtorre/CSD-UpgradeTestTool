using System.Net.Sockets;
using System.Text;

namespace Services.Protocols
{
    public class AstmService : BaseProtocol, IProtocolService
    {
        private const byte ENQ = 0x05;
        private const byte EOT = 0x04;
        private const char CR = (char)0x0D;
        private const char STX = (char)0x02;
        private const char ETX = (char)0x03;
        private const char LF = (char)0x0A;
        private const byte ACK = 0x06;

        public AstmService(IConfiguration configuration) : base(configuration)
        {
        }

        public ProtocolType GetProtocolType()
        {
            return ProtocolType.Astm;
        }

        public async Task SendMessageAsync(TcpClient client, string message, bool checkAck = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var stream = _stream ?? client.GetStream();
                //Covers the edgecase when message has \r\n. If not message will be send as one chunk
                var messageShards = message.Split("\r\n");

                await stream.WriteAsync(new[] { ENQ });

                var ack = stream.ReadByte();

                if (ack != ACK)
                {
                    //If status is not ACK that means the host is busy, what should we do in this case?
                }

                for (var i = 0; i < messageShards.Length; i++)
                {
                    var framedMessage = BuildFramedMessage(messageShards[i], i + 1);

                    var messageBytes = Encoding.ASCII.GetBytes(framedMessage);

                    await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

                    var frameAck = stream.ReadByte();

                    if (frameAck != ACK)
                    {
                        // Same here
                    }
                }

                await stream.WriteAsync(new[] { EOT });

                client.Dispose();
                stream.Dispose();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending ASTM message: {ex.Message}");
            }
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

        private string BuildFramedMessage(string message, int index)
        {
            var hexSum = CalculateHexSumOfPlainMessage(message, index);
            return STX + index.ToString() + message + CR + ETX + hexSum + CR + LF;
        }
    }
}