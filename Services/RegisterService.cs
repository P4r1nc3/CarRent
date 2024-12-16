using System;
using CarRentApp.Context;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Services
{
    public class RegisterService
    {
        private readonly UserRepository _userRepository;
        private readonly UserContext _userContext;

        public RegisterService()
        {
            _userRepository = new UserRepository();
            _userContext = UserContext.GetInstance();
        }

        public User RegisterUser(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("All fields are required.");
            }

            // Create and save the new user
            var user = _userRepository.AddUser(firstName, lastName, email, password, Role.Customer);

            // Set the new user in the UserContext
            _userContext.SetCurrentUser(user);

            return user;
        }
    }
}
