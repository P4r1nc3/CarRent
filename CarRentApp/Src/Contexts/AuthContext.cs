using CarRentApp.Src.Models;

namespace CarRentApp.Src.Contexts
{
    public class AuthContext
    {
        private static AuthContext? _instance;
        private User? _currentUser;

        public event Action? CurrentUserChanged;

        private AuthContext() { }

        public static AuthContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AuthContext();
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
