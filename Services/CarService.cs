using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CarRentApp.Models;
using CarRentApp.Context;

namespace CarRentApp.Services
{
    public class CarService
    {
        private readonly DatabaseContext _dbContext;

        public CarService(DatabaseContext dbContext)
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

        public Car GetCar(int carId)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }
            return car;
        }

        public List<Car> GetCars()
        {
            return _dbContext.Cars.ToList();
        }

        public void UpdateCar(int carId, string make, string model, int year, int horsePower, CarState carState)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            if (car != null)
            {
                car.Make = make;
                car.Model = model;
                car.Year = year;
                car.HorsePower = horsePower;
                car.CarState = carState;

                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }
        }

        public void RemoveCar(int carId)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            if (car != null)
            {
                _dbContext.Cars.Remove(car);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }
        }
    }
}
