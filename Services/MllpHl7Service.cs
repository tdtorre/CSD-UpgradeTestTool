namespace Services
{
    public class MllpHl7Service : IHl7Service
    {
        private const byte SB = 0x0B; // VT
        private const byte EB = 0x1C; // FS
        private const byte CR = 0x0D; // CR

        public async Task SendMessageAsync(string host, int port, string hl7Message, CancellationToken cancellationToken = default)
        {
            using var client = new System.Net.Sockets.TcpClient();
            await client.ConnectAsync(host, port, cancellationToken);
            using var stream = client.GetStream();
            var payload = new System.Text.UTF8Encoding().GetBytes(hl7Message);
            var framed = new byte[1 + payload.Length + 2];
            framed[0] = SB;
            Buffer.BlockCopy(payload, 0, framed, 1, payload.Length);
            framed[1 + payload.Length] = EB;
            framed[1 + payload.Length + 1] = CR;
            await stream.WriteAsync(framed, 0, framed.Length, cancellationToken);
        }
    }
}