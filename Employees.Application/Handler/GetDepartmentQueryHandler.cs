using Employees.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Handler
{
    public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery,IEnumerable<Department>>
    {
        public readonly IDepartmentRepository DepartmentRepository;

        public GetDepartmentQueryHandler(IDepartmentRepository departmentRepository)
        {
            DepartmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            return await DepartmentRepository.GetDepartmentAsync();
        }
    }
}
