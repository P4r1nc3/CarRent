using System.Windows;
using System.Windows.Controls;
using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;

namespace CarRentApp.Views.Users.Customer
{
    public partial class CustomerView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly RequestRepository _requestRepository;
        private readonly CarRepository _carRepository;

        private List<Car> _allCars = [];

        public CustomerView(DatabaseContext dbContext)
        {
            InitializeComponent();

            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;
            _authContext.CurrentUserChanged += LoadRequestListByUserId;

            _requestRepository = new RequestRepository(dbContext);
            _carRepository = new CarRepository(dbContext);

            _allCars = _carRepository.GetCars();

            LoadUserInfo();
            LoadRequestListByUserId();
            LoadCarList();
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

        private void LoadRequestListByUserId()
        {
            var currentUser = _authContext.GetCurrentUser();
            
            RequestsDataGrid.ItemsSource = currentUser != null ? _requestRepository
                .GetRequestsByUserIdDescending(currentUser!.Id)
                .Where(r => r.IsAccepted)
                .Join<Request, Car, int, object>(
                    _allCars,
                    request => request.CarId,
                    car => car.Id,
                    (req, car) => new
                    {
                        CarMake = car.Make,
                        CarModel = car.Model,
                        CarYear = car.Year,
                        StartDate = req.StartDate,
                        EndDate = req.EndDate,
                    }
                )
                .ToList() : [];
        }

        private void LoadCarList()
        {
            CarDataGrid.ItemsSource = _allCars.Where(c => c.CarState == CarState.Available);
        }

        private void RentRequest_Click(object sender, RoutedEventArgs e)
        {

            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;

            if (startDate.Value == null || endDate.Value == null)
            {
                MessageBox.Show("Please select correct rental period.", "No date selected", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            
            var selectedRequestCar = CarDataGrid.SelectedItem as Car;
            var currentUser = _authContext.GetCurrentUser();

            if (selectedRequestCar == null)
            {
                MessageBox.Show("Please select a car to rent.", "No Selection", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (IsOnlyOneCarRentedCheck())
            {
                MessageBox.Show("You can rent only one car at a time.", "Limit exceeded", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                _requestRepository.CreateRequest(selectedRequestCar.Id, currentUser!.Id, startDate.Value, endDate.Value, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsOnlyOneCarRentedCheck()
        {
            var user = _authContext.GetCurrentUser();

            var userRequests = user != null ? _requestRepository.GetRequestsByUserIdDescending(user!.Id) : [];
            var rentedCars = _allCars.Where(car => car.CarState is CarState.Rented or CarState.Reserved);
            
            return userRequests.Exists(r => r.EndDate > DateTime.Now && rentedCars.Contains(r.Car));
        }
    }
}