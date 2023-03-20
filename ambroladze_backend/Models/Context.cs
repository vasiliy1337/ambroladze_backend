using Microsoft.EntityFrameworkCore;

namespace ambroladze_backend.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
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

        public DbSet<Order> Orders { get; set; }
        
        public DbSet<User> Users { get; set; }
    }
}
