using Services.Protocols;
using static Services.Protocols.BaseProtocol;

namespace Models
{
    public enum InstrumentType
    {
        CobasPRO,
        CobasT511,
        Cobas6500,
        CobasE411,
    }

    public class Instrument
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public InstrumentType Type { get; set; }
        public List<TestMapping> TestMapping { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public Instrument(string id, string name, string host, int port)
        {
            this.Id = id;
            this.Name = name;
            this.Host = host;
            this.Port = port;
            this.TestMapping = new List<TestMapping>();
            this.Type = ParseNameToInstrumentType(name);
        }
        
        public ProtocolType GetProtocolType()
        {
            switch (this.Type)
            {
                case InstrumentType.CobasPRO:
                case InstrumentType.CobasT511:
                    return ProtocolType.Hl7;
                case InstrumentType.Cobas6500:
                case InstrumentType.CobasE411:
                    return ProtocolType.Astm;
                default:
                    throw new NotSupportedException($"Instrument '{this.Type}' does not have a defined protocol type.");
            }
        }
        
        private InstrumentType ParseNameToInstrumentType(string name)
        {
            return Enum.Parse<InstrumentType>(name.Replace(" ", ""), true);
        } 
    }
}