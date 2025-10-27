using DTOs;

namespace Models
{
    public class IcaModule : IIcaModule
    {
        private string TEST_MAPPING_QUERY = $"SELECT distinct rInstruments AS Id, Test, AnalyserTest,AnalyserSample, rInstruments->Name " +
                                            "FROM tconTestMapping AS tm " +
                                            "LEFT JOIN tTests AS t ON tm.Test=t.id " +
                                            "WHERE rInstruments->status=1 AND tTests.status=1" + 
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