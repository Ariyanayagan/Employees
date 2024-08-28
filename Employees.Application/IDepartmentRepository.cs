using Employees.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application
{
    public interface IDepartmentRepository
    {
        Task<Department?> GetDepartmentByNameAsync(string departmentName);

        Task<IEnumerable<Department>> GetDepartmentAsync();
    }
}
