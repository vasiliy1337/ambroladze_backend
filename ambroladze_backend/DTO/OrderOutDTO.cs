namespace ambroladze_backend.DTO
{
    public class OrderOutDTO
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }
        public string? TypeName { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfStart { get; set; }
        public DateTime DateOfEnd { get; set; }
    }
}
