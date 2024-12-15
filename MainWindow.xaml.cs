using System.Windows;
using CarRentApp.Views.Login;
using CarRentApp.Views.Register;

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

            // Event handlers as lambdas
            loginView.SwitchToRegister += (_, __) => MainContent.Content = registerView;
            registerView.SwitchToLogin += (_, __) => MainContent.Content = loginView;

            // Set initial view
            MainContent.Content = loginView;
        }
    }
}
