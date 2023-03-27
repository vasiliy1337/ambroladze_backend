using System.ComponentModel.DataAnnotations;

namespace ambroladze_backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        //
        public string UserName { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfEnd { get; set; }
    }
}
