using System.Text;
using System.Security.Cryptography;
using ambroladze_backend.DTO;

namespace ambroladze_backend.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        private byte[] password { get; set; }
        public string Password {
            get
            {
                var sb = new StringBuilder();
                foreach (var b in MD5.HashData(password))
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            set { password = Encoding.UTF8.GetBytes(value); }
        }
        public string? PhoneNumber { get; set; }

        public bool IsAdmin => Login == "admin";

        //public bool IsWorker => Login.StartsWith("woker_");

        public bool CheckPassword(string password) => Password == password;

        public List<Order>? Orders { get; set; }

        public Client(){}
        
        public Client(ClientDTO udto)
        {
            Name = udto.Name;
            Login = udto.Login;
            Email = udto.Email;
            Password = udto.Password;
            PhoneNumber = udto.PhoneNumber;
        }

    }
}
