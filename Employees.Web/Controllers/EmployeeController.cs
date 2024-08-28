using Employees.Application;
using Employees.Application.Commands;
using Employees.Application.Dtos;
using Employees.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Employees.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ISender _sender;

        public EmployeeController(IEmployeeService employeeService, ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _sender.Send(new GetAllEmployeesQuery());
            return View(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employees = await _sender.Send(new GetEmployeeByIdQuery(id));

            if (employees is null)
            {
                return NotFound();
            }
            return View(employees);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var departments = await _sender.Send(new GetDepartmentQuery());

            var departmentSelectList = departments.Select(department => new SelectListItem
            {
                Value = department.DepartmentName ?? "",
                Text = department.DepartmentName ?? ""
            }).ToList();

            ViewBag.Departments = departmentSelectList;

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var departments = await _sender.Send(new GetDepartmentQuery());

            var departmentSelectList = departments.Select(department => new SelectListItem
            {
                Value = department.DepartmentName ?? "",
                Text = department.DepartmentName ?? ""
            }).ToList();

            ViewBag.Departments = departmentSelectList;

            var employee = await _sender.Send(new GetEmployeeByIdQuery(id));

            var model = new EmployeeUpdateDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Address = employee.Address,
                EmployeeId = employee.EmployeeId,
                DepartmentName = employee.Department.DepartmentName,
                PhoneNumber = employee.PhoneNumber,

            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EmployeeAddDto employeeAddDto)
        {
            if (ModelState.IsValid)
            {
                await _sender.Send(new CreateEmployeeCommand(employeeAddDto));
                TempData["success"] = "Employee created successfully!";
                return RedirectToAction("Index");
            }

            return View(employeeAddDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(EmployeeUpdateDto employeeUpdateDto)
        {
            if (ModelState.IsValid)
            {
                await _sender.Send(new UpdateEmployeeCommand(employeeUpdateDto));
                TempData["success"] = "Employee Update successfully!";
                return RedirectToAction("Index");
            }

            return View(employeeUpdateDto);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _sender.Send(new DeleteEmployeeCommand(id));
            TempData["success"] = "Employee deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}
