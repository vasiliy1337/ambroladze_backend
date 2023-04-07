using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Metadata;

namespace ambroladze_backend.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<TypeOfWork> TypesOfWork { get; set; }
        
        public DbSet<Client> Clients { get; set; }

        public DbSet<Order> Orders { get; set; }



    }
}
