using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Tests.Repositories
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
            CarRepository carRepo = new CarRepository(context);
            carRepo.AddCar("Toyota", "Corolla", 2020, 132, CarState.Available);
            int carId = context.Cars.First().Id;

            UserRepository userRepo = new UserRepository(context);
            User user = userRepo.AddUser("John", "Doe", "john.doe@example.com", "password123", Role.Customer);
            int userId = user.Id;

            return (carId, userId);
        }

        [Fact]
        public void CreateRequest_Should_AddRequestToDatabase()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int requestId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(3);

            using (DatabaseContext context = new DatabaseContext(options))
            {
                (int carId, int userId) = SetupCarAndUser(context);
                RequestRepository requestRepo = new RequestRepository(context);
                // Act
                requestRepo.CreateRequest(carId, userId, startDate, endDate, RequestState.Requested);
                requestId = context.Requests.First().Id;
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                Request request = requestRepo.GetRequest(requestId);
                Assert.NotNull(request);
                Assert.Equal(startDate, request.StartDate);
                Assert.Equal(endDate, request.EndDate);
                Assert.Equal(RequestState.Requested, request.RequestState);
                Assert.Equal(context.Cars.First().Id, request.CarId);
                Assert.Equal(context.Users.First().Id, request.UserId);
            }
        }

        [Fact]
        public void GetRequest_Should_ReturnRequest()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int requestId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(2);

            using (DatabaseContext context = new DatabaseContext(options))
            {
                (int carId, int userId) = SetupCarAndUser(context);
                RequestRepository requestRepo = new RequestRepository(context);
                requestRepo.CreateRequest(carId, userId, startDate, endDate, RequestState.Rented);
                requestId = context.Requests.First().Id;
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                Request request = requestRepo.GetRequest(requestId);
                Assert.NotNull(request);
                Assert.Equal(RequestState.Rented, request.RequestState);
            }
        }

        [Fact]
        public void GetRequest_Should_ThrowException_WhenRequestNotFound()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);
            RequestRepository requestRepo = new RequestRepository(context);
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => requestRepo.GetRequest(999));
        }

        [Fact]
        public void GetRequests_Should_ReturnAllRequests()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                (int carId, int userId) = SetupCarAndUser(context);
                requestRepo.CreateRequest(carId, userId, DateTime.Now, DateTime.Now.AddDays(2), RequestState.Rented);
                requestRepo.CreateRequest(carId, userId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(5), RequestState.Rented);
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                List<Request> requests = requestRepo.GetRequests();
                Assert.Equal(2, requests.Count);
            }
        }

        [Fact]
        public void UpdateRequest_Should_UpdateRequestProperties()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int requestId;
            int carId, userId;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(2);

            using (DatabaseContext context = new DatabaseContext(options))
            {
                (carId, userId) = SetupCarAndUser(context);
                RequestRepository requestRepo = new RequestRepository(context);
                requestRepo.CreateRequest(carId, userId, startDate, endDate, RequestState.Rented);
                requestId = context.Requests.First().Id;
            }

            DateTime newStartDate = DateTime.Now.AddDays(1);
            DateTime newEndDate = DateTime.Now.AddDays(4);

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                requestRepo.UpdateRequest(requestId, carId, userId, newStartDate, newEndDate, RequestState.Rented);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                Request request = requestRepo.GetRequest(requestId);
                Assert.NotNull(request);
                Assert.Equal(newStartDate, request.StartDate);
                Assert.Equal(newEndDate, request.EndDate);
                Assert.Equal(RequestState.Rented, request.RequestState);
            }
        }

        [Fact]
        public void RemoveRequest_Should_RemoveRequestFromDatabase()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int requestId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                (int carId, int userId) = SetupCarAndUser(context);
                RequestRepository requestRepo = new RequestRepository(context);
                requestRepo.CreateRequest(carId, userId, DateTime.Now, DateTime.Now.AddDays(2), RequestState.Rented);
                requestId = context.Requests.First().Id;
            }

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RequestRepository requestRepo = new RequestRepository(context);
                requestRepo.RemoveRequest(requestId);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                Request? request = context.Requests.FirstOrDefault(r => r.Id == requestId);
                Assert.Null(request);
            }
        }
    }
}
