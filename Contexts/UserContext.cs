using CarRentApp.Models;
using System;

namespace CarRentApp.Contexts
{
    public class UserContext
    {
        private static UserContext? _instance;
        private User? _currentUser;

        public event Action? CurrentUserChanged;

        private UserContext() { }

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
            CurrentUserChanged?.Invoke();
        }

        public User? GetCurrentUser()
        {
            return _currentUser;
        }

        public void Logout()
        {
            _currentUser = null;
            CurrentUserChanged?.Invoke();
        }
    }
}
