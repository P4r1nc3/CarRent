using System.Windows;
using System.Windows.Controls;
using CarRentApp.Contexts;
using CarRentApp.Models;
using CarRentApp.Repositories;

namespace CarRentApp.Views.Users.Mechanic
{
    public partial class MechanicView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly CarRepository _carRepository;
        private readonly RequestRepository _requestRepository;

        public MechanicView(DatabaseContext dbContext)
        {
            InitializeComponent();

            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            _carRepository = new CarRepository(dbContext);
            _requestRepository = new RequestRepository(dbContext);

            PopulateComboBoxes();

            LoadUserInfo();
            LoadCarList();
        }

        private void PopulateComboBoxes()
        {
            List<Car> allCars = _carRepository.GetCars();

            List<string> makes = allCars.Select(car => car.Make)
                               .Distinct()
                               .OrderBy(s => s)
                               .ToList();
            makes.Insert(0, "All");
            MakeComboBox.ItemsSource = makes;
            MakeComboBox.SelectedIndex = 0;

            List<string> models = allCars.Select(car => car.Model)
                                .Distinct()
                                .OrderBy(s => s)
                                .ToList();
            models.Insert(0, "All");
            ModelComboBox.ItemsSource = models;
            ModelComboBox.SelectedIndex = 0;

            List<int> years = allCars.Select(car => car.Year)
                               .Distinct()
                               .OrderBy(y => y)
                               .ToList();

            List<string> yearStrings = years.Select(y => y.ToString()).ToList();
            yearStrings.Insert(0, "All");
            YearComboBox.ItemsSource = yearStrings;
            YearComboBox.SelectedIndex = 0;

            List<int> horsePowers = allCars.Select(car => car.HorsePower)
                                     .Distinct()
                                     .OrderBy(h => h)
                                     .ToList();
            List<string> hpStrings = horsePowers.Select(h => h.ToString()).ToList();
            hpStrings.Insert(0, "All");
            HorsePowerComboBox.ItemsSource = hpStrings;
            HorsePowerComboBox.SelectedIndex = 0;
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCarList();
        }

        private void LoadCarList()
        {
            IEnumerable<Car> cars = _carRepository.GetCars()
                                                    .Where(car => car.CarState == CarState.InService);

            if (MakeComboBox.SelectedItem != null)
            {
                string selectedMake = MakeComboBox.SelectedItem.ToString();
                if (!string.IsNullOrWhiteSpace(selectedMake) && selectedMake != "All")
                {
                    cars = cars.Where(car => car.Make.Equals(selectedMake, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (ModelComboBox.SelectedItem != null)
            {
                string selectedModel = ModelComboBox.SelectedItem.ToString();
                if (!string.IsNullOrWhiteSpace(selectedModel) && selectedModel != "All")
                {
                    cars = cars.Where(car => car.Model.Equals(selectedModel, StringComparison.OrdinalIgnoreCase));
                }
            }

            if (YearComboBox.SelectedItem != null)
            {
                string yearSelection = YearComboBox.SelectedItem.ToString();
                if (yearSelection != "All" && int.TryParse(yearSelection, out int selectedYear))
                {
                    cars = cars.Where(car => car.Year == selectedYear);
                }
            }

            if (HorsePowerComboBox.SelectedItem != null)
            {
                string hpSelection = HorsePowerComboBox.SelectedItem.ToString();
                if (hpSelection != "All" && int.TryParse(hpSelection, out int selectedHorsePower))
                {
                    cars = cars.Where(car => car.HorsePower == selectedHorsePower);
                }
            }

            CarDataGrid.ItemsSource = cars.ToList();
        }

        private void SetAsRepaired_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve selected values from the ComboBoxes.
            string make = MakeComboBox.SelectedItem?.ToString() ?? "";
            string model = ModelComboBox.SelectedItem?.ToString() ?? "";
            string yearText = YearComboBox.SelectedItem?.ToString() ?? "";
            string horsePowerText = HorsePowerComboBox.SelectedItem?.ToString() ?? "";

            // Basic validation: Ensure the user has selected specific values (not "All") in each field.
            if (string.IsNullOrEmpty(make) || string.IsNullOrEmpty(model) ||
                string.IsNullOrEmpty(yearText) || string.IsNullOrEmpty(horsePowerText) ||
                make == "All" || model == "All" || yearText == "All" || horsePowerText == "All")
            {
                MessageBox.Show("Please select specific values for each field.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(yearText, out int year) || !int.TryParse(horsePowerText, out int horsePower))
            {
                MessageBox.Show("Year and Horse Power must be valid numbers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // e.g.
                // _carRepository.MarkCarAsRepaired(make, model, year, horsePower);

                LoadCarList();

                MessageBox.Show("Car set as repaired successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void LoadUserInfo()
        {
            User? currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = currentUser != null
                ? $"Logged in as: {currentUser.Name} {currentUser.Surname}"
                : "No user is currently logged in.";
        }
    }
}
