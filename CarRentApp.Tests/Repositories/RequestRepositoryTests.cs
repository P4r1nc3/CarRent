using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CarRentApp.Models;
using CarRentApp.Repositories;
using CarRentApp.Contexts;

namespace CarRentApp.Tests
{
    public class RequestRepositoryTests
    {
        private DbContextOptions<DatabaseContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private (int carId, int userId) SetupCarAndUser(DatabaseContext context)
        {
            var carRepo = new CarRepository(context);
            carRepo.AddCar("Toyota", "Corolla", 2020, 132, CarState.Available);
            int carId = context.Cars.First().Id;

            var userRepo = new UserRepository(context);
            var user = userRepo.AddUser("John", "Doe", "john.doe@example.com", "password123", Role.Customer);
            int userId = user.Id;

            return (carId, userId);
        }

        [Fact]
        public void CreateRequest_Should_AddRequestToDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int requestId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(3);

            using (var context = new DatabaseContext(options))
            {
                var (carId, userId) = SetupCarAndUser(context);
                var requestRepo = new RequestRepository(context);
                // Act
                requestRepo.CreateRequest(carId, userId, startDate, endDate, false);
                requestId = context.Requests.First().Id;
            }

            // Assert
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                var request = requestRepo.GetRequest(requestId);
                Assert.NotNull(request);
                Assert.Equal(startDate, request.StartDate);
                Assert.Equal(endDate, request.EndDate);
                Assert.False(request.IsAccepted);
                Assert.Equal(context.Cars.First().Id, request.CarId);
                Assert.Equal(context.Users.First().Id, request.UserId);
            }
        }

        [Fact]
        public void GetRequest_Should_ReturnRequest()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int requestId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(2);

            using (var context = new DatabaseContext(options))
            {
                var (carId, userId) = SetupCarAndUser(context);
                var requestRepo = new RequestRepository(context);
                requestRepo.CreateRequest(carId, userId, startDate, endDate, true);
                requestId = context.Requests.First().Id;
            }

            // Act & Assert
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                var request = requestRepo.GetRequest(requestId);
                Assert.NotNull(request);
                Assert.True(request.IsAccepted);
            }
        }

        [Fact]
        public void GetRequest_Should_ThrowException_WhenRequestNotFound()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                // Act & Assert
                Assert.Throws<KeyNotFoundException>(() => requestRepo.GetRequest(999));
            }
        }

        [Fact]
        public void GetRequests_Should_ReturnAllRequests()
        {
            // Arrange
            var options = CreateNewContextOptions();
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                var (carId, userId) = SetupCarAndUser(context);
                requestRepo.CreateRequest(carId, userId, DateTime.Now, DateTime.Now.AddDays(2), false);
                requestRepo.CreateRequest(carId, userId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(5), true);
            }

            // Act & Assert
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                var requests = requestRepo.GetRequests();
                Assert.Equal(2, requests.Count);
            }
        }

        [Fact]
        public void UpdateRequest_Should_UpdateRequestProperties()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int requestId;
            int carId, userId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(2);

            using (var context = new DatabaseContext(options))
            {
                (carId, userId) = SetupCarAndUser(context);
                var requestRepo = new RequestRepository(context);
                requestRepo.CreateRequest(carId, userId, startDate, endDate, false);
                requestId = context.Requests.First().Id;
            }

            DateTime newStartDate = DateTime.Now.AddDays(1);
            DateTime newEndDate = DateTime.Now.AddDays(4);

            // Act
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                requestRepo.UpdateRequest(requestId, carId, userId, newStartDate, newEndDate, true);
            }

            // Assert
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                var request = requestRepo.GetRequest(requestId);
                Assert.NotNull(request);
                Assert.Equal(newStartDate, request.StartDate);
                Assert.Equal(newEndDate, request.EndDate);
                Assert.True(request.IsAccepted);
            }
        }

        [Fact]
        public void RemoveRequest_Should_RemoveRequestFromDatabase()
        {
            // Arrange
            var options = CreateNewContextOptions();
            int requestId;
            using (var context = new DatabaseContext(options))
            {
                var (carId, userId) = SetupCarAndUser(context);
                var requestRepo = new RequestRepository(context);
                requestRepo.CreateRequest(carId, userId, DateTime.Now, DateTime.Now.AddDays(2), false);
                requestId = context.Requests.First().Id;
            }

            // Act
            using (var context = new DatabaseContext(options))
            {
                var requestRepo = new RequestRepository(context);
                requestRepo.RemoveRequest(requestId);
            }

            // Assert
            using (var context = new DatabaseContext(options))
            {
                var request = context.Requests.FirstOrDefault(r => r.Id == requestId);
                Assert.Null(request);
            }
        }
    }
}
