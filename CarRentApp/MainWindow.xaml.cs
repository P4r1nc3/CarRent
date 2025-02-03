using System.Windows;
using Microsoft.EntityFrameworkCore;
using CarRentApp.Views.Auth.Register;
using CarRentApp.Views.Auth.Login;
using CarRentApp.Views.Users.Admin;
using CarRentApp.Views.Users.Customer;
using CarRentApp.Views.Users.Employee;
using CarRentApp.Views.Users.Mechanic;
using CarRentApp.Contexts;

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

            // Initialize DatabaseContext
            var options = new DbContextOptionsBuilder<DatabaseContext>().Options;
            var dbContext = new DatabaseContext(options);

            // Initialize views
            _registerView = new RegisterView(dbContext);
            _loginView = new LoginView(dbContext);

            _adminView = new AdminView(dbContext);
            _customerView = new CustomerView(dbContext);
            _employeeView = new EmployeeView(dbContext);
            _mechanicView = new MechanicView(dbContext);

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
