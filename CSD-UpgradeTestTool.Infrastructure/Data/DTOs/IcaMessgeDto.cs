namespace CSD.UpgradeTestTool.Infrastructure.Data.DTOs
{
    public class IcaMessageDto
    {
        public string InstrumentId { get; set; }
        public string Protocol { get; set; }
        public string QueryMessage { get; set; }
        public string QueryReplyMessage { get; set; }
        public string PatientResultMessage { get; set; }
    }
}