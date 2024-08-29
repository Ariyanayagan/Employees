using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Employees.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me ?")]
        public bool rememberMe { get; set; }

        public string returnUrl { get; set; } = string.Empty;

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();
    }
}
