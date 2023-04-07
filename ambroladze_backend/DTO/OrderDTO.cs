namespace ambroladze_backend.DTO
{
    public class OrderDTO
    {
        public int TypeId { get; set; }
        public int ClientId { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfStart { get; set; }
    }
}
