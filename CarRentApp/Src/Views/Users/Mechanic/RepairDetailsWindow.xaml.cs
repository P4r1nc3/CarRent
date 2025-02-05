using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using CarRentApp.Src.Models; // Ensure this namespace contains RepairItem and other models

namespace CarRentApp.Views.Users.Mechanic
{
    public partial class RepairDetailsWindow : Window, INotifyPropertyChanged
    {
        // Observable collection for repair items.
        public ObservableCollection<RepairItem> RepairItems { get; set; } = [];

        // New property for an optional overall repair description.
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

            // Subscribe to collection changes to update the total cost.
            RepairItems.CollectionChanged += (s, e) =>
            {
                UpdateTotal();

                // Subscribe to PropertyChanged events for any new items.
                if (e.NewItems != null)
                {
                    foreach (RepairItem item in e.NewItems)
                    {
                        item.PropertyChanged += RepairItem_PropertyChanged;
                    }
                }

                // Unsubscribe for removed items.
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

        /// <summary>
        /// Recalculates the total cost (sum of Cost * Quantity for each repair item).
        /// </summary>
        private void UpdateTotal()
        {
            decimal totalCost = RepairItems.Sum(item => item.Cost * item.Quantity);
            TotalCostTextBlock.Text = totalCost.ToString("C");
        }

        /// <summary>
        /// Returns a formatted summary string for the repair items.
        /// If an overall repair description is provided, it is shown at the top.
        /// For each repair item, if the individual description is not provided,
        /// a default ("Item") is used.
        /// </summary>
        public string GetRepairItemsSummary()
        {
            if (!RepairItems.Any())
            {
                return "No repair items entered.";
            }

            StringBuilder sb = new StringBuilder();

            // If the mechanic provided an overall repair description, include it.
            if (!string.IsNullOrWhiteSpace(OverallDescription))
            {
                sb.AppendLine("Repair Description:");
                sb.AppendLine(OverallDescription);
                sb.AppendLine();
            }

            sb.AppendLine("Repair Items:");
            foreach (RepairItem item in RepairItems)
            {
                // Use "Item" as default if individual description is blank.
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
            // Optionally perform validation here.
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
