using Xunit;
using CarRentApp.Models;

namespace CarRentApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestCarInitialization()
        {
            // Arrange
            var car = new Car
            {
                Make = "Toyota",
                Model = "Corolla",
                Year = 2022,
                HorsePower = 120,
                CarState = CarState.Available
            };

            // Act & Assert
            Assert.Equal("Toyota", car.Make);
            Assert.Equal("Corolla", car.Model);
            Assert.Equal(2022, car.Year);
            Assert.Equal(120, car.HorsePower);
            Assert.Equal(CarState.Available, car.CarState);
        }
    }
}
