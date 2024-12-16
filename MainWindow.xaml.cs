using System.Windows;
using CarRentApp.Views.Auth.Register;
using CarRentApp.Views.Auth.Login;
using CarRentApp.Views.Users.Admin;
using CarRentApp.Views.Users.Mechanic;
using CarRentApp.Views.Users.Customer;
using CarRentApp.Views.Users.Employee;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        private readonly RegisterView _registerView;
        private readonly LoginView _loginView;

        private readonly AdminView _adminView;
        private readonly MechanicView _mechanicView;
        private readonly EmployeeView _employeeView;
        private readonly CustomerView _customerView;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize views
            _registerView = new RegisterView();
            _loginView = new LoginView();

            _adminView = new AdminView();
            _mechanicView = new MechanicView();
            _employeeView = new EmployeeView();
            _customerView = new CustomerView();


            // Navigation Events
            _loginView.SwitchToRegister += (_, __) => MainContent.Content = _registerView;
            _registerView.SwitchToLogin += (_, __) => MainContent.Content = _loginView;

            _loginView.SwitchToAdmin += (_, __) => MainContent.Content = _adminView;
            _loginView.SwitchToMechanic += (_, __) => MainContent.Content = _mechanicView;
            _loginView.SwitchToCustomer += (_, __) => MainContent.Content = _customerView;
            _loginView.SwitchToEmployee += (_, __) => MainContent.Content = _employeeView;

            // Logout Events
            _adminView.Logout += (_, __) => MainContent.Content = _loginView;
            _mechanicView.Logout += (_, __) => MainContent.Content = _loginView;
            _customerView.Logout += (_, __) => MainContent.Content = _loginView;
            _employeeView.Logout += (_, __) => MainContent.Content = _loginView;

            // Set initial content
            MainContent.Content = _loginView;
        }
    }
}
