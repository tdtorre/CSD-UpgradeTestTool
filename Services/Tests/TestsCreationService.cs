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
            // TODO. Filtering Instruments by Protocol HL7
            instruments.Where(i => i.GetProtocolType() == Protocols.ProtocolType.Hl7).ToList().ForEach(i =>
            {
                var protocolType = i.GetProtocolType();
                testCases.AddRange(CreateInstrumentTestCases(i.Type, protocolType, i.TestMapping));
            });

            return testCases;
        }

        private IList<TestCase> CreateInstrumentTestCases(InstrumentType instrumentName, Protocols.ProtocolType protocolType, List<TestMapping> testMapping)
        {
            var instrumentTestCases = new List<TestCase>();
            long orderRandomNumber = new Random().Next(10000, 99999) * 10000000;
            testMapping.ForEach(tm =>
            {
                var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                instrumentTestCases.Add(new TestCase($"{instrumentName}-{protocolType}-{tm.ExternalCode}-{tm.InstrumentSample}-{tm.InstrumentTest}")
                {
                    // TODO. Remove hardcoded message template
                    // The message structure should be created according to the protocol used by the instrument
                    Message = $"MSH|^~\\&|ctws^||host||20251023081551||OML^O21|{GenerateRandomControlMessageId()}||2.5|||NE|NE||8859/1\rPID|1||ANONIMO||||19911231|M||||||||||||||||||||||||\rPV1|||\rORC|NW|{orderRandomNumber}|||||^^^^^R||{currentDateTime}||||^^^|||||||||||||||||||\rOBR|1|{orderRandomNumber}||{tm.ExternalCode}|||{currentDateTime}||||A",
                    Assert = new Assert
                    {
                        // TODO. Review assertion part
                        Expected = $"Expected value for {tm.InstrumentTest}",
                        Actual = $"Actual value for {tm.InstrumentTest}"
                    },
                    ProtocolType = protocolType
                });
                orderRandomNumber++;
            });

            return instrumentTestCases;
        }
        
        private string GenerateRandomControlMessageId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 17)
                .Select(s => s[random.Next(s.Length)]).ToArray()).ToString();
        }
    }
}