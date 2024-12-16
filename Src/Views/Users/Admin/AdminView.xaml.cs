using System.Windows;
using System.Windows.Controls;
using CarRentApp.Contexts;

namespace CarRentApp.Views.Users.Admin
{
    public partial class AdminView : UserControl
    {
        private readonly AuthContext _authContext;

        public event RoutedEventHandler? Logout;

        public AdminView()
        {
            InitializeComponent();
            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            LoadUserInfo();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void LoadUserInfo()
        {
            var currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = currentUser != null
                ? $"Logged in as: {currentUser.Name} {currentUser.Surname}"
                : "No user is currently logged in.";
        }
    }
}
