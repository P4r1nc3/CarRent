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
    }
}
