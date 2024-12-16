using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRentApp.Contexts;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Views.Users.Customer
{
    public partial class CustomerView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly CarRepository _carRepository;
        private readonly RequestRepository _requestRepository;

        private List<Car> _cars = new();
        private List<Request> _existingRequests = new();
        private Car? _selectedCar;

        public CustomerView()
        {
            InitializeComponent();
            _authContext = AuthContext.GetInstance();
            _carRepository = new CarRepository();
            _requestRepository = new RequestRepository();

            LoadCars();
        }

        private void LoadCars()
        {
            _cars = _carRepository.GetCars();

            foreach (var car in _cars)
            {
                var carCard = new Button
                {
                    Content = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock
                            {
                                Text = $"{car.Make} {car.Model}",
                                FontSize = 16,
                                FontWeight = FontWeights.Bold,
                                Margin = new Thickness(5)
                            },
                            new TextBlock
                            {
                                Text = $"Year: {car.Year}, HP: {car.HorsePower}",
                                FontSize = 14,
                                Margin = new Thickness(5)
                            }
                        }
                    },
                    Margin = new Thickness(10),
                    Tag = car
                };

                carCard.Click += CarCard_Click;
                CarsGrid.Children.Add(carCard);
            }
        }

        private void CarCard_Click(object sender, RoutedEventArgs e)
        {
            _selectedCar = (sender as Button)?.Tag as Car;

            if (_selectedCar != null)
            {
                MainView.Visibility = Visibility.Collapsed;
                CarDetailView.Visibility = Visibility.Visible;

                CarDetailTitle.Text = $"{_selectedCar.Make} {_selectedCar.Model} ({_selectedCar.Year})";
                CarDetailDescription.Text = $"HorsePower: {_selectedCar.HorsePower}\nState: {_selectedCar.CarState}";

                LoadExistingReservations();
            }
        }

        private void LoadExistingReservations()
        {
            _existingRequests = _requestRepository.GetRequests()
                .Where(r => r.CarId == _selectedCar?.Id).ToList();

            StartDatePicker.BlackoutDates.Clear();
            EndDatePicker.BlackoutDates.Clear();

            foreach (var request in _existingRequests)
            {
                var blackoutRange = new CalendarDateRange(request.StartDate, request.EndDate);
                StartDatePicker.BlackoutDates.Add(blackoutRange);
                EndDatePicker.BlackoutDates.Add(blackoutRange);
            }
        }


        private void DatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                var startDate = StartDatePicker.SelectedDate.Value;
                var endDate = EndDatePicker.SelectedDate.Value;

                if (startDate > endDate)
                {
                    MessageBox.Show("End date must be after start date.", "Invalid Dates", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (IsDateRangeUnavailable(startDate, endDate))
                {
                    MessageBox.Show("Selected dates overlap with an existing reservation.", "Unavailable Dates", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsDateRangeUnavailable(DateTime start, DateTime end)
        {
            return _existingRequests.Any(r => start < r.EndDate && end > r.StartDate);
        }

        private void ReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both start and end dates.", "Missing Dates", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var startDate = StartDatePicker.SelectedDate.Value;
            var endDate = EndDatePicker.SelectedDate.Value;

            if (IsDateRangeUnavailable(startDate, endDate))
            {
                MessageBox.Show("Selected dates overlap with an existing reservation.", "Unavailable Dates", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var currentUser = _authContext.GetCurrentUser();
            if (currentUser != null && _selectedCar != null)
            {
                _requestRepository.CreateRequest(_selectedCar.Id, currentUser.Id, startDate, endDate, false);
                MessageBox.Show("Reservation successfully created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error creating reservation. Ensure you are logged in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToMainView_Click(object sender, RoutedEventArgs e)
        {
            CarDetailView.Visibility = Visibility.Collapsed;
            MainView.Visibility = Visibility.Visible;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }
    }
}
