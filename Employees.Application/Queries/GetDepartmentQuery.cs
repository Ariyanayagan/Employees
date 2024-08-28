using Employees.Domain.Entities;
using MediatR;

public record GetDepartmentQuery() : IRequest<IEnumerable<Department>>;

