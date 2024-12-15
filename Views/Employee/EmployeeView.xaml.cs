using System.Windows;
using System.Windows.Controls;
using CarRentApp.Context;
using CarRentApp.Models;
using CarRentApp.Services;
using System.Linq;

namespace CarRentApp.Views.Employee
{
    public partial class EmployeeView : UserControl
    {
        private readonly UserContext _userContext;
        private readonly CarService _carService;

        public event RoutedEventHandler? Logout;

        public EmployeeView()
        {
            InitializeComponent();
            _userContext = UserContext.GetInstance();
            _userContext.CurrentUserChanged += DisplayCurrentUserInfo;
            _carService = new CarService();

            DisplayCurrentUserInfo();
            LoadCarList();
            LoadCarStates();
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve data from input fields
            var make = MakeTextBox.Text.Trim();
            var model = ModelTextBox.Text.Trim();
            var yearText = YearTextBox.Text.Trim();
            var horsePowerText = HorsePowerTextBox.Text.Trim();
            var carState = (CarState)CarStateComboBox.SelectedItem;

            // Basic validation
            if (string.IsNullOrEmpty(make) || string.IsNullOrEmpty(model) ||
                string.IsNullOrEmpty(yearText) || string.IsNullOrEmpty(horsePowerText))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(yearText, out var year) || !int.TryParse(horsePowerText, out var horsePower))
            {
                MessageBox.Show("Year and Horse Power must be valid numbers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Add car to the database
                _carService.AddCar(make, model, year, horsePower, carState);

                // Refresh the car list
                LoadCarList();

                // Clear input fields
                MakeTextBox.Clear();
                ModelTextBox.Clear();
                YearTextBox.Clear();
                HorsePowerTextBox.Clear();
                CarStateComboBox.SelectedIndex = 0;

                MessageBox.Show("Car added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _userContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void DisplayCurrentUserInfo()
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser != null)
            {
                UserInfoTextBlock.Text = $"Logged in as: {currentUser.Name} {currentUser.Surname}";
            }
            else
            {
                UserInfoTextBlock.Text = "No user is currently logged in.";
            }
        }

        private void LoadCarList()
        {
            CarDataGrid.ItemsSource = _carService.GetCars();
        }

        private void LoadCarStates()
        {
            CarStateComboBox.ItemsSource = System.Enum.GetValues(typeof(CarState)).Cast<CarState>();
            CarStateComboBox.SelectedIndex = 0;
        }
    }
}
