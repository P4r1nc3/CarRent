using System;
using System.Collections.Generic;
using System.Linq;
using CarRentApp.Contexts;
using CarRentApp.Models;

namespace CarRentApp.Repositories
{
    public class RequestRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserRepository _userRepository;
        private readonly CarRepository _carRepository;

        public RequestRepository()
        {
            _dbContext = new DatabaseContext();
            _userRepository = new UserRepository();
            _carRepository = new CarRepository();
        }

        public void CreateRequest(int carId, int userId, DateTime startDate, DateTime endDate, bool isAccepted)
        {
            var car = _carRepository.GetCar(carId);
            var user = _userRepository.GetUser(userId);

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

        public void RemoveRequest(int requestId)
        {
            var request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                _dbContext.Requests.Remove(request);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");
            }
        }
    }
}
