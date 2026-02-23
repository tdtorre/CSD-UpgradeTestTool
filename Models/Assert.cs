namespace Models
{
    public enum AssertType
    {
        None,
        NotExistInDb
    }

    public class Assert
    {
        public string Expected { get; set; }
        public string Actual { get; set; }
        public string SampleId { get; set; }
        public bool IsSuccessful { get; set; }

        public AssertType Type { get; set; }
    }
}