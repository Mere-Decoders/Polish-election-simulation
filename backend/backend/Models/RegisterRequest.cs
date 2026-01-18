using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public required string Username { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
    }
}