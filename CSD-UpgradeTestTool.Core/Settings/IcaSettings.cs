namespace CSD.UpgradeTestTool.Core.Settings;
public class IcaSettings
{
    public string DefaultHost { get; set; } = string.Empty;
    public List<int> PatientIds { get; set; } = new List<int>();
    public string PatientSegmentQuery { get; set; } = string.Empty;
    public string LabTestMappingQuery { get; set; } = string.Empty;
    public string CreateOrderMessageQuery { get; set; } = string.Empty;
}