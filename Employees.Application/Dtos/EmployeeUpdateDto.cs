namespace Employees.Application.Dtos
{
    public class EmployeeUpdateDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }

        public string DepartmentName { get; set; }
    }
}
