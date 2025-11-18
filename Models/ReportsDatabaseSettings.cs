namespace Models
{
    public class ReportsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ReportsCollectionName { get; set; } = null!;
    }
}
