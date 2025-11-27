using System.Reflection;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using CSD.UpgradeTestTool.Core.Models.Modules;
using CSD.UpgradeTestTool.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSD.UpgradeTestTool.Core.Services.Modules
{
    public class IcaExecutionService : ModuleExecutionService
    {
        private readonly IIcaRepository _icaRepository;
        private readonly IMongoRepository _mongoRepository;
        private readonly IIrisRepository _irisRepository;
        private readonly ICommunicationManager _communicationManager;
        private readonly IOptions<IcaSettings> _icaSettings;
        private readonly ILogger<IcaExecutionService> _logger;

        public IcaModule Module { get { return _module as IcaModule; }}

        public IcaExecutionService(IIcaRepository icaRepository, 
                                   IMongoRepository mongoRepository,
                                   IIrisRepository irisRepository,
                                   ICommunicationManager communicationManager,
                                   IOptions<IcaSettings> icaSettings,
                                   ILogger<IcaExecutionService> logger) : base(new IcaModule())
        {
            _icaRepository = icaRepository ?? throw new ArgumentNullException(nameof(icaRepository));
            _mongoRepository = mongoRepository ?? throw new ArgumentNullException(nameof(mongoRepository));
            _irisRepository = irisRepository ?? throw new ArgumentNullException(nameof(irisRepository));
            _communicationManager = communicationManager ?? throw new ArgumentNullException(nameof(communicationManager));
            _icaSettings = icaSettings ?? throw new ArgumentNullException(nameof(icaSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // [PRELOAD] 0. PreLoad data: get instruments + get labtest mappging x instrument
        // [CREATE TEST] 1. Create Orders in Navify connecting as a Host and using the data from the test mappings --> ORDER CREATION MESSAGE (Order =  SampleId + SampleType)
        // [CREATE TEST] 1.1 Evaluate if the orders has been created on the Iris DB --> QUERY DB (Joan)
        // [EXECUTE TEST] 2. Which are the lab tests that we need to execute? --> QUERY MESSAGE --> QUERY REPLY MESSAGE (Lab Test to execute x Order)
        // [EXECUTE TEST] 3. Execute Lab Test against Navify --> PATIENT RESULT MESSAGE
        // [EVALUATE TEST] 4. Evaluate if the lab test results was received in the db --> QUERY DB

        protected override async Task PreLoadDataAsync()
        {
            _logger.LogInformation("PreLoad Data phase started");

            var instruments = await _irisRepository.GetInstrumentsAsync();
            if(instruments == null)
                throw new InvalidOperationException("No instruments found for the affiliate.");

            Module.Host = await _irisRepository.GetHostByIdAsync(_icaSettings.Value.DefaultHost);
            Module.Instruments = instruments.ToList();
            await AssignLabTestMappingsToInstruments();
        }

        protected override async Task TestsCreationAsync()
        {
            _logger.LogInformation("Tests Creation phase started");

            await CreateOrdersFromLabTests();
            // await EvaluateCreatedOrders();
        }

        protected override Task TestsExecutionAsync()
        {
            _logger.LogInformation("Tests Execution phase started");
            return Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override Task TestsValidationAsync()
        {
            _logger.LogInformation("Tests Validation phase started");
            return Task.CompletedTask;
            throw new NotImplementedException();
        }

        private async Task AssignLabTestMappingsToInstruments()
        {
            var labTestMappings = await _icaRepository.GetLabTestMappingsAsync();
            Module.Instruments.ForEach(i =>
            {
                var labTestMappingsForInstrument = labTestMappings.Where(tm => tm.InstrumentId == i.Id).ToList();
                i.LabTestMapping.Clear();
                i.LabTestMapping.AddRange(labTestMappingsForInstrument);
            });
        }

        // private async Task<IList<TestCase>> CreateInstrumentTestCases(Instrument instrument)
        // {
        //     var instrumentTestCases = new List<TestCase>();
        //     long orderRandomNumber = new Random().Next(10000, 99999) * 10000000;

        //     var randomIndex = new Random(_icaSettings.Value.PatientIds.Count());
        //     var randomPatient = _icaSettings.Value.PatientIds[randomIndex.Next()-1];

        //     var patientId = await _irisRepository.GetPatientAsync(randomPatient); // Obtain information from the settings. Can we use random patient for each message or we need a specific one? Could we parametrize this option?
        //     var message = await _mongoRepository.GetInstrumentMessageAsync(instrument.Id); // Parse message to the expected parameters
        //     instrument.LabTestMapping.ForEach(tm =>
        //     {
        //         var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        //         instrumentTestCases.Add(new TestCase($"{instrument.Name}-{instrument.Protocol}-{tm.ExternalCode}-{tm.InstrumentSample}-{tm.InstrumentTest}")
        //         {
        //             // TODO. Obtain information about the patient --> PID|1||ANONIMO||||19911231|M||||||||||||||||||||||||
        //             // TODO. Remove hardcoded message template
        //             // TODO. Retrieve information about PatientId from settings or other source
        //             // The message structure should be created according to the protocol used by the instrument
        //             // Message = $"MSH|^~\\&|ctws^||host||20251023081551||OML^O21|{GenerateRandomControlMessageId()}||2.5|||NE|NE||8859/1\rPID|1||ANONIMO||||19911231|M||||||||||||||||||||||||\rPV1|||\rORC|NW|{orderRandomNumber}|||||^^^^^R||{currentDateTime}||||^^^|||||||||||||||||||\rOBR|1|{orderRandomNumber}||{tm.ExternalCode}|||{currentDateTime}||||A",
        //             Message = message,
        //             Assert = new Assert
        //             {
        //                 // TODO. Review assertion part
        //                 Expected = $"Expected value for {tm.InstrumentTest}",
        //                 Actual = $"Actual value for {tm.InstrumentTest}"
        //             }
        //         });
        //         orderRandomNumber++;
        //     });

        //     return instrumentTestCases;
        // }

        private async Task CreateOrdersFromLabTests()
        {
            long orderRandomNumber = new Random().Next(10000, 99999) * 10000000;
            var instrument = Module.Instruments.Where(i => i.Id == "800||1").First();

            try
            {
                
            
            // foreach(var instrument in Module.Instruments)
            // {
                var createOrderMessages = new List<IMessage>();
                var randomPatient = await GetRandomPatient();
                
                instrument.LabTestMapping.ForEach(ltm =>
                {
                    var currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var parameters = new Dictionary<string, string>
                    {
                        { "controlMessageId", GenerateRandomControlMessageId() },
                        { "patientSegment", GetPatientSegment(randomPatient) },
                        { "orderRandomNumber", orderRandomNumber.ToString() },
                        { "currentDateTime", currentDateTime},
                        { "labTestMappingexternalCode", ltm.ExternalCode}
                    };
                    var message = _communicationManager.CreateMessage(Module.Host.Protocol, MessageType.CreateOrderMessage, _icaSettings.Value.CreateOrderMessageQuery, parameters);
                    createOrderMessages.Add(message);
                    orderRandomNumber++;
                });

                var connection = _communicationManager.CreateConnection(Module.Host.Role, Module.Host.HostAddress, Module.Host.Port);
                await connection.StablishConnectionAsync();
                await connection.SendBulkMessageAsync(createOrderMessages);

                var messagesTotal = createOrderMessages.Count();
                var messagesSent = createOrderMessages.Count(m => m.IsSent);
                var messagesWithError = createOrderMessages.Count(m => m.HasError);

                Console.WriteLine($"Has been created {messagesTotal} Order Creation messages for Instrumen '{instrument.Name}'. Messages sent: {messagesSent}. Messages with error: {messagesWithError}.");
            // }
            }
            catch(Exception ex)
            {
                _logger.LogError("Something happened in CreateOrdersFromLabTests:" + ex.Message);
            }
        }

        private async Task EvaluateCreatedOrders()
        {
            throw new NotImplementedException();
        }

        private string GetPatientSegment(Patient patient)
        {
            return string.Format(_icaSettings.Value.PatientSegmentQuery, patient.Id, patient.Name, patient.Birthdate, patient.GenderToSex);
        }

        private async Task<Patient> GetRandomPatient()
        {
            var randomIndex = new Random();
            var randomPatient = _icaSettings.Value.PatientIds[randomIndex.Next(0, _icaSettings.Value.PatientIds.Count()-1)];
            return await _irisRepository.GetPatientAsync(randomPatient);
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