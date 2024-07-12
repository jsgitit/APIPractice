using CompanyWebApi.Contracts.Dto.V3;
using CompanyWebApi.Tests.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.V3;

public class EmployeesControllerTests : ControllerTestsBase
{
    private const string API_VERSION = "V3";
    private readonly string _baseUrl;
    private readonly HttpClientHelper _httpClientHelper;

    public EmployeesControllerTests(WebApiTestFactory factory)
        : base(factory)
    {
        _baseUrl = $"/api/{API_VERSION.ToLower()}/employees/";
        _httpClientHelper = new HttpClientHelper(Client);
        _httpClientHelper.Client.SetFakeBearerToken((object)Token);
    }

    [Fact]
    public async Task CanCreateAndDeleteEmployee()
    {
        // Arrange
        var newEmployee = new EmployeeCreateDto
        {
            FirstName = "Sylvester",
            LastName = "Holt",
            BirthDate = new DateTime(1995, 8, 7),
            CompanyId = 1,
            DepartmentId = 1,
            Addresses = new List<EmployeeAddressCreateDto>
            {
                new EmployeeAddressCreateDto
                {
                    AddressTypeId = AddressType.Work,
                    Address = "123 Work St"
                },
                new EmployeeAddressCreateDto
                {
                    AddressTypeId = AddressType.Residential,
                    Address = "456 Home Ave"
                }
            },
            Password = "password",
            Username = "password"
        };

        // Act
        var employee = await _httpClientHelper
            .PostAsync<EmployeeCreateDto, EmployeeDto>(_baseUrl + "create", newEmployee);

        // Assert
        Assert.Equal(newEmployee.FirstName, employee.FirstName);
        Assert.Equal(newEmployee.LastName, employee.LastName);
        Assert.Equal(newEmployee.BirthDate, employee.BirthDate);
        Assert.Equal(newEmployee.Addresses.Count, employee.Addresses.Count);

        // clean up
        await _httpClientHelper.DeleteAsync(_baseUrl + $"{employee.EmployeeId}");
    }

    [Fact]
    public async Task CanGetAllEmployees()
    {
        var employees = await _httpClientHelper.GetAsync<List<EmployeeDto>>(_baseUrl + "getall");
        Assert.Contains(employees, p => p.FirstName == "Julia");
    }

    [Fact]
    public async Task CanGetEmployee()
    {
        // The endpoint or route of the controller action.
        var employee = await _httpClientHelper.GetAsync<EmployeeDto>(_baseUrl + "3");
        Assert.Equal(3, employee.EmployeeId);
        Assert.Equal("Julia", employee.FirstName);
    }

    [Fact]
    public async Task CanUpdateEmployee()
    {
        // Arrange a new employee with two addresses
        var newEmployee = new EmployeeCreateDto
        {
            CompanyId = 1,
            DepartmentId = 1,
            Username = "JohnDoeUsername",
            Password = "JohnDoepassword",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1995, 8, 7),
            Addresses = new List<EmployeeAddressCreateDto>
            {
                new EmployeeAddressCreateDto
                {
                    AddressTypeId = AddressType.Residential,
                    Address = "123 Main St"
                },
                new EmployeeAddressCreateDto
                {
                    AddressTypeId = AddressType.Work,
                    Address = "123 Work St"
                }
            }
        };
        var newFullEmployee = await _httpClientHelper
            .PostAsync<EmployeeCreateDto, EmployeeDto>(_baseUrl + "create", newEmployee);

        // Arrange an update to the employee, where one address is updated
        var updateEmployee = new EmployeeUpdateDto
        {
            EmployeeId = newFullEmployee.EmployeeId,
            FirstName = "John updated",
            LastName = "Doe updated",
            BirthDate = new DateTime(1994, 8, 7),
            Addresses = new List<EmployeeAddressUpdateDto>
            {
                new EmployeeAddressUpdateDto 
                {
                    EmployeeId = newFullEmployee.EmployeeId,
                    AddressTypeId = AddressType.Residential,
                    Address = "789 New Home St" 
                },
                new EmployeeAddressUpdateDto
                {
                    EmployeeId = newFullEmployee.EmployeeId,
                    AddressTypeId = AddressType.Unknown,
                    Address = "444 Unknown St"
                }
            }
        };

        // Act
        var result = await _httpClientHelper
            .PutAsync<EmployeeUpdateDto, EmployeeDto>(_baseUrl + "update", updateEmployee);

        // Assert
        Assert.Equal("John updated", result.FirstName);
        Assert.Equal("Doe updated", result.LastName);
        Assert.Equal(new DateTime(1994, 8, 7), result.BirthDate);
        Assert.Equal(2, result.Addresses.Count);
        Assert.Contains(result.Addresses,
            a => a.Address == "789 New Home St" &&
            a.AddressTypeId == AddressType.Residential);
        Assert.Contains(result.Addresses,
            a => a.Address == "444 Unknown St" &&
            a.AddressTypeId == AddressType.Unknown);

        // clean up test employee
        var removeEmployee = await _httpClientHelper
            .DeleteAsync(_baseUrl + result.EmployeeId);
    }


    [Fact]
    public async Task CanUpsertEmployee()
    {
        // Arrange a new employee with two addresses
        var newEmployee = new EmployeeCreateDto
        {
            CompanyId = 1,
            DepartmentId = 1,
            Username = "JohnDoeUsername",
            Password = "JohnDoepassword",
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1995, 8, 7),
            Addresses = new List<EmployeeAddressCreateDto>
            {
                new EmployeeAddressCreateDto
                {
                    AddressTypeId = AddressType.Work,
                    Address = "123 Work St"
                },
                new EmployeeAddressCreateDto
                {
                    AddressTypeId = AddressType.Residential,
                    Address = "123 Main St"
                }
            }
        };
        var newFullEmployee = await _httpClientHelper
            .PostAsync<EmployeeCreateDto, EmployeeDto>(_baseUrl + "create", newEmployee);

        // Arrange an update to the employee, where one address is updated, and one is new
        var updateEmployee = new EmployeeUpdateDto
        {
            EmployeeId = newFullEmployee.EmployeeId,
            FirstName = "John updated",
            LastName = "Doe updated",
            BirthDate = new DateTime(1994, 8, 7),
            Addresses = new List<EmployeeAddressUpdateDto>
            {
                new EmployeeAddressUpdateDto
                {
                    EmployeeId = newFullEmployee.EmployeeId,
                    AddressTypeId = AddressType.Unknown,
                    Address = "444 Unknown St"
                },
                new EmployeeAddressUpdateDto
                {
                    EmployeeId = newFullEmployee.EmployeeId,
                    AddressTypeId = AddressType.Residential,
                    Address = "789 New Home St"
                }
            }
        };

        // Act
        var result = await _httpClientHelper
            .PutAsync<EmployeeUpdateDto, EmployeeDto>(_baseUrl + "upsert", updateEmployee);

        // Assert
        Assert.Equal("John updated", result.FirstName);
        Assert.Equal("Doe updated", result.LastName);
        Assert.Equal(new DateTime(1994, 8, 7), result.BirthDate);
        Assert.Equal(3, result.Addresses.Count);
        Assert.Contains(result.Addresses,
            a => a.Address == "444 Unknown St" &&
            a.AddressTypeId == AddressType.Unknown);
        Assert.Contains(result.Addresses,
             a => a.Address == "123 Work St" &&
            a.AddressTypeId == AddressType.Work);
        Assert.Contains(result.Addresses,
            a => a.Address == "789 New Home St" &&
            a.AddressTypeId == AddressType.Residential);

        // clean up test employee
        var removeEmployee = await _httpClientHelper
            .DeleteAsync(_baseUrl + result.EmployeeId);

    }
}
