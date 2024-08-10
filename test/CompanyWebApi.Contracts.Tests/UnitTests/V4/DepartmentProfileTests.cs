using AutoMapper;
using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V4.Tests;

/// <summary>
/// Suggested Unit Tests
/// 
/// 1. Basic Mapping Verification: Test that all properties of the Department 
///    entity are correctly mapped to the DepartmentDto and DepartmentFullDto.
/// 2. Nested Object Mapping: Ensure that nested collections, such as Employees, 
///    are correctly mapped, and properties within these collections 
///    are accurately transferred.
/// 3. Null and Default Values Handling: Verify that null and default values 
///    in the source objects are handled correctly and do not cause issues 
///    during the mapping process.
/// 4. Complex Property Mapping: Verify that properties such as CompanyName 
///    and Username are correctly handled, even when the associated objects are null.
/// 
/// </summary>
public class DepartmentProfileTests
{
    private readonly IMapper _mapper;

    public DepartmentProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DepartmentProfile());
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void DepartmentToDepartmentDto_ShouldMapCorrectly()
    {
        // Arrange
        var department = new Department
        {
            DepartmentId = 1,
            Name = "HR",
            CompanyId = 2,
            Company = new Company
            {
                CompanyId = 2,
                Name = "Test Company"
            },
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        // Act
        var departmentDto = _mapper.Map<DepartmentDto>(department);

        // Assert
        Assert.Equal(department.DepartmentId, departmentDto.DepartmentId);
        Assert.Equal(department.Name, departmentDto.Name);
        Assert.Equal(department.CompanyId, departmentDto.CompanyId);
        Assert.Equal(department.Company.Name, departmentDto.CompanyName);
        Assert.Equal(department.Created, departmentDto.Created);
        Assert.Equal(department.Modified, departmentDto.Modified);
    }

    [Fact]
    public void DepartmentToDepartmentFullDto_ShouldMapCorrectly()
    {
        // Arrange
        var department = new Department
        {
            DepartmentId = 1,
            Name = "HR",
            CompanyId = 2,
            Company = new Company
            {
                CompanyId = 2,
                Name = "Test Company"
            },
            Employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    EmployeeAddresses = new List<EmployeeAddress>
                    {
                        new EmployeeAddress
                        {
                            AddressTypeId = AddressType.Work,
                            Address = "123 Main St"
                        }
                    },
                    User = new User
                    {
                        Username = "jdoe"
                    }
                }
            },
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        // Act
        var departmentFullDto = _mapper.Map<DepartmentFullDto>(department);

        // Assert
        Assert.Equal(department.DepartmentId, departmentFullDto.DepartmentId);
        Assert.Equal(department.Name, departmentFullDto.Name);
        Assert.Equal(department.CompanyId, departmentFullDto.CompanyId);
        Assert.Equal(department.Company.Name, departmentFullDto.CompanyName);
        Assert.Single(departmentFullDto.Employees);

        var employeeDto = departmentFullDto.Employees.First();
        Assert.Equal(1, employeeDto.EmployeeId);
        Assert.Equal("John", employeeDto.FirstName);
        Assert.Equal("Doe", employeeDto.LastName);
        Assert.Single(employeeDto.Addresses);
        Assert.Equal("123 Main St", employeeDto.Addresses.First().Address);
        Assert.Equal("jdoe", employeeDto.Username);
    }

    [Fact]
    public void DepartmentToDepartmentFullDto_ShouldHandleNullValues()
    {
        // Arrange
        var department = new Department
        {
            DepartmentId = 1,
            Name = "HR",
            CompanyId = 2,
            Company = new Company
            {
                CompanyId = 2,
                Name = "Test Company"
            },
            Employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    EmployeeAddresses = null,
                    User = null
                }
            },
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow
        };

        // Act
        var departmentFullDto = _mapper.Map<DepartmentFullDto>(department);

        // Assert
        var employeeDto = departmentFullDto.Employees.First();
        Assert.NotNull(employeeDto.Addresses); // Should not be null, even if source is null
        Assert.Empty(employeeDto.Addresses); // Should be an empty list
        Assert.Equal(string.Empty, employeeDto.Username); // Should be empty string if User is null
    }
}
