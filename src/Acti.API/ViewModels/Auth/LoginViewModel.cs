using System.ComponentModel.DataAnnotations;

namespace Acti.API.ViewModels.Auth;

public class LoginViewModel
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] [MinLength(8)] public string Password { get; set; }
}