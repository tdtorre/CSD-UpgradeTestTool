using DTOs;

namespace Services
{
    public interface IDatabaseService
    {
        Task<string> ExecuteQueryAsync(string query);

        List<TestMappingDto> GetTestMappings();
    }
}