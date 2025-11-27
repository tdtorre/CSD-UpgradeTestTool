using System.Runtime.InteropServices.Marshalling;
using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Core.Models;
using CSD.UpgradeTestTool.Infrastructure.Communication.Messages;
using CSD.UpgradeTestTool.Infrastructure.Data.DbContext;
using CSD.UpgradeTestTool.Infrastructure.Data.DTOs;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CSD.UpgradeTestTool.Infrastructure.Data.Repositories;

public class MongoRepository : IMongoRepository
{
    private readonly MongoDbContext _mongoDbContext;
    private readonly ILogger<MongoRepository> _logger;

    public MongoRepository(MongoDbContext mongoDbContext, ILogger<MongoRepository> logger)
    {
        _mongoDbContext = mongoDbContext ?? throw new ArgumentNullException(nameof(mongoDbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));         
    }

    public async Task<IMessage> GetInstrumentMessageAsync(string instrumentId)
    {
        var icaMessagesCollection = _mongoDbContext.GetCollection<IcaMessageDto>("IcaMessages");
        var icaMessage = await icaMessagesCollection.Find(m => m.InstrumentId == instrumentId).FirstOrDefaultAsync();

        if(icaMessage == null)
        {
            throw new InvalidOperationException($"There is no message configured for Instrument ID: '{instrumentId}'."); 
        }
        
        if(icaMessage.Protocol.ToUpper() == "ASTM")
        {
            return new AstmMessage(MessageType.QueryMessage, icaMessage.QueryMessage);
        }

        if(icaMessage.Protocol.ToUpper() == "HL7")
        {
            return new Hl7Message(MessageType.QueryMessage, icaMessage.QueryMessage);
        }

        throw new InvalidOperationException($"The Protocol '{icaMessage.Protocol}' doesn't match with any of the existing options."); 
    }
}