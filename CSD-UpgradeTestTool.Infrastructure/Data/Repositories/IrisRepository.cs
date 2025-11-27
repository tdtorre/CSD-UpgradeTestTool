using CSD.UpgradeTestTool.Infrastructure.Data.DbContext;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CSD.UpgradeTestTool.Infrastructure.Settings;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using Dapper;
using CSD.UpgradeTestTool.Infrastructure.Data.DTOs;

namespace CSD.UpgradeTestTool.Infrastructure.Data.Repositories
{
    public class  IrisRepository: IIrisRepository
    {
        private readonly IrisDbContext _irisDbContext;
        private readonly IOptions<IrisDbSettings> _irisDbSettings;
        private readonly IOptions<CommonDataSettings> _commonDataSettings;
        private readonly ILogger<IrisRepository> _logger;

        public IrisRepository(IrisDbContext irisDbContext, 
                                   IOptions<IrisDbSettings> irisDbSettings,
                                   IOptions<CommonDataSettings> commonDataSettings,
                                   ILogger<IrisRepository> logger)
        {
            _irisDbContext = irisDbContext ?? throw new ArgumentNullException(nameof(irisDbContext));
            _irisDbSettings = irisDbSettings ?? throw new ArgumentNullException(nameof(irisDbSettings));
            _commonDataSettings = commonDataSettings ?? throw new ArgumentNullException(nameof(commonDataSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<Affiliate> GetAffiliateAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Patient> GetPatientAsync(int patientId)
        {
            _logger.LogInformation($"Getting patient '{patientId}' from IRIS database.");

            try
            {
                if (_irisDbContext?.Connection == null)
                        throw new InvalidOperationException("Database connection is not initialized.");

                var query = string.Format(_commonDataSettings.Value.GetPatientQuery, patientId);
                var data = await _irisDbContext.Connection.QueryAsync<PatientDto>(query);

                if (data != null && data.Any())
                {
                    _logger.LogInformation($"Retrieved patient {patientId} from database.");
                    return data.Select(d => new Patient(){ Id = patientId, Name = d.firstname, Birthdate = d.dateOfBirth, Gender = d.sex == 'F' ? Gender.Female : Gender.Male }).First();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPatientAsync failed");
            }

            throw new InvalidOperationException($"Patient '{patientId}' not found in the database.");
        }

        public async Task<IEnumerable<Host>> GetHostsAsync()
        {
            _logger.LogInformation("Getting Hosts list from IRIS database.");

            try
            {
                if (_irisDbContext?.Connection == null)
                    throw new InvalidOperationException("Database connection is not initialized.");

                var hosts = new List<Host>();
                var query = string.Format(_commonDataSettings.Value.GetHostsQuery, _irisDbSettings.Value.Server);
                var data = await _irisDbContext.Connection.QueryAsync<HostDto>(query);

                if (data != null && data.Any())
                {
                    // TODO. Review Protocol and Role
                    hosts = data.Select(d => new Host(d.HostName, d.ConnectionName, d.IPaddress, d.TCPPort, ProtocolType.Hl7, CommunicationRole.Client)).ToList();
                }                    

                _logger.LogInformation($"Retrieved {hosts.Count} hosts from database.");
                return hosts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetHostsAsync failed");
                return Enumerable.Empty<Host>();
            }
        }

        public async Task<IEnumerable<Instrument>> GetInstrumentsAsync()
        {
            _logger.LogInformation("Getting Instruments list from IRIS database.");

            try
            {
                if (_irisDbContext?.Connection == null)
                    throw new InvalidOperationException("Database connection is not initialized.");

                var instruments = new List<Instrument>();
                var query = string.Format(_commonDataSettings.Value.GetInstrumentsQuery, _irisDbSettings.Value.Server);
                var data = await _irisDbContext.Connection.QueryAsync<InstrumentDto>(query);

                if (data != null && data.Any())
                {
                    // TODO. We need to handle the protocol of each instrument
                    instruments = data.Select(d => new Instrument(d.ID, d.rInstrumentTypes, d.HostAddress, d.ConnectionDesc)).ToList();
                }                    

                _logger.LogInformation($"Retrieved {instruments.Count} instruments from database.");
                return instruments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInstrumentsAsync failed");
                return Enumerable.Empty<Instrument>();
            }
        }

        public async Task<Instrument> GetInstrumentByIdAsync(string instrumentId)
        {
            _logger.LogInformation($"Getting Instrument '{instrumentId}' from IRIS database.");

            try
            {
                if (_irisDbContext?.Connection == null)
                    throw new InvalidOperationException("Database connection is not initialized.");

                var instruments = new List<Instrument>();
                var query = string.Format(_commonDataSettings.Value.GetInstrumentByIdQuery, _irisDbSettings.Value.Server, instrumentId);
                var data = await _irisDbContext.Connection.QueryAsync<InstrumentDto>(query);

                if (data == null || !data.Any())
                    throw new InvalidOperationException($"Instrument '{instrumentId}' was not found in the database.");
                
                return data.Select(d => new Instrument(d.ID, d.rInstrumentTypes, d.HostAddress, d.ConnectionDesc)).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInstrumentByIdAsync failed");
                return null;
            }
        }

        public async Task<Host> GetHostByIdAsync(string hostId)
        {
            _logger.LogInformation($"Getting Host '{hostId}' from IRIS database.");

            try
            {
                if (_irisDbContext?.Connection == null)
                    throw new InvalidOperationException("Database connection is not initialized.");

                var instruments = new List<Host>();
                var query = string.Format(_commonDataSettings.Value.GetHostByIdQuery, _irisDbSettings.Value.Server, hostId);
                var data = await _irisDbContext.Connection.QueryAsync<HostDto>(query);

                if (data == null || !data.Any())
                    throw new InvalidOperationException($"Host '{hostId}' was not found in the database.");

                // TODO. Review Protocol and Role
                return data.Select(d => new Host(d.HostName, d.ConnectionName, d.IPaddress, d.TCPPort, ProtocolType.Hl7, CommunicationRole.Client)).First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetHostByIdAsync failed");
                return null;
            }
        }
    }
}   