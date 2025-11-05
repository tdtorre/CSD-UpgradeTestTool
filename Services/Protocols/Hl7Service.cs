using System.Net.Sockets;
using System.Text;

namespace Services.Protocols
{
    public class Hl7Service : BaseProtocol, IProtocolService
    {        
        private const byte SB = 0x0B; // VT
        private const byte EB = 0x1C; // FS
        private const byte CR = 0x0D; // CR

        public Hl7Service(IConfiguration configuration): base(configuration)
        {
        }

        public ProtocolType GetProtocolType()
        {
            return ProtocolType.Hl7;
        }

        public async Task SendMessageAsync(TcpClient client, string message, bool checkAck = false, CancellationToken cancellationToken = default)
        {
            try
            {
                _stream ??= client.GetStream();
                var messageBytes = Encoding.UTF8.GetBytes(message);
                using (var ms = new MemoryStream())
                {
                    ms.WriteByte(SB);
                    ms.Write(messageBytes, 0, messageBytes.Length);
                    ms.WriteByte(EB);
                    ms.WriteByte(CR);
                    var toSend = ms.ToArray();
                    _stream.Write(toSend, 0, toSend.Length);
                }

                if (checkAck)
                {
                    var buffer = new byte[1024];
                    var ackMessage = new StringBuilder();

                    int bytesRead;
                    while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) > 0)
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