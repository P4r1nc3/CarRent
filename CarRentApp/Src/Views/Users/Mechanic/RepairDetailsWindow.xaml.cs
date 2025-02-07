using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using CarRentApp.Src.Models;

namespace CarRentApp.Views.Users.Mechanic
{
    public partial class RepairDetailsWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<RepairItem> RepairItems { get; set; } = [];

        private string _overallDescription = string.Empty;
        public string OverallDescription
        {
            get => _overallDescription;
            set
            {
                if (_overallDescription != value)
                {
                    _overallDescription = value;
                    OnPropertyChanged(nameof(OverallDescription));
                }
            }
        }

        public RepairDetailsWindow()
        {
            InitializeComponent();
            DataContext = this;

            RepairItems.CollectionChanged += (s, e) =>
            {
                UpdateTotal();

                if (e.NewItems != null)
                {
                    foreach (RepairItem item in e.NewItems)
                    {
                        item.PropertyChanged += RepairItem_PropertyChanged;
                    }
                }

                if (e.OldItems != null)
                {
                    foreach (RepairItem item in e.OldItems)
                    {
                        item.PropertyChanged -= RepairItem_PropertyChanged;
                    }
                }
            };
        }

        private void RepairItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is (nameof(RepairItem.Cost)) or (nameof(RepairItem.Quantity)))
            {
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            decimal totalCost = RepairItems.Sum(item => item.Cost * item.Quantity);
            TotalCostTextBlock.Text = totalCost.ToString("C");
        }

        public string GetRepairItemsSummary()
        {
            if (!RepairItems.Any())
            {
                return "No repair items entered.";
            }

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(OverallDescription))
            {
                sb.AppendLine("Repair Description:");
                sb.AppendLine(OverallDescription);
                sb.AppendLine();
            }

            sb.AppendLine("Repair Items:");
            foreach (RepairItem item in RepairItems)
            {
                string itemDesc = string.IsNullOrWhiteSpace(item.Description) ? "Item" : item.Description;
                decimal subTotal = item.Cost * item.Quantity;
                sb.AppendLine($"{itemDesc} ({item.Quantity} x {item.Cost:C} = {subTotal:C})");
            }
            sb.AppendLine();
            sb.AppendLine($"Total Cost: {RepairItems.Sum(item => item.Cost * item.Quantity):C}");
            return sb.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
