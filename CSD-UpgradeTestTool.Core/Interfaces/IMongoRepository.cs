using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IMongoRepository
    {
        Task<IMessage> GetInstrumentMessageAsync(string instrumentId);
    }
}   