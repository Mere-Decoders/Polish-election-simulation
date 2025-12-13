using System.ComponentModel.DataAnnotations;
namespace backend.Models.DBModels;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(32)]
    public required string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public required string PasswordHash { get; set; }
}
