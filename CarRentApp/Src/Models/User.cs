using System.ComponentModel.DataAnnotations;

namespace CarRentApp.Src.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required Role Role { get; set; }
    }
}
