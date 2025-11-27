using CSD.UpgradeTestTool.Infrastructure.Data.DbContext;
using Microsoft.Extensions.Logging;
using Dapper;
using Microsoft.Extensions.Options;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using CSD.UpgradeTestTool.Infrastructure.Data.DTOs;
using CSD.UpgradeTestTool.Core.Settings;

namespace CSD.UpgradeTestTool.Infrastructure.Data.Repositories
{
    public class  IcaRepository: IIcaRepository
    {
        private readonly IrisDbContext _irisDbContext;
        private readonly IOptions<IcaSettings> _icaSettings;
        private readonly ILogger<IcaRepository> _logger;

        public IcaRepository(IrisDbContext irisDbContext, 
                             IOptions<IcaSettings> icaSettings,
                             ILogger<IcaRepository> logger)
        {
            _irisDbContext = irisDbContext ?? throw new ArgumentNullException(nameof(irisDbContext));
            _icaSettings = icaSettings ?? throw new ArgumentNullException(nameof(icaSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<IEnumerable<LabTestMapping>> GetLabTestMappingsAsync()
        {
            _logger.LogInformation("Executing TestMappings query against database.");

            try{
                if (_irisDbContext?.Connection == null)
                        throw new InvalidOperationException("Database connection is not initialized.");

                var testMappings = new List<LabTestMapping>();
                var query = string.Format(_icaSettings.Value.LabTestMappingQuery, _icaSettings.Value.DefaultHost);
                var data = await _irisDbContext.Connection.QueryAsync<TestMappingDto>(query);

                if (data != null && data.Any())
                    testMappings = data.Select(d => new LabTestMapping() { InstrumentId = d.Id, InstrumentTest = d.AnalyserTest, InstrumentSample = d.AnalyserSample, ExternalCode = d.ExternalCode }).ToList();

                _logger.LogInformation($"Retrieved {testMappings.Count} test mappings from database.");
                return testMappings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTestMappingsAsync failed");
                return Enumerable.Empty<LabTestMapping>();
            }
        }
    }
}   