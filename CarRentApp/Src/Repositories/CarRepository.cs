using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;

namespace CarRentApp.Src.Repositories
{
    public class CarRepository
    {
        private readonly DatabaseContext _dbContext;

        public CarRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddCar(string make, string model, int year, int horsePower, CarState carState)
        {
            Car newCar = new()
            {
                Make = make,
                Model = model,
                Year = year,
                HorsePower = horsePower,
                CarState = carState,
                Repairs = []
            };

            _dbContext.Cars.Add(newCar);
            _dbContext.SaveChanges();
        }

        public Car GetCar(int carId)
        {
            Car? car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            return car == null ? throw new KeyNotFoundException($"Car with ID {carId} not found.") : car;
        }

        public List<Car> GetCars()
        {
            return _dbContext.Cars.ToList();
        }

        public void UpdateCar(int carId, string make, string model, int year, int horsePower, CarState carState)
        {
            Car? car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
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
            Car? car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
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
