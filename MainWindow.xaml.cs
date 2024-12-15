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
            registerView.SwitchToLogin += (_, __) => MainContent.Content = loginView;
            registerView.SwitchToEmployee += (_, __) => MainContent.Content = employeeView;
            employeeView.Logout += (_, __) => MainContent.Content = loginView;

            // Set initial view
            MainContent.Content = loginView;
        }
    }
}
