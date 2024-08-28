using AutoMapper;
using Employees.Application.Commands;
using Employees.Application.Dtos;
using Employees.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Handler
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, IDepartmentRepository departmentRepository)
        {
            _EmployeeRepository = employeeRepository;
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        async Task IRequestHandler<CreateEmployeeCommand>.Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetDepartmentByNameAsync(request.employeeAddDto.DepartmentName);
            var employee = _mapper.Map<Employee>(request.employeeAddDto, opt =>
            {
                opt.Items["Department"] = department;
            });
            //var employee = _mapper.Map<Employee>(request.employeeAddDto);
            await _EmployeeRepository.AddEmployeeAsync(employee);
        }
    }
}
