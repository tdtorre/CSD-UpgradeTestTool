namespace CSD.UpgradeTestTool.Core.Models
{
    public class Assert
    {
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool IsSuccessful { get; set; }
        public AssertType Type { get; set; }
    }
}