using Microsoft.Extensions.Configuration;
using Models;

namespace Services.Tests
{
    public class TestValidationService : ITestValidationService
    {
        private readonly IDatabaseService _databaseService;

        private const int BATCH_SIZE = 50;

        public TestValidationService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<TestCase>> ValidateOrdersInDb(List<TestCase> testCases)
        {
            var current_batch = 0;
            await Task.Delay(10000);
            while (testCases.Count > current_batch * BATCH_SIZE)
            {
                var testCasesBatch = testCases.Skip(current_batch * BATCH_SIZE).Take(BATCH_SIZE);
                var query = $"SELECT ExtSampleID FROM tINTOrders where ExtSampleID IN ({string.Join(",", testCasesBatch.Select(p => $"{p.Assert.SampleId}"))})";
                var existIds = await _databaseService.ExecuteQueryAsync<string>(query);

                var notExistIds = testCasesBatch.Select(tc => tc.Assert.SampleId).Except(existIds.ToList()).ToList();

                notExistIds.ForEach(tc =>
                {
                    testCases.First(itc => itc.Assert.SampleId == tc).Assert.Type = AssertType.NotExistInDb;
                });
                current_batch++;
            }
            return testCases;
        }
    }
}
