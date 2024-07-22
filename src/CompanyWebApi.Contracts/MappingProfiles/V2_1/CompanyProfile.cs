using AutoMapper;
using CompanyWebApi.Contracts.Dto;
using CompanyWebApi.Contracts.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.MappingProfiles.V2_1
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom<CompanyEmployeesResolver>());
        }
    }

    public class CompanyEmployeesResolver : IValueResolver<Company, CompanyDto, ICollection<string>>
    {
        public ICollection<string> Resolve(Company source, CompanyDto destination, ICollection<string> destMember, ResolutionContext context)
        {
            var employees = source.Departments
                .SelectMany(d => d.Employees.Select(e => new
                {
                    Employee = e,
                    DepartmentName = d.Name
                }))
                .ToList();

            var employeeStrings = new List<string>();
            foreach (var emp in employees)
            {
                var address = emp.Employee.EmployeeAddresses?
                    .FirstOrDefault(a => a.AddressTypeId == AddressType.Work)?.Address ?? string.Empty;
                var username = emp.Employee.User?.Username ?? string.Empty;
                var employeeString = $"{emp.Employee.FirstName} {emp.Employee.LastName}, Address: {address}, Department: {emp.DepartmentName}, Username: {username}";
                employeeStrings.Add(employeeString);
            }

            return employeeStrings;
        }
    }
}
