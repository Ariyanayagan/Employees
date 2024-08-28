using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.Commands
{
    public record  DeleteEmployeeCommand(int id) : IRequest;
   
}
