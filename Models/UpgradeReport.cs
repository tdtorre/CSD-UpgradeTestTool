namespace Models
{
    public class UpgradeReport
    {
        public string Afiliate { get; set; }
        public string ProjectName { get; set; }
        public DateTime GeneratedOn { get; set; }
        public List<string> Summary { get; set; }

        public static UpgradeReport Generate(UpgradeProject project)
        {
            var report = new UpgradeReport();
            report.Afiliate = project.Afiliate.Name;
            report.ProjectName = project.Name;
            report.GeneratedOn = DateTime.Now;
            report.Summary = new List<string>();

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