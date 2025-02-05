using System.ComponentModel;

namespace CarRentApp.Src.Models
{
    public class RepairItem : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private decimal _cost;
        public decimal Cost
        {
            get => _cost;
            set
            {
                if (_cost != value)
                {
                    _cost = value;
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }

        private int _quantity = 1;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public int RepairId { get; set; }

        public required Repair Repair { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
