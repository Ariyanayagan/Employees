using AutoMapper;
using Employees.Application.Dtos;
using Employees.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Application.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeAddDto, Employee>()
            .ForMember(dest => dest.DepartmentId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Department, opt => opt.Ignore())   
            .AfterMap((src, dest, context) =>
            {
                var department = context.Items["Department"] as Department;
                dest.Department = department;
                dest.DepartmentId = department.DepartmentId;
            }).ReverseMap();

            CreateMap<EmployeeUpdateDto, Employee>()
            .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .AfterMap((src, dest, context) =>
            {
                var department = context.Items["Department"] as Department;
                dest.Department = department;
                dest.DepartmentId = department.DepartmentId;
            }).ReverseMap();

            // Example mapping from Employee to a DTO
            //CreateMap<Employee, EmployeeDto>();
        }
    }
}
