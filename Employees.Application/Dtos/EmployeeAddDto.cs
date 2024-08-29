using System.ComponentModel.DataAnnotations;

namespace Employees.Application.Dtos
{
    public class EmployeeAddDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(1000000000, 9999999999, ErrorMessage = "Phone number must be exactly 10 digits long.")]
        public long PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string DepartmentName { get; set; }


    }
}
