using ambroladze_backend.DTO;

namespace ambroladze_backend.Models
{
    public class TypeOfWork
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public double Cost { get; set; }

        public List<Order>? Orders { get; set; }

        public TypeOfWork() { }

        public TypeOfWork(TypeOfWorkDTO tpdto)
        {
            Name = tpdto.Name;
            Description = tpdto.Description;
            Duration = tpdto.Duration;
            Cost = tpdto.Cost;
        } 
    }
}
