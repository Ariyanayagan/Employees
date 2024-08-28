using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Employees.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and confirm password should match!")]
        public string ConfirmPassword { get; set; }
    }
}
