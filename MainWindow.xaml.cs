using System.Linq;
using System.Windows;
using CarRentApp.Data;
using CarRentApp.Models;

namespace CarRentApp
{
    public partial class MainWindow : Window
    {
        private AppDbContext _dbContext;

        public MainWindow()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            LoadCars();
        }

        private void LoadCars()
        {
            CarsListView.ItemsSource = _dbContext.Cars.ToList();
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MakeTextBox.Text) ||
                string.IsNullOrWhiteSpace(ModelTextBox.Text) ||
                !int.TryParse(YearTextBox.Text, out int year) ||
                !decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                MessageBox.Show("Wypełnij wszystkie pola poprawnie!");
                return;
            }

            var newCar = new Car
            {
                Make = MakeTextBox.Text,
                Model = ModelTextBox.Text,
                Year = year,
                PricePerDay = price
            };

            _dbContext.Cars.Add(newCar);
            _dbContext.SaveChanges();

            LoadCars();

            MakeTextBox.Clear();
            ModelTextBox.Clear();
            YearTextBox.Clear();
            PriceTextBox.Clear();
        }
    }
}