using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private List<Car> _cars; // List of cars
        private Car _selectedCar; // Selected car

        public CustomerView()
        {
            InitializeComponent();
            _authContext = AuthContext.GetInstance();
            _carRepository = new CarRepository();

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
                                Foreground = Brushes.Black,
                                Margin = new Thickness(5)
                            },
                            new TextBlock
                            {
                                Text = $"Year: {car.Year}, HP: {car.HorsePower}",
                                FontSize = 14,
                                Foreground = Brushes.Gray,
                                Margin = new Thickness(5)
                            }
                        }
                    },
                    Background = Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    Margin = new Thickness(10),
                    Padding = new Thickness(5),
                    Tag = car
                };

                carCard.Click += CarCard_Click;
                CarsGrid.Children.Add(carCard);
            }
        }

        private void CarCard_Click(object sender, RoutedEventArgs e)
        {
            var car = (sender as Button)?.Tag as Car;

            if (car != null)
            {
                _selectedCar = car;

                MainView.Visibility = Visibility.Collapsed;
                CarDetailView.Visibility = Visibility.Visible;

                CarDetailTitle.Text = $"{car.Make} {car.Model} ({car.Year})";
                CarDetailDescription.Text = $"HorsePower: {car.HorsePower}\nState: {car.CarState}";
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

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = _authContext.GetCurrentUser();
            if (currentUser != null)
            {
                MessageBox.Show($"Name: {currentUser.Name}\nSurname: {currentUser.Surname}\nEmail: {currentUser.Email}",
                                "User Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No user information available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
