using Employees.Application.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Handler
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        public readonly IEmployeeRepository _employeeRepo;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

            public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
            {
                var existingEmployee = await _employeeRepo.GetEmployeeByIdAsync(request.id);
                if (existingEmployee == null)
                {
                    throw new Exception("Employee not found");
                }
                await _employeeRepo.DeleteEmployeeAsync(request.id);
            }
    }
}
