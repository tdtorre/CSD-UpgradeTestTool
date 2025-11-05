using Models;

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
            using (var protocolClient = new TCPConnectionsUnitOfWork(_configuration))
            {                
                foreach (var tc in testCases)
                {
                    try
                    {
                        var (client, service) = protocolClient.GetOrCreateClient(tc.InstrumentName, tc.ProtocolType);
                        tc.StartingAt = DateTime.UtcNow;
                        if (protocolClient != null)
                        {
                            await service.SendMessageAsync(client, tc.Message);
                            tc.EndingAt = DateTime.UtcNow;
                        }
                    }
                    catch (Exception ex)
                    {
                        tc.Error = ex.Message;
                    }
                }
            }
            return testCases;
        }
    }
}