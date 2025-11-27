using System.Net.Sockets;
using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Infrastructure.Communication.Connections
{
    public class ServerConnection : BaseConnection
    {
        private TcpListener? _server = null;
        private TcpClient? _client = null;
        private NetworkStream? _stream = null;

        public ServerConnection(string host, int port) : base(host, port)
        {
        }
        
        public override Task StablishConnectionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // TODO. The host must be our local machine?
                _server = new TcpListener(Host, Port);
                _server.Start();
                StartListening(cancellationToken);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not create the TCP Listener. Error: {ex.Message}");
            }
        }

        private void StartListening(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if(_server == null)
                    throw new InvalidOperationException("The server is not initialized.");

                _client = _server.AcceptTcpClient();
                _stream = _client.GetStream();

                while (_client.Connected)
                {
                    byte[] data = new byte[1024];
                    _stream.ReadExactly(data);
                    Console.WriteLine(System.Text.Encoding.ASCII.GetString(data));
                }
            }
        }

        public override async Task SendMessageAsync(IMessage message)
        {
            if(_stream == null)
                throw new InvalidOperationException("No client is connected to the server.");

            try
            {
                await message.SendAsync(_stream);   
                message.MarkAsSent(); 
            }
            catch(Exception ex)
            {
                message.MarkAsSent(ex.InnerException.Message);
            }
        }

        public override void Dispose()
        {
            if(_stream != null) _stream.Dispose();
            if(_client != null) _client.Dispose();
            if(_server != null) _server.Dispose();
        }
    }
}