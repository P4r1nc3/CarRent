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
}
