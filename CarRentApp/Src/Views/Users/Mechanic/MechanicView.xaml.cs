using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;
using CarRentApp.Src.Repositories;

namespace CarRentApp.Views.Users.Mechanic
{
    public partial class MechanicView : UserControl
    {
        public event RoutedEventHandler? Logout;

        private readonly AuthContext _authContext;
        private readonly CarRepository _carRepository;
        private readonly RepairRepository _repairRepository;
        private readonly RequestRepository _requestRepository;

        private List<Car> _allCars = [];

        public MechanicView(DatabaseContext dbContext)
        {
            InitializeComponent();

            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            _carRepository = new CarRepository(dbContext);
            _repairRepository = new RepairRepository(dbContext);
            _requestRepository = new RequestRepository(dbContext);

            _allCars = _carRepository.GetCars();
            PopulateComboBoxes();

            LoadUserInfo();
            LoadCarList();
            LoadRepairs();
        }

        #region Filtering Helpers

        private IEnumerable<Car> GetFilteredCarsFromAll()
        {
            IEnumerable<Car> filtered = _allCars;
            if (MakeComboBox.SelectedItem != null && MakeComboBox.SelectedItem.ToString() != "All")
            {
                filtered = filtered.Where(car => car.Make.Equals(MakeComboBox.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            if (ModelComboBox.SelectedItem != null && ModelComboBox.SelectedItem.ToString() != "All")
            {
                filtered = filtered.Where(car => car.Model.Equals(ModelComboBox.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            if (YearComboBox.SelectedItem != null && YearComboBox.SelectedItem.ToString() != "All" &&
                int.TryParse(YearComboBox.SelectedItem.ToString(), out int selectedYear))
            {
                filtered = filtered.Where(car => car.Year == selectedYear);
            }

            if (HorsePowerComboBox.SelectedItem != null && HorsePowerComboBox.SelectedItem.ToString() != "All" &&
                int.TryParse(HorsePowerComboBox.SelectedItem.ToString(), out int selectedHorsePower))
            {
                filtered = filtered.Where(car => car.HorsePower == selectedHorsePower);
            }

            return filtered;
        }

        private IEnumerable<Car> GetCarsByMake()
        {
            IEnumerable<Car> filtered = _allCars;
            if (MakeComboBox.SelectedItem != null && MakeComboBox.SelectedItem.ToString() != "All")
            {
                filtered = filtered.Where(car => car.Make.Equals(MakeComboBox.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            return filtered;
        }

        private IEnumerable<Car> GetCarsByMakeAndModel()
        {
            IEnumerable<Car> filtered = GetCarsByMake();
            if (ModelComboBox.SelectedItem != null && ModelComboBox.SelectedItem.ToString() != "All")
            {
                filtered = filtered.Where(car => car.Model.Equals(ModelComboBox.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            return filtered;
        }


        private IEnumerable<Car> GetCarsByMakeModelYear()
        {
            IEnumerable<Car> filtered = GetCarsByMakeAndModel();
            if (YearComboBox.SelectedItem != null && YearComboBox.SelectedItem.ToString() != "All" &&
                int.TryParse(YearComboBox.SelectedItem.ToString(), out int selectedYear))
            {
                filtered = filtered.Where(car => car.Year == selectedYear);
            }

            return filtered;
        }

        #endregion

        #region ComboBox Population & Updates


        private void PopulateComboBoxes()
        {
            List<string> makes = _allCars
                .Select(car => car.Make)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            makes.Insert(0, "All");
            MakeComboBox.ItemsSource = makes;
            MakeComboBox.SelectedIndex = 0;

            UpdateComboBoxes();
        }

        private void UpdateComboBoxes()
        {
            List<string> models = GetCarsByMake()
                .Select(car => car.Model)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            models.Insert(0, "All");
            ModelComboBox.ItemsSource = models;
            if (ModelComboBox.SelectedItem == null || (ModelComboBox.SelectedItem is string selectedModel && !models.Contains(selectedModel)))
            {
                ModelComboBox.SelectedIndex = 0;
            }

            List<string> years = GetCarsByMakeAndModel()
                .Select(car => car.Year.ToString())
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            years.Insert(0, "All");
            YearComboBox.ItemsSource = years;
            if (YearComboBox.SelectedItem == null || (YearComboBox.SelectedItem is string selectedYear && !years.Contains(selectedYear)))
            {
                YearComboBox.SelectedIndex = 0;
            }

            List<string> horsePowers = GetCarsByMakeModelYear()
                .Select(car => car.HorsePower.ToString())
                .Distinct()
                .OrderBy(s => s)
                .ToList();
            horsePowers.Insert(0, "All");
            HorsePowerComboBox.ItemsSource = horsePowers;
            if (HorsePowerComboBox.SelectedItem == null || (HorsePowerComboBox.SelectedItem is string selectedHorsePower && !horsePowers.Contains(selectedHorsePower)))
            {
                HorsePowerComboBox.SelectedIndex = 0;
            }
        }

        #endregion

        #region Event Handlers

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateComboBoxes();
            LoadCarList();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authContext.Logout();
            Logout?.Invoke(this, new RoutedEventArgs());
        }

        private void SetAsRepaired_Click(object sender, RoutedEventArgs e)
        {
            if (CarDataGrid.SelectedItem is Car selectedCar)
            {
                int carId = selectedCar.Id;

                RepairDetailsWindow repairWindow = new RepairDetailsWindow();
                bool? result = repairWindow.ShowDialog();

                if (result == true)
                {
                    ObservableCollection<RepairItem> repairItems = repairWindow.RepairItems;
                    decimal totalCost = repairItems.Sum(item => item.Cost * item.Quantity);
                    string repairSummary = repairWindow.GetRepairItemsSummary();

                    _repairRepository.MarkCarAsRepaired(carId, repairItems, totalCost, repairSummary);

                    MessageBox.Show(
                        $"Car marked as repaired with a total repair cost of {totalCost:C}.\n\nDetails:\n{repairSummary}",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                    LoadCarList();
                    LoadRepairs();
                }
            }
            else
            {
                MessageBox.Show(
                    "Please select a car row before setting it as repaired.",
                    "No Car Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void SetAsNotRepaired_Click(object sender, RoutedEventArgs e)
        {
            if (CarDataGrid.SelectedItem is Car selectedCar)
            {
                _carRepository.UpdateCar(
                    selectedCar.Id,
                    selectedCar.Make,
                    selectedCar.Model,
                    selectedCar.Year,
                    selectedCar.HorsePower,
                    CarState.Unavailable
                );

                MessageBox.Show(
                        $"Car marked as Unavailable (Car cannot be fixed)",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );

                LoadCarList();
                LoadRepairs();
            }
            else
            {
                MessageBox.Show(
                    "Please select a car row before setting it as destroyed.",
                    "No Car Selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadCarList();
            LoadRepairs();
        }

        #endregion

            #region Data Loading


        private void LoadCarList()
        {
            IEnumerable<Car> cars = GetFilteredCarsFromAll()
                .Where(car => car.CarState == CarState.InService);
            CarDataGrid.ItemsSource = cars.ToList();
        }

        private void LoadRepairs()
        {
            List<Repair> repairs = _repairRepository.GetAllRepairs();
            RepairsDataGrid.ItemsSource = repairs;
        }

        private void LoadUserInfo()
        {
            User? currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = currentUser != null
                ? $"Logged in as: {currentUser.Name} {currentUser.Surname}"
                : "No user is currently logged in.";
        }

        #endregion
    }
}
