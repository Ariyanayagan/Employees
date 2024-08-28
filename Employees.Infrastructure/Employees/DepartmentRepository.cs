using Employees.Domain.Entities;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public readonly AppDbcontext _dbcontext;

        public DepartmentRepository(AppDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<Department>> GetDepartmentAsync()
        {
            return await _dbcontext.Departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentByNameAsync(string departmentName)
        {
            return await _dbcontext.Departments
                             .FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
        }
    }
}
