using System.ComponentModel.DataAnnotations;

namespace CarRentApp.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Make { get; set; }

        [Required]
        public required string Model { get; set; }

        public int Year { get; set; }

        public int HorsePower { get; set; }

        [Required]
        public required CarState CarState { get; set; }
    }
}
