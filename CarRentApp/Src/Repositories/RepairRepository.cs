using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentApp.Src.Repositories
{
    public class RepairRepository
    {
        private readonly DatabaseContext _dbContext;

        public RepairRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new repair record for the specified car.
        /// </summary>
        /// <param name="carId">The ID of the car being repaired.</param>
        /// <param name="repairItems">A collection of repair items.</param>
        /// <param name="totalCost">The total repair cost.</param>
        /// <param name="repairSummary">A formatted string summarizing the repair details.</param>
        public void MarkCarAsRepaired(int carId, IEnumerable<RepairItem> repairItems, decimal totalCost, string repairSummary)
        {
            // Retrieve the car from the database.
            Car? car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                throw new ArgumentException("Car not found", nameof(carId));
            }

            // Create a new repair record.
            Repair repair = new()
            {
                CarId = carId,
                TotalCost = totalCost,
                RepairSummary = repairSummary,
                RepairDate = DateTime.Now,
                RepairItems = repairItems.ToList(),
                Car = car
            };

            _dbContext.Repairs.Add(repair);

            // Optionally update the car's state.
            car.CarState = CarState.Available;  // Change to the appropriate state as needed.

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Retrieves all repairs for a specific car.
        /// </summary>
        public List<Repair> GetRepairsByCarId(int carId)
        {
            return _dbContext.Repairs
                .Where(r => r.CarId == carId)
                .ToList();
        }

        public List<Repair> GetAllRepairs()
        {
            // Include the Car navigation property so that you can bind Car.Make and Car.Model.
            return _dbContext.Repairs.Include(r => r.Car).ToList();
        }

        // Additional repair-related methods can be added here.
    }
}
