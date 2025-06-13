using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;

namespace CarRentApp.Src.Services
{
    public class LoginService
    {
        private readonly AuthContext _authContext;
        private readonly UserRepository _userRepository;

        public LoginService(DatabaseContext dbContext)
        {
            _authContext = AuthContext.GetInstance();
            _userRepository = new UserRepository(dbContext);
        }

        public User? LoginUser(string email, string password)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email and password cannot be empty.");
            }

            // Check user credentials
            User? user = _userRepository.GetUsers()
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
