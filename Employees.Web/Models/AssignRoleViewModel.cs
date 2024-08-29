using System.ComponentModel.DataAnnotations;

namespace Employees.Web.Models
{
    public class AssignRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
