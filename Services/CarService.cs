using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CarRentApp.Data;
using CarRentApp.Models;

namespace CarRentApp.Services
{
    public class CarService
    {
        private readonly AppDbContext _dbContext;

        public CarService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
