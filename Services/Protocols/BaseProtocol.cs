using System.Net;
using System.Net.Sockets;

namespace Services.Protocols
{
    public enum ProtocolType
    {
        Astm,
        Hl7
    }
        
    public class BaseProtocol
    {
        private readonly IConfiguration _configuration;

        public BaseProtocol(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<TcpClient?> CreateClient(string host, int port, ProtocolType protocolType, CancellationToken cancellationToken = default)
        {
            try
            {
                if (host == null)
                {
                    throw new InvalidOperationException($"{protocolType} Server configuration is missing.");
                }

                using var client = new TcpClient();
                await client.ConnectAsync(IPAddress.Parse(host), port, cancellationToken);
                return client;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not create {protocolType} client ({host}:{port}). Error: {ex.Message}");
            }
        }

        public async Task<TcpClient?> CreateClient(ProtocolType protocolType, CancellationToken cancellationToken = default)
        {
            try
            {
                var host = _configuration.GetSection($"{protocolType}Server:Host").Value;
                var port = _configuration.GetSection($"{protocolType}Server:Port").Value;

                if (host == null || port == null)
                {
                    throw new InvalidOperationException($"{protocolType} Server configuration is missing.");
                }

                using var client = new TcpClient();
                await client.ConnectAsync(IPAddress.Parse(host), int.Parse(port), cancellationToken);
                return client;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not create {protocolType} client. Error: {ex.Message}");
            }
        }

        public TcpClient GetProtocolClient(IProtocolService protocolService)
        {
            return GetProtocolClient(protocolService, new Dictionary<ProtocolType, TcpClient>());
        }

        public TcpClient GetProtocolClient(string host, int port, IProtocolService protocolService)
        {
            var protocolType = protocolService.GetProtocolType();
            var client = ((BaseProtocol)protocolService).CreateClient(host, port, protocolType).Result;
            if (client == null)
            {
                throw new InvalidOperationException($"Could not create client for protocol {protocolType}");
            }
            return client;
        }

        public TcpClient GetProtocolClient(IProtocolService protocolService, Dictionary<ProtocolType, TcpClient> clients)
        {
            var protocolType = protocolService.GetProtocolType();
            if (clients.ContainsKey(protocolType) == false)
            {
                var client = ((BaseProtocol)protocolService).CreateClient(protocolType).Result;
                if (client != null)
                {
                    clients.Add(protocolType, client);
                }
                else
                {
                    throw new InvalidOperationException($"Could not create client for protocol {protocolType}");
                }
            }
                
            return clients[protocolType];
        }
    }
}