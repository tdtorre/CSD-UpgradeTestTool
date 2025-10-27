using DTOs;

namespace Models
{
    public class IcaModule : IIcaModule
    {
        // TODO. Remove this hardcoded query and also the filter by HOSTNAME
        private string TEST_MAPPING_QUERY = $"SELECT distinct rInstruments as Id, tconTestMapping.Test, AnalyserTest, AnalyserSample, rInstruments->Name, hca.ExternalCode " +
                                            "FROM tconTestMapping " +
                                            "LEFT JOIN tTests ON tconTestMapping.Test=tTests.id " +
                                            "LEFT JOIN thcaIETestMapping hca ON tTests.id=hca.internalcode " +
                                            "WHERE rInstruments->status=1 AND tTests.status=1 and rhcaIEHostConfig->rhcaHost->Hostname='HL7_CTWS' " +
                                            "ORDER BY rInstruments, AnalyserSample,Test";
        private readonly IConfiguration _config;
        public IModuleDataSource<TestMappingDto> DataSource { get; set; }
        public List<TestCase> TestCases { get; set; }
        public List<Instrument> Instruments { get; set; } = new List<Instrument>();

        public IcaModule(IConfiguration config)
        {
            _config = config;
            DataSource = new DbDataSource<TestMappingDto>(_config);
        }
        
        public async Task<List<TestMappingDto>> ExtractData()
        {
            return await DataSource.GetData(TEST_MAPPING_QUERY);
        }
    }
}