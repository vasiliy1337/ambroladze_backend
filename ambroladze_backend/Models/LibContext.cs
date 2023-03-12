using Microsoft.EntityFrameworkCore;

namespace ambroladze_backend.Models
{
    public class LibContext : DbContext
    {
        public LibContext(DbContextOptions<LibContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
        */

        public DbSet<Book> Books { get; set; }
        
        public DbSet<User> Users { get; set; }
    }
}
