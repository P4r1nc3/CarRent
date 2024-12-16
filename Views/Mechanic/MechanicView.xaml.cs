using System.Windows;
using System.Windows.Controls;
using CarRentApp.Contexts;

namespace CarRentApp.Views.Mechanic
{
    public partial class MechanicView : UserControl
    {
        private readonly AuthContext _authContext;

        public event RoutedEventHandler? Logout;

        public MechanicView()
        {
            InitializeComponent();
            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            LoadUserInfo();
        }

        // TODO move this somewhere to not duplicate the login in all user views
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void LoadUserInfo()
        {
            var currentUser = _authContext.GetCurrentUser();

            if (currentUser != null)
            {
                UserInfoTextBlock.Text = $"Logged in as: {currentUser.Name} {currentUser.Surname}";
            }
            else
            {
                UserInfoTextBlock.Text = "No user is currently logged in.";
            }
        }
    }
}
