using Employees.Domain.Entities;
using MediatR;

public record GetEmployeeByIdQuery(int Id) : IRequest<Employee>;

