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
            UserContext.GetInstance().CurrentUserChanged += DisplayCurrentUserInfo;
            DisplayCurrentUserInfo();
        }

        private void DisplayCurrentUserInfo()
        {
            var currentUser = UserContext.GetInstance().GetCurrentUser();

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
            UserContext.GetInstance().Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }
    }
}
