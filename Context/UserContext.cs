using CarRentApp.Models;

namespace CarRentApp.Context
{
    public class UserContext
    {
        private static UserContext? _instance;

        public User? CurrentUser { get; private set; }

        private UserContext() { }

        public static UserContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserContext();
                }
                return _instance;
            }
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
