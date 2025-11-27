namespace CSD.UpgradeTestTool.Core.Models
{
    public enum Gender
    {
        Female,
        Male
    }

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public Gender Gender { get; set; }
        public char GenderToSex
        { 
            get
            {
                return Gender == Gender.Female ? 'F' : 'M';
            } 
        }
    }
}