using AutoMapper;
using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Contracts.MappingProfiles.V3;
using System;
using System.Collections.Generic;
using Xunit;

namespace CompanyWebApi.Contracts.MappingProfiles.V3.Tests;

public class EmployeeProfileTests
{
    private readonly IMapper _mapper;

    public EmployeeProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<EmployeeProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void EmployeeToEmployeeDto_ShouldMapCorrectly()
    {
        // Arrange
        var employee = new Employee
        {
            EmployeeId = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1),
            CompanyId = 123,
            Company = new Company { Name = "TechCorp" },
            DepartmentId = 10,
            Department = new Department { Name = "Engineering" },
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
            }
        };

        // Act
        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        // Assert
        Assert.NotNull(employeeDto);
        Assert.Equal(employee.EmployeeId, employeeDto.EmployeeId);
        Assert.Equal(employee.FirstName, employeeDto.FirstName);
        Assert.Equal(employee.LastName, employeeDto.LastName);
        Assert.Equal(employee.BirthDate, employeeDto.BirthDate);
        Assert.Equal(employee.Age, employeeDto.Age);
        Assert.Equal("123 Work St", employeeDto.Addresses.FirstOrDefault()?.Address);
        Assert.Equal("jdoe", employeeDto.Username);
        Assert.Equal(employee.CompanyId, employeeDto.CompanyId);
        Assert.Equal("TechCorp", employeeDto.Company);
        Assert.Equal(employee.DepartmentId, employeeDto.DepartmentId);
        Assert.Equal("Engineering", employeeDto.Department);
    }

    [Fact]
    public void EmployeesToEmployeeDtos_ShouldMapCorrectly()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee
            {
                EmployeeId = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                CompanyId = 123,
                Company = new Company { Name = "TechCorp" },
                DepartmentId = 10,
                Department = new Department { Name = "Engineering" },
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
                }
            },
            new Employee
            {
                EmployeeId = 2,
                FirstName = "Jane",
                LastName = "Smith",
                BirthDate = new DateTime(1985, 2, 15),
                CompanyId = 124,
                Company = new Company { Name = "Innovatech" },
                DepartmentId = 11,
                Department = new Department { Name = "HR" },
                EmployeeAddresses = new List<EmployeeAddress>(),
                User = null
            }
        };

        // Act
        var employeeDtos = _mapper.Map<IList<EmployeeDto>>(employees);

        // Assert
        Assert.NotNull(employeeDtos);
        Assert.Equal(2, employeeDtos.Count);

        var firstEmployeeDto = employeeDtos.First();
        Assert.Equal(1, firstEmployeeDto.EmployeeId);
        Assert.Equal("John", firstEmployeeDto.FirstName);
        Assert.Equal("Doe", firstEmployeeDto.LastName);
        Assert.Equal(new DateTime(1990, 1, 1), firstEmployeeDto.BirthDate);
        Assert.Equal("123 Work St", firstEmployeeDto.Addresses.FirstOrDefault()?.Address);
        Assert.Equal("jdoe", firstEmployeeDto.Username);
        Assert.Equal(123, firstEmployeeDto.CompanyId);
        Assert.Equal("TechCorp", firstEmployeeDto.Company);
        Assert.Equal(10, firstEmployeeDto.DepartmentId);
        Assert.Equal("Engineering", firstEmployeeDto.Department);

        var secondEmployeeDto = employeeDtos.Last();
        Assert.Equal(2, secondEmployeeDto.EmployeeId);
        Assert.Equal("Jane", secondEmployeeDto.FirstName);
        Assert.Equal("Smith", secondEmployeeDto.LastName);
        Assert.Equal(new DateTime(1985, 2, 15), secondEmployeeDto.BirthDate);
        Assert.Empty(secondEmployeeDto.Addresses); // No address provided
        Assert.Equal(string.Empty, secondEmployeeDto.Username); // No user provided
        Assert.Equal(124, secondEmployeeDto.CompanyId);
        Assert.Equal("Innovatech", secondEmployeeDto.Company);
        Assert.Equal(11, secondEmployeeDto.DepartmentId);
        Assert.Equal("HR", secondEmployeeDto.Department);
    }

    [Fact]
    public void EmployeeCreateDtoToEmployee_ShouldMapCorrectly()
    {
        // Arrange
        var employeeCreateDto = new EmployeeCreateDto
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1980, 1, 1),
            Addresses = new List<EmployeeAddressCreateWithoutEmployeeIdDto>
                {
                    new EmployeeAddressCreateWithoutEmployeeIdDto
                    {
                        AddressTypeId = AddressType.Work,
                        Address = "123 Work St"
                    }
                },
            Username = "jdoe",
            Password = "password123",
            CompanyId = 123,
            DepartmentId = 456
        };

        // Act
        var employee = _mapper.Map<Employee>(employeeCreateDto);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal("John", employee.FirstName);
        Assert.Equal("Doe", employee.LastName);
        Assert.Equal(new DateTime(1980, 1, 1), employee.BirthDate);
        Assert.Equal("123 Work St", employee.EmployeeAddresses.FirstOrDefault()?.Address);
        Assert.NotNull(employee.User);
        Assert.Equal("jdoe", employee.User.Username);
        Assert.Equal("password123", employee.User.Password);
        Assert.Equal(123, employee.CompanyId);
        Assert.Equal(456, employee.DepartmentId);
    }

    [Fact]
    public void EmployeeCreateDtoListToEmployeeList_ShouldMapCorrectly()
    {
        // Arrange
        var employeeCreateDtoList = new List<EmployeeCreateDto>
        {
            new EmployeeCreateDto
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1980, 1, 1),
                Addresses = new List<EmployeeAddressCreateWithoutEmployeeIdDto>
                {
                    new EmployeeAddressCreateWithoutEmployeeIdDto
                    {
                        AddressTypeId = AddressType.Work,
                        Address = "123 Work St"
                    }
                },
                Username = "jdoe",
                Password = "password123",
                CompanyId = 123,
                DepartmentId = 456
            },
            new EmployeeCreateDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                BirthDate = new DateTime(1990, 2, 2),
                Addresses = new List<EmployeeAddressCreateWithoutEmployeeIdDto>
                {
                    new EmployeeAddressCreateWithoutEmployeeIdDto
                    {
                        AddressTypeId = AddressType.Work,
                        Address = "456 Work Ave"
                    }
                },
                Username = "",
                Password = null,
                CompanyId = 789,
                DepartmentId = 101
            }
        };

        // Act
        var employeeList = _mapper.Map<IList<Employee>>(employeeCreateDtoList);

        // Assert
        Assert.NotNull(employeeList);
        Assert.Equal(2, employeeList.Count);

        var firstEmployee = employeeList.First();
        Assert.Equal("John", firstEmployee.FirstName);
        Assert.Equal("Doe", firstEmployee.LastName);
        Assert.Equal(new DateTime(1980, 1, 1), firstEmployee.BirthDate);
        Assert.Equal("123 Work St", firstEmployee.EmployeeAddresses.FirstOrDefault()?.Address);
        Assert.NotNull(firstEmployee.User);
        Assert.Equal("jdoe", firstEmployee.User.Username);
        Assert.Equal("password123", firstEmployee.User.Password);
        Assert.Equal(123, firstEmployee.CompanyId);
        Assert.Equal(456, firstEmployee.DepartmentId);

        var secondEmployee = employeeList.Last();
        Assert.Equal("Jane", secondEmployee.FirstName);
        Assert.Equal("Smith", secondEmployee.LastName);
        Assert.Equal(new DateTime(1990, 2, 2), secondEmployee.BirthDate);
        Assert.Equal("456 Work Ave", secondEmployee.EmployeeAddresses.FirstOrDefault()?.Address);
        Assert.Null(secondEmployee.User); // null because Username or Password is null, so can't create User object.
        Assert.Equal(789, secondEmployee.CompanyId);
        Assert.Equal(101, secondEmployee.DepartmentId);
    }
}
