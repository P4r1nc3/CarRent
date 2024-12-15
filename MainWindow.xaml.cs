using System.Windows;
using CarRentApp.Views.Login;
using CarRentApp.Views.Register;
using CarRentApp.Views.Employee;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Initialize views
            var loginView = new LoginView();
            var registerView = new RegisterView();
            var employeeView = new EmployeeView();

            // Event handlers
            loginView.SwitchToRegister += (_, __) => MainContent.Content = registerView;
            loginView.SwitchToEmployee += (_, __) => MainContent.Content = employeeView;
            registerView.SwitchToLogin += (_, __) => MainContent.Content = loginView;
            employeeView.Logout += (_, __) => MainContent.Content = loginView;

            // Set initial view
            MainContent.Content = loginView;
        }
    }
}
