using System.Windows;
using CarRentApp.Views.Auth.Register;
using CarRentApp.Views.Auth.Login;
using CarRentApp.Views.Users.Admin;
using CarRentApp.Views.Users.Customer;
using CarRentApp.Views.Users.Employee;
using CarRentApp.Views.Users.Mechanic;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        private readonly RegisterView _registerView;
        private readonly LoginView _loginView;

        private readonly AdminView _adminView;
        private readonly CustomerView _customerView;
        private readonly EmployeeView _employeeView;
        private readonly MechanicView _mechanicView;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize views
            _registerView = new RegisterView();
            _loginView = new LoginView();

            _adminView = new AdminView();
            _customerView = new CustomerView();
            _employeeView = new EmployeeView();
            _mechanicView = new MechanicView();

            // Navigation Events
            _loginView.SwitchToRegister += (_, __) => MainContent.Content = _registerView;
            _registerView.SwitchToLogin += (_, __) => MainContent.Content = _loginView;

            _loginView.SwitchToAdmin += (_, __) => MainContent.Content = _adminView;
            _loginView.SwitchToCustomer += (_, __) => MainContent.Content = _customerView;
            _loginView.SwitchToEmployee += (_, __) => MainContent.Content = _employeeView;
            _loginView.SwitchToMechanic += (_, __) => MainContent.Content = _mechanicView;

            // Logout Events
            _adminView.Logout += (_, __) => MainContent.Content = _loginView;
            _customerView.Logout += (_, __) => MainContent.Content = _loginView;
            _employeeView.Logout += (_, __) => MainContent.Content = _loginView;
            _mechanicView.Logout += (_, __) => MainContent.Content = _loginView;
  

            // Set initial content
            MainContent.Content = _loginView;
        }
    }
}
