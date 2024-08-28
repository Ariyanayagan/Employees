using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Domain.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }

        // Navigation property
        public Department Department { get; set; }
    }
}
