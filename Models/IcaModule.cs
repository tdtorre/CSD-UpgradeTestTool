using DTOs;

namespace Models
{
    public class IcaModule : IIcaModule
    {
        private string TEST_MAPPING_QUERY = "SELECT distinct i.Id, i.Name, AnalyserSample, AnalyserTest " +
                                            "FROM SQLUser.tconTestMapping AS tm " +
                                            "LEFT JOIN SQLUser.tInstruments AS i ON i.id = tm.AnalyserCode||'||'||tm.AnalyserNumber " +
                                            "WHERE i.qc = 1";
        private readonly IConfiguration _config;
        public IModuleDataSource<TestMappingDto> DataSource { get; set; }
        public List<TestCase> TestCases { get; set; }
        public List<Instrument> Instruments { get; set; } = new List<Instrument>();
        public List<string> Samples { get; set; }

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