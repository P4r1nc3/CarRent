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

        public MechanicView(DatabaseContext dbContext)
        {
            InitializeComponent();

            _authContext = AuthContext.GetInstance();
            _authContext.CurrentUserChanged += LoadUserInfo;

            _carRepository = new CarRepository(dbContext);
            _repairRepository = new RepairRepository(dbContext);
            _requestRepository = new RequestRepository(dbContext);

            PopulateComboBoxes();

            LoadUserInfo();
            LoadCarList();
            LoadRepairs();
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



        private void SetAsRepaired_Click(object sender, RoutedEventArgs e)
        {
            if (CarDataGrid.SelectedItem is Car selectedCar)
            {
                int carId = selectedCar.Id;

                RepairDetailsWindow repairWindow = new RepairDetailsWindow();
                bool? result = repairWindow.ShowDialog();

                if (result == true)
                {
                    System.Collections.ObjectModel.ObservableCollection<RepairItem> repairItems = repairWindow.RepairItems;
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

        private void LoadUserInfo()
        {
            User? currentUser = _authContext.GetCurrentUser();
            UserInfoTextBlock.Text = currentUser != null
                ? $"Logged in as: {currentUser.Name} {currentUser.Surname}"
                : "No user is currently logged in.";
        }

        private void LoadRepairs()
        {
            List<Repair> repairs = _repairRepository.GetAllRepairs();
            RepairsDataGrid.ItemsSource = repairs;
        }
    }
}
