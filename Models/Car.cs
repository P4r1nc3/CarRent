using System.ComponentModel.DataAnnotations;

namespace CarRentApp.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        public int Year { get; set; }

        public int HorsePower { get; set; }

        [Required]
        public CarState CarState { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }
    }
}
