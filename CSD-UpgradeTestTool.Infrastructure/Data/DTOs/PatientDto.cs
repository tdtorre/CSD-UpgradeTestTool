namespace CSD.UpgradeTestTool.Infrastructure.Data.DTOs
{
    public class PatientDto
    {
        public required string id { get; set; }
        public required DateTime dateOfBirth { get; set; }
        public required string firstname { get; set; }
        public required string lasstname { get; set; }
        public required char sex { get; set; }
    }
}