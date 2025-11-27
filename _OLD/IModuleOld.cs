using DTOs;

namespace Models
{
    public interface IModuleOld<T> where T : IDataDto
    {
        public IModuleDataSource<T> DataSource { get; set; } 
        public List<TestCase> TestCases { get; set; }
        public Task<List<T>> ExtractData();
    }
}