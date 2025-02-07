using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;
using CarRentApp.Src.Services;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Tests.Services
{
    public class LoginServiceTests
    {
        public LoginServiceTests()
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
        public void LoginUser_Should_ReturnUser_When_CredentialsAreValid()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository userRepository = new UserRepository(context);
                userRepository.AddUser("Test", "User", "test.user@example.com", "secret", Role.Customer);
            }

            User? result;
            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                LoginService loginService = new LoginService(context);
                result = loginService.LoginUser("test.user@example.com", "secret");
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test.user@example.com", result!.Email);

            AuthContext authContext = AuthContext.GetInstance();
            Assert.Equal(result.Id, authContext.GetCurrentUser()?.Id);
        }

        [Fact]
        public void LoginUser_Should_ReturnNull_When_CredentialsAreInvalid()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository userRepository = new UserRepository(context);
                userRepository.AddUser("Test", "User", "test.user@example.com", "secret", Role.Customer);
            }

            User? result;
            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                LoginService loginService = new LoginService(context);
                result = loginService.LoginUser("test.user@example.com", "wrongpassword");
            }

            // Assert
            Assert.Null(result);

            AuthContext authContext = AuthContext.GetInstance();
            Assert.Null(authContext.GetCurrentUser());
        }

        [Fact]
        public void LoginUser_Should_ThrowArgumentException_When_EmailIsEmpty()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            // Act & Assert
            using DatabaseContext context = new DatabaseContext(options);
            LoginService loginService = new LoginService(context);
            Assert.Throws<ArgumentException>(() => loginService.LoginUser("", "secret"));
        }

        [Fact]
        public void LoginUser_Should_ThrowArgumentException_When_PasswordIsEmpty()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();

            // Act & Assert
            using DatabaseContext context = new DatabaseContext(options);
            LoginService loginService = new LoginService(context);
            Assert.Throws<ArgumentException>(() => loginService.LoginUser("test.user@example.com", ""));
        }
    }
}
