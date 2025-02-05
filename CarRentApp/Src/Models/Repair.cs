namespace CarRentApp.Src.Models
{
    public class Repair
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public string RepairSummary { get; set; } = string.Empty;

        public decimal TotalCost { get; set; }

        public DateTime RepairDate { get; set; } = DateTime.Now;

        public required Car Car { get; set; }

        public required ICollection<RepairItem> RepairItems { get; set; }
    }
}
