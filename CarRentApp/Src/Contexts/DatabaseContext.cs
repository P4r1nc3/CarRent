using CarRentApp.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Src.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairItem> RepairItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? connectionString = Environment.GetEnvironmentVariable("CAR_RENT_DB_CONNECTION");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("The connection string was not found in the environment variables.");
                }

                optionsBuilder.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32))
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repair>()
                .HasMany(r => r.RepairItems)
                .WithOne(ri => ri.Repair)
                .HasForeignKey(ri => ri.RepairId);

            modelBuilder.Entity<Repair>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Repairs)
                .HasForeignKey(r => r.CarId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
