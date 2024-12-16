using System;
using System.Windows;
using System.Windows.Controls;
using CarRentApp.Models;
using CarRentApp.Services;

namespace CarRentApp.Views.Auth.Register
{
    public partial class RegisterView : UserControl
    {
        public event RoutedEventHandler? SwitchToLogin;

        private readonly RegisterService _registerService;

        public RegisterView()
        {
            InitializeComponent();
            _registerService = new RegisterService();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // User registration
                _registerService.RegisterUser(
                    FirstNameTextBox.Text.Trim(), 
                    LastNameTextBox.Text.Trim(), 
                    EmailTextBox.Text.Trim(), 
                    PasswordBox.Password.Trim()
                );

                // Clear input fields
                ClearForm();

                // Success message
                MessageBox.Show("Account created successfully!\nNow you can log in!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Navigate to Login View
                SwitchToLogin?.Invoke(this, new RoutedEventArgs());
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            PasswordBox.Clear();
        }

        private void SwitchToLoginView(object sender, RoutedEventArgs e)
        {
            SwitchToLogin?.Invoke(this, new RoutedEventArgs());
        }
    }
}
