using System.ComponentModel.DataAnnotations;

namespace Acti.API.ViewModels;

public class UpdateUserViewModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(100)]
    public string? Name { get; set; }

    [MinLength(7)]
    [MaxLength(180)]
    [EmailAddress]
    public string? Email { get; set; }

    [MinLength(8)]
    [MaxLength(180)]
    public string? Password { get; set; }

    [MinLength(8)][MaxLength(180)] public string? Avatar { get; set; }
}