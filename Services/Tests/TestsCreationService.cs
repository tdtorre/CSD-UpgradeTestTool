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

            instruments.ForEach(i =>
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
                long sampleId = new Random().Next(10000, 99999) * 10000000;
                var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                instrumentTestCases.Add(new TestCase($"{instrumentName}-{protocolType}-{tm.ExternalCode}-{tm.InstrumentSample}-{tm.InstrumentTest}")
                {
                    // TODO. Remove hardcoded message template
                    // The message structure should be created according to the protocol used by the instrument
                    Message = GenerateMessageForInstrument(InstrumentType.Host, tm.ExternalCode, sampleId),
                    InstrumentName = instrumentName,
                    Assert = new Assert
                    {
                        // TODO. Review assertion part
                        SampleId = sampleId.ToString(),
                        Expected = $"Expected value for {tm.InstrumentTest}",
                        Actual = $"Actual value for {tm.InstrumentTest}"
                    },
                    ProtocolType = protocolType
                });
                orderRandomNumber++;
            });

            return instrumentTestCases;
        }

        private string GenerateMessageForInstrument(InstrumentType instrumentName, string externalCode, long sampleId)
        {
            var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            var patientId = 4;
            var patientName = "Fisherman";
            var patientLastName = "Dory";
            var patientDateOfBirth = "20010101";
            var patientSex = "F";
            switch (instrumentName)
            {
                case InstrumentType.CobasE411:
                    return "H|\\^&||||||||||P||\r\nP|1||||||||||||||||||||||||||||||||\r\nO|1|000000000007|1^0012^1^^SAMPLE^NORMAL|ALL|R|20251029100000|||||X||||||||||||||O|||||\r\nR|1|^^^236^^0|2.1|pg/ml|0.000^14.00|N||F|||20251029100000|20251029100000|\r\nL|1|N";
                case InstrumentType.Cobas6500:
                    return "H|\\^&|||Instrument Name^u601^0.17.0^5^201009003^|||||||P|LIS2-A2|20251029100000\r\nP|1\r\nO|1|100000000008|12345^1^Service^SAMPLE|C|R||||||N|||20251029100000|||||||||||F\r\nR|1|1^BIL|12^^||International|||F||Service|||u601\r\nM|1|RC|u601|123456|20121201|20120507|29188300|20121101\r\nL|1|N";
                case InstrumentType.Host:
                    return $"MSH|^~\\&|ctws^||host||20251023081551||OML^O21|{GenerateRandomControlMessageId()}||2.5|||NE|NE||8859/1\rPID|1||ANONIMO||||19911231|M||||||||||||||||||||||||\rPV1|||\rORC|NW|{sampleId}|||||^^^^^R||{currentDateTime}||||^^^|||||||||||||||||||\rOBR|1|{sampleId}||{externalCode}|||{currentDateTime}||||A";
                default:
                    return $"H|\\^&|||CDS|||||||P|1|{currentDateTime}\r\nP|1|{patientId}|{sampleId}||{patientName}^{patientLastName}||{patientDateOfBirth}|{patientSex}|||||||||||||||||TAMU||/HUMAN BITE, IMMUNITY SCRE\r\nO|1|{sampleId}||^^^ALB\\|R|202308221200|||||A||||||||||||||O\r\nL|1|N";
                }
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