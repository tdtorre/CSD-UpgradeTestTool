namespace DTOs
{
    public class TestMappingDto: IDataDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string AnalyserSample { get; set; }
        public required string AnalyserTest { get; set; }
    }
}