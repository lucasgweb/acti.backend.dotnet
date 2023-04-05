using System.ComponentModel.DataAnnotations;

namespace Acti.API.ViewModels;

public class CreateUserViewModel
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MinLength(7)]
    [MaxLength(180)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(180)]
    public string Password { get; set; }

    [MinLength(8)][MaxLength(180)] public string? Avatar { get; set; }
}