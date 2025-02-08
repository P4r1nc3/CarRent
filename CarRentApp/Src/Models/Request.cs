using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentApp.Src.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public required Car Car { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public required User User { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        [Required]
        public required DateTime EndDate { get; set; }

        [Required]
        public required RequestState RequestState { get; set; }
    }
}
