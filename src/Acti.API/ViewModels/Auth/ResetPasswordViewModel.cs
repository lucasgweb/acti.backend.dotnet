using System.ComponentModel.DataAnnotations;

namespace Acti.API.ViewModels.Auth;

public class ResetPasswordViewModel
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Token { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string PasswordConfirm { get; set; }
}