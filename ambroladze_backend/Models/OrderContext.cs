using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ambroladze_backend.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options)
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

        public DbSet<TypeOfWork> TypesOfWork { get; set; }
        
        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        //public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        //{
        //    public DateOnlyConverter()
        //        : base(dateOnly =>
        //                dateOnly.ToDateTime(TimeOnly.MinValue),
        //            dateTime => DateOnly.FromDateTime(dateTime))
        //    { }
        //}

        //protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        //{

        //    builder.Properties<DateOnly>()
        //        .HaveConversion<DateOnlyConverter>()
        //        .HaveColumnType("date");

        //    base.ConfigureConventions(builder);

        //}
    }
}
