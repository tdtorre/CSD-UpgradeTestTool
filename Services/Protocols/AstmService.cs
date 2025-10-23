using System.Net.Sockets;

namespace Services.Protocols
{
    public class AstmService : BaseProtocol, IProtocolService
    {
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte CR = 0x0D;

        public AstmService( IConfiguration configuration) : base(configuration)
        {
        }

        public ProtocolType GetProtocolType()
        {
            return ProtocolType.Astm;
        }

        public async Task SendMessageAsync(TcpClient client, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                using var stream = client.GetStream();
                var payload = new System.Text.UTF8Encoding().GetBytes(message + (char)CR);
                var framed = new byte[1 + payload.Length + 1];
                framed[0] = STX;
                Buffer.BlockCopy(payload, 0, framed, 1, payload.Length);
                framed[1 + payload.Length] = ETX;
                await stream.WriteAsync(framed, 0, framed.Length, cancellationToken);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error sending ASTM message: {ex.Message}");
            }
        }
    }
}