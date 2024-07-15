using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace CompanyWebApi.Contracts.Converters.V3;

/// <summary>
/// Employee from EmployeeCreateDto converter
/// </summary>
public class EmployeeFromDtoConverter : IConverter<EmployeeCreateDto, Employee>, IConverter<IList<EmployeeCreateDto>, IList<Employee>>
{
    private readonly ILogger<EmployeeFromDtoConverter> _logger;

    public EmployeeFromDtoConverter(ILogger<EmployeeFromDtoConverter> logger)
    {
        _logger = logger;
    }

    public Employee Convert(EmployeeCreateDto employee)
    {
        _logger.LogDebug("Convert");
        var employeeDto = new Employee
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            CompanyId = employee.CompanyId,
            DepartmentId = employee.DepartmentId,
            BirthDate = employee.BirthDate,
            EmployeeAddresses = employee.Addresses?.Select(a => new EmployeeAddress
            {
                AddressTypeId = a.AddressTypeId,
                Address = a.Address
            }).ToList(),
        };

        if (!string.IsNullOrEmpty(employee.Username) &&
            !string.IsNullOrEmpty(employee.Password))
        {
            employeeDto.User = new User
            {
                Username = string.IsNullOrEmpty(employee.Username) ? string.Empty : employee.Username,
                Password = string.IsNullOrEmpty(employee.Password) ? string.Empty : employee.Password
            };
        }

        return employeeDto;
    }

    public IList<Employee> Convert(IList<EmployeeCreateDto> employees)
    {
        _logger.LogDebug("ConvertList");
        return employees.Select(Convert).ToList();
    }
}
