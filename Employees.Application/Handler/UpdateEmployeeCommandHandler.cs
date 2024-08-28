using AutoMapper;
using Employees.Application.Commands;
using Employees.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Handler
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IMapper _mapper;
        private readonly IDepartmentRepository _departmentRepository;


        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(request.employeeUpdateDto.EmployeeId);
            if (existingEmployee == null)
            {
                throw new Exception("Employee not found");
            }

            var department = await _departmentRepository.GetDepartmentByNameAsync(request.employeeUpdateDto.DepartmentName);

            _mapper.Map(request.employeeUpdateDto, existingEmployee, opt =>
            {
                opt.Items["Department"] = department;
            });

            // Update the employee in the repository
            await _employeeRepository.UpdateEmployeeAsync(existingEmployee);

        }
    }
}
