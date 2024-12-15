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
            _userContext = UserContext.GetInstance();
            InitializeComponent();
            DisplayCurrentUserInfo();
        }

        private void DisplayCurrentUserInfo()
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser != null)
            {
                // Set the TextBlock to display user info
                UserInfoTextBlock.Text = $"Logged in as: {currentUser.Name} {currentUser.Surname}";
            }
            else
            {
                // Fallback text if no user is logged in
                UserInfoTextBlock.Text = "No user is currently logged in.";
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Log out the current user
            _userContext.Logout();

            // Trigger logout event to navigate back to login view
            Logout?.Invoke(this, new RoutedEventArgs());
        }
    }
}
