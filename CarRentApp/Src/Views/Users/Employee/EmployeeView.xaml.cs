using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;

namespace CarRentApp.Views.Users.Employee
{
    public partial class EmployeeView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly CarRepository _carRepository;
        private readonly RequestRepository _requestRepository;
        private readonly UserRepository _userRepository;

        public EmployeeView(DatabaseContext dbContext)
        {
            InitializeComponent();

            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            _carRepository = new CarRepository(dbContext);
            _requestRepository = new RequestRepository(dbContext);
            _userRepository = new UserRepository(dbContext);

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
                _carRepository.AddCar(make, model, year, horsePower, carState);

                LoadCarList();
                ClearCarForm();

                MessageBox.Show("Car added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RentRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a request to reject.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var requestId = RequestsDataGrid.SelectedItem?.GetType()?.GetProperty("Id")?.GetValue(RequestsDataGrid.SelectedItem) ?? 0;
            var selectedRequest = _requestRepository.GetRequest(
                (int)requestId
            );

            try
            {
                if (selectedRequest.RequestState == RequestState.Rejected)
                {
                    MessageBox.Show("This request was already rejected", "Already rejected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedRequest.RequestState == RequestState.Rented)
                { return; }

                    // Update request status to "Accepted"
                    _requestRepository.UpdateRequest(
                    selectedRequest.Id,
                    selectedRequest.CarId,
                    selectedRequest.UserId,
                    selectedRequest.StartDate,
                    selectedRequest.EndDate,
                    RequestState.Rented);

                // Update car state to "Rented"
                var car = _carRepository.GetCar(selectedRequest.CarId);
                _carRepository.UpdateCar(
                    car.Id,
                    car.Make,
                    car.Model,
                    car.Year,
                    car.HorsePower,
                    CarState.Rented);

                LoadRequestList();
                LoadCarList();

                MessageBox.Show("Request accepted and car state updated to Rented!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ReserveRequest_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a request to reject.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var requestId = RequestsDataGrid.SelectedItem?.GetType()?.GetProperty("Id")?.GetValue(RequestsDataGrid.SelectedItem) ?? 0;
            var selectedRequest = _requestRepository.GetRequest(
                (int)requestId
            );

            try
            {
                if (selectedRequest.RequestState == RequestState.Rejected)
                {
                    MessageBox.Show("This request was already rejected", "Already rejected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedRequest.RequestState == RequestState.Rented)
                {
                    MessageBox.Show("This request was already rented", "Already rented", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (selectedRequest.RequestState == RequestState.Reserved)
                { return; }

                // Update request status to "Accepted"
                _requestRepository.UpdateRequest(
                    selectedRequest.Id,
                    selectedRequest.CarId,
                    selectedRequest.UserId,
                    selectedRequest.StartDate,
                    selectedRequest.EndDate,
                    RequestState.Reserved);

                // Update car state to "Reserved"
                var car = _carRepository.GetCar(selectedRequest.CarId);
                _carRepository.UpdateCar(
                    car.Id,
                    car.Make,
                    car.Model,
                    car.Year,
                    car.HorsePower,
                    CarState.Reserved);

                LoadRequestList();
                LoadCarList();

                MessageBox.Show("Request accepted and car state updated to Reserved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {

            if (RequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a request to reject.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var requestId = RequestsDataGrid.SelectedItem?.GetType()?.GetProperty("Id")?.GetValue(RequestsDataGrid.SelectedItem) ?? 0;
            var selectedRequest = _requestRepository.GetRequest(
                (int)requestId
            );

            if (selectedRequest.RequestState == RequestState.Rejected)
            { return; }

            try
            {
                if (selectedRequest.RequestState == RequestState.Rented)
                {
                    MessageBox.Show("This request was already rented", "Already rented", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _requestRepository.UpdateRequest(
                    selectedRequest.Id,
                    selectedRequest.CarId,
                    selectedRequest.UserId,
                    selectedRequest.StartDate,
                    selectedRequest.EndDate,
                    RequestState.Rejected);

                // Update car state to "Available"
                var car = _carRepository.GetCar(selectedRequest.CarId);
                _carRepository.UpdateCar(
                    car.Id,
                    car.Make,
                    car.Model,
                    car.Year,
                    car.HorsePower,
                    CarState.Available);
                
                LoadRequestList();
                LoadCarList();
                
                MessageBox.Show("Request rejected successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshRequest_Click(object sender, RoutedEventArgs e)
        {
            LoadCarList();
            LoadRequestList();
        }

        private void SendToService_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a request to reject.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var requestId = RequestsDataGrid.SelectedItem?.GetType()?.GetProperty("Id")?.GetValue(RequestsDataGrid.SelectedItem) ?? 0;
            var selectedRequest = _requestRepository.GetRequest(
                (int)requestId
            );

            try
            {
                _requestRepository.UpdateRequest(
                    selectedRequest.Id,
                    selectedRequest.CarId,
                    selectedRequest.UserId,
                    selectedRequest.StartDate,
                    selectedRequest.EndDate,
                    RequestState.Rejected);

                // Update car state to "Available"
                var car = _carRepository.GetCar(selectedRequest.CarId);
                _carRepository.UpdateCar(
                    car.Id,
                    car.Make,
                    car.Model,
                    car.Year,
                    car.HorsePower,
                    CarState.InService);

                LoadRequestList();
                LoadCarList();

                MessageBox.Show("Request rejected successfully! Car is marked as InService", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void LoadUserInfo()
        {
            var currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = currentUser != null
                ? $"Logged in as: {currentUser.Name} {currentUser.Surname}"
                : "No user is currently logged in.";
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
            RequestsDataGrid.ItemsSource = _requestRepository.GetRequests()
            .Join(
                _userRepository.GetUsers(),
                r => r.UserId,
                u => u.Id,
                (r, u) =>
                new
                {
                    r,
                    u
                }).Join(
                    _carRepository.GetCars(),
                    ur => ur.r.CarId,
                    c => c.Id,
                    (ur, c) =>
                    new
                    {
                        Id = ur.r.Id,
                        CarString = $"{c.Make} {c.Model} {c.Year}",
                        Email = ur.u.Email,
                        StartDate = ur.r.StartDate,
                        EndDate = ur.r.EndDate,
                        Status = ur.r.RequestState,
                    }
                );
        }

        private void ClearCarForm()
        {
            MakeTextBox.Clear();
            ModelTextBox.Clear();
            YearTextBox.Clear();
            HorsePowerTextBox.Clear();
            CarStateComboBox.SelectedIndex = 0;
        }
    }
}
