namespace Models
{
    public class UpgradeReport
    {
        public string Afiliate { get; set; }
        public string ProjectName { get; set; }
        public DateTime GeneratedOn { get; set; }
        public List<string>? Summary { get; set; }

        public UpgradeReport(string afiliate, string projectName)
        {
            Afiliate = afiliate;
            ProjectName = projectName;
            GeneratedOn = DateTime.Now;
            Summary = [];
        }

        public static UpgradeReport Generate(UpgradeProject project)
        {
            var report = new UpgradeReport(project.Afiliate.Name, project.Name);
            
            project.UpgradeExecutions.ForEach(execution =>
            {
                report.Summary.Add($"ICA Module Execution on {execution.ExecutionDate}:");
                report.Summary.Add($"Number of instruments: {execution.IcaModule.Instruments.Count}");
                report.Summary.Add($"Number of test cases executed: {execution.IcaModule.TestCases.Count}");
                report.Summary.Add($"Number of test cases passed: {execution.IcaModule.TestCases.Where(tc => tc.Assert.IsSuccessful).Count()}");
            });

            return report;
        }
    }
}   