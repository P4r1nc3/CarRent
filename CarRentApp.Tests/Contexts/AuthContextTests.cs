using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;

namespace CarRentApp.Tests.Contexts
{
    public class AuthContextTests
    {
        public AuthContextTests()
        {
            AuthContext.GetInstance().Logout();
        }

        [Fact]
        public void SetCurrentUser_Should_UpdateCurrentUser_And_FireEvent()
        {
            // Arrange
            AuthContext authContext = AuthContext.GetInstance();
            User testUser = new User
            {
                Id = 1,
                Name = "Test",
                Surname = "User",
                Email = "test@example.com",
                Password = "password",
                Role = Role.Customer
            };

            bool eventFired = false;
            authContext.CurrentUserChanged += () => eventFired = true;

            // Act
            authContext.SetCurrentUser(testUser);

            // Assert
            Assert.Equal(testUser, authContext.GetCurrentUser());
            Assert.True(eventFired);
        }

        [Fact]
        public void Logout_Should_ClearCurrentUser_And_FireEvent()
        {
            // Arrange
            AuthContext authContext = AuthContext.GetInstance();
            User testUser = new User
            {
                Id = 1,
                Name = "Test",
                Surname = "User",
                Email = "test@example.com",
                Password = "password",
                Role = Role.Customer
            };

            authContext.SetCurrentUser(testUser);

            bool eventFired = false;
            authContext.CurrentUserChanged += () => eventFired = true;

            // Act
            authContext.Logout();

            // Assert
            Assert.Null(authContext.GetCurrentUser());
            Assert.True(eventFired);
        }
    }
}
