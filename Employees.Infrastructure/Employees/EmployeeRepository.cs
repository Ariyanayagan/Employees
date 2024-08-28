using Azure.Core;
using Employees.Application;
using Employees.Domain.Entities;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Infrastructure.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbcontext _Dbcontext;

        public EmployeeRepository(AppDbcontext appDbcontext)
        {
            this._Dbcontext = appDbcontext;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _Dbcontext.AddAsync(employee);
            await _Dbcontext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _Dbcontext.Employees.FindAsync(id);
            if (employee != null)
            {
                _Dbcontext.Employees.Remove(employee);
                await _Dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Employee not found");
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _Dbcontext.Employees.Include(e => e.Department).ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int? id)
        {
            return await _Dbcontext.Employees
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public  IEnumerable<Employee> GetEmployees()
        {
            return _Dbcontext.Employees.Include("Department").ToList();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
             _Dbcontext.Update(employee);
            await _Dbcontext.SaveChangesAsync();

        }
    }
}
