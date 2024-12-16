using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarRentApp.Models;
using CarRentApp.Services;

namespace CarRentApp.Views.Login
{
    public partial class LoginView : UserControl
    {
        private readonly LoginService _loginService;

        public event RoutedEventHandler? SwitchToRegister;
        public event RoutedEventHandler? SwitchToEmployee;

        public LoginView()
        {
            InitializeComponent();
            _loginService = new LoginService();
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
                        case Role.Customer:
                            MessageBox.Show("Customer view not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case Role.Employee:
                            SwitchToEmployee?.Invoke(this, new RoutedEventArgs());
                            break;
                        case Role.Mechanic:
                            MessageBox.Show("Mechanic view not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case Role.Admin:
                            MessageBox.Show("Admin view not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
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
