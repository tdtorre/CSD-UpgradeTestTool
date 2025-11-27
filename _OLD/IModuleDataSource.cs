using DTOs;

namespace Models
{
    public interface IModuleDataSource<T> where T : IDataDto
    {
        public Task<List<T>> GetData(string query);
    }
}