namespace ambroladze_backend.Models
{
    public class TypeOfWork
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public double Cost { get; set; }
    }
}
