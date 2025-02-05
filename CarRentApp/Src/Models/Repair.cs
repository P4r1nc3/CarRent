namespace CarRentApp.Src.Models
{
    public class Repair
    {
        public int Id { get; set; }

        // Foreign key to Car.
        public int CarId { get; set; }

        // A human-readable summary string.
        public string RepairSummary { get; set; } = string.Empty;

        // The total cost of the repair.
        public decimal TotalCost { get; set; }

        // The date the repair was performed.
        public DateTime RepairDate { get; set; } = DateTime.Now;

        // Navigation property to the associated Car.
        public required Car Car { get; set; }

        // Navigation property for the repair items.
        public required ICollection<RepairItem> RepairItems { get; set; }
    }
}
