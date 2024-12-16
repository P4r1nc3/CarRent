using System.Linq;
using CarRentApp.Contexts;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Services
{
    public class LoginService
    {
        private readonly UserRepository _userRepository;
        private readonly AuthContext _authContext;

        public LoginService()
        {
            _userRepository = new UserRepository();
            _authContext = AuthContext.GetInstance();
        }

        public User? LoginUser(string email, string password)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email and password cannot be empty.");

            // Check user credentials
            var user = _userRepository.GetUsers()
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Set the current user in the context
                _authContext.SetCurrentUser(user);
            }

            return user;
        }
    }
}
