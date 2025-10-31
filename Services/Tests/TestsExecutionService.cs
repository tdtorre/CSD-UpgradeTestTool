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
           foreach (var tc in testCases)
            {
                try
                    {
                    tc.StartingAt = DateTime.UtcNow;
                    var protocolService = ProtocolServiceFactory.GetProtocolService(tc.ProtocolType, _configuration);
                    
                    // TODO. As we are not ready to send messages to an ASTM host, we are forcing to use HL7 for all messages
                    var protocolClient = ((BaseProtocol)protocolService).CreateClient(tc.ProtocolType).Result;
                    if (protocolClient != null)
                    {
                        await protocolService.SendMessageAsync(protocolClient, tc.Message);
                        tc.EndingAt = DateTime.UtcNow;
                    }
                }
                catch (Exception ex)
                {
                    tc.Error = ex.Message;
                }
            }
            
            return testCases;
        }
    }
}