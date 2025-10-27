using Services.Protocols;
using static Services.Protocols.BaseProtocol;

namespace Models
{
    public class TestCase
    {
        public TestCase(string name)
        {
            Name = name;
        }
        
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; }
        public string Description { get; set; }
        public DateTime StartingAt { get; set; }
        public DateTime EndingAt { get; set; }
        public string Message { get; set; }
        public Assert Assert { get; set; }
        public ProtocolType ProtocolType { get; internal set; }

        public TimeSpan GetDuration()
        {
            return EndingAt - StartingAt;
        }
    }
}