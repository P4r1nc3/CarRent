using System.Windows;
using System.Windows.Controls;
using CarRentApp.Context;

namespace CarRentApp.Views.Employee
{
    public partial class EmployeeView : UserControl
    {
        private readonly UserContext _userContext;

        public event RoutedEventHandler? Logout;

        public EmployeeView()
        {
            InitializeComponent();
            _userContext = UserContext.GetInstance();
            _userContext.CurrentUserChanged += DisplayCurrentUserInfo;
            DisplayCurrentUserInfo();
        }

        private void DisplayCurrentUserInfo()
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser != null)
            {
                UserInfoTextBlock.Text = $"Logged in as: {currentUser.Name} {currentUser.Surname}";
            }
            else
            {
                UserInfoTextBlock.Text = "No user is currently logged in.";
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _userContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }
    }
}
