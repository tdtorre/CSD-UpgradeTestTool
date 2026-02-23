using Models;

namespace Services.Tests
{
    public interface ITestValidationService
    {
        Task<List<TestCase>> ValidateOrdersInDb(List<TestCase> testCases);
    }
}
