using System.Net.Sockets;
using Microsoft.Identity.Client;
using Models;
using Services.Protocols;

namespace Services.Tests
{
    public class TestsCreationService : ITestsCreationService
    {
        private readonly IConfiguration _configuration;

        public TestsCreationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<TestCase>> IcaTestsCreation(List<Instrument> instruments)
        {
            var testCases = new List<TestCase>();
            var clients = new Dictionary<Protocols.ProtocolType, TcpClient>();
            instruments.ForEach(i =>
            {
                var protocolType = i.GetProtocolType();
                var protocolService = ProtocolServiceFactory.GetProtocolService(protocolType, _configuration);
                var protocolClient = ((BaseProtocol)protocolService).GetProtocolClient(protocolService, clients);

                testCases.AddRange(CreateInstrumentTestCases(i.Type, protocolType, i.TestMapping));

                // Do we need to send messages here or in the execution service?
                // testCases.ForEach(async tc => await protocolService.SendMessageAsync(protocolClient, tc.Message));

                // What does the SendMessageAsync shall return?
                // Where is the sample located? Where and when we must create the sample?
                // What about the assert?
            });
            return testCases;
        }

        private IList<TestCase> CreateInstrumentTestCases(InstrumentType instrumentName, Protocols.ProtocolType protocolType, List<TestMapping> testMapping)
        {
            var instrumentTestCases = new List<TestCase>();
            testMapping.ForEach(tm =>
            {
                instrumentTestCases.Add(new TestCase($"{instrumentName}-{protocolType}-{tm.InstrumentSample}-{tm.InstrumentTest}")
                {
                    // The message structure should be created according to the protocol used by the instrument
                    Message = $"Message for test {tm.InstrumentTest} with sample {tm.InstrumentSample}",
                    Assert = new Assert
                    {
                        Expected = $"Expected value for {tm.InstrumentTest}",
                        Actual = $"Actual value for {tm.InstrumentTest}"
                    },
                    ProtocolType = protocolType
                });
            });
            
            return instrumentTestCases;
        }
    }
}