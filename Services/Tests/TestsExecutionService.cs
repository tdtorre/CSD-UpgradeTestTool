using System.Net.Sockets;
using Models;
using Services.Protocols;

namespace Services.Tests
{
    public class TestsExecutionService: ITestsExecutionService
    {
        private readonly IConfiguration _configuration;

        public TestsExecutionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<TestCase>> IcaTestsExecution(List<TestCase> testCases)
        {
            var clients = new Dictionary<Protocols.ProtocolType, TcpClient>();
            testCases.ForEach(async tc =>
            {
                var protocolService = ProtocolServiceFactory.GetProtocolService(tc.ProtocolType, _configuration);
                var protocolClient = (protocolService as BaseProtocol).GetProtocolClient(protocolService, clients);

                // What shall we do with the response?
                // await protocolService.SendMessageAsync(protocolClient, tc.Message);
            });

            return testCases;
        }
    }
}