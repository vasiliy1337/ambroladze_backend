namespace ambroladze_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }

        //public Book Book;

        public User(string Name, string Email, string Password) { 
            this.Name = Name;
            this.Email = Email; 
            this.Password = Password;
        }
        
        /*
        void AddBook(Book book)
        {
            this.Book = book;
        }
        */
    }
}
