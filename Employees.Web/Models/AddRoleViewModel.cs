using System.ComponentModel.DataAnnotations;

namespace Employees.Web.Models
{
    public class AddRoleViewModel
    {
        [Required]
        public string role { get; set; }
    }
}
