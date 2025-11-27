using System.Net.Sockets;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Infrastructure.Communication.Connections
{
    public class ClientConnection : BaseConnection
    {
        private TcpClient? _client = null;
     
        public ClientConnection(string host, int port) : base(host, port)
        {
        }

        public async override Task StablishConnectionAsync(CancellationToken cancellationToken = default)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(Host, Port, cancellationToken);
        }
     
        public override async Task SendMessageAsync(IMessage message)
        {
            if(_client == null)
                throw new InvalidOperationException("The client is not initialized.");

            try
            {
                await message.SendAsync(_client.GetStream());   
                message.MarkAsSent(); 
            }
            catch(Exception ex)
            {
                message.MarkAsSent(ex.InnerException.Message);
            }
        }

        public override void Dispose()
        {
            if(_client != null) _client.Dispose();
        }
    }
}