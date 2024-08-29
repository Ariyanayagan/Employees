using System.ComponentModel.DataAnnotations;

namespace Employees.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
