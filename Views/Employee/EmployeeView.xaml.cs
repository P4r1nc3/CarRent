using System.Windows;
using System.Windows.Controls;
using CarRentApp.Context;

namespace CarRentApp.Views.Employee
{
    public partial class EmployeeView : UserControl
    {
        public event RoutedEventHandler? Logout;

        public EmployeeView()
        {
            InitializeComponent();
            DisplayCurrentUserInfo();
        }

        private void DisplayCurrentUserInfo()
        {
            var currentUser = UserContext.Instance.CurrentUser;

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
            UserContext.Instance.Logout();

            // Trigger logout event to navigate back to login view
            Logout?.Invoke(this, new RoutedEventArgs());
        }
    }
}
