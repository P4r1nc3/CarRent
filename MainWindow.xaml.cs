using System.Windows;
using CarRentApp.Views.Customer;
using CarRentApp.Views.Employee;
using CarRentApp.Views.Login;
using CarRentApp.Views.Register;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        private readonly LoginView _loginView;
        private readonly RegisterView _registerView;
        private readonly EmployeeView _employeeView;
        private readonly CustomerView _customerView;

        public MainWindow()
        {
            InitializeComponent();

            _loginView = new LoginView();
            _registerView = new RegisterView();
            _employeeView = new EmployeeView();
            _customerView = new CustomerView();

            _loginView.SwitchToRegister += (_, __) => MainContent.Content = _registerView;
            _registerView.SwitchToLogin += (_, __) => MainContent.Content = _loginView;
            _loginView.SwitchToEmployee += (_, __) => MainContent.Content = _employeeView;
            _loginView.SwitchToCustomer += (_, __) => MainContent.Content = _customerView;

            _employeeView.Logout += (_, __) => MainContent.Content = _loginView;
            _customerView.Logout += (_, __) => MainContent.Content = _loginView;

            MainContent.Content = _loginView;
        }
    }
}
