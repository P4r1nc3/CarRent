using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Tests.Repositories
{
    public class RepairRepositoryTests
    {
        private DbContextOptions<DatabaseContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private int AddTestCar(DatabaseContext context, CarState initialState = CarState.InService)
        {
            Car car = new()
            {
                Make = "TestMake",
                Model = "TestModel",
                Year = 2022,
                HorsePower = 200,
                CarState = initialState,
                Repairs = []
            };
            context.Cars.Add(car);
            context.SaveChanges();
            return car.Id;
        }

        private List<RepairItem> GetDummyRepairItems()
        {
            return
            [
                new RepairItem { Description = "Brake replacement", Cost = 150, Repair = null },
                new RepairItem { Description = "Oil change", Cost = 50, Repair = null }
            ];
        }

        [Fact]
        public void MarkCarAsRepaired_Should_AddRepairToDatabase_And_UpdateCarState()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);

            int carId;
            carId = AddTestCar(context, CarState.InService);

            // Act
            RepairRepository repairRepository = new RepairRepository(context);
            List<RepairItem> repairItems = GetDummyRepairItems();
            decimal totalCost = repairItems.Sum(ri => ri.Cost);
            string repairSummary = "Replaced brakes and changed oil";

            repairRepository.MarkCarAsRepaired(carId, repairItems, totalCost, repairSummary);

            // Assert
            Repair? repair = context.Repairs.FirstOrDefault(r => r.CarId == carId);
            Assert.NotNull(repair);
            Assert.Equal(carId, repair.CarId);
            Assert.Equal(totalCost, repair.TotalCost);
            Assert.Equal(repairSummary, repair.RepairSummary);
            Assert.True((DateTime.Now - repair.RepairDate).TotalSeconds < 10);
            Assert.Equal(2, repair.RepairItems.Count);

            Car? car = context.Cars.FirstOrDefault(c => c.Id == carId);
            Assert.NotNull(car);
            Assert.Equal(CarState.Available, car.CarState);
        }

        [Fact]
        public void MarkCarAsRepaired_Should_ThrowArgumentException_WhenCarNotFound()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            using DatabaseContext context = new DatabaseContext(options);
            RepairRepository repairRepository = new RepairRepository(context);
            List<RepairItem> repairItems = GetDummyRepairItems();
            decimal totalCost = repairItems.Sum(ri => ri.Cost);
            string repairSummary = "Test repair";

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => repairRepository.MarkCarAsRepaired(999, repairItems, totalCost, repairSummary));
        }

        [Fact]
        public void GetRepairsByCarId_Should_ReturnRepairsForSpecifiedCar()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int carId;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                carId = AddTestCar(context, CarState.InService);
                RepairRepository repairRepository = new RepairRepository(context);
                List<RepairItem> repairItems = GetDummyRepairItems();
                decimal totalCost = repairItems.Sum(ri => ri.Cost);
                string repairSummary = "Repair for GetRepairsByCarId";

                repairRepository.MarkCarAsRepaired(carId, repairItems, totalCost, repairSummary);
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RepairRepository repairRepository = new RepairRepository(context);
                List<Repair> repairs = repairRepository.GetRepairsByCarId(carId);
                Assert.Single(repairs);
                Assert.Equal("Repair for GetRepairsByCarId", repairs.First().RepairSummary);
            }
        }

        [Fact]
        public void GetAllRepairs_Should_ReturnAllRepairsWithCarIncluded()
        {
            // Arrange
            DbContextOptions<DatabaseContext> options = CreateNewContextOptions();
            int carId1, carId2;
            using (DatabaseContext context = new DatabaseContext(options))
            {
                carId1 = AddTestCar(context, CarState.InService);
                carId2 = AddTestCar(context, CarState.InService);

                RepairRepository repairRepository = new RepairRepository(context);
                List<RepairItem> repairItems = GetDummyRepairItems();
                decimal totalCost = repairItems.Sum(ri => ri.Cost);
                repairRepository.MarkCarAsRepaired(carId1, repairItems, totalCost, "Repair for car 1");
                repairRepository.MarkCarAsRepaired(carId2, repairItems, totalCost, "Repair for car 2");
            }

            // Act & Assert
            using (DatabaseContext context = new DatabaseContext(options))
            {
                RepairRepository repairRepository = new RepairRepository(context);
                List<Repair> repairs = repairRepository.GetAllRepairs();
                Assert.Equal(2, repairs.Count);
                // Verify that each repair includes the associated car.
                Assert.All(repairs, r => Assert.NotNull(r.Car));
            }
        }
    }
}
