using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private DbContextOptions<DatabaseContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void AddUser_Should_AddUserToDatabase()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            User addedUser;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                // Act
                addedUser = repository.AddUser("John", "Doe", "john.doe@example.com", "password123", Role.Admin);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                User? user = context.Users.FirstOrDefault(u => u.Id == addedUser.Id);
                Assert.NotNull(user);
                Assert.Equal("John", user.Name);
                Assert.Equal("Doe", user.Surname);
                Assert.Equal("john.doe@example.com", user.Email);
                Assert.Equal("password123", user.Password);
                Assert.Equal(Role.Admin, user.Role);
            }
        }

        [Fact]
        public void AddUser_Should_ThrowException_WhenDuplicateEmail()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);
            UserRepository repository = new UserRepository(context);
            // Act
            repository.AddUser("Jane", "Doe", "jane.doe@example.com", "password", Role.Customer);
            // Assert
            Assert.Throws<InvalidOperationException>(() =>
                repository.AddUser("John", "Smith", "jane.doe@example.com", "password123", Role.Admin));
        }

        [Fact]
        public void GetUser_Should_ReturnUser()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int userId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                User user = repository.AddUser("Alice", "Wonderland", "alice@example.com", "secret", Role.Customer);
                userId = user.Id;
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                User user = repository.GetUser(userId);
                Assert.NotNull(user);
                Assert.Equal("Alice", user.Name);
                Assert.Equal("Wonderland", user.Surname);
                Assert.Equal("alice@example.com", user.Email);
                Assert.Equal("secret", user.Password);
                Assert.Equal(Role.Customer, user.Role);
            }
        }

        [Fact]
        public void GetUser_Should_ThrowException_WhenUserNotFound()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);
            UserRepository repository = new UserRepository(context);
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repository.GetUser(999));
        }

        [Fact]
        public void GetUsers_Should_ReturnAllUsers()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                repository.AddUser("User1", "Last1", "user1@example.com", "pass1", Role.Admin);
                repository.AddUser("User2", "Last2", "user2@example.com", "pass2", Role.Customer);
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                List<User> users = repository.GetUsers();
                Assert.Equal(2, users.Count);
            }
        }

        [Fact]
        public void UpdateUser_Should_UpdateUserProperties()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int userId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                User user = repository.AddUser("Bob", "Marley", "bob@example.com", "oldpass", Role.Customer);
                userId = user.Id;
            }

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                repository.UpdateUser(userId, "Bob", "Marley", "bob.new@example.com", "newpass", Role.Admin);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                User? user = context.Users.FirstOrDefault(u => u.Id == userId);
                Assert.NotNull(user);
                Assert.Equal("Bob", user.Name);
                Assert.Equal("Marley", user.Surname);
                Assert.Equal("bob.new@example.com", user.Email);
                Assert.Equal("newpass", user.Password);
                Assert.Equal(Role.Admin, user.Role);
            }
        }

        [Fact]
        public void UpdateUser_Should_ThrowException_WhenUserNotFound()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);
            UserRepository repository = new UserRepository(context);
            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() =>
                repository.UpdateUser(999, "Test", "User", "test@example.com", "pass", Role.Customer));
        }

        [Fact]
        public void UpdateUser_Should_ThrowException_WhenDuplicateEmail()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int userId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                repository.AddUser("User1", "Last1", "unique@example.com", "pass1", Role.Admin);
                User user2 = repository.AddUser("User2", "Last2", "other@example.com", "pass2", Role.Customer);
                userId = user2.Id;
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                Assert.Throws<InvalidOperationException>(() =>
                    repository.UpdateUser(userId, "User2", "Last2", "unique@example.com", "pass2", Role.Customer));
            }
        }

        [Fact]
        public void RemoveUser_Should_RemoveUserFromDatabase()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int userId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                User user = repository.AddUser("Charlie", "Brown", "charlie@example.com", "password", Role.Customer);
                userId = user.Id;
            }

            // Act
            using (DatabaseContext context = new DatabaseContext(options))
            {
                UserRepository repository = new UserRepository(context);
                repository.RemoveUser(userId);
            }

            // Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                User? user = context.Users.FirstOrDefault(u => u.Id == userId);
                Assert.Null(user);
            }
        }
    }
}
