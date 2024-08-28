using Employees.Domain.Entities;
using MediatR;

public record GetAllEmployeesQuery: IRequest<IEnumerable<Employee>>;