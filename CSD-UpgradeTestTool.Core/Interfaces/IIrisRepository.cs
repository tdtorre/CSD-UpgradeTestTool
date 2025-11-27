using CSD.UpgradeTestTool.Core.Models;

namespace CSD.UpgradeTestTool.Core.Interfaces
{
    public interface IIrisRepository
    {
        Task<Affiliate> GetAffiliateAsync();
        Task<Patient> GetPatientAsync(int patientId);
        Task<Instrument> GetInstrumentByIdAsync(string instrumentId);
        Task<IEnumerable<Instrument>> GetInstrumentsAsync();
        Task<Host> GetHostByIdAsync(string hostId);
        Task<IEnumerable<Host>> GetHostsAsync();
    }
}   