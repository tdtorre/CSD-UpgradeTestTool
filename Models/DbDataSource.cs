using DTOs;
using Dapper;
using InterSystems.Data.IRISClient;

namespace Models
{
    public class DbDataSource<T> : IModuleDataSource<T> where T : IDataDto
    {
        private readonly IConfiguration _config;

        public DbDataSource(IConfiguration config)
        {
            _config = config;
        }
        
        public List<T> GetData(string query)
        {
            var connectionString = _config.GetConnectionString("IrisDb");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("DB connection string 'IrisDb' is not configured.");
            
            using var irisConnection = new IRISConnection(connectionString);
            try
            {
                irisConnection.Open();
                var result = irisConnection.Query<T>(query);

                if (result != null)
                {
                    return result.ToList();
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                throw new Exception("GetData failed: " + ex.Message);
            }
        }
    }
}