using System;
using System.Collections.Generic;
using CarRentApp.Contexts;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Services
{
    public class EmployeeService
    {
        private readonly CarRepository _carRepository;
        private readonly RequestRepository _requestRepository;
        private readonly UserContext _userContext;

        public event Action? CurrentUserChanged;

        public EmployeeService()
        {
            _carRepository = new CarRepository();
            _requestRepository = new RequestRepository();
            _userContext = UserContext.GetInstance();

            _userContext.CurrentUserChanged += OnCurrentUserChanged;
        }

        private void OnCurrentUserChanged()
        {
            CurrentUserChanged?.Invoke();
        }

        public User? GetCurrentUser()
        {
            return _userContext.GetCurrentUser();
        }

        public void LogoutUser()
        {
            _userContext.Logout();
        }

        public List<Car> GetAllCars()
        {
            return _carRepository.GetCars();
        }

        public void AddNewCar(string make, string model, int year, int horsePower, CarState carState)
        {
            if (string.IsNullOrEmpty(make) || string.IsNullOrEmpty(model))
            {
                throw new ArgumentException("Make and model are required.");
            }

            if (year <= 0 || horsePower <= 0)
            {
                throw new ArgumentException("Year and Horse Power must be positive values.");
            }

            _carRepository.AddCar(make, model, year, horsePower, carState);
        }

        public List<Request> GetAllRequests()
        {
            return _requestRepository.GetRequests();
        }

        public void UpdateRequest(int requestId, int carId, int userId, DateTime startDate, DateTime endDate, bool isAccepted)
        {
            _requestRepository.UpdateRequest(requestId, carId, userId, startDate, endDate, isAccepted);
        }
    }
}
