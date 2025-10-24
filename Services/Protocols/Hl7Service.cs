using System.Net.Sockets;

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

        public async Task SendMessageAsync(TcpClient client, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                using var stream = client.GetStream();
                var payload = new System.Text.UTF8Encoding().GetBytes(message);
                var framed = new byte[1 + payload.Length + 2];
                framed[0] = SB;
                Buffer.BlockCopy(payload, 0, framed, 1, payload.Length);
                framed[1 + payload.Length] = EB;
                framed[1 + payload.Length + 1] = CR;
                await stream.WriteAsync(framed, 0, framed.Length, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending HL7 message: {ex.Message}");
            }
        }
    }
}