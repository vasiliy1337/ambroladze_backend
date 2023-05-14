using System.ComponentModel.DataAnnotations;
using ambroladze_backend.DTO;

namespace ambroladze_backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TypeOfWorkId { get; set; }
        public TypeOfWork? TypeOfWork { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public string ?Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfStart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfEnd { get; set; }

        public Order() { }

        public Order(OrderDTO odto)
        {
            TypeOfWorkId = odto.TypeId;
            ClientId = odto.ClientId;
            Address = odto.Address;
            DateOfStart= odto.DateOfStart;
        }

    }
}
