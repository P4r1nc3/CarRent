using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CarRentApp.Context;
using CarRentApp.Data;
using CarRentApp.Models;
using CarRentApp.Services;

namespace CarRentApp.Views.Login
{
    public partial class LoginView : UserControl
    {
        private readonly UserService _userService;

        public event RoutedEventHandler? SwitchToRegister;
        public event RoutedEventHandler? SwitchToEmployee;

        public LoginView()
        {
            InitializeComponent();
            _userService = new UserService(new AppDbContext());
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();

            // Validate input fields
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Verify user credentials
                var user = _userService.GetUsers()
                    .FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    // Save the current user to the UserContext
                    UserContext.Instance.SetCurrentUser(user);

                    // Switch to the appropriate view based on user role
                    if (user.Role == Role.Customer)
                    {
                        MessageBox.Show("Customer view not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (user.Role == Role.Employee)
                    {
                        SwitchToEmployeeView();
                    }
                    else if (user.Role == Role.Mechanic)
                    {
                        MessageBox.Show("Mechanic view not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (user.Role == Role.Admin)
                    {
                        MessageBox.Show("Admin view not implemented yet.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        public void SwitchToEmployeeView()
        {
            SwitchToEmployee?.Invoke(this, new RoutedEventArgs());
        }
    }
}
