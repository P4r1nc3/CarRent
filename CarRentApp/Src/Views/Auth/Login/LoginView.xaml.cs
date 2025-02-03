using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarRentApp.Models;
using CarRentApp.Services;
using CarRentApp.Contexts;


namespace CarRentApp.Views.Auth.Login
{
    public partial class LoginView : UserControl
    {
        public event RoutedEventHandler? SwitchToRegister;
        public event RoutedEventHandler? SwitchToAdmin;
        public event RoutedEventHandler? SwitchToCustomer;
        public event RoutedEventHandler? SwitchToEmployee;
        public event RoutedEventHandler? SwitchToMechanic;

        private readonly LoginService _loginService;

        public LoginView(DatabaseContext dbContext)
        {
            InitializeComponent();
            _loginService = new LoginService(dbContext);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _loginService.LoginUser(
                    EmailTextBox.Text.Trim(), 
                    PasswordBox.Password.Trim());

                if (user != null)
                {
                    // Clear input fields
                    ClearForm();

                    // Navigate based on role
                    switch (user.Role)
                    {
                        case Role.Admin:
                            SwitchToAdmin?.Invoke(this, new RoutedEventArgs());
                            break;
                        case Role.Customer:
                            SwitchToCustomer?.Invoke(this, new RoutedEventArgs());
                            break;
                        case Role.Employee:
                            SwitchToEmployee?.Invoke(this, new RoutedEventArgs());
                            break;
                        case Role.Mechanic:
                            SwitchToMechanic?.Invoke(this, new RoutedEventArgs());
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SwitchToRegisterView(object sender, MouseButtonEventArgs e)
        {
            SwitchToRegister?.Invoke(this, new RoutedEventArgs());
        }

        private void ClearForm()
        {
            EmailTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
        }
    }
}
