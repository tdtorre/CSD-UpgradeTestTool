using System.Net.Sockets;
using System.Text;

namespace Services.Protocols
{
    public class AstmService : BaseProtocol, IProtocolService
    {
        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte CR = 0x0D;

        private NetworkStream _stream = null;

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
                var stream = _stream ?? client.GetStream();
                var payload = new System.Text.UTF8Encoding().GetBytes(message + (char)CR);
                var framed = new byte[1 + payload.Length + 1];
                framed[0] = STX;
                Buffer.BlockCopy(payload, 0, framed, 1, payload.Length);
                framed[1 + payload.Length] = ETX;
                await stream.WriteAsync(framed, 0, framed.Length, cancellationToken);
                await stream.FlushAsync();

                byte[] buffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string ack = new System.Text.UTF8Encoding().GetString(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error sending ASTM message: {ex.Message}");
            }
        }
    }
}