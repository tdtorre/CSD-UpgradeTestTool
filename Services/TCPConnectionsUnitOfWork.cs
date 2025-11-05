using Models;
using Services.Protocols;
using System.Net;
using System.Net.Sockets;

namespace Services
{
    public class TCPConnectionsUnitOfWork : IDisposable
    {
        private readonly Dictionary<InstrumentType, (TcpClient Client, IProtocolService Service)> _deviceClientMap;
        private readonly IConfiguration _configuration;

        public TCPConnectionsUnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            _deviceClientMap = new Dictionary<InstrumentType, (TcpClient, IProtocolService)>();
        }

        public (TcpClient Client, IProtocolService Service) GetOrCreateClient(InstrumentType instrumentType, Protocols.ProtocolType protocolType)
        {
            if (_deviceClientMap.ContainsKey(instrumentType)) {
                return _deviceClientMap[instrumentType];
            }

            var host = _configuration.GetSection($"Devices:{instrumentType}:Host").Value;
            var port = _configuration.GetSection($"Devices:{instrumentType}:Port").Value;

            if (host == null || port == null)
            {
                throw new InvalidOperationException($"{protocolType} Server configuration is missing.");
            }

            var protocolService = ProtocolServiceFactory.GetProtocolService(protocolType, _configuration);
            var protocolClient = ((BaseProtocol)protocolService).CreateClient(host, int.Parse(port), protocolType).Result;

            if (protocolClient == null)
            {
                throw new ArgumentNullException(nameof(protocolClient));
            }

            if (protocolService == null)
            {
                throw new ArgumentNullException(nameof(protocolClient));
            }

            _deviceClientMap.Add(instrumentType, (Client: protocolClient, Service: protocolService));
            return (protocolClient, protocolService);
        }

        public void Dispose()
        {
            foreach (var kvp in _deviceClientMap.Values)
            {
                kvp.Client?.Dispose();
                kvp.Service?.Dispose();
            }
        }
    }
}
