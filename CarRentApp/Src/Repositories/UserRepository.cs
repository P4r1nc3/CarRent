using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;

namespace CarRentApp.Src.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseContext _dbContext;

        public UserRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddUser(string name, string surname, string email, string password, Role role)
        {
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                throw new InvalidOperationException("User with the given email already exists.");
            }

            User newUser = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                Role = role
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return newUser;
        }

        public User GetUser(int userId)
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            return user == null ? throw new KeyNotFoundException($"User with ID {userId} not found.") : user;
        }

        public List<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public void UpdateUser(int userId, string name, string surname, string email, string password, Role role)
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                if (_dbContext.Users.Any(u => u.Email == email && u.Id != userId))
                {
                    throw new InvalidOperationException("Another user with the given email already exists.");
                }

                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.Password = password;
                user.Role = role;

                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
        }

        public void RemoveUser(int userId)
        {
            User? user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
        }
    }
}
