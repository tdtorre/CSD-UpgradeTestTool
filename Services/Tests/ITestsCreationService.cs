using Models;

namespace Services.Tests
{
    public interface ITestsCreationService
    {
        public Task<List<TestCase>> IcaTestsCreation(List<Instrument> instruments);
    }
}