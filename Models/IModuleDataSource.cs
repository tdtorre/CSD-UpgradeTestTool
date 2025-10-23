using DTOs;

namespace Models
{
    public interface IModuleDataSource<T> where T : IDataDto
    {
        public List<T> GetData(string query);
    }
}