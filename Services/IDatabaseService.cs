using DTOs;

namespace Services
{
    public interface IDatabaseService
    {
        Task<string> ExecuteQueryAsync(string query);

        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query);

        Task<List<TestMappingDto>> GetTestMappings();

        Task<T?> ExecuteSingleOrDefaultAsync<T>(string query);
    }
}