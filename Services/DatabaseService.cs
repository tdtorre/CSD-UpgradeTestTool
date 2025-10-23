using System.Text;
using Dapper;
using DTOs;
using InterSystems.Data.IRISClient;

namespace Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IConfiguration _config;

        public DatabaseService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> ExecuteQueryAsync(string query)
        {
            var connectionString = _config.GetConnectionString("IrisDb");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("DB connection string 'IrisDb' is not configured.");
            using var irisConnection = new IRISConnection(connectionString);
            try
            {
                irisConnection.Open();
                using var dbCommand = new IRISCommand(query, irisConnection);
                using var reader = dbCommand.ExecuteReader();
                var response = new StringBuilder();
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        response.Append(reader.GetName(i)).Append('=').Append(reader.GetValue(i)?.ToString() ?? "NULL");
                        if (i < reader.FieldCount - 1) response.Append(';');
                    }
                    response.AppendLine();
                }

                return response.ToString();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<List<TestMappingDto>> GetTestMappings()
        {
            var connectionString = _config.GetConnectionString("IrisDb");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("DB connection string 'IrisDb' is not configured.");
            using var irisConnection = new IRISConnection(connectionString);
            try
            {
                irisConnection.Open();
                var testMappings = irisConnection.Query<TestMappingDto>("SELECT distinct AnalyserSample,AnalyserTest FROM SQLUser.tconTestMapping");

                if (testMappings != null)
                {
                    var analyzers = testMappings.DistinctBy(m => m.AnalyserSample);
                    Console.WriteLine($"Analysers found: {analyzers.Count()}");
                    foreach (var mapping in testMappings)
                    {
                        Console.WriteLine($"Analyzer: {mapping.AnalyserSample}   Test: {mapping.AnalyserTest}");
                    }

                    return testMappings.ToList();
                }

                return new List<TestMappingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return new List<TestMappingDto>();
            }
        }
    }
}