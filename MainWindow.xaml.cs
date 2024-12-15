using System.Windows;
using CarRentApp.Views;

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

            // Set event handlers
            _loginView.SwitchToRegister += SwitchToRegisterView;
            _registerView.SwitchToLogin += SwitchToLoginView;

            // Set initial view
            MainContent.Content = _loginView;
        }

        private void SwitchToRegisterView(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _registerView;
        }

        private void SwitchToLoginView(object sender, RoutedEventArgs e)
        {
            MainContent.Content = _loginView;
        }
    }
}
