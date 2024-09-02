using CompanyWebApi.Contracts.Dto.V4;
using CompanyWebApi.Contracts.Entities;
using CompanyWebApi.Tests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CompanyWebApi.Tests.IntegrationTests.Controllers.V4;

public class UsersControllerTests : ControllerTestsBase
{
    private const string API_VERSION = "V4";
    private readonly string _usersBaseUrl;
    private readonly string _employeesBaseUrl;
    private readonly HttpClientHelper _httpClientHelper;

    public UsersControllerTests(WebApiTestFactory factory)
        : base(factory)
    {
        _usersBaseUrl = $"/api/{API_VERSION.ToLower()}/users/";
        _employeesBaseUrl = $"/api/{API_VERSION.ToLower()}/employees/";
        _httpClientHelper = new HttpClientHelper(Client);
        _httpClientHelper.Client.SetFakeBearerToken((object)Token);
    }

    [Fact]
    public async Task CanCreateAndDeleteUsers()
    {
        var newEmployee = new EmployeeCreateDto()
        {
            CompanyId = 1,
            DepartmentId = 1,
            FirstName = "TestFirstName",
            LastName = "TestLastName",
            BirthDate = new DateTime(1991, 8, 7),
            Addresses = new List<EmployeeAddressCreateWithoutEmployeeIdDto>
            {
                new EmployeeAddressCreateWithoutEmployeeIdDto
                {
                    AddressTypeId = AddressType.Work,
                    Address = "123 Work St"
                },
                new EmployeeAddressCreateWithoutEmployeeIdDto
                {
                    AddressTypeId = AddressType.Residential,
                    Address = "456 Home Ave"
                }
            }
        };
        // Save test employee
        var employee = await _httpClientHelper.PostAsync<EmployeeCreateDto, EmployeeDto>(_employeesBaseUrl, newEmployee);

        // Create test user
        var newUser = new UserCreateDto()
        {
            EmployeeId = employee.EmployeeId,
            Username = "testuser",
            Password = "password"
        };

        var user = await _httpClientHelper.PostAsync<UserCreateDto, UserDto>(_usersBaseUrl, newUser);
        Assert.Equal("testuser", user.Username);

        // Delete test user
        await _httpClientHelper.DeleteAsync(_usersBaseUrl + user.EmployeeId);

        // Delete test employee
        await _httpClientHelper.DeleteAsync(_employeesBaseUrl + employee.EmployeeId);

    }

    [Fact]
    public async Task CanGetAllUsers()
    {
        _httpClientHelper.Client.SetFakeBearerToken((object)Token);
        var users = await _httpClientHelper.GetAsync<List<UserDto>>(_usersBaseUrl);
        Assert.Contains(users, p => p.Username == "johnw");
    }

    [Fact]
    public async Task CanGetUser()
    {
        _httpClientHelper.Client.SetFakeBearerToken((object)Token);
        var users = await _httpClientHelper.GetAsync<List<UserDto>>(_usersBaseUrl + "johnw");
        Assert.True(users.Any());
    }

    [Fact]
    public async Task CanUpdateUser()
    {
        // Get user and change Password
        var newUser = new User
        {
            EmployeeId = 2,
            Username = "mathiasg",
            Password = "password",
            Token = string.Empty
        };

        var updatedUser = await _httpClientHelper.PutAsync(_usersBaseUrl, newUser);
        Assert.Equal("password", updatedUser.Password);
    }

    [Fact]
    public async Task CanUserAuthenticate()
    {
        var user = new AuthenticateModel
        {
            Username = "johnw",
            Password = "test"
        };

        var authenticatedUser = await _httpClientHelper.PostAsync<AuthenticateModel, UserAuthenticateDto>(_usersBaseUrl + "authenticate", user);

        Assert.NotNull(authenticatedUser);
    }

}
