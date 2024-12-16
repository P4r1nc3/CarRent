using System.Windows;
using System.Windows.Controls;
using CarRentApp.Context;
using CarRentApp.Models;
using CarRentApp.Repositories;
using System.Linq;

namespace CarRentApp.Views.Employee
{
    public partial class EmployeeView : UserControl
    {
        private readonly UserContext _userContext;
        private readonly CarRepository _carRepository;
        private readonly RequestRepository _requestRepository;

        public event RoutedEventHandler? Logout;

        public EmployeeView()
        {
            InitializeComponent();
            _userContext = UserContext.GetInstance();
            _userContext.CurrentUserChanged += LoadUserInfo;
            _carRepository = new CarRepository();
            _requestRepository = new RequestRepository();

            LoadUserInfo();
            LoadCarList();
            LoadCarStates();
            LoadRequestList();
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
                _carRepository.AddCar(make, model, year, horsePower, carState);

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

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = RequestsDataGrid.SelectedItem as Request;

            if (selectedRequest == null)
            {
                MessageBox.Show("Please select a request to accept.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _requestRepository.UpdateRequest(selectedRequest.Id, selectedRequest.CarId, selectedRequest.UserId,
                                              selectedRequest.StartDate, selectedRequest.EndDate, true);
                MessageBox.Show("Request accepted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadRequestList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error accepting request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = RequestsDataGrid.SelectedItem as Request;

            if (selectedRequest == null)
            {
                MessageBox.Show("Please select a request to reject.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _requestRepository.UpdateRequest(selectedRequest.Id, selectedRequest.CarId, selectedRequest.UserId,
                                              selectedRequest.StartDate, selectedRequest.EndDate, false);
                MessageBox.Show("Request rejected successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadRequestList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error rejecting request: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _userContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void LoadUserInfo()
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
            CarDataGrid.ItemsSource = _carRepository.GetCars();
        }

        private void LoadCarStates()
        {
            CarStateComboBox.ItemsSource = System.Enum.GetValues(typeof(CarState)).Cast<CarState>();
            CarStateComboBox.SelectedIndex = 0;
        }

        private void LoadRequestList()
        {
            RequestsDataGrid.ItemsSource = _requestRepository.GetRequests();
        }
    }
}
