using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;

namespace CarRentApp.Src.Services
{
    public class RegisterService
    {
        private readonly AuthContext _authContext;
        private readonly UserRepository _userRepository;

        public RegisterService(DatabaseContext dbContext)
        {
            _authContext = AuthContext.GetInstance();
            _userRepository = new UserRepository(dbContext);
        }

        public User RegisterUser(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("All fields are required.");
            }

            // Create and save the new user
            User user = _userRepository.AddUser(firstName, lastName, email, password, Role.Customer);

            // Set the new user in the UserContext
            _authContext.SetCurrentUser(user);

            return user;
        }
    }
}
