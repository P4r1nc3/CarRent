using System.Windows;
using CarRentApp.Context;
using CarRentApp.Views.Login;
using CarRentApp.Views.Register;
using CarRentApp.Views.Employee;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        private readonly LoginView _loginView;
        private readonly RegisterView _registerView;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize views
            _loginView = new LoginView();
            _registerView = new RegisterView();

            // Event handlers
            _loginView.SwitchToRegister += (_, __) => MainContent.Content = _registerView;
            _registerView.SwitchToLogin += (_, __) => MainContent.Content = _loginView;

            _loginView.SwitchToEmployee += (_, __) => MainContent.Content = new EmployeeView();
            // employeeView.Logout += (_, __) => MainContent.Content = loginView;

            // Set initial view
            MainContent.Content = _loginView;
        }
    }
}
