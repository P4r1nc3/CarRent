using System;
using System.Windows;
using System.Windows.Controls;
using CarRentApp.Context;
using CarRentApp.Data;
using CarRentApp.Models;
using CarRentApp.Services;

namespace CarRentApp.Views.Register
{
    public partial class RegisterView : UserControl
    {
        private readonly UserService _userService;
        private readonly UserContext _userContext;

        public event RoutedEventHandler? SwitchToLogin;
        public event RoutedEventHandler? SwitchToEmployee;

        public RegisterView()
        {
            InitializeComponent();
            _userService = new UserService(new AppDbContext());
            _userContext = UserContext.GetInstance();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input
            var firstName = FirstNameTextBox.Text.Trim();
            var lastName = LastNameTextBox.Text.Trim();
            var email = EmailTextBox.Text.Trim();
            var password = PasswordBox.Password.Trim();

            // Basic validation
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Add user to the database
                User user = _userService.AddUser(firstName, lastName, email, password, Role.Customer);

                // Set the current user in the UserContext
                _userContext.SetCurrentUser(user);

                // Success message
                MessageBox.Show("Account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Call the method to switch to Employee view
                SwitchToEmployeeView();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SwitchToLoginView(object sender, RoutedEventArgs e)
        {
            SwitchToLogin?.Invoke(this, new RoutedEventArgs());
        }

        public void SwitchToEmployeeView()
        {
            SwitchToEmployee?.Invoke(this, new RoutedEventArgs());
        }
    }
}
