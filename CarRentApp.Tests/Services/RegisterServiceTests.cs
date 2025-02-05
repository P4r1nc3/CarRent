using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Services;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Tests.Services
{
    public class RegisterServiceTests
    {
        public RegisterServiceTests()
        {
            AuthContext authContext = AuthContext.GetInstance();
            authContext.SetCurrentUser(null!);
        }

        private DbContextOptions<DatabaseContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void RegisterUser_Should_RegisterUser_When_InputIsValid()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            User result;
            string firstName = "Alice";
            string lastName = "Smith";
            string email = "alice@example.com";
            string password = "securePassword";

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RegisterService registerService = new RegisterService(context);
                result = registerService.RegisterUser(firstName, lastName, email, password);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(firstName, result.Name);
            Assert.Equal(lastName, result.Surname);
            Assert.Equal(email, result.Email);
            Assert.Equal(password, result.Password);
            Assert.Equal(Role.Customer, result.Role);

            AuthContext authContext = AuthContext.GetInstance();
            Assert.Equal(result.Id, authContext.GetCurrentUser()?.Id);
        }

        [Fact]
        public void RegisterUser_Should_ThrowArgumentException_When_FirstNameIsEmpty()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            // Act & Assert
            using DatabaseContext context = new DatabaseContext(options);
            RegisterService registerService = new RegisterService(context);
            Assert.Throws<ArgumentException>(() =>
                registerService.RegisterUser("", "Smith", "alice@example.com", "password"));
        }

        [Fact]
        public void RegisterUser_Should_ThrowArgumentException_When_LastNameIsEmpty()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            // Act & Assert
            using DatabaseContext context = new DatabaseContext(options);
            RegisterService registerService = new RegisterService(context);
            Assert.Throws<ArgumentException>(() =>
                registerService.RegisterUser("Alice", "", "alice@example.com", "password"));
        }

        [Fact]
        public void RegisterUser_Should_ThrowArgumentException_When_EmailIsEmpty()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            // Act & Assert
            using DatabaseContext context = new DatabaseContext(options);
            RegisterService registerService = new RegisterService(context);
            Assert.Throws<ArgumentException>(() =>
                registerService.RegisterUser("Alice", "Smith", "", "password"));
        }

        [Fact]
        public void RegisterUser_Should_ThrowArgumentException_When_PasswordIsEmpty()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            // Act & Assert
            using DatabaseContext context = new DatabaseContext(options);
            RegisterService registerService = new RegisterService(context);
            Assert.Throws<ArgumentException>(() =>
                registerService.RegisterUser("Alice", "Smith", "alice@example.com", ""));
        }
    }
}
