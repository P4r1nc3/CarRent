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

        public void MarkCarAsRepaired(int carId, IEnumerable<RepairItem> repairItems, decimal totalCost, string repairSummary)
        {
            Car? car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                throw new ArgumentException("Car not found", nameof(carId));
            }

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

            car.CarState = CarState.Available;

            _dbContext.SaveChanges();
        }

        public List<Repair> GetRepairsByCarId(int carId)
        {
            return _dbContext.Repairs
                .Where(r => r.CarId == carId)
                .ToList();
        }

        public List<Repair> GetAllRepairs()
        {
            return _dbContext.Repairs.Include(r => r.Car).ToList();
        }
    }
}
