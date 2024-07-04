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
        var employee = await _httpClientHelper.PostAsync<EmployeeCreateDto, EmployeeDto>(_baseUrl + "create", newEmployee);

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
        // Get first employee
        /* Create Employee */
        var newEmployee = new EmployeeUpdateDto()
        {
            EmployeeId = 1,
            FirstName = "Johnny",
            LastName = "Holt",
            BirthDate = new DateTime(1995, 8, 7)
        };

        // Update employee
        var updatedEmployee = await _httpClientHelper.PutAsync<EmployeeUpdateDto, EmployeeDto>(_baseUrl + "update", newEmployee);

        // First name should be a new one
        Assert.Equal("Johnny", updatedEmployee.FirstName);
    }
}
