using AutoMapper;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;

namespace CompanyWebApi.Contracts.MappingProfiles.V3.Tests;

public class DepartmentProfileTests
{
    private readonly IMapper _mapper;

    public DepartmentProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DepartmentProfile>();
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
            Name = "Engineering",
            CompanyId = 123,
            Company = new Company { Name = "TechCorp" },
            Employees = new List<Employee>
            {
                new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmployeeAddresses = new List<EmployeeAddress>
                    {
                        new EmployeeAddress
                        {
                            AddressTypeId = AddressType.Work,
                            Address = "123 Work St"
                        }
                    },
                    User = new User
                    {
                        Username = "jdoe"
                    },
                    Department = new Department { Name = "Engineering" }
                },
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    EmployeeAddresses = new List<EmployeeAddress>(),
                    User = null,
                    Department = new Department { Name = "Engineering" }
                }
            }
        };

        // Act
        var departmentDto = _mapper.Map<DepartmentDto>(department);

        // Assert
        Assert.NotNull(departmentDto);
        Assert.Equal(1, departmentDto.DepartmentId);
        Assert.Equal("Engineering", departmentDto.Name);
        Assert.Equal(123, departmentDto.CompanyId);
        Assert.Equal("TechCorp", departmentDto.CompanyName); // Make sure this property is included in the profile mapping

        Assert.NotNull(departmentDto.Employees);
        Assert.Equal(2, departmentDto.Employees.Count);

        Assert.Equal("John Doe, Address: 123 Work St, Department: Engineering, Username: jdoe", departmentDto.Employees.First());
        Assert.Equal("Jane Smith, Address: , Department: Engineering, Username: ", departmentDto.Employees.Last());

    }

    [Fact]
    public void DepartmentsToDepartmentDtos_ShouldMapCorrectly()
    {
        // Arrange
        var departments = new List<Department>
        {
            new Department
            {
                DepartmentId = 1,
                Name = "Engineering",
                CompanyId = 123,
                Company = new Company { Name = "TechCorp" },
                Employees = new List<Employee>
                {
                    new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        EmployeeAddresses = new List<EmployeeAddress>
                        {
                            new EmployeeAddress
                            {
                                AddressTypeId = AddressType.Work,
                                Address = "123 Work St"
                            }
                        },
                        User = new User
                        {
                            Username = "jdoe"
                        },
                        Department = new Department { Name = "Engineering" }
                    },
                    new Employee
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        EmployeeAddresses = new List<EmployeeAddress>(),
                        User = null,
                        Department = new Department { Name = "Engineering" }
                    }
                }
            },
            new Department
            {
                DepartmentId = 2,
                Name = "HR",
                CompanyId = 123,
                Company = new Company { Name = "TechCorp" },
                Employees = new List<Employee>
                {
                    new Employee
                    {
                        FirstName = "Alice",
                        LastName = "Johnson",
                        EmployeeAddresses = new List<EmployeeAddress>
                        {
                            new EmployeeAddress
                            {
                                AddressTypeId = AddressType.Work,
                                Address = "456 HR St"
                            }
                        },
                        User = new User
                        {
                            Username = "ajohnson"
                        },
                        Department = new Department { Name = "HR" }
                    }
                }
            }
        };

        // Act
        var departmentDtos = _mapper.Map<IList<DepartmentDto>>(departments);

        // Assert
        Assert.Equal(departments.Count, departmentDtos.Count);

        var firstDepartment = departments.First(); 
        var firstDepartmentDto = departmentDtos.First();
        
        Assert.Equal(firstDepartment.DepartmentId, firstDepartmentDto.DepartmentId);
        Assert.Equal(firstDepartment.Name, firstDepartmentDto.Name);
        Assert.Equal(firstDepartment.CompanyId, firstDepartmentDto.CompanyId);
        Assert.Equal(firstDepartment.Company.Name, firstDepartmentDto.CompanyName);
        Assert.Equal(2, firstDepartmentDto.Employees.Count);
        Assert.Equal("John Doe, Address: 123 Work St, Department: Engineering, Username: jdoe", firstDepartmentDto.Employees.First());
        Assert.Equal("Jane Smith, Address: , Department: Engineering, Username: ", firstDepartmentDto.Employees.Last());

        var secondDepartment = departments.Last();
        var secondDepartmentDto = departmentDtos.Last();
        Assert.Equal(secondDepartment.DepartmentId, secondDepartmentDto.DepartmentId);
        Assert.Equal(secondDepartment.Name, secondDepartmentDto.Name);
        Assert.Equal(secondDepartment.CompanyId, secondDepartmentDto.CompanyId);
        Assert.Equal(secondDepartment.Company.Name, secondDepartmentDto.CompanyName);
        Assert.Single(secondDepartmentDto.Employees);
        Assert.Equal("Alice Johnson, Address: 456 HR St, Department: HR, Username: ajohnson", secondDepartmentDto.Employees.First());
    }
}
