using CarRentApp.Models;

namespace CarRentApp.Context
{
    public class UserContext
    {
        private static UserContext? _instance;
        private User? _currentUser;

        private UserContext() {}

        public static UserContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserContext();
            }
            return _instance;
        }

        public void SetCurrentUser(User user)
        {
            _currentUser = user;
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public void Logout()
        {
            _currentUser = null;
        }
    }
}
