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

        public void AddCar(string make, string model, int year, int horsePower, CarState carState)
        {
            var newCar = new Car
            {
                Make = make,
                Model = model,
                Year = year,
                HorsePower = horsePower,
                CarState = carState
            };

            _dbContext.Cars.Add(newCar);
            _dbContext.SaveChanges();
        }

        public List<Car> GetCars()
        {
            return _dbContext.Cars.ToList();
        }
    }
}
