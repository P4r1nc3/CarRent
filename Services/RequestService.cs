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

        public Request GetRequest(int requestId)
        {
            var request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request == null)
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");
            }
            return request;
        }

        public List<Request> GetRequests()
        {
            return _dbContext.Requests.ToList();
        }

        public void UpdateRequest(int requestId, int carId, int userId, DateTime startDate, DateTime endDate, bool isAccepted)
        {
            var request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.CarId = carId;
                request.UserId = userId;
                request.StartDate = startDate;
                request.EndDate = endDate;
                request.IsAccepted = isAccepted;

                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");
            }
        }

    }
}
