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
            foreach(var tc in testCases)
            {
                var protocolService = ProtocolServiceFactory.GetProtocolService(tc.ProtocolType, _configuration);
                var protocolClient = ((BaseProtocol)protocolService).GetProtocolClient(protocolService, clients);

                await protocolService.SendMessageAsync(protocolClient, tc.Message + " Executed");
            }

            return testCases;
        }
    }
}