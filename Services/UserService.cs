using System;
using System.Collections.Generic;
using System.Linq;
using CarRentApp.Data;
using CarRentApp.Models;

namespace CarRentApp.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(string name, string surname, string email, string password, Role role)
        {
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new InvalidOperationException("User with the given email already exists.");
            }

            var newUser = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                Role = role
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public User GetUser(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            return user;
        }
    }
}
