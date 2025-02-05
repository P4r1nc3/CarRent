using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CarRentApp.Repositories;
using CarRentApp.Services;
using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;

namespace CarRentApp.Tests
{
    public class LoginServiceTests
    {
        public LoginServiceTests()
        {
            var authContext = AuthContext.GetInstance();
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
            var options = CreateNewContextOptions();

            using (var context = new DatabaseContext(options))
            {
                var userRepository = new UserRepository(context);
                userRepository.AddUser("Test", "User", "test.user@example.com", "secret", Role.Customer);
            }

            User? result;
            // Act
            using (var context = new DatabaseContext(options))
            {
                var loginService = new LoginService(context);
                result = loginService.LoginUser("test.user@example.com", "secret");
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test.user@example.com", result!.Email);

            var authContext = AuthContext.GetInstance();
            Assert.Equal(result.Id, authContext.GetCurrentUser()?.Id);
        }

        [Fact]
        public void LoginUser_Should_ReturnNull_When_CredentialsAreInvalid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using (var context = new DatabaseContext(options))
            {
                var userRepository = new UserRepository(context);
                userRepository.AddUser("Test", "User", "test.user@example.com", "secret", Role.Customer);
            }

            User? result;
            // Act
            using (var context = new DatabaseContext(options))
            {
                var loginService = new LoginService(context);
                result = loginService.LoginUser("test.user@example.com", "wrongpassword");
            }

            // Assert
            Assert.Null(result);

            var authContext = AuthContext.GetInstance();
            Assert.Null(authContext.GetCurrentUser());
        }

        [Fact]
        public void LoginUser_Should_ThrowArgumentException_When_EmailIsEmpty()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Act & Assert
            using (var context = new DatabaseContext(options))
            {
                var loginService = new LoginService(context);
                Assert.Throws<ArgumentException>(() => loginService.LoginUser("", "secret"));
            }
        }

        [Fact]
        public void LoginUser_Should_ThrowArgumentException_When_PasswordIsEmpty()
        {
            // Arrange
            var options = CreateNewContextOptions();

            // Act & Assert
            using (var context = new DatabaseContext(options))
            {
                var loginService = new LoginService(context);
                Assert.Throws<ArgumentException>(() => loginService.LoginUser("test.user@example.com", ""));
            }
        }
    }
}
