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
            var execution = project.UpgradeExecutions.Last();

            report.Summary.Add($"ICA Module Execution on {execution.ExecutionDate}:");
            report.Summary.Add($"Number of instruments: {execution.IcaModule.Instruments.Count}");
            report.Summary.Add($"Number of test cases executed: {execution.IcaModule.TestCases.Count}");
            report.Summary.Add($"Number of test cases passed: {execution.IcaModule.TestCases.Where(tc => tc.Assert.IsSuccessful).Count()}");
            report.Summary.Add($"Time duration of tests execution: {execution.IcaModule.TestCases.Last().EndingAt - execution.IcaModule.TestCases.First().StartingAt}");
            report.Summary.Add($"==============================");
            report.Summary.Add($"Test Cases:");

            execution.IcaModule.TestCases.ForEach(tc =>
            {
                var status = tc.Assert.IsSuccessful ? "PASSED" : (String.IsNullOrEmpty(tc.Error) ? "FAILED" : "ERROR");
                var ifError = (status == "ERROR") ? " - Error: " + tc.Error : "";
                report.Summary.Add($"{tc.Name}: {status} (Duration: {tc.GetDuration()}){ifError}");
            });

            return report;
        }
    }
}   