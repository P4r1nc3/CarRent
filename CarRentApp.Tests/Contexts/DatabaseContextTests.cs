using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CarRentApp.Contexts;

namespace CarRentApp.Tests
{
    public class DatabaseContextTests
    {
        [Fact]
        public void ConstructorWithoutOptions_ShouldThrowInvalidOperationException_WhenEnvVarNotSet()
        {
            // Arrange
            string? previousValue = Environment.GetEnvironmentVariable("CAR_RENT_DB_CONNECTION");
            Environment.SetEnvironmentVariable("CAR_RENT_DB_CONNECTION", "");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                using (var context = new DatabaseContext())
                {
                    context.Database.EnsureCreated();
                }
            });

            // Cleanup
            Environment.SetEnvironmentVariable("CAR_RENT_DB_CONNECTION", previousValue);
        }

        [Fact]
        public void ConstructorWithOptions_ShouldNotThrowException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act & Assert
            var exception = Record.Exception(() =>
            {
                using (var context = new DatabaseContext(options))
                {
                    context.Database.EnsureCreated();
                }
            });

            Assert.Null(exception);
        }
    }
}
