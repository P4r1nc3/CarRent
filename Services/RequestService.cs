using System;
using System.Collections.Generic;
using System.Linq;
using CarRentApp.Data;
using CarRentApp.Models;

namespace CarRentApp.Services
{
    public class RequestService
    {
        private readonly AppDbContext _dbContext;

        public RequestService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateRequest(int carId, int userId, DateTime startDate, DateTime endDate, bool isAccepted)
        {
            var car = _dbContext.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var newRequest = new Request
            {
                CarId = carId,
                UserId = userId,
                Car = car,
                User = user,
                StartDate = startDate,
                EndDate = endDate,
                IsAccepted = isAccepted
            };

            _dbContext.Requests.Add(newRequest);
            _dbContext.SaveChanges();
        }

    }
}
