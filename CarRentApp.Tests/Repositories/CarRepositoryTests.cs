using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Tests.Repositories
{
    public class CarRepositoryTests
    {
        private DbContextOptions<DatabaseContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AddCar_Should_AddCarToDatabase()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                // Act
                repository.AddCar("Toyota", "Corolla", 2020, 132, CarState.Available);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                Car? car = context.Cars.FirstOrDefault();
                Assert.NotNull(car);
                Assert.Equal("Toyota", car.Make);
                Assert.Equal("Corolla", car.Model);
                Assert.Equal(2020, car.Year);
                Assert.Equal(132, car.HorsePower);
                Assert.Equal(CarState.Available, car.CarState);
            }
        }

        [Fact]
        public void GetCar_Should_ReturnCar()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int carId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                repository.AddCar("Honda", "Civic", 2019, 120, CarState.Available);
                carId = context.Cars.First().Id;
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                Car car = repository.GetCar(carId);
                Assert.NotNull(car);
                Assert.Equal("Honda", car.Make);
                Assert.Equal("Civic", car.Model);
            }
        }

        [Fact]
        public void GetCar_Should_ThrowException_WhenCarNotFound()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);
            CarRepository repository = new CarRepository(context);
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetCar(999));
        }

        [Fact]
        public void GetCars_Should_ReturnAllCars()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                repository.AddCar("Toyota", "Corolla", 2020, 132, CarState.Available);
                repository.AddCar("Honda", "Civic", 2019, 120, CarState.Available);
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                List<Car> cars = repository.GetCars();
                Assert.Equal(2, cars.Count);
            }
        }

        [Fact]
        public void UpdateCar_Should_UpdateCarProperties()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int carId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                repository.AddCar("Toyota", "Corolla", 2020, 132, CarState.Available);
                carId = context.Cars.First().Id;
            }

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                repository.UpdateCar(carId, "Toyota", "Camry", 2021, 150, CarState.Reserved);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                Car? car = context.Cars.FirstOrDefault(c => c.Id == carId);
                Assert.NotNull(car);
                Assert.Equal("Toyota", car.Make);
                Assert.Equal("Camry", car.Model);
                Assert.Equal(2021, car.Year);
                Assert.Equal(150, car.HorsePower);
                Assert.Equal(CarState.Reserved, car.CarState);
            }
        }

        [Fact]
        public void RemoveCar_Should_RemoveCarFromDatabase()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int carId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                repository.AddCar("Ford", "Focus", 2018, 110, CarState.Available);
                carId = context.Cars.First().Id;
            }

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                CarRepository repository = new CarRepository(context);
                repository.RemoveCar(carId);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                Car? car = context.Cars.FirstOrDefault(c => c.Id == carId);
                Assert.Null(car);
            }
        }
    }
}
