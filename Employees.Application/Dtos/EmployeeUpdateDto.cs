using System.ComponentModel.DataAnnotations;

namespace Employees.Application.Dtos
{
    public class EmployeeUpdateDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }


        [Required]
        [MaxLength(10)]
        public long PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        public string DepartmentName { get; set; }
    }
}
