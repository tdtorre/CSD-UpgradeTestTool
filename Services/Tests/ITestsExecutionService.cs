using Models;

namespace Services.Tests
{
    public interface ITestsExecutionService
    {
        public Task<List<TestCase>> IcaTestsExecution(List<TestCase> testCases);
    }
}