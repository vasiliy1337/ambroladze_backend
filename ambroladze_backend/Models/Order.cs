namespace ambroladze_backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
