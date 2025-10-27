namespace Models
{
    public class Order
    {
        public string Number { get; set; }
        public List<Sample> Samples { get; set; }
    }
}