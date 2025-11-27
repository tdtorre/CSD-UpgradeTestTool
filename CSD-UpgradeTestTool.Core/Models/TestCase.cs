namespace CSD.UpgradeTestTool.Core.Models
{
    public class TestCase
    {
        public TestCase(string name)
        {
            Name = name;
        }
        
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Description { get; set; }
        public DateTime StartingAt { get; set; }
        public DateTime EndingAt { get; set; }
        public IMessage Message { get; set; }
        public Assert Assert { get; set; }
        public string Error { get; set; }

        public TimeSpan GetDuration()
        {
            return EndingAt - StartingAt;
        }
    }
}