using System;
using CarRentApp.Contexts;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Services
{
    public class RegisterService
    {
        private readonly UserRepository _userRepository;
        private readonly AuthContext _authContext;

        public RegisterService()
        {
            _userRepository = new UserRepository();
            _authContext = AuthContext.GetInstance();
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
            _authContext.SetCurrentUser(user);

            return user;
        }
    }
}
