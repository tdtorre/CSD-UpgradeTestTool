using CSD.UpgradeTestTool.Core.Interfaces;
using CSD.UpgradeTestTool.Infrastructure.Communication;
using Microsoft.Extensions.Configuration;
using ProtocolType = CSD.UpgradeTestTool.Core.Models.ProtocolType;

namespace CSD.UpgradeTestTool.Core.Services.Protocols
{
    public class ProtocolServiceFactory
    {
        public static IProtocolService GetProtocolService(ProtocolType protocolType, IConfiguration configuration)
        {
            switch (protocolType)
            {
                case ProtocolType.Astm:
                    return new AstmService(configuration);
                case ProtocolType.Hl7:
                    return new Hl7Service(configuration);
                default:
                    throw new NotSupportedException($"Protocol type '{protocolType}' doesn't exist in the Service Factory.");
            }
        }
    }
}