vvusing Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CarRentApp.Models;

namespace CarRentApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO This will need to be retrieved from environment variables or a local configuration file
            optionsBuilder.UseMySql(
                "server=localhost;database=car-rent;user=root;password=admin12345",
                new MySqlServerVersion(new Version(8, 0, 32))
            );
        }
    }
}