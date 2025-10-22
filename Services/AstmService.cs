namespace Services
{
    public class AstmService : IAstmService
{
    private const byte STX = 0x02;
    private const byte ETX = 0x03;
    private const byte CR = 0x0D;

    public async Task SendMessageAsync(string host, int port, string astmMessage, CancellationToken cancellationToken = default)
    {
        using var client = new System.Net.Sockets.TcpClient();
        await client.ConnectAsync(host, port, cancellationToken);
        using var stream = client.GetStream();
        var payload = new System.Text.UTF8Encoding().GetBytes(astmMessage + (char)CR);
        var framed = new byte[1 + payload.Length + 1];
        framed[0] = STX;
        Buffer.BlockCopy(payload, 0, framed, 1, payload.Length);
        framed[1 + payload.Length] = ETX;
        await stream.WriteAsync(framed, 0, framed.Length, cancellationToken);
    }
}
}