using System.ComponentModel.DataAnnotations;

namespace Acti.API.ViewModels.Auth
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public  string Email { get; set; }
    }
}
